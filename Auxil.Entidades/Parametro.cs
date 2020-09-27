using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Auxil.Entidades
{
    public class Parametro: Base.BaseID
    {
        public virtual Processo Processo{get;set;}
        public virtual string Valor { get; set; }
        public virtual bool Pesquisavel { get; set; }

    }
}
