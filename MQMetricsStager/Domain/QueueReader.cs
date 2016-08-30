using System;
using System.Diagnostics.CodeAnalysis;
using RTRSCommon;

namespace MQMetricsStager.Domain
{
    public interface IQueueReader
    {
        int GetCurrentDepth();
        void Reset();
    }

    [ExcludeFromCodeCoverage]
    public class QueueReader : IDisposable, IQueueReader
    {
        private MqConnection _mqConnection;
        private readonly MqSettings _mqSettings;

        public QueueReader(MqSettings mqSettings)
        {
            _mqSettings = mqSettings;
        }

        public int GetCurrentDepth()
        {
            if (_mqConnection == null)
            {
                _mqConnection = new MqConnection(_mqSettings);
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
