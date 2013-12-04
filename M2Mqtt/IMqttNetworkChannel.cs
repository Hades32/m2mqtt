using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uPLibrary.Networking.M2Mqtt
{
    public interface IMqttNetworkChannel
    {
        bool DataAvailable { get; }

        int Receive(byte[] fixedHeaderFirstByte);

        int Send(byte[] msgBytes);

        void Close();

        void Connect();
    }
}
