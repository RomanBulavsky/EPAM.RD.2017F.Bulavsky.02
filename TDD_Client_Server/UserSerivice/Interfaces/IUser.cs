using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserSerivice.Interfaces
{
    public interface IUser
    {
         int? Id { get; set; }

         string FirstName { get; set; }

         string LastName { get; set; }

         DateTime DateOfBirth { get; set; }
    }
}
