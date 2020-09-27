using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using System.Reflection;

namespace Auxil.AcessoDados
{
    public enum TipoConexao{SQLite, SQLServer}
    public sealed class BancoDados
    {   
        //public static AcessoDados.Interfaces.IAcessoFactory DAO;
        public static Conexao.FluentNHibernate.Conexao Cn; //= new Conexao.FluentNHibernate.Conexao(Conexao.FluentNHibernate.TipoConexao.SQLite,;
        public static string CaminhoBase = "";
        public static List<Assembly> Mapeamentos = new List<Assembly>();
        public static void Config(TipoConexao tpConexao, string[] param)
        {
            Conexao.FluentNHibernate.TipoConexao tpCn;
            if (tpConexao == TipoConexao.SQLite)
            {
                tpCn = Conexao.FluentNHibernate.TipoConexao.SQLite;
                //TrataDAO = new AcessoDados.AcessoFactory().CriaDAO(TipoDAO.NHibernate);
            }
            else if (tpConexao == TipoConexao.SQLServer)
            {
                tpCn = Conexao.FluentNHibernate.TipoConexao.SQLServer;
                //TrataDAO = new AcessoDados.AcessoFactory().CriaDAO(TipoDAO.SQLServer);
            }
            else
                tpCn = Conexao.FluentNHibernate.TipoConexao.Indefinido;

            Mapeamentos.Add(typeof(ProcessoMap).Assembly);
            //Mapeamentos.AddRange(Assistente.Negocio.Host.RetMapeamentos());
            Cn = new Conexao.FluentNHibernate.Conexao(tpCn, param, Mapeamentos);
            //DAO = new NHLinqFactory(ref Cn);
        }

        public static bool AbrirSessao()
        {
            try{ Cn.AbrirSessao(); }
            catch (Exception) { return false; }
            return true;
        }

        public static bool FecharSessao()
        {
            try { Cn.FecharSessao(); }
            catch (Exception) { return false; }
            return true;
        }
        public static bool ValidaSessao()
        {
            if (Cn == null)
                throw new Exception("A conexão é inválida ou não definida.");

            if (Cn.Sessao == null)
                throw new Exception("A sessão é inválida ou não definida.");
            return true;
        }
        public static bool ValidaCaminhoBase(string p)
        {
            if (!Util.Arquivos.ArquivoExiste(p))
                return false;
            else
                return true;
        }
        /*public static bool CriarBase(string vstrCaminhoBase)
        {
            try
            {
                CaminhoBase = vstrCaminhoBase;
                Geral.Config.CaminhoBase = CaminhoBase;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }*/
        /*public static bool ValidaCaminhoBase(string vstrCaminhoXML)
        {
            DataSet ds = new DataSet();
            ds.ReadXml(vstrCaminhoXML);
            if (string.IsNullOrEmpty(ds.Tables[0].Rows[0].ItemArray.GetValue(0).ToString())
                || !Util.Arquivos.ArquivoExiste(ds.Tables[0].Rows[0].ItemArray.GetValue(0).ToString() + "AssistenteDB.db3"))
                return false;
            else
                BancoDados.CaminhoBase = ds.Tables[0].Rows[0].ItemArray.GetValue(0).ToString();

            return true;
        }*/

        /*public static bool CriarBase(string vstrCaminhoBase, string vstrCaminhoAplic)
        {
            try
            {
                CaminhoBase = vstrCaminhoBase;
                XmlDocument doc = new XmlDocument();
                string caminho = vstrCaminhoAplic + "config.xml";
                doc.Load(caminho);
                XmlNode no = doc.SelectSingleNode("/config");
                no.SelectSingleNode("./caminhoBase").InnerText = CaminhoBase;
                doc.Save(caminho);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }*/
       
    }
}
