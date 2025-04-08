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
    public class EvaluacionesNegocio
    {

        //***************** VARIABLES *******************
        SQLServerContext_Datos objDatos = new SQLServerContext_Datos();

        public List<EvaluacionModel> ListarEvaluaciones()
        {
            List<EvaluacionModel> lista = new();

            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Accion", "SELECT")
                };

                DataTable dt = objDatos.EjecutarSQLconSP_DT("[adm].[sp_CrudEvaluaciones]", parametros);

                foreach (DataRow row in dt.Rows)
                {
                    lista.Add(new EvaluacionModel
                    {
                        IdEvaluacion = Convert.ToInt32(row["idEvaluacion"]),
                        IdFuncionario = row["idFuncionario"].ToString(),
                        Observaciones = row["Observaciones"]?.ToString(),
                        FechaCreacion = Convert.ToDateTime(row["fechaCreacion"]),
                        EstadoEvaluacion = Convert.ToInt32(row["estadoEvaluacion"])
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Fallo en EvaluacionesNegocio " + ex.Message);
            }

            return lista;
        }

        public void CrearEvaluacion(EvaluacionModel nueva)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Accion", "INSERT"),
                    new SqlParameter("@IdFuncionario", nueva.IdFuncionario),
                    new SqlParameter("@Observaciones", (object)nueva.Observaciones ?? DBNull.Value),
                    new SqlParameter("@FechaCreacion", nueva.FechaCreacion),
                    new SqlParameter("@EstadoEvaluacion", nueva.EstadoEvaluacion)
                };

                objDatos.EjecutarSQLconSP_Void("[adm].[sp_CrudEvaluaciones]", parametros);
            }
            catch (Exception ex)
            {
                throw new Exception("Fallo al crear evaluación: " + ex.Message);
            }
        }

        public void ModificarEvaluacion(EvaluacionModel evaluacion)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Accion", "UPDATE"),
                    new SqlParameter("@IdEvaluacion", evaluacion.IdEvaluacion),
                    new SqlParameter("@IdFuncionario", evaluacion.IdFuncionario),
                    new SqlParameter("@Observaciones", (object)evaluacion.Observaciones ?? DBNull.Value),
                    new SqlParameter("@FechaCreacion", evaluacion.FechaCreacion),
                    new SqlParameter("@EstadoEvaluacion", evaluacion.EstadoEvaluacion)
                };

                objDatos.EjecutarSQLconSP_Void("[adm].[sp_CrudEvaluaciones]", parametros);
            }
            catch (Exception ex)
            {
                throw new Exception("Fallo al modificar evaluación: " + ex.Message);
            }
        }

        public void EliminarEvaluacion(int idEvaluacion)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Accion", "DELETE"),
                    new SqlParameter("@IdEvaluacion", idEvaluacion)
                };

                objDatos.EjecutarSQLconSP_Void("[adm].[sp_CrudEvaluaciones]", parametros);
            }
            catch (Exception ex)
            {
                throw new Exception("Fallo al eliminar evaluación: " + ex.Message);
            }
        }

        public EvaluacionModel ConsultarEvaluacionPorID(int idEvaluacion)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@IdEvaluacion", idEvaluacion)
                };

                DataTable dt = objDatos.EjecutarSQLconSP_DT("adm.sp_ConsultarEvaluacionPorID", parametros);

                if (dt.Rows.Count == 0)
                    return null;

                DataRow row = dt.Rows[0];

                return new EvaluacionModel
                {
                    IdEvaluacion = Convert.ToInt32(row["idEvaluacion"]),
                    IdFuncionario = row["idFuncionario"].ToString(),
                    Observaciones = row["Observaciones"]?.ToString(),
                    FechaCreacion = Convert.ToDateTime(row["fechaCreacion"]),
                    EstadoEvaluacion = Convert.ToInt32(row["estadoEvaluacion"])
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error al consultar evaluación por ID: " + ex.Message);
            }
        }

        public (List<ObjetivoModel>, List<CompetenciasModel>) ListarObjYCompetenciasXConglomerado(int idConglomerado)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@idConglomerado", idConglomerado)
                };

                DataSet ds = objDatos.EjecutarSQLconSP_DS("sp_ListaObjetivosYCompetencias", parametros);

                List<ObjetivoModel> listaObjetivos = new();
                List<CompetenciasModel> listaCompetencias = new();

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        listaObjetivos.Add(new ObjetivoModel
                        {
                            IdObjetivo = Convert.ToInt32(row["idObjetivo"]),
                            Objetivo = row["Objetivo"].ToString(),
                            Porcentaje = Convert.ToDecimal(row["PorcentajeObjetivo"]),
                            IdTipoObjetivo = row.Table.Columns.Contains("idTipoObjetivo") && row["idTipoObjetivo"] != DBNull.Value ? Convert.ToInt32(row["idTipoObjetivo"]) : (int?)null
                        });
                    }
                }

                if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[1].Rows)
                    {
                        listaCompetencias.Add(new CompetenciasModel
                        {
                            IdCompetencia = Convert.ToInt32(row["idCompetencia"]),
                            Competencia = row["Competencia"].ToString(),
                            Calificacion = Convert.ToDecimal(row["Calificacion"]),
                            IdTipoCompetencia = row.Table.Columns.Contains("idTipoCompetencia") && row["idTipoCompetencia"] != DBNull.Value ? Convert.ToInt32(row["idTipoCompetencia"]) : (int?)null
                        });
                    }
                }

                return (listaObjetivos, listaCompetencias);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar objetivos y competencias por conglomerado: " + ex.Message);
            }
        }

    }//fin class
}//fin space
