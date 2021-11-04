using CuraduriaFacturas.Datos;
using CuraduriaFacturas.Factura;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SrvEnvio = CuraduriaFacturas.ServiceEnvio;
using SrvAjunto = CuraduriaFacturas.ServiceAdjuntos;
using CuraduriaFacturas.ServiceEnvio;
using System.IO;
using System.Xml.Serialization;
using System.Data.OleDb;
using System.Configuration;
using CuraduriaFacturas.NotasCredito;
using Microsoft.Win32;
using System.Net.NetworkInformation;
using CuraduriaFacturas.Documento;

namespace CuraduriaFacturas
{

    public partial class MainWindow : Window
    {

        public enum IsTypeFEorNC { FE, NC };
        string Api = ConfigurationManager.AppSettings["Api"].ToString();
        string URLGETFE = ConfigurationManager.AppSettings["URLGETFE"].ToString();
        string URLGETNC = ConfigurationManager.AppSettings["URLGETNC"].ToString();
        string URLFC = ConfigurationManager.AppSettings["URLFC"].ToString();
        string URLNC = ConfigurationManager.AppSettings["URLNC"].ToString();

        IsTypeFEorNC IsFEorNC;

        string fileNameFE = "factura.xml";
        string fileNameNC = "nota.xml";
        string ArchivoRequest = "";
        string tokenEmpresa = ConfigurationManager.AppSettings["tokenEmpresa"].ToString();
        string tokenAuthorizacion = ConfigurationManager.AppSettings["tokenAuthorizacion"].ToString();

        SrvEnvio.ServiceClient serviceClienteEnvio = new SrvEnvio.ServiceClient();
        List<ValueDefault> cabeza_default = new List<ValueDefault>();
        List<ValueDefault> cuerpo_default = new List<ValueDefault>();

        string ConnectionFox = ConfigurationManager.AppSettings["ConnectionFox"].ToString();

        #region inicio

        public MainWindow()
        {
            InitializeComponent();
            TxDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            ArchivoRequest = $"{AppDomain.CurrentDomain.BaseDirectory}";
            loadFields();
        }
        public void loadFields()
        {
            try
            {
                #region cabeza

                cabeza_default.Add(new ValueDefault() { campo = "OTRO_TER", valdefault = "''" });
                cabeza_default.Add(new ValueDefault() { campo = "FACTURA", valdefault = "''" });
                cabeza_default.Add(new ValueDefault() { campo = "DIA_PLAZ", valdefault = "0" });
                cabeza_default.Add(new ValueDefault() { campo = "FEC_POSF", valdefault = "date()" });
                cabeza_default.Add(new ValueDefault() { campo = "FEC_VEN", valdefault = "date()" });
                cabeza_default.Add(new ValueDefault() { campo = "COD_VEN", valdefault = "''" });
                cabeza_default.Add(new ValueDefault() { campo = "COD_BAN", valdefault = "''" });
                cabeza_default.Add(new ValueDefault() { campo = "FIN", valdefault = "''" });
                cabeza_default.Add(new ValueDefault() { campo = "NUM_IMP", valdefault = "''" });
                cabeza_default.Add(new ValueDefault() { campo = "TRM", valdefault = "0" });
                cabeza_default.Add(new ValueDefault() { campo = "RC_PROV", valdefault = "''" });
                cabeza_default.Add(new ValueDefault() { campo = "FEC_CONT", valdefault = "date()" });
                cabeza_default.Add(new ValueDefault() { campo = "SUC_CLI", valdefault = "''" });
                cabeza_default.Add(new ValueDefault() { campo = "IND_COM", valdefault = "0" });
                cabeza_default.Add(new ValueDefault() { campo = "VEN_COM", valdefault = "''" });
                cabeza_default.Add(new ValueDefault() { campo = "N_AR", valdefault = "''" });
                cabeza_default.Add(new ValueDefault() { campo = "RESOLU", valdefault = "''" });
                cabeza_default.Add(new ValueDefault() { campo = "ID_OPG", valdefault = "''" });
                cabeza_default.Add(new ValueDefault() { campo = "_USU", valdefault = "''" });
                cabeza_default.Add(new ValueDefault() { campo = "CIRCULAR", valdefault = "0" });
                cabeza_default.Add(new ValueDefault() { campo = "XXXXX", valdefault = "''" });
                #endregion

                #region cuerpo

                cuerpo_default.Add(new ValueDefault() { campo = "COD_CIU", valdefault = "''" });
                cuerpo_default.Add(new ValueDefault() { campo = "COD_SUC", valdefault = "''" });
                cuerpo_default.Add(new ValueDefault() { campo = "COD_CCO", valdefault = "''" });
                cuerpo_default.Add(new ValueDefault() { campo = "NUM_CHQ", valdefault = "''" });
                cuerpo_default.Add(new ValueDefault() { campo = "DOC_MOV", valdefault = "''" });
                cuerpo_default.Add(new ValueDefault() { campo = "FIN", valdefault = "0" });
                cuerpo_default.Add(new ValueDefault() { campo = "SALDO", valdefault = "0" });
                cuerpo_default.Add(new ValueDefault() { campo = "DOC_CRUC", valdefault = "''" });
                cuerpo_default.Add(new ValueDefault() { campo = "DOC_REF", valdefault = "''" });
                cuerpo_default.Add(new ValueDefault() { campo = "ANO_REF", valdefault = "''" });
                cuerpo_default.Add(new ValueDefault() { campo = "VR_ABONO", valdefault = "0" });
                cuerpo_default.Add(new ValueDefault() { campo = "CRUZAR", valdefault = "0" });
                cuerpo_default.Add(new ValueDefault() { campo = "SAL_DOC", valdefault = "0" });
                cuerpo_default.Add(new ValueDefault() { campo = "INI", valdefault = "''" });
                cuerpo_default.Add(new ValueDefault() { campo = "VAL_ME", valdefault = "0" });
                cuerpo_default.Add(new ValueDefault() { campo = "FEC_VENC", valdefault = "date()" });
                cuerpo_default.Add(new ValueDefault() { campo = "COD_BANC", valdefault = "''" });
                cuerpo_default.Add(new ValueDefault() { campo = "FEC_CON", valdefault = "date()" });
                cuerpo_default.Add(new ValueDefault() { campo = "ORD_PAG", valdefault = "''" });
                cuerpo_default.Add(new ValueDefault() { campo = "FEC_SUSC", valdefault = "''" });
                cuerpo_default.Add(new ValueDefault() { campo = "TIP_CLAS", valdefault = "''" });
                cuerpo_default.Add(new ValueDefault() { campo = "FEC_ING", valdefault = "''" });
                cuerpo_default.Add(new ValueDefault() { campo = "FEC_SALI", valdefault = "''" });


                #endregion


            }
            catch (Exception w)
            {
                MessageBox.Show("error al llenar campos:" + w);
            }
        }

        #endregion

        #region test

        public bool TestConnectionInternet()
        {
            try
            {
                return new Ping().Send("www.google.com.co").Status == IPStatus.Success;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ConecctionForticlient()
        {
            try
            {
                return new Ping().Send("192.168.102.1").Status == IPStatus.Success;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool TestConnectionFox()
        {
            try
            {
                bool flag = false;
                string strCon = @"Provider=VFPOLEDB.1;Data Source=" + ConnectionFox + ";Collating Sequence=MACHINE;Connection Timeout=20;Exclusive=NO;DELETED=True;EXACT=False";
                OleDbConnection con = new OleDbConnection(strCon);
                con.Open();
                if (con.State == ConnectionState.Open) flag = true;
                con.Close();
                return flag;
            }
            catch (OleDbException)
            {
                MessageBox.Show($"Invalid Path or File Name:{ConnectionFox}", "alert", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            catch (Exception)
            {
                MessageBox.Show("connection test error", "alert", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        #endregion

        #region consulta

        private async void BtnConsultarFE_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                #region validaciones

                if (!TestConnectionInternet())
                {
                    MessageBox.Show("conecte su equipo a internet", "alerta", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                if (!ConecctionForticlient())
                {
                    MessageBox.Show("conecte su equipo a la VPN", "alerta", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                #endregion


                TxDetalle.Text = "";
                dataGridAllFact.ItemsSource = null;
                sfBusyIndicator.IsBusy = true;
                IsFEorNC = IsTypeFEorNC.FE;
                ColumnFENC.MappingName = "numFactura";

                var valor = await GetDataFEandNc(URLGETFE, IsFEorNC);

                if (valor != null)
                {
                    dataGridAllFact.ItemsSource = valor;
                    TxTotFacturas.Text = valor.Count.ToString();
                }
                else
                {
                    MessageBox.Show("no hay facturas disponibles", "alerta", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    TxTotFacturas.Text = valor.Count.ToString();
                }


                sfBusyIndicator.IsBusy = false;

            }
            catch (Exception w)
            {
                MessageBox.Show("error al consultar:" + w);
            }
        }

        private async void BtnConsultarNC_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                #region validaciones

                if (!TestConnectionInternet())
                {
                    MessageBox.Show("conecte su equipo a internet", "alerta", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                if (!ConecctionForticlient())
                {
                    MessageBox.Show("conecte su equipo a la VPN", "alerta", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                #endregion

                TxDetalle.Text = "";
                dataGridAllFact.ItemsSource = null;
                sfBusyIndicator.IsBusy = true;
                IsFEorNC = IsTypeFEorNC.NC;
                ColumnFENC.MappingName = "numNotaCredito";

                var valor = await GetDataFEandNc(URLGETNC, IsFEorNC);

                if (valor != null)
                {
                    dataGridAllFact.ItemsSource = valor;
                    TxTotFacturas.Text = valor.Count.ToString();
                }
                else
                {
                    MessageBox.Show("no hay notas credito disponibles", "alerta", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    TxTotFacturas.Text = valor.Count.ToString();
                }



                sfBusyIndicator.IsBusy = false;

            }
            catch (Exception w)
            {
                MessageBox.Show("error al consultar:" + w);
            }
        }

        public async Task<dynamic> GetDataFEandNc(string URL, IsTypeFEorNC tipo)
        {
            try
            {
                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(Api);

                    HttpResponseMessage response = new HttpResponseMessage();

                    response = await client.GetAsync(URL);

                    if (response.IsSuccessStatusCode)
                    {
                        string result = response.Content.ReadAsStringAsync().Result;

                        if (tipo == IsTypeFEorNC.FE)
                        {
                            dynamic json = JsonConvert.DeserializeObject<List<Facturas>>(result);
                            response.Dispose();
                            return json;
                        }
                        else
                        {
                            dynamic json = JsonConvert.DeserializeObject<List<Notas>>(result);
                            response.Dispose();
                            return json;
                        }
                    }
                    else
                    {
                        string result = response.Content.ReadAsStringAsync().Result;
                        response.Dispose();
                        return null;
                    }
                }

            }
            catch (Exception w)
            {
                MessageBox.Show("error en Button_Click:" + w);
                return null;
            }
        }

        #endregion

        #region opciones de listado

        private async void BtnGetFact_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                #region validaciones

                if (!TestConnectionInternet())
                {
                    MessageBox.Show("conecte su equipo a internet", "alerta", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                if (!ConecctionForticlient())
                {
                    MessageBox.Show("conecte su equipo a la VPN", "alerta", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                #endregion

                sfBusyIndicator.IsBusy = true;

                string factura = "";
                string url = "";

                if (IsFEorNC == IsTypeFEorNC.FE)
                {
                    Facturas fact = (Facturas)dataGridAllFact.SelectedItems[0];
                    factura = fact.numFactura;
                    url = URLFC;
                }
                else
                {
                    Notas fact = (Notas)dataGridAllFact.SelectedItems[0];
                    factura = fact.numNotaCredito;
                    url = URLNC;
                }

                var datos = await GetDetails(url, factura, IsFEorNC);

                if (datos != null)
                {
                    var json = JsonConvert.SerializeObject(datos);
                    string jsonFormatted = JValue.Parse(json).ToString(Formatting.Indented);
                    TxDetalle.Text = jsonFormatted;
                }

                sfBusyIndicator.IsBusy = false;
            }
            catch (Exception w)
            {
                MessageBox.Show("error al obtener factura:" + w);
            }
        }

        public async Task<dynamic> GetDetails(string url, string factura, IsTypeFEorNC tipo)
        {
            try
            {


                using (var client = new HttpClient())
                {


                    client.BaseAddress = new Uri(Api);

                    HttpResponseMessage response = new HttpResponseMessage();

                    string urlfactura = url + factura;

                    response = await client.GetAsync(urlfactura);

                    if (response.IsSuccessStatusCode)
                    {
                        string result = response.Content.ReadAsStringAsync().Result;

                        if (tipo == IsTypeFEorNC.FE)
                        {
                            var json = JsonConvert.DeserializeObject<RootFacturas>(result);
                            response.Dispose();
                            return json;
                        }
                        else
                        {
                            var json = JsonConvert.DeserializeObject<RootNotas>(result);
                            response.Dispose();
                            return json;
                        }
                    }
                    else
                    {
                        response.Dispose();
                        return null;
                    }
                }


            }
            catch (Exception w)
            {
                MessageBox.Show("error en Button_Click:" + w);
                return null;
            }
        }

        private async void BtnInsContab_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                #region validaciones

                if (!TestConnectionInternet())
                {
                    MessageBox.Show("conecte su equipo a internet", "alerta", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                if (!ConecctionForticlient())
                {
                    MessageBox.Show("conecte su equipo a la VPN", "alerta", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                if (!TestConnectionFox())
                {
                    MessageBox.Show($"la conexion {ConnectionFox} fue erronea con el sistema siasoft vpf", "alerta", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                
                #endregion

                sfBusyIndicator.IsBusy = true;

                string factura = "";
                string url = "";
                string cod_trn = IsFEorNC == IsTypeFEorNC.FE ? "04" : "08";
                if (IsFEorNC == IsTypeFEorNC.FE)
                {
                    Facturas fact = (Facturas)dataGridAllFact.SelectedItems[0];
                    factura = fact.numFactura;
                    url = URLFC;
                }
                else
                {
                    Notas fact = (Notas)dataGridAllFact.SelectedItems[0];
                    factura = fact.numNotaCredito;
                    url = URLNC;
                }

                var IsExists = await SelectFox($"select cod_trn,num_trn,fec_doc from cab_doc where cod_trn='{cod_trn}' and num_trn='{factura}' ");

                if (IsExists > 0)
                {
                    MessageBoxResult overwrite = MessageBox.Show($"ya existe un movimiento con la contabilizacion del documento {cod_trn}-{factura} usted desea sobrescribirlo", "alerta", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (overwrite == MessageBoxResult.Yes)
                    {
                        List<string> del_list = new List<string>();
                        string delete_cab = $"delete from cab_doc where cod_trn='{cod_trn}' and num_trn='{factura}';";
                        del_list.Add(delete_cab);
                        string delete_cue = $"delete from cue_doc where cod_trn='{cod_trn}' and num_trn='{factura}'; ";
                        del_list.Add(delete_cue);
                        var del = await InsertFox(del_list);
                        
                        if (del)                        
                            MessageBox.Show($"el documento {cod_trn}-{factura} se elimino exitosamente","alerta",MessageBoxButton.OK,MessageBoxImage.Information);                 
                        else                        
                            MessageBox.Show($"error en la eliminacion del documento {cod_trn}-{factura}", "alerta", MessageBoxButton.OK, MessageBoxImage.Exclamation);                        
                    }
                    else
                    {
                        sfBusyIndicator.IsBusy = false;
                        return;
                    }
                }


                MessageBoxResult result = MessageBox.Show($"usted desea generar el documento contable de la factura {factura}", "alerta", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    var datos = await GetDetails(url, factura, IsFEorNC);

                    if (datos != null)
                    {
                        List<string> query = new List<string>();

                        DateTime date = Convert.ToDateTime(datos.fechaEmision);
                        string año = date.Year.ToString();
                        string mes = date.ToString("MM");
                        string num_trn = factura;
                        string nombre_ter = datos.facturador.nombreRegistrado.Trim();


                        string colm_parm_cab = String.Join(",", cabeza_default.Select(s => s.campo).ToArray());
                        string val_parm_cab = String.Join(",", cabeza_default.Select(s => s.valdefault).ToArray());

                        string fec_trn = date.ToString("dd/MM/yyyy");

                        string cabeza = $"INSERT INTO CAB_DOC (ANO_DOC,PER_DOC,COD_TRN,NUM_TRN,FEC_DOC,DETALLE,{colm_parm_cab}) VALUES ";
                        cabeza += $"('{año}','{mes}','{cod_trn}','{num_trn}',date(),'WEB API',{val_parm_cab});";
                        query.Add(cabeza);

                        string colm_parm_cue = String.Join(",", cuerpo_default.Select(s => s.campo).ToArray());
                        string val_parm_cue = String.Join(",", cuerpo_default.Select(s => s.valdefault).ToArray());


                        List<CuentasContable> cuentasContables = datos.cuentasContables;


                        foreach (var cta in cuentasContables)
                        {

                            decimal valor = Convert.ToDecimal(cta.valor);
                            string cuenta = cta.numerocuenta.Trim();
                            
                            string deb_cre = "";
                            string des_mov = "";
                            switch (cuenta.Substring(0,1))
                            {
                                case "4":
                                    deb_cre = IsFEorNC == IsTypeFEorNC.FE ? "DEB_MOV,CRE_MOV" : "CRE_MOV,DEB_MOV";
                                    des_mov = $"INGRESO:{nombre_ter}";
                                    break;
                                case "2":
                                    deb_cre = IsFEorNC == IsTypeFEorNC.FE ? "DEB_MOV,CRE_MOV" : "CRE_MOV,DEB_MOV";
                                    des_mov = $"IVA:{nombre_ter}";
                                    break;
                                case "1":
                                    deb_cre = IsFEorNC == IsTypeFEorNC.FE ? "CRE_MOV,DEB_MOV" : "DEB_MOV,CRE_MOV";
                                    des_mov = $"CAJA:{nombre_ter}";
                                    break;
                            }

                            string deb_cre1 = IsFEorNC == IsTypeFEorNC.FE ? "DEB_MOV,CRE_MOV" : "CRE_MOV,DEB_MOV";
                            string cuerpo1 = $"INSERT INTO CUE_DOC (ANO_DOC,PER_DOC,COD_TRN,NUM_TRN,COD_CTA,COD_TER,DES_MOV,BAS_MOV,{deb_cre},{colm_parm_cue}) VALUES ";
                            cuerpo1 += $"('{año}','{mes}','{cod_trn}','{num_trn}','{cuenta}','{datos.facturador.identificacion}','{des_mov}',0,0,{valor},{val_parm_cue});";                            
                            query.Add(cuerpo1);                            
                        }
                        


                        var fox = await InsertFox(query);
                        if (fox)
                            MessageBox.Show("inserto exitosamente", "alerta", MessageBoxButton.OK, MessageBoxImage.Information);
                        else
                            MessageBox.Show("fallo la insercion de la contabilizacion", "alerta", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }

                sfBusyIndicator.IsBusy = false;

            }
            catch (Exception w)
            {
                MessageBox.Show("error al contabilisar:" + w);
            }
        }

        public async Task<bool> InsertFox(List<string> query)
        {
            try
            {
                bool flag = false;
                string strCon = @"Provider=VFPOLEDB.1;Data Source=" + ConnectionFox + ";Collating Sequence=MACHINE;Connection Timeout=20;Exclusive=NO;DELETED=True;EXACT=False";
                using (OleDbConnection con = new OleDbConnection(strCon))
                {
                    foreach (var item in query)
                    {
                        OleDbCommand cmd = new OleDbCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = item;
                        cmd.Connection = con;
                        await con.OpenAsync();
                        int id = cmd.ExecuteNonQuery();
                        if (id > 0) flag = true;
                        con.Close();
                    }
                }
                return flag;
            }
            catch (Exception w)
            {
                MessageBox.Show("error al insertar o actualizar en fox pro:" + w);
                return false;
            }
        }

        public async Task<int> SelectFox(string query)
        {
            try
            {
                int id = 0;
                string strCon = @"Provider=VFPOLEDB.1;Data Source=" + ConnectionFox + ";Collating Sequence=MACHINE;Connection Timeout=20;Exclusive=NO;DELETED=True;EXACT=False";
                using (OleDbConnection con = new OleDbConnection(strCon))
                {
                    OleDbCommand cmd = new OleDbCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;
                    cmd.Connection = con;
                    await con.OpenAsync();
                    id = cmd.ExecuteNonQuery();
                    con.Close();
                }
                return id;
            }
            catch (Exception w)
            {
                MessageBox.Show("error al insertar o actualizar en fox pro:" + w);
                return 0;
            }
        }

        private async void BtnEnviar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                #region validaciones

                if (!TestConnectionInternet())
                {
                    MessageBox.Show("conecte su equipo a internet", "alerta", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                if (!ConecctionForticlient())
                {
                    MessageBox.Show("conecte su equipo a la VPN", "alerta", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                #endregion

                if (dataGridAllFact.SelectedIndex >= 0)
                {

                    sfBusyIndicator.IsBusy = true;

                    string factura = "";
                    string url = "";
                    if (IsFEorNC == IsTypeFEorNC.FE)
                    {
                        Facturas fact = (Facturas)dataGridAllFact.SelectedItems[0];
                        factura = fact.numFactura;
                        url = URLFC;
                    }
                    else
                    {
                        Notas fact = (Notas)dataGridAllFact.SelectedItems[0];
                        factura = fact.numNotaCredito;
                        url = URLNC;
                    }

                    var datos = await GetDetails(url, factura, IsFEorNC);

                    if (datos != null)
                    {
                        Enviando(datos, IsFEorNC);
                    }

                }
                else
                {
                    MessageBox.Show("seleccione un registro para enviar", "alerta", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }

            }
            catch (Exception w)
            {
                MessageBox.Show("error al enviar:" + w);
            }
        }

        private async Task<FacturaGeneral> BuildFactura(dynamic root, IsTypeFEorNC tipo)
        {
            try
            {


                FacturaGeneral facturaDemo = new FacturaGeneral
                {
                    cantidadDecimales = "2"
                };

                if (tipo == IsTypeFEorNC.FE)
                {
                    facturaDemo.consecutivoDocumento = root.numeroDocumento;
                }
                else
                {
                    facturaDemo.consecutivoDocumento = "XX" + root.numeroDocumento;
                }


                facturaDemo.fechaEmision = root.fechaEmision + " " + root.horaEmision;
                facturaDemo.fechaVencimiento = root.fechaEmision;

                #region cliente
                Cliente cliente = new Cliente
                {
                    actividadEconomicaCIIU = "0010",
                    destinatario = new Destinatario[1]
                };
                Destinatario destinatario = new Destinatario
                {
                    canalDeEntrega = "0"
                };

                Destinatario destinatario1 = destinatario;

                string[] correoEntrega = new string[1];
                correoEntrega[0] = root.facturador.email;

                destinatario1.email = correoEntrega;
                destinatario1.fechaProgramada = root.fechaEmision + " " + root.horaEmision;

                destinatario1.nitProveedorReceptor = "1";
                destinatario1.telefono = root.facturador.telefono;
                cliente.destinatario[0] = destinatario1;
                cliente.detallesTributarios = new Tributos[1];
                Tributos tributos1 = new Tributos
                {
                    codigoImpuesto = "01"
                };
                cliente.detallesTributarios[0] = tributos1;
                string codciu = root.facturador.direccion.codigoCiudad;

                SrvEnvio.Direccion direccionFiscal = new SrvEnvio.Direccion
                {
                    ciudad = root.facturador.direccion.nombreCiudad,
                    codigoDepartamento = root.facturador.direccion.codigoDepartamento,
                    departamento = root.facturador.direccion.nombreDepartamento,
                    direccion = root.facturador.direccion.direccionFisica,
                    lenguaje = "es",
                    municipio = codciu,
                    pais = root.facturador.direccion.codigoPais,
                    zonaPostal = ""
                };

                cliente.direccionFiscal = direccionFiscal;
                cliente.email = root.facturador.contacto.email;


                InformacionLegal informacionLegal = new InformacionLegal
                {
                    codigoEstablecimiento = "00001",
                    nombreRegistroRUT = root.facturador.contacto.nombre,
                    numeroIdentificacion = root.facturador.identificacion,
                    numeroIdentificacionDV = root.facturador.digitoVerificacion,
                    tipoIdentificacion = root.facturador.tipoIdentificacion
                };

                InformacionLegal informacionLegalCliente = informacionLegal;
                cliente.informacionLegalCliente = informacionLegalCliente;
                cliente.nombreRazonSocial = root.facturador.contacto.nombre;
                cliente.notificar = "SI";


                cliente.numeroDocumento = root.facturador.identificacion;
                cliente.numeroIdentificacionDV = root.facturador.digitoVerificacion;
                cliente.responsabilidadesRut = new Obligaciones[1];

                string tdoc = root.facturador.tipoIdentificacion;


                Obligaciones obligaciones1 = new Obligaciones
                {
                    obligaciones = root.facturador.responsabilidadFiscal,//******** ver si toca cambiar
                    regimen = tdoc == "13" ? "05" : "04"
                };

                cliente.responsabilidadesRut[0] = obligaciones1;

                cliente.tipoIdentificacion = root.facturador.tipoIdentificacion;
                cliente.tipoPersona = "1";

                facturaDemo.cliente = cliente;
                #endregion               

                #region detalleDeFactura
                //int ItemsCue = dsImprimir.Tables[1].Rows.Count;
                int ItemsCue = 1;//******** ver si toca cambiar
                facturaDemo.detalleDeFactura = new FacturaDetalle[ItemsCue];
                int item = 0;

                //foreach (DataRow row in dsImprimir.Tables[1].Rows)
                if (root.listaProductos != null)
                {
                    FacturaDetalle producto1 = new FacturaDetalle
                    {
                        cantidadPorEmpaque = "1",
                        cantidadReal = root.listaProductos.cantidadReal.ToString(),
                        cantidadRealUnidadMedida = root.listaProductos.codigoUnidad.ToString(),
                        cantidadUnidades = root.listaProductos.cantidad.ToString(),
                        codigoProducto = root.listaProductos.idProducto,
                        descripcion = root.listaProductos.item.descripcion,
                        descripcionTecnica = root.listaProductos.item.descripcion,
                        estandarCodigo = "999",//******** ver si toca cambiar
                        estandarCodigoProducto = root.listaProductos.idProducto,
                        impuestosDetalles = new FacturaImpuestos[1],
                        cargosDescuentos = new CargosDescuentos[1],
                    };

                    if (root.listaProductos.listaImpuestos != null)
                    {
                        foreach (var imp in root.listaProductos.listaImpuestos)
                        {
                            FacturaImpuestos impuesto1 = new FacturaImpuestos
                            {
                                baseImponibleTOTALImp = imp.baseGravable.ToString(),
                                codigoTOTALImp = imp.codigo,
                                controlInterno = "",
                                porcentajeTOTALImp = imp.porcentaje.ToString(),
                                unidadMedida = imp.codigoUnidad.ToString(),
                                unidadMedidaTributo = "",
                                valorTOTALImp = imp.valor.ToString(),
                                valorTributoUnidad = ""
                            };
                            producto1.impuestosDetalles[0] = impuesto1;
                        }
                    }

                    if (root.listaProductos.listaDeducciones != null)//******** ver si toca cambiar
                    {
                        //if (Convert.ToDecimal(row["val_des"]) > 0)
                        //{
                        //    CargosDescuentos cargoDescto = new CargosDescuentos
                        //    {
                        //        codigo = "07",
                        //        monto = Convert.ToDecimal(row["val_des"]).ToString(),

                        //        montoBase = Convert.ToDecimal(row["subtotal"]).ToString(),
                        //        porcentaje = Convert.ToDecimal(row["por_des"]).ToString(),
                        //        indicador = "0",
                        //        secuencia = "1",
                        //        descripcion = "Descuento de temporada"
                        //    };
                        //    producto1.cargosDescuentos[0] = cargoDescto;
                        //}
                    }

                    if (root.listaProductos.listaImpuestos != null)
                    {
                        foreach (var lp in root.listaProductos.listaImpuestos)
                        {
                            producto1.impuestosTotales = new ImpuestosTotales[1];
                            ImpuestosTotales impuestoTOTAL1 = new ImpuestosTotales
                            {
                                codigoTOTALImp = lp.codigo,
                                montoTotal = lp.valor.ToString()
                            };
                            producto1.impuestosTotales[0] = impuestoTOTAL1;
                        }
                    }

                    producto1.marca = "HKA";

                    #region muestra

                    //string muestra = row["muestra"].ToString().Trim();
                    string muestra = root.listaProductos.esMuestraComercial == true ? "1" : "0";

                    //if (muestra == "1")
                    //{
                    //    decimal pres_default = 1000;
                    //    decimal pre_ref = Convert.ToDecimal(row["precio_refer"]);
                    //    producto1.precioReferencia = pre_ref > 0 ? pre_ref.ToString() : pres_default.ToString();
                    //}

                    #endregion

                    producto1.muestraGratis = muestra;


                    producto1.precioTotal = muestra == "1" ? "0" : root.listaProductos.valorTotal.ToString();

                    //producto1.precioTotalSinImpuestos = muestra == "1" ? "0" : Convert.ToDecimal(row["base"]).ToString();
                    producto1.precioTotalSinImpuestos = muestra == "1" ? "0" : root.listaProductos.valorTotal.ToString();

                    producto1.precioVentaUnitario = root.listaProductos.valorUnitario.ToString();

                    producto1.secuencia = root.listaProductos.numeroLinea.ToString();
                    producto1.unidadMedida = root.listaProductos.codigoUnidad.ToString();
                    facturaDemo.detalleDeFactura[item] = producto1;
                    item++;
                }
                #endregion

                #region DocumentosReferenciados

                if (tipo == IsTypeFEorNC.NC)
                {
                    facturaDemo.documentosReferenciados = new DocumentoReferenciado[2];

                    #region DiscrepansyResponse

                    string num_anu = root.listaDocumentosReferenciados.id;
                    string fec_anu = root.listaDocumentosReferenciados.fecha;
                    string cufe = await GetCufeFE(num_anu);

                    DocumentoReferenciado DocumentoReferenciado1 = new DocumentoReferenciado
                    {
                        codigoEstatusDocumento = "2",
                        codigoInterno = "4",
                        cufeDocReferenciado = cufe
                    };

                    string[] descripcion = new string[1];
                    descripcion[0] = "Nota";
                    DocumentoReferenciado1.descripcion = descripcion;
                    DocumentoReferenciado1.numeroDocumento = num_anu;
                    #endregion
                    facturaDemo.documentosReferenciados[0] = DocumentoReferenciado1;

                    #region BillingReference
                    DocumentoReferenciado DocumentoReferenciado2 = new DocumentoReferenciado
                    {
                        codigoInterno = "5",
                        cufeDocReferenciado = cufe,
                        fecha = Convert.ToDateTime(fec_anu).ToString("yyyy-MM-dd"),
                        numeroDocumento = num_anu
                    };
                    #endregion
                    facturaDemo.documentosReferenciados[1] = DocumentoReferenciado2;
                }

                #endregion

                #region impuestosGenerales


                facturaDemo.impuestosGenerales = new FacturaImpuestos[1];
                FacturaImpuestos impuestoGeneral1 = new FacturaImpuestos
                {
                    baseImponibleTOTALImp = root.gruposImpuestos.listaImpuestos.baseGravable.ToString(),
                    codigoTOTALImp = root.gruposImpuestos.listaImpuestos.codigo.ToString(),
                    porcentajeTOTALImp = root.gruposImpuestos.listaImpuestos.porcentaje.ToString(),
                    unidadMedida = root.gruposImpuestos.listaImpuestos.codigoUnidad.ToString(),
                    valorTOTALImp = root.gruposImpuestos.listaImpuestos.valor.ToString()
                };

                facturaDemo.impuestosGenerales[0] = impuestoGeneral1;
                #endregion

                #region impuestosTotales
                facturaDemo.impuestosTotales = new ImpuestosTotales[1];
                ImpuestosTotales impuestoGeneralTOTAL1 = new ImpuestosTotales
                {
                    codigoTOTALImp = root.gruposImpuestos.codigo,
                    montoTotal = root.gruposImpuestos.total.ToString()
                };
                facturaDemo.impuestosTotales[0] = impuestoGeneralTOTAL1;
                #endregion

                #region mediosDePago                                
                facturaDemo.mediosDePago = new MediosDePago[1];


                MediosDePago medioPago1 = new MediosDePago();

                string codigo = root.pago.codigoMetodoPago.ToString();
                if (codigo == "0") codigo = "2";

                medioPago1.medioPago = root.pago.codigoMedioPago.ToString();
                medioPago1.metodoDePago = codigo;
                medioPago1.numeroDeReferencia = "";
                medioPago1.fechaDeVencimiento = root.pago.fechaVencimiento.ToString();
                facturaDemo.mediosDePago[0] = medioPago1;

                #endregion

                #region rango numeracion


                facturaDemo.moneda = root.codigoMoneda.ToString();

                string rangoNumeracion = "";
                if (tipo == IsTypeFEorNC.FE)
                    rangoNumeracion = "-" + root.resolucion.numeracion.hasta;
                else rangoNumeracion = "XX-700";

                facturaDemo.rangoNumeracion = rangoNumeracion;

                //facturaDemo.redondeoAplicado = "0.00";
                facturaDemo.redondeoAplicado = root.redondeo.ToString();
                facturaDemo.tipoDocumento = root.codigoTipoDocumento.ToString();

                #endregion


                facturaDemo.tipoOperacion = root.tipoOperacion.ToString();
                facturaDemo.totalProductos = "1";

                facturaDemo.totalBaseImponible = root.totalBaseImponible.ToString();
                facturaDemo.totalBrutoConImpuesto = root.total.ToString();
                facturaDemo.totalMonto = root.total.ToString();
                facturaDemo.totalSinImpuestos = root.totalBaseImponible.ToString();

                return facturaDemo;
            }
            catch (Exception ex)
            {
                MessageBox.Show("error en la construccion de la factura:" + ex.Message, "BuildFactrua1");
                return null;

            }
        }


        private async Task<string> GetCufeFE(string numtrn)
        {
            try
            {
                DocumentStatusResponse response = await serviceClienteEnvio.EstadoDocumentoAsync(tokenEmpresa, tokenAuthorizacion, numtrn);
                return response.cufe;
            }
            catch (Exception)
            {
                MessageBox.Show("el documento no se ecnuentra en el portar de the factoryhka", "alerta", MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }
        }

        private async void Enviando(dynamic request, IsTypeFEorNC tipo)
        {
            try
            {
                #region validaciones

                if (!TestConnectionInternet())
                {
                    MessageBox.Show("conecte su equipo a internet", "alerta", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                if (!ConecctionForticlient())
                {
                    MessageBox.Show("conecte su equipo a la VPN", "alerta", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                #endregion

                FacturaGeneral factura = await BuildFactura(request, tipo);

                if (factura == null)
                {
                    MessageBox.Show("Error en creacion de factura..", "BuildFactura");
                    return;
                }

                string file = tipo == IsTypeFEorNC.FE ? fileNameFE : fileNameNC;
                ArchivoRequest = $"{AppDomain.CurrentDomain.BaseDirectory}{file}";

                StreamWriter MyFile = new StreamWriter(ArchivoRequest);
                XmlSerializer Serializer1 = new XmlSerializer(typeof(FacturaGeneral));
                Serializer1.Serialize(MyFile, factura);
                MyFile.Close();


                Task<DocumentResponse> docRespuesta;
                TxLogFE.Clear();
                TxLogFE.Text = "Envio de Factura:" + Environment.NewLine;



                if (MessageBox.Show("Confirmar envio ?", "Enviando documento", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    string adjuntos = "0";


                    sfBusyIndicator.IsBusy = true;
                    //GridMain.IsEnabled = false;


                    docRespuesta = serviceClienteEnvio.EnviarAsync(tokenEmpresa, tokenAuthorizacion, factura, adjuntos);
                    await docRespuesta;

                    if (docRespuesta.IsCompleted)
                    {
                        sfBusyIndicator.IsBusy = false;

                        StringBuilder msgError = new StringBuilder();


                        if (docRespuesta.Result.mensajesValidacion != null)
                        {
                            int nReturnMsg = docRespuesta.Result.mensajesValidacion.Count();

                            for (int i = 0; i < nReturnMsg; i++)
                                msgError.Append(docRespuesta.Result.mensajesValidacion[i].ToString() + Environment.NewLine);
                        }

                        if (docRespuesta.Result.codigo == 114)  //documento emitdo previa mente
                        {
                            DocumentStatusResponse resp = serviceClienteEnvio.EstadoDocumento(tokenEmpresa, tokenAuthorizacion, factura.consecutivoDocumento.ToString());
                            if (resp.codigo == 200)
                            {

                                TxLogFE.Text += "ReEnvio de Factura emitido previa mente:" + docRespuesta.Result.codigo + Environment.NewLine;
                                TxLogFE.Text += "Codigo: " + resp.codigo.ToString() + Environment.NewLine;
                                TxLogFE.Text += "Consecutivo Documento: " + resp.consecutivo + Environment.NewLine;
                                TxLogFE.Text += "Cufe: " + resp.cufe + Environment.NewLine;
                                TxLogFE.Text += "Mensaje: " + resp.mensaje + Environment.NewLine;
                                TxLogFE.Text += "Resultado: " + resp.resultado + Environment.NewLine;
                                return;
                            }
                        }

                        //envio factura 
                        if (docRespuesta.Result.codigo == 200 || docRespuesta.Result.codigo == 201)
                        {

                            StringBuilder response = new StringBuilder();

                            response.Append("Codigo: " + docRespuesta.Result.codigo.ToString() + Environment.NewLine);
                            response.Append("Consecutivo Documento: " + docRespuesta.Result.consecutivoDocumento + Environment.NewLine);
                            response.Append("Cufe: " + docRespuesta.Result.cufe + Environment.NewLine);
                            response.Append("Mensaje: " + docRespuesta.Result.mensaje + Environment.NewLine);
                            response.Append("Resultado: " + docRespuesta.Result.resultado + Environment.NewLine);

                            TxLogFE.Text += response.ToString();

                        }
                        else
                        {
                            StringBuilder response = new StringBuilder();
                            response.Append("x Codigo x:" + docRespuesta.Result.codigo.ToString() + Environment.NewLine);
                            response.Append("Consecutivo Documento :" + docRespuesta.Result.consecutivoDocumento + Environment.NewLine);
                            response.Append("Cufe: " + docRespuesta.Result.cufe + Environment.NewLine);
                            response.Append("Mensaje :" + docRespuesta.Result.mensaje + Environment.NewLine);
                            response.Append("Resultado :" + docRespuesta.Result.resultado + Environment.NewLine);
                            response.Append("Errores :" + msgError.ToString() + Environment.NewLine);


                            if (docRespuesta.Result.reglasValidacionDIAN != null)
                            {
                                for (int i = 0; i < docRespuesta.Result.reglasValidacionDIAN.Count(); i++)
                                {
                                    response.Append("DIAN:" + docRespuesta.Result.reglasValidacionDIAN[i].ToString() + Environment.NewLine);
                                }
                            }

                            TxLogFE.Text += response.ToString();
                        }
                    }

                    sfBusyIndicator.IsBusy = false;
                    //GridMain.IsEnabled = true;
                    //GridMain.Opacity = 1;


                }

                sfBusyIndicator.IsBusy = false;
                //GridMain.IsEnabled = true;
                //GridMain.Opacity = 1;

            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR Enviando:" + ex.Message);
                sfBusyIndicator.IsBusy = false;
                GridMain.IsEnabled = true;
                GridMain.Opacity = 1;
            }
        }


        #endregion

        #region opciones de facturacion electronica     
        private async void BtnReenvio_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                #region validaciones

                if (!TestConnectionInternet())
                {
                    MessageBox.Show("conecte su equipo a internet", "alerta", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                if (!ConecctionForticlient())
                {
                    MessageBox.Show("conecte su equipo a la VPN", "alerta", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                if (string.IsNullOrEmpty(TxEmail.Text))
                {
                    MessageBox.Show("el campo email debe de estar lleno", "alerta", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                if (dataGridAllFact.SelectedIndex < 0)
                {
                    MessageBox.Show("debe de seleccionar alguna factura de la grilla", "alerta", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                #endregion

                TxLogFE.Text = "Envio de Factura:" + Environment.NewLine;

                string factura = "";

                if (IsFEorNC == IsTypeFEorNC.FE)
                {
                    Facturas fact = (Facturas)dataGridAllFact.SelectedItems[0];
                    factura = fact.numFactura;
                }
                else
                {
                    Notas fact = (Notas)dataGridAllFact.SelectedItems[0];
                    factura = fact.numNotaCredito;
                }

                sfBusyIndicator.IsBusy = true;

                GridMain.IsEnabled = false;
                GridMain.Opacity = 0.5;


                Task<SendEmailResponse> docRespuesta;
                docRespuesta = serviceClienteEnvio.EnvioCorreoAsync(tokenEmpresa, tokenAuthorizacion, factura, TxEmail.Text, "0");
                await docRespuesta;

                if (docRespuesta.IsCompleted)
                {

                    StringBuilder response = new StringBuilder();
                    response.Append("x Codigo x:" + docRespuesta.Result.codigo.ToString() + Environment.NewLine);
                    response.Append("Mensaje :" + docRespuesta.Result.mensaje + Environment.NewLine);
                    response.Append("Resultado :" + docRespuesta.Result.resultado + Environment.NewLine);
                    TxLogFE.Text += response.ToString();
                }

                sfBusyIndicator.IsBusy = false;
                GridMain.IsEnabled = true;
                GridMain.Opacity = 1;


                TxLogFE.Text += "** FIN ** " + Environment.NewLine;


            }
            catch (Exception w)
            {
                MessageBox.Show("error al reenviar adjunto:" + w);
            }
        }

        private async void BtnEstado_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                #region validaciones

                if (!TestConnectionInternet())
                {
                    MessageBox.Show("conecte su equipo a internet", "alerta", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                if (!ConecctionForticlient())
                {
                    MessageBox.Show("conecte su equipo a la VPN", "alerta", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                #endregion

                if (string.IsNullOrEmpty(TxDoc.Text))
                {
                    MessageBox.Show("el campo documento debe de estar lleno", "alerta", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                sfBusyIndicator.IsBusy = true;
                GridMain.IsEnabled = false;
                GridMain.Opacity = 0.5;

                DocumentStatusResponse response = await serviceClienteEnvio.EstadoDocumentoAsync(tokenEmpresa, tokenAuthorizacion, TxDoc.Text);
                StringBuilder st = new StringBuilder();
                st.Append("ACEPTACION FISICA: " + (response.aceptacionFisica ? "SI" : "NO") + Environment.NewLine);
                st.Append("CANDENA CODIGO QR: " + response.cadenaCodigoQR.ToString().Trim() + Environment.NewLine);
                st.Append("CANDENA CODIGO CUFE: " + response.cadenaCufe.ToString().Trim() + Environment.NewLine);
                st.Append("CODIGO: " + response.codigo.ToString().Trim() + Environment.NewLine);
                st.Append("CONSECUTIVO: " + response.consecutivo.ToString().Trim() + Environment.NewLine);
                st.Append("CUFE: " + response.cufe.ToString().Trim() + Environment.NewLine);
                st.Append("ESTADO DOC: " + response.descripcionEstatusDocumento.ToString().Trim() + Environment.NewLine);
                st.Append("VALIDACION DIAN: " + (response.esValidoDIAN ? "ACEPTADA" : "EN ESPERA") + Environment.NewLine);
                st.Append("FECHA DOC: " + response.fechaDocumento.ToString().Trim() + Environment.NewLine);
                st.Append("MENSAJE: " + response.mensaje.ToString().Trim() + Environment.NewLine);
                st.Append("MENSAJE DOC: " + response.mensajeDocumento.ToString().Trim() + Environment.NewLine);
                st.Append("POSEE ADJUNTO: " + (response.poseeAdjuntos ? "SI" : "NO") + Environment.NewLine);
                st.Append("RESULTADO: " + response.resultado.ToString().Trim() + Environment.NewLine);

                sfBusyIndicator.IsBusy = false;
                GridMain.IsEnabled = true;
                GridMain.Opacity = 1;
                MessageBox.Show(st.ToString(), "Estado de Documento", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            catch (Exception)
            {
                MessageBox.Show("el documento no existe", "alerta", MessageBoxButton.OK, MessageBoxImage.Warning);
                sfBusyIndicator.IsBusy = false;
                GridMain.IsEnabled = true;
                GridMain.Opacity = 1;
            }
        }

        private async void Btndescargar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                #region validaciones

                if (!TestConnectionInternet())
                {
                    MessageBox.Show("conecte su equipo a internet", "alerta", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                if (!ConecctionForticlient())
                {
                    MessageBox.Show("conecte su equipo a la VPN", "alerta", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                #endregion

                if (string.IsNullOrEmpty(TxDoc.Text))
                {
                    MessageBox.Show("el campo documento debe de estar lleno", "alerta", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                sfBusyIndicator.IsBusy = true;
                GridMain.IsEnabled = false;
                GridMain.Opacity = 0.5;

                DownloadPDFResponse pdfResponse = null;
                pdfResponse = await serviceClienteEnvio.DescargaPDFAsync(tokenEmpresa, tokenAuthorizacion, TxDoc.Text);

                if (pdfResponse.codigo == 200)
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "Pdf|*.pdf";
                    saveFileDialog.Title = "Save File";
                    saveFileDialog.ShowDialog();

                    if (!string.IsNullOrEmpty(saveFileDialog.FileName))
                    {
                        string path = saveFileDialog.FileName;
                        File.WriteAllBytes(path, Convert.FromBase64String(pdfResponse.documento));
                        MessageBox.Show("se guardo el archivo exitosamente", "alerta", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }

                sfBusyIndicator.IsBusy = false;
                GridMain.IsEnabled = true;
                GridMain.Opacity = 1;


            }
            catch (Exception w)
            {
                MessageBox.Show("error al descargar formato de documento:" + w);
            }
        }
        #endregion


    }
}

