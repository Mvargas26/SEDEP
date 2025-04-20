using Datos;
using Modelos;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;

namespace Negocios
{
    public class PesosConglomeradoNegocio
    {
        private readonly SQLServerContext_Datos _context;

        public PesosConglomeradoNegocio()
        {
            _context = new SQLServerContext_Datos();
        }

        public void Crear(PesosConglomeradoModel model)
        {
            var parametros = new List<SqlParameter>
            {
                new SqlParameter("@Operacion", "INSERT"),
                new SqlParameter("@idConglomerado", model.IdConglomerado),
                new SqlParameter("@idTipoObjetivo", (object?)model.IdTipoObjetivo ?? DBNull.Value),
                new SqlParameter("@idTipoCompetencia", (object?)model.IdTipoCompetencia ?? DBNull.Value),
                new SqlParameter("@Porcentaje", model.Porcentaje)
            };

            _context.EjecutarSQLconSP_Void("SP_PesoXConglomeradoCRUD", parametros);
        }

        public void Modificar(PesosConglomeradoModel model)
        {
            var parametros = new List<SqlParameter>
            {
                new SqlParameter("@Operacion", "UPDATE"),
                new SqlParameter("@idPesoXConglomerado", model.IdPesoXConglomerado),
                new SqlParameter("@idConglomerado", model.IdConglomerado),
                new SqlParameter("@idTipoObjetivo", (object?)model.IdTipoObjetivo ?? DBNull.Value),
                new SqlParameter("@idTipoCompetencia", (object?)model.IdTipoCompetencia ?? DBNull.Value),
                new SqlParameter("@Porcentaje", model.Porcentaje)
            };

            _context.EjecutarSQLconSP_Void("SP_PesoXConglomeradoCRUD", parametros);
        }

        public void Eliminar(int id)
        {
            var parametros = new List<SqlParameter>
            {
                new SqlParameter("@Operacion", "DELETE"),
                new SqlParameter("@idPesoXConglomerado", id)
            };

            _context.EjecutarSQLconSP_Void("SP_PesoXConglomeradoCRUD", parametros);
        }

        //*********************************************************************************************

        //estos select no se si los vayan a ocupar
        public List<PesosConglomeradoModel> ObtenerTodos()
        {
            List<SqlParameter> parametros = new()
            {
                new SqlParameter("@Operacion", "SELECT")
            };

            DataTable dt = _context.EjecutarSQLconSP_DT("SP_PesoXConglomeradoCRUD", parametros);
            List<PesosConglomeradoModel> lista = new();

            foreach (DataRow row in dt.Rows)
            {
                lista.Add(new PesosConglomeradoModel
                {
                    IdPesoXConglomerado = Convert.ToInt32(row["idPesoXConglomerado"]),
                    IdConglomerado = Convert.ToInt32(row["idConglomerado"]),
                    IdTipoObjetivo = Convert.ToInt32(row["idTipoObjetivo"]),
                    IdTipoCompetencia = Convert.ToInt32(row["idTipoCompetencia"]),
                    Porcentaje = Convert.ToDecimal(row["Porcentaje"])
                });
            }

            return lista;
        }

        public PesosConglomeradoModel ConsultarPorID(int id)
        {
            List<SqlParameter> parametros = new()
            {
                new SqlParameter("@Operacion", "SELECT"),
                new SqlParameter("@idPesoXConglomerado", id)
            };

            DataTable dt = _context.EjecutarSQLconSP_DT("SP_PesoXConglomeradoCRUD", parametros);
            if (dt.Rows.Count == 0)
                return null;

            DataRow row = dt.Rows[0];
            return new PesosConglomeradoModel
            {
                IdPesoXConglomerado = Convert.ToInt32(row["idPesoXConglomerado"]),
                IdConglomerado = Convert.ToInt32(row["idConglomerado"]),
                IdTipoObjetivo = row["idTipoObjetivo"] != DBNull.Value ? Convert.ToInt32(row["idTipoObjetivo"]) : (int?)null,
                IdTipoCompetencia = row["idTipoCompetencia"] != DBNull.Value ? Convert.ToInt32(row["idTipoCompetencia"]) : (int?)null,
                Porcentaje = Convert.ToDecimal(row["Porcentaje"])
            };
        }

        public List<PesosConglomeradoModel> ListarPorIdConglomerado(int idConglomerado)
        {
            List<SqlParameter> parametros = new()
            {
                new SqlParameter("@Operacion", "SELECT"),
                new SqlParameter("@idConglomerado", idConglomerado)
            };

            DataTable dt = _context.EjecutarSQLconSP_DT("SP_PesoXConglomeradoCRUD", parametros);
            List<PesosConglomeradoModel> lista = new();

            foreach (DataRow row in dt.Rows)
            {
                lista.Add(new PesosConglomeradoModel
                {
                    IdPesoXConglomerado = Convert.ToInt32(row["idPesoXConglomerado"]),
                    IdConglomerado = Convert.ToInt32(row["idConglomerado"]),
                    IdTipoObjetivo = row["idTipoObjetivo"] != DBNull.Value ? Convert.ToInt32(row["idTipoObjetivo"]) : (int?)null,
                    IdTipoCompetencia = row["idTipoCompetencia"] != DBNull.Value ? Convert.ToInt32(row["idTipoCompetencia"]) : (int?)null,
                    Porcentaje = Convert.ToDecimal(row["Porcentaje"])
                });
            }

            return lista;
        }


    }//fn class
}//fin space
