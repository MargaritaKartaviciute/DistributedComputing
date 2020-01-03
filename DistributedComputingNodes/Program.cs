using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apache.Ignite.Core;
using Apache.Ignite.Core.Cluster;
using Apache.Ignite.Core.Discovery.Tcp;
using Apache.Ignite.Core.Discovery.Tcp.Static;
using Apache.Ignite.Core.Events;

namespace DistributedComputing
{
    class Program
    {
        static void Main(string[] args)
        {
            Ignition.StartFromApplicationConfiguration();
            Console.ReadKey(); // keep the node running
        }

        
    }
}
