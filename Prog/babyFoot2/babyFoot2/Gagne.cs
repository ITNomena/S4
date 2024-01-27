using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace babyFoot2
{
    public partial class Gagne : Form
    {
        public static String message;
        public Gagne()
        {
            InitializeComponent();
        }

        private void Gagne_Load(object sender, EventArgs e)
        {

        }

        public void change_message()
        {
            labelMessage.Text = message;
        }
    }
}
