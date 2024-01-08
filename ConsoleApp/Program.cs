using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RemarkableBackupLibs;

namespace ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Configuration.Load();

            SSH ssh = new SSH();

            try
            {
                Console.WriteLine($"Attempting to fetch files from Remarkable tablet at {Configuration.SSH.Host} via SSH.");
                ssh.FetchFiles(RemarkableSpecialFolders.UserXochitlData, Configuration.Sync.TargetPath);
            }
            catch (Exception ex)
            {
                AbortWith($"Unable to fetch files from Remarkable tablet via SSH. {ex.Message}", -1);
            }

            AbortWith("Done!");
        }

        private static void AbortWith(string message, int exitCode = 0)
        {
            Console.WriteLine(message);
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();

            Environment.Exit(exitCode);
        }
    }
}
