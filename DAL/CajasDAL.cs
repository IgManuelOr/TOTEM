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
    public class CajasDAL
    {
        //Instanciar conexión con la base de datos(DAL)
        private Acceso acceso = new Acceso();

        //Instanciar conexión con el DAL de ofertas
        //OfertasDAL GestorOfertas = new OfertasDAL();

        //--------------------------------------------------------------------------Acciones base para el ABM------------------------------------------------------------------------------------\\

        public List<Caja> GetCajas (int IdCaja)
        {
            //carga de parametros con los datos a ser inseridos
            List<SqlParameter> parametros = new List<SqlParameter>();

            if (IdCaja != 0)
            {
                parametros.Add(acceso.CrearParametro("@IdCaja", IdCaja));
            }

            //Abertura/Cerradura de la conexión
            acceso.AbrirConexion();
            DataTable tabla = acceso.Leer("sp_GetCajas", parametros);
            acceso.CerrarConexion();

            //Instanciar un listado para return
            List<Caja> ListadoCajas = new List<Caja>();

            foreach (DataRow registro in tabla.Rows)
            {
                Caja caja = new Caja();

                //Lectura de datos a un servicio localmente armazenado
                caja.idCaja = int.Parse(registro["IdCaja"].ToString());
                caja.estado = int.Parse(registro["Estado"].ToString());
                caja.clientes = AsociarClientes(caja).Where(c => c.estado != 3).Count();
                caja.atendidos = AsociarClientes(caja).Where(c => c.estado == 3).Count();
                caja.fechaApertura = DateTime.Parse(registro["FechaApertura"].ToString());
                caja.operador = registro["Operador"].ToString();
                caja.modelo = registro["Modelo"].ToString();
                caja.fechaUltima = DateTime.Parse(registro["FechaUltima"].ToString());

                //Lectura de clientes asociados
                caja.clientesAsociados = AsociarClientes(caja);

                //adicionar registro a un listado para return
                ListadoCajas.Add(caja);
            }

            //retornar listado
            return ListadoCajas;
        }

        public int InsertCaja(Caja caja)
        {

            //carga de parametros con los datos a ser inseridos
            List<SqlParameter> parametros = new List<SqlParameter>();

            //Se asocia el estado 1 (Abierto) en la apertura de una caja
            parametros.Add(acceso.CrearParametro("@Estado", 1));
            parametros.Add(acceso.CrearParametro("@Clientes", caja.clientes));
            
            //Atendidos empieza en 0
            parametros.Add(acceso.CrearParametro("@Atendidos", 0));

            parametros.Add(acceso.CrearParametro("@Modelo", caja.modelo));
            parametros.Add(acceso.CrearParametro("@Operador", caja.operador));
            //Abrir/cerrar conexión a la base de datos
            acceso.AbrirConexion();
            int filas = acceso.Escribir("sp_insertCaja", parametros);
            acceso.CerrarConexion();

            //retornar numero de filas afectadas (idealmente 1)
            return filas;
        }

        public int UpdateCaja(Caja caja)
        {
            //crear Lista de parametros para uso dentro de la stored procedure
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@IdCaja", caja.idCaja));
            parametros.Add(acceso.CrearParametro("@Estado", caja.estado));
            parametros.Add(acceso.CrearParametro("@Clientes", caja.clientes));
            parametros.Add(acceso.CrearParametro("@Atendidos", caja.atendidos));
            parametros.Add(acceso.CrearParametro("@Operador", caja.operador));
            parametros.Add(acceso.CrearParametro("@Modelo", caja.modelo));

            //Abrir/cerrar conexión a la base de datos
            acceso.AbrirConexion();
            int filas = acceso.Escribir("sp_updateCaja", parametros);
            acceso.CerrarConexion();

            //retornar numero de filas afectadas (idealmente 1)
            return filas;
        }

        public int DeleteCaja(int IdCaja)
        {
            //crear Lista de parametros para uso dentro de la stored procedure
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@IdCaja", IdCaja));

            //Abrir/cerrar conexión a la base de datos
            acceso.AbrirConexion();
            int filas = acceso.Escribir("sp_DeleteCaja", parametros);
            acceso.CerrarConexion();

            //retornar numero de filas afectadas (idealmente 1)
            return filas;
        }
        public int CambiarEstadoCaja (int IdCaja)
        {
            //crear Lista de parametros para uso dentro de la stored procedure
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@IdCaja", IdCaja));

            //Abrir/cerrar conexión a la base de datos
            acceso.AbrirConexion();
            int filas = acceso.Escribir("sp_CambiarEstadoCaja", parametros);
            acceso.CerrarConexion();

            //retornar numero de filas afectadas (idealmente 1)
            return filas;
        }

        public List<Cliente> AsociarClientes (Caja caja)
        {
            //crear Lista de parametros para uso dentro de la stored procedure
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@IdCaja", caja.idCaja));

            //Abrir/cerrar conexión a la base de datos
            acceso.AbrirConexion();
            DataTable tabla = acceso.Leer("sp_GetClientesAsociados", parametros);
            acceso.CerrarConexion();

            //Instanciar un listado para return
            List<Cliente> ListadoCliente = new List<Cliente>();

            foreach (DataRow registro in tabla.Rows)
            {
                Cliente cliente = new Cliente();

                //Lectura de datos a un servicio localmente armazenado
                cliente.idCliente = int.Parse(registro["IdCliente"].ToString());
                cliente.idCaja = int.Parse(registro["IdCaja"].ToString());
                cliente.nombreCliente = registro["NombreCliente"].ToString();
                cliente.estado = int.Parse(registro["Estado"].ToString());
                //adicionar registro a un listado para return
                ListadoCliente.Add(cliente);
            }

            //retornar numero de filas afectadas (idealmente 1)
            return ListadoCliente;
        }
    }
}
