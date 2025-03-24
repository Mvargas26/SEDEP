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
                new SqlParameter("@IdConglomerado", model.IdConglomerado),
                new SqlParameter("@IdTipoObjetivo", (object?)model.IdTipoObjetivo ?? DBNull.Value),
                new SqlParameter("@IdTipoCompetencia", (object?)model.IdTipoCompetencia ?? DBNull.Value),
                new SqlParameter("@Porcentaje", model.Porcentaje)
            };

            _context.EjecutarSQLconSP_Void("sp_CrearPesoPorConglomerado", parametros);
        }

        public void Modificar(PesosConglomeradoModel model)
        {
            var parametros = new List<SqlParameter>
            {
                new SqlParameter("@IdPesoXConglomerado", model.IdPesoXConglomerado),
                new SqlParameter("@IdConglomerado", model.IdConglomerado),
                new SqlParameter("@IdTipoObjetivo", (object?)model.IdTipoObjetivo ?? DBNull.Value),
                new SqlParameter("@IdTipoCompetencia", (object?)model.IdTipoCompetencia ?? DBNull.Value),
                new SqlParameter("@Porcentaje", model.Porcentaje)
            };

            _context.EjecutarSQLconSP_Void("sp_CrearPesoPorConglomerado", parametros);
        }

        public void Eliminar(int id)
        {
            var parametros = new List<SqlParameter>
            {
                new SqlParameter("@IdPesoXConglomerado", id)
            };

            _context.EjecutarSQLconSP_Void("sp_EliminarPesoPorConglomerado", parametros);
        }

        public List<PesosConglomeradoModel> ObtenerTodos()
        {
            var dt = _context.EjecutarSQLconSP_DT("sp_ObtenerPesosPorConglomerado", new List<SqlParameter>());
            var lista = new List<PesosConglomeradoModel>();

            foreach (DataRow row in dt.Rows)
            {
                lista.Add(new PesosConglomeradoModel
                {
                    IdPesoXConglomerado = (int)row["IdPesoXConglomerado"],
                    IdConglomerado = (int)row["IdConglomerado"],
                    IdTipoObjetivo = row["IdTipoObjetivo"] != DBNull.Value ? (int?)row["IdTipoObjetivo"] : null,
                    IdTipoCompetencia = row["IdTipoCompetencia"] != DBNull.Value ? (int?)row["IdTipoCompetencia"] : null,
                    Porcentaje = (decimal)row["Porcentaje"]
                });
            }

            return lista;
        }
    }
}
