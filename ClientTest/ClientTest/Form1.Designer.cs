namespace ClientTest
{
    partial class CreClients1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Clientsquan = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.Clientquantity_Box = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ReqQuanBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // Clientsquan
            // 
            this.Clientsquan.AutoSize = true;
            this.Clientsquan.Location = new System.Drawing.Point(12, 9);
            this.Clientsquan.Name = "Clientsquan";
            this.Clientsquan.Size = new System.Drawing.Size(111, 20);
            this.Clientsquan.TabIndex = 0;
            this.Clientsquan.Text = "Clients quantity";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(261, 166);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(94, 29);
            this.button1.TabIndex = 1;
            this.button1.Text = "Start";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Clientquantity_Box
            // 
            this.Clientquantity_Box.DisplayMember = "(none)";
            this.Clientquantity_Box.FormattingEnabled = true;
            this.Clientquantity_Box.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20"});
            this.Clientquantity_Box.Location = new System.Drawing.Point(131, 6);
            this.Clientquantity_Box.Name = "Clientquantity_Box";
            this.Clientquantity_Box.Size = new System.Drawing.Size(58, 28);
            this.Clientquantity_Box.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(212, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Requst quantity vor each client";
            // 
            // ReqQuanBox
            // 
            this.ReqQuanBox.Location = new System.Drawing.Point(230, 55);
            this.ReqQuanBox.Name = "ReqQuanBox";
            this.ReqQuanBox.Size = new System.Drawing.Size(125, 27);
            this.ReqQuanBox.TabIndex = 4;
            // 
            // CreClients1
            // 
            this.ClientSize = new System.Drawing.Size(392, 217);
            this.Controls.Add(this.ReqQuanBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Clientquantity_Box);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Clientsquan);
            this.Name = "CreClients1";
            this.Text = "Create Clients";
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        private Label Clientsquan;
        private Button button1;
        private ComboBox Clientquantity_Box;
        private Label label1;
        private TextBox ReqQuanBox;
    }
}