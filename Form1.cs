using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace cg_lab3
{
    public partial class Loader : Form
    {
        public Loader()
        {
            InitializeComponent();
        }

        private void Task1bClick(object sender, EventArgs e)
        {
            (new Task1b.Task1b()).Show();
        }

        private void Task1aClick(object sender, EventArgs e)
        {
            (new Task1a.Task1a()).Show();
        }

        private void Task2Click(object sender, EventArgs e)
        {
            (new Task2.Task2()).Show();
        }
    }
}
