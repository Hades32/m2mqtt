using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;

namespace uPLibrary.Networking.M2Mqtt
{
    public class Dns
    {
        public static IPHostEntry GetHostEntry(string hostName)
        {
            return ResolveDNS(hostName).Result;
        }

        public static async Task<IPHostEntry> ResolveDNS(string remoteHostName)
        {
            if (string.IsNullOrEmpty(remoteHostName))
                return null;

            IPHostEntry result = new IPHostEntry();

            try
            {
                IReadOnlyList<EndpointPair> data =
                  await DatagramSocket.GetEndpointPairsAsync(new HostName(remoteHostName), "0");

                if (data != null && data.Count > 0)
                {
                    result.AddressList = new IPAddress[data.Count];
                    int i = 0;
                    foreach (EndpointPair item in data)
                    {
                        if (item != null && item.RemoteHostName != null &&
                                      item.RemoteHostName.Type == HostNameType.Ipv4)
                        {
                            result.AddressList[i] = IPAddress.Parse(item.RemoteHostName.CanonicalName);
                        }
                        i++;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("error resolving dns name {0}. ex:\n{1}", remoteHostName, ex);
                return null;
            }

            return result;
        } 
    }
}
