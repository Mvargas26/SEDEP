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
    public class TiposObjetivosNegocios
    {
        //***************** VARIABLES *******************
        SQLServerContext_Datos objDatos = new SQLServerContext_Datos();

        public TiposObjetivosModel ConsultarTipoObjetivoX_ID(int id)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                new SqlParameter("@Operacion", "R"),
                new SqlParameter("@idTipoObjetivo", id)
                };

                DataTable dt = objDatos.EjecutarSQLconSP_DT("sp_TipoObjetivos_CRUD", parametros);

                if (dt.Rows.Count == 0)
                    return null;

                DataRow row = dt.Rows[0];

                return new TiposObjetivosModel
                {
                    IdTipoObjetivo = Convert.ToInt32(row["idTipoObjetivo"]),
                    Tipo = row["Tipo"].ToString()
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Fallo en TiposObjetivos Negocios " + ex);
            }
        }//fin ConsultarTipoObjetivoX_ID

        public List<TiposObjetivosModel> ListarTiposObjetivos()
        {
            List<TiposObjetivosModel> lista = new();

            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Operacion", "R"),
                    new SqlParameter("@idTipoObjetivo", DBNull.Value)
                };

                DataTable dt = objDatos.EjecutarSQLconSP_DT("adm.sp_TipoObjetivos_CRUD", parametros);

                foreach (DataRow row in dt.Rows)
                {
                    lista.Add(new TiposObjetivosModel
                    {
                        IdTipoObjetivo = Convert.ToInt32(row["idTipoObjetivo"]),
                        Tipo = row["Tipo"].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar tipos de objetivos: " + ex.Message, ex);
            }

            return lista;
        }//fn ListarTiposObjetivos
        public void CrearTipoObjetivo(TiposObjetivosModel tipoObjetivoNuevo)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Operacion", "C"),
                    new SqlParameter("@Tipo", tipoObjetivoNuevo.Tipo)
                };

                objDatos.EjecutarSQLconSP_Void("adm.sp_TipoObjetivos_CRUD", parametros);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear el tipo de objetivo: " + ex.Message, ex);
            }
        }//fin CrearTipoObjetivo
        public void ModificarTipoObjetivo(TiposObjetivosModel tipoObjetivo)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Operacion", "U"),
                    new SqlParameter("@idTipoObjetivo", tipoObjetivo.IdTipoObjetivo),
                    new SqlParameter("@Tipo", tipoObjetivo.Tipo)
                };

                objDatos.EjecutarSQLconSP_Void("adm.sp_TipoObjetivos_CRUD", parametros);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al modificar el tipo de objetivo: " + ex.Message, ex);
            }
        }//fn ModificarTipoObjetivo
        public void EliminarTipoObjetivo(int idTipoObjetivo)
        {
            try
            {
                List<SqlParameter> parametros = new()
        {
            new SqlParameter("@Operacion", "D"),
            new SqlParameter("@idTipoObjetivo", idTipoObjetivo)
        };

                objDatos.EjecutarSQLconSP_Void("adm.sp_TipoObjetivos_CRUD", parametros);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el tipo de objetivo: " + ex.Message, ex);
            }
        }//fin EliminarTipoObjetivo

    }//fin class
}//fin space
