using System;

namespace UserSerivice.Exceptions
{
    public class NotExistingUserException : Exception
    {
        public NotExistingUserException()
        {
        }

        public NotExistingUserException(string message) : base(message)
        {
        }

        public NotExistingUserException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
