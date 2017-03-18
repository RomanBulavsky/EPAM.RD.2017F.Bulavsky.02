using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserSerivice.Implimentation;
using System.Diagnostics;

namespace Server
{
    class Program
    {
        private static BooleanSwitch boolSwitch = new BooleanSwitch("mySwitch",
            "Switch in config file");

        static void Main(string[] args)
        {
            Console.WriteLine("Boolean switch {0} configured as {1}",
                boolSwitch.DisplayName, boolSwitch.Enabled.ToString());
            

                var v = new UserService(boolSwitch.Enabled);
         
        }
    }
}