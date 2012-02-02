using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//Agregados
using System.Data;
using System.Data.SqlClient;
using Telerik.Web.UI;
using System.IO;
using System.Text;

namespace Dispenser.Dst
{
    public partial class Solicitud_Dispensadores : System.Web.UI.Page
    {

        #region Globales

        //Listas para los cambios de datos
        static List<string> datos;
        static List<string> campos;

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

                item3.Text = "Clientes Nuevos";
                item3.NavigateUrl = "Solicitud_Clientes_Nuevos.aspx";

                item2.ChildItems.Add(item3);
                item2.Text = "Solicitud Dispensadores";
                item2.Selectable = false;
                
                item.Text = "Seguimiento Solicitudes";
                item.NavigateUrl = "SeguimientoSolicitudes.aspx";
                menu.Items[1].ChildItems.Add(item2);
                menu.Items[1].ChildItems.Add(item);

                Connection conexion = new Connection();

                if (Convert.ToDouble(Session.Contents["presupuesto"].ToString()) <= 0)
                    radajaxmanager.ResponseScripts.Add(@"sinTP();");

                if (!IsPostBack)
                {

                    if (Session.Contents["rol"].ToString().Equals("KCPADM") || Session.Contents["rol"].ToString().Equals("KCPCCR"))
                        Response.Redirect("../Default.aspx");
                    
                    
                    DateTime hoy = DateTime.Now;

                    dpFechaSolicitada.MinDate = hoy.AddDays(7);
                    dpFechaSolicitada.SelectedDate = hoy.AddDays(7);

                    //cargarRazones();
                    //cargarClientesFinales();

                    setDataSources();

                    //Listas para los cambios de datos
                    datos = new List<string>();
                    campos = new List<string>();

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

        #region Eventos

        protected void cmbNombreComercial_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                codigoDispensadores.Clear();
                codigoProducto.Clear();
                cantidadDispensadores.Clear();
                cantidadProductos.Clear();
                costoDetalle.Clear();
                datos.Clear();
                campos.Clear();

                if (cmbNombreComercial.SelectedValue.Equals(String.Empty))
                {
                    porDefecto();
                    radajaxmanager.ResponseScripts.Add(@"alert('Valor no valido, seleccione un valor de la lista!');");
                    return;
                }

                inicializarComponentes();

                cargarData(cmbNombreComercial.SelectedValue);
                //cargarVendedores(cmbNombreComercial.SelectedValue);
                //cargarCondicionesPago(cmbNombreComercial.SelectedValue);
                //cargarSubsegmentos(cmbNombreComercial.SelectedValue);
                //cargarCiudades(cmbNombreComercial.SelectedValue);

            }
            catch (Exception error)
            {
                radajaxmanager.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }
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

        protected void btEnviar_Click(object sender, EventArgs e)
        {

            try
            {
                if (!CustomValidator1.IsValid || !CustomValidator2.IsValid || !CustomValidator3.IsValid || !CustomValidator4.IsValid)
                {
                    radajaxmanager.ResponseScripts.Add(@"alert('Existen conflictos en campos requeridos favor verifique.');");
                    return;
                }
                
                Connection conexion = new Connection();
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

                /*if (cmbMotivos.SelectedValue.Equals("3"))
                {
                    string queryUser = "INSERT INTO CLIENTES_FINALES (END_USER_ID, CLIENT_ID, TRADE_NAME, SOCIAL_REASON, CORPORATE_ID, E_MAIL, TELEPHONE," +
                    " SEGMENT_ID, SUB_SEGMENT_ID, ADDRESS, NEIGHBOR, STATE, CITY, POSTAL_CODE, ID_PAYMENT_CONDITION, PURCHASE_FREQUENCY, MAINTENANCE_FREQUENCY, " +
                    "STRATEGIC_CUSTOMER, TRAFFIC_TYPE, TERTIARY_CLEANING, EMPLOYEES, VISITORS, WASHBASIN, MALE_BATH, FEMALE_BATH, CONTACT_PERSON, " +
                    "CONTACT_TELEPHONE, CONTACT_MAIL, CONTACT_POSITION, SELLER_ID, CLIENT_STATUS) VALUES ('" + txtCodigoClienteFinal.Text + "', '" +
                    clientid + "', '" + txtNombreComercial.Text + "', '" + txtRazonSocial.Text + "', '" + txtCedulaJuridica.Text + "', '" +
                    txtEMail.Text + "', '" + txtTelefono.Text + "', '" + cmbSegmento.SelectedValue + "', '" + cmbSubSegmento.SelectedValue + "', '" +
                    txtDireccion.Text + "', '" + txtBarrio.Text + "', '" + cmbDepartamento.SelectedValue + "', '" + cmbCiudad.SelectedValue + "', '" +
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
                        datos.Clear();
                        campos.Clear();
                        radajaxmanager.ResponseScripts.Add(@"alert('Error inesperado al crear el nuevo cliente.');");
                        return;
                    }

                    query = "INSERT INTO SOLICITUD_DISPENSADORES (DATE_REQUEST, ID_COUNTRY, REASON_ID, INSTALL_DATE, CLIENT_ID, SALES_ID, COMMENTS, END_USER_ID, SEGMENT_ID, SUB_SEGMENT_ID, ADDRESS, NEIGHBORHOOD, CITY, "
                    + "STATE, POSTAL_CODE, CONTACT_TELEPHONE, CONTACT_NAME, CONTACT_EMAIL, CONTACT_POSITION, ID_PAYMENT_CONDITION, PURCHASE_FREQUENCY, MAINTENANCE_FREQUENCY, "
                    + "STRATEGIC_CUSTOMER, TRAFFIC_TYPE, TERTIARY_CLEANING, EMPLOYEES, VISITORS, WASHBASIN, MALE_BATHROOM, FEMALE_BATHROOM, STATUS_ID, INVER_SOLICITADA, " +
                    "NEXT_MONTH, IS_EDITABLE) VALUES ('" + hoy.ToString("yyyMMdd") + "', '" + idpais + "', '" + cmbMotivos.SelectedValue + "', '"
                    + fechaRequerida.ToString("yyyMMdd") + "', '" + clientid + "', '" + cmbVendedor.SelectedValue + "', '" + txtComentarios.Text + "', '" +
                    txtCodigoClienteFinal.Text + "', '" + cmbSegmento.SelectedValue + "', '" + cmbSubSegmento.SelectedValue + "', '" + txtDireccion.Text + "', '" +
                    txtBarrio.Text + "', '" + cmbCiudad.SelectedValue + "', '" + cmbDepartamento.SelectedValue + "', '" + txtCodigoPostal.Text + "', '" +
                    txtTelefono.Text + "', '" + txtPersonaContacto.Text + "', '" + txtCorreoContacto.Text + "', '" + txtPosicion.Text + "', '" +
                    cmbCondicionPago.SelectedValue + "', '" + cmbFrecuenciaCompra.SelectedValue + "', '" + cmbFrecuenciaMantenimiento.SelectedValue + "', '" +
                    cmbClienteEstrategico.SelectedValue + "', '" + cmbTipoTrafico.SelectedValue + "', '" + cmbLimpiezaTercerizada.SelectedValue + "', " +
                    Convert.ToInt32(txtCantidadEmpleados.Text) + ", " + Convert.ToInt32(txtCantidadVisitantes.Text) + ", " + Convert.ToInt32(txtCantidadLavatorios.Text) + ", " +
                    Convert.ToInt32(txtBañoHombre.Text) + ", " + Convert.ToInt32(txtBañoMujer.Text) + ", 1, " + valorDetalle2 + ", '" + paraSiguienteMes + "', '" +
                    esEditable + "')";
                }
                else
                {*/
                    verificarUpdates();

                    string queryTemp = String.Format("SELECT SEGMENT_ID FROM SUB_SEGMENTOS WHERE SUB_SEGMENT_ID = '{0}'", cmbSubSegmento.SelectedValue);
                    DataTable tablaTemp = conexion.getGridDataSource(queryTemp);

                    query = "INSERT INTO SOLICITUD_DISPENSADORES (DATE_REQUEST, ID_COUNTRY, REASON_ID, INSTALL_DATE, CLIENT_ID, SALES_ID, COMMENTS, END_USER_ID, SEGMENT_ID, SUB_SEGMENT_ID, ADDRESS, NEIGHBORHOOD, CITY, "
                    + "STATE, POSTAL_CODE, CONTACT_TELEPHONE, CONTACT_NAME, CONTACT_EMAIL, CONTACT_POSITION, ID_PAYMENT_CONDITION, PURCHASE_FREQUENCY, MAINTENANCE_FREQUENCY, "
                    + "STRATEGIC_CUSTOMER, TRAFFIC_TYPE, TERTIARY_CLEANING, EMPLOYEES, VISITORS, WASHBASIN, MALE_BATHROOM, FEMALE_BATHROOM, STATUS_ID, INVER_SOLICITADA, "
                    + "NEXT_MONTH, IS_EDITABLE) VALUES ('" + hoy.ToString("yyyMMdd") + "', '" + idpais + "', '" + cmbMotivos.SelectedValue + "', '"
                    + fechaRequerida.ToString("yyyMMdd") + "', '" + clientid + "', '" + cmbVendedor.SelectedValue + "', '" + txtComentarios.Text + "', '" +
                    cmbNombreComercial.SelectedValue + "', '" + tablaTemp.Rows[0]["SEGMENT_ID"].ToString() + "', '" + cmbSubSegmento.SelectedValue + "', '" + txtDireccion.Text + "', '" +
                    txtBarrio.Text + "', '" + cmbCiudad.SelectedValue + "', '" + cmbCiudad.Text + "', '" + txtCodigoPostal.Text + "', '" +
                    txtTelefono.Text + "', '" + txtPersonaContacto.Text + "', '" + txtCorreoContacto.Text + "', '" + txtPosicion.Text + "', '" +
                    cmbCondicionPago.SelectedValue + "', '" + cmbFrecuenciaCompra.SelectedValue + "', '" + cmbFrecuenciaMantenimiento.SelectedValue + "', '" +
                    cmbClienteEstrategico.SelectedValue + "', '" + cmbTipoTrafico.SelectedValue + "', '" + cmbLimpiezaTercerizada.SelectedValue + "', " +
                    Convert.ToInt32(txtCantidadEmpleados.Text) + ", " + Convert.ToInt32(txtCantidadVisitantes.Text) + ", " + Convert.ToInt32(txtCantidadLavatorios.Text) + ", " +
                    Convert.ToInt32(txtBañoHombre.Text) + ", " + Convert.ToInt32(txtBañoMujer.Text) + ", 1, " + valorDetalle2 + ", '" + paraSiguienteMes + "', '" +
                    esEditable + "')";
                //}
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
                        datosGenerales.Add(conexion.getEndClientInfo("TRADE_NAME", "END_USER_ID", cmbNombreComercial.SelectedValue, "CLIENT_ID", clientid)); //Indice 1
                        datosGenerales.Add(conexion.getUsersInfo("USER_NAME", "USER_ID", Session.Contents["userid"].ToString()));//indice 2
                        datosGenerales.Add(hoy.ToString("dd/MM/yyy"));//indice 3
                        datosGenerales.Add(fechaRequerida.ToString("dd/MM/yyy"));//indice 4
                        datosGenerales.Add(cmbMotivos.Text);//indice 5
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
                            datos.Clear();
                            campos.Clear();

                            if (!paraSiguienteMes)
                                radajaxmanager.ResponseScripts.Add(String.Format("alerta('{0}');", valDetTemp));
                            else
                                radajaxmanager.ResponseScripts.Add(String.Format("alerta2('{0}');", valDetTemp));
                        }
                        else
                        {
                            datosGenerales.Clear();
                            codigoDispensadores.Clear();
                            codigoProducto.Clear();
                            cantidadDispensadores.Clear();
                            cantidadProductos.Clear();
                            costoDetalle.Clear();
                            datos.Clear();
                            campos.Clear();
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
                        datos.Clear();
                        campos.Clear();
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
                    datos.Clear();
                    campos.Clear();
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

        protected void cmbCiudad_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
        {
            e.Item.Text = ((DataRowView)e.Item.DataItem)["DIVISION_NAME"].ToString();
            e.Item.Value = ((DataRowView)e.Item.DataItem)["CITY_NAME"].ToString();
        }

        protected void cmbCiudad_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
        {
            sqlCiudad.SelectCommand = "SELECT DIVISION_NAME, CITY_NAME FROM DIVISION_TERRITORIAL WHERE CITY_NAME LIKE '%" + e.Text + "%'";
            //cmbCiudad.DataBind();
        }

        protected void cmbMotivos_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
        {
            e.Item.Text = ((DataRowView)e.Item.DataItem)["REASON_DESCRIP"].ToString();
            e.Item.Value = ((DataRowView)e.Item.DataItem)["ID_REASON"].ToString();
        }

        protected void cmbNombreComercial_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
        {
            e.Item.Text = ((DataRowView)e.Item.DataItem)["TRADE_NAME"].ToString();
            e.Item.Value = ((DataRowView)e.Item.DataItem)["END_USER_ID"].ToString();
        }

        protected void cmbNombreComercial_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
        {
            Connection conexion = new Connection();
            string clientid = conexion.getUsersInfo("CLIENT_ID", "USER_ID", Session["userid"].ToString());
            sqlCiudad.SelectCommand = String.Format("SELECT END_USER_ID, TRADE_NAME FROM CLIENTES_FINALES WHERE CLIENT_ID = '{0}' AND CLIENT_STATUS = 1 AND TRADE_NAME LIKE '%{1}%'", clientid, e.Text);
        }

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

        protected void cmbCondicionPago_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
        {
            e.Item.Text = ((DataRowView)e.Item.DataItem)["PAYMENT_CONDITION"].ToString();
            e.Item.Value = ((DataRowView)e.Item.DataItem)["ID_PAYMENT_CONDITION"].ToString();
        }
        #endregion

        #region Funciones
        protected void porDefecto()
        {
            cmbVendedor.Enabled = false;
            txtNombreComercial.Enabled = false;
            txtNombreComercial.Text = String.Empty;
            txtRazonSocial.Enabled = false;
            txtRazonSocial.Text = String.Empty;
            txtCedulaJuridica.Enabled = false;
            txtCedulaJuridica.Text = String.Empty;
            cmbSubSegmento.Enabled = false;
            txtDireccion.Enabled = false;
            txtDireccion.Text = String.Empty;
            txtBarrio.Enabled = false;
            txtBarrio.Text = String.Empty;
            cmbCiudad.Enabled = false;
            txtCodigoPostal.Enabled = false;
            txtCodigoPostal.Text = String.Empty;
            txtTelefono.Enabled = false;
            txtTelefono.Text = String.Empty;
            txtEMail.Enabled = false;
            txtEMail.Text = String.Empty;
            cmbCondicionPago.Enabled = false;
            cmbFrecuenciaCompra.Enabled = false;
            cmbFrecuenciaMantenimiento.Enabled = false;
            cmbClienteEstrategico.Enabled = false;
            cmbTipoTrafico.Enabled = false;
            cmbLimpiezaTercerizada.Enabled = false;
            txtCantidadEmpleados.Enabled = false;
            txtCantidadEmpleados.Text = "0";
            txtCantidadVisitantes.Enabled = false;
            txtCantidadVisitantes.Text = "0";
            txtCantidadLavatorios.Enabled = false;
            txtCantidadLavatorios.Text = "0";
            txtBañoHombre.Enabled = false;
            txtBañoHombre.Text = "0";
            txtBañoMujer.Enabled = false;
            txtBañoMujer.Text = "0";
            txtPersonaContacto.Enabled = false;
            txtPersonaContacto.Text = String.Empty;
            txtTelefonoContacto.Enabled = false;
            txtTelefonoContacto.Text = String.Empty;
            txtCorreoContacto.Enabled = false;
            txtCorreoContacto.Text = String.Empty;
            txtPosicion.Enabled = false;
            txtPosicion.Text = String.Empty;
            grdDispensadoresProducto.Enabled = false;
            btEnviar.Enabled = false;

            cmbVendedor.Items.Clear();
            cmbSubSegmento.Items.Clear();
            cmbCiudad.Items.Clear();
            cmbCondicionPago.Items.Clear();
            cmbNombreComercial.Items.FindItemByValue(String.Empty, true).Selected = true;
        }

        protected void cargarData(string codigo)
        {

            try
            {
                Connection conexion = new Connection();
                string listasDesplegables = String.Empty;
                string clientid = conexion.getUsersInfo("CLIENT_ID", "USER_ID", Session.Contents["userid"].ToString());
                string query = String.Format("SELECT * FROM CLIENTES_FINALES WHERE END_USER_ID = '{0}' AND CLIENT_ID = '{1}'"
                    , codigo, clientid);

                DataTable clientesFinales = conexion.getGridDataSource(query);

                string clienteEscogido = clientesFinales.Rows[0]["END_USER_ID"].ToString() + " | " + clientesFinales.Rows[0]["TRADE_NAME"].ToString();

                txtNombreComercial.Text = clientesFinales.Rows[0]["TRADE_NAME"].ToString();

                txtRazonSocial.Text = clientesFinales.Rows[0]["SOCIAL_REASON"].ToString();
                if (txtRazonSocial.Text.Equals("N/A"))
                    txtRazonSocial.Text = String.Empty;

                txtCedulaJuridica.Text = clientesFinales.Rows[0]["CORPORATE_ID"].ToString();
                if (txtCedulaJuridica.Text.Equals("N/A"))
                    txtCedulaJuridica.Text = String.Empty;

                txtDireccion.Text = clientesFinales.Rows[0]["ADDRESS"].ToString();
                if (txtDireccion.Text.Equals("N/A"))
                    txtDireccion.Text = String.Empty;

                txtBarrio.Text = clientesFinales.Rows[0]["NEIGHBOR"].ToString();
                if (txtBarrio.Text.Equals("N/A"))
                    txtBarrio.Text = String.Empty;

                txtCodigoPostal.Text = clientesFinales.Rows[0]["POSTAL_CODE"].ToString();
                if (txtCodigoPostal.Text.Equals("N/A"))
                    txtCodigoPostal.Text = String.Empty;

                txtTelefono.Text = clientesFinales.Rows[0]["TELEPHONE"].ToString();

                txtEMail.Text = clientesFinales.Rows[0]["E_MAIL"].ToString();
                if (txtEMail.Text.Equals("N/A"))
                    txtEMail.Text = String.Empty;

                txtCantidadEmpleados.Text = clientesFinales.Rows[0]["EMPLOYEES"].ToString();
                txtCantidadVisitantes.Text = clientesFinales.Rows[0]["VISITORS"].ToString();
                txtCantidadLavatorios.Text = clientesFinales.Rows[0]["WASHBASIN"].ToString();
                txtBañoHombre.Text = clientesFinales.Rows[0]["MALE_BATH"].ToString();
                txtBañoMujer.Text = clientesFinales.Rows[0]["FEMALE_BATH"].ToString();

                txtPersonaContacto.Text = clientesFinales.Rows[0]["CONTACT_PERSON"].ToString();
                if (txtPersonaContacto.Text.Equals("N/A"))
                    txtPersonaContacto.Text = String.Empty;

                txtTelefonoContacto.Text = clientesFinales.Rows[0]["CONTACT_TELEPHONE"].ToString();
                if (txtTelefonoContacto.Text.Equals("N/A"))
                    txtTelefonoContacto.Text = String.Empty;

                txtCorreoContacto.Text = clientesFinales.Rows[0]["CONTACT_MAIL"].ToString();
                if (txtCorreoContacto.Text.Equals("N/A"))
                    txtCorreoContacto.Text = String.Empty;

                txtPosicion.Text = clientesFinales.Rows[0]["CONTACT_POSITION"].ToString();
                if (txtPosicion.Text.Equals("N/A"))
                    txtPosicion.Text = String.Empty;

                listasDesplegables = clientesFinales.Rows[0]["SELLER_ID"].ToString();
                if(!listasDesplegables.Equals("N/A"))
                    cmbVendedor.FindItemByValue(listasDesplegables, true).Selected = true;

                listasDesplegables = clientesFinales.Rows[0]["SUB_SEGMENT_ID"].ToString();
                if (!listasDesplegables.Equals("0"))
                    cmbSubSegmento.FindItemByValue(listasDesplegables, false).Selected = true;
                else
                {
                    cmbSubSegmento.Text = String.Empty;
                    cmbSubSegmento.ClearSelection();
                }
                
                listasDesplegables = clientesFinales.Rows[0]["CITY"].ToString();
                if (!listasDesplegables.Equals("N/A"))
                    cmbCiudad.FindItemByValue(listasDesplegables, true).Selected = true;
                else
                {
                    cmbCiudad.Text = String.Empty;
                    cmbCiudad.ClearSelection();
                }

                listasDesplegables = clientesFinales.Rows[0]["ID_PAYMENT_CONDITION"].ToString();
                cmbCondicionPago.FindItemByValue(listasDesplegables, true).Selected = true;
                
                listasDesplegables = clientesFinales.Rows[0]["PURCHASE_FREQUENCY"].ToString();
                cmbFrecuenciaCompra.FindItemByText(listasDesplegables, true).Selected = true;

                listasDesplegables = clientesFinales.Rows[0]["MAINTENANCE_FREQUENCY"].ToString();
                cmbFrecuenciaMantenimiento.FindItemByValue(listasDesplegables, true).Selected = true;

                listasDesplegables = clientesFinales.Rows[0]["STRATEGIC_CUSTOMER"].ToString();
                cmbClienteEstrategico.FindItemByValue(listasDesplegables, true).Selected = true;

                listasDesplegables = clientesFinales.Rows[0]["TRAFFIC_TYPE"].ToString();
                cmbTipoTrafico.FindItemByText(listasDesplegables, true).Selected = true;

                listasDesplegables = clientesFinales.Rows[0]["TERTIARY_CLEANING"].ToString();
                cmbLimpiezaTercerizada.FindItemByValue(listasDesplegables, true).Selected = true;

                grdDispensadoresProducto.Enabled = true;
                btEnviar.Enabled = true;
            }
            catch (Exception error)
            {
                radajaxmanager.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }

        }

        protected void inicializarComponentes()
        {

            cmbVendedor.Enabled = true;

            txtNombreComercial.Enabled = true;
            txtRazonSocial.Enabled = true;
            txtCedulaJuridica.Enabled = true;
            cmbSubSegmento.Enabled = true;
            txtDireccion.Enabled = true;
            txtBarrio.Enabled = true;
            cmbCiudad.Enabled = true;
            txtCodigoPostal.Enabled = true;
            txtTelefono.Enabled = true;
            txtEMail.Enabled = true;
            cmbCondicionPago.Enabled = true;
            cmbFrecuenciaCompra.Enabled = true;
            cmbFrecuenciaMantenimiento.Enabled = true;
            cmbClienteEstrategico.Enabled = true;
            cmbTipoTrafico.Enabled = true;
            cmbLimpiezaTercerizada.Enabled = true;
            txtCantidadEmpleados.Enabled = true;
            txtCantidadVisitantes.Enabled = true;
            txtCantidadLavatorios.Enabled = true;
            txtBañoHombre.Enabled = true;
            txtBañoMujer.Enabled = true;
            txtPersonaContacto.Enabled = true;
            txtTelefonoContacto.Enabled = true;
            txtCorreoContacto.Enabled = true;
            txtPosicion.Enabled = true;
            grdDispensadoresProducto.Enabled = true;
            btEnviar.Enabled = true;
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

        protected void verificarUpdates()
        {

            if (campos.Count > 0 && datos.Count > 0)
            {
                if (campos.Count == datos.Count)
                {
                    for (int i = 0; i < campos.Count; i++)
                    {
                        if (!datos[i].Equals(String.Empty))
                            mandarActualizar(campos[i], datos[i]);
                    }
                }
            }
        }

        protected bool mandarActualizar(string campo, string valor)
        {
            try
            {
                Connection conexion = new Connection();

                string clientid = conexion.getUsersInfo("CLIENT_ID", "USER_ID", Session["userid"].ToString());
                string query = String.Format("UPDATE CLIENTES_FINALES SET {0} = '{1}' WHERE CLIENT_ID = '{2}' AND END_USER_ID = '{3}'", campo, valor, clientid, cmbNombreComercial.SelectedValue);

                if (conexion.updateClientesFinales(query))
                    return true;
            }
            catch (Exception error)
            {
                radajaxmanager.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }
            return false;
        }

        protected void setDataSources()
        {
            Connection conexion = new Connection();
            
            sqlCiudad.ConnectionString = conexion.getConnectionString();
            sqlCiudad.SelectCommand = String.Format("SELECT DIVISION_NAME, CITY_NAME FROM DIVISION_TERRITORIAL WHERE COUNTRY_ID = '{0}'", conexion.getUserCountry(Session.Contents["userid"].ToString()));

            sqlMotivos.ConnectionString = conexion.getConnectionString();
            sqlMotivos.SelectCommand = String.Format("SELECT ID_REASON, REASON_DESCRIP FROM RAZONES WHERE ID_REASON <> 3");

            sqlClientes.ConnectionString = conexion.getConnectionString();
            string clientid = conexion.getUsersInfo("CLIENT_ID", "USER_ID", Session["userid"].ToString());
            sqlClientes.SelectCommand = String.Format("SELECT END_USER_ID, TRADE_NAME FROM CLIENTES_FINALES WHERE CLIENT_ID = '{0}' AND CLIENT_STATUS = 1", clientid);

            sqlVendedores.ConnectionString = conexion.getConnectionString();
            sqlVendedores.SelectCommand = String.Format("SELECT SALES_ID, SALES_NAME FROM VENDEDORES WHERE CLIENT_ID = '{0}'", clientid);

            sqlSegmento.ConnectionString = conexion.getConnectionString();
            sqlSegmento.SelectCommand = String.Format("SELECT SUB_SEGMENT_ID, SUB_SEGMENT_NAME FROM SUB_SEGMENTOS WHERE SUB_SEGMENT_ID <> 0 ORDER BY SUB_SEGMENT_ID ASC");

            sqlCondicionesPago.ConnectionString = conexion.getConnectionString();
            sqlCondicionesPago.SelectCommand = String.Format("SELECT ID_PAYMENT_CONDITION, PAYMENT_CONDITION FROM CONDICIONES_PAGO");

            
        }
        #endregion

        #region Eventos de cambio de datos (modificacion)

        protected void txtCedulaJuridica_TextChanged1(object sender, EventArgs e)
        {
            campos.Add("CORPORATE_ID");
            datos.Add(txtCedulaJuridica.Text);
        }

        protected void txtNombreComercial_TextChanged2(object sender, EventArgs e)
        {
            campos.Add("TRADE_NAME");
            datos.Add(txtNombreComercial.Text);
        }

        protected void txtRazonSocial_TextChanged1(object sender, EventArgs e)
        {
            campos.Add("SOCIAL_REASON");
            datos.Add(txtRazonSocial.Text);
        }

        protected void txtDireccion_TextChanged1(object sender, EventArgs e)
        {
            campos.Add("ADDRESS");
            datos.Add(txtDireccion.Text);
        }

        protected void txtBarrio_TextChanged(object sender, EventArgs e)
        {
            campos.Add("NEIGHBOR");
            datos.Add(txtBarrio.Text);
        }

        protected void txtEMail_TextChanged1(object sender, EventArgs e)
        {
            campos.Add("E_MAIL");
            datos.Add(txtEMail.Text);
        }

        protected void txtPersonaContacto_TextChanged1(object sender, EventArgs e)
        {
            campos.Add("CONTACT_PERSON");
            datos.Add(txtPersonaContacto.Text);
        }

        protected void txtCorreoContacto_TextChanged1(object sender, EventArgs e)
        {
            campos.Add("CONTACT_MAIL");
            datos.Add(txtCorreoContacto.Text);
        }

        protected void txtTelefonoContacto_TextChanged(object sender, EventArgs e)
        {
            campos.Add("CONTACT_TELEPHONE");
            datos.Add(txtTelefonoContacto.Text);
        }

        protected void txtCodigoPostal_TextChanged(object sender, EventArgs e)
        {
            campos.Add("POSTAL_CODE");
            datos.Add(txtCodigoPostal.Text);
        }

        protected void txtTelefono_TextChanged1(object sender, EventArgs e)
        {
            campos.Add("TELEPHONE");
            datos.Add(txtTelefono.Text);
        }

        protected void txtCantidadEmpleados_TextChanged(object sender, EventArgs e)
        {
            campos.Add("EMPLOYEES");
            datos.Add(txtCantidadEmpleados.Text);
        }

        protected void txtCantidadVisitantes_TextChanged(object sender, EventArgs e)
        {
            campos.Add("VISITORS");
            datos.Add(txtCantidadVisitantes.Text);
        }

        protected void txtCantidadLavatorios_TextChanged(object sender, EventArgs e)
        {
            campos.Add("WASHBASIN");
            datos.Add(txtCantidadLavatorios.Text);
        }

        protected void txtBañoHombre_TextChanged(object sender, EventArgs e)
        {
            campos.Add("MALE_BATH");
            datos.Add(txtBañoHombre.Text);
        }

        protected void txtBañoMujer_TextChanged(object sender, EventArgs e)
        {
            campos.Add("FEMALE_BATH");
            datos.Add(txtBañoMujer.Text);
        }

        protected void txtPosicion_TextChanged(object sender, EventArgs e)
        {
            campos.Add("CONTACT_POSITION");
            datos.Add(txtPosicion.Text);
        }

        protected void cmbVendedor_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            campos.Add("SELLER_ID");
            datos.Add(cmbVendedor.SelectedValue);
        }

        protected void cmbSubSegmento_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                Connection conexion = new Connection();

                string query = String.Format("SELECT SEGMENT_ID FROM SUB_SEGMENTOS WHERE SUB_SEGMENT_ID = '{0}'", cmbSubSegmento.SelectedValue);
                DataTable segmento = conexion.getGridDataSource(query);

                campos.Add("SEGMENT_ID");
                datos.Add(segmento.Rows[0]["SEGMENT_ID"].ToString());

                campos.Add("SUB_SEGMENT_ID");
                datos.Add(cmbSubSegmento.SelectedValue);
            }
            catch (Exception error)
            {
                radajaxmanager.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }
        }

        protected void cmbCiudad_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            Connection conexion = new Connection();

            string departamentoActual = conexion.getEndClientInfo("STATE", "END_USER_ID", cmbNombreComercial.SelectedValue, "CLIENT_ID", conexion.getUsersInfo("CLIENT_ID", "USER_ID", Session.Contents["userid"].ToString()));
            
            if (!departamentoActual.Equals(cmbCiudad.Text))
            {
                campos.Add("STATE");
                datos.Add(cmbCiudad.Text);
            }

            campos.Add("CITY");
            datos.Add(cmbCiudad.SelectedValue);
        }

        protected void cmbCondicionPago_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            campos.Add("ID_PAYMENT_CONDITION");
            datos.Add(cmbCondicionPago.SelectedValue);
        }

        protected void cmbFrecuenciaCompra_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            campos.Add("PURCHASE_FREQUENCY");
            datos.Add(cmbFrecuenciaCompra.SelectedValue);
        }

        protected void cmbFrecuenciaMantenimiento_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            campos.Add("PURCHASE_FREQUENCY");
            datos.Add(cmbFrecuenciaCompra.SelectedValue);
        }

        protected void cmbClienteEstrategico_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            campos.Add("STRATEGIC_CUSTOMER");
            datos.Add(cmbClienteEstrategico.SelectedValue);
        }

        protected void cmbTipoTrafico_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            campos.Add("TRAFFIC_TYPE");
            datos.Add(cmbTipoTrafico.SelectedValue);
        }

        protected void cmbLimpiezaTercerizada_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            campos.Add("TERTIARY_CLEANING");
            datos.Add(cmbLimpiezaTercerizada.SelectedValue);
        }
        #endregion
        
    }
}