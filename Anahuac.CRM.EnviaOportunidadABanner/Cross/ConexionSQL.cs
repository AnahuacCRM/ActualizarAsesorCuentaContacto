using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anahuac.CRM.EnviaOportunidadABanner.Cross
{
    public class ConexionSQL
    {

        public SqlConnection pConexion { get; set; }
        string sCone = "";

        public ConexionSQL()
        {
            sCone = "Data Source=omarca75v01dbsrv.database.windows.net;Initial Catalog=OmarCA75V01DB;User ID=omarca75v01sa;Password=Micontraes.1";
            SqlConnection conexion = new SqlConnection(sCone);
            conexion.Open();
            this.pConexion = conexion;
        }




        public void SqlAbrirConexion()
        {
            SqlConnection conexion = new SqlConnection(sCone);
            conexion.Open();
        }
        public void InsertarBanderasBD(AlmacenaIdOportunidad almacenidOp, string Mensaje)
        {
            string saveStaff = "INSERT into Plugin (Mensaje,Banner,VPD,Periodo,idCuenta,IdOportunidad,Programa,NumeroSolicitudAdmision) " +
                    " VALUES ('" + Mensaje + "', '" + almacenidOp.Id_Banner + "', '" + almacenidOp.VPD + "','" + almacenidOp.Periodo + "'" +
                    ",'" + almacenidOp.Id_Cuenta + "','" + almacenidOp.Id_Oportunidad + "','" + almacenidOp.Programa + "','" + almacenidOp.Numero_Solicitud + "');";

            SqlCommand ComandoInsertar = new SqlCommand(saveStaff);
            ComandoInsertar.Connection = this.pConexion;
            try
            {
                ComandoInsertar.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                SqlCommand ComandoInsertarExepsion = new SqlCommand("INSERT into Plugin (Mensaje)  VALUES ('" + ex.Message + "');");
                ComandoInsertarExepsion.Connection = this.pConexion;
                ComandoInsertarExepsion.ExecuteNonQuery();
            }

        }

        public void InsertarBanderasBDPrueba(string Mensaje)
        {
            

            string saveStaff = "INSERT into Plugin (Mensaje) " +
                    " VALUES ('" + Mensaje + "');";

            SqlCommand ComandoInsertar = new SqlCommand(saveStaff);
            ComandoInsertar.Connection = this.pConexion;
            try
            {
                ComandoInsertar.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                SqlCommand ComandoInsertarExepsion = new SqlCommand(ex.Message);
                ComandoInsertarExepsion.ExecuteNonQuery();
            }

        }
    }
}
