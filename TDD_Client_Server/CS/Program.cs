using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace CS
{
    class Program
    {
        static void Main(string[] args)
        {
            Servicer sss = new Servicer();
            sss.Add(new Person("vasya", 12));
            sss.Add(new Person("vasya", 13));
            sss.Add(new Person("vasya", 14));

            string ms = Console.ReadLine();

            if (ms.ToUpperInvariant() == "M")
            {
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
                        Servicer s = new Servicer();
                        s.Add(new Person("vasya", 12));
                        s.Add(new Person("vasya", 13));
                        s.Add(new Person("vasya", 14));

                       

                        var zz = new List<Person>();
                        zz.Add(new Person("asdad",12));
                        var list = new HashSet<int>();
                        list.Add(1);
                        list.Add(12);
                        list.Add(13);
                        var zx = new List<Person>();
                        zx.AddRange(s.NameStorage);
                        new BinaryFormatter().Serialize(stream,s.NameStorage);

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
            else
            {
                TcpClient client = null;
                try
                {
                    client = new TcpClient("127.0.0.1", 8888);
                    NetworkStream stream = client.GetStream();

                    object obj = new BinaryFormatter().Deserialize(stream);

                    var objects = obj as List<Person>;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    client.Close();
                }
            }
        }
    }
}
