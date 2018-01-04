using Anahuac.CRM.EnviaOportunidadABanner.CRM;
using Anahuac.CRM.EnviaOportunidadABanner.DataLayer;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using XRM;

namespace Anahuac.CRM.EnviaOportunidadABanner
{

    public class ActualizaAsesor : IPlugin
    {
        private readonly string postImageUpdate = "UpdateImage";

        //private readonly string postCreateImageUpdate = "CreateImage";


        // ConexionSQL ConSQL;
        public void Execute(IServiceProvider serviceProvider)
        {
            ServerConnection _cnx;
            _cnx = new ServerConnection(serviceProvider);
            _cnx.context.SharedVariables.Clear();

            _cnx.trace.Trace("********Inicio de la validación del plugin JC**********");
            #region " Validaciones de Plugin "


            if (_cnx.context.MessageName != "Create" && _cnx.context.MessageName != "Update")
            {
                _cnx.context.SharedVariables.Add("AbortProcess", "Is not Update or Create");
                _cnx.trace.Trace("Is not Update or Create");
                return;
            }

            if (_cnx.context.Stage != 40)
            {
                _cnx.context.SharedVariables.Add("AbortProcess", "Invalid Stage");
                _cnx.trace.Trace("Invalid Stage");
                return;
            }

            if (_cnx.context.Depth > 4)
            {
                _cnx.context.SharedVariables.Add("AbortProcess", "Deepth has exceded");
                _cnx.trace.Trace("Deepth has exceded");
                return;
            }

            if (!_cnx.context.InputParameters.Contains("Target"))
            {
                _cnx.context.SharedVariables.Add("AbortProcess", "Do not Contains Target");
                _cnx.trace.Trace("Do not Contains Target");
                return;
            }

            if (!(_cnx.context.InputParameters["Target"] is Entity))
            {
                _cnx.context.SharedVariables.Add("AbortProcess", "Is Not an Entity");
                _cnx.trace.Trace("Is Not an Entity");
                return;
            }

            if (_cnx.context.PrimaryEntityName != "opportunity")
            {
                _cnx.context.SharedVariables.Add("AbortProcess", "Is not a opportunity");
                _cnx.trace.Trace("Is not a opportunity");
                return;
            }


            #endregion

            try
            {
                var userId = _cnx.context.UserId;
                VariablesRepository u2 = new VariablesRepository(_cnx);
                var countUsuarioServicioCRM = u2.getUsuario(userId.ToString());

             

                //if(countUsuarioServicioCRM > 0)
                //{
                //    _cnx.trace.Trace("Termina Plugnin de ejecución debido a que se detectó el usuario ServicioCRM@anahuac.mx.");
                //    return;
                //}
                _cnx.trace.Trace("Inicio Plugin.");
                IPluginExecutionContext context = _cnx.context;
                Entity currententidad = (Entity)_cnx.context.InputParameters["Target"];

                Entity postImageEntity = (context.PostEntityImages != null &&
                    context.PostEntityImages.Contains(this.postImageUpdate)) ? context.PostEntityImages[this.postImageUpdate] : null;

                //Entity postCreateImageEntity = (context.PostEntityImages != null &&
                //   context.PostEntityImages.Contains(this.postCreateImageUpdate)) ? context.PostEntityImages[this.postCreateImageUpdate] : null;

                if (postImageEntity == null)
                {
                    postImageEntity = new Entity("opportunity");
                }

                Opportunity dbRecord = postImageEntity.ToEntity<Opportunity>();
                Opportunity currentrecord = currententidad.ToEntity<Opportunity>();
                var text = dbRecord.ua_codigo_campus.Name;
                var text2 = dbRecord.ua_desc_nivel.Name;
                _cnx.trace.Trace("Id Oportunidad del Context  {0}.", currentrecord.OpportunityId.Value);

                _cnx.trace.Trace("Obtención de datos del CRM.");
                var StageId = dbRecord.StageId != null ? dbRecord.StageId : currentrecord.StageId;
                _cnx.trace.Trace("Obteniendo Origen.");
                var Origen = dbRecord.ua_origen != null ? dbRecord.ua_origen : currentrecord.ua_origen;
                _cnx.trace.Trace("Obteniendo Asesor.");
                var asesor = dbRecord.OwnerId != null ? dbRecord.OwnerId : null;
                // var statusAlumno = dbRecord.ua_estatus_alumno != null ? dbRecord.ua_estatus_alumno.Id : default(Guid);
                var titpoAlumno = dbRecord.ua_desc_tipo_alumno != null ? dbRecord.ua_desc_tipo_alumno.Id : default(Guid);
                var campusOrigen = dbRecord.ua_campus_origen != null ? dbRecord.ua_campus_origen.Id : default(Guid);
                // _cnx.trace.Trace("statusAlumno:  " + statusAlumno);
                _cnx.trace.Trace("Tipo Alumno:  " + titpoAlumno);
                _cnx.trace.Trace("Campus Origen:  " + campusOrigen);
                _cnx.trace.Trace("Asesor:  " + asesor.Name);


                //string codigostatusAlumno = "", stipoAlumno;
                string stipoAlumno;

                //if (Origen != null)
                //{
                //    _cnx.trace.Trace("Origen de Oportunidad: {0}", ((OptionSetValue)Origen).Value);
                //}
                //else
                //{
                //    _cnx.trace.Trace("No se tiene el origen del registro");
                //    //Se le asigna el valor 1, para indicar que es naicdo en CRM
                //    Origen = new OptionSetValue(1);
                //}


                _cnx.trace.Trace("Oportunidad from DB: " + dbRecord.Id);

                var idbanerImagenubdate = dbRecord.ua_idbanner != null ? dbRecord.ua_idbanner : currentrecord.ua_idbanner;
                _cnx.trace.Trace("banner de la imagen " + idbanerImagenubdate);
                var idbaner2 = currentrecord.ua_idbanner != null ? currentrecord.ua_idbanner : currentrecord.ua_idbanner;
                _cnx.trace.Trace("banner de la entidad " + idbaner2);

                //_cnx.trace.Trace("Validando IdBanner de Oportunidad");
                //_cnx.trace.Trace(" instanciando VariablesRepository");
                if (titpoAlumno != Guid.Empty && asesor != null)
                {

                    VariablesRepository u = new VariablesRepository(_cnx);
                    _cnx.trace.Trace("Obteniendo el codigo strin de  status alumno ");
                    stipoAlumno = u.GetCodigoStatusAlumno(titpoAlumno.ToString());
                    _cnx.trace.Trace("Estatus del código Alumno: " + stipoAlumno);
                    if (stipoAlumno.ToUpper() == "S")// && asesor.Id.ToString()!= "0540b4eb-bdff-e611-8106-e0071b6700e1")
                    {
                        _cnx.trace.Trace("Entrando a la condición Código Alumno == S");
                        _cnx.trace.Trace("Validando el estatus de la oportunidad.");
                        bool abierta = u.IsClosed(new Guid(currentrecord.OpportunityId.Value.ToString()));
                        _cnx.trace.Trace("Estatus de la oportunidad " + abierta);
                        if (!abierta)
                        {


                            _cnx.trace.Trace("Obteniendo el id cuenta de la oportunidad ");
                            var idcuent = u.GetIdCuentaBYOportunidad(currentrecord.OpportunityId.Value.ToString());

                            var cuent = new Account();
                            cuent.AccountId = new Guid(idcuent);
                            cuent.OwnerId = asesor;
                            _cnx.trace.Trace("Actualizando el objeto CUENTA.");
                            _cnx.service.Update(cuent);
                            _cnx.trace.Trace("se actualuzó la CUENTA.");
                        }

                        ////Actualizamos el id banner en contacto
                        //Contact contacoUpdate = new Contact();
                        ////contacoUpdate.ua_idbanner = idbanerRespues;
                        //_cnx.trace.Trace("Ejecutamos obtener el id del contacto principal de la cuenta");
                        //contacoUpdate.ContactId = u.RetriveContactoPrincipalCuenta(new Guid(currentrecord.OpportunityId.Value.ToString()));
                        //contacoUpdate.OwnerId = cuent.OwnerId;
                        //_cnx.trace.Trace("Ejecutamos la actualziacion del contacto" + contacoUpdate.ContactId.ToString());
                        //_cnx.service.Update(contacoUpdate);
                    }
                    else //Si no es tipo S= Transferencia look  que sea una priemera oportunidad
                    {
                        _cnx.trace.Trace("Entrando a la condición Código Alumno != S");
                        int numOportunidades = u.GetOportunidades(idbanerImagenubdate);
                        if (numOportunidades == 1)
                        {
                            _cnx.trace.Trace("Entrando a la condición Número de Oportunidades == 1");
                            var idcuent = u.GetIdCuentaBYOportunidad(currentrecord.OpportunityId.Value.ToString());
                            var cuent = new Account();
                            cuent.AccountId = new Guid(idcuent);
                            cuent.OwnerId = asesor;
                            _cnx.trace.Trace("Actualizando el objeto CUENTA.");
                            _cnx.service.Update(cuent);
                            _cnx.trace.Trace("se actualizó la CUENTA.");
                        }
                    }

                }

            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                _cnx.trace.Trace("The application terminated with an error.");
                _cnx.trace.Trace("Timestamp: {0}", ex.Detail.Timestamp);
                _cnx.trace.Trace("Code: {0}", ex.Detail.ErrorCode);
                _cnx.trace.Trace("Message: {0}", ex.Detail.Message);
                _cnx.trace.Trace("Inner Fault: {0}",
                    null == ex.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");
                throw new Exception("Error en FaultException<OrganizationServiceFault>.", ex.InnerException);
            }
            catch (System.TimeoutException ex)
            {
                _cnx.trace.Trace("The application terminated with an error.");
                _cnx.trace.Trace("Message: {0}", ex.Message);
                _cnx.trace.Trace("Stack Trace: {0}", ex.StackTrace);
                _cnx.trace.Trace("Inner Fault: {0}",
                    null == ex.InnerException.Message ? "No Inner Fault" : ex.InnerException.Message);
                throw new Exception("Error en TimeoutException.", ex.InnerException);
            }
            catch (System.Exception ex)
            {
                _cnx.trace.Trace("The application terminated with an error.");
                _cnx.trace.Trace(ex.Message);

                // Display the details of the inner exception.
                if (ex.InnerException != null)
                {
                    _cnx.trace.Trace(ex.InnerException.Message);

                    FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> fe = ex.InnerException
                        as FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault>;
                    if (fe != null)
                    {
                        _cnx.trace.Trace("Timestamp: {0}", fe.Detail.Timestamp);
                        _cnx.trace.Trace("Code: {0}", fe.Detail.ErrorCode);
                        _cnx.trace.Trace("Message: {0}", fe.Detail.Message);
                        _cnx.trace.Trace("Trace: {0}", fe.Detail.TraceText);
                        _cnx.trace.Trace("Inner Fault: {0}",
                            null == fe.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");
                    }
                }
                throw new Exception("Error en Exception.", ex.InnerException);
            }
        }
    }
}