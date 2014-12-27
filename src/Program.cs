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

            //Board.Link = new SynchronousSerialPort();
            Board.Link = new SynchronousFtdiSerialPort();
			Board.Watches.Add(new WatchedVar(4) { Name = "tmp", Address = 0x20000008, Format = WatchFormat.UnsignedInt, Period = TimeSpan.FromMilliseconds(100) });
			Board.Watches.Add(new WatchedVar(4) { Name = "tmp2", Address = 0x2000000c, Format = WatchFormat.UnsignedInt, Period = TimeSpan.FromMilliseconds(100) });

            Application.Run(new frmMain());

            Board.Dispose();            
        }
    }
}
