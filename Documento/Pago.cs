using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuraduriaFacturas.Documento
{
    public class Pago
    {
        public int codigoMetodoPago { get; set; }
        public string codigoMedioPago { get; set; }
        public string fechaVencimiento { get; set; }
    }
}
