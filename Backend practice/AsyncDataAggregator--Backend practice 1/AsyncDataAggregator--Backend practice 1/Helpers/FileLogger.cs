using System;
using System.Text;
using System.IO;

namespace AsyncDataAggregator__Backend_practice_1.Helpers
{
    public static class FileLogger
    {
        private static readonly string LogFilePath = "error.log";

        public static void LogError(string message)
        {
            try
            {
                string LogLine = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - ERROR: {message}";
                File.AppendAllText(LogFilePath, LogLine + Environment.NewLine);
            }
            catch
            {
                //of logging fails, do nothing, dont crach the app
            }

        }
    }

}
