using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenInsider.Core
{
    public enum WatchFormat
    {
        FormatArray = 0,
        FormatUInt32 = 1,
    }
    public class WatchedVar
    {
        public string Name { get; set; }         // variable name
        public UInt32 Address { get; set; }      // address of var
        public UInt32 Size { get; set; }         // size of var
        public DateTime TimeStamp { get; set; }  // time of last query
        public TimeSpan Period { get; set; }     // period of query
        public WatchFormat Format { get; set; }  // Displayed format
        public byte[] Value { get; set; }

        public string GetFormattedValue()
        {
            switch (Format)
            {
                case WatchFormat.FormatUInt32:
                    return string.Format("0x{0:X8}", AsUInt32());

                default:
                case WatchFormat.FormatArray:
                    return string.Join(":", Value.Select(x => x.ToString("X2")));
            }
        }

        public UInt32 AsUInt32()
        {
            return (UInt32)(Value[0] | (Value[1] << 8) | (Value[2] << 16) | (Value[3] << 24));
        }


    }
}
