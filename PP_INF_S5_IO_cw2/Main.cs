using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerEchoLibrary
{
    class ServerMain
    {
        static void Main(string[] args)
        {
            ServerEchoAPM sAPM = new ServerEchoAPM(System.Net.IPAddress.Parse("127.0.0.1"), 1234);
            sAPM.Start();
        }
    }
}
