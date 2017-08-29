using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.Xrm.Sdk;
using Anahuac.CRM.EnviaOportunidadABanner;
using Anahuac.CRM.EnviaOportunidadABanner.Cross;

namespace UnitTestEnvioOportunidades
{
    [TestClass]
    public class UnitTestEnvioOportunidadesaBaner
    {
        ActualizaAsesor penvio = default(ActualizaAsesor);
        Mock<IServiceProvider> mockServiceProvider = default(Mock<IServiceProvider>);
        Mock<IPluginExecutionContext> mockPluginExecutionContext = default(Mock<IPluginExecutionContext>);
        Mock<IOrganizationServiceFactory> mockOrganizationServiceFactory = default(Mock<IOrganizationServiceFactory>);
        Mock<ITracingService> mockTracingService = default(Mock<ITracingService>);
        Mock<IOrganizationService> mockOrganizationService = default(Mock<IOrganizationService>);
        Entity oportunidad = default(Entity);
        Entity ImagenOportunidad = default(Entity);

        [TestInitialize]
        public void Initialize()
        {
            penvio = new ActualizaAsesor();
            mockServiceProvider = new Mock<IServiceProvider>();
            mockPluginExecutionContext = new Mock<IPluginExecutionContext>();
            mockOrganizationServiceFactory = new Mock<IOrganizationServiceFactory>();
            mockTracingService = new Mock<ITracingService>();
            mockOrganizationService = new Mock<IOrganizationService>();

            //Inicializar Contexto
            mockServiceProvider.Setup(sp => sp.GetService(typeof(IPluginExecutionContext)))
                .Returns(mockPluginExecutionContext.Object);

            mockServiceProvider.Setup(sp => sp.GetService(typeof(IOrganizationServiceFactory)))
                .Returns(mockOrganizationServiceFactory.Object);

            mockServiceProvider.Setup(sp => sp.GetService(typeof(ITracingService)))
                .Returns(mockTracingService.Object);

            mockOrganizationServiceFactory.Setup(osf => osf.CreateOrganizationService(It.IsAny<Guid>()))
                .Returns(mockOrganizationService.Object);

            //Informacion del Contexto del Plugin
            mockPluginExecutionContext.Setup(pec => pec.MessageName).Returns("Update");
            mockPluginExecutionContext.Setup(pec => pec.Stage).Returns(40);
            mockPluginExecutionContext.Setup(pec => pec.Depth).Returns(1);
            mockPluginExecutionContext.Setup(pec => pec.PrimaryEntityName).Returns("opportunity");


            var OppId = Guid.NewGuid();

            oportunidad = new Entity("opportunity");
            oportunidad.Attributes["opportunityid"] = OppId;
            oportunidad.Attributes["rs_banderaenviooportunidad"] = new OptionSetValue(0);
            oportunidad.Attributes["rs_idbanner"] = "12345";
            oportunidad.Attributes["rs_periodoid"] = new EntityReference("rs_periodo", Guid.NewGuid());
            oportunidad.Attributes["rs_programaid"] = new EntityReference("rs_programaid", Guid.NewGuid());
            oportunidad.Attributes["rs_vpdid"] = new EntityReference("rs_vpdid", Guid.NewGuid());


            //Establecer parametros de default a la entidad del plugin

            mockPluginExecutionContext.Setup(pec => pec.InputParameters)
                .Returns(new ParameterCollection { { "Target", oportunidad } });

            mockPluginExecutionContext.Setup(pec => pec.SharedVariables)
                .Returns(new ParameterCollection());

            //Se agrega la imagen
            ImagenOportunidad = new Entity("opportunity");
            ImagenOportunidad.Attributes["opportunityid"] = OppId;

            ImagenOportunidad.Attributes["rs_periodoid"] = new EntityReference("rs_periodo", Guid.NewGuid());
            ImagenOportunidad.Attributes["rs_programaid"] = new EntityReference("rs_programaid", Guid.NewGuid());
            ImagenOportunidad.Attributes["rs_vpdid"] = new EntityReference("rs_vpdid", Guid.NewGuid());


            var eic = new EntityImageCollection();
            eic.Add("UpdateImage", ImagenOportunidad);
            mockPluginExecutionContext.Setup(pec => pec.PostEntityImages)
                .Returns(eic);

        }


        #region Validaciones de default del plugin
        [TestMethod]
        public void InvalidMessageTest()
        {
            //Arrange
            mockPluginExecutionContext.Setup(pec => pec.MessageName).Returns("Delete");

            //Act
            penvio.Execute(mockServiceProvider.Object);

            //Assert
            var Expected = "Is not Update or Create";
            var Actual = default(string);
            if (mockPluginExecutionContext.Object.SharedVariables.Contains("AbortProcess"))
            {
                Actual = (string)mockPluginExecutionContext.Object.SharedVariables["AbortProcess"];
            }
            Assert.AreEqual(Expected, Actual);

        }

        [TestMethod]
        public void DeepthExcededTest()
        {
            //Arrange
            mockPluginExecutionContext.Setup(pec => pec.Depth).Returns(5);

            //Act
            penvio.Execute(mockServiceProvider.Object);

            //Assert
            var Expected = "Deepth has exceded";
            var Actual = default(string);
            if (mockPluginExecutionContext.Object.SharedVariables.Contains("AbortProcess"))
            {
                Actual = (string)mockPluginExecutionContext.Object.SharedVariables["AbortProcess"];
            }
            Assert.AreEqual(Expected, Actual);
        }

        [TestMethod]
        public void InvalidStageTest()
        {
            //Arrange
            mockPluginExecutionContext.Setup(pec => pec.Stage).Returns(10);

            //Act
            penvio.Execute(mockServiceProvider.Object);

            //Assert
            var Expected = "Invalid Stage";
            var Actual = default(string);
            if (mockPluginExecutionContext.Object.SharedVariables.Contains("AbortProcess"))
            {
                Actual = (string)mockPluginExecutionContext.Object.SharedVariables["AbortProcess"];
            }
            Assert.AreEqual(Expected, Actual);
        }

        [TestMethod]
        public void InvalidEntityTest()
        {
            //Arrange
            mockPluginExecutionContext.Setup(pec => pec.PrimaryEntityName).Returns("account");

            //Act
            penvio.Execute(mockServiceProvider.Object);

            //Assert
            var Expected = "Is not a opportunity";
            var Actual = default(string);
            if (mockPluginExecutionContext.Object.SharedVariables.Contains("AbortProcess"))
            {
                Actual = (string)mockPluginExecutionContext.Object.SharedVariables["AbortProcess"];
            }
            Assert.AreEqual(Expected, Actual);
        }

        [TestMethod]
        public void EmptyTargetTest()
        {
            //Arrange
            mockPluginExecutionContext.Setup(pec => pec.InputParameters).Returns(new ParameterCollection());

            //Act
            penvio.Execute(mockServiceProvider.Object);

            //Assert
            var Expected = "Do not Contains Target";
            var Actual = default(string);
            if (mockPluginExecutionContext.Object.SharedVariables.Contains("AbortProcess"))
            {
                Actual = (string)mockPluginExecutionContext.Object.SharedVariables["AbortProcess"];
            }
            Assert.AreEqual(Expected, Actual);

        }

        [TestMethod]
        public void InvalidTargetTest()
        {
            //Arrange
            mockPluginExecutionContext.Setup(pec => pec.InputParameters)
                .Returns(new ParameterCollection { { "Target", new EntityReference() } });

            //Act
            penvio.Execute(mockServiceProvider.Object);

            //Assert
            var Expected = "Is Not an Entity";
            var Actual = default(string);
            if (mockPluginExecutionContext.Object.SharedVariables.Contains("AbortProcess"))
            {
                Actual = (string)mockPluginExecutionContext.Object.SharedVariables["AbortProcess"];
            }
            Assert.AreEqual(Expected, Actual);
        }
        #endregion


        #region ValidaInformaciondelRegistro

        [TestMethod]
        public void WitouthIdBannerTest()
        {
            //Arrange
            if (oportunidad.Attributes.Contains("rs_idbanner"))
                oportunidad.Attributes.Remove("rs_idbanner");

            //Act
            penvio.Execute(mockServiceProvider.Object);

            //Assert
            var Expected = "It not Contains Id Banner";
            var Actual = default(string);
            if (mockPluginExecutionContext.Object.SharedVariables.Contains("AbortProcess"))
            {
                Actual = (string)mockPluginExecutionContext.Object.SharedVariables["AbortProcess"];
            }
            Assert.AreEqual(Expected, Actual);
        }




        [TestMethod]
        public void WitouthPeriodoTest()
        {
            //Arrange
            if (ImagenOportunidad.Attributes.Contains("rs_periodoid"))
                ImagenOportunidad.Attributes.Remove("rs_periodoid");

            if (oportunidad.Attributes.Contains("rs_periodoid"))
                oportunidad.Attributes.Remove("rs_periodoid");




            //Act
            penvio.Execute(mockServiceProvider.Object);

            //Assert
            var Expected = "It not Contains Periodo";
            var Actual = default(string);
            if (mockPluginExecutionContext.Object.SharedVariables.Contains("AbortProcess"))
            {
                Actual = (string)mockPluginExecutionContext.Object.SharedVariables["AbortProcess"];
            }
            Assert.AreEqual(Expected, Actual);
        }

        [TestMethod]
        public void WitouthProgramaTest()
        {
            //Arrange
            if (ImagenOportunidad.Attributes.Contains("rs_programaid"))
                ImagenOportunidad.Attributes.Remove("rs_programaid");

            if (oportunidad.Attributes.Contains("rs_programaid"))
                oportunidad.Attributes.Remove("rs_programaid");

            //Act
            penvio.Execute(mockServiceProvider.Object);

            //Assert
            var Expected = "It not Contains Programa";
            var Actual = default(string);
            if (mockPluginExecutionContext.Object.SharedVariables.Contains("AbortProcess"))
            {
                Actual = (string)mockPluginExecutionContext.Object.SharedVariables["AbortProcess"];
            }
            Assert.AreEqual(Expected, Actual);
        }


        [TestMethod]
        public void WitouthVPDITest()
        {
            //Arrange
            if (ImagenOportunidad.Attributes.Contains("rs_vpdid"))
                ImagenOportunidad.Attributes.Remove("rs_vpdid");

            if (oportunidad.Attributes.Contains("rs_vpdid"))
                oportunidad.Attributes.Remove("rs_vpdid");

            //Act
            penvio.Execute(mockServiceProvider.Object);

            //Assert
            var Expected = "It not Contains VPDI";
            var Actual = default(string);
            if (mockPluginExecutionContext.Object.SharedVariables.Contains("AbortProcess"))
            {
                Actual = (string)mockPluginExecutionContext.Object.SharedVariables["AbortProcess"];
            }
            Assert.AreEqual(Expected, Actual);
        }

        [TestMethod]
        public void InsertarSQL()
        {
           var DatosInsertar = new AlmacenaIdOportunidad
           {
              
               Id_Banner = "Banner",
               Id_Cuenta = "Cuenta",
               Id_Oportunidad = "Oportunidad",
               Periodo = "201018",
               Programa = "LC-INE-UAN",
               VPD = null
           };

            ConexionSQL cone = new ConexionSQL();

            cone.InsertarBanderasBD(DatosInsertar, "Mensaje prueba 2");
        }

        #endregion

    }
}
