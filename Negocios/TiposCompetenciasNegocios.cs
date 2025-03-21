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
        //***************** VARIABLES *******************
        SQLServerContext_Datos objDatos = new SQLServerContext_Datos();

        public List<TiposCompetenciasModel> ListarTiposCompetencias()
        {
            List<TiposCompetenciasModel> lista = new();

            try
            {
                // Definir los parámetros para el SP
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Accion", "SELECT"), // Acción para listar todos los registros
                    new SqlParameter("@idTipoCompetencia", DBNull.Value) // No se necesita un ID específico
                };

                // Ejecutar el SP y obtener los resultados en un DataTable
                DataTable dt = objDatos.EjecutarSQLconSP_DT("dbo.TiposCompetenciasCRUD", parametros);

                // Mapear los datos del DataTable a la lista de modelos
                foreach (DataRow row in dt.Rows)
                {
                    lista.Add(new TiposCompetenciasModel
                    {
                        idTipoCompetencia = Convert.ToInt32(row["idTipoCompetencia"]), 
                        Tipo = row["Tipo"].ToString() 
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar tipos de competencias: " + ex.Message, ex);
            }

            return lista;
        }//fin ListarTiposCompetencias









    }//fn class
}//fin space
