using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisplayController
{
    public static class Extensions
    {
        public static UInt16 SwitchEndianness(this UInt16 i)
        {
            return (UInt16)((i << 8) + (i >> 8));
        }
    }
}
