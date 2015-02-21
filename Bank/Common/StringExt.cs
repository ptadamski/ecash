using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    static public class StringExt
    {
        static public string GetString(this byte[] data)
        {
            return Encoding.Default.GetString(data);
        }

        static public byte[] GetBytes(this string data)
        {
            return Encoding.Default.GetBytes(data);
        }
    }
}
