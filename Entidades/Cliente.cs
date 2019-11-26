using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Entidades
{
    public class Cliente
    {
        private int IdCliente;

        public int idCliente
        {
            get { return IdCliente; }
            set { IdCliente = value; }
        }

        private int IdCaja;

        public int idCaja
        {
            get { return IdCaja; }
            set { IdCaja = value; }
        }

        private string NombreCliente;

        public string nombreCliente
        {
            get { return NombreCliente; }
            set { NombreCliente = value; }
        }

        private int Estado;

        public int estado
        {
            get { return Estado; }
            set { Estado = value; }
        }

    }
}
