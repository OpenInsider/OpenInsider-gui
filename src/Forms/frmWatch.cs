using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Core.Files;
using OpenInsider.Core;

namespace OpenInsider
{
	public partial class frmWatch : Form
	{
		public frmWatch()
		{
			InitializeComponent();

			DataType.Items.Clear();
			foreach (var item in Enum.GetValues(typeof(WatchFormat)))
				DataType.Items.Add(item);

			ElfVar.Items.Clear();
			if (Board.ProjectFile != null)
				foreach (var item in Board.ProjectFile.Symbols.Where(x=>x.Typ == SymbolType.Object).OrderBy(x=>x.Value))
						ElfVar.Items.Add(item);
		}

		public static bool Execute(ref WatchedVar var)
		{
			using (frmWatch frm = new frmWatch())
			{
				/* fill form edits */
				frm.VarName.Text = var.Name;
				frm.Address.Text = TextFormats.AddressToString(var.Address);
				frm.DataSize.Text = var.Value.Length.ToString();
				frm.DataType.SelectedItem = var.Format;
				frm.Period.Text = TextFormats.PeriodToString(var.Period);

				if (frm.ShowDialog() != DialogResult.OK)
					return false;

				/* fill result */
				var.Name = frm.VarName.Text;
				var.Address = TextFormats.AddressParse(frm.Address.Text);
				var.Value = new byte[uint.Parse(frm.DataSize.Text, CultureInfo.InvariantCulture)];
				var.Period = TextFormats.PeriodParse(frm.Period.Text);
				var.Format = (WatchFormat)frm.DataType.SelectedItem;

				return true;
			}
		}

		private void ElfVar_SelectedIndexChanged(object sender, EventArgs e)
		{
			btnWatchThis.Enabled = ElfVar.SelectedIndex >= 0;
		}

		private void btnWatchThis_Click(object sender, EventArgs e)
		{
			Symbol s = ElfVar.SelectedItem as Symbol;
			if (s == null)
				return;

			VarName.Text = s.Name;
			DataSize.Text = s.Size.ToString();
			Address.Text = TextFormats.AddressToString(s.Value);
		}


	}
}
