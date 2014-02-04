using OpenInsider.Core;
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

        void ch_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Config.Refresh();
        }

        public static bool Execute()
        {
            using (frmLinkConfig frm = new frmLinkConfig())
            {
                frm.Config.SelectedObject = Board.Link.Configuration;
                INotifyPropertyChanged ch = Board.Link.Configuration as INotifyPropertyChanged;

                if (ch != null)
                    ch.PropertyChanged += frm.ch_PropertyChanged;

                DialogResult dr = frm.ShowDialog();

                if (ch != null)
                    ch.PropertyChanged -= frm.ch_PropertyChanged;

                if (dr != DialogResult.OK)
                    return false;

                Board.Link.Configuration = frm.Config.SelectedObject;
            }
            return true;
        }

        

    }
}
