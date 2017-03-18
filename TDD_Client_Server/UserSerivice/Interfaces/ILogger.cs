using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserSerivice.Interfaces
{
    public interface ILogger
    {
        void LogError(Exception exception);

        void LogWarn(Exception exception);

        void LogWarn(string message);

        void LogInfo(string message);

        void LogTrace(string message);
    }
}