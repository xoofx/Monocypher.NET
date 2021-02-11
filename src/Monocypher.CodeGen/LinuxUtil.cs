using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Monocypher.CodeGen
{
    public static class LinuxUtil
    {
        public static string RunLinuxExe(string exe, string arguments, string distribution = "Ubuntu-20.04", string currentDirectory = null)
        {
            if (exe == null) throw new ArgumentNullException(nameof(exe));
            if (arguments == null) throw new ArgumentNullException(nameof(arguments));
            if (distribution == null) throw new ArgumentNullException(nameof(distribution));

            // redirect to a file the output as there is a bug reading back stdout with WSL
            var wslOut = $"wsl_stdout_{Guid.NewGuid()}.txt";

            bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            if (isWindows)
            {
                arguments = $"-d {distribution} {exe} {arguments} > {wslOut}";
                exe = "wsl.exe";
            }

            StringBuilder errorBuilder = null;
            StringBuilder outputBuilder = new StringBuilder();

            using (var process = new Process()
            {
                StartInfo = new ProcessStartInfo(exe, arguments)
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = !isWindows,
                    CreateNoWindow = true,
                    RedirectStandardError = true,
                    WorkingDirectory = currentDirectory ?? Environment.CurrentDirectory
                },
            })
            {

                process.ErrorDataReceived += (sender, args) =>
                {
                    if (errorBuilder == null)
                    {
                        errorBuilder = new StringBuilder();
                    }

                    errorBuilder.Append(args.Data).Append('\n');
                };

                if (!isWindows)
                {
                    process.OutputDataReceived += (sender, args) => { outputBuilder.Append(args.Data).Append('\n'); };
                }

                process.Start();
                process.BeginErrorReadLine();

                if (!isWindows)
                {
                    process.BeginOutputReadLine();
                }

                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    throw new InvalidOperationException($"Error while running command `{exe} {arguments}`: {errorBuilder}");
                }

                if (isWindows)
                {
                    var generated = Path.Combine(process.StartInfo.WorkingDirectory, wslOut);
                    var result = File.ReadAllText(generated);
                    try
                    {
                        File.Delete(generated);
                    }
                    catch
                    {
                        // ignore
                    }

                    return result;
                }
            }

            return outputBuilder.ToString();
        }
    }
}