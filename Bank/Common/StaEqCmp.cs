using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    static public class StaEqCmp
    {
        static public bool IsEqual(this byte[] obj, byte[] other)
        {
            if (obj.Length != other.Length)
                return false;

            for (int i = 0; i < obj.Length; i++)
                if (obj[i] != other[i])
                    return false;

            return true;
        }
    }
}
