using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using Conexao.FluentNHibernate;
using Auxil.AcessoDados;

namespace Auxil
{
    static class Program
    {
        private static Mutex mut = new Mutex(); // make sure that one instance is running at the time
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            /*BaseDAO<Anotacao> anotDAO = new BaseDAO<Anotacao>();

            Util.IniFile iniFile = new Util.IniFile(@"C:\Anot.ini");

            //MessageBox.Show("-" + iniFile.IniReadValue("Averbador", "Linguagens")+"-");

            iniFile.IniWriteValue("A", "B", "C");
            iniFile.IniWriteValue("A", "B", "D");
            iniFile.IniWriteValue("A", "X", "Y");
            IList<Anotacao> dt = anotDAO.Retorna();

            Anotacao a = new Anotacao();
            a.Descricao = "teste2";
            //anotDAO.Salvar(a);
            */
            BancoDados.Config(Auxil.AcessoDados.TipoConexao.SQLite, new string[] { Auxil.Properties.Settings.Default.Processos });
            try
            {
                Application.Run(new frmAux());
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.Message);
            }
            
        }
    }
}
