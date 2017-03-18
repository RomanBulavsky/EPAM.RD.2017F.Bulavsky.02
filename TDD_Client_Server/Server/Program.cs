using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserSerivice.Implimentation;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Remoting;

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
            var v2 = new UserService(boolSwitch.Enabled);
            var v3 = new UserService(boolSwitch.Enabled);

            v.notifier.AddEvent += v2.AddToStorage;
            v.notifier.RemoveEvent += v2.RemoveFromStorage;
            v.notifier.AddEvent += v3.AddToStorage;

            v.Add(new User() {DateOfBirth = DateTime.Now, FirstName = "fn", LastName = "asda"});
            v.Add(new User() {DateOfBirth = DateTime.Now, FirstName = "fn2", LastName = "asda2"});
            v.Add(new User() {DateOfBirth = DateTime.Now, FirstName = "fn3", LastName = "asd3a"});
            v.Add(new User() {DateOfBirth = DateTime.Now, FirstName = "fn32", LastName = "asd3a"});
            v.Add(new User() {DateOfBirth = DateTime.Now, FirstName = "fn31", LastName = "asd3a4"});

            var userdel = v.Storage.FirstOrDefault(u => u.FirstName == "fn31");
            v.Delete(userdel);

            Console.WriteLine("\nMain\n");
            v.Show().ToList().ForEach(Console.Write);
            Console.WriteLine("\nSecound\n");
            v2.Show().ToList().ForEach(Console.Write);
            Console.WriteLine("\nTheard\n");
            v3.Show().ToList().ForEach(Console.Write);


            AppDomain mainAppDomain = AppDomain.CreateDomain("Main Domain");
            AppDomain secondDomain = AppDomain.CreateDomain("Secound domain");
            AppDomain thirdDomain = AppDomain.CreateDomain("Theard domain");
            //Console.WriteLine(Assembly.GetAssembly(typeof(UserService)).GetName().Name);
            //Console.WriteLine(Assembly.GetExecutingAssembly().GetName().Name);
            // secoundDomain.CreateInstance(Assembly.GetAssembly(typeof(UserService)).GetName().Name, typeof(UserService).FullName);


            ObjectHandle secondDomainInstanceHandle =
                secondDomain.CreateInstance(Assembly.GetAssembly(typeof(UserService)).GetName().Name,
                    typeof(UserService).FullName);
            ObjectHandle thirdDomainInstanceHandle =
                thirdDomain.CreateInstance(Assembly.GetAssembly(typeof(UserService)).GetName().Name,
                    typeof(UserService).FullName);
            ObjectHandle mainDomainInstanceHandle =
                mainAppDomain.CreateInstance(Assembly.GetAssembly(typeof(UserService)).GetName().Name,
                    typeof(UserService).FullName);


            var mainInstance = mainDomainInstanceHandle.Unwrap() as UserService;
            var secoundInstance = secondDomainInstanceHandle.Unwrap() as UserService;
            var theardInstance = thirdDomainInstanceHandle.Unwrap() as UserService;


            mainInstance.notifier.AddEvent += secoundInstance.UpdateStorage;
            mainInstance.notifier.RemoveEvent += secoundInstance.UpdateStorage;
            //mainInstance.AddRemoveEvent += theardInstance.UpdateSlaveCollection;

            //mainInstance.IsMaster = true;
            mainInstance.Add(new User() {LastName = "ln", FirstName = "fn"});
            // = new HashSet<string>() { "asd1", "asd2", "asd3", "asd4" };
            mainInstance.Add(new User() {LastName = "ln2", FirstName = "fn2"});
            // = new HashSet<string>() { "asd1", "asd2", "asd3", "asd4" };

            //secoundInstance.UpdateStorage(mainInstance, EventArgs.Empty); // = mainInstance.Strings;
            //theardInstance.UpdateStorage(mainInstance, EventArgs.Empty); //.Strings = mainInstance.Strings;

            mainInstance.Show().ToList().ForEach(Console.Write);
            Console.WriteLine("\nSecound\n");
            secoundInstance.Show().ToList().ForEach(Console.Write);
            Console.WriteLine("\nTheard\n");
            theardInstance.Show().ToList().ForEach(Console.Write);


            try
            {
                Console.WriteLine("\nAdding");
                mainInstance.Add(new User() {LastName = "ln23", FirstName = "fn23"});
                // = new HashSet<string>() { "asd1", "asd2", "asd3", "asd4" };

                //mainInstance.OnAddRemoveEventHandler();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine("\nMain\n");
            mainInstance.Show().ToList().ForEach(Console.Write);
            Console.WriteLine("\nSecound\n");
            secoundInstance.Show().ToList().ForEach(Console.Write);
            Console.WriteLine("\nTheard\n");
            theardInstance.Show().ToList().ForEach(Console.Write);

            Console.WriteLine("\n");


            //mainInstance.SnapShot();
            Console.WriteLine(mainInstance);

            //
            Console.WriteLine("asdasdasdasd------");
            Console.WriteLine("bad version");
            secoundInstance.Show().ToList().ForEach(Console.Write);
            var secoundInstance2 = secondDomainInstanceHandle.Unwrap() as UserService;
            Console.WriteLine("real");
            secoundInstance2.Show().ToList().ForEach(Console.Write);
        }
    }
}