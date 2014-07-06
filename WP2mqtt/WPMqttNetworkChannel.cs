using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace uPLibrary.Networking.M2Mqtt
{
    public class WPMqttNetworkChannel : IMqttNetworkChannel
    {
        string remoteHostName; 
        IPAddress remoteIpAddress; 
        int remotePort; 
        bool secure; 
        X509Certificate caCert;
        StreamSocket socket;

        public WPMqttNetworkChannel(string remoteHostName, IPAddress remoteIpAddress, int remotePort, bool secure = false, X509Certificate caCert = null)
        {
            this.remoteHostName = remoteHostName;
            this.remoteIpAddress = remoteIpAddress;
            this.remotePort = remotePort;
            this.secure = secure;
            this.caCert = caCert;

            socket = new StreamSocket();
            socket.Control.NoDelay = true;
            socket.Control.KeepAlive = true;
        }

        public void Connect()
        {
            if (!ConnectAsync().Wait(60 * 1000))
                throw new TimeoutException("Connection to broker took longer than expected");
        }

        private async Task ConnectAsync()
        {
            // TODO maybe don't allow NULL cipher
            SocketProtectionLevel protectionLevel = secure ? SocketProtectionLevel.Ssl : SocketProtectionLevel.PlainSocket;
            await socket.ConnectAsync(new HostName(this.remoteHostName), this.remotePort.ToString(), protectionLevel);
        }

        public bool DataAvailable
        {
            // check if this is ok
            get { return true; }
        }

        public int Receive(byte[] buffer)
        {
            return ReceiveAsync(buffer).Result;
        }

        private async Task<int> ReceiveAsync(byte[] outbuffer)
        {
            IBuffer buffer = new Windows.Storage.Streams.Buffer((uint)outbuffer.Length);
            buffer = await socket.InputStream.ReadAsync(buffer, buffer.Capacity, Windows.Storage.Streams.InputStreamOptions.None);
            if (buffer.Length < outbuffer.Length)
            {
                var outbuffer2 = new byte[buffer.Length];
                DataReader.FromBuffer(buffer).ReadBytes(outbuffer2);
                Array.Copy(outbuffer2, outbuffer, Math.Min(outbuffer.Length, outbuffer2.Length));
            }
            else
            {
                DataReader.FromBuffer(buffer).ReadBytes(outbuffer);
            }
            return (int)buffer.Length;
        }

        public int Send(byte[] buffer)
        {
            return SendAsync(buffer).Result;
        }

        private async Task<int> SendAsync(byte[] inbuffer)
        {
            using (var writer = new Windows.Storage.Streams.DataWriter(socket.OutputStream))
            {
                writer.WriteBytes(inbuffer);
                await writer.StoreAsync();
                writer.DetachStream();
            }
            // hopefully true...
            return inbuffer.Length;
        }

        public void Close()
        {
            if (socket == null)
                return;

            socket.Dispose();
            socket = null;
        }
    }
}
