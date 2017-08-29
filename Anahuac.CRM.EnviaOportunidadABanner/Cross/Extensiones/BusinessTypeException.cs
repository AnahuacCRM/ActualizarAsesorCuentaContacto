using System;
using System.Runtime.Serialization;

namespace Anahuac.CRM.EnviaOportunidadABanner.Cross.Extensiones
{
    [Serializable]
    internal class BusinessTypeException : Exception
    {
        public BusinessTypeException()
        {
        }

        public BusinessTypeException(string message) : base(message)
        {
        }

        public BusinessTypeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BusinessTypeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}