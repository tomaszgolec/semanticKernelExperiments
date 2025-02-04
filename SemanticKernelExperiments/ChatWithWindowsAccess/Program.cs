using System;
using System.Diagnostics;
using System.IO;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        if (IsRunningAsAdmin())
        {
            Console.WriteLine("This program cannot be run with administrative privileges. Please re-run the program as a standard user (non-admin). For example, close this window and open the program without selecting 'Run as Administrator'.");
            return;
        }

        Console.WriteLine("Persistent CMD Proxy");
        Console.WriteLine("Type a command to execute in the command line, or type 'exit' to quit.");

        RunPersistentShell();
    }

    static bool IsRunningAsAdmin()
    {
        try
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while checking admin privileges: {ex.Message}");
            return false;
        }
    }

    static void RunPersistentShell()
    {
        using (Process process = new Process())
        {
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.RedirectStandardInput = true;  // Redirect standard input
            process.StartInfo.RedirectStandardOutput = true; // Redirect standard output
            process.StartInfo.RedirectStandardError = true;  // Redirect standard error
            process.StartInfo.UseShellExecute = false;       // Don't use shell execute
            process.StartInfo.CreateNoWindow = true;         // Don't create a command window

            process.Start();

            StreamWriter inputWriter = process.StandardInput;
            StreamReader outputReader = process.StandardOutput;
            StreamReader errorReader = process.StandardError;

            Task.Run(() => ReadOutputAsync(outputReader)); // Asynchronous output reading
            Task.Run(() => ReadOutputAsync(errorReader)); // Asynchronous error reading

            while (true)
            {
                Console.Write("CMD> ");
                string command = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(command))
                    continue;

                if (command.ToLower() == "exit")
                {
                    inputWriter.WriteLine("exit");
                    inputWriter.Flush();
                    break;
                }

                inputWriter.WriteLine(command);
                inputWriter.Flush();
            }

            process.WaitForExit();
        }
    }

    static async Task ReadOutputAsync(StreamReader reader)
    {
        char[] buffer = new char[1024];
        while (!reader.EndOfStream)
        {
            int read = await reader.ReadAsync(buffer, 0, buffer.Length);
            if (read > 0)
            {
                Console.Write(new string(buffer, 0, read));
            }
        }
    }
}
