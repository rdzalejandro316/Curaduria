using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuraduriaFacturas.Documento
{
    public class Direccion
    {
        public string codigoPais { get; set; }
        public string nombrePais { get; set; }
        public string codigoLenguajePais { get; set; }
        public string codigoDepartamento { get; set; }
        public string nombreDepartamento { get; set; }
        public string codigoCiudad { get; set; }
        public string nombreCiudad { get; set; }
        public string direccionFisica { get; set; }
        public string codigoPostal { get; set; }
    }
}
