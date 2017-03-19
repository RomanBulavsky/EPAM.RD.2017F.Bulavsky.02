using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.config
{
    [ConfigurationCollection(typeof(MasterSlaveElement))]
    public class MasterSlaveCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new MasterSlaveElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((MasterSlaveElement)(element)).MasterSlaveType;
        }

        public MasterSlaveElement this[int idx]
        {
            get { return (MasterSlaveElement)BaseGet(idx); }
        }
    }
}
