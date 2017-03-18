using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserSerivice.Interfaces
{
    public interface IService<T> where T : IUser
    {
        IEnumerable<T> SearchByLastName(string lastName);
        IEnumerable<T> SearchByFirstName(string firstName);
        void Add(T user);
        void Delete(T user);
    }
}
