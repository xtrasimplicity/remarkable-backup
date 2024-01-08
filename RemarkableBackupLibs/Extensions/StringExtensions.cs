using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemarkableBackupLibs.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveSurroundingQuotes(this string str)
        {
            if (str == null) return str;

            string s = str;

            if (str[0] == '"') s = s.Substring(1, s.Length - 1);

            if (str[str.Length - 1] == '"') s = s.Substring(0, s.Length - 1);

            return s;
        }
    }
}
