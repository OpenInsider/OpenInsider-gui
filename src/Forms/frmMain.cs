using OpenInsider.Core.LinkLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenInsider
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            Watch.RowCount = 5;
        }

        private void Watch_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            switch (e.ColumnIndex)
            {
                case 0: e.Value = "Variable"; break;
                case 1: e.Value = "3.14159 mV"; break;
                case 2: e.Value = "0x00000000"; break;
                case 3: e.Value = "100 ms"; break;
            }
        }

        ILinkLayer Link;

        private void button1_Click(object sender, EventArgs e)
        {
            if (Link == null)
                Link = new LinkSerial();

            frmLinkConfig.Execute(Link);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Link == null)
                throw new InvalidOperationException("Link is not defined!");

            if (Link.Opened)
                Link.Close();
            else
                Link.Open();

            button2.Text = (Link.Opened) ? "Close" : "Open";
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            if ((Link != null) && (Link is IDisposable))
                (Link as IDisposable).Dispose();
        }
    }
}
