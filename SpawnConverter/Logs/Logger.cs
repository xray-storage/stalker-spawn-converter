using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace SpawnConverter.Logs
{
    public sealed class Logger
    {
        public static event EventHandler<LogEventArgs> OnError;

        public static async void Write(string message)
        {
            using StreamWriter sw = new(Path.Combine(LOGINFO.DIRECTORY, LOGINFO.FILENAME), true);
            await sw.WriteAsync($"{message + sw.NewLine}");
        }

        public static async void CreateLog()
        {
            if (!Directory.Exists(LOGINFO.DIRECTORY))
            {
                Directory.CreateDirectory(LOGINFO.DIRECTORY);
            }

            StringBuilder sb = new();

            sb.AppendLine($"Loading: {LOGINFO.NAME} v.{LOGINFO.VERSION}");
            sb.AppendLine($"Start time: {DateTime.Now}");
            sb.AppendLine();

            using StreamWriter sw = new(Path.Combine(LOGINFO.DIRECTORY, LOGINFO.FILENAME));
            await sw.WriteAsync($"{sb}");

            sb.Clear();
        }

        public static void SendError(object sender, LogEventArgs e)
        {
            OnError(sender, e);
            Write(FormatErrorMessage(sender, e));
        }

        public static string FormatErrorMessage(object sender, LogEventArgs e, bool for_wndow = false)
        {
            StringBuilder sb = new();

            string source = e.Caller;
            string message = string.IsNullOrEmpty(e.Message) ? ErrorMessage.GetMessageByCode(e.Code) : e.Message;

            #region For compilation only: trim the path to the solution.

            if (!string.IsNullOrEmpty(source))
            {
                source = Regex.Replace(source, "^(.*SpawnConverter)?", "").Trim('\\');
            }

            #endregion

            if (!for_wndow)
            {
                sb.AppendLine();
                sb.AppendLine($"ERROR!!!");
            }

            sb.AppendLine($"Error code:\t{(uint)e.Code}");
            sb.AppendLine($"Source:\t\t{source}");
            sb.AppendLine($"Method:\t\t{e.Method}()");
            sb.AppendLine($"Line:\t\t{e.Line}");
            sb.AppendLine($"Descr:\t\t{message}");

            return sb.ToString();
        }
    }

    public struct LOGINFO
    {
        public static string NAME { get; } = typeof(Logger).Assembly.GetName().Name;
        public static string VERSION { get; } = typeof(Logger).Assembly.GetName().Version.ToString();
        public static string DIRECTORY { get; } = Environment.CurrentDirectory + @"\Log";
        public static string FILENAME { get; } = NAME + ".log";
    }
}
