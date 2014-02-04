using OpenInsider.Core;
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

        private void frmMain_Load(object sender, EventArgs e)
        {
            Board.Link.ConnectionChanged += Link_ConnectionChanged;

            Link_ConnectionChanged(Board.Link, EventArgs.Empty);
        }

        void Link_ConnectionChanged(object sender, EventArgs e)
        {
            button2.Text = (Board.Link.IsOpen) ? "Close" : "Open";
            button2.BackColor = (Board.Link.IsOpen) ? Color.LightPink : Color.LightGreen;
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

        

        private void button1_Click(object sender, EventArgs e)
        {
            frmLinkConfig.Execute();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Board.Link == null)
                throw new InvalidOperationException("Link is not defined!");

            if (Board.Link.IsOpen)
                Board.Link.Close();
            else
                Board.Link.Open();

            
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = Protocol.GetBoardInfo();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!Board.Link.IsOpen)
                return;

            UInt32 value = 0;
            if (Protocol.ReadMem32Ex(0x2000001c, ref value))
                label1.Text = string.Format("0x{0:X8}", value);
            else
                label1.Text = "?";

        }

        
    }
}
