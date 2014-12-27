using OpenInsider.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenInsider.Core
{
    public class TypeConverterVersion : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value.GetType() == typeof(UInt16))
                return string.Format("{0}.{1}", ((UInt16)(value) >> 8), ((UInt16)(value) & 0x0F));
            return base.ConvertTo(context, culture, value, destinationType);
        }
    } 

    public class BoardInfo
    {
        [Category("Standard info")]
        public byte ProtocolVersion { get; private set; }

        [Category("Standard info")]
        [TypeConverter(typeof(TypeConverterVersion))]
        public UInt16 BoardVersion { get; private set; }

        [Category("Standard info")]
        public byte CommBufferSize { get; private set; }

        [Category("Standard info")]
        public bool HasExtendedInfo { get; private set; }

        [Category("Extended info")]
        public UInt16 RecorderBufferSize { get; private set; }

        [Category("Extended info")]
        public UInt16 RecorderTimeBase { get; private set; }

        [Category("Extended info")]
        public string BoardName { get; private set; }

        public static BoardInfo Parse(byte[] data)
        {
            if (data.Length < 3)
                return null;

            if (data[1] != 0x00)
                return null;

            BoardInfo bi = new BoardInfo();
            if (data.Length > 7)
            {
                bi.ProtocolVersion = data[2];
                bi.BoardVersion = (UInt16)((data[5] << 8) | data[6]);
                bi.CommBufferSize = data[7];
                bi.HasExtendedInfo = false;
            }

            if (data.Length > 35)
            {
                bi.RecorderBufferSize = (UInt16)(data[8] + data[9] << 8);
                bi.RecorderTimeBase = (UInt16)(data[10] + data[11] << 8);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < 25; i++)
                    if (data[12 + i] != 0)
                        sb.Append((char)data[12 + i]);

                bi.BoardName = sb.ToString();
                bi.HasExtendedInfo = true;
            }

            return bi;
        }
    }

    public static class Protocol
    {
        private static byte[] Transact(byte[] msg, int EstimatedSize)
        {
            if ((Board.Link == null) || !Board.Link.IsOpen)
                throw new InvalidOperationException("Link not open");

            List<byte> frame = new List<byte>();
            byte crc = 0;
            frame.Add(0x2b);
            foreach (byte b in msg)
            {
                crc += b;
                frame.Add(b);
                if (b == (byte)'+')
                    frame.Add(b);
            }
            frame.Add((byte)(0x100 - crc));

            Board.Link.DiscardInput();
            if (Board.Link.Write(frame.ToArray()) != frame.Count)
                throw new IOException();

            frame.Clear();

            int TotalSize = 3;

            do
            {
                byte[] DataToRead = new byte[EstimatedSize + 5];

                int sz = Board.Link.Read(DataToRead);
                if (sz == 0)    // timeouted
                    break;

                for (int i = 0; i < sz; i++)
                {
                    byte d = DataToRead[i];

                    switch (frame.Count)
                    {
                        case 0:
                            // search for START
                            if (d == (byte)'+')
                                frame.Add(d);
                            continue;

                        case 1:
                            // parse return code
                            frame.Add(d);
                            if (d == 0x00)
                                TotalSize = EstimatedSize + 3;
                            continue;

                        default:
                            if ((d == (byte)'+') && frame.Last() == (byte)'+')
                                continue;

                            if (frame.Count < TotalSize)
                                frame.Add(d);

                            if (frame.Count != TotalSize)
                                continue;

                            break;
                    }
                    break;
                }


            }
            while (frame.Count != TotalSize);

            if (frame.Count != TotalSize)
                return new byte[0];                // No valid frame received

            crc = 0;
            for (int i = 1; i < frame.Count; i++)
                crc += frame[i];

            if (crc != 0x00)
                return new byte[0];                // bad CRC

            return frame.ToArray();
        }

        public static BoardInfo GetBoardInfo()
        {
            BoardInfo bi = BoardInfo.Parse(Transact(new byte[] { 0xc0 }, 35));

            if (bi == null)
                bi = BoardInfo.Parse(Transact(new byte[] { 0xc8 }, 6));

            return bi;
        }

        public static bool ReadMem32Ex(UInt32 Address, ref byte[] value)
        {
            byte[] data = Transact(new byte[] { 0xe2, (byte)(Address), (byte)(Address >> 8),  (byte)(Address >> 16),  (byte)(Address >> 24) }, 4);

            if (data.Length < 3)
                return false;

            if (data[1] != 0x00)
                return false;

            for (int i=0; i<4; i++)
                value[i] = data[2 + i];
            
            return true;
        }

        public static bool ReadMem16Ex(UInt32 Address, ref byte[] value)
        {
            byte[] data = Transact(new byte[] { 0xe1, (byte)(Address), (byte)(Address >> 8), (byte)(Address >> 16), (byte)(Address >> 24) }, 2);

            if (data.Length < 3)
                return false;

            if (data[1] != 0x00)
                return false;

            for (int i = 0; i < 2; i++)
                value[i] = data[2 + i];
            

            return true;
        }

        public static bool ReadMem8Ex(UInt32 Address, ref byte value)
        {
            byte[] data = Transact(new byte[] { 0xe0, (byte)(Address), (byte)(Address >> 8), (byte)(Address >> 16), (byte)(Address >> 24) }, 1);

            if (data.Length < 3)
                return false;

            if (data[1] != 0x00)
                return false;

            value = data[2];

            return true;
        }


        public static bool ReadWatch(WatchedVar v)
        {
			DateTime now = DateTime.Now;
			if (v.TimeStamp + v.Period > now)
				return false;

			v.TimeStamp = now;

            byte[] val = v.Value;
			if (val.Length == 4)
			{
				if (!Protocol.ReadMem32Ex(v.Address, ref val))
					return false;
			}
			else if (val.Length == 2)
			{
				if (!Protocol.ReadMem16Ex(v.Address, ref val))
					return false;
			}
			else if (val.Length == 1)
			{
				if (!Protocol.ReadMem8Ex(v.Address, ref val[0]))
					return false;
			}
			else
				return false; /* noncompatible size of var, use read memory instead */
            
            v.Value = val;            
            return true;            
        }

        public static bool Detect()
        {
            return GetBoardInfo() != null;
        }
    }
}
