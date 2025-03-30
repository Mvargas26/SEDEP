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
    public class CompetenciasNegocio
    {
        SQLServerContext_Datos objDatos = new SQLServerContext_Datos();

        public CompetenciasModel ConsultarCompetenciaID(int idCompetencia)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Operacion", "R"),
                    new SqlParameter("@idCompetencia", idCompetencia)
                };

                DataTable dt = objDatos.EjecutarSQLconSP_DT("adm.sp_Competencias_CRUD", parametros);

                if (dt.Rows.Count == 0)
                    return null;

                DataRow row = dt.Rows[0];

                return new CompetenciasModel
                {
                    IdCompetencia = Convert.ToInt32(row["idCompetencia"]),
                    Competencia = row["Competencia"].ToString(),
                    Calificacion = Convert.ToDecimal(row["Calificacion"]),
                    IdTipoCompetencia = row["idTipoCompetencia"] != DBNull.Value ? Convert.ToInt32(row["idTipoCompetencia"]) : (int?)null
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error al consultar el objetivo por ID: " + ex.Message);
            }
        }//fin ConsultarObjetivoID

        public List<CompetenciasModel> ListarCompetencias()
        {
            List<CompetenciasModel> lista = new();

            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Operacion", "R"),
                    new SqlParameter("@idCompetencia", DBNull.Value)
                };

                DataTable dt = objDatos.EjecutarSQLconSP_DT("adm.sp_Competencias_CRUD", parametros);

                foreach (DataRow row in dt.Rows)
                {
                    lista.Add(new CompetenciasModel
                    {
                        IdCompetencia = Convert.ToInt32(row["idCompetencia"]),
                        Competencia = row["Competencia"].ToString(),
                        Calificacion = Convert.ToDecimal(row["Calificacion"]),
                        Tipo = row["Tipo"].ToString(),
                        IdTipoCompetencia = row["idTipoCompetencia"] != DBNull.Value ? Convert.ToInt32(row["idTipoCompetencia"]) : (int?)null
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar las competencias: " + ex.Message);
            }

            return lista;
        }//fin ListarObjetivos

        public void CrearCompetencia(CompetenciasModel objetivo)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Operacion", "C"),
                    new SqlParameter("@Competencia", objetivo.Competencia),
                    new SqlParameter("@Calificacion", objetivo.Calificacion),
                    new SqlParameter("@idTipoCompetencia", objetivo.IdTipoCompetencia)
                };

                objDatos.EjecutarSQLconSP_Void("adm.sp_Competencias_CRUD", parametros);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear la competencia: " + ex.Message);
            }
        }//fin CrearObjetivo

        public void ModificarCompetencia(CompetenciasModel objetivo)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Operacion", "U"),
                    new SqlParameter("@idCompetencia", objetivo.IdCompetencia),
                    new SqlParameter("@Competencia", objetivo.Competencia),
                    new SqlParameter("@Calificacion", objetivo.Calificacion),
                    new SqlParameter("@idTipoCompetencia", objetivo.IdTipoCompetencia)
                };

                objDatos.EjecutarSQLconSP_Void("adm.sp_Competencias_CRUD", parametros);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al modificar el competencia: " + ex.Message);
            }
        }//fin ModificarObjetivo

        public void EliminarCompetencia(int id)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Operacion", "D"),
                    new SqlParameter("@idCompetencia", id)
                };

                objDatos.EjecutarSQLconSP_Void("adm.sp_Competencias_CRUD", parametros);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el competencia: " + ex.Message);
            }
        }//fin EliminarObjetivo
    }
}
