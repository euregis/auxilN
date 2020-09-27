namespace Auxil
{
    partial class frmAux
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAux));
            this.ntiAux = new System.Windows.Forms.NotifyIcon(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.txtPesquisa = new System.Windows.Forms.TextBox();
            this.lstItens = new System.Windows.Forms.ListBox();
            this.sessionChangeHandler1 = new Auxil.SessionChangeHandler();
            this.SuspendLayout();
            // 
            // ntiAux
            // 
            this.ntiAux.Icon = ((System.Drawing.Icon)(resources.GetObject("ntiAux.Icon")));
            this.ntiAux.Text = "Aux";
            this.ntiAux.Visible = true;
            //this.ntiAux.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ntiAux_MouseMove);
            this.ntiAux.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ntiAux_MouseClick);
            this.ntiAux.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Gadugi", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(-1, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "Pesquisa";
            // 
            // txtPesquisa
            // 
            this.txtPesquisa.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPesquisa.Font = new System.Drawing.Font("Gadugi", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPesquisa.Location = new System.Drawing.Point(55, 3);
            this.txtPesquisa.Name = "txtPesquisa";
            this.txtPesquisa.Size = new System.Drawing.Size(247, 22);
            this.txtPesquisa.TabIndex = 1;
            this.txtPesquisa.TextChanged += new System.EventHandler(this.txtPesquisa_TextChanged);
            this.txtPesquisa.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPesquisa_KeyDown);
            // 
            // lstItens
            // 
            this.lstItens.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstItens.Font = new System.Drawing.Font("Gadugi", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstItens.FormattingEnabled = true;
            this.lstItens.ItemHeight = 14;
            this.lstItens.Location = new System.Drawing.Point(2, 27);
            this.lstItens.Name = "lstItens";
            this.lstItens.Size = new System.Drawing.Size(300, 102);
            this.lstItens.TabIndex = 2;
            this.lstItens.DoubleClick += new System.EventHandler(this.lstItens_DoubleClick);
            this.lstItens.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstItens_KeyDown);
            // 
            // sessionChangeHandler1
            // 
            this.sessionChangeHandler1.Location = new System.Drawing.Point(222, 3);
            this.sessionChangeHandler1.Name = "sessionChangeHandler1";
            this.sessionChangeHandler1.Size = new System.Drawing.Size(75, 25);
            this.sessionChangeHandler1.TabIndex = 3;
            this.sessionChangeHandler1.Text = "sessionChangeHandler1";
            this.sessionChangeHandler1.Visible = false;
            // 
            // frmAux
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(304, 130);
            this.Controls.Add(this.sessionChangeHandler1);
            this.Controls.Add(this.lstItens);
            this.Controls.Add(this.txtPesquisa);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Gadugi", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmAux";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Auxil";
            this.TopMost = true;
            this.Deactivate += new System.EventHandler(this.frmAux_Deactivate);
            this.Activated += new System.EventHandler(this.frmAux_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmAux_FormClosing);
            this.Resize += new System.EventHandler(this.frmAux_Resize);
            //this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmAux_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon ntiAux;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPesquisa;
        private System.Windows.Forms.ListBox lstItens;
        private SessionChangeHandler sessionChangeHandler1;
    }
}

