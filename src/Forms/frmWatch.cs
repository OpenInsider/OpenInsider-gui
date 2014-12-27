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

		private static uint TryParseAddress(string text)
		{
			text = text.Trim();

			if (text.StartsWith("0x"))
				text = text.Substring(2);

			return uint.Parse(text, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
		}

		public static bool Execute(ref WatchedVar var)
		{
			using (frmWatch frm = new frmWatch())
			{
				/* fill form edits */
				frm.Address.Text = "0x" + var.Address.ToString("X8");
				frm.DataSize.Text = var.Size.ToString();
				frm.DataType.SelectedItem = var.Format;
				frm.Period.Text = var.Period.ToString();

				if (frm.ShowDialog() != DialogResult.OK)
					return false;

				/* fill result */
				var.Address = TryParseAddress(frm.Address.Text);
				var.Size = uint.Parse(frm.DataSize.Text, CultureInfo.InvariantCulture);
				var.Period = TimeSpan.Parse(frm.Period.Text, CultureInfo.InvariantCulture);
				var.Format = (WatchFormat)frm.DataType.SelectedItem;
				var.Value = new byte[var.Size];

				return true;
			}
		}


	}
}
