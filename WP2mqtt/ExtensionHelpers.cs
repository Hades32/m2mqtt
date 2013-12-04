using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace uPLibrary.Networking.M2Mqtt
{
    public static class ExtensionHelpers
    {
        public static bool WaitOne(this AutoResetEvent are, int millisecondsTimeout, bool exitContext)
        {
            // simply ignoring exitContext as not available on Windows Phone
            return are.WaitOne(millisecondsTimeout);
        }
    }
}
