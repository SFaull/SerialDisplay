using System;

namespace SerialDeviceDriver
{
    public static class UInt16Extensions
    {
        public static UInt16 SwitchEndianness(this UInt16 i)
        {
            return (UInt16)((i << 8) + (i >> 8));
        }
    }
}
