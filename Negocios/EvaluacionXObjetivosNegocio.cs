using Datos;
using Microsoft.Data.SqlClient;
using Modelos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocios
{
    public class EvaluacionXObjetivosNegocio
    {

        SQLServerContext_Datos objDatos = new SQLServerContext_Datos();

        public List<EvaluacionXObjetivoModel> ListarEvaluacionesXObjetivo()
        {
            List<EvaluacionXObjetivoModel> lista = new();

            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Accion", "SELECT")
                };

                DataTable dt = objDatos.EjecutarSQLconSP_DT("[adm].[sp_CrudEvaluacionXObjetivo]", parametros);

                foreach (DataRow row in dt.Rows)
                {
                    lista.Add(new EvaluacionXObjetivoModel
                    {
                        IdEvaxObj = Convert.ToInt32(row["idEvaxObj"]),
                        IdEvaluacion = Convert.ToInt32(row["idEvaluacion"]),
                        IdObjetivo = Convert.ToInt32(row["idObjetivo"]),
                        ValorObtenido = Convert.ToDecimal(row["ValorObtenido"])
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Fallo en EvaluacionXObjetivosNegocio " + ex.Message);
            }

            return lista;
        }

        public void CrearEvaluacionXObjetivo(EvaluacionXObjetivoModel nueva)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Accion", "INSERT"),
                    new SqlParameter("@IdEvaluacion", nueva.IdEvaluacion),
                    new SqlParameter("@IdObjetivo", nueva.IdObjetivo),
                    new SqlParameter("@ValorObtenido", nueva.ValorObtenido)
                };

                objDatos.EjecutarSQLconSP_Void("[adm].[sp_CrudEvaluacionXObjetivo]", parametros);
            }
            catch (Exception ex)
            {
                throw new Exception("Fallo al crear Evaluación x Objetivo: " + ex.Message);
            }
        }

        public void ModificarEvaluacionXObjetivo(EvaluacionXObjetivoModel evaluacion)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Accion", "UPDATE"),
                    new SqlParameter("@IdEvaxObj", evaluacion.IdEvaxObj),
                    new SqlParameter("@IdEvaluacion", evaluacion.IdEvaluacion),
                    new SqlParameter("@IdObjetivo", evaluacion.IdObjetivo),
                    new SqlParameter("@ValorObtenido", evaluacion.ValorObtenido)
                };

                objDatos.EjecutarSQLconSP_Void("[adm].[sp_CrudEvaluacionXObjetivo]", parametros);
            }
            catch (Exception ex)
            {
                throw new Exception("Fallo al modificar Evaluación x Objetivo: " + ex.Message);
            }
        }

        public void EliminarEvaluacionXObjetivo(int idEvaxObj)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Accion", "DELETE"),
                    new SqlParameter("@IdEvaxObj", idEvaxObj)
                };

                objDatos.EjecutarSQLconSP_Void("[adm].[sp_CrudEvaluacionXObjetivo]", parametros);
            }
            catch (Exception ex)
            {
                throw new Exception("Fallo al eliminar Evaluación x Objetivo: " + ex.Message);
            }
        }

        public EvaluacionXObjetivoModel ConsultarEvaluacionXObjetivoPorID(int idEvaxObj)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@IdEvaxObj", idEvaxObj)
                };

                DataTable dt = objDatos.EjecutarSQLconSP_DT("adm.sp_ConsultarEvaluacionXObjetivoPorID", parametros);

                if (dt.Rows.Count == 0)
                    return null;

                DataRow row = dt.Rows[0];

                return new EvaluacionXObjetivoModel
                {
                    IdEvaxObj = Convert.ToInt32(row["idEvaxObj"]),
                    IdEvaluacion = Convert.ToInt32(row["idEvaluacion"]),
                    IdObjetivo = Convert.ToInt32(row["idObjetivo"]),
                    ValorObtenido = Convert.ToDecimal(row["ValorObtenido"])
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error al consultar Evaluación x Objetivo por ID: " + ex.Message);
            }
        }

    } //Fin Clase 

} // Fin Space 
