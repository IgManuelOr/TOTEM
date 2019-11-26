using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    internal class Acceso
    {
        private SqlConnection conexion;
        private SqlTransaction transaccion;

        public void AbrirConexion()
        {
            conexion = new SqlConnection();
            conexion.ConnectionString = "Data Source=Localhost;Initial Catalog=TOTEM;Integrated Security=True";
            conexion.Open();
        }

        public void CerrarConexion()
        {
            conexion.Close();
            conexion.Dispose();
            conexion = null;
            GC.Collect();
        }

        public void ComenzarTransaccion()
        {
            if (conexion != null && conexion.State == ConnectionState.Open)
            {
                transaccion = conexion.BeginTransaction();
            }
        }

        public void ConfirmarTransaccion()
        {
            transaccion.Commit();
            transaccion.Dispose();
            transaccion = null;
            GC.Collect();
        }

        public void CancelarTransaccion()
        {
            transaccion.Rollback();
            transaccion.Dispose();
            transaccion = null;
            GC.Collect();
        }

        public SqlCommand CrearComando(string nombre, List<SqlParameter> parametros)
        {
            using (SqlCommand comando = new SqlCommand())
            {
                comando.Connection = conexion;
                comando.Transaction = transaccion;
                comando.CommandType = CommandType.StoredProcedure;
                comando.CommandText = nombre;

                if (parametros != null && parametros.Count > 0)
                {
                    comando.Parameters.AddRange(parametros.ToArray());
                }

                return comando;
            }
        }

        public DataTable Leer(string nombre, List<SqlParameter> parametros)
        {
            DataTable tabla = new DataTable();

            using (SqlDataAdapter adaptador = new SqlDataAdapter())
            {
                adaptador.SelectCommand = CrearComando(nombre, parametros);
                adaptador.Fill(tabla);
            }

            return tabla;
        }

        public int Escribir(string nombre, List<SqlParameter> parametros)
        {
            int filas = 0;

            using (SqlCommand comando = CrearComando(nombre, parametros))
            {
                try
                {
                    filas = comando.ExecuteNonQuery();
                }
                catch
                {
                    filas = -1;
                }

                comando.Dispose();
            }

            return filas;
        }

        public SqlParameter CrearParametro(string nombre, string valor)
        {
            SqlParameter parametro = new SqlParameter();
            parametro.ParameterName = nombre;
            parametro.DbType = DbType.String;
            parametro.Value = valor;
            return parametro;
        }

        public SqlParameter CrearParametro(string nombre, int valor)
        {
            SqlParameter parametro = new SqlParameter();
            parametro.ParameterName = nombre;
            parametro.DbType = DbType.Int32;
            parametro.Value = valor;
            return parametro;
        }

        public SqlParameter CrearParametro(string nombre, float valor)
        {
            SqlParameter parametro = new SqlParameter();
            parametro.ParameterName = nombre;
            parametro.DbType = DbType.Single;
            parametro.Value = valor;
            return parametro;
        }

        public SqlParameter CrearParametro(string nombre, object valor)
        {
            SqlParameter parametro = new SqlParameter();
            parametro.ParameterName = nombre;
            parametro.DbType = DbType.Object;
            parametro.Value = valor;
            return parametro;
        }
    }
}
