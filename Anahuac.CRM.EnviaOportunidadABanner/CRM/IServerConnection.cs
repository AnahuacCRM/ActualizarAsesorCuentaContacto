using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anahuac.CRM.EnviaOportunidadABanner.CRM
{
    public interface IServerConnection
    {
        IPluginExecutionContext context { get; set; }
        IOrganizationServiceFactory factory { get; set; }
        IOrganizationService service { get; set; }
        ITracingService trace { get; set; }
        //OrganizationServiceContext xrmContext { get; set; }
    }
}
