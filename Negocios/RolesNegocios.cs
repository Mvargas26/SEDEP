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
    public class RolesNegocios
    {
        //***************** VARIABLES *******************
        SQLServerContext_Datos objDatos = new SQLServerContext_Datos();

        //***************** MÉTODOS *********************

        public RolesModel ConsultarRolID(int id)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@accion", "Consultar"),
                    new SqlParameter("@idRol", id)
                };

                DataTable dt = objDatos.EjecutarSQLconSP_DT("sp_RolesCRUD", parametros);

                if (dt.Rows.Count == 0)
                    return null;

                DataRow row = dt.Rows[0];

                return new RolesModel
                {
                    idRol = Convert.ToInt32(row["idRol"]),
                    Rol = row["Rol"].ToString()
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Fallo en RolesNegocios.ConsultarRolID: " + ex.Message);
            }
        }//fin ConsultarRolID

        public List<RolesModel> ListarRoles()
        {
            try
            {
                List<SqlParameter> parametros = new()
            {
                new SqlParameter("@accion", "Listar")
            };

                DataTable dt = objDatos.EjecutarSQLconSP_DT("sp_RolesCRUD", parametros);
                List<RolesModel> lista = new();

                foreach (DataRow row in dt.Rows)
                {
                    lista.Add(new RolesModel
                    {
                        idRol = Convert.ToInt32(row["idRol"]),
                        Rol = row["Rol"].ToString()
                    });
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Fallo en RolesNegocios.ListarRoles: " + ex.Message);
            }
        }//fin

        public int CrearRol(RolesModel rol)
        {
            try
            {
                List<SqlParameter> parametros = new()
            {
                new SqlParameter("@accion", "Crear"),
                new SqlParameter("@Rol", rol.Rol)
            };

                DataTable dt = objDatos.EjecutarSQLconSP_DT("sp_RolesCRUD", parametros);

                if (dt.Rows.Count > 0 && dt.Rows[0]["idRol"] != DBNull.Value)
                {
                    return Convert.ToInt32(dt.Rows[0]["idRol"]);
                }

                throw new Exception("No se pudo obtener el ID del nuevo rol.");
            }
            catch (Exception ex)
            {
                throw new Exception("Fallo en RolesNegocios.CrearRol: " + ex.Message);
            }
        }//fin

        public bool ActualizarRol(RolesModel rol)
        {
            try
            {
                List<SqlParameter> parametros = new()
            {
                new SqlParameter("@accion", "Modificar"),
                new SqlParameter("@idRol", rol.idRol),
                new SqlParameter("@Rol", rol.Rol)
            };

                DataTable dt = objDatos.EjecutarSQLconSP_DT("sp_RolesCRUD", parametros);

                // Si llegamos aquí sin excepciones, consideramos que fue exitoso
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Fallo en RolesNegocios.ActualizarRol: " + ex.Message);
            }
        }//fin

        public bool EliminarRol(int idRol)
        {
            try
            {
                List<SqlParameter> parametros = new()
            {
                new SqlParameter("@accion", "Eliminar"),
                new SqlParameter("@idRol", idRol)
            };

                DataTable dt = objDatos.EjecutarSQLconSP_DT("sp_RolesCRUD", parametros);

                // Si llegamos aquí sin excepciones, consideramos que fue exitoso
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Fallo en RolesNegocios.EliminarRol: " + ex.Message);
            }
        }//fin
    }
}//fin space
