using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using NLog.Fluent;

namespace UserSerivice.Implimentation
{
    public class Logger : UserSerivice.Interfaces.ILogger
    {
        private static readonly NLog.Logger Log;

        static Logger()
        {
            Log = LogManager.GetCurrentClassLogger();
        }
        
        public void LogError(Exception exception)
        {
            Log.Error(exception.Message, exception);
        }
        
        public void LogWarn(Exception exception)
        {
            Log.Warn(exception.Message, exception);
        }
        
        public void LogWarn(string message)
        {
            Log.Warn(message);
        }

        /// <summary>
        /// Log info message
        /// </summary>
        /// <param name="message"></param>
        public void LogInfo(string message)
        {
            Log.Info(message);
        }

        /// <summary>
        /// Log trace message
        /// </summary>
        /// <param name="message"></param>
        public void LogTrace(string message)
        {
            Log.Trace(message);
        }
    }
}
