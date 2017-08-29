using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anahuac.CRM.EnviaOportunidadABanner.Cross
{
   public class CrearCuentaBanner: BusinessTypeBase
    {

        [Required(ErrorMessage = "El atributo {0} es requerido.")]
        [MaxLength(36, ErrorMessage = "La longitud maxima para el atributo {0} es de {1} caracteres")]
        public string id_Cta { get; set; }

        [Required(ErrorMessage = "El atributo {0} es requerido.")]
        [MaxLength(36, ErrorMessage = "La longitud maxima para el atributo {0} es de {1} caracteres")]
        public string id_Oportunidad { get; set; }

        [MaxLength(60, ErrorMessage = "La longitud maxima para el atributo {0} es de {1} caracteres")]
        [Required(ErrorMessage = "El atributo {0} es requerido.")]
        public string Nombre { get; set; }

        public string Segundo_Nombre { get; set; }

        [Required(ErrorMessage = "El atributo {0} es requerido.")]
        public string ApellidoPaterno { get; set; }

        public string Apellido_Materno { get; set; }

        public FechaNacimiento Fecha_Nacimiento { get; set; }

        public Correo Correo { get; set; }

        [Required(ErrorMessage = "El atributo {0} es requerido.")]
        public string Campus { get; set; }

        [Required(ErrorMessage = "El atributo {0} es requerido.")]
        public string Sexo { get; set; }

        public Telefono Telefono { get; set; }

       

        [Required(ErrorMessage = "El atributo {0} es requerido.")]
        public string Periodo { get; set; }

        [Required(ErrorMessage = "El atributo {0} es requerido.")]
        public string VPD { get; set; }

        [Required(ErrorMessage = "El atributo {0} es requerido.")]
        public string Programa { get; set; }

    }

    public class Correo
    {
        [MaxLength(128, ErrorMessage = "La longitud maxima para el atributo {0} es de {1} caracteres")]
        public string Correo_Electronico { get; set; }
    }

    public class Telefono
    {
        [MaxLength(6, ErrorMessage = "La longitud maxima para el atributo {0} es de {1} caracteres")]
        public string Codigo_Area { get; set; }

        [MaxLength(12, ErrorMessage = "La longitud maxima para el atributo {0} es de {1} caracteres")]
        public string Numero_Telefono { get; set; }

    }
}
