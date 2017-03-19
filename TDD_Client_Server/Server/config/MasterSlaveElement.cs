using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.config
{
    public class MasterSlaveElement : ConfigurationElement
    {

        [ConfigurationProperty("instanceType", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string MasterSlaveType
        {
            get { return ((string)(base["instanceType"])); }
            set { base["instanceType"] = value; }
        }

        [ConfigurationProperty("instanceCount", DefaultValue = "", IsKey = false, IsRequired = false)]
        public string MasterSlaveCount
        {
            get { return ((string)(base["instanceCount"])); }
            set { base["instanceCount"] = value; }
        }
    }

}
