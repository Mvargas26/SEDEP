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
        private readonly SQLServerContext_Datos _datos;


        public ConglomeradosNegocios(SQLServerContext_Datos datos)
        {
            _datos = datos;
        }

        public ConglomeradoModel ConsultarConglomeradoID(int id)
        {
            List<SqlParameter> parametros = new()
            {
                new SqlParameter("@idConglomerado", id)
            };

            DataTable dt = _datos.EjecutarSQLconSP_DT("sp_ConsultarConglomeradoID", parametros);

            if (dt.Rows.Count == 0)
                return null;

            DataRow row = dt.Rows[0];

            return new ConglomeradoModel
            {
                IdConglomerado = Convert.ToInt32(row["idConglomerado"]),
                NombreConglomerado = row["nombreConglomerado"].ToString(),
                Descripcion = row["Descripcion"].ToString()
            };
        }//fin ConsultarConglomeradoID

        public List<ConglomeradoModel> ListarConglomerados()
        {
            DataTable dt = _datos.EjecutarSQLconSP_DT("sp_ListarConglomerados", new List<SqlParameter>());
            List<ConglomeradoModel> lista = new();

            foreach (DataRow row in dt.Rows)
            {
                lista.Add(new ConglomeradoModel
                {
                    IdConglomerado = Convert.ToInt32(row["idConglomerado"]),
                    NombreConglomerado = row["nombreConglomerado"].ToString(),
                    Descripcion = row["Descripcion"].ToString()
                });
            }

            return lista;
        }//fin ListarConglomerados

        public void CrearConglomerado(ConglomeradoModel conglomerado)
        {
            List<SqlParameter> parametros = new()
            {
                new SqlParameter("@nombreConglomerado", conglomerado.NombreConglomerado),
                new SqlParameter("@Descripcion", conglomerado.Descripcion)
            };

            _datos.EjecutarSQLconSP_Void("sp_CrearConglomerado", parametros);
        }//fin CrearConglomerado 

        public void ModificarConglomerado(ConglomeradoModel conglomerado)
        {
            List<SqlParameter> parametros = new()
            {
                new SqlParameter("@idConglomerado", conglomerado.IdConglomerado),
                new SqlParameter("@nombreConglomerado", conglomerado.NombreConglomerado),
                new SqlParameter("@Descripcion", conglomerado.Descripcion)
            };

            _datos.EjecutarSQLconSP_Void("sp_ModificarConglomerado", parametros);
        }//fin ModificarConglomerado

        public void EliminarConglomerado(int id)
        {
            List<SqlParameter> parametros = new()
            {
                new SqlParameter("@idConglomerado", id)
            };

            _datos.EjecutarSQLconSP_Void("sp_EliminarConglomerado", parametros);
        }//fin class


    }//fin class
}//fin space
