using OpenInsider.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenInsider.Core
{
    public static class Protocol
    {
        public static byte[] Transact(byte[] msg, int EstimatedSize)
        {
            if ((Board.Link == null) || !Board.Link.Opened)
                throw new InvalidOperationException("Link not open");

            List<byte> frame = new List<byte>();
            frame.Add(0x2b);
            frame.AddRange(msg);

            byte crc = 0;
            foreach (byte b in msg)
                crc += b;

            frame.Add((byte)(0x100 - crc));
           
            byte[] data = Board.Link.Transact(frame.ToArray());

            return data;
        }

        public static bool Detect()
        {
            byte[] data = Transact(new byte[] { 0xc0 }, 36);

            if (data.Length < 3)
                return false;

            if (data[0] != 0x2b)
                return false;

            if (data[1] != 0x00)
                return false;

            if (data.Length > 7)
            {
                byte ProtVer = data[2];
                byte VerMajor = data[5];
                byte VerMinor = data[6];
                byte BufSiz = data[7];
            }

            if (data.Length > 35)
            {
                UInt16 RecBufSiz = (UInt16)(data[8] + data[9] << 8);
                UInt16 RecTimeBase = (UInt16)(data[10] + data[11] << 8);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < 25; i++)
                    if (data[12 + i] != 0)
                        sb.Append((char)data[12 + i]);

                string boardname = sb.ToString();


                return true;
            }

            return false;
        }
    }
}
