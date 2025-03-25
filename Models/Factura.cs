using System;
using System.Collections.Generic;

namespace TEATRO.Models
{
    public class Factura
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public int EventoId { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public string MetodoPago { get; set; }
        public List<Entrada> Entradas { get; set; } = new List<Entrada>();
    }
}
