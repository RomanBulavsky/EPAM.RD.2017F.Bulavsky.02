using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserSerivice.Interfaces
{
    public interface IIdGenerator
    {
        Func<object, int> IdCreator { get; }
    }
}
