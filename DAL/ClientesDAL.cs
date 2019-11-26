using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Entidades;

namespace DAL
{
    public class ClientesDAL
    {
        //Instanciar conexión con la base de datos(DAL)
        private Acceso acceso = new Acceso();
        private CajasDAL GestorCajas = new CajasDAL();


        public List<Cliente> GetClientes(Cliente cliente, int estado = 1)
        {
            //carga de parametros con los datos a ser inseridos
            List<SqlParameter> parametros = new List<SqlParameter>();

            parametros.Add(acceso.CrearParametro("@IdCliente", cliente.idCliente));
            parametros.Add(acceso.CrearParametro("@IdCaja", cliente.idCaja));


            //Abertura/Cerradura de la conexión
            acceso.AbrirConexion();
            DataTable tabla = acceso.Leer("sp_GetClientes", parametros);
            acceso.CerrarConexion();

            //Instanciar un listado para return
            List<Cliente> ListadoClientes = new List<Cliente>();

            foreach (DataRow registro in tabla.Rows)
            {
                Cliente clientelocal = new Cliente();

                //Lectura de datos a un servicio localmente armazenado
                clientelocal.idCliente = int.Parse(registro["idCliente"].ToString());
                if (registro["IdCaja"].ToString() != DBNull.Value.ToString())
                {
                    clientelocal.idCaja = int.Parse(registro["IdCaja"].ToString());
                }
                clientelocal.nombreCliente = registro["NombreCliente"].ToString();
                clientelocal.estado = int.Parse(registro["Estado"].ToString());
                //adicionar registro a un listado para return
                ListadoClientes.Add(clientelocal);
            }

            //retornar listado
            return ListadoClientes;
        }

        public int InsertCliente(Cliente cliente)
        {

            //carga de parametros con los datos a ser inseridos
            List<SqlParameter> parametros = new List<SqlParameter>();

            //Se asocia el estado 1 (Abierto) en la apertura de una caja
            parametros.Add(acceso.CrearParametro("@NombreCliente", cliente.nombreCliente));

            //Abrir/cerrar conexión a la base de datos
            acceso.AbrirConexion();
            int filas = acceso.Escribir("sp_InsertClientes", parametros);
            acceso.CerrarConexion();

            //retornar numero de filas afectadas (idealmente 1)
            return filas;
        }

        public int UpdateCliente(Cliente cliente)
        {
            cliente.estado = GetClientes(cliente).Where(c => c.idCliente == cliente.idCliente).FirstOrDefault().estado;
            //crear Lista de parametros para uso dentro de la stored procedure
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@IdCliente", cliente.idCliente));
            parametros.Add(acceso.CrearParametro("@NombreCliente", cliente.nombreCliente));
            parametros.Add(acceso.CrearParametro("@IdCaja", cliente.idCaja));
            parametros.Add(acceso.CrearParametro("@Estado", cliente.estado));

            //Abrir/cerrar conexión a la base de datos
            acceso.AbrirConexion();
            int filas = acceso.Escribir("sp_UpdateCliente", parametros);
            acceso.CerrarConexion();

            //retornar numero de filas afectadas (idealmente 1)
            return filas;
        }

        public int DeleteCliente(int IdCliente)
        {
            //crear Lista de parametros para uso dentro de la stored procedure
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@IdCliente", IdCliente));

            //Abrir/cerrar conexión a la base de datos
            acceso.AbrirConexion();
            int filas = acceso.Escribir("sp_DeleteCliente", parametros);
            acceso.CerrarConexion();

            //retornar numero de filas afectadas (idealmente 1)
            return filas;
        }

        public int AsociarClientes() {

            List<Cliente> ListadoClientes = new List<Cliente>();

            int Afectados = 0;

            //Dummy Cliente
            Cliente cliente = new Cliente();
            cliente.idCaja = 0;
            cliente.idCliente = 0;

            ListadoClientes = GetClientes(cliente);
            ListadoClientes = ListadoClientes.Where(c => c.estado == 1).ToList();

            foreach (var clienteAsociar in ListadoClientes)
            {
                try {
                    List<Caja> ListadoCajas = GestorCajas.GetCajas(0).OrderBy(c => c.clientesAsociados.Count()).ToList();
                    clienteAsociar.estado = 2;
                    clienteAsociar.idCaja = ListadoCajas.FirstOrDefault().idCaja;
                    UpdateCliente(clienteAsociar);
                    Afectados++;
                } 
                catch
                {
                    return 0;
                }
            }

            return Afectados;
        }

        public int AtenderCliente (int IdCaja)
        {
            List<Cliente> ListadoClientes = new List<Cliente>();

            int Afectados = 0;

            //Dummy Cliente
            Cliente cliente = new Cliente();
            cliente.idCaja = 0;
            cliente.idCliente = 0;

            ListadoClientes = GetClientes(cliente);
            ListadoClientes = ListadoClientes.Where(c => c.estado == 2).OrderBy(c => c.idCliente).ToList();
            cliente.idCliente = ListadoClientes.Where(c => c.idCaja == IdCaja).FirstOrDefault().idCliente;

            foreach (var clienteAsociar in ListadoClientes)
            {
                try
                {
                    List<Caja> ListadoCajas = GestorCajas.GetCajas(IdCaja);
                    clienteAsociar.estado = 3;
                    clienteAsociar.idCaja = IdCaja;
                    UpdateCliente(clienteAsociar);
                    Afectados++;
                }
                catch
                {
                    return 0;
                }
            }

            return Afectados;
        }
    }
}
