using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace uPLibrary.Networking.M2Mqtt
{
    public class AsyncMqttClient : MqttClient
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="brokerIpAddress">Broker IP address</param>
        /// <param name="brokerPort">Broker port</param>
        /// <param name="secure">Using secure connection</param>
        /// <param name="caCert">CA certificate for secure connection</param>
        public AsyncMqttClient(IPAddress brokerIpAddress, int brokerPort = MQTT_BROKER_DEFAULT_PORT, bool secure = false, X509Certificate caCert = null)
            : base(brokerIpAddress, brokerPort, secure, caCert)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="brokerHostName">Broker Host Name</param>
        /// <param name="brokerPort">Broker port</param>
        /// <param name="secure">Using secure connection</param>
        /// <param name="caCert">CA certificate for secure connection</param>
        public AsyncMqttClient(string brokerHostName, int brokerPort = MQTT_BROKER_DEFAULT_PORT, bool secure = false, X509Certificate caCert = null)
            : base(brokerHostName, brokerPort, secure, caCert, skipIdAdressResolution: true)
        {
        }
        
        /// <summary>
        /// Connect to broker
        /// </summary>
        /// <param name="clientId">Client identifier</param>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <param name="willRetain">Will retain flag</param>
        /// <param name="willQosLevel">Will QOS level</param>
        /// <param name="willFlag">Will flag</param>
        /// <param name="willTopic">Will topic</param>
        /// <param name="willMessage">Will message</param>
        /// <param name="cleanSession">Clean sessione flag</param>
        /// <param name="keepAlivePeriod">Keep alive period</param>
        /// <returns>Return code of CONNACK message from broker</returns>
        public Task<bool> ConnectAsync(string clientId, 
            string username = null,
            string password = null,
            bool willRetain = false,
            byte willQosLevel = MqttMsgConnect.QOS_LEVEL_AT_LEAST_ONCE,
            bool willFlag = false,
            string willTopic = null,
            string willMessage = null,
            bool cleanSession = true,
            ushort keepAlivePeriod = MqttMsgConnect.KEEP_ALIVE_PERIOD_DEFAULT)
        {
            var task = new Task<bool>(() =>
            {
                var res = base.Connect(clientId, username, password, willRetain, willQosLevel, willFlag, willTopic, willMessage, cleanSession, keepAlivePeriod);
                return this.IsConnected;
            });
            task.Start();
            return task;
        }

        /// <summary>
        /// Publish a message to the broker
        /// </summary>
        /// <param name="topic">Message topic</param>
        /// <param name="message">Message data (payload)</param>
        /// <param name="qosLevel">QoS Level</param>
        /// <param name="retain">Retain flag</param>
        /// <returns>Message Id related to PUBLISH message</returns>
        public Task<ushort> PublishAsync(string topic, byte[] message,
            byte qosLevel = MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE,
            bool retain = false)
        {
            var task = new Task<ushort>(() =>
            {
                return base.Publish(topic, message, qosLevel, retain);
            });
            task.Start();
            return task;
        }

        public Task<byte> SubscribeAsync(string topic, byte qos)
        {
            var task = new Task<byte>(() =>
            {
                return base.Subscribe(topic, qos);
            });
            task.Start();
            return task;
        }

        /// <summary>
        /// Subscribe for message topics
        /// </summary>
        /// <param name="topics">List of topics to subscribe</param>
        /// <param name="qosLevels">QOS levels related to topics</param>
        /// <returns>Granted QoS Levels in SUBACK message from broker</returns>
        public Task<byte[]> SubscribeAsync(string[] topics, byte[] qosLevels)
        {
            var task = new Task<byte[]>(() =>
            {
                return base.Subscribe(topics, qosLevels);
            });
            task.Start();
            return task;
        }
    }
}
