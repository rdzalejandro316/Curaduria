using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuraduriaFacturas.Documento
{
    public class Adquiriente
    {
        public string razonSocial { get; set; }
        public string nombreRegistrado { get; set; }
        public string tipoIdentificacion { get; set; }
        public string identificacion { get; set; }
        public object digitoVerificacion { get; set; }
        public int naturaleza { get; set; }
        public string codigoRegimen { get; set; }
        public string responsabilidadFiscal { get; set; }
        public string telefono { get; set; }
        public string email { get; set; }
        public Contacto contacto { get; set; }
        public Direccion direccion { get; set; }
        public string codigoImpuesto { get; set; }
        public string nombreImpuesto { get; set; }
    }
}
