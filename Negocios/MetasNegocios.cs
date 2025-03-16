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
    class MetasNegocios
    {
        //***************** VARIABLES *******************
        SQLServerContext_Datos objDatos = new SQLServerContext_Datos();

        public MetaModel ConsultarMetaID(int id)
        {
            List<SqlParameter> parametros = new()
            {
                new SqlParameter("@Operacion", "R"),
                new SqlParameter("@idMeta", id)
            };

            DataTable dt = objDatos.EjecutarSQLconSP_DT("sp_Metas_CRUD", parametros);

            if (dt.Rows.Count == 0)
                return null;

            DataRow row = dt.Rows[0];

            return new MetaModel
            {
                IdMeta = Convert.ToInt32(row["idMeta"]),
                Meta = row["Meta"].ToString(),
                Porcentaje = Convert.ToDecimal(row["Porcentaje"])
            };
        }//fin ConsultarMetaID

        public List<MetaModel> ListarMetas()
        {
            List<SqlParameter> parametros = new()
            {
                new SqlParameter("@Operacion", "R"),
                new SqlParameter("@idMeta", DBNull.Value)
            };

            DataTable dt = objDatos.EjecutarSQLconSP_DT("sp_Metas_CRUD", parametros);
            List<MetaModel> lista = new();

            foreach (DataRow row in dt.Rows)
            {
                lista.Add(new MetaModel
                {
                    IdMeta = Convert.ToInt32(row["idMeta"]),
                    Meta = row["Meta"].ToString(),
                    Porcentaje = Convert.ToDecimal(row["Porcentaje"])
                });
            }

            return lista;
        }//fin ListarMetas

        public void CrearMeta(MetaModel metaNueva)
        {
            List<SqlParameter> parametros = new()
            {
                new SqlParameter("@Operacion", "C"),
                new SqlParameter("@Meta", metaNueva.Meta),
                new SqlParameter("@Porcentaje", metaNueva.Porcentaje)
            };

            objDatos.EjecutarSQLconSP_Void("sp_Metas_CRUD", parametros);
        }//fin CrearMeta

        public void ModificarMeta(MetaModel meta)
        {
            List<SqlParameter> parametros = new()
            {
                new SqlParameter("@Operacion", "U"),
                new SqlParameter("@idMeta", meta.IdMeta),
                new SqlParameter("@Meta", meta.Meta),
                new SqlParameter("@Porcentaje", meta.Porcentaje)
            };

            objDatos.EjecutarSQLconSP_Void("sp_Metas_CRUD", parametros);
        }//fin ModificarMeta

        public void EliminarMeta(int idMeta)
        {
            List<SqlParameter> parametros = new()
            {
                new SqlParameter("@Operacion", "D"),
                new SqlParameter("@idMeta", idMeta)
            };

            objDatos.EjecutarSQLconSP_Void("sp_Metas_CRUD", parametros);
        }//fin EliminarMeta

    }//fn class
}//fin space
