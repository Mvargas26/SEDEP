using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Microsoft.Data.SqlClient;
using Modelos;

namespace Negocios
{
    public class ReportesNegocio
    {
        SQLServerContext_Datos objDatos = new SQLServerContext_Datos();

        public List<ReporteMolde> ListarReportes()
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Accion", "SELECT")
                };

                DataTable dt = objDatos.EjecutarSQLconSP_DT("adm.sp_Reportes", parametros);
                List<ReporteMolde> lista = new();

                foreach (DataRow row in dt.Rows)
                {
                    lista.Add(new ReporteMolde
                    {
                        Cedula = row["cedula"].ToString(),
                        Nombre = row["Nombre"].ToString(),
                        IdEva = Convert.ToInt32(row["idEvaluacion"]),
                        Fecha = Convert.ToDateTime(row["fechaCreacion"]),
                        Observaciones = row["Observaciones"].ToString(),
                        Objetivo = row["Objetivo"].ToString(),
                        Competencia = row["Competencia"].ToString(),
                        Nota = Convert.ToDouble(row["ValorObtenido"])
                    });
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar los reportes: " + ex.Message);
            }
        }

        public List<ReporteMolde> ListarReportesID(string Cedula)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                   new SqlParameter("@Accion", "SELECTUNO"),
                    new SqlParameter("@ID", Cedula)
                };

                DataTable dt = objDatos.EjecutarSQLconSP_DT("adm.sp_Reportes", parametros);
                List<ReporteMolde> lista = new();

                foreach (DataRow row in dt.Rows)
                {
                    lista.Add(new ReporteMolde
                    {
                        Cedula = row["cedula"].ToString(),
                        Nombre = row["Nombre"].ToString(),
                        IdEva = Convert.ToInt32(row["idEvaluacion"]),
                        Fecha = Convert.ToDateTime(row["fechaCreacion"]),
                        Observaciones = row["Observaciones"].ToString(),
                        Objetivo = row["Objetivo"].ToString(),
                        Competencia = row["Competencia"].ToString(),
                        Nota = Convert.ToDouble(row["ValorObtenido"])
                    });
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar los reportes: " + ex.Message);
            }
                
        }

    }
}
