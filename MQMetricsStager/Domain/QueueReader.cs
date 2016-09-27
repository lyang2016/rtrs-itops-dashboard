using System;
<<<<<<< HEAD
using System.Diagnostics.CodeAnalysis;
=======
>>>>>>> e0e1ff74d1444cc92b0949760451e6c7a2b14f0a
using RTRSCommon;

namespace MQMetricsStager.Domain
{
<<<<<<< HEAD
    public interface IQueueReader
    {
        int GetCurrentDepth();
        void Reset();
    }

    [ExcludeFromCodeCoverage]
    public class QueueReader : IDisposable, IQueueReader
    {
        private MqPeekConnection _mqConnection;
        private readonly MqSettings _mqSettings;
=======
    public class QueueReader : IDisposable
    {
        private readonly MqSettings _mqSettings;
        private MqConnection _mqConnection;
>>>>>>> e0e1ff74d1444cc92b0949760451e6c7a2b14f0a

        public QueueReader(MqSettings mqSettings)
        {
            _mqSettings = mqSettings;
        }

        public int GetCurrentDepth()
        {
            if (_mqConnection == null)
            {
<<<<<<< HEAD
                _mqConnection = new MqPeekConnection(_mqSettings);
=======
                _mqConnection = new MqConnection(_mqSettings);
>>>>>>> e0e1ff74d1444cc92b0949760451e6c7a2b14f0a
                _mqConnection.OpenQueue();
            }
            return _mqConnection.GetCurrentDepth();
        }

        public void Reset()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (_mqConnection != null)
            {
                _mqConnection.CloseQueue();
                _mqConnection.Dispose();
            }
            _mqConnection = null;
        }
    }
}
