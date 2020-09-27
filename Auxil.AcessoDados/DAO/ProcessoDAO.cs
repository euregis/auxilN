using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Auxil.Entidades;
using NHibernate;

namespace Auxil.AcessoDados.DAO
{
    public class ProcessoDAO:BaseDAO<Processo>
    {
        public bool ExcluiReplicados(ITransaction objTransaction)
        {
            if (!BancoDados.ValidaSessao()) return false;

            if (objTransaction != null)
            {
                if (!objTransaction.IsActive) { objTransaction = BancoDados.Cn.Sessao.BeginTransaction(); }
            }
            else { objTransaction = BancoDados.Cn.Sessao.BeginTransaction(); }

            try
            {
                BancoDados.Cn.Sessao.Delete("FROM Processo WHERE MAQUINA = '';");
                objTransaction.Commit();
                BancoDados.Cn.Sessao.Flush();
                return true;
            }
            catch (Exception ex)
            {
                objTransaction.Rollback();
                BancoDados.Cn.Sessao.Close();
                BancoDados.Cn.AbrirSessao();
                throw new Exception("Náo foi possivel inluir este registro.\n\n" + ex.Message);
            }
        }
    }
}
