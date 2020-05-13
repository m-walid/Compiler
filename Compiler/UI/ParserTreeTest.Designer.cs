namespace Compiler
{
    partial class ParserTreeTest
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.expandBtn = new System.Windows.Forms.Button();
            this.collapseBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(-1, 3);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(432, 448);
            this.treeView1.TabIndex = 0;
            // 
            // expandBtn
            // 
            this.expandBtn.Location = new System.Drawing.Point(463, 120);
            this.expandBtn.Name = "expandBtn";
            this.expandBtn.Size = new System.Drawing.Size(75, 23);
            this.expandBtn.TabIndex = 1;
            this.expandBtn.Text = "Expand";
            this.expandBtn.UseVisualStyleBackColor = true;
            this.expandBtn.Click += new System.EventHandler(this.expandBtn_Click);
            // 
            // collapseBtn
            // 
            this.collapseBtn.Location = new System.Drawing.Point(463, 300);
            this.collapseBtn.Name = "collapseBtn";
            this.collapseBtn.Size = new System.Drawing.Size(75, 23);
            this.collapseBtn.TabIndex = 2;
            this.collapseBtn.Text = "Minimize";
            this.collapseBtn.UseVisualStyleBackColor = true;
            this.collapseBtn.Click += new System.EventHandler(this.collapseBtn_Click);
            // 
            // ParserTreeTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(575, 450);
            this.Controls.Add(this.collapseBtn);
            this.Controls.Add(this.expandBtn);
            this.Controls.Add(this.treeView1);
            this.Name = "ParserTreeTest";
            this.Text = "ParserTreeTest";
            this.Load += new System.EventHandler(this.ParserTreeTest_Load);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Button expandBtn;
        private System.Windows.Forms.Button collapseBtn;
    }
}