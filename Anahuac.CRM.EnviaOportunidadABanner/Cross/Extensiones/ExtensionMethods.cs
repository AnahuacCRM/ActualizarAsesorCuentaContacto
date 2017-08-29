using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anahuac.CRM.EnviaOportunidadABanner.Cross.Extensiones
{
    public static class ExtensionMethods
    {
        public static void Validate(this BusinessTypeBase target)
        {
            if (target == null)
            {
                throw new BusinessTypeException("No se proporciono una entidad para validarse");
            }
            var context = new ValidationContext(target);
            var results = new List<ValidationResult>();
            if (Validator.TryValidateObject(target, context, results, true) == false)
            {
                var errors = results.Select(r => r.ErrorMessage);
                var mensajeerror = "";
                foreach (var e in errors)
                {
                    mensajeerror += string.Format("{0} \n", e);

                }
                throw new BusinessTypeException(mensajeerror);
            }



        }
    }
}
