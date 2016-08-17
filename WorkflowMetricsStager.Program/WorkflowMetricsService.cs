using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;
using System.Timers;
using Emma.Config;
using RTRSCommon;
using RTRSCommon.Exceptions;
using WorkflowMetricsStager.Domain;
using Timer = System.Timers.Timer;

namespace WorkflowMetricsStager.Program
{
    public partial class WorkflowMetricsService : ServiceBase
    {
        private static readonly Timer _timer = new Timer();
        private static readonly object _lock = new object();
        private readonly IConfiguration _configuration = Configuration.Instance;
        private ManualResetEvent _stopEvent = new ManualResetEvent(true);
        private int _stopWaitTimeout;
        private bool _stopping;
        private StagerRunner _stageRunner;
        private int _timerInterval;
        private int _fatalStopCountLimit;
        private int _errorWaitInterval;
        private FatalErrorCounter _fatalErrorCounter;
        private const string ComponentName = "WorkflowMetricsStager";
        private bool _serviceErrorFlag;
        private string _timerStartTime;
        private string _timerEndTime;

        public WorkflowMetricsService()
        {
            InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_stopEvent != null)
                {
                    _stopEvent.Dispose();
                    _stopEvent = null;
                }
            }
            base.Dispose(disposing);
        }

        protected override void OnStart(string[] args)
        {
            LoadConfiguration();
            _fatalErrorCounter = new FatalErrorCounter(_errorWaitInterval, _fatalStopCountLimit, 10);
            StartProcessing();
        }

        protected override void OnStop()
        {
            StopProcessing();
        }

        private void LoadConfiguration()
        {
            try
            {
                _stopWaitTimeout = int.Parse(_configuration.Properties["workflow_stager_stop_wait_interval_secs", true]) * 1000;
                _timerInterval = int.Parse(_configuration.Properties["workflow_stager_sleep_interval_millisecs", true]);
                _fatalStopCountLimit = int.Parse(_configuration.Properties["workflow_stager_fatal_error_stop_count", true]);
                _errorWaitInterval = int.Parse(_configuration.Properties["workflow_stager_error_wait_interval_secs", true]) * 1000;
                _timerStartTime = _configuration.Properties["workflow_stager_start_time", true];
                _timerEndTime = _configuration.Properties["workflow_stager_end_time", true];
            }
            catch (Exception ex)
            {
                Loggers.ApplicationTrace.Error(ex);
                throw;
            }

        }

        private void StartProcessing()
        {
            _stopping = false;
            Loggers.ApplicationTrace.Info("Processing started");

            try
            {
                if (!_timer.Enabled)
                {
                    _timer.Elapsed += OnTimedEvent;
                    //_timer.Interval = _timerInterval;
                    _timer.Interval = GetTimerIntervalInMilliSeconds();
                    _timer.Start();
                }
            }
            catch (Exception ex)
            {
                Loggers.ApplicationTrace.Error("Service start up exception", ex);
                ServiceEmailer.SendErrorEmail(ComponentName, "Service start up exception", ex);
                throw;
            }

            ServiceEmailer.SendEmail(ComponentName, "Service started normally");
        }

        private void StopProcessing()
        {
            _stopping = true;
            try
            {
                if (_timer.Enabled)
                {
                    _timer.Stop();
                }

                if (_stopWaitTimeout > 0)
                {
                    RequestAdditionalTime(_stopWaitTimeout);
                }
                _stopEvent.WaitOne(_stopWaitTimeout);
                Loggers.ApplicationTrace.Info("Processing stopped");
                ServiceEmailer.SendEmail(ComponentName,
                    _serviceErrorFlag ? string.Format("Service stopped with exception after {0} retries", _fatalErrorCounter.FatalErrorCount) : "service stopped normally");
            }
            catch (Exception ex)
            {
                Loggers.ApplicationTrace.Error("Service shutdown exception", ex);
                ServiceEmailer.SendErrorEmail(ComponentName, "Service exception while shutting down", ex);
            }
        }


        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            lock (_lock)
            {
                _timer.Stop();
                _stopEvent.Reset();
                try
                {
                    _fatalErrorCounter.Reevaluate();
                    var stopwatch = Stopwatch.StartNew();

                    if (_stageRunner == null)
                    {
                        _stageRunner = new StagerRunner();
                    }

                    if (!_stopping)
                    {
                        _stageRunner.Run();                        
                    }

                    Loggers.ApplicationTrace.DebugFormat("OnTimedEvent: start: {0}, duration: {1} ms", DateTime.Now, stopwatch.ElapsedMilliseconds);
                    stopwatch.Stop();

                }
                catch (Exception ex)
                {
                    HandleProcessingException(ex);
                }
                finally
                {
                    if (_stopEvent != null)
                    {
                        _stopEvent.Set();
                    }
                    if (!_stopping)
                    {
                        _timer.Start();
                        _timer.Interval = GetTimerIntervalInMilliSeconds();
                    }
                }
            }
        }

        private void HandleProcessingException(Exception exception)
        {
            Loggers.ApplicationTrace.Error("Service processing exception", exception);
            _fatalErrorCounter.TimeOfLastError = DateTime.Now;
            _fatalErrorCounter.Increment();
            Loggers.ApplicationTrace.Info(string.Format("fatal error count: {0}", _fatalErrorCounter.FatalErrorCount));
            if (_fatalErrorCounter.HasExceededFatalCount())
            {
                Loggers.ApplicationTrace.ErrorFormat("Service processing exception, shutting down service, fatal stop count: {0}, fatal stop count limit: {1}",
                    _fatalErrorCounter.FatalErrorCount, _fatalStopCountLimit);
                ServiceEmailer.SendErrorEmail(ComponentName, "Service processing exception", exception);
                ShutdownServiceAfterError();
                return;
            }
            try
            {
                Loggers.ApplicationTrace.WarnFormat("Unexpected error has occurred. Service will wait for {0} ms and then try again.", _errorWaitInterval);
                Thread.Sleep(_errorWaitInterval);
            }
            catch (Exception ex)
            {
                Loggers.ApplicationTrace.Error("Service wait Exception", ex);
            }
        }

        private void ShutdownServiceAfterError()
        {
            Loggers.ApplicationTrace.Info("Shutting down service.");
            _serviceErrorFlag = true;
            try
            {
                Stop();
            }
            catch (Exception ex)
            {
                Loggers.ApplicationTrace.Error("Couldn't shutdown service.", ex);
                throw;
            }
        }

        private int GetTimerIntervalInMilliSeconds()
        {
            var interval = _timerInterval;
            var midNight = DateTime.Today;
            var currentTime = DateTime.Now;
            var timerStartTime = midNight.Add(TimeSpan.Parse(_timerStartTime));
            var timerEndTime = midNight.Add(TimeSpan.Parse(_timerEndTime));
            var nextDayTimerStartTime = timerStartTime.AddDays(1);

            if (currentTime < timerStartTime)
            {
                interval = (int) timerStartTime.Subtract(currentTime).TotalMilliseconds;
            }
            else if (currentTime > timerEndTime && currentTime < nextDayTimerStartTime)
            {
                interval = (int) nextDayTimerStartTime.Subtract(currentTime).TotalMilliseconds;
            }

            return interval;
        }
    }
}
