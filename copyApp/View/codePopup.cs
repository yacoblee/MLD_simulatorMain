using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace copyApp.View
{
    public partial class codePopup : Form
    {
        public string Code => textBox1.Text;
        public codePopup()
        {
            InitializeComponent();
        }

        private void barBtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
