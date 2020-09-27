using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Auxil.Entidades;

namespace Auxil.AcessoDados
{
   /* public class ParametroMap : ClassMap<Parametro>
    {
        public ParametroMap()
        {
            Id(x => x.Id);
            Map(x => x.Valor).Not.Nullable().Unique().Length(4000);
            Map(x => x.Pesquisavel).Not.Nullable();
            References(x => x.Processo).ForeignKey();
        }
    }*/
}
