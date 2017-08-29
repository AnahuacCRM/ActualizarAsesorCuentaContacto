using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anahuac.CRM.EnviaOportunidadABanner.Cross
{
    public class AlmacenaIdOportunidad : BusinessTypeBase
    {
        //[Required(ErrorMessage = "El atributo {0} es requerido.")]
        [MaxLength(36, ErrorMessage = "La longitud maxima para el atributo {0} es de {1} caracteres")]
        public string Id_Oportunidad { get; set; }

        [Required(ErrorMessage = "El atributo {0} es requerido.")]
        [MaxLength(36, ErrorMessage = "La longitud maxima para el atributo {0} es de {1} caracteres")]
        public string Id_Cuenta { get; set; }

        [Required(ErrorMessage = "El atributo {0} es requerido.")]
        [MaxLength(9, ErrorMessage = "La longitud maxima para el atributo {0} es de {1} caracteres")]
        public string Id_Banner { get; set; }

        [Required(ErrorMessage = "El atributo {0} es requerido.")]
        [MaxLength(6, ErrorMessage = "La longitud maxima para el atributo {0} es de {1} caracteres")]
        public string Periodo { get; set; }

        [Required(ErrorMessage = "El atributo {0} es requerido.")]
        [MaxLength(4, ErrorMessage = "La longitud maxima para el atributo {0} es de {1} caracteres")]
        public string VPD { get; set; }

        [Required(ErrorMessage = "El atributo {0} es requerido.")]
        [MaxLength(12, ErrorMessage = "La longitud maxima para el atributo {0} es de {1} caracteres")]
        public string Programa { get; set; }

        public int Numero_Solicitud { get; set; }

        //public string pStatus_solicitud_Admision { get; set; }

        //[Required(ErrorMessage = "El atributo {0} es requerido.")]
        //public string pNivel { get; set; }

        //[Required(ErrorMessage = "El atributo {0} es requerido.")]
        //public string pEscuela { get; set; }

        //[Required(ErrorMessage = "El atributo {0} es requerido.")]
        //public string pTipo_Alumno { get; set; }

        //[Required(ErrorMessage = "El atributo {0} es requerido.")]
        //public string pTipo_Admision { get; set; }



    }
}
