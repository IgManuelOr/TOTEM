using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Entidades
{
    public class Caja
    {
        [Display(Name = "Caja")]
        private int IdCaja;

        public int idCaja
        {
            get { return IdCaja; }
            set { IdCaja = value; }
        }

        private int Estado;

        public int estado
        {
            get { return Estado; }
            set { Estado = value; }
        }

        private int Clientes;

        public int clientes
        {
            get { return Clientes; }
            set { Clientes = value; }
        }

        private int Atendidos;

        public int atendidos
        {
            get { return Atendidos; }
            set { Atendidos = value; }
        }

        private DateTime FechaApertura;

        public DateTime fechaApertura
        {
            get { return FechaApertura; }
            set { FechaApertura = value; }
        }

        private string Modelo;

        public string modelo
        {
            get { return Modelo; }
            set { Modelo = value; }
        }

        private string Operador;

        public string operador
        {
            get { return Operador; }
            set { Operador = value; }
        }

        [Display(Name = "Fecha Ultima Apertura")]
        private DateTime FechaUltima;

        public DateTime fechaUltima
        {
            get { return FechaUltima; }
            set { FechaUltima = value; }
        }

        private List<Cliente> ClientesAsociados;

        public List<Cliente> clientesAsociados
        {
            get { return ClientesAsociados; }
            set { ClientesAsociados = value; }
        }


    }
}
