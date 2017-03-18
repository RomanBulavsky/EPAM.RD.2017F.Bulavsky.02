using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserSerivice.Interfaces;

namespace UserSerivice.Implimentation
{
    class UserIdGenerator : IIdGenerator
    {
        public UserIdGenerator(Func<object, int> idCreator)
        {
            IdCreator = idCreator;
        }

        public Func<object, int> IdCreator { get; }
    }
}
