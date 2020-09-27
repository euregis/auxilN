using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Auxil.AcessoDados;
using Auxil.Entidades;

namespace Auxil.Replica
{
    public static class Replicar
    {
        private static IList<Processo> procsOrigem = null;
        
        private static AcessoDados.DAO.BaseDAO<Processo> procDAO = null;

        public static void Replicacao(String origem, String destino)
        {
            IList<Processo> procsDestino = new List<Processo>();
            
            if (!Util.Arquivos.ArquivoExiste(origem) || !Util.Arquivos.ArquivoExiste(destino))
                return;

            BancoDados.Config(Auxil.AcessoDados.TipoConexao.SQLite, new string[] { origem });
            BancoDados.AbrirSessao();
            procDAO = new AcessoDados.DAO.BaseDAO<Processo>();
            procsOrigem = procDAO.Retorna(x => x.ProcessoPai == null && !x.Inativo).OrderBy(x => x.Nome).ToList<Processo>();
            Processo proc = null;
            foreach (var item in procsOrigem)
            {
                proc = new Processo();
                proc.Nome = item.Nome;
                proc.Maquina = "";
                proc.Valor = item.Valor;
                proc.Parametros = item.Parametros;
                proc.DataAlteracao = item.DataAlteracao;
                proc.CampoPesquisa = item.CampoPesquisa;
                proc.AreaTransf = item.AreaTransf;
                proc.CapturaRetorno = item.CapturaRetorno;
                proc.ValorPesquisavel = item.ValorPesquisavel;
                proc.MostrarValor = item.MostrarValor;
                proc.ExecutarValor = item.ExecutarValor;
                proc.Complemento = item.Complemento;
                proc.Inativo = false;
                //procDAO.Salvar(proc);

                ReplicaItem(item.Processos, proc);
                procsDestino.Add(proc);
            }
            
            BancoDados.FecharSessao();

            BancoDados.Config(Auxil.AcessoDados.TipoConexao.SQLite, new string[] { destino });
            BancoDados.AbrirSessao();
            new AcessoDados.DAO.ProcessoDAO().ExcluiReplicados(null);

            procDAO = new AcessoDados.DAO.BaseDAO<Processo>();
            
            /*IList<Processo> procsDestinoBase = procDAO.Retorna(x => x.Maquina == "" && x.ProcessoPai == null);
            foreach (var item in procsDestinoBase)
            {
                ExcluirFilhos(item.Processos);
                procDAO.Excluir(item);
            }*/

            foreach (var item in procsDestino)
            {
                procDAO.Salvar(item);
                SalvarFilhos(item.Processos);
            }

            
            BancoDados.FecharSessao();
        }
        private static void ReplicaItem(IList<Processo> origem, Processo procPai)
        {
            Processo proc = null;
            foreach (var item in origem)
            {
                proc = new Processo();
                proc.Nome = item.Nome;
                proc.Maquina = "";
                proc.Valor = item.Valor;
                proc.Parametros = item.Parametros;
                proc.DataAlteracao = item.DataAlteracao;
                proc.CampoPesquisa = item.CampoPesquisa;
                proc.AreaTransf = item.AreaTransf;
                proc.CapturaRetorno = item.CapturaRetorno;
                proc.ValorPesquisavel = item.ValorPesquisavel;
                proc.MostrarValor = item.MostrarValor;
                proc.ExecutarValor = item.ExecutarValor;
                proc.Complemento = item.Complemento;
                proc.Inativo = false;
                proc.ProcessoPai = procPai;
                procPai.Processos.Add(proc);
                //procDAO.Salvar(proc);
                if (item.Processos.Count() > 0)
                    ReplicaItem(item.Processos, proc);
            }
        }
        private static void SalvarFilhos(IList<Processo> filhos)
        {
            foreach (var item in filhos)
            {
                procDAO.Salvar(item);
                if (item.Processos.Count() > 0)
                    SalvarFilhos(item.Processos);
                
            }
        }

        private static void ExcluirFilhos(IList<Processo> filhos)
        {
            for (int i = filhos.Count()-1; i >=0 ; i--)
            {
                if (filhos[i].Processos.Count() > 0)
                    ExcluirFilhos(filhos[i].Processos);
                procDAO.Excluir(filhos[i]);
                filhos.Remove(filhos[i]);
            }
        }
    }
}
