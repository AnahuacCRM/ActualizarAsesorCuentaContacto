using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anahuac.CRM.EnviaOportunidadABanner.Cross
{
  public  class ContactoBasico: BusinessTypeBase
    {
        public string NombreC { get; set; }

        public string Segundo_NombreC { get; set; }

        public string ApellidoPaternoC { get; set; }

        public string Apellido_MaternoC { get; set; }


        public Correo CorreoC { get; set; }

        public Telefono TelefonoC { get; set; }
    }
}
