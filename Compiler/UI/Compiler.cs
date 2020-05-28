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
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;    
                return cp;
            }
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
                parseBtn.Enabled = false;
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
                    if (token.lexeme != "")
                    {
                        tabel1.Controls.Add(lexeme, 0, rowCount);
                        tabel1.Controls.Add(tokens, 1, rowCount);
                        rowCount++;
                    } 
                }
                parseBtn.Enabled = true;
            }
            else
            {
                PopUpForm popUpForm = new PopUpForm();
                popUpForm.errorLabel.Text = scanner.errorMsg;
                popUpForm.errorLabel.UseMnemonic = false;
                DialogResult dialogresult = popUpForm.ShowDialog();
                codeBox.Focus();
                codeBox.SelectionStart = scanner.index;
                
                parseBtn.Enabled = false;
                popUpForm.Dispose();  
            }
        }
        private void parseBtn_Click(object sender, EventArgs e)
        {
            ParserTreeTest parseTreeTest = new ParserTreeTest();
            try
            {
            Parser parser = new Parser(scanner.tokens);
            parseTreeTest.treeView1.Nodes.AddRange(parser.rootNodes.ToArray());
            DialogResult dialogResultparse = parseTreeTest.ShowDialog();
            parseTreeTest.Dispose();
            }
            catch(InvalidExpectedToken error)
            {
                ParseError parseError = new ParseError();
                parseError.parseErrorLbl.Text = error.Message;
                parseError.parseErrorLbl.UseMnemonic = false;
                DialogResult dialogResultParseError = parseError.ShowDialog();
                codeBox.Focus();
                parseBtn.Enabled = false;
                parseError.Dispose();
            }
        }
    }
}
