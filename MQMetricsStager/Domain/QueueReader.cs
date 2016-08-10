using System;
using RTRSCommon;

namespace MQMetricsStager.Domain
{
    public class QueueReader : IDisposable
    {
        private readonly MqSettings _mqSettings;
        private MqConnection _mqConnection;

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
