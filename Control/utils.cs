using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base
{
    public class utils
    {
        public static String GetFileSuffix(string name)
        {
            return System.IO.Path.GetExtension(name);
        }
    }
}
