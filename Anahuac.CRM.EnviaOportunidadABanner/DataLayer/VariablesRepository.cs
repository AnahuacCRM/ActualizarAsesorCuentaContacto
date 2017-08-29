using Anahuac.CRM.EnviaOportunidadABanner.CRM;
using Anahuac.CRM.EnviaOportunidadABanner.Cross;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Rhino.RetrieveBearerToken;
using Rhino.RetrieveBearerToken.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using XRM;

namespace Anahuac.CRM.EnviaOportunidadABanner.DataLayer
{
    public class VariablesRepository : IVariablesRepository
    {
        private readonly IServerConnection _cnx;

        public VariablesRepository(IServerConnection cnx)
        {
            _cnx = cnx;
        }

        public bool IsOpen(Guid OpportunityId)
        {
            var op = (Opportunity)_cnx.service.Retrieve(Opportunity.EntityLogicalName, OpportunityId, new ColumnSet(new string[] { "statuscode", "statecode" }));
            return ((OptionSetValue)op.StatusCode).Value == 0;
        }

        public bool IsClosed(Guid OpportunityId)
        {
            var op = (Opportunity)_cnx.service.Retrieve(Opportunity.EntityLogicalName, OpportunityId, new ColumnSet(new string[] { "statuscode", "statecode" }));
            return ((OptionSetValue)op.StatusCode).Value == 4;
        }

        public string GetIdCuenta(string idBanner)
        {
            string resultado = "";
            if (string.IsNullOrWhiteSpace(idBanner))
                return "";
            QueryExpression Query = new QueryExpression("account")
            {
                NoLock = true,
                ColumnSet = new ColumnSet(new string[] { "accountid" }),
                Criteria = {
                    Conditions = {
                        new ConditionExpression("ua_idbanner", ConditionOperator.Equal, idBanner)
                    }
                }
            };

            var ec = _cnx.service.RetrieveMultiple(Query);
            if (ec.Entities.Any())
            {
                var prospecto = ec.Entities.FirstOrDefault();
                resultado = prospecto.Attributes["accountid"].ToString();

            }
            return resultado;

        }

        public string GetIdCuentaBYOportunidad(string Oportunidad)
        {

            string resultado = "";
            Guid Oport = new Guid(Oportunidad);

            Opportunity op = new Opportunity();

            QueryExpression Query = new QueryExpression(Opportunity.EntityLogicalName)
            {
                NoLock = true,
                ColumnSet = new ColumnSet(new string[] { "parentaccountid" }),
                Criteria = {
                    Conditions = {
                        new ConditionExpression("opportunityid", ConditionOperator.Equal, Oport)
                    }
                }
            };

            var ec = _cnx.service.RetrieveMultiple(Query);
            if (ec.Entities.Any())
            {
                var prospecto = ec.Entities.FirstOrDefault();
                // resultado = prospecto.Attributes["accountid"].ToString();
                if (prospecto.Attributes.ContainsKey("parentaccountid"))
                    resultado = ((EntityReference)prospecto.Attributes["parentaccountid"]).Id.ToString();

            }
            return resultado;

        }


        public Guid ObtenerContactoPrincipalCuenta(Guid pCuenta)
        {
            Guid resultado = default(Guid);

            //Cuenta Existe este PrimaryContactId


            QueryExpression Query = new QueryExpression(Account.EntityLogicalName)
            {
                NoLock = true,
                //ColumnSet = new ColumnSet(new string[] { "accountid", "primarycontactid" }),
                ColumnSet = new ColumnSet(new string[] { "primaryContactid" }),
                Criteria = {
                        Conditions = {

                            new ConditionExpression("accountid", ConditionOperator.Equal, pCuenta),

                        }
                    }
            };
            var ec = _cnx.service.RetrieveMultiple(Query);
            if (ec.Entities.Any())
            {
                var Contactoprim = ec.Entities.FirstOrDefault();

                //resultado = new Guid(Contactoprim.Attributes["primarycontactid"].ToString());
                resultado = ((EntityReference)Contactoprim.Attributes["primarycontactid"]).Id;

            }






            return resultado;
        }

        public Guid RetriveContactoPrincipalCuenta(Guid pCuenta)
        {
            Guid resultado = default(Guid);

            //Cuenta Existe este PrimaryContactId
            Account c = new Account();

            QueryExpression Query = new QueryExpression(Account.EntityLogicalName)
            {
                NoLock = true,
                //ColumnSet = new ColumnSet(new string[] { "accountid", "primarycontactid" }),
                ColumnSet = new ColumnSet(new string[] { "primarycontactid" }),
                Criteria = {
                        Conditions = {

                            new ConditionExpression("accountid", ConditionOperator.Equal, pCuenta),

                        }
                    }
            };
            var ec = _cnx.service.RetrieveMultiple(Query);
            if (ec.Entities.Any())
            {
                var Contactoprim = ec.Entities.FirstOrDefault();

                //resultado = new Guid(Contactoprim.Attributes["primarycontactid"].ToString());
                if (Contactoprim.Attributes.Contains("primarycontactid"))
                    resultado = ((EntityReference)Contactoprim.Attributes["primarycontactid"]).Id;

            }
            else
                throw new InvalidPluginExecutionException("La cuenta " + pCuenta + " no existe");

            //consultar cuenta


            return resultado;
        }

        public Guid ObtenerProspectoDeLaOportunidad(Guid idOportunity)
        {
            Guid resultado = default(Guid);

            //Cuenta Existe este PrimaryContactId
            Opportunity op = new Opportunity();


            QueryExpression Query = new QueryExpression(Opportunity.EntityLogicalName)
            {
                NoLock = true,
                //ColumnSet = new ColumnSet(new string[] { "accountid", "primarycontactid" }),
                ColumnSet = new ColumnSet(new string[] { "originatingleadid" }),
                Criteria = {
                        Conditions = {

                            new ConditionExpression("opportunityid", ConditionOperator.Equal, idOportunity),

                        }
                    }
            };
            var ec = _cnx.service.RetrieveMultiple(Query);
            if (ec.Entities.Any())
            {
                var Contactoprim = ec.Entities.FirstOrDefault();

                //resultado = new Guid(Contactoprim.Attributes["primarycontactid"].ToString());
                resultado = ((EntityReference)Contactoprim.Attributes["originatingleadid"]).Id;

            }






            return resultado;
        }

        public string GetCodigoStatusAlumno(string idtipo)
        {
            if (string.IsNullOrWhiteSpace(idtipo))
                return "";
            Guid idstatusGuid = new Guid(idtipo);
            string resultado = "";
           
            QueryExpression Query = new QueryExpression(ua_tipoalumno.EntityLogicalName)
            {
                NoLock = true,
                ColumnSet = new ColumnSet(new string[] { ua_tipoalumno.Fields.ua_codigo_tipo_alumno}),
                Criteria = {
                    Conditions = {
                        new ConditionExpression(ua_tipoalumno.Fields.ua_tipoalumnoId, ConditionOperator.Equal, idtipo)
                    }
                }
            };

            var ec = _cnx.service.RetrieveMultiple(Query);
            if (ec.Entities.Any())
            {
                var prospecto = ec.Entities.FirstOrDefault();
                resultado = prospecto.Attributes[ua_tipoalumno.Fields.ua_codigo_tipo_alumno].ToString();

            }
            return resultado;

        }

        public int GetOportunidades(string idbanner)
        {
            int cont = 0;
            Opportunity op = new Opportunity();

            QueryExpression QueryOportEx = new QueryExpression(Opportunity.EntityLogicalName)
            {

                NoLock = false,
                ColumnSet = new ColumnSet(new string[] { "ua_idbanner", "name" }),
                //ColumnSet = new ColumnSet { AllColumns = true },
                Criteria = {
                    Conditions = {
                        new ConditionExpression("ua_idbanner", ConditionOperator.Equal,  idbanner),

                    }
                }
            };
            var ListOportRel = _cnx.service.RetrieveMultiple(QueryOportEx);

            if (ListOportRel != null)
            {
                cont = ListOportRel.Entities.Count;
            }

            return cont;


        }

        public string ObtenerVariableSistema(string EntityLogicalName, string Variable)
        {


            string resultado = string.Empty;

            QueryExpression query = new QueryExpression()
            {
                NoLock = true,
                EntityName = EntityLogicalName,
                ColumnSet = new ColumnSet(ua_variablesistema.Fields.ua_Valortexto),
                Criteria =
                {
                    Conditions = {
                         new ConditionExpression(ua_variablesistema.Fields.ua_name, ConditionOperator.Equal, Variable)
                    }
                },
            };

            EntityCollection ec = _cnx.service.RetrieveMultiple(query);

            if (ec.Entities.Any())
                resultado = ec.Entities[0].GetAttributeValue<string>("ua_valortexto");

            return resultado;
        }



    }
}
