using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerVisionService.Utils
{
    public class ScriptRunner
    {
        public static string RunFromCmd(string file, string args, out string elapsedTime)
        {
            string result = string.Empty;
            var sw = Stopwatch.StartNew();
            try
            {
                var info = new ProcessStartInfo(file)
                {
                    Arguments = args,
                    RedirectStandardInput = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var proc = new Process())
                {
                    proc.StartInfo = info;
                    proc.Start();
                    proc.WaitForExit(100000);
                    result += proc.StandardError.ReadToEnd();
                    result += proc.StandardOutput.ReadToEnd();
                }
                sw.Stop();
                elapsedTime = sw.Elapsed.ToString();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Script failed: " + result, ex);
            }
        }
    }
}
