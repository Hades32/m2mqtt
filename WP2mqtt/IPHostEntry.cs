using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace uPLibrary.Networking.M2Mqtt
{
    // from system.net....
    
    // Summary:
    //     Provides a container class for Internet host address information.
    public class IPHostEntry
    {
        // Summary:
        //     Initializes a new instance of the System.Net.IPHostEntry class.
        public IPHostEntry()
        {

        }

        // Summary:
        //     Gets or sets a list of IP addresses that are associated with a host.
        //
        // Returns:
        //     An array of type System.Net.IPAddress that contains IP addresses that resolve
        //     to the host names that are contained in the System.Net.IPHostEntry.Aliases
        //     property.
        public IPAddress[] AddressList { get; set; }
        //
        // Summary:
        //     Gets or sets a list of aliases that are associated with a host.
        //
        // Returns:
        //     An array of strings that contain DNS names that resolve to the IP addresses
        //     in the System.Net.IPHostEntry.AddressList property.
        public string[] Aliases { get; set; }
        //
        // Summary:
        //     Gets or sets the DNS name of the host.
        //
        // Returns:
        //     A string that contains the primary host name for the server.
        public string HostName { get; set; }
    }
}
