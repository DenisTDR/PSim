using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PSim
{
    public partial class GraphicForm : Form
    {
        public GraphicForm()
        {
            InitializeComponent();
            this.Load += GraphicForm_Load;
        }

        void GraphicForm_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.tPanel1.Clear();
        }
    }
}
