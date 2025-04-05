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
    public class PeriodosEvaluacionNegocio
    {
        SQLServerContext_Datos objDatos = new SQLServerContext_Datos();

        public List<PeriodoEvaluacionModel> ListarPeriodos()
        {
            List<PeriodoEvaluacionModel> lista = new();

            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Accion", "SELECT")
                };

                DataTable dt = objDatos.EjecutarSQLconSP_DT("[adm].[sp_CrudPeriodosEvaluacion]", parametros);

                foreach (DataRow row in dt.Rows)
                {
                    lista.Add(new PeriodoEvaluacionModel
                    {
                        Anio = Convert.ToInt32(row["anio"]),
                        FechaMaxima = Convert.ToDateTime(row["fechaMaxima"])
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Fallo en PeriodosEvaluacionNegocio " + ex.Message);
            }

            return lista;
        }

        public void CrearPeriodo(PeriodoEvaluacionModel nuevo)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Accion", "INSERT"),
                    new SqlParameter("@Anio", nuevo.Anio),
                    new SqlParameter("@FechaMaxima", nuevo.FechaMaxima)
                };

                objDatos.EjecutarSQLconSP_Void("[adm].[sp_CrudPeriodosEvaluacion]", parametros);
            }
            catch (Exception ex)
            {
                throw new Exception("Fallo al crear el período: " + ex.Message);
            }
        }

        public void ModificarPeriodo(PeriodoEvaluacionModel periodo)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Accion", "UPDATE"),
                    new SqlParameter("@Anio", periodo.Anio),
                    new SqlParameter("@FechaMaxima", periodo.FechaMaxima)
                };

                objDatos.EjecutarSQLconSP_Void("[adm].[sp_CrudPeriodosEvaluacion]", parametros);
            }
            catch (Exception ex)
            {
                throw new Exception("Fallo al modificar el período: " + ex.Message);
            }
        }

        public void EliminarPeriodo(int anio)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Accion", "DELETE"),
                    new SqlParameter("@Anio", anio)
                };

                objDatos.EjecutarSQLconSP_Void("[adm].[sp_CrudPeriodosEvaluacion]", parametros);
            }
            catch (Exception ex)
            {
                throw new Exception("Fallo al eliminar el período: " + ex.Message);
            }
        }

        public PeriodoEvaluacionModel ConsultarPeriodoPorAnio(int anio)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Anio", anio)
                };

                DataTable dt = objDatos.EjecutarSQLconSP_DT("adm.sp_ConsultarPeriodoEvaluacion", parametros);

                if (dt.Rows.Count == 0)
                    return null;

                DataRow row = dt.Rows[0];

                return new PeriodoEvaluacionModel
                {
                    Anio = Convert.ToInt32(row["anio"]),
                    FechaMaxima = Convert.ToDateTime(row["fechaMaxima"])
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error al consultar el período por año: " + ex.Message);
            }
        }

        }//fin clase 
}//Fin Space 
