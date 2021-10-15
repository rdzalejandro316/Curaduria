using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuraduriaFacturas.Documento
{
    public class ListaProducto
    {
        public object listaDeducciones { get; set; }
        public int numeroLinea { get; set; }
        public int cantidad { get; set; }
        public int valorTotal { get; set; }
        public int valorUnitario { get; set; }
        public int cantidadReal { get; set; }
        public bool esMuestraComercial { get; set; }
        public Item item { get; set; }
        public object listaCargosDescuentos { get; set; }
        public string idProducto { get; set; }
        public string codigoUnidad { get; set; }
        public List<ListaImpuesto> listaImpuestos { get; set; }
    }


}
