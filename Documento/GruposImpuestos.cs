using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuraduriaFacturas.Documento
{
    public class GruposImpuestos
    {
        public ListaImpuesto listaImpuestos { get; set; }
        public string codigo { get; set; }
        public int total { get; set; }
    }
}
