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
                    new SqlParameter("@Operacion", "R") 
                };

                DataTable dt = objDatos.EjecutarSQLconSP_DT("[adm].[sp_EvaluacionPorObjetivo_CRUD]", parametros);

                foreach (DataRow row in dt.Rows)
                {
                    lista.Add(new EvaluacionXObjetivoModel
                    {
                        IdEvaxObj = Convert.ToInt32(row["idEvaxObj"]),
                        IdEvaluacion = Convert.ToInt32(row["idEvaluacion"]),
                        IdObjetivo = Convert.ToInt32(row["idObjetivo"]),
                        ValorObtenido = row["ValorObtenido"] != DBNull.Value ? Convert.ToDecimal(row["ValorObtenido"]) : 0,
                        peso = row["peso"] != DBNull.Value ? Convert.ToDecimal(row["peso"]) : 0,
                        meta = row["meta"] != DBNull.Value ? Convert.ToString(row["meta"]) : string.Empty
                    });
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores (opcional)
                Console.WriteLine($"Error al listar evaluaciones por objetivo: {ex.Message}");
                throw; // Re-lanzar la excepción para manejo superior
            }

            return lista;
        }
        public void CrearEvaluacionXObjetivo(EvaluacionXObjetivoModel nueva)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Operacion", "C"),
                    new SqlParameter("@idEvaluacion", nueva.IdEvaluacion),
                    new SqlParameter("@idObjetivo", nueva.IdObjetivo),
                    new SqlParameter("@ValorObtenido", nueva.ValorObtenido),
                    new SqlParameter("@peso", nueva.peso),
                    new SqlParameter("@meta", nueva.meta ?? string.Empty)
                };

                objDatos.EjecutarSQLconSP_Void("[adm].[sp_EvaluacionPorObjetivo_CRUD]", parametros);
            }
            catch (Exception ex)
            {
                throw new Exception("Fallo al crear Evaluación x Objetivo: " + ex.Message);
            }
        }//fin CrearEvaluacionXObjetivo

        public void ModificarEvaluacionXObjetivo(EvaluacionXObjetivoModel evaluacion)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Operacion", "U"),
                    new SqlParameter("@idEvaxObj", evaluacion.IdEvaxObj),
                    new SqlParameter("@idEvaluacion", evaluacion.IdEvaluacion),
                    new SqlParameter("@idObjetivo", evaluacion.IdObjetivo),
                    new SqlParameter("@ValorObtenido", evaluacion.ValorObtenido),
                    new SqlParameter("@peso", evaluacion.peso),
                    new SqlParameter("@meta", evaluacion.meta ?? string.Empty)
                };

                objDatos.EjecutarSQLconSP_Void("[adm].[sp_EvaluacionPorObjetivo_CRUD]", parametros);
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
                    new SqlParameter("@Operacion", "D"),
                    new SqlParameter("@idEvaxObj", idEvaxObj)
                };

                objDatos.EjecutarSQLconSP_Void("[adm].[sp_EvaluacionPorObjetivo_CRUD]", parametros);
            }
            catch (Exception ex)
            {
                throw new Exception("Fallo al eliminar Evaluación x Objetivo: " + ex.Message);
            }
        }//fin

        public EvaluacionXObjetivoModel ConsultarEvaluacionXObjetivoPorID(int idEvaxObj)
        {
            try
            {
                List<SqlParameter> parametros = new()
        {
            new SqlParameter("@Operacion", "R"),
            new SqlParameter("@idEvaxObj", idEvaxObj)
        };

                DataTable dt = objDatos.EjecutarSQLconSP_DT("[adm].[sp_EvaluacionPorObjetivo_CRUD]", parametros);

                if (dt.Rows.Count == 0)
                    return null;

                DataRow row = dt.Rows[0];

                return new EvaluacionXObjetivoModel
                {
                    IdEvaxObj = Convert.ToInt32(row["idEvaxObj"]),
                    IdEvaluacion = Convert.ToInt32(row["idEvaluacion"]),
                    IdObjetivo = Convert.ToInt32(row["idObjetivo"]),
                    ValorObtenido = row["ValorObtenido"] != DBNull.Value ? Convert.ToDecimal(row["ValorObtenido"]) : 0,
                    peso = row["peso"] != DBNull.Value ? Convert.ToDecimal(row["peso"]) : 0,
                    meta = row["meta"] != DBNull.Value ? Convert.ToString(row["meta"]) : string.Empty
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error al consultar Evaluación x Objetivo por ID: " + ex.Message);
            }
        }


    } //Fin Clase 

} // Fin Space 
