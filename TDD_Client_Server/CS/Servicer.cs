using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS
{
    [Serializable]
    class Servicer
    {
        public bool IsMaster = false;

        public List<Person> NameStorage = new List<Person>();

        //public Servicer(bool isMaster, List<Person> nameStorage)
        //{
        //    IsMaster = isMaster;
        //    NameStorage = nameStorage;
        //}

        public void Add(Person name)
        {
            if(IsMaster)
            NameStorage.Add(name);
        }
        public void Remove(Person name)
        {
            if (IsMaster)
                NameStorage.Remove(name);
        }

        public Person SearchByLetters(string letters)
        {
            return NameStorage.FirstOrDefault(o => o.Name.Contains(letters));
        }
    }
}
