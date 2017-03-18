using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using UserSerivice.Interfaces;

namespace UserSerivice.Implimentation
{
    [Serializable]
    public class User : IUser, IXmlSerializable
    {
        public int? Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; } = DateTime.Today;

        // Override GetHC and Eq

        public override string ToString()
        {
            return $" ID:{Id} : FN:{FirstName}-{LastName} ";
        }

        public XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }

        public void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        public void WriteXml(XmlWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}