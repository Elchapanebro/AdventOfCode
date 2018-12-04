using System.Diagnostics;
using System.Runtime.InteropServices;

namespace AdventOfCode.Utilities
{
    public static class Clipboard
    {
        public static string CopyToClipboard(string content)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return Run("cmd.exe", $"/c \"echo {content.Replace("\"", "\\\"")} | clip\"");
            }
            else
            {
                return Run("/bin/bash", $"-c \"echo \"{content.Replace("\"", "\\\"")}\" | pbcopy\"");
            }
        }

        private static string Run(string filename, string arguments)
        {
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = filename,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = false,
                }
            };
            process.Start();

            string result = process.StandardOutput.ReadToEnd();

            process.WaitForExit();

            return result;
        }
    }
}
