using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Auxil.Entidades
{
    public class Processo : Base.BaseID
    {
        public virtual string Nome { get; set; }
        public virtual string Valor { get; set; }
        public virtual string Parametros { get; set; }
        public virtual string Maquina { get; set; }
        public virtual DateTime DataAlteracao { get; set; }
        public virtual bool CampoPesquisa { get; set; }
        public virtual bool AreaTransf { get; set; }
        public virtual bool CapturaRetorno { get; set; }
        public virtual bool ValorPesquisavel { get; set; }
        public virtual bool MostrarValor { get; set; }
        public virtual bool ExecutarValor { get; set; }
        public virtual bool Complemento { get; set; }
        public virtual bool Inativo { get; set; }
        public virtual Processo ProcessoPai { get; set; }

        public virtual IList<Processo> Processos { get; set; }

        public Processo()
        { 
            Processos = new List<Processo>(); 
        }

        public override string ToString()
        {
            return Nome;
        }

    }
}
