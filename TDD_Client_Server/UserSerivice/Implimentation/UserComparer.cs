﻿using System.Collections.Generic;

namespace UserSerivice.Implimentation
{
    class UserComparer : IEqualityComparer<User>
    {
        public bool Equals(User x, User y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(User obj)
        {
            return obj?.GetHashCode() ?? -1;
        }
    }
}
