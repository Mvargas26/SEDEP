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
    public class PuestosNegocio
    {
        private readonly SQLServerContext_Datos _contexto;

        public PuestosNegocio()
        {
            _contexto = new SQLServerContext_Datos();
        }

        //Obtener todos
        public List<PuestoModel> ObtenerPuestos()
        {
            List<PuestoModel> lista = new();

            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Accion", "SELECT"),
                    new SqlParameter("@idPuesto", DBNull.Value)
                };

                DataTable dt = _contexto.EjecutarSQLconSP_DT("adm.sp_CrudPuesto", parametros);

                foreach (DataRow row in dt.Rows)
                {
                    lista.Add(new PuestoModel
                    {
                        idPuesto = Convert.ToInt32(row["idPuesto"]),
                        Puesto = row["Puesto"].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar los puestos: " + ex.Message);
            }

            return lista;

        }

        // Obtener por ID
        public PuestoModel ObtenerPuestoId(int id)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Accion", "SELECT"),
                    new SqlParameter("@idPuesto", id)
                };

                DataTable dt = _contexto.EjecutarSQLconSP_DT("adm.sp_CrudPuesto", parametros);

                if (dt.Rows.Count == 0)
                    return null;

                DataRow row = dt.Rows[0];

                return new PuestoModel
                {
                    idPuesto = Convert.ToInt32(row["idPuesto"]),
                    Puesto = row["Puesto"].ToString()
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error al consultar el puesto por ID: " + ex.Message);
            }
        }

        // Agregar
        public void AgregarPuesto(PuestoModel puesto)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Accion", "INSERT"),
                    new SqlParameter("@Puesto", puesto.Puesto)
                };

                _contexto.EjecutarSQLconSP_Void("adm.sp_CrudPuesto", parametros);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear el objetivo: " + ex.Message);
            }
        }

        // Actualizar
        public void ActualizarPuesto(PuestoModel puesto)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                   new SqlParameter("@Accion", "UPDATE"),
                   new SqlParameter("@idPuesto", puesto.idPuesto),
                   new SqlParameter("@Puesto", puesto.Puesto)
                };

                _contexto.EjecutarSQLconSP_Void("adm.sp_CrudPuesto", parametros);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al modificar el puesto: " + ex.Message);
            }
        }

        // Eliminar
        public void EliminarPuesto(int id)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Accion", "DELETE"),
                    new SqlParameter("@idPuesto", id)
                };

                _contexto.EjecutarSQLconSP_Void("adm.sp_CrudPuesto", parametros);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el puesto: " + ex.Message);
            }
        }
    }
}
