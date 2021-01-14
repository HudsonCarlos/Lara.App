using Lara.Model;
using Lara.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lara.Data.Mapping
{
    public class Pessoa_Map : IEntityTypeConfiguration<Pessoa>
    {
        public void Configure(EntityTypeBuilder<Pessoa> builder)
        {
            try
            {
                builder.Property(t => t.IdPessoa).HasColumnName("IdPessoa");
                builder.Property(t => t.Nome).HasColumnName("Nome");
                builder.Property(t => t.Apelido).HasColumnName("Apelido");
                builder.Property(t => t.Idade).HasColumnName("Idade");
                builder.Property(t => t.Sexo).HasColumnName("Sexo");
                builder.Property(t => t.EstadoCivil).HasColumnName("EstadoCivil");

                builder.ToTable("bpm_pessoa").HasKey(t => t.IdPessoa);
            }
            catch (Exception ex)
            {
                Constantes.LogLara.EnfileirarLogErro("Erro em Pessoa_Map", ex);
            }
        }
    }
}
