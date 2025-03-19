using Datos;
using Microsoft.Data.SqlClient;
using Modelos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Negocios
{
    public class DepartamentosNegocio
    {
        //***************** VARIABLES *******************
        SQLServerContext_Datos objDatos = new SQLServerContext_Datos();

        // Consultar un departamento por ID
        //public DepartamentoModel ConsultarDepartamentoID(int id)
        //{
        //    List<SqlParameter> parametros = new()
        //    {
        //        new SqlParameter("@idDepartamento", id)
        //    };

        //    DataTable dt = objDatos.EjecutarSQLconSP_DT("sp_ConsultarDepartamentoID", parametros);

        //    if (dt.Rows.Count == 0)
        //        return null;

        //    DataRow row = dt.Rows[0];

        //    return new DepartamentoModel
        //    {
        //        IdDepartamento = Convert.ToInt32(row["idDepartamento"]),
        //        Departamento = row["Departamento"].ToString()
        //    };
        //}//fin ConsultarDepartamentoID

        // Listar todos los departamentos
        public List<DepartamentoModel> ListarDepartamentos()
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Accion", "SELECT")
                };

                DataTable dt = objDatos.EjecutarSQLconSP_DT("adm.sp_CrudDepartamentos", parametros);
                List<DepartamentoModel> lista = new();

                foreach (DataRow row in dt.Rows)
                {
                    lista.Add(new DepartamentoModel
                    {
                        IdDepartamento = Convert.ToInt32(row["idDepartamento"]),
                        Departamento = row["Departamento"].ToString()
                    });
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar los departamentos: " + ex.Message);
            }
        }//ListarDepartamentos

        // Crear un nuevo departamento
        public void CrearDepartamento(DepartamentoModel departamento)
        {
            try
            {
                List<SqlParameter> parametros = new()
            {
                new SqlParameter("@Accion", "INSERT"),
                new SqlParameter("@Departamento", departamento.Departamento)
            };

                objDatos.EjecutarSQLconSP_Void("adm.sp_CrudDepartamentos", parametros);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear el departamento: " + ex.Message);
            }
        }//fin CrearDepartamento

        // Modificar un departamento existente
        public void ModificarDepartamento(DepartamentoModel departamento)
        {
            try
            {
                List<SqlParameter> parametros = new()
        {
            new SqlParameter("@Accion", "UPDATE"),
            new SqlParameter("@idDepartamento", departamento.IdDepartamento),
            new SqlParameter("@Departamento", departamento.Departamento)
        };

                objDatos.EjecutarSQLconSP_Void("adm.sp_CrudDepartamentos", parametros);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al modificar el departamento: " + ex.Message);
            }
        }//fin ModificarDepartamento

        // Eliminar un departamento por ID
        public void EliminarDepartamento(int id)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Accion", "DELETE"),
                    new SqlParameter("@idDepartamento", id)
                };

                objDatos.EjecutarSQLconSP_Void("adm.sp_CrudDepartamentos", parametros);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el departamento: " + ex.Message);
            }
        }//fin EliminarDepartamento

    } //Fin Clase
} //Fin Space
