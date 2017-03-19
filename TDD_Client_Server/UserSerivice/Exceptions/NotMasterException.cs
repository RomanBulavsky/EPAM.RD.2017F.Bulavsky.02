using System;

namespace UserSerivice.Exceptions
{
    public class NotMasterException : Exception
    {
        public NotMasterException()
        {
        }

        public NotMasterException(string message) : base(message)
        {
        }

        public NotMasterException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
