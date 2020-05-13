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
    public partial class Form1 : Form
    {
        Scanner scanner;
        string code;
        int rowCount = 0;
        Label lexeme;
        Label tokens;
        public Form1()
        {
            InitializeComponent();
        }

        private void codeBox_Enter(object sender, EventArgs e)
        {
            if(codeBox.Text == "Enter code here")
            {
                codeBox.Text = "";
                codeBox.ForeColor = Color.Black;
            }
        }

        private void codeBox_Leave(object sender, EventArgs e)
        {
            if (codeBox.Text == "")
            {
                codeBox.Text = "Enter code here";
                codeBox.ForeColor = Color.Gray;
              
            }
        }

  
        private void codeBox_TextChanged(object sender, EventArgs e)
        {
            if (codeBox.Text != "")
            { 
                compileBtn.Enabled = true;
                resetBtn.Enabled = true;
            }
            else
            {
                compileBtn.Enabled = false;
                resetBtn.Enabled = false;
            }
        }

        private void resetBtn_Click(object sender, EventArgs e)
        {
            
            codeBox.Text = "Enter code here";
            codeBox.ForeColor = Color.Gray;
            resetBtn.Enabled = false;
            compileBtn.Enabled = false;
            parseBtn.Enabled = false;
            tabel1.Controls.Clear();
            tabel1.RowCount = 0;
            rowCount = 0;
           

            
        }

        private void compileBtn_Click(object sender, EventArgs e)
        {
            tabel1.Controls.Clear();
            tabel1.RowCount = 0;
            rowCount = 0;
            code = codeBox.Text;
            
            tabel1.Visible = true;
            scanner = new Scanner(codeBox.Text + " ");
            AddLexeme();
            parseBtn.Enabled = true;
            rowCount++;
            //tabel1.RowCount++;
           
        }
        

       public void AddLexeme()
        {
            if (scanner.flag == false)
            {
                foreach (Token token in scanner.tokens)
                {
                    
                    lexeme = new Label();
                    tokens = new Label();
                    lexeme.Text = token.lexeme;
                    lexeme.UseMnemonic=false;
                    tokens.Text = token.tokenType.ToString();
                    tabel1.Controls.Add(lexeme, 0, rowCount);
                    tabel1.Controls.Add(tokens, 1, rowCount);
                    rowCount++;
                    tabel1.RowCount++;
                }
            }
            else
            {
                PopUpForm popUpForm = new PopUpForm();
                popUpForm.errorLabel.Text = scanner.errorMsg;
                popUpForm.errorLabel.UseMnemonic = false;
                DialogResult dialogresult = popUpForm.ShowDialog();
                codeBox.Focus();
                codeBox.SelectionStart = scanner.index;

                popUpForm.Dispose();
                
            }
        }

        private void parseBtn_Click(object sender, EventArgs e)
        {
            ParserTreeTest parseTreeTest = new ParserTreeTest();
            Parser parser = new Parser(scanner.tokens);
            parseTreeTest.treeView1.Nodes.AddRange(parser.root_nodes.ToArray());
            //parseTreeTest.treeView1.Nodes.RemoveAt(parseTreeTest.treeView1.Nodes.Count - 1);
            DialogResult dialogResultparse = parseTreeTest.ShowDialog();
            parseTreeTest.Dispose();
        }
    }
}
