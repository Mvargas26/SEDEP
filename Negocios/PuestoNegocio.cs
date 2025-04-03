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
            List<PuestoModel> listaPuestos = new();
            DataTable dt = _contexto.EjecutarSqlDirecto_DT("SELECT * FROM vw_Puesto");

            foreach (DataRow row in dt.Rows)
            {
                listaPuestos.Add(new PuestoModel
                {
                    idPuesto = Convert.ToInt32(row["idPuesto"]),
                    Puesto = row["Puesto"].ToString()
                });
            }

            return listaPuestos;
        }

        // Obtener por ID
        public PuestoModel ObtenerPuestoId(int id)
        {
            List<SqlParameter> parametros = new()
            {
                new SqlParameter("@idPuesto", id)
            };

            DataTable dt = _contexto.EjecutarSQLconSP_DT("sp_ObtenerPuesto", parametros);

            if (dt.Rows.Count > 0)
            {
                return new PuestoModel
                {
                    idPuesto = Convert.ToInt32(dt.Rows[0]["idPuesto"]),
                    Puesto = dt.Rows[0]["Puesto"].ToString()
                };
            }

            return null;
        }

        // Agregar
        public void AgregarPuesto(PuestoModel puesto)
        {
            List<SqlParameter> parametros = new()
            {
                new SqlParameter("@Puesto", puesto.Puesto)
            };

            _contexto.EjecutarSQLconSP_Void("sp_InsertarPuesto", parametros);
        }

        // Actualizar
        public void ActualizarPuesto(PuestoModel puesto)
        {
            List<SqlParameter> parametros = new()
            {
                new SqlParameter("@idPuesto", puesto.idPuesto),
                new SqlParameter("@Puesto", puesto.Puesto)
            };

            _contexto.EjecutarSQLconSP_Void("sp_ActualizarPuesto", parametros);
        }

        // Eliminar
        public void EliminarPuesto(int id)
        {
            List<SqlParameter> parametros = new()
            {
                new SqlParameter("@idPuesto", id)
            };

            _contexto.EjecutarSQLconSP_Void("sp_EliminarPuesto", parametros);
        }
    }

}
