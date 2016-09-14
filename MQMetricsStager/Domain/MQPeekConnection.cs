using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using IBM.WMQ;
using RTRSCommon;

namespace MQMetricsStager.Domain
{
    [ExcludeFromCodeCoverage]
	public class MqPeekConnection : IDisposable
    {

        private readonly MqSettings _mqSettings;

        private MQQueue _mq;
        private MQQueueManager _mqManager;

        public MqPeekConnection(MqSettings mqSettings)
	    {
	        _mqSettings = mqSettings;
        }

        public int GetCurrentDepth()
        {
            return _mq.CurrentDepth;
        }

	    public void OpenQueue()
	    {
	        var connectionProperties = new Hashtable
	        {
	            {MQC.HOST_NAME_PROPERTY, _mqSettings.QHostName},
	            {MQC.PORT_PROPERTY, _mqSettings.QPort},
	            {MQC.CHANNEL_PROPERTY, _mqSettings.QChannel},
	            {MQC.TRANSPORT_PROPERTY, MQC.TRANSPORT_MQSERIES_XACLIENT}
	        };
	        _mqManager = new MQQueueManager(_mqSettings.QManager, connectionProperties);
	        var openOptions = MQC.MQOO_INQUIRE;
	        _mq = _mqManager.AccessQueue(_mqSettings.QName, openOptions);
	    }

	    public void CloseQueue()
	    {
	        if (_mq != null && _mq.IsOpen)
	        {
	            _mq.Close();
	            _mq = null;
	        }
	        if (_mqManager != null)
	        {
	            if (_mqManager.IsConnected)
	            {
	                _mqManager.Disconnect();
	            }
	            if (_mqManager is IDisposable)
	            {
	                (_mqManager as IDisposable).Dispose();
	            }
	            _mqManager = null;
	        }
	    }


	    public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

	    private void Dispose(bool disposing)
	    {
	        if (disposing)
	        {
	            CloseQueue();
	        }
	        _mq = null;
	        _mqManager = null;
	    }

	    ~MqPeekConnection()
        {
            Dispose(false);
        }
    }
}