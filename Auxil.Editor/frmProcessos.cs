using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Auxil.Entidades;
using Auxil.AcessoDados;

namespace Auxil.Editor
{
    public partial class frmProcessos : Form
    {
        Processo processo=null;
        
        ContextMenu mnu = new ContextMenu();

        public frmProcessos()
        {
            InitializeComponent();
            
            BancoDados.Config(Auxil.AcessoDados.TipoConexao.SQLite, new string[] { Auxil.Editor.Properties.Settings.Default.Local});
            BancoDados.AbrirSessao();
            treeView1.Nodes.Clear();
            CarregarProcessos(null, treeView1.Nodes);
            BancoDados.FecharSessao();
            
            MenuItem myMenuItem = new MenuItem("Incluir");
            myMenuItem.Tag = 0;
            mnu.MenuItems.Add(myMenuItem);
            myMenuItem.Click += new EventHandler(myMenuItem_Click);

            myMenuItem = new MenuItem("Alterar");
            myMenuItem.Tag = -1;

            mnu.MenuItems.Add(myMenuItem);
            myMenuItem.Click += new EventHandler(myMenuItem_Click);


            ContextMenu mnu2 = new ContextMenu();
            MenuItem myMenuItem2 = new MenuItem("Incluir");
            myMenuItem2.Tag = 0;
            mnu2.MenuItems.Add(myMenuItem2);
            myMenuItem2.Click += new EventHandler(myMenuItem_Click);
            treeView1.ContextMenu = mnu2;
        }

        private void CarregarProcessos(Processo p, TreeNodeCollection n)
        {
            IList<Processo> procs = null;
            if (rdbLocal.Checked)
                procs = new AcessoDados.DAO.BaseDAO<Processo>().Retorna(x => x.ProcessoPai == p && x.Maquina == System.Environment.MachineName).OrderBy(x => x.Nome).ToList<Processo>();
            else
                procs = new AcessoDados.DAO.BaseDAO<Processo>().Retorna(x => x.ProcessoPai == p && x.Maquina == "").OrderBy(x => x.Nome).ToList<Processo>();
            
            TreeNode tn = null;
            foreach (var item in procs)
            {
                tn = new TreeNode(item.Nome);
                tn.Tag = item;
                if (item.Inativo)
                    tn.ForeColor = Color.Silver;
                n.Add(tn);
                CarregarProcessos(item, tn.Nodes);
            }

        }
        void myMenuItem_Click(object sender, EventArgs e)
        {
            frmCadProcesso frm = new frmCadProcesso();
            if ((int)((Menu)sender).Tag == 0)
            { 
                Processo p = new Processo(){
                    ProcessoPai = processo,
                    //CampoPesquisa=true,
                    ValorPesquisavel=true,
                    ExecutarValor=true
                };
                if (rdbLocal.Checked)
                    p.Maquina = System.Environment.MachineName;
                else
                    p.Maquina = "";
                processo=p;
            }
            frm.Processo = processo;
            frm.Carregar(this);
            
        }
        public void Recarregar()
        {
            treeView1.Nodes.Clear();
            BancoDados.AbrirSessao();
            CarregarProcessos(null, treeView1.Nodes);
            BancoDados.FecharSessao();
            processo = null;
        }
        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                processo = (Processo)e.Node.Tag;
                mnu.Show(treeView1, e.Location);
            }
        }

        private void rdbLocal_CheckedChanged(object sender, EventArgs e)
        {
            if(rdbCompartilhado.Checked)
                BancoDados.Config(Auxil.AcessoDados.TipoConexao.SQLite, new string[] { Auxil.Editor.Properties.Settings.Default.Compartilhado });
            else
                BancoDados.Config(Auxil.AcessoDados.TipoConexao.SQLite, new string[] { Auxil.Editor.Properties.Settings.Default.Local });
            
            treeView1.Nodes.Clear();
            BancoDados.AbrirSessao();
            CarregarProcessos(null, treeView1.Nodes);
            BancoDados.FecharSessao();
        }
    }
}
