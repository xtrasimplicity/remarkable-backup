using RemarkableBackupLibs.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemarkableBackupLibs
{
    public class Configuration
    {
        private const string CONFIGURATION_FILE = "config.cfg";
        public struct SSHConfiguration
        {
            internal int _connectionTimeOut;
            internal string _host;
            internal string _username;
            internal string _password;

            public string Host { get { return _host; } }
            public string Username { get { return _username; } }
            public string Password { get { return _password; } }
            public int ConnectionTimeout { get { return _connectionTimeOut;} }
        }

        public struct SyncConfiguration
        {
            internal string _targetPath;

            public string TargetPath { get { return _targetPath; } }
        }

        public static SSHConfiguration SSH = new SSHConfiguration();
        public static SyncConfiguration Sync = new SyncConfiguration();

        public static void Load()
        {
            if (!File.Exists(CONFIGURATION_FILE)) return;

            string[] rawContents = File.ReadAllLines(CONFIGURATION_FILE);

            foreach(string line in rawContents)
            {
                string[] parts = line.Split(new string[] { "=" }, 2, StringSplitOptions.RemoveEmptyEntries);

                string key = parts.First().Trim();
                string value = parts.Last().Trim().RemoveSurroundingQuotes();

                switch(key.ToLower())
                {
                    case "ssh.host":
                        SSH._host = value;
                        continue;
                    case "ssh.username":
                        SSH._username = value;
                        continue;
                    case "ssh.password":
                        SSH._password = value;
                        continue;
                    case "ssh.connection_timeout":
                        SSH._connectionTimeOut = int.Parse(value);
                        continue;
                    case "sync.target_path":
                        Sync._targetPath = value;
                        continue;

                }
            }
        }
    }

    public class MissingConfigurationException : Exception
    {
        public MissingConfigurationException(string message) : base(message) { }
    }
}
