using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserSerivice.Interfaces;

namespace UserSerivice.Implimentation
{
    //we need to work with that class byref
    public class AddRemoveNotifier : MarshalByRefObject,IAddRemoveNotifier
    {
        public AddRemoveNotifier()
        {
        }

        public event EventHandler AddEvent;
        public event EventHandler RemoveEvent;
        public void AddNotification(object o, EventArgs args)
        {
            Console.WriteLine("Add event");
            AddEvent?.Invoke(o,args);
        }

        public void RemoveNotification(object o, EventArgs args)
        {
            Console.WriteLine("RemoveEvent");
            RemoveEvent?.Invoke(o, args);
        }
    }
}
