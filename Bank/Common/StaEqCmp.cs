using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    static public class Tools
    {
        static int h17(this byte[] obj) 
        {
            int result = 0;
            for (int i = 0; i < obj.Length; i++)
            {
                result = 17 * result + obj[i];  
            }
            return result;
        }


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
