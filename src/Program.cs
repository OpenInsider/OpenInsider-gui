using OpenInsider.Core;
using OpenInsider.Core.LinkLayer;
using System;
using System.Windows.Forms;

namespace OpenInsider
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Board.Link = new SynchronousSerialPort();
            //Board.Link = new SynchronousFtdiSerialPort();
            Board.ActiveWatches.Add(new WatchedVar() { Name = "tmp", Address = 0x2000001c, Size = 4, Format = WatchFormat.FormatUInt32, Period = TimeSpan.FromMilliseconds(100), Value = new byte[] { 0, 0, 0, 0 }, TimeStamp = DateTime.Now });

            Application.Run(new frmMain());

            Board.Dispose();            
        }
    }
}
