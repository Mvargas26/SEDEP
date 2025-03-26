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
    public class TiposCompetenciasNegocios
    {
        SQLServerContext_Datos objDatos = new SQLServerContext_Datos();

        // Consultar Tipo de Competencia por ID
        public TiposCompetenciasModel ConsultarTipoCompetenciaID(int idTipoCompetencia)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Operacion", "R"),
                    new SqlParameter("@idTipoCompetencia", idTipoCompetencia)
                };

                DataTable dt = objDatos.EjecutarSQLconSP_DT("sp_TipoCompetencias_CRUD", parametros);

                if (dt.Rows.Count == 0)
                    return null;

                DataRow row = dt.Rows[0];

                return new TiposCompetenciasModel
                {
                    IdTipoCompetencia = Convert.ToInt32(row["idTipoCompetencia"]),
                    Tipo = row["Tipo"].ToString()
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error al consultar el Tipo de Competencia por ID: " + ex.Message);
            }
        }

        // Listar todos los Tipos de Competencias
        public List<TiposCompetenciasModel> ListarTiposCompetencias()
        {
            List<TiposCompetenciasModel> lista = new();

            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Operacion", "R")
                };

                DataTable dt = objDatos.EjecutarSQLconSP_DT("sp_TipoCompetencias_CRUD", parametros);

                foreach (DataRow row in dt.Rows)
                {
                    lista.Add(new TiposCompetenciasModel
                    {
                        IdTipoCompetencia = Convert.ToInt32(row["idTipoCompetencia"]),
                        Tipo = row["Tipo"].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar los Tipos de Competencias: " + ex.Message);
            }

            return lista;
        }

        // Crear un nuevo Tipo de Competencia
        public void CrearTipoCompetencia(TiposCompetenciasModel tipoCompetencia)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Operacion", "C"),
                    new SqlParameter("@Tipo", tipoCompetencia.Tipo)
                };

                objDatos.EjecutarSQLconSP_Void("sp_TipoCompetencias_CRUD", parametros);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear el Tipo de Competencia: " + ex.Message);
            }
        }

        // Modificar un Tipo de Competencia existente
        public void ModificarTipoCompetencia(TiposCompetenciasModel tipoCompetencia)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Operacion", "U"),
                    new SqlParameter("@idTipoCompetencia", tipoCompetencia.IdTipoCompetencia),
                    new SqlParameter("@Tipo", tipoCompetencia.Tipo)
                };

                objDatos.EjecutarSQLconSP_Void("sp_TipoCompetencias_CRUD", parametros);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al modificar el Tipo de Competencia: " + ex.Message);
            }
        }

        // Eliminar un Tipo de Competencia
        public void EliminarTipoCompetencia(int idTipoCompetencia)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Operacion", "D"),
                    new SqlParameter("@idTipoCompetencia", idTipoCompetencia)
                };

                objDatos.EjecutarSQLconSP_Void("sp_TipoCompetencias_CRUD", parametros);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el Tipo de Competencia: " + ex.Message);
            }
        }

    }
}
