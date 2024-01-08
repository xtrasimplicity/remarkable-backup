using Renci.SshNet;
using Renci.SshNet.Sftp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemarkableBackupLibs
{
    public class SSH : ISync
    {
        private int _timeout;
        private SftpClient _client;

        public SSH() {
            _client = new SftpClient(Configuration.SSH.Host, Configuration.SSH.Username, Configuration.SSH.Password);
            _timeout = Configuration.SSH.ConnectionTimeout;
        }

        public void FetchFiles(string sourcePath, string destinationPath)
        {
            EnsureConnected();

            if (! Directory.Exists(destinationPath))
                Directory.CreateDirectory(destinationPath);

            IEnumerable<ISftpFile> files = _client.ListDirectory(sourcePath);

            foreach(ISftpFile file in files)
            {
                if (file.Name == "." || file.Name == "..") continue;

                string absoluteSourchPath = $"{sourcePath}/{file.Name}";
                string absoluteDestinationPath = Path.Combine(destinationPath, file.Name);

                if (file.IsDirectory)
                {
                    FetchFiles(absoluteSourchPath, absoluteDestinationPath);
                    continue;
                }

                // We're working with a file itself, not a directory.

                using (Stream fs = File.Create(absoluteDestinationPath))
                {
                    _client.DownloadFile(absoluteSourchPath, fs);
                }
            }
        }

        public void PutFiles(string sourcePath, string destinationPath)
        {
            throw new NotImplementedException();
        }

        private void EnsureConnected()
        {
            if (_client.IsConnected) return;


            var task = Task.Run(() => _client.Connect());
            bool success = task.Wait(TimeSpan.FromSeconds(_timeout));

            if (!success) throw new SSHConnectionTimedOutException(Configuration.SSH.Host, Configuration.SSH.ConnectionTimeout);
        }


        public class SSHConnectionTimedOutException : Exception
        {
            public SSHConnectionTimedOutException(string host, int timeout) : base($"The connection to {host} timed out after {timeout} seconds.") { }
        }
    }
}
