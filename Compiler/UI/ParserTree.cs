using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Compiler
{
    public partial class ParserTreeTest : Form
    {
        public ParserTreeTest()
        {
            InitializeComponent();
        }

        private void ParserTreeTest_Load(object sender, EventArgs e)
        {
            
        }


        private void expandBtn_Click(object sender, EventArgs e)
        {
            treeView1.ExpandAll();
        }

        private void collapseBtn_Click(object sender, EventArgs e)
        {
            treeView1.CollapseAll();
        }
    }
}

