using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TA.NexDome.Server
    {
    public partial class DowngradeWarning : Form
        {
        public DowngradeWarning()
            {
            InitializeComponent();
            }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
            {
            WindowUtils.NavigateToWebDestination(sender);
            }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
            {
            WindowUtils.NavigateToWebDestination(sender);
            }
        }
    }
