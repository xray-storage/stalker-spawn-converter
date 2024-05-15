using System;
using System.Runtime.CompilerServices;

namespace SpawnConverter.Logs
{
    public abstract class LogEvent
    {
        protected virtual void Log(string message = "") => Logger.Write(message);

        protected virtual void LogError(
            CODES ID,
            string message = null,
            [CallerMemberName] string method = null,
            [CallerLineNumber] int? line = null,
            [CallerFilePath] string caller = null)
        {
            Logger.SendError(this, new LogEventArgs(ID, message, method, line, caller));
        }

        protected virtual void Assert(
            bool cond,
            CODES ID,
            string message = null,
            [CallerMemberName] string method = null,
            [CallerLineNumber] int? line = null,
            [CallerFilePath] string caller = null)
        {
            if(!cond)
            {
                Logger.SendError(this, new LogEventArgs(ID, message, method, line, caller));
            }
        }
    }

    public class LogEventArgs : EventArgs
    {
        public CODES Code { get; }
        public string Message { get; }
        public string Method { get; }
        public int? Line { get; }
        public string Caller { get; }

        public LogEventArgs(CODES code, string message, string method, int? line, string caller)
        {
            Code = code;
            Message = message;
            Method = method;
            Line = line;
            Caller = caller;
        }
    }
}
