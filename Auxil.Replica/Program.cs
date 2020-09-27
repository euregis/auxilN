using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Auxil.Replica
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            /*Application.Run(new Form1());*/
            Replicar.Replicacao(args[0], args[1]);
            MessageBox.Show("Sincronização de bases concluída!", "Sincronização", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
