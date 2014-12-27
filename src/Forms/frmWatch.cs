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
		}

		public static bool Execute(ref WatchedVar var)
		{
			using (frmWatch frm = new frmWatch())
			{
				/* fill form edits */
				frm.Address.Text = TextFormats.AddressToString(var.Address);
				frm.DataSize.Text = var.Value.Length.ToString();
				frm.DataType.SelectedItem = var.Format;
				frm.Period.Text = TextFormats.PeriodToString(var.Period);

				if (frm.ShowDialog() != DialogResult.OK)
					return false;

				/* fill result */
				var.Address = TextFormats.AddressParse(frm.Address.Text);
				var.Value = new byte[uint.Parse(frm.DataSize.Text, CultureInfo.InvariantCulture)];
				var.Period = TextFormats.PeriodParse(frm.Period.Text);
				var.Format = (WatchFormat)frm.DataType.SelectedItem;

				return true;
			}
		}


	}
}
