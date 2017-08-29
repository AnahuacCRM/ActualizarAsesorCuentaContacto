using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anahuac.CRM.EnviaOportunidadABanner.CRM
{
    public class ServerConnection : IServerConnection, IDisposable
    {
        public IPluginExecutionContext context { get; set; }
        public IOrganizationServiceFactory factory { get; set; }
        public IOrganizationService service { get; set; }
        public ITracingService trace { get; set; }
        //public OrganizationServiceContext xrmContext { get; set; }


        bool disposed = false;

        public ServerConnection(IServiceProvider serviceProvider)
        {
            context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            service = factory.CreateOrganizationService(context.UserId);
            trace = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            //xrmContext = new OrganizationServiceContext(service);
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                context = null;
                service = null;
                trace = null;
                factory = null;
            }
            disposed = true;
        }
    }
}
