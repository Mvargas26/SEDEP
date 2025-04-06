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
    class EvaluacionXcompetenciaNegocios
    {
        //***************** VARIABLES *******************
        SQLServerContext_Datos objDatos = new SQLServerContext_Datos();

        public void CrearEvaluacionXCompetencia(EvaluacionXcompetenciaModel nueva)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Operacion", "C"),
                    new SqlParameter("@idEvaluacion", nueva.IdEvaluacion),
                    new SqlParameter("@idCompetencia", nueva.IdCompetencia),
                    new SqlParameter("@valorObtenido", nueva.ValorObtenido)
                };

                objDatos.EjecutarSQLconSP_Void("[adm].[sp_EvaluacionPorCompetencia_CRUD]", parametros);
            }
            catch (Exception ex)
            {
                throw new Exception("Fallo al crear evaluación por competencia: " + ex.Message);
            }
        }//fin CrearEvaluacionXCompetencia
        public List<EvaluacionXcompetenciaModel> ListarEvaluacionXCompetencia(int? idEvaxComp = null)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Operacion", "R"),
                    new SqlParameter("@idEvaxComp", (object)idEvaxComp ?? DBNull.Value)
                };

                DataTable dt = objDatos.EjecutarSQLconSP_DT("[adm].[sp_EvaluacionPorCompetencia_CRUD]", parametros);

                return dt.AsEnumerable().Select(row => new EvaluacionXcompetenciaModel
                {
                    IdEvaxComp = Convert.ToInt32(row["idEvaxComp"]),
                    IdEvaluacion = Convert.ToInt32(row["idEvaluacion"]),
                    IdCompetencia = Convert.ToInt32(row["idCompetencia"]),
                    ValorObtenido = Convert.ToDecimal(row["valorObtenido"])
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Fallo al leer evaluación por competencia: " + ex.Message);
            }
        }//fin ListarEvaluacionXCompetencia

        public void ActualizarEvaluacionXCompetencia(EvaluacionXcompetenciaModel actualizada)
        {
            try
            {
                    List<SqlParameter> parametros = new()
            {
                new SqlParameter("@Operacion", "U"),
                new SqlParameter("@idEvaxComp", actualizada.IdEvaxComp),
                new SqlParameter("@idEvaluacion", actualizada.IdEvaluacion),
                new SqlParameter("@idCompetencia", actualizada.IdCompetencia),
                new SqlParameter("@valorObtenido", actualizada.ValorObtenido)
            };

                objDatos.EjecutarSQLconSP_Void("[adm].[sp_EvaluacionPorCompetencia_CRUD]", parametros);
            }
            catch (Exception ex)
            {
                throw new Exception("Fallo al actualizar evaluación por competencia: " + ex.Message);
            }
        }//fin ActualizarEvaluacionXCompetencia

        public void EliminarEvaluacionXCompetencia(int idEvaxComp)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Operacion", "D"),
                    new SqlParameter("@idEvaxComp", idEvaxComp)
                };

                objDatos.EjecutarSQLconSP_Void("[adm].[sp_EvaluacionPorCompetencia_CRUD]", parametros);
            }
            catch (Exception ex)
            {
                throw new Exception("Fallo al eliminar evaluación por competencia: " + ex.Message);
            }
        }//fin EliminarEvaluacionXCompetencia


    }//fin class
}//fin space
