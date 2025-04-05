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
    public class EstadoEvaluacionNegocio
    {

        SQLServerContext_Datos objDatos = new SQLServerContext_Datos();

        public List<EstadoEvaluacionModel> ListarEstados()
        {
            List<EstadoEvaluacionModel> lista = new();

            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Accion", "SELECT")
                };

                DataTable dt = objDatos.EjecutarSQLconSP_DT("[adm].[sp_CrudEstadoEvaluacion]", parametros);

                foreach (DataRow row in dt.Rows)
                {
                    lista.Add(new EstadoEvaluacionModel
                    {
                        IdEstado = Convert.ToInt32(row["idEstado"]),
                        EstadoEvaluacion = row["EstadoEvaluacion"].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Fallo en EstadoEvaluacionNegocio " + ex.Message);
            }

            return lista;
        }

        public void CrearEstado(EstadoEvaluacionModel nuevo)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Accion", "INSERT"),
                    new SqlParameter("@EstadoEvaluacion", nuevo.EstadoEvaluacion)
                };

                objDatos.EjecutarSQLconSP_Void("[adm].[sp_CrudEstadoEvaluacion]", parametros);
            }
            catch (Exception ex)
            {
                throw new Exception("Fallo al crear el estado de evaluación: " + ex.Message);
            }
        }

        public void ModificarEstado(EstadoEvaluacionModel estado)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Accion", "UPDATE"),
                    new SqlParameter("@IdEstado", estado.IdEstado),
                    new SqlParameter("@EstadoEvaluacion", estado.EstadoEvaluacion)
                };

                objDatos.EjecutarSQLconSP_Void("[adm].[sp_CrudEstadoEvaluacion]", parametros);
            }
            catch (Exception ex)
            {
                throw new Exception("Fallo al modificar el estado de evaluación: " + ex.Message);
            }
        }

        public void EliminarEstado(int idEstado)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Accion", "DELETE"),
                    new SqlParameter("@IdEstado", idEstado)
                };

                objDatos.EjecutarSQLconSP_Void("[adm].[sp_CrudEstadoEvaluacion]", parametros);
            }
            catch (Exception ex)
            {
                throw new Exception("Fallo al eliminar el estado de evaluación: " + ex.Message);
            }
        }

        public EstadoEvaluacionModel ConsultarEstadoPorID(int idEstado)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@IdEstado", idEstado)
                };

                DataTable dt = objDatos.EjecutarSQLconSP_DT("adm.sp_ConsultarEstadoEvaluacionPorID", parametros);

                if (dt.Rows.Count == 0)
                    return null;

                DataRow row = dt.Rows[0];

                return new EstadoEvaluacionModel
                {
                    IdEstado = Convert.ToInt32(row["idEstado"]),
                    EstadoEvaluacion = row["EstadoEvaluacion"].ToString()
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error al consultar el estado por ID: " + ex.Message);
            }
        }


    }//Fin Clase 
} // Fin Space 
