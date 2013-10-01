using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Manager;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var demoSink = new DemoSink();

            var manager = new VoodooManager<DemoCat>(new DemoCat("James"), demoSink);

            demoSink.Manager = manager;

            manager.DefinePath<DemoCat>("GetCatsName", cat => Console.WriteLine(cat.Name));

            manager.StartPath("GetCatsName", "BensPC", "BensPC");

            Console.ReadLine();
        }
    }
}
