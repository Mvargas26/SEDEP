using Datos;
using Microsoft.Data.SqlClient;
using Modelos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace Negocios
{
    public class ConglomeradosNegocios
    {
       //***************** VARIABLES *******************
        SQLServerContext_Datos objDatos = new SQLServerContext_Datos();


        public ConglomeradoModel ConsultarConglomeradoID(int id)
        {
            List<SqlParameter> parametros = new()
            {
                new SqlParameter("@Operacion", "SELECT"),
                new SqlParameter("@idConglomerado", id)
            };

            DataTable dt = objDatos.EjecutarSQLconSP_DT("adm.sp_Conglomerados_CRUD", parametros);

            if (dt.Rows.Count == 0)
                return null;

            DataRow row = dt.Rows[0];

            return new ConglomeradoModel
            {
                IdConglomerado = Convert.ToInt32(row["idConglomerado"]),
                NombreConglomerado = row["nombreConglomerado"].ToString(),
                Descripcion = row["Descripcion"].ToString()
            };
        }//fin ConsultarConglomeradoID


        public List<ConglomeradoModel> ListarConglomerados()
        {
            List<SqlParameter> parametros = new()
            {
                new SqlParameter("@Operacion", "SELECT"),
                new SqlParameter("@idConglomerado", DBNull.Value)
            };

            DataTable dt = objDatos.EjecutarSQLconSP_DT("adm.sp_Conglomerados_CRUD", parametros);
            List<ConglomeradoModel> lista = new();

            foreach (DataRow row in dt.Rows)
            {
                lista.Add(new ConglomeradoModel
                {
                    IdConglomerado = Convert.ToInt32(row["idConglomerado"]),
                    NombreConglomerado = row["nombreConglomerado"].ToString(),
                    Descripcion = row["Descripcion"].ToString()
                });
            }

            return lista;
        }//fin ListarConglomerados

        public void CrearConglomerado(ConglomeradoModel conglomerado)
        {
            List<SqlParameter> parametros = new()
            {
                new SqlParameter("@Operacion", "INSERT"),
                new SqlParameter("@nombreConglomerado", conglomerado.NombreConglomerado),
                new SqlParameter("@Descripcion", conglomerado.Descripcion)
            };

            objDatos.EjecutarSQLconSP_Void("adm.sp_Conglomerados_CRUD", parametros);
        }//fin CrearConglomerado

        public void ModificarConglomerado(ConglomeradoModel conglomerado)
        {
            List<SqlParameter> parametros = new()
            {
                new SqlParameter("@Operacion", "UPDATE"),
                new SqlParameter("@idConglomerado", conglomerado.IdConglomerado),
                new SqlParameter("@nombreConglomerado", conglomerado.NombreConglomerado),
                new SqlParameter("@Descripcion", conglomerado.Descripcion)
            };

            objDatos.EjecutarSQLconSP_Void("adm.sp_Conglomerados_CRUD", parametros);
        }//fin ModificarConglomerado

        public void EliminarConglomerado(int id)
        {
            List<SqlParameter> parametros = new()
            {
                new SqlParameter("@Operacion", "D"),
                new SqlParameter("@idConglomerado", id)
            };

            objDatos.EjecutarSQLconSP_Void("adm.sp_Conglomerados_CRUD", parametros);
        }//fin EliminarConglomerado


        public string pruebaConexion()
        {
            string mensaje = "";
            try
            {

                objDatos.pruebaCon();

                mensaje = "Conexion Exitosa";
            }
            catch (Exception)
            {
                mensaje = "Conexion fallo";
                throw;
            }

            return mensaje;
        }
    }//fin class
}//fin space
