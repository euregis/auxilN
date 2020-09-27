namespace Auxil.Editor
{
    partial class frmProcessos
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
            this.rdbLocal = new System.Windows.Forms.RadioButton();
            this.rdbCompartilhado = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView1.Location = new System.Drawing.Point(12, 35);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(313, 465);
            this.treeView1.TabIndex = 0;
            this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
            // 
            // rdbLocal
            // 
            this.rdbLocal.AutoSize = true;
            this.rdbLocal.Checked = true;
            this.rdbLocal.Location = new System.Drawing.Point(33, 12);
            this.rdbLocal.Name = "rdbLocal";
            this.rdbLocal.Size = new System.Drawing.Size(51, 17);
            this.rdbLocal.TabIndex = 1;
            this.rdbLocal.TabStop = true;
            this.rdbLocal.Text = "Local";
            this.rdbLocal.UseVisualStyleBackColor = true;
            this.rdbLocal.CheckedChanged += new System.EventHandler(this.rdbLocal_CheckedChanged);
            // 
            // rdbCompartilhado
            // 
            this.rdbCompartilhado.AutoSize = true;
            this.rdbCompartilhado.Location = new System.Drawing.Point(188, 12);
            this.rdbCompartilhado.Name = "rdbCompartilhado";
            this.rdbCompartilhado.Size = new System.Drawing.Size(92, 17);
            this.rdbCompartilhado.TabIndex = 2;
            this.rdbCompartilhado.TabStop = true;
            this.rdbCompartilhado.Text = "Compartilhado";
            this.rdbCompartilhado.UseVisualStyleBackColor = true;
            this.rdbCompartilhado.CheckedChanged += new System.EventHandler(this.rdbLocal_CheckedChanged);
            // 
            // frmProcessos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(337, 512);
            this.Controls.Add(this.rdbCompartilhado);
            this.Controls.Add(this.rdbLocal);
            this.Controls.Add(this.treeView1);
            this.Name = "frmProcessos";
            this.Text = "Processos";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.RadioButton rdbLocal;
        private System.Windows.Forms.RadioButton rdbCompartilhado;
    }
}

