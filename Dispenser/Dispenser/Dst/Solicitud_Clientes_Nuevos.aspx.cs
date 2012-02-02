using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//Agregados
using Telerik.Web.UI;
using System.Data;
using System.Data.SqlClient;

namespace Dispenser.Dst
{
    public partial class Solicitud_Clientes_Nuevos : System.Web.UI.Page
    {

        #region Globales

        //Listas para los productos y dispensadores
        static List<string> codigoDispensadores;
        static List<string> codigoProducto;

        //Listas para las cantidades de dispensador y producto
        static List<int> cantidadDispensadores;
        static List<int> cantidadProductos;

        //Lista de inversion de solicitud
        static List<double> costoDetalle;
        
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Menu menu = Master.FindControl("NavigationMenu") as Menu;
                MenuItem item = new MenuItem();
                MenuItem item2 = new MenuItem();
                MenuItem item3 = new MenuItem();

                item3.Text = "Clientes Existentes";
                item3.NavigateUrl = "Solicitud_Dispensadores.aspx";

                item2.ChildItems.Add(item3);
                item2.Text = "Solicitud Dispensadores";
                item2.Selectable = false;

                item.Text = "Seguimiento Solicitudes";
                item.NavigateUrl = "SeguimientoSolicitudes.aspx";
                menu.Items[1].ChildItems.Add(item2);
                menu.Items[1].ChildItems.Add(item);

                Connection conexion = new Connection();

                if (!IsPostBack)
                {

                    DateTime hoy = DateTime.Now;

                    if (Session.Contents["rol"].ToString().Equals("KCPADM") || Session.Contents["rol"].ToString().Equals("KCPCCR"))
                        Response.Redirect("../Default.aspx");

                    dpFechaSolicitada.MinDate = hoy.AddDays(7);
                    dpFechaSolicitada.SelectedDate = hoy.AddDays(7);

                    txtMotivo.Text = conexion.getReason("REASON_DESCRIP", "ID_REASON", "3");

                    setDataSources();

                    //Listas para los productos y dispensadores
                    codigoDispensadores = new List<string>();
                    codigoProducto = new List<string>();

                    //Listas para las cantidades de dispensador y producto
                    cantidadDispensadores = new List<int>();
                    cantidadProductos = new List<int>();

                    //Lista de inversion de solicitud
                    costoDetalle = new List<double>();

                    if (Convert.ToDouble(Session["porcentaje"].ToString()) >= 80 && Convert.ToDouble(Session["porcentaje"].ToString()) < 100)
                    {
                        radajaxmanager.ResponseScripts.Add(@"alert('Advertencia su presupuesto es menor o igual a un 20%.');");

                        if (!Convert.ToBoolean(Session.Contents["correoBajo"].ToString()))
                        {
                            conexion.correoPresupuesto(conexion.getUsersInfo("CLIENT_ID", "USER_ID", Session.Contents["userid"].ToString()), Convert.ToDouble(Session["porcentaje"].ToString()));
                            Session.Contents["correoBajo"] = true;
                        }
                    }
                    else if (Convert.ToDouble(Session["porcentaje"].ToString()) >= 100)
                    {
                        radajaxmanager.ResponseScripts.Add(@"alert('Advertencia presupuesto agotado toda solicitud sera movida al siguiente mes.');");
                        dpFechaSolicitada.MinDate = hoy.AddDays(new DateTime(hoy.Year, hoy.Month + 1, 1).DayOfYear - hoy.DayOfYear);

                        if (!Convert.ToBoolean(Session.Contents["correoSobre"].ToString()))
                        {
                            conexion.correoPresupuesto(conexion.getUsersInfo("CLIENT_ID", "USER_ID", Session.Contents["userid"].ToString()), Convert.ToDouble(Session["porcentaje"].ToString()));
                            Session.Contents["correoSobre"] = true;
                        }
                    }
                }
            }
            catch (Exception error)
            {
                radajaxmanager.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }
        }

        #region Eventos de carga de combobox
        protected void cmbVendedor_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
        {
            e.Item.Text = ((DataRowView)e.Item.DataItem)["SALES_NAME"].ToString();
            e.Item.Value = ((DataRowView)e.Item.DataItem)["SALES_ID"].ToString();
        }

        protected void cmbSubSegmento_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
        {
            e.Item.Text = ((DataRowView)e.Item.DataItem)["SUB_SEGMENT_NAME"].ToString();
            e.Item.Value = ((DataRowView)e.Item.DataItem)["SUB_SEGMENT_ID"].ToString();
        }

        protected void cmbCiudad_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
        {
            e.Item.Text = ((DataRowView)e.Item.DataItem)["DIVISION_NAME"].ToString();
            e.Item.Value = ((DataRowView)e.Item.DataItem)["CITY_NAME"].ToString();
        }

        protected void cmbCiudad_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
        {
            sqlCiudad.SelectCommand = "SELECT DIVISION_NAME, CITY_NAME FROM DIVISION_TERRITORIAL WHERE CITY_NAME LIKE '%" + e.Text + "%'";
        }

        protected void cmbCondicionPago_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
        {
            e.Item.Text = ((DataRowView)e.Item.DataItem)["PAYMENT_CONDITION"].ToString();
            e.Item.Value = ((DataRowView)e.Item.DataItem)["ID_PAYMENT_CONDITION"].ToString();
        }
        #endregion

        #region Eventos
        protected void btEnviar_Click(object sender, EventArgs e)
        {
            try
            {

                string codigoTemp = quitarCeros(txtCodigo.Text);
                
                Connection conexion = new Connection();

                if (conexion.seekEndUser(conexion.getUsersInfo("CLIENT_ID", "USER_ID", Session.Contents["userid"].ToString()), codigoTemp))
                {
                    radajaxmanager.ResponseScripts.Add(@"alert('Codigo que ingresa ya existe.');");
                    return;
                }

                string clientid = conexion.getUsersInfo("CLIENT_ID", "USER_ID", Session["userid"].ToString());
                bool paraSiguienteMes = false;
                bool esEditable = true;

                int inconsistencias = 0;

                #region Recuperacion de data del grid
                foreach (GridDataItem dataItem in grdDispensadoresProducto.MasterTableView.Items)
                {
                    if ((dataItem.FindControl("txtCantidadDispensadores") as RadNumericTextBox).Text.Equals(String.Empty))
                        (dataItem.FindControl("txtCantidadDispensadores") as RadNumericTextBox).Text = "0";

                    if ((dataItem.FindControl("txtCantidadProducto") as RadNumericTextBox).Text.Equals(String.Empty))
                        (dataItem.FindControl("txtCantidadProducto") as RadNumericTextBox).Text = "0";

                    if (((dataItem.FindControl("txtCantidadDispensadores") as RadNumericTextBox).Text != "0" && (dataItem.FindControl("txtCantidadDispensadores") as RadNumericTextBox).Text != String.Empty)
                        && ((dataItem.FindControl("txtCantidadProducto") as RadNumericTextBox).Text != "0" && (dataItem.FindControl("txtCantidadProducto") as RadNumericTextBox).Text != String.Empty))
                    {
                        string temporal = String.Empty;
                        string temporal1 = String.Empty;

                        TableCell celda = dataItem["Codigo"];
                        TableCell celdaPrecio = dataItem["dispenserPrice"];
                        temporal = celda.Text;
                        codigoDispensadores.Add(temporal);

                        temporal1 = (dataItem.FindControl("cmbProducto") as RadComboBox).SelectedValue;
                        codigoProducto.Add(temporal1);

                        int dispensadores = Convert.ToInt32((dataItem.FindControl("txtCantidadDispensadores") as RadNumericTextBox).Text);
                        int productos = Convert.ToInt32((dataItem.FindControl("txtCantidadProducto") as RadNumericTextBox).Text);
                        double valorDetalle = Convert.ToDouble(celdaPrecio.Text.Substring(1)) * dispensadores;

                        cantidadDispensadores.Add(dispensadores);
                        cantidadProductos.Add(productos);
                        costoDetalle.Add(valorDetalle);
                    }
                    else
                    {
                        if (((dataItem.FindControl("txtCantidadDispensadores") as RadNumericTextBox).Text != "0" && (dataItem.FindControl("txtCantidadProducto") as RadNumericTextBox).Text == "0")
                            || ((dataItem.FindControl("txtCantidadDispensadores") as RadNumericTextBox).Text == "0" && (dataItem.FindControl("txtCantidadProducto") as RadNumericTextBox).Text != "0"))
                            inconsistencias++;

                    }
                }
                #endregion

                if (inconsistencias > 0)
                {
                    codigoDispensadores.Clear();
                    codigoProducto.Clear();
                    cantidadDispensadores.Clear();
                    cantidadProductos.Clear();
                    costoDetalle.Clear();
                    /*datos.Clear();
                    campos.Clear();*/

                    radajaxmanager.ResponseScripts.Add(String.Format(@"alert('Se han encontrado {0} inconsistencias favor revise la tabla de dispensadores y productos.');", inconsistencias));

                    return;
                }

                //Validacion de la cantidad de dispensadores
                if (cantidadDispensadores.Count == 0)
                {
                    codigoDispensadores.Clear();
                    codigoProducto.Clear();
                    cantidadDispensadores.Clear();
                    cantidadProductos.Clear();
                    costoDetalle.Clear();
                    /*datos.Clear();
                    campos.Clear();*/

                    radajaxmanager.ResponseScripts.Add(@"alert('No se han contabilizado productos.');");

                    return;
                }

                #region Datos y proceso necesario para el query
                DateTime hoy = DateTime.Now;
                Session.Add("fechasolicitud", hoy.ToString("yyyMMdd"));

                DateTime fechaRequerida = DateTime.Parse(Convert.ToString(dpFechaSolicitada.SelectedDate));
                Session.Add("fecharequerida", fechaRequerida.ToString("yyyMMdd"));

                string query = String.Empty;
                string idpais = conexion.getUserCountry(Session["userid"].ToString());

                //Sumo todos los detalles para conseguir el costo de toda la solicitud
                double valorDetalle2 = 0;
                for (int i = 0; i < costoDetalle.Count; i++)
                    valorDetalle2 += costoDetalle[i];

                if ((valorDetalle2 + Convert.ToDouble(Session.Contents["pendiente"].ToString()) + Convert.ToDouble(Session.Contents["aprobado"].ToString()))
                    > Convert.ToDouble(Session.Contents["presupuesto"].ToString()))
                {
                    paraSiguienteMes = true;
                    esEditable = false;
                }

                string queryTemp = String.Format("SELECT SEGMENT_ID FROM SUB_SEGMENTOS WHERE SUB_SEGMENT_ID = '{0}'", cmbSubSegmento.SelectedValue);
                DataTable tablaTemp = conexion.getGridDataSource(queryTemp);

                string queryUser = "INSERT INTO CLIENTES_FINALES (END_USER_ID, CLIENT_ID, TRADE_NAME, SOCIAL_REASON, CORPORATE_ID, E_MAIL, TELEPHONE," +
                " SEGMENT_ID, SUB_SEGMENT_ID, ADDRESS, NEIGHBOR, STATE, CITY, POSTAL_CODE, ID_PAYMENT_CONDITION, PURCHASE_FREQUENCY, MAINTENANCE_FREQUENCY, " +
                "STRATEGIC_CUSTOMER, TRAFFIC_TYPE, TERTIARY_CLEANING, EMPLOYEES, VISITORS, WASHBASIN, MALE_BATH, FEMALE_BATH, CONTACT_PERSON, " +
                "CONTACT_TELEPHONE, CONTACT_MAIL, CONTACT_POSITION, SELLER_ID, CLIENT_STATUS) VALUES ('" + txtCodigo.Text + "', '" +
                clientid + "', '" + txtNombreComercial.Text + "', '" + txtRazonSocial.Text + "', '" + txtCedulaJuridica.Text + "', '" +
                txtEMail.Text + "', '" + txtTelefono.Text + "', '" + cmbSubSegmento.Text + "', '" + cmbSubSegmento.SelectedValue + "', '" +
                txtDireccion.Text + "', '" + txtBarrio.Text + "', '" + cmbCiudad.Text + "', '" + cmbCiudad.SelectedValue + "', '" +
                txtCodigoPostal.Text + "', '" + cmbCondicionPago.SelectedValue + "', '" + cmbFrecuenciaCompra.SelectedValue + "', '" +
                cmbFrecuenciaMantenimiento.SelectedValue + "', '" + cmbClienteEstrategico.SelectedValue + "', '" + cmbTipoTrafico.SelectedValue + "', '" +
                cmbLimpiezaTercerizada.SelectedValue + "', '" + txtCantidadEmpleados.Text + "', '" + txtCantidadVisitantes.Text + "', '" +
                txtCantidadLavatorios.Text + "', '" + txtBañoHombre.Text + "', '" + txtBañoMujer.Text + "', '" + txtPersonaContacto.Text + "', '" +
                txtTelefonoContacto.Text + "', '" + txtCorreoContacto.Text + "', '" + txtPosicion.Text + "', '" + cmbVendedor.SelectedValue + "', '1')";

                if (!conexion.setNewEndUser(queryUser))
                {
                    codigoDispensadores.Clear();
                    codigoProducto.Clear();
                    cantidadDispensadores.Clear();
                    cantidadProductos.Clear();
                    costoDetalle.Clear();
                    radajaxmanager.ResponseScripts.Add(@"alert('Error inesperado al crear el nuevo cliente.');");
                    return;
                }

                query = "INSERT INTO SOLICITUD_DISPENSADORES (DATE_REQUEST, ID_COUNTRY, REASON_ID, INSTALL_DATE, CLIENT_ID, SALES_ID, COMMENTS, END_USER_ID, SEGMENT_ID, SUB_SEGMENT_ID, ADDRESS, NEIGHBORHOOD, CITY, "
                + "STATE, POSTAL_CODE, CONTACT_TELEPHONE, CONTACT_NAME, CONTACT_EMAIL, CONTACT_POSITION, ID_PAYMENT_CONDITION, PURCHASE_FREQUENCY, MAINTENANCE_FREQUENCY, "
                + "STRATEGIC_CUSTOMER, TRAFFIC_TYPE, TERTIARY_CLEANING, EMPLOYEES, VISITORS, WASHBASIN, MALE_BATHROOM, FEMALE_BATHROOM, STATUS_ID, INVER_SOLICITADA, " +
                "NEXT_MONTH, IS_EDITABLE) VALUES ('" + hoy.ToString("yyyMMdd") + "', '" + idpais + "', '" + 3 + "', '"
                + fechaRequerida.ToString("yyyMMdd") + "', '" + clientid + "', '" + cmbVendedor.SelectedValue + "', '" + txtComentarios.Text + "', '" +
                txtCodigo.Text + "', '" + tablaTemp.Rows[0]["SEGMENT_ID"].ToString() + "', '" + cmbSubSegmento.SelectedValue + "', '" + txtDireccion.Text + "', '" +
                txtBarrio.Text + "', '" + cmbCiudad.SelectedValue + "', '" + cmbCiudad.Text + "', '" + txtCodigoPostal.Text + "', '" +
                txtTelefono.Text + "', '" + txtPersonaContacto.Text + "', '" + txtCorreoContacto.Text + "', '" + txtPosicion.Text + "', '" +
                cmbCondicionPago.SelectedValue + "', '" + cmbFrecuenciaCompra.SelectedValue + "', '" + cmbFrecuenciaMantenimiento.SelectedValue + "', '" +
                cmbClienteEstrategico.SelectedValue + "', '" + cmbTipoTrafico.SelectedValue + "', '" + cmbLimpiezaTercerizada.SelectedValue + "', " +
                Convert.ToInt32(txtCantidadEmpleados.Text) + ", " + Convert.ToInt32(txtCantidadVisitantes.Text) + ", " + Convert.ToInt32(txtCantidadLavatorios.Text) + ", " +
                Convert.ToInt32(txtBañoHombre.Text) + ", " + Convert.ToInt32(txtBañoMujer.Text) + ", 1, " + valorDetalle2 + ", '" + paraSiguienteMes + "', '" +
                esEditable + "')";

                #endregion

                #region Inserccion
                string dispenserId = String.Empty;

                //Lista para el correo
                List<string> datosGenerales = new List<string>();

                if (conexion.setDispenserGeneral(query))
                {
                    string codigoCliente = conexion.getClientKCInfo("SUBSIDIARY_ID", "CLIENT_ID", clientid);
                    dispenserId = conexion.getdispenserReqId(hoy.ToString("yyyMMdd"), codigoCliente);

                    if (conexion.setDescripcionDis(dispenserId, codigoDispensadores, codigoProducto, cantidadDispensadores, cantidadProductos, costoDetalle))
                    {
                        double valDetTemp = valorDetalle2;
                        valorDetalle2 += Convert.ToDouble(Session.Contents["pendiente"].ToString());

                        string query2 = String.Format("UPDATE CLIENTES_KC SET INVERSION_FLOTANTE = {0} WHERE CLIENT_ID = '{1}'", valorDetalle2, clientid);
                        conexion.updateClientesFinales(query2);

                        btEnviar.Visible = false;
                        grdDispensadoresProducto.Visible = false;

                        main.Visible = false;
                        string nombrecliente = conexion.getClientKCInfo("CLIENT_NAME", "CLIENT_ID", clientid);


                        //Para el correo electronico
                        datosGenerales.Add(nombrecliente);//Indice 0
                        datosGenerales.Add(conexion.getEndClientInfo("TRADE_NAME", "END_USER_ID", txtNombreComercial.Text, "CLIENT_ID", clientid)); //Indice 1
                        datosGenerales.Add(conexion.getUsersInfo("USER_NAME", "USER_ID", Session.Contents["userid"].ToString()));//indice 2
                        datosGenerales.Add(hoy.ToString("dd/MM/yyy"));//indice 3
                        datosGenerales.Add(fechaRequerida.ToString("dd/MM/yyy"));//indice 4
                        datosGenerales.Add(txtMotivo.Text);//indice 5
                        datosGenerales.Add(txtDireccion.Text);//indice 6
                        datosGenerales.Add(txtPersonaContacto.Text);//inidice 7
                        datosGenerales.Add(txtTelefonoContacto.Text);//incice 8
                        datosGenerales.Add(txtComentarios.Text);//indice 9

                        if (conexion.enviarEmail(datosGenerales, codigoDispensadores, codigoProducto, cantidadDispensadores, cantidadProductos, idpais,
                             clientid, paraSiguienteMes))
                        {
                            datosGenerales.Clear();
                            codigoDispensadores.Clear();
                            codigoProducto.Clear();
                            cantidadDispensadores.Clear();
                            cantidadProductos.Clear();
                            costoDetalle.Clear();
                            
                            if (!paraSiguienteMes)
                                radajaxmanager.ResponseScripts.Add(String.Format("alerta3('{0}');", valDetTemp));
                            else
                                radajaxmanager.ResponseScripts.Add(String.Format("alerta4('{0}');", valDetTemp));
                        }
                        else
                        {
                            datosGenerales.Clear();
                            codigoDispensadores.Clear();
                            codigoProducto.Clear();
                            cantidadDispensadores.Clear();
                            cantidadProductos.Clear();
                            costoDetalle.Clear();
                            
                            if (!paraSiguienteMes)
                                radajaxmanager.ResponseScripts.Add(String.Format("errorEnvio(\"La solicitud fue creada costo total ${0}, sin aviso por correo.\");", valDetTemp));
                            else
                                radajaxmanager.ResponseScripts.Add(String.Format("errorEnvio(\"La solicitud fue creada costo total ${0} y movida al siguiente mes presupuesto excedido, sin aviso por correo.\");", valDetTemp));
                        }
                    }
                    else
                    {

                        string queryError = String.Format("DELETE SOLICITUD_DISPENSADORES WHERE DR_ID = '{0}'", dispenserId);
                        conexion.setDispenserGeneral(queryError);

                        codigoDispensadores.Clear();
                        codigoProducto.Clear();
                        cantidadDispensadores.Clear();
                        cantidadProductos.Clear();
                        costoDetalle.Clear();
                        datosGenerales.Clear();
                        radajaxmanager.ResponseScripts.Add(@"alert('Error al guardar la Descripcion de solicitud intentelo mas tarde.');");

                        return;
                    }

                }
                else
                {
                    codigoDispensadores.Clear();
                    codigoProducto.Clear();
                    cantidadDispensadores.Clear();
                    cantidadProductos.Clear();
                    costoDetalle.Clear();
                    datosGenerales.Clear();
                    radajaxmanager.ResponseScripts.Add(@"alert('Error al guardar la solicitud intentelo mas tarde.');");

                    return;
                }

                #endregion
            }
            catch (Exception error)
            {
                radajaxmanager.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }
        }

        protected void CustomValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {

                CustomValidator custom = (CustomValidator)source;

                TextBox textbox = (TextBox)custom.FindControl(custom.ControlToValidate);

                if (textbox.Text.Contains(',') || textbox.Text.Contains('\'') || textbox.Text.Contains('-') || textbox.Text.Contains(';'))
                {
                    args.IsValid = false;
                    return;
                }

                args.IsValid = true;
            }
            catch
            {
                args.IsValid = false;
                return;
            }
        }

        protected void cmbProducto_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
        {
            e.Item.Text = ((DataRowView)e.Item.DataItem)["PRODUCT_DESCRIP"].ToString();
            e.Item.Value = ((DataRowView)e.Item.DataItem)["PRODUCT_ID"].ToString();
        }

        protected void grdDispensadoresProducto_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Connection conexion = new Connection();

                string idpais = conexion.getUserCountry(Session["userid"].ToString());

                string query = String.Format("SELECT D.ID_DISPENSER, D.DESCRIPTION, D.DISPENSER_FACE, D.DISPENSER_TYPE, DP.DISPENSER_PRICE FROM DISPENSADORES AS D " +
                    "JOIN DISPENSADOR_PAIS AS DP ON D.ID_DISPENSER = DP.DISPENSER_ID WHERE DP.DISPENSER_STATUS = 'TRUE' AND DP.COUNTRY_ID = '{0}'", idpais);
                grdDispensadoresProducto.DataSource = conexion.getGridDataSource(query);
            }
            catch (Exception error)
            {
                radajaxmanager.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }
        }

        protected void RadScriptManager1_AsyncPostBackError(object sender, AsyncPostBackErrorEventArgs e)
        {
            RadScriptManager1.AsyncPostBackErrorMessage = "Error inesperado:\n" + e.Exception.Message;
        }
        #endregion

        #region Funciones
        protected void setDataSources()
        {
            Connection conexion = new Connection();
            string clientid = conexion.getUsersInfo("CLIENT_ID", "USER_ID", Session["userid"].ToString());

            sqlCiudad.ConnectionString = conexion.getConnectionString();
            sqlCiudad.SelectCommand = String.Format("SELECT DIVISION_NAME, CITY_NAME FROM DIVISION_TERRITORIAL WHERE COUNTRY_ID = '{0}'", conexion.getUserCountry(Session.Contents["userid"].ToString()));

            sqlVendedores.ConnectionString = conexion.getConnectionString();
            sqlVendedores.SelectCommand = String.Format("SELECT SALES_ID, SALES_NAME FROM VENDEDORES WHERE CLIENT_ID = '{0}'", clientid);

            sqlSegmento.ConnectionString = conexion.getConnectionString();
            sqlSegmento.SelectCommand = String.Format("SELECT SUB_SEGMENT_ID, SUB_SEGMENT_NAME FROM SUB_SEGMENTOS WHERE SUB_SEGMENT_ID <> 0");

            sqlCondicionesPago.ConnectionString = conexion.getConnectionString();
            sqlCondicionesPago.SelectCommand = String.Format("SELECT ID_PAYMENT_CONDITION, PAYMENT_CONDITION FROM CONDICIONES_PAGO");

        }

        protected DataTable funcion(string idDispensador)
        {
            try
            {
                Connection conexion = new Connection();

                string idpais = conexion.getUserCountry(Session["userid"].ToString());
                string query = String.Format("SELECT PRODUCT_ID, PRODUCT_DESCRIP FROM PRODUCTOS WHERE DISPENSER_ID = '{0}' AND COUNTRY_ID = '{1}'", idDispensador, idpais);
                return conexion.getGridDataSource(query);
            }
            catch (Exception error)
            {
                radajaxmanager.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }

            return new DataTable();
        }

        private string quitarCeros(string codigo)
        {
            try
            {

                string cadena = String.Empty;
                char []arreglo = codigo.ToCharArray();
                int longitud = 0;
                for (int i = 0; i < codigo.Length; i++)
                {
                    if (arreglo[i].Equals('0'))
                        longitud++;
                    else
                        break;
                }

                cadena = codigo.Substring(longitud);
                return cadena;
            }
            catch (Exception error)
            {
                radajaxmanager.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }

            return String.Empty;

        }
        #endregion

    }
}