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
    public class ConglomeradosNegocios
    {
       //***************** VARIABLES *******************
        SQLServerContext_Datos objDatos = new SQLServerContext_Datos();

        public ConglomeradoModel ConsultarConglomeradoID(int id)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                new SqlParameter("@Operacion", "SELECT"),
                new SqlParameter("@idConglomerado", id)
                };

                DataTable dt = objDatos.EjecutarSQLconSP_DT("adm.sp_Conglomerados_CRUD", parametros);

                if (dt.Rows.Count == 0)
                    return null;

                DataRow row = dt.Rows[0];

                return new ConglomeradoModel
                {
                    IdConglomerado = Convert.ToInt32(row["idConglomerado"]),
                    NombreConglomerado = row["nombreConglomerado"].ToString(),
                    Descripcion = row["Descripcion"].ToString()
                };
            }
            catch (Exception ex)
            {

                throw new Exception("Fallo en Conglomerado Negocios " + ex);
            }
        }//fin ConsultarConglomeradoID

        public List<ConglomeradoModel> ListarConglomerados()
        {
            List<ConglomeradoModel> lista = new();

            try
            {
                List<SqlParameter> parametros = new()
            {
                new SqlParameter("@Operacion", "SELECT"),
                new SqlParameter("@idConglomerado", DBNull.Value)
            };

                DataTable dt = objDatos.EjecutarSQLconSP_DT("adm.sp_Conglomerados_CRUD", parametros);
                
                foreach (DataRow row in dt.Rows)
                {
                    lista.Add(new ConglomeradoModel
                    {
                        IdConglomerado = Convert.ToInt32(row["idConglomerado"]),
                        NombreConglomerado = row["nombreConglomerado"].ToString(),
                        Descripcion = row["Descripcion"].ToString()
                    });
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Fallo en Conglomerado Negocios " + ex);
            }

            return lista;
        }//fin ListarConglomerados

        public void CrearConglomerado(ConglomeradoModel conglomerado)
        {
            try
            {
                List<SqlParameter> parametros = new()
            {
                new SqlParameter("@Operacion", "INSERT"),
                new SqlParameter("@nombreConglomerado", conglomerado.NombreConglomerado),
                new SqlParameter("@Descripcion", conglomerado.Descripcion)
            };

                objDatos.EjecutarSQLconSP_Void("adm.sp_Conglomerados_CRUD", parametros);
            }
            catch (Exception ex)
            {

                throw new Exception("Fallo en Conglomerado Negocios " + ex);
            }
        }//fin CrearConglomerado

        public void ModificarConglomerado(ConglomeradoModel conglomerado)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                new SqlParameter("@Operacion", "UPDATE"),
                new SqlParameter("@idConglomerado", conglomerado.IdConglomerado),
                new SqlParameter("@nombreConglomerado", conglomerado.NombreConglomerado),
                new SqlParameter("@Descripcion", conglomerado.Descripcion)
                };

                objDatos.EjecutarSQLconSP_Void("adm.sp_Conglomerados_CRUD", parametros);
            }
            catch (Exception ex)
            {

                throw new Exception("Fallo en Conglomerado Negocios " + ex);
            }
        }//fin ModificarConglomerado

        public void EliminarConglomerado(int id)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                new SqlParameter("@Operacion", "D"),
                new SqlParameter("@idConglomerado", id)
                };

                objDatos.EjecutarSQLconSP_Void("adm.sp_Conglomerados_CRUD", parametros);
            }
            catch (Exception ex)
            {

                throw new Exception("Fallo en Conglomerado Negocios " + ex);
            }
        }//fin EliminarConglomerado

        public List<PesosConglomeradoModel> ConsultarPesosXConglomerado(int id)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@conglomeradoID", id)
                };

                DataTable dt = objDatos.EjecutarSQLconSP_DT("SP_PesosXConglomerado", parametros);

                List<PesosConglomeradoModel> listaPesos = new();

                foreach (DataRow row in dt.Rows)
                {
                    listaPesos.Add(new PesosConglomeradoModel
                    {
                        IdPesoXConglomerado = Convert.ToInt32(row["idPesoXConglomerado"]),
                        IdConglomerado = Convert.ToInt32(row["idConglomerado"]),
                        IdTipoObjetivo = row["idTipoObjetivo"] as int?,
                        IdTipoCompetencia = row["idTipoCompetencia"] as int?,
                        Porcentaje = Convert.ToDecimal(row["Porcentaje"])
                    });
                }

                return listaPesos;
            }
            catch (Exception ex)
            {
                throw new Exception("Fallo en PesosConglomerado Negocios " + ex);
            }
        }//fin ConsultarPesosXConglomerado

        public List<FuncionarioXConglomeradoModel> ConsultarConglomeradoXFuncionario(string idFuncionario)
        {
            try
            {
                List<SqlParameter> parametros = new()
        {
            new SqlParameter("@idFuncionario", idFuncionario)
        };

                DataTable dt = objDatos.EjecutarSQLconSP_DT("SP_ConsultarConglomeradoXFuncionario", parametros);

                List<FuncionarioXConglomeradoModel> listaFuncionarios = new();

                foreach (DataRow row in dt.Rows)
                {
                    listaFuncionarios.Add(new FuncionarioXConglomeradoModel
                    {
                        IdFuncXConglo = Convert.ToInt32(row["idFuncXConglo"]),
                        IdFuncionario = row["idFuncionario"].ToString(),
                        IdConglomerado = Convert.ToInt32(row["idConglomerado"])
                    });
                }

                return listaFuncionarios;
            }
            catch (Exception ex)
            {
                throw new Exception("Fallo en ConsultarConglomeradoXFuncionario " + ex);
            }
        }


        public string pruebaConexion()
        {
            string mensaje = "";
            try
            {

                objDatos.pruebaCon();

                mensaje = "Conexion Exitosa";
            }
            catch (Exception)
            {
                mensaje = "Conexion fallo";
                throw;
            }

            return mensaje;
        }
    }//fin class
}//fin space
