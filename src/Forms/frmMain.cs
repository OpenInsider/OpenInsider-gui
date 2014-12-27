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
using Core.Files;

namespace OpenInsider
{
    public partial class frmMain : Form
    {
		

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            Board.Link.ConnectionChanged += Link_ConnectionChanged;
			Board.WatchesUpdated += Board_ActiveWatchesUpdated;

            Link_ConnectionChanged(Board.Link, EventArgs.Empty);
			Board_ActiveWatchesUpdated(null, -1);
			
        }

		private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
		{
			Board.Link.ConnectionChanged -= Link_ConnectionChanged;
			Board.WatchesUpdated -= Board_ActiveWatchesUpdated;
		}

		void Board_ActiveWatchesUpdated(object sender, int e)
		{
			if (e < 0)
				Watch.RowCount = Board.Watches.Count;
			else
				Watch.UpdateCellValue(1, e);
		}

        void Link_ConnectionChanged(object sender, EventArgs e)
        {
            button2.Text = (Board.Link.IsOpen) ? "Close" : "Open";
            button2.BackColor = (Board.Link.IsOpen) ? Color.LightPink : Color.LightGreen;
        }

        private void Watch_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
			if ((e.RowIndex < 0) || (e.RowIndex >= Board.Watches.Count))
				return;

            WatchedVar var = Board.Watches[e.RowIndex];

            switch (e.ColumnIndex)
            {
                case 0: e.Value = var.Name; break;
                case 1: e.Value = var.GetFormattedValue(); break;
                case 2: e.Value = TextFormats.AddressToString(var.Address); break;
                case 3: e.Value = TextFormats.PeriodToString(var.Period); break;
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

        private void button3_Click(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = Protocol.GetBoardInfo();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!Board.Link.IsOpen)
                return;

			Board.Poll();
        }

		private void button4_Click(object sender, EventArgs e)
		{
			using (OpenFileDialog ofd = new OpenFileDialog())
			{
				ofd.Filter = "Elf files (*.elf)|*.elf";
				if (ofd.ShowDialog() != System.Windows.Forms.DialogResult.OK)
					return;

				Board.ProjectFile = new ElfFile(ofd.FileName);

				MessageBox.Show("Load ELF OK");
			}
		}

		private void btnRemoveWatch_Click(object sender, EventArgs e)
		{
			if (Watch.SelectedRows.Count != 1)
				return;

			foreach (DataGridViewRow r in Watch.SelectedRows)
				Board.WatchRemove(r.Index);
		}

		private void btnNewWatch_Click(object sender, EventArgs e)
		{
			WatchedVar watch = new WatchedVar();
			if (frmWatch.Execute(ref watch))
				Board.WatchAdd(watch);
		}

		private void btnEditWatch_Click(object sender, EventArgs e)
		{
			foreach (DataGridViewRow r in Watch.SelectedRows)
			{
				WatchedVar watch = Board.Watches[r.Index];

				if (frmWatch.Execute(ref watch))
					Board.WatchUpdated(r.Index);
			}
		}
        
    }
}
