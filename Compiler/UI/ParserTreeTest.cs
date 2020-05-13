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
            //TreeNode tNode = new TreeNode("ez");
            //tNode.Nodes.Add(new TreeNode("zoz"));
            //treeView1.Nodes.Add(tNode);

            //TreeNode tNode = new TreeNode("Windows");
            //treeView1.Nodes.Add(tNode);
            //tNode = new TreeNode("Linux");
            //treeView1.Nodes.Add(tNode);
            //treeView1.Nodes[0].Nodes.Add("zbb");
            //treeView1.Nodes[1].Nodes.Add("asdasd");
            //treeView1.Nodes[1].Nodes[0].Nodes.Add("OMG I HAVE A GRANDCHILD");
            //tNode.Nodes.Add("manga");

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

