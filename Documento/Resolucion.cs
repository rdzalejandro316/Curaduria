using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuraduriaFacturas.Documento
{
    public class Resolucion
    {
        public string numero { get; set; }
        public string fechaInicio { get; set; }
        public string fechaFin { get; set; }
        public Numeracion numeracion { get; set; }
    }
}
