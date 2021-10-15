using CuraduriaFacturas.Documento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuraduriaFacturas.NotasCredito
{
    public class RootNotas
    {
        public ListaProducto listaProductos { get; set; }
        public object gruposDeducciones { get; set; }
        public Resolucion resolucion { get; set; }
        public string codigoTipoDocumento { get; set; }
        public string tipoOperacion { get; set; }
        public string prefijoDocumento { get; set; }
        public string numeroDocumento { get; set; }
        public string fechaEmision { get; set; }
        public string horaEmision { get; set; }
        public int numeroLineas { get; set; }
        public int totalBaseImponible { get; set; }
        public int subtotal { get; set; }
        public int totalDescuentos { get; set; }
        public int totalCargos { get; set; }
        public int totalAnticipos { get; set; }
        public int redondeo { get; set; }
        public int total { get; set; }
        public string codigoMoneda { get; set; }
        public object tasaCambio { get; set; }
        public Pago pago { get; set; }
        public ListaDocumentosReferenciados listaDocumentosReferenciados { get; set; }
        public ListaDescripciones listaDescripciones { get; set; }
        public ListaCorrecciones listaCorrecciones { get; set; }
        public object listaAnticipos { get; set; }
        public object listaCargosDescuentos { get; set; }
        public GruposImpuestos gruposImpuestos { get; set; }
        public int subtotalMasTributos { get; set; }
        public Facturador facturador { get; set; }
        public Adquiriente adquiriente { get; set; }

    }
}
