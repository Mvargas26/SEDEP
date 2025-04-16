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

        public EvaluacionModel CrearEvaluacion(EvaluacionModel nueva)
        {
            try
            {
                string fechaTratada = nueva.FechaCreacion.ToString("yyyy-MM-dd");
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Operacion", "C"),
                    new SqlParameter("@idFuncionario", nueva.IdFuncionario),
                    new SqlParameter("@observaciones", (object)nueva.Observaciones ?? DBNull.Value),
                    new SqlParameter("@fechaCreacion", fechaTratada),
                new SqlParameter("@estadoEvaluacion", nueva.EstadoEvaluacion),
                    new SqlParameter("@idConglomerado", nueva.IdConglomerado)
                };

                // Ejecutar el procedimiento almacenado
                DataTable dt = objDatos.EjecutarSQLconSP_DT("[sp_Evaluacion_CRUD]", parametros);

                if (dt.Rows.Count == 0)
                    return null;

                DataRow row = dt.Rows[0];

                return new EvaluacionModel
                {
                    IdEvaluacion = Convert.ToInt32(row["idEvaluacion"]),
                };
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
            new SqlParameter("@Operacion", "U"),
            new SqlParameter("@idEvaluacion", evaluacion.IdEvaluacion),
            new SqlParameter("@idFuncionario", evaluacion.IdFuncionario),
            new SqlParameter("@idConglomerado", (object)evaluacion.IdConglomerado ?? DBNull.Value),
            new SqlParameter("@Observaciones", (object)evaluacion.Observaciones ?? DBNull.Value),
            new SqlParameter("@fechaCreacion", evaluacion.FechaCreacion),
            new SqlParameter("@estadoEvaluacion", evaluacion.EstadoEvaluacion)
        };

                objDatos.EjecutarSQLconSP_Void("[adm].[sp_Evaluacion_CRUD]", parametros);
            }
            catch (Exception ex)
            {
                throw new Exception("Fallo al modificar evaluación: " + ex.Message);
            }
        }//fin ModificarEvaluacion

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
                    new SqlParameter("@Operacion","R"),  
                    new SqlParameter("@idEvaluacion", idEvaluacion)  
                };

                DataTable dt = objDatos.EjecutarSQLconSP_DT("sp_Evaluacion_CRUD", parametros);  

                if (dt.Rows.Count == 0)
                    return null;

                DataRow row = dt.Rows[0];

                return new EvaluacionModel
                {
                    IdEvaluacion = Convert.ToInt32(row["idEvaluacion"]),
                    IdFuncionario = row["idFuncionario"] != DBNull.Value ? row["idFuncionario"].ToString() : null,
                    IdConglomerado = Convert.ToInt32(row["idConglomerado"]),
                    Observaciones = row["Observaciones"] != DBNull.Value ? row["Observaciones"].ToString() : null,
                    FechaCreacion = Convert.ToDateTime(row["fechaCreacion"]),
                    EstadoEvaluacion = Convert.ToInt32(row["estadoEvaluacion"])
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error al consultar evaluación por ID: " + ex.Message);
            }
        }//fin ConsultarEvaluacionPorID

        public EvaluacionModel ConsultarEvaluacionComoFuncionario(string idFuncionario, int idConglomerado)
        {
            try
            {
                List<SqlParameter> parametros = new()
        {
            new SqlParameter("@idFuncionario", idFuncionario),
            new SqlParameter("@idConglomerado", idConglomerado)
        };

                DataTable dt = objDatos.EjecutarSQLconSP_DT("adm.sp_consultarEvaluacionComoFuncionario", parametros);

                if (dt.Rows.Count == 0)
                    return null;

                DataRow row = dt.Rows[0];

                return new EvaluacionModel
                {
                    IdEvaluacion = Convert.ToInt32(row["idEvaluacion"]),
                    IdFuncionario = row["idFuncionario"].ToString(),
                    Observaciones = row["Observaciones"]?.ToString(),
                    FechaCreacion = Convert.ToDateTime(row["fechaCreacion"]),
                    EstadoEvaluacion = Convert.ToInt32(row["estadoEvaluacion"]),
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error al consultar evaluación del funcionario: " + ex.Message);
            }
        }//fin 

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
        }//fin ConsultarEvaluacionPorID

        public (List<EvaluacionXObjetivoModel>,List <EvaluacionXcompetenciaModel>) Listar_objetivosYCompetenciasXEvaluacion (int idEvaluacion)
        {
            List<EvaluacionXObjetivoModel> listaObjetivos = new();
            List<EvaluacionXcompetenciaModel> listaCompetencias = new();

            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@idEvaluacion", idEvaluacion)
                };

                DataSet ds = objDatos.EjecutarSQLconSP_DS("sp_objetivosYCompetenciasXEvaluacion", parametros);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        listaCompetencias.Add(new EvaluacionXcompetenciaModel
                        {
                            IdEvaxComp = Convert.ToInt32(row["IdEvaxComp"]),
                            IdEvaluacion = Convert.ToInt32(row["idEvaluacion"]),
                            IdCompetencia = Convert.ToInt32(row["idCompetencia"]),
                            ValorObtenido = Convert.ToDecimal(row["valorObtenido"]),
                            Peso = Convert.ToDecimal(row["peso"]),
                            Meta = row["meta"].ToString(),
                            NombreCompetencia = row["NombreCompetencia"].ToString(),
                            TipoCompetencia = row["TipoCompetencia"].ToString()
                        });
                    }
                }

                if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[1].Rows)
                    {
                        listaObjetivos.Add(new EvaluacionXObjetivoModel
                        {
                            IdEvaxObj = Convert.ToInt32(row["IdEvaxObj"]),
                            IdEvaluacion = Convert.ToInt32(row["idEvaluacion"]),
                            IdObjetivo = Convert.ToInt32(row["idObjetivo"]),
                            ValorObtenido = Convert.ToDecimal(row["valorObtenido"]),
                            Peso = Convert.ToDecimal(row["peso"]),
                            Meta = row["meta"].ToString(),
                            NombreObjetivo = row["NombreObjetivo"].ToString(),
                            TipoObjetivo = row["TipoObjetivo"].ToString()
                        });
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

            return (listaObjetivos, listaCompetencias);
        }//fin Listar_objetivosYCompetenciasXEvaluacion

    }//fin class
}//fin space
