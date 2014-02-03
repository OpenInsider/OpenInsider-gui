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

            Board.Link = new LinkSerial();

            Application.Run(new frmMain());

            Board.Dispose();            
        }
    }
}
