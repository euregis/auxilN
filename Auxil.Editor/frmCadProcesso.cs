using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Auxil.Entidades;

namespace Auxil.Editor
{
    public partial class frmCadProcesso : Form
    {
        public Processo Processo;
        private bool Recarregar;
        public frmCadProcesso()
        {
            InitializeComponent();
        }
        public void Carregar(frmProcessos formOwner)
        {
            this.Owner = formOwner;
            this.ShowDialog();
            //formOwner.Hide();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Recarregar = textBox2.Text != Processo.Nome;

                AcessoDados.BancoDados.AbrirSessao();
                Processo.Nome = textBox2.Text.Trim();
                Processo.Valor = textBox3.Text.Trim();
                Processo.Parametros = textBox4.Text.Trim();
                Processo.CampoPesquisa = chkCampoPesquisa.Checked;
                Processo.AreaTransf = chkAreaTransf.Checked;
                Processo.CapturaRetorno = chkCapturaRetorno.Checked;
                Processo.ValorPesquisavel = chkValorPesquisavel.Checked;
                Processo.MostrarValor = chkMostrarValor.Checked;
                Processo.ExecutarValor = chkExecutarValor.Checked;
                Processo.Complemento = chkComplemento.Checked;
                Processo.Inativo = chkInativo.Checked;
                Processo.DataAlteracao = DateTime.Now;

                /*if (Processo.Complemento)
                    Processo.ValorPesquisavel=false;*/

                new AcessoDados.DAO.BaseDAO<Processo>().Salvar(Processo);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message);
            }
            finally
            {
                AcessoDados.BancoDados.FecharSessao();
            }
        }

        private void frmCadProcesso_Load(object sender, EventArgs e)
        {
            if (Processo.ProcessoPai != null)
                textBox1.Text = Processo.ProcessoPai.Nome;

            textBox2.Text = Processo.Nome;
            textBox3.Text = Processo.Valor;
            textBox4.Text = Processo.Parametros;
            chkCampoPesquisa.Checked= Processo.CampoPesquisa;
            chkAreaTransf.Checked= Processo.AreaTransf;
            chkCapturaRetorno.Checked= Processo.CapturaRetorno;
            chkValorPesquisavel.Checked= Processo.ValorPesquisavel;
            chkMostrarValor.Checked= Processo.MostrarValor;
            chkExecutarValor.Checked= Processo.ExecutarValor;
            chkComplemento.Checked=Processo.Complemento ;
            chkInativo.Checked = Processo.Inativo;

        }

        private void frmCadProcesso_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.None) e.Cancel = true;
            //this.Owner.Visible = true;
            if(Recarregar)
                ((frmProcessos)this.Owner).Recarregar();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
    }
}
