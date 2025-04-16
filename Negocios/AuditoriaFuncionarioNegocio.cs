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
        SQLServerContext_Datos objDatos = new SQLServerContext_Datos();

        public List<AuditoriaFuncionarioModel> ListarAuditorias()
        {
            List<AuditoriaFuncionarioModel> lista = new();

            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Operacion", "R")
                };

                DataTable dt = objDatos.EjecutarSQLconSP_DT("sp_AuditoriaFuncionarios_CRUD", parametros);

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
                throw new Exception("Fallo al listar auditorías: " + ex.Message);
            }

            return lista;
        }

        public void CrearAuditoria(AuditoriaFuncionarioModel auditoria)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Operacion", "C"),
                    new SqlParameter("@IdFuncionario", auditoria.IdFuncionario),
                    new SqlParameter("@Accion", auditoria.Accion),
                    new SqlParameter("@FechaHora", auditoria.FechaHora),
                    new SqlParameter("@UsuarioAccion", auditoria.UsuarioAccion),
                    new SqlParameter("@CampoModificado", auditoria.CampoModificado),
                    new SqlParameter("@ValorAnterior", auditoria.ValorAnterior ?? (object)DBNull.Value),
                    new SqlParameter("@ValorNuevo", auditoria.ValorNuevo ?? (object)DBNull.Value)
                };

                objDatos.EjecutarSQLconSP_Void("sp_AuditoriaFuncionarios_CRUD", parametros);
            }
            catch (Exception ex)
            {
                throw new Exception("Fallo al crear auditoría: " + ex.Message);
            }
        }

        public void ModificarAuditoria(AuditoriaFuncionarioModel auditoria)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Operacion", "U"),
                    new SqlParameter("@IdAuditoria", auditoria.IdAuditoria),
                    new SqlParameter("@IdFuncionario", auditoria.IdFuncionario),
                    new SqlParameter("@Accion", auditoria.Accion),
                    new SqlParameter("@FechaHora", auditoria.FechaHora),
                    new SqlParameter("@UsuarioAccion", auditoria.UsuarioAccion),
                    new SqlParameter("@CampoModificado", auditoria.CampoModificado),
                    new SqlParameter("@ValorAnterior", auditoria.ValorAnterior ?? (object)DBNull.Value),
                    new SqlParameter("@ValorNuevo", auditoria.ValorNuevo ?? (object)DBNull.Value)
                };

                objDatos.EjecutarSQLconSP_Void("sp_AuditoriaFuncionarios_CRUD", parametros);
            }
            catch (Exception ex)
            {
                throw new Exception("Fallo al modificar auditoría: " + ex.Message);
            }
        }

        public void EliminarAuditoria(string idAuditoria)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Operacion", "D"),
                    new SqlParameter("@IdAuditoria", idAuditoria)
                };

                objDatos.EjecutarSQLconSP_Void("sp_AuditoriaFuncionarios_CRUD", parametros);
            }
            catch (Exception ex)
            {
                throw new Exception("Fallo al eliminar auditoría: " + ex.Message);
            }
        }

        public AuditoriaFuncionarioModel ConsultarAuditoriaPorID(string idAuditoria)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@IdAuditoria", idAuditoria)
                };

                DataTable dt = objDatos.EjecutarSQLconSP_DT("sp_ConsultarAuditoriaPorID", parametros);

                if (dt.Rows.Count == 0)
                    return null;

                DataRow row = dt.Rows[0];

                return new AuditoriaFuncionarioModel
                {
                    IdAuditoria = row["idAuditoria"].ToString(),
                    IdFuncionario = row["idFuncionario"].ToString(),
                    Accion = row["accion"].ToString(),
                    FechaHora = row["fechaHora"].ToString(),
                    UsuarioAccion = row["usuarioAccion"].ToString(),
                    CampoModificado = row["campoModificado"].ToString(),
                    ValorAnterior = row["valorAnterior"].ToString(),
                    ValorNuevo = row["valorNuevo"].ToString()
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error al consultar auditoría por ID: " + ex.Message);
            }
        }
    }
}
