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
    public class AuditoriaFuncionarioNegocio
    {

        //***************** VARIABLES *******************
        SQLServerContext_Datos objDatos = new SQLServerContext_Datos();

        public List<AuditoriaFuncionarioModel> ListarAuditorias()
        {
            List<AuditoriaFuncionarioModel> lista = new();

            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Accion", "SELECT")
                };

                DataTable dt = objDatos.EjecutarSQLconSP_DT("[adm].[sp_CrudAuditoriaFuncionarios]", parametros);

                foreach (DataRow row in dt.Rows)
                {
                    lista.Add(new AuditoriaFuncionarioModel
                    {
                        IdAuditoria = row["idAuditoria"].ToString(),
                        IdFuncionario = row["idFuncionario"].ToString(),
                        Accion = row["accion"].ToString(),
                        FechaHora = row["fechaHora"].ToString(),
                        UsuarioAccion = row["usuarioAccion"].ToString(),
                        CampoModificado = row["campoModificado"].ToString(),
                        ValorAnterior = row["valorAnterior"].ToString(),
                        ValorNuevo = row["valorNuevo"].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Fallo en AuditoriaFuncionarioNegocios " + ex);
            }
            return lista;
        }

        public void CrearAuditoria(AuditoriaFuncionarioModel auditoriaNueva)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Accion", "INSERT"),
                    new SqlParameter("@IdAuditoria", auditoriaNueva.IdAuditoria),
                    new SqlParameter("@IdFuncionario", auditoriaNueva.IdFuncionario),
                    new SqlParameter("@AccionRealizada", auditoriaNueva.Accion),
                    new SqlParameter("@FechaHora", auditoriaNueva.FechaHora),
                    new SqlParameter("@UsuarioAccion", auditoriaNueva.UsuarioAccion),
                    new SqlParameter("@CampoModificado", auditoriaNueva.CampoModificado),
                    new SqlParameter("@ValorAnterior", auditoriaNueva.ValorAnterior),
                    new SqlParameter("@ValorNuevo", auditoriaNueva.ValorNuevo)
                };

                objDatos.EjecutarSQLconSP_Void("[adm].[sp_CrudAuditoriaFuncionarios]", parametros);
            }
            catch (Exception ex)
            {
                throw new Exception("Fallo en AuditoriaFuncionarioNegocios " + ex);
            }
        }

        public List<AuditoriaFuncionarioModel> ObtenerAuditoriasPorFuncionario(string idFuncionario)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@IdFuncionario", idFuncionario)
                };

                DataTable dt = objDatos.EjecutarSQLconSP_DT("adm.sp_AuditoriasPorFuncionario", parametros);
                List<AuditoriaFuncionarioModel> lista = new();

                foreach (DataRow row in dt.Rows)
                {
                    lista.Add(new AuditoriaFuncionarioModel
                    {
                        IdAuditoria = row["idAuditoria"].ToString(),
                        IdFuncionario = row["idFuncionario"].ToString(),
                        Accion = row["accion"].ToString(),
                        FechaHora = row["fechaHora"].ToString(),
                        UsuarioAccion = row["usuarioAccion"].ToString(),
                        CampoModificado = row["campoModificado"].ToString(),
                        ValorAnterior = row["valorAnterior"].ToString(),
                        ValorNuevo = row["valorNuevo"].ToString()
                    });
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener auditorías por funcionario: " + ex.Message);
            }
        }

        public void EliminarAuditoria(string idAuditoria)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Accion", "DELETE"),
                    new SqlParameter("@IdAuditoria", idAuditoria)
                };
                objDatos.EjecutarSQLconSP_Void("[adm].[sp_CrudAuditoriaFuncionarios]", parametros);
            }
            catch (Exception ex)
            {
                throw new Exception("Fallo en AuditoriaFuncionarioNegocios " + ex);
            }
        }

    }
}
