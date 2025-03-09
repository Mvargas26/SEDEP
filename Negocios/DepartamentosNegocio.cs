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
        private readonly SQLServerContext_Datos _datos;

        public DepartamentoNegocios(SQLServerContext_Datos datos)
        {
            _datos = datos;
        }

        // Consultar un departamento por ID
        public DepartamentoModel ConsultarDepartamentoID(int id)
        {
            List<SqlParameter> parametros = new()
        {
            new SqlParameter("@idDepartamento", id)
        };

            DataTable dt = _datos.EjecutarSQLconSP_DT("sp_ConsultarDepartamentoID", parametros);

            if (dt.Rows.Count == 0)
                return null;

            DataRow row = dt.Rows[0];

            return new DepartamentoModel
            {
                IdDepartamento = Convert.ToInt32(row["idDepartamento"]),
                Departamento = row["Departamento"].ToString()
            };
        }//fin ConsultarDepartamentoID

        // Listar todos los departamentos
        public List<DepartamentoModel> ListarDepartamentos()
        {
            DataTable dt = _datos.EjecutarSQLconSP_DT("sp_ListarDepartamentos", new List<SqlParameter>());
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
        }//fin ListarDepartamentos

        // Crear un nuevo departamento
        public void CrearDepartamento(DepartamentoModel departamento)
        {
            List<SqlParameter> parametros = new()
        {
            new SqlParameter("@Departamento", departamento.Departamento)
        };

            _datos.EjecutarSQLconSP_Void("sp_CrearDepartamento", parametros);
        }//fin CrearDepartamento 

        // Modificar un departamento existente
        public void ModificarDepartamento(DepartamentoModel departamento)
        {
            List<SqlParameter> parametros = new()
        {
            new SqlParameter("@idDepartamento", departamento.IdDepartamento),
            new SqlParameter("@Departamento", departamento.Departamento)
        };

            _datos.EjecutarSQLconSP_Void("sp_ModificarDepartamento", parametros);
        }//fin ModificarDepartamento

        // Eliminar un departamento por ID
        public void EliminarDepartamento(int id)
        {
            List<SqlParameter> parametros = new()
        {
            new SqlParameter("@idDepartamento", id)
        };

            _datos.EjecutarSQLconSP_Void("sp_EliminarDepartamento", parametros);
        }//fin EliminarDepartamento

    } //Fin Clase
} //Fin Space
