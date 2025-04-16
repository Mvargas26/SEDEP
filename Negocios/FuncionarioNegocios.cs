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
    public class FuncionarioNegocios
    {
        SQLServerContext_Datos objDatos = new SQLServerContext_Datos();

        public string GenerarCodigoSeguridad()
        {
            Random random = new Random();
            string codigo = random.Next(100000, 999999).ToString();
            return codigo;
        }

        public void EstablecerCodigoSeguridad(string cedula, string codigoSeguridad)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Operacion", "U"),
                    new SqlParameter("@Cedula", cedula),
                    new SqlParameter("@CodigoSeguridad", codigoSeguridad)
                };

                objDatos.EjecutarSQLconSP_Void("sp_Funcionarios_CRUD", parametros);
            }
            catch (Exception ex)
            {
                throw new Exception("Fallo al establecer el código de seguridad", ex);
            }
        }

        public FuncionarioModel ConsultarFuncionarioID(string cedula)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Cedula", cedula)
                };

                DataTable dt = objDatos.EjecutarSQLconSP_DT("sp_ConsultarFuncionarioID", parametros);

                if (dt.Rows.Count == 0)
                    return null;

                DataRow row = dt.Rows[0];

                return new FuncionarioModel
                {
                    Cedula = row["cedula"].ToString(),
                    Nombre = row["nombre"].ToString(),
                    Apellido1 = row["apellido1"].ToString(),
                    Apellido2 = row["apellido2"].ToString(),
                    Correo = row["correo"].ToString(),
                    Password = row["password"].ToString(),
                    Departamento = row["Departamento"].ToString(),
                    Rol = row["Rol"].ToString(),
                    Puesto = row["Puesto"].ToString(),
                    Estado = row["Estado"].ToString(),
                    CodigoSeguridad = row["CodigoSeguridad"]?.ToString()
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Fallo en Funcionario Negocios " + ex.Message);
            }
        }

        public List<FuncionarioModel> ListarFuncionarios()
        {
            List<FuncionarioModel> lista = new();

            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Operacion", "R")
                };

                DataTable dt = objDatos.EjecutarSQLconSP_DT("sp_Funcionarios_CRUD", parametros);

                foreach (DataRow row in dt.Rows)
                {
                    lista.Add(new FuncionarioModel
                    {
                        Cedula = row["cedula"].ToString(),
                        Nombre = row["nombre"].ToString(),
                        Apellido1 = row["apellido1"].ToString(),
                        Apellido2 = row["apellido2"].ToString(),
                        Correo = row["correo"].ToString(),
                        Password = row["password"].ToString(),
                        Departamento = row["Departamento"].ToString(),
                        Rol = row["Rol"].ToString(),
                        Puesto = row["Puesto"].ToString(),
                        Estado = row["Estado"].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Fallo en Funcionario Negocios " + ex);
            }
            return lista;
        }

        public void CrearFuncionario(FuncionarioModel funcionarioNuevo)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Operacion", "C"),
                    new SqlParameter("@Cedula", funcionarioNuevo.Cedula),
                    new SqlParameter("@Nombre", funcionarioNuevo.Nombre),
                    new SqlParameter("@Apellido1", funcionarioNuevo.Apellido1),
                    new SqlParameter("@Apellido2", funcionarioNuevo.Apellido2 ?? (object)DBNull.Value),
                    new SqlParameter("@Correo", funcionarioNuevo.Correo),
                    new SqlParameter("@Password", funcionarioNuevo.Password),
                    new SqlParameter("@IdDepartamento", funcionarioNuevo.IdDepartamento),
                    new SqlParameter("@IdRol", funcionarioNuevo.IdRol),
                    new SqlParameter("@IdPuesto", funcionarioNuevo.IdPuesto),
                    new SqlParameter("@IdEstadoFuncionario", funcionarioNuevo.IdEstadoFuncionario)
                };

                objDatos.EjecutarSQLconSP_Void("sp_Funcionarios_CRUD", parametros);

                new AuditoriaFuncionarioNegocio().CrearAuditoria(new AuditoriaFuncionarioModel
                {
                    IdAuditoria = Guid.NewGuid().ToString().Substring(0, 20),
                    IdFuncionario = funcionarioNuevo.Cedula,
                    Accion = "INSERT",
                    FechaHora = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    UsuarioAccion = "Sistema",
                    CampoModificado = "Todos",
                    ValorAnterior = "-",
                    ValorNuevo = "Registro completo"
                });
            }
            catch (Exception ex)
            {
                throw new Exception("Fallo en Funcionario Negocios " + ex);
            }
        }

        public void ModificarFuncionario(FuncionarioModel funcionario)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Operacion", "U"),
                    new SqlParameter("@Cedula", funcionario.Cedula),
                    new SqlParameter("@Nombre", funcionario.Nombre ?? (object)DBNull.Value),
                    new SqlParameter("@Apellido1", funcionario.Apellido1 ?? (object)DBNull.Value),
                    new SqlParameter("@Apellido2", funcionario.Apellido2 ?? (object)DBNull.Value),
                    new SqlParameter("@Correo", funcionario.Correo ?? (object)DBNull.Value),
                    new SqlParameter("@Password", funcionario.Password ?? (object)DBNull.Value),
                    new SqlParameter("@IdDepartamento", funcionario.IdDepartamento),
                    new SqlParameter("@IdRol", funcionario.IdRol),
                    new SqlParameter("@IdPuesto", funcionario.IdPuesto),
                    new SqlParameter("@IdEstadoFuncionario", funcionario.IdEstadoFuncionario)
                };

                objDatos.EjecutarSQLconSP_Void("sp_Funcionarios_CRUD", parametros);

                new AuditoriaFuncionarioNegocio().CrearAuditoria(new AuditoriaFuncionarioModel
                {
                    IdAuditoria = Guid.NewGuid().ToString().Substring(0, 20),
                    IdFuncionario = funcionario.Cedula,
                    Accion = "UPDATE",
                    FechaHora = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    UsuarioAccion = "Sistema",
                    CampoModificado = "Todos",
                    ValorAnterior = "-",
                    ValorNuevo = "Datos modificados"
                });
            }
            catch (Exception ex)
            {
                throw new Exception("Fallo en Funcionario Negocios " + ex);
            }
        }

        public void EliminarFuncionario(string cedula)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Operacion", "D"),
                    new SqlParameter("@Cedula", cedula)
                };

                objDatos.EjecutarSQLconSP_Void("sp_Funcionarios_CRUD", parametros);

                new AuditoriaFuncionarioNegocio().CrearAuditoria(new AuditoriaFuncionarioModel
                {
                    IdAuditoria = Guid.NewGuid().ToString().Substring(0, 20),
                    IdFuncionario = cedula,
                    Accion = "DELETE",
                    FechaHora = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    UsuarioAccion = "Sistema",
                    CampoModificado = "Todos",
                    ValorAnterior = "Registro completo",
                    ValorNuevo = "-"
                });
            }
            catch (Exception ex)
            {
                throw new Exception("Fallo en Funcionario Negocios " + ex);
            }
        }

        public List<FuncionarioModel> ObtenerFuncionariosPorDepartamento(int idDepartamento)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@idDepartamento", idDepartamento)
                };

                DataTable dt = objDatos.EjecutarSQLconSP_DT("adm.sp_FuncionariosXDepartamento", parametros);
                List<FuncionarioModel> lista = new();

                foreach (DataRow row in dt.Rows)
                {
                    lista.Add(new FuncionarioModel
                    {
                        Cedula = row["cedula"].ToString(),
                        Nombre = row["nombre"].ToString(),
                        Apellido1 = row["apellido1"].ToString(),
                        Apellido2 = row["apellido2"]?.ToString(),
                        Correo = row["correo"].ToString(),
                        Password = row["password"].ToString(),
                        IdDepartamento = Convert.ToInt32(row["idDepartamento"]),
                        IdRol = Convert.ToInt32(row["idRol"]),
                        IdPuesto = Convert.ToInt32(row["idPuesto"]),
                        IdEstadoFuncionario = Convert.ToInt32(row["idEstadoFuncionario"])
                    });
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los funcionarios por departamento: " + ex.Message);
            }
        }
    }
}
