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

namespace Dispenser.Kcp
{
    public partial class AutorizacionSolicitudes : System.Web.UI.Page
    {

        #region Globales
        static string pathAbsoluto = String.Empty;

        static List<string> campos;
        static List<string> valores;

        static List<string> camposDetalle;
        static List<string> valoresDetalle;

        static List<string> codigoDispensador;
        static List<string> codigoProducto;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                if (Session.Contents["rol"].ToString().Equals("DSTADM"))
                    Response.Redirect("../Default.aspx");

                campos = new List<string>();
                valores = new List<string>();

                camposDetalle = new List<string>();
                valoresDetalle = new List<string>();

                codigoDispensador = new List<string>();
                codigoProducto = new List<string>();

                cargarDataSources();
                
            }
        }

        #region Tab 1
        protected void grdAutorizaciones_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                Connection conexion = new Connection();

                string userId = Session["userid"].ToString();
                string Pais = conexion.getUsersInfo("ID_COUNTRY", "USER_ID", userId);

                int mesActual = DateTime.Now.Month;
                int mesHistorico = 0;
                int anioHistorico = 0;

                switch (mesActual)
                {
                    case 1:
                        mesHistorico = 11;  //Mes de noviembre del año anterior
                        anioHistorico = DateTime.Now.Year - 1;
                        break;
                    case 2:
                        mesHistorico = 12; //Mes de diciembre del año anterior
                        anioHistorico = DateTime.Now.Year - 1;
                        break;
                    default:
                        mesHistorico = mesActual - 2; //Se calculan dos meses anterirores 
                        anioHistorico = DateTime.Now.Year;
                        break;
                }

                DateTime fechaHistorico = new DateTime(anioHistorico, mesHistorico, 1);

                string query = String.Format("SELECT SD.DR_ID, SD.DATE_REQUEST, R.REASON_DESCRIP, SD.INSTALL_DATE, SD.CLIENT_ID, CKC.CLIENT_NAME," +
                        " (SELECT CF.TRADE_NAME FROM CLIENTES_FINALES AS CF WHERE CF.END_USER_ID = SD.END_USER_ID AND CF.CLIENT_ID = SD.CLIENT_ID) AS TRADE_NAME, " +
                        " S.SEGMENT_NAME, ST.STATUS_DESCRIP, SD.COMMENTS, SD.PROGRAMMING_DATE, SD.NEXT_MONTH, SD.INVER_SOLICITADA, SD.INVER_APRO " +
                        " FROM SOLICITUD_DISPENSADORES AS SD JOIN RAZONES AS R ON SD.REASON_ID = R.ID_REASON" +
                        " JOIN SEGMENTOS AS S ON SD.SEGMENT_ID = S.SEGMENT_ID JOIN STATUS_DESCRIP AS ST ON SD.STATUS_ID = ST.STATUS_ID " +
                        " JOIN CLIENTES_KC AS CKC ON SD.CLIENT_ID = CKC.CLIENT_ID WHERE SD.ID_COUNTRY = '{0}' AND SD.DATE_REQUEST BETWEEN '{1}' AND '{2}' ORDER BY SD.DR_ID DESC",
                        Pais, fechaHistorico.ToString("yyyMMdd"), DateTime.Now.ToString("yyyMMdd"));


                grdAutorizaciones.DataSource = conexion.getGridDataSource(query);
            }
            catch (Exception error)
            {
                radajaxmanager.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }
        }

        protected void grdAutorizaciones_DetailTableDataBind(object source, Telerik.Web.UI.GridDetailTableDataBindEventArgs e)
        {
            try
            {
                Connection conexion = new Connection();
                GridDataItem dataitem = (GridDataItem)e.DetailTableView.ParentItem;

                switch (e.DetailTableView.Name)
                {
                    case "Solicitud":
                        {
                            string requestId = dataitem.GetDataKeyValue("DR_ID").ToString();
                            string query = String.Format("SELECT DD.DISPENSER_ID, DD.PRODUCT_ID, DD.DISPENSER_QUANTITY, DD.PRODUCT_QUANTITY, DD.APPROVAL_QTY," +
                            " SD.STATUS_DESCRIP, DD.INVERSION FROM DESCRIPCION_DISPENSADORES AS DD JOIN STATUS_DESCRIP AS SD ON DD.STATUS_ID = SD.STATUS_ID " +
                                " WHERE DR_ID = '{0}'", requestId);

                            e.DetailTableView.DataSource = conexion.getGridDataSource(query);
                            break;
                        }
                }
            }
            catch (Exception error)
            {
                radajaxmanager.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }
        }

        protected void btVer_Click(object sender, EventArgs e)
        {
            try
            {
                Connection conexion = new Connection();
                GridDataItem fila;

                fila = (sender as LinkButton).Parent.Parent as GridDataItem;
                TableCell numeroSolicitud = fila["Codigo"];
                TableCell estadoSolicitud = fila["Estado"];
                TableCell CodigoDistribuidor = fila["CodigoDistribuidor"];
                TableCell nombreDistribuidor = fila["nombreDistribuidor"];

                lblSolicitud.Text = String.Format("Solicitud No. {0}", numeroSolicitud.Text);
                lblCodigo.Text = String.Format("Codigo Cliente Final {0}", conexion.getSolicitudInfo("END_USER_ID", numeroSolicitud.Text));
                lblNombreDistribuidor.Text = nombreDistribuidor.Text;
                lblDescripcionEstado.Text = estadoSolicitud.Text;

                
                //Para tener presente estos valores.
                lblNumeroSol.Text = numeroSolicitud.Text;
                lblcodigoDis.Text = CodigoDistribuidor.Text;

                RadTabStrip1.Tabs[1].Selected = true;
                RadMultiPage1.PageViews[1].Selected = true;

                cargarData();
                grdDescripciones.Rebind();
            }
            catch (Exception error)
            {
                radajaxmanager.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }
        }
        #endregion

        #region Tab 2

        #region Eventos de controles
        protected void btAtras_Click(object sender, EventArgs e)
        {
            try
            {
                RadTabStrip1.Tabs[0].Selected = true;
                RadMultiPage1.PageViews[0].Selected = true;

                //Borrado de los campos que tubieron cambios y se dio el boton atras.
                campos.Clear();
                valores.Clear();
                camposDetalle.Clear();
                valoresDetalle.Clear();
            }
            catch (Exception error)
            {
                radajaxmanager.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }
        }

        protected void btCrear_Click(object sender, EventArgs e)
        {
            
            try
            {
                Connection conexion = new Connection();
                
                foreach (GridDataItem dataItem in grdDescripciones.MasterTableView.Items)
                {
                    TableCell celdaProducto = dataItem["CodigoProducto"];
                    TableCell celdaDispensador = dataItem["CodigoDispensador"];
                    string tempProducto = celdaProducto.Text;
                    string tempDispensador = celdaDispensador.Text;

                    if (cmbDispensador.SelectedValue.Equals(tempDispensador))
                    {

                        string queryProducto = String.Format("SELECT PRODUCT_ID FROM PRODUCTOS WHERE DISPENSER_ID = '{0}' AND COUNTRY_ID = '{1}'",
                            cmbDispensador.SelectedValue, conexion.getUserCountry(Session.Contents["userid"].ToString()));
                        DataTable codigosProducto = conexion.getGridDataSource(queryProducto);

                        foreach (DataRow fila in codigosProducto.Rows)
                        {
                            if (fila["PRODUCT_ID"].ToString().Equals(tempProducto))
                            {
                                radajaxmanager.ResponseScripts.Add(@"alert('Ya existe un detalle en la solicitud con estos requisitos.');");
                                return;
                            }
                        }
                    }
                }

                DataTable tabla = conexion.getGridDataSource(String.Format("SELECT DISPENSER_PRICE FROM DISPENSADOR_PAIS WHERE DISPENSER_ID = '{0}' AND COUNTRY_ID = '{1}'",
                    cmbDispensador.SelectedValue, conexion.getUserCountry(Session.Contents["userid"].ToString())));

                double precio = Convert.ToDouble(tabla.Rows[0]["DISPENSER_PRICE"].ToString());
                double inversion = precio * Convert.ToInt32(txtCantDis.Text);

                /*string query = String.Format("INSERT INTO DESCRIPCION_DISPENSADORES (DR_ID, DISPENSER_ID, PRODUCT_ID, DISPENSER_QUANTITY, PRODUCT_QUANTITY, STATUS_ID, INVERSION) VALUES " +
                    "({0}, '{1}', '{2}', {3}, {4}, 1, {5})", Convert.ToInt32(Session["solicitud"].ToString()), cmbDispensador.SelectedValue, cmbProducto.SelectedValue, Convert.ToInt32(txtCantDis.Text),
                    Convert.ToInt32(txtProducto.Text), inversion);

                string estadoActual = conexion.getSolicitudInfo("STATUS_ID", Session["solicitud"].ToString());

                //Se usa dispenser general ya que acepta un string como query se agrega el nuevo dispensador
                if (conexion.setDispenserGeneral(query))
                    RadAjaxManager1.ResponseScripts.Add(@"alert('Ingresado con exito.');");
                else
                    RadAjaxManager1.ResponseScripts.Add(@"alert('Error al insertar contactese con el administrador.');");

                //Recalcula la inversion despues de la inserccion
                recalcularInversion();

                query = String.Format("UPDATE SOLICITUD_DISPENSADORES SET STATUS_ID = 5 WHERE DR_ID = '{0}'", Session.Contents["solicitud"].ToString());

                if (estadoActual.Equals("1"))
                    if (conexion.Actualizar(query))
                        lblDescripcionEstado.Text = conexion.getStatusDescripInfo("STATUS_DESCRIP", "STATUS_ID", "5");


                cmbDispensador.Items.Clear();
                cmbProducto.Items.Clear();
                txtCantDis.Value = 0;
                txtProducto.Value = 0;

                mostrarOcultar(false);
                btNuevo.Visible = true;
                pnlDescripcionSol.Visible = true;
                btAprobar.Visible = true;
                btRechazar.Visible = true;
                btGuardar.Visible = true;
                btCerrarCita.Visible = true;

                grdDescripciones.Rebind();*/
            }
            catch (Exception error)
            {
                radajaxmanager.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }
        }

        protected void cmbDepartamento_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
        {
            e.Item.Text = ((DataRowView)e.Item.DataItem)["DIVISION_NAME"].ToString();
            e.Item.Value = ((DataRowView)e.Item.DataItem)["CITY_NAME"].ToString();
        }

        protected void cmbDepartamento_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            sqlEstadoCiudad.SelectCommand = "SELECT DIVISION_NAME, CITY_NAME FROM DIVISION_TERRITORIAL WHERE CITY_NAME LIKE '%" + e.Text + "%'";
        }

        protected void cmbSegmento_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
        {
            e.Item.Text = ((DataRowView)e.Item.DataItem)["SUB_SEGMENT_NAME"].ToString();
            e.Item.Value = ((DataRowView)e.Item.DataItem)["SUB_SEGMENT_ID"].ToString();
        }

        protected void cmbDispensador_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
        {
            e.Item.Text = ((DataRowView)e.Item.DataItem)["DESCRIPTION"].ToString();
            e.Item.Value = ((DataRowView)e.Item.DataItem)["ID_DISPENSER"].ToString();
        }

        protected void cmbDispensador_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            cargarProducto(cmbDispensador.SelectedValue);
        }

        private void cargarProducto(string dispenserID)
        {
            try
            {
                Connection conexion = new Connection();
                cmbProducto.EmptyMessage = "Seleccione un producto";
                cmbProducto.Items.Clear();

                string userCountry = conexion.getUserCountry(Session["userid"].ToString());

                string query = String.Format("SELECT PRODUCT_ID, PRODUCT_DESCRIP FROM PRODUCTOS WHERE DISPENSER_ID = '{0}' AND COUNTRY_ID = '{1}'", dispenserID, userCountry);

                try
                {
                    SqlConnection bridge = new SqlConnection(conexion.getConnectionString());
                    SqlDataAdapter adapter = new SqlDataAdapter(query, bridge);
                    DataTable datatable = new DataTable();
                    adapter.Fill(datatable);

                    foreach (DataRow datarow in datatable.Rows)
                    {
                        RadComboBoxItem item = new RadComboBoxItem();
                        item.Text = datarow["PRODUCT_ID"].ToString() + " " + datarow["PRODUCT_DESCRIP"].ToString();
                        item.Value = datarow["PRODUCT_ID"].ToString();

                        cmbProducto.Items.Add(item);
                        item.DataBind();
                    }
                }
                catch (SqlException)
                {
                    radajaxmanager.ResponseScripts.Add(@"alert('Error inesperado de conexion, no se logro cargar la lista de productos refresque la pagina" +
                        " o intentelo mas tarde.');");
                    return;
                }
            }
            catch (Exception error)
            {
                radajaxmanager.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }
        }
        #endregion

        #region Carga la data al grid de detalle
        protected void grdDescripciones_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Connection conexion = new Connection();

                string query = String.Format("SELECT DD.PRODUCT_ID, DD.PRODUCT_QUANTITY, DD.DISPENSER_ID, D.DESCRIPTION, DD.DISPENSER_QUANTITY, DD.INVERSION, " +
                    "(SELECT DP.DISPENSER_PRICE FROM DISPENSADOR_PAIS AS DP WHERE DP.DISPENSER_ID = DD.DISPENSER_ID AND DP.COUNTRY_ID = '{0}') AS UNIDAD "
                + "FROM DESCRIPCION_DISPENSADORES AS DD JOIN DISPENSADORES AS D ON D.ID_DISPENSER = DD.DISPENSER_ID WHERE DD.DR_ID = {1}",
                conexion.getUsersInfo("ID_COUNTRY", "USER_ID", Session.Contents["userid"].ToString()), lblNumeroSol.Text);
                grdDescripciones.DataSource = conexion.getGridDataSource(query);
            }
            catch (Exception error)
            {
                radajaxmanager.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }
        }

        protected DataTable comboInGrid(string dispensador, string producto)
        {
            try
            {
                Connection conexion = new Connection();

                int estadoActual = Convert.ToInt32(conexion.getDetalleSol("STATUS_ID", dispensador, producto, lblNumeroSol.Text));

                string query = "SELECT STATUS_ID, STATUS_DESCRIP FROM STATUS_DESCRIP WHERE STATUS_ID BETWEEN 1 AND 3";
                DataTable temp = conexion.getGridDataSource(query);
                DataTable miDataTable = new DataTable();

                miDataTable.Columns.Add("STATUS_ID");
                miDataTable.Columns.Add("STATUS_DESCRIP");

                foreach (DataRow data in temp.Rows)
                {
                    int estado = Convert.ToInt32(data["STATUS_ID"].ToString());

                    if (estado == estadoActual)
                        miDataTable.ImportRow(data);
                }

                foreach (DataRow data in temp.Rows)
                {
                    int estado = Convert.ToInt32(data["STATUS_ID"].ToString());

                    if (estado != estadoActual)
                        miDataTable.ImportRow(data);
                }

                return miDataTable;
            }
            catch (Exception error)
            {
                radajaxmanager.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }

            return new DataTable();
        }

        protected double cantidad(string dispensador, string producto)
        {
            try
            {
                Connection conexion = new Connection();

                string cantidad = conexion.getDetalleSol("APPROVAL_QTY", dispensador, producto, lblNumeroSol.Text);
                double cantidadAutorizada = 0;
                double cantidadDispensador = Convert.ToDouble(conexion.getDetalleSol("DISPENSER_QUANTITY", dispensador, producto, lblNumeroSol.Text));

                if (!cantidad.Equals(String.Empty))
                    cantidadAutorizada = Convert.ToDouble(cantidad);

                if (cantidadAutorizada > 0)
                    return cantidadAutorizada;
                else
                {
                    string query = String.Format("UPDATE DESCRIPCION_DISPENSADORES SET APPROVAL_QTY = {0} WHERE DR_ID = '{1}' AND DISPENSER_ID = '{2}' AND PRODUCT_ID = '{3}'",
                        cantidadDispensador, lblNumeroSol.Text, dispensador, producto);

                    conexion.Actualizar(query);
                    return cantidadDispensador;
                }
            }
            catch (Exception error)
            {
                radajaxmanager.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }

            return 0;
        }

        protected string comentarios(string dispensador, string producto)
        {
            try
            {
                Connection conexion = new Connection();
                return conexion.getDetalleSol("ADMIN_COMMENT", dispensador, producto, lblNumeroSol.Text);
            }
            catch (Exception error)
            {
                radajaxmanager.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }

            return String.Empty;
        }
        #endregion

        #region Cambios en la solicitud

        /*---------------- Controles dentro del grid ----------------*/
        protected void cmbEstados_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                camposDetalle.Add("STATUS_ID");
                valoresDetalle.Add((o as RadComboBox).SelectedValue);

                GridDataItem fila = (o as RadComboBox).Parent.Parent as GridDataItem;
                TableCell celda = fila["CodigoDispensador"];
                TableCell celda2 = fila["CodigoProducto"];

                codigoProducto.Add(celda2.Text);
                codigoDispensador.Add(celda.Text);
            }
            catch (Exception error)
            {
                radajaxmanager.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }
        }

        protected void txtDisAprobados_TextChanged(object sender, EventArgs e)
        {
            try
            {
                camposDetalle.Add("APPROVAL_QTY");
                valoresDetalle.Add((sender as RadNumericTextBox).Text);

                GridDataItem fila = (sender as RadNumericTextBox).Parent.Parent as GridDataItem;
                TableCell celda = fila["CodigoDispensador"];
                TableCell celda2 = fila["CodigoProducto"];

                codigoProducto.Add(celda2.Text);
                codigoDispensador.Add(celda.Text);
            }
            catch (Exception error)
            {
                radajaxmanager.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }
        }

        protected void txtComentariosGrid_TextChanged(object sender, EventArgs e)
        {
            try
            {
                camposDetalle.Add("ADMIN_COMMENT");
                valoresDetalle.Add((sender as RadTextBox).Text);

                GridDataItem fila = (sender as RadTextBox).Parent.Parent as GridDataItem;
                TableCell celda = fila["CodigoDispensador"];
                TableCell celda2 = fila["CodigoProducto"];

                codigoProducto.Add(celda2.Text);
                codigoDispensador.Add(celda.Text);
            }
            catch (Exception error)
            {
                radajaxmanager.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }
        }
        /*-----------------------------------------------------------*/

        protected void txtNombreComercial_TextChanged(object sender, EventArgs e)
        {
            campos.Add("TRADE_NAME");
            valores.Add(txtNombreComercial.Text);
        }

        protected void txtRazonSocial_TextChanged(object sender, EventArgs e)
        {
            campos.Add("SOCIAL_REASON");
            valores.Add(txtRazonSocial.Text);
        }

        protected void txtTelefono_TextChanged(object sender, EventArgs e)
        {
            campos.Add("TELEPHONE");
            valores.Add(txtTelefono.Text);
        }

        protected void txtCedulaJuridica_TextChanged(object sender, EventArgs e)
        {
            campos.Add("CORPORATE_ID");
            valores.Add(txtCedulaJuridica.Text);
        }

        protected void txtTelefonoContacto_TextChanged(object sender, EventArgs e)
        {
            campos.Add("CONTACT_TELEPHONE");
            valores.Add(txtTelefonoContacto.Text);
        }

        protected void txtPersonaContacto_TextChanged(object sender, EventArgs e)
        {
            campos.Add("CONTACT_PERSON");
            valores.Add(txtPersonaContacto.Text);
        }

        protected void cmbDepartamento_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {

            try
            {
                Connection conexion = new Connection();
                
                string departamentoActual = conexion.getEndClientInfo("STATE", "END_USER_ID", lblNumeroSol.Text, "CLIENT_ID", lblcodigoDis.Text);

                if (!departamentoActual.Equals(cmbDepartamento.Text))
                {
                    campos.Add("STATE");
                    valores.Add(cmbDepartamento.Text);
                }

                campos.Add("CITY");
                valores.Add(cmbDepartamento.SelectedValue);
            }
            catch (Exception error)
            {
                radajaxmanager.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }
        }

        protected void cmbSegmento_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                Connection conexion = new Connection();

                string query = String.Format("SELECT SEGMENT_ID FROM SUB_SEGMENTOS WHERE SUB_SEGMENT_ID = '{0}'", cmbSegmento.SelectedValue);
                DataTable segmento = conexion.getGridDataSource(query);

                campos.Add("SEGMENT_ID");
                valores.Add(segmento.Rows[0]["SEGMENT_ID"].ToString());

                campos.Add("SUB_SEGMENT_ID");
                valores.Add(cmbSegmento.SelectedValue);
            }
            catch (Exception error)
            {
                radajaxmanager.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }
        }

        protected void txtDireccion_TextChanged(object sender, EventArgs e)
        {
            campos.Add("ADDRESS");
            valores.Add(txtDireccion.Text);
        }

        #endregion

        #region Funciones
        private void cargarDataSources()
        {
            try
            {
                Connection conexion = new Connection();
                sqlEstadoCiudad.ConnectionString = conexion.getConnectionString();
                sqlEstadoCiudad.SelectCommand = String.Format("SELECT DIVISION_NAME, CITY_NAME FROM DIVISION_TERRITORIAL WHERE COUNTRY_ID = '{0}';", conexion.getUserCountry(Session.Contents["userid"].ToString()));

                sqlSegmentos.ConnectionString = conexion.getConnectionString();
                sqlSegmentos.SelectCommand = String.Format("SELECT SUB_SEGMENT_ID, SUB_SEGMENT_NAME FROM SUB_SEGMENTOS WHERE SUB_SEGMENT_ID <> 0 ORDER BY SUB_SEGMENT_ID ASC;");

                sqlDispensadorProducto.ConnectionString = conexion.getConnectionString();
                sqlDispensadorProducto.SelectCommand = String.Format("SELECT D.ID_DISPENSER, D.DESCRIPTION, D.DISPENSER_FACE FROM DISPENSADORES AS D JOIN DISPENSADOR_PAIS AS DP ON DP.DISPENSER_ID = D.ID_DISPENSER WHERE DP.COUNTRY_ID = '{0}' AND DP.DISPENSER_STATUS = 1 ORDER BY D.ID_DISPENSER ASC;",
                    conexion.getUserCountry(Session.Contents["userid"].ToString()));
            }
            catch (Exception error)
            {
                radajaxmanager.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }
        }

        private void cargarData()
        {
            try
            {
                Connection conexion = new Connection();

                string query = String.Format("SELECT R.REASON_DESCRIP, CF.TRADE_NAME, CF.SOCIAL_REASON, CF.CORPORATE_ID, CF.TELEPHONE, " +
                    "SD.INSTALL_DATE, SD.ADDRESS, SD.CONTACT_NAME, SD.CONTACT_TELEPHONE, SD.COMMENTS, SD.STATUS_ID, SD.SUB_SEGMENT_ID, SD.CITY " +
                    "FROM SOLICITUD_DISPENSADORES AS SD JOIN RAZONES AS R ON SD.REASON_ID = R.ID_REASON " +
                    "JOIN CLIENTES_FINALES AS CF ON SD.END_USER_ID = CF.END_USER_ID AND SD.CLIENT_ID = CF.CLIENT_ID WHERE SD.DR_ID = '{0}'", lblNumeroSol.Text);

                DataTable solcitud = conexion.getGridDataSource(query);

                //Las solicitudes son autoincrementables asi que no creo que se vengan solicitudes con mismo codigo :s
                foreach (DataRow fila in solcitud.Rows)
                {
                    lblDescripcionMotivo.Text = fila["REASON_DESCRIP"].ToString();
                    txtNombreComercial.Text = (!fila["TRADE_NAME"].ToString().Equals("N/A")) ? fila["TRADE_NAME"].ToString() : String.Empty;
                    txtRazonSocial.Text = (!fila["SOCIAL_REASON"].ToString().Equals("N/A")) ? fila["SOCIAL_REASON"].ToString() : String.Empty;
                    txtCedulaJuridica.Text = (!fila["CORPORATE_ID"].ToString().Equals("N/A")) ? fila["CORPORATE_ID"].ToString() : String.Empty;
                    txtTelefono.Text = (!fila["TELEPHONE"].ToString().Equals("N/A")) ? fila["TELEPHONE"].ToString() : String.Empty;
                    txtFechaSol.SelectedDate = DateTime.Parse(fila["INSTALL_DATE"].ToString());
                    txtDireccion.Text = (!fila["ADDRESS"].ToString().Equals("N/A")) ? fila["ADDRESS"].ToString() : String.Empty;
                    txtPersonaContacto.Text = (!fila["CONTACT_NAME"].ToString().Equals("N/A")) ? fila["CONTACT_NAME"].ToString() : String.Empty;
                    txtTelefonoContacto.Text = (!fila["CONTACT_TELEPHONE"].ToString().Equals("N/A")) ? fila["CONTACT_TELEPHONE"].ToString() : String.Empty;
                    lblDescripcionComentarios.Text = (!fila["COMMENTS"].ToString().Equals("N/A")) ? fila["COMMENTS"].ToString() : String.Empty;

                    if (!fila["SUB_SEGMENT_ID"].ToString().Equals("0"))
                        cmbSegmento.FindItemByValue(fila["SUB_SEGMENT_ID"].ToString(), false).Selected = true;
                    else
                    {
                        cmbSegmento.Text = String.Empty;
                        cmbSegmento.ClearSelection();
                    }

                    if(!fila[""].ToString().Equals(""))
                }
            }
            catch (Exception error)
            {
                radajaxmanager.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }

        }
        #endregion

        #endregion

    }
}