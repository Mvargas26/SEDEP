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
    public class MetaXObjetivoNegocios
    {
        //***************** VARIABLES *******************
        SQLServerContext_Datos objDatos = new SQLServerContext_Datos();

        public MetaXObjetivoModel ConsultarMetaXObjetivoID(int id)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Operacion", "R"),
                    new SqlParameter("@idMetaXObj", id)
                };

                DataTable dt = objDatos.EjecutarSQLconSP_DT("metaXObjetivo_CRUD", parametros);

                if (dt.Rows.Count == 0)
                    return null;

                DataRow row = dt.Rows[0];

                return new MetaXObjetivoModel
                {
                    IdMetaXObj = Convert.ToInt32(row["idMetaXObj"]),
                    IdObjetivo = Convert.ToInt32(row["idObjetivo"]),
                    IdMeta = Convert.ToInt32(row["idMeta"]),
                    ValorObtenido = Convert.ToDecimal(row["valorObtenido"])
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error al consultar MetaXObjetivo por ID: " + ex.Message);
            }
        }//fin ConsultarMetaXObjetivoID

        public List<MetaXObjetivoModel> ListarMetaXObjetivos()
        {
            List<MetaXObjetivoModel> lista = new();

            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Operacion", "R"),
                    new SqlParameter("@idMetaXObj", DBNull.Value)
                };

                DataTable dt = objDatos.EjecutarSQLconSP_DT("metaXObjetivo_CRUD", parametros);

                foreach (DataRow row in dt.Rows)
                {
                    lista.Add(new MetaXObjetivoModel
                    {
                        IdMetaXObj = Convert.ToInt32(row["idMetaXObj"]),
                        IdObjetivo = Convert.ToInt32(row["idObjetivo"]),
                        IdMeta = Convert.ToInt32(row["idMeta"]),
                        ValorObtenido = Convert.ToDecimal(row["valorObtenido"])
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar MetaXObjetivos: " + ex.Message);
            }

            return lista;
        }//fin ListarMetaXObjetivos

        public int CrearMetaXObjetivo(MetaXObjetivoModel modelo)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Operacion", "C"),
                    new SqlParameter("@idObjetivo", modelo.IdObjetivo),
                    new SqlParameter("@idMeta", modelo.IdMeta),
                    new SqlParameter("@valorObtenido", modelo.ValorObtenido)
                };

                DataTable dt = objDatos.EjecutarSQLconSP_DT("metaXObjetivo_CRUD", parametros);

                if (dt.Rows.Count > 0)
                    return Convert.ToInt32(dt.Rows[0]["idMetaXObj"]);

                return 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear MetaXObjetivo: " + ex.Message);
            }
        }//fin CrearMetaXObjetivo

        public bool ActualizarMetaXObjetivo(MetaXObjetivoModel modelo)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Operacion", "U"),
                    new SqlParameter("@idMetaXObj", modelo.IdMetaXObj),
                    new SqlParameter("@idObjetivo", modelo.IdObjetivo),
                    new SqlParameter("@idMeta", modelo.IdMeta),
                    new SqlParameter("@valorObtenido", modelo.ValorObtenido)
                };

                objDatos.EjecutarSQLconSP_DT("metaXObjetivo_CRUD", parametros);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar MetaXObjetivo: " + ex.Message);
            }
        }//fn ActualizarMetaXObjetivo

        public bool EliminarMetaXObjetivo(int id)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Operacion", "D"),
                    new SqlParameter("@idMetaXObj", id)
                };

                objDatos.EjecutarSQLconSP_DT("metaXObjetivo_CRUD", parametros);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar MetaXObjetivo: " + ex.Message);
            }
        }//fin EliminarMetaXObjetivo


    }//fin class
}//fn space
