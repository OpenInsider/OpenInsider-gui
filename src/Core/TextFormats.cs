using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenInsider.Core
{
	static class TextFormats
	{
		public static UInt32 AddressParse(string str)
		{
			str = str.Trim();

			if (str.StartsWith("0x"))
				str = str.Substring(2);

			return uint.Parse(str, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
		}

		public static string AddressToString(UInt32 addr)
		{
			return string.Format("0x{0:X8}", addr);
		}

		public static TimeSpan PeriodParse(string str)
		{
			str = str.Trim();
			if (str == "fastest")
				return TimeSpan.FromMilliseconds(0);

			if (str.EndsWith("ms"))
			{
				str = str.Substring(0, str.Length - 2);
				return TimeSpan.FromMilliseconds(double.Parse(str,CultureInfo.InvariantCulture));
			}

			if (str.EndsWith("s"))
			{
				str = str.Substring(0, str.Length - 1);
				return TimeSpan.FromSeconds(double.Parse(str, CultureInfo.InvariantCulture));
			}

			return TimeSpan.Parse(str, CultureInfo.InvariantCulture);
		}

		public static string PeriodToString(TimeSpan period)
		{
			if (period.TotalMilliseconds == 0)
				return "fastest";

			if (period.TotalMilliseconds < 10000)
				return period.TotalMilliseconds.ToString() + "ms";

			if (period.TotalSeconds < 120)
				return period.TotalSeconds.ToString() + "s";

			return period.ToString();
		}
	}
}
