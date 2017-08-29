using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anahuac.CRM.EnviaOportunidadABanner.Cross
{
    public class FechaNacimiento : BusinessTypeBase
    {

        [Range(1900, 2199, ErrorMessage = "El año proporcionado es incorrecto")]
        public int Year { get; set; }
        [Range(1, 12, ErrorMessage = "El mes proporcionado es incorrecto")]
        public int Month { get; set; }
        [Range(1, 31, ErrorMessage = "El día proporcionado es incorrecto")]
        public int Day { get; set; }
    }
}
