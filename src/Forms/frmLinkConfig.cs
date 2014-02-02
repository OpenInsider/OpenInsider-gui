using OpenInsider.Core.LinkLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenInsider
{
    public partial class frmLinkConfig : Form
    {
        public frmLinkConfig()
        {
            InitializeComponent();
        }

        public static bool Execute(ILinkLayer ll)
        {
            using (frmLinkConfig frm = new frmLinkConfig())
            {
                frm.Config.SelectedObject = ll.Configuration;

                if (frm.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    return false;

                ll.Configuration = frm.Config.SelectedObject;
            }
            return true;
        }

    }
}
