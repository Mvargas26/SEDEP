﻿using Datos;
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
        //***************** VARIABLES *******************
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
            new SqlParameter("@Accion", "UPDATE"),
            new SqlParameter("@Cedula", cedula),
            new SqlParameter("@CodigoSeguridad", codigoSeguridad),
            new SqlParameter("@MensajeError", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output }
        };

                objDatos.EjecutarSQLconSP_Void("adm.sp_CrudFuncionarios", parametros);
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
            new SqlParameter("@Accion", "CONSULTAID"),
            new SqlParameter("@Cedula", cedula),
            new SqlParameter("@MensajeError", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output } // Parámetro de salida
        };

                DataTable dt = objDatos.EjecutarSQLconSP_DT("adm.sp_CrudFuncionarios", parametros);

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
                    CodigoSeguridad = row["CodigoSeguridad"]?.ToString(),
                    IdDepartamento = Convert.ToInt32(row["idDepartamento"])
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Fallo en Funcionario Negocios " + ex.Message);
            }
        }//fin ConsultarFuncionarioID


        public List<FuncionarioModel> ListarFuncionarios()
        {
            List<FuncionarioModel> lista = new();

            try
            {
                List<SqlParameter> parametros = new()
            {
                new SqlParameter("@Accion", "SELECT"),
                new SqlParameter("@MensajeError", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output } // Parámetro de salida
            };

                DataTable dt = objDatos.EjecutarSQLconSP_DT("[adm].[sp_CrudFuncionarios]", parametros);

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
        }//fn ListarFuncionarios


        public void CrearFuncionario(FuncionarioModel funcionarioNuevo)
        {
            try
            {
                List<SqlParameter> parametros = new()
            {
                new SqlParameter("@Accion", "INSERT"),
                new SqlParameter("@Cedula", funcionarioNuevo.Cedula),
                new SqlParameter("@Nombre", funcionarioNuevo.Nombre),
                new SqlParameter("@Apellido1", funcionarioNuevo.Apellido1),
                new SqlParameter("@Apellido2", funcionarioNuevo.Apellido2 ?? (object)DBNull.Value),
                new SqlParameter("@Correo", funcionarioNuevo.Correo),
                new SqlParameter("@Password", funcionarioNuevo.Password),
                new SqlParameter("@IdDepartamento", funcionarioNuevo.IdDepartamento),
                new SqlParameter("@IdRol", funcionarioNuevo.IdRol),
                new SqlParameter("@IdPuesto", funcionarioNuevo.IdPuesto),
                new SqlParameter("@IdEstadoFuncionario", funcionarioNuevo.IdEstadoFuncionario),
                new SqlParameter("@MensajeError", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output }
            };

                objDatos.EjecutarSQLconSP_Void("[adm].[sp_CrudFuncionarios]", parametros);
            }
            catch (Exception ex)
            {

                throw new Exception("Fallo en Funcionario Negocios " + ex);
            }
        }//fin CrearFuncionario


        public void ModificarFuncionario(FuncionarioModel funcionario)
        {
            try
            {
                List<SqlParameter> parametros = new()
            {
                new SqlParameter("@Accion", "UPDATE"),
                new SqlParameter("@Cedula", funcionario.Cedula),
                new SqlParameter("@Nombre", funcionario.Nombre ?? (object)DBNull.Value),
                new SqlParameter("@Apellido1", funcionario.Apellido1 ?? (object)DBNull.Value),
                new SqlParameter("@Apellido2", funcionario.Apellido2 ?? (object)DBNull.Value),
                new SqlParameter("@Correo", funcionario.Correo ?? (object)DBNull.Value),
                new SqlParameter("@Password", funcionario.Password ?? (object)DBNull.Value),
                new SqlParameter("@IdDepartamento", funcionario.IdDepartamento) ,
                new SqlParameter("@IdRol", funcionario.IdRol) ,
                new SqlParameter("@IdPuesto", funcionario.IdPuesto) ,
                new SqlParameter("@IdEstadoFuncionario", funcionario.IdEstadoFuncionario),
                new SqlParameter("@MensajeError", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output }
            };

                objDatos.EjecutarSQLconSP_Void("[adm].[sp_CrudFuncionarios]", parametros);
            }
            catch (Exception ex)
            {
                throw new Exception("Fallo en Funcionario Negocios " + ex);
            }
        }//fin ModificarFuncionario

        public void EliminarFuncionario(string cedula)
        {
            try
            {
                List<SqlParameter> parametros = new()
            {
                new SqlParameter("@Accion", "DELETE"),
                new SqlParameter("@Cedula", cedula),
                new SqlParameter("@MensajeError", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output }
            };
                objDatos.EjecutarSQLconSP_Void("[adm].[sp_CrudFuncionarios]", parametros);

                // Captura el mensaje de error o éxito
                string mensajeError = parametros.FirstOrDefault(p => p.ParameterName == "@MensajeError")?.Value?.ToString();

                // Lanza una excepción si el mensaje es un error
                if (mensajeError.Contains("No"))
                {
                    throw new Exception(mensajeError);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }//fin EliminarFuncionario

        public List<FuncionarioModel> ObtenerFuncionariosPorDepartamento(int idDepartamento)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@idDepartamento", idDepartamento),
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
                        Apellido2 = row["apellido2"]?.ToString(), // Manejo de nulos
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

        public List<FuncionarioModel> Obt_Func_ConEvaluacionXAprobarXDepart(int idDepartamento)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@idDepartamento", idDepartamento)
                };

                DataTable dt = objDatos.EjecutarSQLconSP_DT("adm.sp_funcionariosConEvalucionXAprobar", parametros);
                List<FuncionarioModel> lista = new();

                foreach (DataRow row in dt.Rows)
                {
                    lista.Add(new FuncionarioModel
                    {
                        Cedula = row["cedula"].ToString(),
                        Nombre = row["nombre"].ToString(),
                        Apellido1 = row["apellido1"].ToString(),
                        Apellido2 = row["apellido2"]?.ToString(), // Manejo de nulos
                        Correo = row["correo"].ToString(),
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
        // para ver todos los estados posibles de funcionario 
        public List<EstadoFuncionarioModel> ListarEstadosFuncionario()
        {
            List<EstadoFuncionarioModel> lista = new();

            try
            {
               
                List<SqlParameter> parametros = new()
        {
            new SqlParameter("@Accion", "SELECT")
            
        };

                DataTable dt = objDatos.EjecutarSQLconSP_DT("dbo.sp_CrudEstadoFuncionarios", parametros);

                foreach (DataRow row in dt.Rows)
                {
                    lista.Add(new EstadoFuncionarioModel
                    {
                        IdEstadoFuncionario = Convert.ToInt32(row["idEstadofunc"]),
                        Estado = row["EstadoFuncionario"].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar estados de funcionario: " + ex.Message);
            }

            return lista;
        }
    }//fn class
}//fn space
