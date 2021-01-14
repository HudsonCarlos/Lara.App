using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lara_MP.Model
{
    public class Enum
    {


    }

    public enum EnumEstadoCivil
    {
        [Display(Name = "Solteiro")]
        Solteiro = 0,
        [Display(Name = "Casado")]
        Casado = 1,
        [Display(Name = "Viúvo")]
        Viuvo = 2,
        [Display(Name = "Separado judicialmente")]
        SeparadoJudicialmente = 3,
        [Display(Name = "Divorciado")]
        Divorciado = 4,
    }

    public enum EnumSexo
    {
        [Display(Name = "Masculino")]
        Masculino = 0,
        [Display(Name = "Feminino")]
        Feminino = 1,
    }
}
