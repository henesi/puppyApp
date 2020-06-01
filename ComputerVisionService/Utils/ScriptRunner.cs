using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerVisionService.Utils
{
    public class ScriptRunner
    {
        public static string RunFromCmd(string file, string args)
        {
            string result = string.Empty;
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
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Script failed: " + result, ex);
            }
        }
    }
}
