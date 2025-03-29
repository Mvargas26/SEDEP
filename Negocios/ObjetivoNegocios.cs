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
    public class ObjetivoNegocios
    {
        //***************** VARIABLES *******************
        SQLServerContext_Datos objDatos = new SQLServerContext_Datos();

        public ObjetivoModel ConsultarObjetivoID(int idObjetivo)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Operacion", "R"),
                    new SqlParameter("@idObjetivo", idObjetivo)
                };

                DataTable dt = objDatos.EjecutarSQLconSP_DT("adm.sp_Objetivos_CRUD", parametros);

                if (dt.Rows.Count == 0)
                    return null;

                DataRow row = dt.Rows[0];

                return new ObjetivoModel
                {
                    IdObjetivo = Convert.ToInt32(row["idObjetivo"]),
                    Objetivo = row["Objetivo"].ToString(),
                    Porcentaje = Convert.ToDecimal(row["Porcentaje"]),
                    IdTipoObjetivo = row["idTipoObjetivo"] != DBNull.Value ? Convert.ToInt32(row["idTipoObjetivo"]) : (int?)null
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error al consultar el objetivo por ID: " + ex.Message);
            }
        }//fin ConsultarObjetivoID

        public List<ObjetivoModel> ListarObjetivos()
        {
            List<ObjetivoModel> lista = new();

            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Operacion", "R"),
                    new SqlParameter("@idObjetivo", DBNull.Value)
                };

                DataTable dt = objDatos.EjecutarSQLconSP_DT("adm.sp_Objetivos_CRUD", parametros);

                foreach (DataRow row in dt.Rows)
                {
                    lista.Add(new ObjetivoModel
                    {
                        IdObjetivo = Convert.ToInt32(row["idObjetivo"]),
                        Objetivo = row["Objetivo"].ToString(),
                        Porcentaje = Convert.ToDecimal(row["Porcentaje"]),
                        Tipo = row["Tipo"].ToString(),
                        IdTipoObjetivo = row["idTipoObjetivo"] != DBNull.Value ? Convert.ToInt32(row["idTipoObjetivo"]) : (int?)null
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar los objetivos: " + ex.Message);
            }

            return lista;
        }//fin ListarObjetivos

        public void CrearObjetivo(ObjetivoModel objetivo)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Operacion", "C"),
                    new SqlParameter("@Objetivo", objetivo.Objetivo),
                    new SqlParameter("@Porcentaje", objetivo.Porcentaje),
                    new SqlParameter("@idTipoObjetivo", objetivo.IdTipoObjetivo)
                };

                objDatos.EjecutarSQLconSP_Void("adm.sp_Objetivos_CRUD", parametros);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear el objetivo: " + ex.Message);
            }
        }//fin CrearObjetivo

        public void ModificarObjetivo(ObjetivoModel objetivo)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Operacion", "U"),
                    new SqlParameter("@idObjetivo", objetivo.IdObjetivo),
                    new SqlParameter("@Objetivo", objetivo.Objetivo),
                    new SqlParameter("@Porcentaje", objetivo.Porcentaje),
                    new SqlParameter("@idTipoObjetivo", objetivo.IdTipoObjetivo)
                };

                objDatos.EjecutarSQLconSP_Void("adm.sp_Objetivos_CRUD", parametros);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al modificar el objetivo: " + ex.Message);
            }
        }//fin ModificarObjetivo

        public void EliminarObjetivo(int id)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Operacion", "D"),
                    new SqlParameter("@idObjetivo", id)
                };

                objDatos.EjecutarSQLconSP_Void("adm.sp_Objetivos_CRUD", parametros);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el objetivo: " + ex.Message);
            }
        }//fin EliminarObjetivo


    }//fin class
}//fin space
