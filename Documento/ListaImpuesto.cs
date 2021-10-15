using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuraduriaFacturas.Documento
{
    public class ListaImpuesto
    {
        public string nombre { get; set; }
        public int valor { get; set; }
        public string codigo { get; set; }
        public int porcentaje { get; set; }
        public int baseGravable { get; set; }
        public int codigoUnidad { get; set; }
    }
}
