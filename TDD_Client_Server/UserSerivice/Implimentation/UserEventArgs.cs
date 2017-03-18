using System;

namespace UserSerivice.Implimentation
{
    internal class UserEventArgs : EventArgs
    {
        public UserEventArgs(User user)
        {
            User = user;
        }

        public User User { get; set; }
    }
}