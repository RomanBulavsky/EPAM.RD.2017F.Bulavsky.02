using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserSerivice.Implimentation;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Remoting;
using Server.config;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace Server
{
    class Program
    {
        private static BooleanSwitch boolSwitch = new BooleanSwitch("mySwitch",
            "Switch in config file");

        static void Main(string[] args)
        {
            string ms = Console.ReadLine();

            if (ms.ToUpperInvariant() == "M")
            {
                var s = new UserService(true,true,new HashSet<User>());
                s.notifier.AddEvent += s.TcpAddToStorage;
                var user = new User() {DateOfBirth = DateTime.Today,FirstName = "FN1",LastName = "Ln1"};
                s.Add(user);
            }
            else
            {
                Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} - MAIN current thread");
                var hs = new HashSet<User>();
                var user = new User() { DateOfBirth = DateTime.Today, FirstName = "FN1", LastName = "Ln1" };
                var user2 = new User() { DateOfBirth = DateTime.Today, FirstName = "FN2", LastName = "Ln2" };
                var user3 = new User() { DateOfBirth = DateTime.Today, FirstName = "FN3", LastName = "Ln3" };
                hs.Add(user);
                hs.Add(user2);
                hs.Add(user3);
                var z = new UserService(false, false, hs);
                
                Task.Run(() => SlaveServer(z));
                Task.Run(() => SlaveServerForClient(z));
                //new Task(SlaveServer).Start();
            }

            Console.ReadKey();

        }
        public static void SlaveServer(UserService z)
        {
            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} - current thread");
            TcpListener server = null;
            try
            {
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");
                server = new TcpListener(localAddr, 8888);

                // запуск слушателя
                server.Start();

                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();

                    NetworkStream stream = client.GetStream();

                    z.Storage = (HashSet<User>) new BinaryFormatter().Deserialize(stream);

                    Console.WriteLine("Slave take it");
                    client.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (server != null)
                    server.Stop();
            }
        }

        public static void SlaveServerForClient(UserService z)
        {
            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} - current thread");
            TcpListener server = null;
            try
            {
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");
                server = new TcpListener(localAddr, 7777);

                // запуск слушателя
                server.Start();
                byte[] data = new byte[64];
                while (true)
                {
                    Console.WriteLine("Ожидание подключений... ");

                    // получаем входящее подключение
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Подключен клиент. Выполнение запроса...");

                    // получаем сетевой поток для чтения и записи
                    NetworkStream stream = client.GetStream();

                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    string message = builder.ToString();

                    Console.WriteLine(message);

                    //// сообщение для отправки клиенту
                    var users = z.SearchByFirstName(message);
                    builder.Clear();

                    foreach (var user in users)
                    {
                        builder.Append(user.ToString());
                    }

                    string response = builder.ToString();
                    //// преобразуем сообщение в массив байтов
                    data = Encoding.Unicode.GetBytes(response);

                    //// отправка сообщения
                    stream.Write(data, 0, data.Length);
                    //Console.WriteLine("Отправлено сообщение: {0}", response);
                    //// закрываем подключение
                    
                    client.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (server != null)
                    server.Stop();
            }
        }

        //static void Server(UserService us)
        //{
        //    TcpListener server = null;
        //    try
        //    {
        //        IPAddress localAddr = IPAddress.Parse("127.0.0.1");
        //        server = new TcpListener(localAddr, 8888);

        //        // запуск слушател
        //        server.Start();

        //        while (true)
        //        {
        //            TcpClient client = server.AcceptTcpClient();

        //            NetworkStream stream = client.GetStream();

        //            new BinaryFormatter().Serialize(stream, );

        //            client.Close();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //    }
        //    finally
        //    {
        //        if (server != null)
        //            server.Stop();
        //    }
        //}

        //static void Client(UserService us)
        //{
        //    TcpClient client = null;
        //    try
        //    {
        //        client = new TcpClient("127.0.0.1", 8888);
        //        NetworkStream stream = client.GetStream();

        //        object obj = new BinaryFormatter().Deserialize(stream);

        //        var objects = obj as List<Person>;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    finally
        //    {
        //        client.Close();
        //    }

        //}
    }
}

//           StartupFoldersConfigSection section = (StartupFoldersConfigSection)ConfigurationManager.GetSection("StartupFolders");

//if ( section != null )
//{
//System.Diagnostics.Debug.WriteLine( section.FolderItems[0].FolderType );
//System.Diagnostics.Debug.WriteLine( section.FolderItems[0].Path );
//}

//    StartupMasterSlaveConfigSection section2 = (StartupMasterSlaveConfigSection)ConfigurationManager.GetSection("StartupMasterSlave");

//    if (section2 != null)
//    {
//        Console.WriteLine(section2.MasterSlaveItems[0].MasterSlaveType);
//        Console.WriteLine(section2.MasterSlaveItems[0].MasterSlaveCount);
//    }


//    Console.WriteLine("Boolean switch {0} configured as {1}",
//        boolSwitch.DisplayName, boolSwitch.Enabled.ToString());


//    var v = new UserService(boolSwitch.Enabled);
//    var v2 = new UserService(boolSwitch.Enabled);
//    var v3 = new UserService(boolSwitch.Enabled);

//    v.notifier.AddEvent += v2.AddToStorage;
//    v.notifier.RemoveEvent += v2.RemoveFromStorage;
//    v.notifier.AddEvent += v3.AddToStorage;

//    v.Add(new User() {DateOfBirth = DateTime.Now, FirstName = "fn", LastName = "asda"});
//    v.Add(new User() {DateOfBirth = DateTime.Now, FirstName = "fn2", LastName = "asda2"});
//    v.Add(new User() {DateOfBirth = DateTime.Now, FirstName = "fn3", LastName = "asd3a"});
//    v.Add(new User() {DateOfBirth = DateTime.Now, FirstName = "fn32", LastName = "asd3a"});
//    v.Add(new User() {DateOfBirth = DateTime.Now, FirstName = "fn31", LastName = "asd3a4"});

//    var userdel = v.Storage.FirstOrDefault(u => u.FirstName == "fn31");
//    v.Delete(userdel);

//    Console.WriteLine("\nMain\n");
//    v.Show().ToList().ForEach(Console.Write);
//    Console.WriteLine("\nSecound\n");
//    v2.Show().ToList().ForEach(Console.Write);
//    Console.WriteLine("\nTheard\n");
//    v3.Show().ToList().ForEach(Console.Write);


//    AppDomain mainAppDomain = AppDomain.CreateDomain("Main Domain");
//    AppDomain secondDomain = AppDomain.CreateDomain("Secound domain");
//    AppDomain thirdDomain = AppDomain.CreateDomain("Theard domain");
//    //Console.WriteLine(Assembly.GetAssembly(typeof(UserService)).GetName().Name);
//    //Console.WriteLine(Assembly.GetExecutingAssembly().GetName().Name);
//    // secoundDomain.CreateInstance(Assembly.GetAssembly(typeof(UserService)).GetName().Name, typeof(UserService).FullName);


//    ObjectHandle secondDomainInstanceHandle =
//        secondDomain.CreateInstance(Assembly.GetAssembly(typeof(UserService)).GetName().Name,
//            typeof(UserService).FullName);
//    ObjectHandle thirdDomainInstanceHandle =
//        thirdDomain.CreateInstance(Assembly.GetAssembly(typeof(UserService)).GetName().Name,
//            typeof(UserService).FullName);
//    ObjectHandle mainDomainInstanceHandle =
//        mainAppDomain.CreateInstance(Assembly.GetAssembly(typeof(UserService)).GetName().Name,
//            typeof(UserService).FullName);


//    var mainInstance = mainDomainInstanceHandle.Unwrap() as UserService;
//    var secoundInstance = secondDomainInstanceHandle.Unwrap() as UserService;
//    var theardInstance = thirdDomainInstanceHandle.Unwrap() as UserService;


//    mainInstance.notifier.AddEvent += secoundInstance.UpdateStorage;
//    mainInstance.notifier.RemoveEvent += secoundInstance.UpdateStorage;
//    //mainInstance.AddRemoveEvent += theardInstance.UpdateSlaveCollection;

//    //mainInstance.IsMaster = true;
//    mainInstance.Add(new User() {LastName = "ln", FirstName = "fn"});
//    // = new HashSet<string>() { "asd1", "asd2", "asd3", "asd4" };
//    mainInstance.Add(new User() {LastName = "ln2", FirstName = "fn2"});
//    // = new HashSet<string>() { "asd1", "asd2", "asd3", "asd4" };

//    //secoundInstance.UpdateStorage(mainInstance, EventArgs.Empty); // = mainInstance.Strings;
//    //theardInstance.UpdateStorage(mainInstance, EventArgs.Empty); //.Strings = mainInstance.Strings;

//    mainInstance.Show().ToList().ForEach(Console.Write);
//    Console.WriteLine("\nSecound\n");
//    secoundInstance.Show().ToList().ForEach(Console.Write);
//    Console.WriteLine("\nTheard\n");
//    theardInstance.Show().ToList().ForEach(Console.Write);


//    try
//    {
//        Console.WriteLine("\nAdding");
//        mainInstance.Add(new User() {LastName = "ln23", FirstName = "fn23"});
//        // = new HashSet<string>() { "asd1", "asd2", "asd3", "asd4" };

//        //mainInstance.OnAddRemoveEventHandler();
//    }
//    catch (Exception e)
//    {
//        Console.WriteLine(e.Message);
//    }

//    Console.WriteLine("\nMain\n");
//    mainInstance.Show().ToList().ForEach(Console.Write);
//    Console.WriteLine("\nSecound\n");
//    secoundInstance.Show().ToList().ForEach(Console.Write);
//    Console.WriteLine("\nTheard\n");
//    theardInstance.Show().ToList().ForEach(Console.Write);

//    Console.WriteLine("\n");


//    //mainInstance.SnapShot();
//    Console.WriteLine(mainInstance);

//    //
//    Console.WriteLine("asdasdasdasd------");
//    Console.WriteLine("bad version");
//    secoundInstance.Show().ToList().ForEach(Console.Write);
//    var secoundInstance2 = secondDomainInstanceHandle.Unwrap() as UserService;
//    Console.WriteLine("real");
//    secoundInstance2.Show().ToList().ForEach(Console.Write);