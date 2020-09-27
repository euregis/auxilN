using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
using System.Threading;
using Auxil.Entidades;
using System.Linq.Expressions;
using Auxil.AcessoDados;

namespace Auxil
{
    public partial class frmAux : Form
    {
        #region Variáveis para CRTL + C
        [DllImport("User32.dll")]
        private static extern bool
        SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static public extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags,
        uint dwExtraInfo);
        #endregion

        #region Constantes
        private const string CONST_AREA_TRANSF = "#ATRANSF#";
        private const string CONST_INI = "#INI#";
        private const string CONST_TIPO_PASTA = " \\";
        private const string CONST_TIPO_ARQUIVO = ".\\";
        #endregion
                               
        #region Variáveis para HOTKEYS
        [DllImport("user32")]
        public static extern int RegisterHotKey(IntPtr hwnd, int id, int fsModifiers, int vk);
        [DllImport("user32.dll")]
        static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private const int MOD_ALT = 0x1;
        private const int MOD_CONTROL = 0x2;
        private const int MOD_SHIFT = 0x4;
        private const int MOD_WIN = 0x8;
        private const int WM_HOTKEY = 0x312;
        private const int WM_C = 0x430006;
        private const int WM_Q = 0x510006;
        private const int WM_W = 0x570006;
        #endregion

        private short indexSair;//INDEX DEFINIDO PARA O MENU SAIR
        private bool oculto = true;
        private List<string> areaTransf = new List<string>();


        private void ExibeOculta(bool exibeTela)
        {
            try
            {
                this.Location = new Point(Screen.PrimaryScreen.Bounds.Width - this.Width, Screen.PrimaryScreen.Bounds.Height - this.Height - 25);

                this.Visible = exibeTela;
                ntiAux.Visible = !exibeTela;
                if (exibeTela)
                {
                    this.WindowState = FormWindowState.Normal;
                }
            }
            catch (Exception)
            {
                
                
            }
            
        }

        public frmAux()
        {
            InitializeComponent();
            RegisterHotKey(this.Handle, 42, MOD_CONTROL | MOD_SHIFT , (int)Keys.W);
            RegisterHotKey(this.Handle, 42, MOD_CONTROL | MOD_SHIFT, (int)Keys.Q);
            RegisterHotKey(this.Handle, 42, MOD_CONTROL | MOD_SHIFT, (int)Keys.C);

            txtPesquisa.Text = "Sincronizando bases ...... Por favor, aguarde!";
                Application.DoEvents();
            Auxil.Replica.Replicar.Replicacao(Auxil.Properties.Settings.Default.ProcessosRede, Auxil.Properties.Settings.Default.Processos);
            txtPesquisa.Text = "";
//            Pesquisar(" ");
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ExibeOculta(true);
        }

        private void ntiAux_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                
                if (e.Button == MouseButtons.Right)
                {
                    RetornaMenu();
                }
                oculto = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void RetornaMenu()
        {
            try
            {
                short indexMenu = -1;
                ContextMenu m = new ContextMenu();
                EventHandler evento = new EventHandler(this.trata);
                
                AcessoDados.BancoDados.AbrirSessao();
                indexMenu = AdicionaMenus(null, m, evento);
                m.MenuItems.Add(indexMenu++, new MenuItem("-"));

                m.MenuItems.Add(indexMenu, new MenuItem("Sair", evento));
                m.MenuItems[indexMenu].Tag = indexMenu;
                indexSair = indexMenu++;
                ntiAux.ContextMenu = m;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                AcessoDados.BancoDados.FecharSessao();
            }
        }

        private short AdicionaMenus(Processo p, Menu m, /*Util.IniFile iniFile, */EventHandler evento)
        {
            //string[] retINI = null;
            short ret = 0;
            short indexMenu = 0;
            IList<Processo> procs = null;
            if(p==null)
                procs = new AcessoDados.DAO.BaseDAO<Processo>().Retorna(x=>x.ProcessoPai==null);
            else
                procs = new AcessoDados.DAO.BaseDAO<Processo>().Retorna(x => x.ProcessoPai== p);

            foreach (var item in procs)
            {
                m.MenuItems.Add(indexMenu, new MenuItem(item.Nome, evento));
                m.MenuItems[indexMenu].Tag = item;
                AdicionaMenus(item, m.MenuItems[indexMenu], evento);
                indexMenu++;
            }
            return indexMenu;
        }

        private void TrataProcesso(Processo proc)
        {
            try
            {
                proc.Valor = proc.Valor.Replace(CONST_AREA_TRANSF, Clipboard.GetText());
                proc.Parametros = proc.Parametros ==null? "":proc.Parametros.Replace(CONST_AREA_TRANSF, Clipboard.GetText());
                
                //bool bC = BuscaComplementos(proc);
                
//                if (
                BuscaComplementos(proc);
//                    )
//                    return;

                if (proc.AreaTransf)
                    Clipboard.SetText(proc.Valor);

                if (proc.MostrarValor)
                    MessageBox.Show(proc.Valor, proc.Nome);

                if (proc.ExecutarValor)
                    Process.Start(proc.Valor, proc.Parametros);

                if (proc.Complemento)
                    BuscaComplementos(proc.ProcessoPai);


            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao tratar o processo: " + ex.Message);
            }
        }

        private bool BuscaComplementos(Processo proc)
        {
            try
            {
                BancoDados.AbrirSessao();
                IList<Processo> procs = new AcessoDados.DAO.BaseDAO<Processo>().Retorna(x => x.ProcessoPai == proc && x.Complemento == true);
                
                if(procs.Count > 0 && !proc.ValorPesquisavel)
                    lstItens.Items.Add(proc);

                for (int i = lstItens.Items.Count - 1; i >= 0; i--)
                {
                    if (((Processo)lstItens.Items[i]).Complemento)
                        lstItens.Items.RemoveAt(i);
                }

                foreach (Processo item in lstItens.Items)
	                 if(item.Complemento)
                         lstItens.Items.Remove(item);
	            
                foreach (var item in procs)
                        lstItens.Items.Add(item);
                
                return procs.Count > 0;
            }
            catch (Exception)
            {

                return false;
            }
            finally {
                BancoDados.FecharSessao();
            }
        }

        private void trata(object sender, EventArgs e)
        {
            try
            {
                if (((Menu)sender).Tag.ToString() == indexSair.ToString())
                    this.Close();
                else
                    TrataProcesso((Processo)(((Menu)sender).Tag));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro");
            }
        }

        private void ntiAux_MouseMove(object sender, MouseEventArgs e)
        {
            RetornaMenu();
            /*oculto = true;*/
        }


        private void frmAux_Activated(object sender, EventArgs e)
        {
            ExibeOculta(oculto);
            if (this.WindowState!= FormWindowState.Normal)
                txtPesquisa_TextChanged(null, null);
            this.WindowState = FormWindowState.Normal;
            this.Size = new Size(Properties.Settings.Default.LarguraTela, Properties.Settings.Default.AlturaTela);
            this.Location = new Point(Screen.PrimaryScreen.Bounds.Width - this.Width, Screen.PrimaryScreen.Bounds.Height - this.Height - 25);
            txtPesquisa.Focus();
            /*if (Auxil.Properties.Settings.Default.IniciaExato && string.IsNullOrEmpty(txtPesquisa.Text))
                txtPesquisa.Text = "\"";

            if (txtPesquisa.Text == "\"")
                txtPesquisa.SelectionStart = 1;
            else*/
                txtPesquisa.SelectAll();
        }
               

        private void frmAux_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Deseja realmente fechar a aplicação?", "Fechar", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                e.Cancel = !e.Cancel;
            else
            {
                UnregisterHotKey(this.Handle, 42);
                Properties.Settings.Default.Save();
            }
        }


        private void txtPesquisa_TextChanged(object sender, EventArgs e)
        {
            try
            {
                short index = -1;
                lstItens.Items.Clear();
                Util.IniFile iniFile = new Util.IniFile(Auxil.Properties.Settings.Default.Processos);
                
                Pesquisar(txtPesquisa.Text.ToString());

                lstItens.Sorted = Properties.Settings.Default.OrdenarLista;
                lstItens.Sorted = true;
                //Verifica se o texto informado é um diretório
                if (!string.IsNullOrEmpty(txtPesquisa.Text.Trim())
                    && !Path.GetInvalidPathChars().Contains(txtPesquisa.Text.Trim()[0]) && Path.IsPathRooted(txtPesquisa.Text.Trim()))
                {
                    try
                    {
                        DirectoryInfo dInfo = new System.IO.DirectoryInfo(txtPesquisa.Text.Trim());
                        if (dInfo.Exists)
                            BuscaCaminhos(dInfo);
                        else if (txtPesquisa.Text.LastIndexOf(@"\") > 1)
                        {
                            dInfo = new System.IO.DirectoryInfo(txtPesquisa.Text.Substring(0, txtPesquisa.Text.LastIndexOf(@"\") + 1));
                            if (dInfo.Exists)
                                BuscaCaminhos(dInfo, txtPesquisa.Text.Substring(txtPesquisa.Text.LastIndexOf(@"\") + 1));
                        }
                    }
                    catch (ArgumentException)
                    { }
                }
                //
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
        private void Pesquisar(string p)
        {
            IList<Processo> processos = null;
            
            Processo proc = null;
            BancoDados.AbrirSessao();
            AcessoDados.ExpressaoDinamica<Processo> eDinam = new Auxil.AcessoDados.ExpressaoDinamica<Processo>();
            Expression e2;
            Expression e1; 
            foreach (var item in p.Split(' '))
            {
                e1 = Expression.OrElse(eDinam.Contem(item.ToLower(), eDinam.Propriedade(typeof(Processo).GetProperty("Nome"))),
                    eDinam.Contem(item.ToUpper(), eDinam.Propriedade(typeof(Processo).GetProperty("Nome"))));
                e2 = Expression.AndAlso(
                    Expression.OrElse(eDinam.Contem(item.ToLower(), eDinam.Propriedade(typeof(Processo).GetProperty("Valor"))),
                    eDinam.Contem(item.ToUpper(), eDinam.Propriedade(typeof(Processo).GetProperty("Valor")))),
                    Expression.Equal(eDinam.Propriedade(typeof(Processo).GetProperty("ValorPesquisavel")), 
                        eDinam.Constante(true, typeof(Processo).GetProperty("ValorPesquisavel").PropertyType)));
                
                eDinam.AddExpressao(Expression.OrElse( e1,e2));

                
            }
            //Criar query normal e converter em objeto https://www.tutorialspoint.com/hibernate/hibernate_native_sql.htm
            processos = new AcessoDados.DAO.BaseDAO<Processo>().Retorna(eDinam.Montar());
            
            foreach (var item in processos)
            {
                if (Auxil.Properties.Settings.Default.ListaMaiusculo)
                    item.Nome = item.Nome.ToUpper();

                if (!item.Inativo && !string.IsNullOrEmpty(item.Valor) && !item.Complemento)
                    lstItens.Items.Add(item);//(!short.TryParse(p, out ret) || short.Parse(p) >= 0 ? p + "." : "") + index + " - " + retINI[0]);
                else if(item.Complemento && processos.Where(x=>x.Id == item.ProcessoPai.Id).Count()==0)
                    lstItens.Items.Add(item.ProcessoPai);
            }
            BancoDados.FecharSessao();
        }
        
        private void BuscaCaminhos(DirectoryInfo dir)
        {
            foreach (var item in dir.GetDirectories())
                lstItens.Items.Add(CONST_TIPO_PASTA + item.Name);

            foreach (var item in dir.GetFiles())
                lstItens.Items.Add(CONST_TIPO_ARQUIVO + item.Name);
        }
        private void BuscaCaminhos(DirectoryInfo dir, string contem)
        {
            foreach (var item in dir.GetDirectories().Where(x => x.Name.ToUpper().StartsWith(contem.ToUpper())))
                lstItens.Items.Add(CONST_TIPO_PASTA + item.Name);

            foreach (var item in dir.GetFiles().Where(x => x.Name.ToUpper().StartsWith(contem.ToUpper())))
                lstItens.Items.Add(CONST_TIPO_ARQUIVO + item.Name);
        }

        private void lstItens_DoubleClick(object sender, EventArgs e)
        {
            if (lstItens.SelectedIndices.Count == 1)
            {

                if (areaTransf.Contains(lstItens.SelectedItem.ToString()))
                    Clipboard.SetText(lstItens.SelectedItem.ToString());
                else
                {
                    Processo processo = null;
                    bool existe = false;
                    if (lstItens.SelectedItems[0].ToString().StartsWith(CONST_TIPO_PASTA))
                    {
                        DirectoryInfo dInfo = new DirectoryInfo(txtPesquisa.Text + "\\" + lstItens.SelectedItems[0].ToString()
                            .Substring(CONST_TIPO_PASTA.Length));
                        if (dInfo.Exists)
                            existe = true;
                        else if (txtPesquisa.Text.LastIndexOf(@"\") > 1)
                        {
                            dInfo = new System.IO.DirectoryInfo(txtPesquisa.Text.Substring(0, txtPesquisa.Text.LastIndexOf(@"\") + 1)
                                + lstItens.SelectedItems[0].ToString().Substring(CONST_TIPO_PASTA.Length));
                            if (dInfo.Exists)
                                existe = true;
                        }

                        if (existe)
                        {
                            txtPesquisa.Text = dInfo.FullName + "\\";
                            txtPesquisa.Focus();
                            txtPesquisa.SelectionStart = txtPesquisa.Text.Length;
                        }

                        return;
                    }
                    else if (lstItens.SelectedItems[0].ToString().StartsWith(CONST_TIPO_ARQUIVO))
                    {
                        FileInfo fInfo = new FileInfo(txtPesquisa.Text + "\\" + lstItens.SelectedItems[0].ToString()
                            .Substring(CONST_TIPO_ARQUIVO.Length));
                        if (fInfo.Exists)
                            existe = true;
                        else if (txtPesquisa.Text.LastIndexOf(@"\") > 1)
                        {
                            fInfo = new FileInfo(txtPesquisa.Text.Substring(0, txtPesquisa.Text.LastIndexOf(@"\") + 1)
                                + lstItens.SelectedItems[0].ToString().Substring(CONST_TIPO_ARQUIVO.Length));
                            if (fInfo.Exists)
                                existe = true;
                        }
                        if (existe)
                            processo = new Processo { Nome = fInfo.Name, Valor = fInfo.FullName, ExecutarValor=true };
                    }
                    else
                    {
                        //Util.IniFile iniFile = new Util.IniFile(Auxil.Properties.Settings.Default.Processos);
                        processo = (Processo)lstItens.SelectedItems[0];
                        if (processo.CampoPesquisa)
                        {
                            txtPesquisa.Text = processo.Valor;
                            txtPesquisa.Focus();
                            txtPesquisa.SelectionStart = txtPesquisa.Text.Length;
                            
                            if (processo.Complemento)
                                BuscaComplementos(processo.ProcessoPai);
                            else
                                BuscaComplementos(processo);

                            return;
                        }
                    }
                    TrataProcesso(processo);
                }

                this.Visible = false;
                ntiAux.Visible = true;
            }
        }

        private void frmAux_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
                ExibeOculta(false);
            else
            {
                Properties.Settings.Default.LarguraTela = this.Size.Width;
                Properties.Settings.Default.AlturaTela = this.Size.Height;
            }
        }

        /*private void frmAux_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                ExibeOculta(false);
            else if (e.KeyCode == Keys.F11)
                CarregarCentralDados();
            else if (e.KeyCode == Keys.F12)
                CarregarAreaTrasf();
            
        }*/

        private void txtPesquisa_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    if (!string.IsNullOrEmpty(txtPesquisa.Text.Trim())
                        && new DirectoryInfo(txtPesquisa.Text.Trim()).Exists)
                    {
                        TrataProcesso(new Processo { Valor = txtPesquisa.Text.Trim(), ExecutarValor = true });

                        this.Visible = false;
                        ntiAux.Visible = true;
                    }
                    break;
                case Keys.Down:
                    if (lstItens.Items.Count > 0)
                    {
                        lstItens.Focus();
                        lstItens.SelectedIndex = 0;
                    }
                    break;
                /*case Keys.Escape:
                    ExibeOculta(false);
                    break;
                case Keys.F11:
                    CarregarCentralDados();
                    break;
                case Keys.F12:
                    CarregarAreaTrasf();
                    break;*/
                default:
                    break;
            }
        }

        private void lstItens_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.OemBackslash:
                    if (lstItens.SelectedItems.Count > 0 && CONST_TIPO_PASTA.Length <= lstItens.SelectedItems[0].ToString().Length
                        && lstItens.SelectedItems[0].ToString().Substring(0, CONST_TIPO_PASTA.Length) == CONST_TIPO_PASTA)
                        lstItens_DoubleClick(lstItens, null);
                    break;
                case Keys.Enter:
                    lstItens_DoubleClick(lstItens, null);
                    break;
                case Keys.Up:
                    if (lstItens.SelectedIndex == 0)
                        txtPesquisa.Focus();
                    break;
                /*case Keys.Escape:
                    ExibeOculta(false);
                    break;
                case Keys.F11:
                    CarregarCentralDados();
                    break; 
                case Keys.F12:
                    CarregarAreaTrasf();
                    break;*/
                default:
                    break;
            }

        }
        

        #region TeclaAtalho

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_HOTKEY)
            {
                if (m.LParam.ToInt32() == WM_W)
                {
                    if (!this.Visible)
                        this.Visible = true;
                    this.Activate();
                } 
                if (m.LParam.ToInt32() == WM_Q)
                {
                    if (!this.Visible)
                        this.Visible = true;
                    this.Activate();
                    txtPesquisa.Text = Clipboard.GetText();
                }
                else if (m.LParam.ToInt32() == WM_C)
                {
                    SendCtrlC(m.LParam);
                    try
                    {
                        if (!areaTransf.Contains(Clipboard.GetText()))
                        {
                            areaTransf.Add(Clipboard.GetText());
                            if (Util.Arquivos.ArquivoExiste(Auxil.Properties.Settings.Default.CaminhoTextoAreaTransf))
                            {
                                System.IO.StreamWriter sw = new System.IO.StreamWriter(Auxil.Properties.Settings.Default.CaminhoTextoAreaTransf, true);
                                sw.WriteLine("\r\r[" + DateTime.Now.ToString() + "]\r" + Clipboard.GetText());
                                sw.Close();
                            }

                            if (areaTransf.Count() > 9)
                                areaTransf.RemoveAt(0);
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }

                }
            }
        }
        //Envia comando CRTL + C para copiar o texto selecionado
        private void SendCtrlC(IntPtr hWnd)
        {
            uint KEYEVENTF_KEYUP = 2;

            byte VK_CONTROL = 0x11;
            SetForegroundWindow(hWnd);
            keybd_event(VK_CONTROL, 0, 0, 0);

            keybd_event(0x43, 0, 0, 0); //Send the C key (43 is "C")

            keybd_event(0x43, 0, KEYEVENTF_KEYUP, 0);

            keybd_event(VK_CONTROL, 0, KEYEVENTF_KEYUP, 0);// 'Left Control Up

        }
        #endregion

        private void CarregarAreaTrasf()
        {
            lstItens.Items.Clear();
            for (int i = areaTransf.Count - 1; i >= 0; i--)
                lstItens.Items.Add(areaTransf[i]);

        }
        private void CarregarCentralDados()
        {
            if (Util.Arquivos.ArquivoExiste(Properties.Settings.Default.CentralDados))
            {
                Process[] p = Process.GetProcessesByName(Util.Arquivos.CapturarNomeArquivo( Properties.Settings.Default.CentralDados,false));
                foreach (Process item in p)
	            	 item.Kill();

                Process.Start(Properties.Settings.Default.CentralDados, txtPesquisa.Text);
            }
            else
            {
                MessageBox.Show("Aplicação " + Properties.Settings.Default.CentralDados + " não encontrada!", "Não encontrada");
            }

        }

        private void frmAux_Deactivate(object sender, EventArgs e)
        {
            ExibeOculta(false);
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Escape))
                ExibeOculta(false);
            else if (keyData == Keys.F12)
                CarregarAreaTrasf();
            else if (keyData == (Keys.Alt | Keys.C) )
                CarregarCentralDados();
            else
                return base.ProcessCmdKey(ref msg, keyData);

            return true;

        }
    }
}
