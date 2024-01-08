using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemarkableBackupLibs
{
    internal interface ISync
    {
        void FetchFiles(string sourcePath, string destinationPath);
        void PutFiles(string sourcePath, string destinationPath);
    }
}
