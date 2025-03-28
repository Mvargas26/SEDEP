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
   public class FuncionarioXConglomeradoNegocios
    {
        //***************** VARIABLES *******************
        SQLServerContext_Datos objDatos = new SQLServerContext_Datos();

        public FuncionarioXConglomeradoModel ConsultarFuncionarioXConglomerado_ID(int idFuncXConglo)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Accion", "CONSULTAID"),
                    new SqlParameter("@IdFuncXConglo", idFuncXConglo)
                };

                DataTable dt = objDatos.EjecutarSQLconSP_DT("adm.sp_CrudFuncionarioXConglomerado", parametros);

                if (dt.Rows.Count == 0)
                    return null;

                DataRow row = dt.Rows[0];

                return new FuncionarioXConglomeradoModel
                {
                    IdFuncXConglo = Convert.ToInt32(row["IdFuncXConglo"]),
                    IdFuncionario = row["IdFuncionario"].ToString(),
                    IdConglomerado = Convert.ToInt32(row["IdConglomerado"])
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Fallo en FuncionarioXConglomerado Negocios " + ex.Message);
            }
        }//fin ConsultarFuncionarioXConglomerado_ID

        public List<FuncionarioXConglomeradoModel> ListarFuncionarioXConglomerado()
        {
            List<FuncionarioXConglomeradoModel> lista = new();

            try
            {
                List<SqlParameter> parametros = new()
        {
            new SqlParameter("@Accion", "SELECT")
        };

                DataTable dt = objDatos.EjecutarSQLconSP_DT("[adm].[sp_CrudFuncionarioXConglomerado]", parametros);

                foreach (DataRow row in dt.Rows)
                {
                    lista.Add(new FuncionarioXConglomeradoModel
                    {
                        IdFuncXConglo = Convert.ToInt32(row["IdFuncXConglo"]),
                        IdFuncionario = row["IdFuncionario"].ToString(),
                        IdConglomerado = Convert.ToInt32(row["IdConglomerado"])
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Fallo en FuncionarioXConglomerado Negocios " + ex.Message);
            }

            return lista;
        }//fin ListarFuncionarioXConglomerado

        public List<FuncionarioXConglomeradoModel> ListarFuncionarioXConglomeradoxIDfuncionario(string idFuncionario)
        {
            List<FuncionarioXConglomeradoModel> lista = new();

            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Accion", "CONSULTAIDFUNCIONARIO"),
                    new SqlParameter("@IdFuncionario", idFuncionario)
                };

                DataTable dt = objDatos.EjecutarSQLconSP_DT("[adm].[sp_CrudFuncionarioXConglomerado]", parametros);

                foreach (DataRow row in dt.Rows)
                {
                    lista.Add(new FuncionarioXConglomeradoModel
                    {
                        IdFuncXConglo = Convert.ToInt32(row["IdFuncXConglo"]),
                        IdFuncionario = row["IdFuncionario"].ToString(),
                        IdConglomerado = Convert.ToInt32(row["IdConglomerado"])
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Fallo en FuncionarioXConglomerado Negocios " + ex.Message);
            }

            return lista;
        }//fin ListarFuncionarioXConglomeradoxIDfuncionario

        public void CrearFuncionarioXConglomerado(FuncionarioXConglomeradoModel nuevoFuncionarioXConglo)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Accion", "INSERT"),
                    new SqlParameter("@IdFuncionario", nuevoFuncionarioXConglo.IdFuncionario),
                    new SqlParameter("@IdConglomerado", nuevoFuncionarioXConglo.IdConglomerado)
                };

                objDatos.EjecutarSQLconSP_Void("[adm].[sp_CrudFuncionarioXConglomerado]", parametros);
            }
            catch (Exception ex)
            {
                throw new Exception("Fallo en FuncionarioXConglomerado Negocios " + ex.Message);
            }
        }//fin CrearFuncionarioXConglomerado

        public void ModificarFuncionarioXConglomerado(FuncionarioXConglomeradoModel funcionarioXConglo)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Accion", "UPDATE"),
                    new SqlParameter("@IdFuncXConglo", funcionarioXConglo.IdFuncXConglo),
                    new SqlParameter("@IdFuncionario", funcionarioXConglo.IdFuncionario),
                    new SqlParameter("@IdConglomerado", funcionarioXConglo.IdConglomerado)
                };

                objDatos.EjecutarSQLconSP_Void("[adm].[sp_CrudFuncionarioXConglomerado]", parametros);
            }
            catch (Exception ex)
            {
                throw new Exception("Fallo en FuncionarioXConglomerado Negocios " + ex.Message);
            }
        }//fin ModificarFuncionarioXConglomerado

        public void EliminarFuncionarioXConglomerado(int idFuncXConglo)
        {
            try
            {
                List<SqlParameter> parametros = new()
                {
                    new SqlParameter("@Accion", "DELETE"),
                    new SqlParameter("@IdFuncXConglo", idFuncXConglo)
                };

                objDatos.EjecutarSQLconSP_Void("[adm].[sp_CrudFuncionarioXConglomerado]", parametros);
            }
            catch (Exception ex)
            {
                throw new Exception("Fallo en FuncionarioXConglomerado Negocios " + ex.Message);
            }
        }//fin EliminarFuncionarioXConglomerado

    }//fin class
}//fin space
