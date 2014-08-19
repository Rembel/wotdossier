using System.Diagnostics;
using System.IO;

namespace WotDossier.Applications
{
    public class ExternalTask
    {
        public static void Execute(string task, string arguments, string logPath, string workingDirectory = null)
        {
            using (var proc = new Process())
            {
                proc.StartInfo.CreateNoWindow = true;
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.FileName = task;
                proc.StartInfo.Arguments = arguments;

                if (!string.IsNullOrEmpty(workingDirectory))
                {
                    proc.StartInfo.WorkingDirectory = workingDirectory;
                }

                proc.Start();

                //write log
                using (var streamWriter = new StreamWriter(logPath, false))
                {
                    streamWriter.WriteLine(proc.StandardOutput.ReadToEnd());
                }

                proc.WaitForExit();
            }
        }
    }
}