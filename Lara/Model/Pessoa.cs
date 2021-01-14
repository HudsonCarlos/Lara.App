using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Lara.Model
{
    public class Pessoa
    {
        public int IdPessoa { get; set; }
        public string Nome { get; set; }
        public string Apelido { get; set; }
        public int Idade { get; set; }
        public EnumEstadoCivil EstadoCivil { get; set; }
        public EnumSexo Sexo { get; set; }
    }
}
