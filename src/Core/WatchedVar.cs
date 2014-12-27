using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenInsider.Core
{
    public enum WatchFormat
    {
        Array = 0,
        UnsignedInt = 1,
		SignedInt = 2,
		Float = 3,
    }
    public class WatchedVar
    {
        public string Name { get; set; }         // variable name
        public UInt32 Address { get; set; }      // address of var
        public DateTime TimeStamp { get; set; }  // time of last query
        public TimeSpan Period { get; set; }     // period of query
        public WatchFormat Format { get; set; }  // Displayed format
        public byte[] Value { get; set; }

		public WatchedVar(int Size = 0)
		{
			Value = new byte[Size];
			TimeStamp = DateTime.Now;
		}

        public string GetFormattedValue()
        {
            switch (Format)
            {
				default:
				case WatchFormat.Array:
					/* array is fall back encoding when any other fails */
					break;					

				case WatchFormat.UnsignedInt:
					if ((Value.Length > 0) && (Value.Length <= 8))
						return string.Format("0x{0:X"+(Value.Length*2).ToString()+"}", AsUInt());

					/* Fall back to array */
					break;
            }

			return string.Join(":", Value.Select(x => x.ToString("X2")));
        }

        public UInt64 AsUInt()
        {
			UInt64 result = 0;

			for (int i = 0 ; i < Value.Length ; i++)
				result |= (UInt64)Value[i] << i * 8;

            return result;
        }


    }
}
