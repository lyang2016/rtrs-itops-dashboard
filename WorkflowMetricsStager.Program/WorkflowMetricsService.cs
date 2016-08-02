using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;
using System.Timers;
using Emma.Config;
using log4net.Repository.Hierarchy;
using RTRSCommon;
using RTRSCommon.Exceptions;
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
        //private DequeueRunner _dequeueRunner;
        private int _timerInterval;
        private int _fatalStopCountLimit;
        private int _errorWaitInterval;
        private int _maxNumberOfBatches;
        private FatalErrorCounter _fatalErrorCounter;
        private const string ComponentName = "WorkflowMetricsStager";
        private bool _serviceErrorFlag;

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
                //if (_dequeueRunner != null)
                //{
                //    _dequeueRunner.Dispose();
                //    _dequeueRunner = null;
                //}
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
                _stopWaitTimeout = int.Parse(_configuration.Properties["rtrs_op_dashboard_stop_wait_interval_secs", true]) * 1000;
                _timerInterval = int.Parse(_configuration.Properties["rtrs_op_dashboard_sleep_interval_millisecs", true]);
                _fatalStopCountLimit = int.Parse(_configuration.Properties["rtrs_op_dashboard_fatal_error_stop_count", true]);
                _errorWaitInterval = int.Parse(_configuration.Properties["rtrs_op_dashboard_error_wait_interval_secs", true]) * 1000;
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
                    _timer.Interval = _timerInterval;
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

                    // StagerRunner code here ...

                    Loggers.ApplicationTrace.DebugFormat("OnTimedEvent: start: {0}, duration: {1} ms", DateTime.Now, stopwatch.ElapsedMilliseconds);
                    stopwatch.Stop();

                    /*
                    var batchCount = 0;
                    var totalMsgCount = 0;
                    if (_dequeueRunner == null)
                    {
                        _dequeueRunner = new DequeueRunner();
                    }
                    while (true)
                    {
                        var msgCount = _dequeueRunner.Run();
                        batchCount++;
                        totalMsgCount += msgCount;

                        if (msgCount == 0 || _stopping)
                        {
                            break;
                        }
                    }
                    stopwatch.Stop();
                    if (totalMsgCount > 0)
                    {
                        Loggers.ApplicationTrace.DebugFormat("OnTimedEvent: processed messages: {0}, batch count: {1}, duration: {2} ms",
                            totalMsgCount, batchCount, stopwatch.ElapsedMilliseconds);
                    }
                     */
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
            //_dequeueRunner.DisposeDbConnetion();
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

    }
}
