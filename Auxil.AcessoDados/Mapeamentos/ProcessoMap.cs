using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Auxil.Entidades;

namespace Auxil.AcessoDados
{
    public class ProcessoMap : ClassMap<Processo>
    {
        public ProcessoMap()
        {
            Id(x => x.Id);
            Map(x => x.Nome).Not.Nullable().UniqueKey("UK_PROCESSO01").Length(30);
            Map(x => x.Maquina).Nullable().Length(15);
            Map(x => x.Valor).Nullable().Length(6000);
            Map(x => x.Parametros).Nullable().Length(6000);
            Map(x => x.DataAlteracao).Not.Nullable();
            Map(x => x.CampoPesquisa).Not.Nullable();
            Map(x => x.AreaTransf).Not.Nullable();
            Map(x => x.CapturaRetorno).Not.Nullable();
            Map(x => x.ValorPesquisavel).Not.Nullable();
            Map(x => x.MostrarValor).Not.Nullable();
            Map(x => x.ExecutarValor).Not.Nullable();
            Map(x => x.Complemento);
            Map(x => x.Inativo).Not.Nullable();
            References(x => x.ProcessoPai).ForeignKey().UniqueKey("UK_PROCESSO01").Nullable();
            HasMany(x => x.Processos)
                .Cascade.Delete()
                .Inverse();
        }
    }    
}
