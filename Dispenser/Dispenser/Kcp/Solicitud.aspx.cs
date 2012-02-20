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

namespace Dispenser.Kcp
{
    public partial class Solicitud : System.Web.UI.Page
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
            try
            {
                if (!IsPostBack)
                {
                    if (Session.Contents["rol"].ToString().Equals("DSTADM"))
                        Response.Redirect("../Default.aspx");
                    
                    Connection conexion = new Connection();

                    lblSolicitud.Text = "Solicitud No: " + Session.Contents["solicitud"].ToString();
                    lblCodigo.Text = "Codigo Cliente Final: " + conexion.getSolicitudInfo("END_USER_ID", Session.Contents["solicitud"].ToString());

                    campos = new List<string>();
                    valores = new List<string>();

                    camposDetalle = new List<string>();
                    valoresDetalle = new List<string>();

                    codigoDispensador = new List<string>();
                    codigoProducto = new List<string>();

                    cargarData();
                    configurarBotones();

                    if (!Convert.ToBoolean(conexion.getSolicitudInfo("IS_EDITABLE", Session.Contents["solicitud"].ToString())))
                    {
                        btNuevo.Enabled = false;
                        btAprobar.Enabled = false;
                        btRechazar.Enabled = true;
                        btCerrarCita.Enabled = false;
                        btGuardar.Enabled = false;
                        RadAjaxManager1.ResponseScripts.Add(@"alert('Solicitud corrida sin acceso a aprobacion. Accesos a rechazo unicamente.');");
                    }
                }
            }
            catch (Exception error)
            {
                RadAjaxManager1.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }
        }

        #region Funciones
        private void configurarBotones()
        {
            try
            {
                Connection conexion = new Connection();

                if (conexion.getSolicitudInfo("STATUS_ID", Session["solicitud"].ToString()).Equals("1")
                        || conexion.getSolicitudInfo("STATUS_ID", Session["solicitud"].ToString()).Equals("5"))
                {
                    dpFechaInstalacion.MinDate = DateTime.Now;
                    btNuevo.Enabled = true;
                    btAprobar.Enabled = true;
                    btRechazar.Enabled = true;
                    btCerrarCita.Enabled = false;
                }
                else if (conexion.getSolicitudInfo("STATUS_ID", Session["solicitud"].ToString()).Equals("4") ||
                    conexion.getSolicitudInfo("STATUS_ID", Session["solicitud"].ToString()).Equals("3"))
                {
                    dpFechaInstalacion.MinDate = DateTime.Now;
                    btNuevo.Enabled = false;
                    btAprobar.Enabled = false;
                    btRechazar.Enabled = false;
                    btCerrarCita.Enabled = false;
                    btGuardar.Enabled = false;
                }
                else
                {
                    lblFechaInstalacion.Text = "Fecha Instalada:";
                    btNuevo.Enabled = false;
                    btGuardar.Enabled = false;
                    btAprobar.Enabled = false;
                    btRechazar.Enabled = false;
                    btCerrarCita.Enabled = true;
                }

                dpFechaInstalacion.Clear();
            }
            catch (Exception error)
            {
                RadAjaxManager1.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }
        }

        private void cargarData()
        {
            try
            {
                Connection conexion = new Connection();

                lblDescripcionEstado.Text = Session.Contents["estado"].ToString();

                string query = String.Format("SELECT R.REASON_DESCRIP, CF.TRADE_NAME, CF.SOCIAL_REASON, CF.CORPORATE_ID, CF.TELEPHONE " +
                    "FROM SOLICITUD_DISPENSADORES AS SD JOIN RAZONES AS R ON SD.REASON_ID = R.ID_REASON " +
                    "JOIN CLIENTES_FINALES AS CF ON SD.END_USER_ID = CF.END_USER_ID AND SD.CLIENT_ID = CF.CLIENT_ID WHERE SD.DR_ID = '{0}'", Session.Contents["solicitud"].ToString());

                SqlConnection bridge = new SqlConnection(conexion.getConnectionString());

                try
                {
                    bridge.Open();
                    SqlCommand sentencia = new SqlCommand(query, bridge);
                    SqlDataReader reader = sentencia.ExecuteReader();

                    while (reader.Read())
                    {
                        lblDescripcionMotivo.Text = Convert.ToString(reader["REASON_DESCRIP"]);
                        txtNombreComercial.Text = Convert.ToString(reader["TRADE_NAME"]);
                        txtRazonSocial.Text = Convert.ToString(reader["SOCIAL_REASON"]);
                        txtCedulaJuridica.Text = Convert.ToString(reader["CORPORATE_ID"]);
                        txtTelefono.Text = Convert.ToString(reader["TELEPHONE"]);
                    }

                    bridge.Close();
                }
                catch (SqlException)
                {
                    RadAjaxManager1.ResponseScripts.Add(@"alert('Error inesperado de conexion, no se lograron cargar algunos datos refresque la pagina o intentelo mas tarde.');");
                    return;
                }

                string query2 = String.Format("SELECT INSTALL_DATE, ADDRESS, CONTACT_NAME, CONTACT_TELEPHONE, COMMENTS, STATUS_ID FROM SOLICITUD_DISPENSADORES " +
                    "WHERE DR_ID = '{0}'", Session.Contents["solicitud"].ToString());

                DataTable tabla = conexion.getGridDataSource(query2);

                foreach (DataRow fila in tabla.Rows)
                {
                    txtFechaSol.SelectedDate = DateTime.Parse(fila["INSTALL_DATE"].ToString());
                    txtDireccion.Text = fila["ADDRESS"].ToString();
                    txtPersonaContacto.Text = fila["CONTACT_NAME"].ToString();
                    txtTelefonoContacto.Text = fila["CONTACT_TELEPHONE"].ToString();
                    lblDescripcionComentarios.Text = fila["COMMENTS"].ToString();


                }

                cargarSegmentos();
                cargarDepartamentos();
            }
            catch (Exception error)
            {
                RadAjaxManager1.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }

        }

        private void cargarSegmentos()
        {
            try
            {
                Connection conexion = new Connection();
                cmbSegmento.Items.Clear();

                string segmentoActual = conexion.getSolicitudInfo("SEGMENT_ID", Session["solicitud"].ToString());
                string query = "SELECT SEGMENT_ID, SEGMENT_NAME FROM SEGMENTOS WHERE SEGMENT_ID <> '0'";

                try
                {
                    SqlConnection bridge = new SqlConnection(conexion.getConnectionString());
                    SqlDataAdapter adapter = new SqlDataAdapter(query, bridge);
                    DataTable datatable = new DataTable();
                    adapter.Fill(datatable);

                    foreach (DataRow datarow in datatable.Rows)
                    {
                        RadComboBoxItem item = new RadComboBoxItem();
                        item.Text = datarow["SEGMENT_NAME"].ToString();
                        item.Value = datarow["SEGMENT_ID"].ToString();

                        if (item.Value.Equals(segmentoActual))
                        {
                            item.Selected = true;
                            cargarSubSegmentos(item.Value);
                        }

                        cmbSegmento.Items.Add(item);
                        item.DataBind();
                    }
                }
                catch (SqlException)
                {
                    RadAjaxManager1.ResponseScripts.Add(@"alert('Error inesperado de conexion, no se logro cargar la lista de segmentos refresque la pagina o intentelo mas tarde.');");
                    return;
                }
            }
            catch (Exception error)
            {
                RadAjaxManager1.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }
        }

        private void cargarSubSegmentos(string codigoSegmento)
        {
            try
            {
                Connection conexion = new Connection();
                cmbSubSegmento.Items.Clear();

                string subSegmentoActual = conexion.getSolicitudInfo("SUB_SEGMENT_ID", Session["solicitud"].ToString());
                string query = String.Format("SELECT SUB_SEGMENT_ID, SUB_SEGMENT_NAME FROM SUB_SEGMENTOS WHERE SEGMENT_ID = '{0}'", codigoSegmento);

                try
                {
                    SqlConnection bridge = new SqlConnection(conexion.getConnectionString());
                    SqlDataAdapter adapter = new SqlDataAdapter(query, bridge);
                    DataTable datatable = new DataTable();
                    adapter.Fill(datatable);

                    foreach (DataRow dataRow in datatable.Rows)
                    {
                        RadComboBoxItem item = new RadComboBoxItem();
                        item.Text = dataRow["SUB_SEGMENT_NAME"].ToString();
                        item.Value = dataRow["SUB_SEGMENT_ID"].ToString();

                        if (item.Value.Equals(subSegmentoActual))
                            item.Selected = true;

                        cmbSubSegmento.Items.Add(item);
                        item.DataBind();
                    }
                }
                catch (SqlException)
                {
                    RadAjaxManager1.ResponseScripts.Add(@"alert('Error inesperado de conexion, no se logro cargar la lista de sub-segmentos refresque la pagina o intentelo mas tarde.');");
                    return;
                }
            }
            catch (Exception error)
            {
                RadAjaxManager1.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }

        }

        private void cargarDepartamentos()
        {
            try
            {
                Connection conexion = new Connection();
                cmbDepartamento.Items.Clear();

                string departamentoActual = conexion.getSolicitudInfo("STATE", Session["solicitud"].ToString());
                string pais = conexion.getSolicitudInfo("ID_COUNTRY", Session["solicitud"].ToString());
                string query = String.Format("SELECT DISTINCT DIVISION_ID, DIVISION_NAME FROM DIVISION_TERRITORIAL WHERE COUNTRY_ID = '{0}'", pais);

                try
                {
                    SqlConnection bridge = new SqlConnection(conexion.getConnectionString());
                    SqlDataAdapter adapter = new SqlDataAdapter(query, bridge);
                    DataTable datatable = new DataTable();
                    adapter.Fill(datatable);

                    foreach (DataRow datarow in datatable.Rows)
                    {
                        RadComboBoxItem item = new RadComboBoxItem();
                        item.Text = datarow["DIVISION_NAME"].ToString();
                        item.Value = datarow["DIVISION_ID"].ToString();

                        if (item.Text.Equals(departamentoActual))
                        {
                            item.Selected = true;
                            cargarCiudades(datarow["DIVISION_ID"].ToString());
                        }

                        cmbDepartamento.Items.Add(item);
                        item.DataBind();
                    }

                }
                catch (SqlException)
                {
                    RadAjaxManager1.ResponseScripts.Add(@"alert('Error inesperado de conexion, no se logro cargar la lista de Departamentos/estados/distritos.\nrefresque la pagina o intentelo mas tarde.');");
                    return;
                }
            }
            catch (Exception error)
            {
                RadAjaxManager1.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }
        }

        private void cargarCiudades(string divisionId)
        {
            try
            {
                Connection conexion = new Connection();
                cmbCiudad.Items.Clear();

                string ciudadActual = conexion.getSolicitudInfo("CITY", Session["solicitud"].ToString());
                string pais = conexion.getSolicitudInfo("ID_COUNTRY", Session["solicitud"].ToString());
                string query = String.Format("SELECT CITY_NAME FROM DIVISION_TERRITORIAL WHERE COUNTRY_ID = '{0}' AND DIVISION_ID = '{1}'", pais, divisionId);

                try
                {
                    SqlConnection bridge = new SqlConnection(conexion.getConnectionString());
                    SqlDataAdapter adapter = new SqlDataAdapter(query, bridge);
                    DataTable datatable = new DataTable();
                    adapter.Fill(datatable);

                    foreach (DataRow datarow in datatable.Rows)
                    {
                        RadComboBoxItem item = new RadComboBoxItem();
                        item.Text = datarow["CITY_NAME"].ToString();
                        item.Value = datarow["CITY_NAME"].ToString();

                        if (item.Value.Equals(ciudadActual))
                            item.Selected = true;

                        cmbCiudad.Items.Add(item);
                        item.DataBind();
                    }
                }
                catch (SqlException)
                {
                    RadAjaxManager1.ResponseScripts.Add(@"alert('Error inesperado de conexion, no se logro cargar la lista de ciudades.\nRefresque la pagina o intentelo mas tarde.');");
                    return;
                }
            }
            catch (Exception error)
            {
                RadAjaxManager1.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }
        }

        private void mostrarOcultar(bool equivalencia)
        {
            lblDispensador.Visible = equivalencia;
            cmbDispensador.Visible = equivalencia;
            lblProducto.Visible = equivalencia;
            cmbProducto.Visible = equivalencia;
            lblcantDisp.Visible = equivalencia;
            txtCantDis.Visible = equivalencia;
            lblcantProd.Visible = equivalencia;
            txtProducto.Visible = equivalencia;
            btCrear.Visible = equivalencia;
            btCancelar.Visible = equivalencia;
        }

        private void cargarDispensador()
        {
            try
            {
                Connection conexion = new Connection();
                cmbDispensador.Items.Clear();

                string userCountry = conexion.getUserCountry(Session["userid"].ToString());

                string query = String.Format("SELECT D.ID_DISPENSER, D.DESCRIPTION FROM DISPENSADORES AS D JOIN DISPENSADOR_PAIS AS DP ON D.ID_DISPENSER = DP.DISPENSER_ID WHERE " +
                    "DP.DISPENSER_STATUS = 'TRUE' AND DP.COUNTRY_ID = '{0}'", userCountry);

                try
                {
                    SqlConnection bridge = new SqlConnection(conexion.getConnectionString());
                    SqlDataAdapter adapter = new SqlDataAdapter(query, bridge);
                    DataTable datatable = new DataTable();
                    adapter.Fill(datatable);

                    RadComboBoxItem temp = new RadComboBoxItem();
                    temp.Text = String.Empty;
                    temp.Selected = true;
                    temp.Value = "0";

                    cmbDispensador.Items.Add(temp);

                    foreach (DataRow datarow in datatable.Rows)
                    {
                        RadComboBoxItem item = new RadComboBoxItem();
                        item.Text = datarow["ID_DISPENSER"].ToString() + " | " + datarow["DESCRIPTION"].ToString();
                        item.Value = datarow["ID_DISPENSER"].ToString();

                        cmbDispensador.Items.Add(item);
                        item.DataBind();
                    }
                }
                catch (SqlException)
                {
                    RadAjaxManager1.ResponseScripts.Add(@"alert('Error inesperado de conexion, no se logro cargar la lista de dispensadores refresque la pagina o intentelo mas tarde.');");
                    return;
                }
            }
            catch (Exception error)
            {
                RadAjaxManager1.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }
        }

        private void cargarProducto(string dispenserID)
        {
            try
            {
                Connection conexion = new Connection();
                cmbProducto.Items.Clear();

                string userCountry = conexion.getUserCountry(Session["userid"].ToString());

                string query = String.Format("SELECT PRODUCT_ID, PRODUCT_DESCRIP FROM PRODUCTOS WHERE DISPENSER_ID = '{0}' AND COUNTRY_ID = '{1}'", dispenserID, userCountry);

                try
                {
                    SqlConnection bridge = new SqlConnection(conexion.getConnectionString());
                    SqlDataAdapter adapter = new SqlDataAdapter(query, bridge);
                    DataTable datatable = new DataTable();
                    adapter.Fill(datatable);

                    RadComboBoxItem temp = new RadComboBoxItem();
                    temp.Text = String.Empty;
                    temp.Value = "0";

                    cmbProducto.Items.Add(temp);

                    foreach (DataRow datarow in datatable.Rows)
                    {
                        RadComboBoxItem item = new RadComboBoxItem();
                        item.Text = datarow["PRODUCT_ID"].ToString() + " | " + datarow["PRODUCT_DESCRIP"].ToString();
                        item.Value = datarow["PRODUCT_ID"].ToString();

                        cmbProducto.Items.Add(item);
                        item.DataBind();
                    }
                }
                catch (SqlException)
                {
                    RadAjaxManager1.ResponseScripts.Add(@"alert('Error inesperado de conexion, no se logro cargar la lista de productos refresque la pagina" +
                        " o intentelo mas tarde.');");
                    return;
                }
            }
            catch (Exception error)
            {
                RadAjaxManager1.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }
        }

        private void verificarUpdatesGeneral()
        {
            if (campos.Count > 0 && valores.Count > 0)
                if (campos.Count == valores.Count)
                    for (int i = 0; i < campos.Count; i++)
                        if (!valores[i].Equals(String.Empty) || !valores[i].Equals("0"))
                            mandarActualizar(campos[i], valores[i]);
        }

        private void verificarUpdatesDetalle()
        {
            if ((camposDetalle.Count > 0 && valoresDetalle.Count > 0) && (codigoDispensador.Count > 0 && codigoProducto.Count > 0))
                if ((camposDetalle.Count == valoresDetalle.Count) && (codigoDispensador.Count == codigoProducto.Count))
                    for (int i = 0; i < camposDetalle.Count; i++)
                        if (!valoresDetalle[i].Equals(String.Empty))
                            mandarActualizarDetalle(camposDetalle[i], valoresDetalle[i], codigoDispensador[i], codigoProducto[i]);
        }

        private bool mandarActualizar(string campo, string valor)
        {

            try
            {
                Connection conexion = new Connection();

                string userid = Session["userid"].ToString();
                string clienteKC = conexion.getSolicitudInfo("CLIENT_ID", Session["solicitud"].ToString());

                string query = String.Format("UPDATE CLIENTES_FINALES SET {0} = '{1}' WHERE CLIENT_ID = '{2}' AND END_USER_ID = '{3}'"
                    , campo, valor, clienteKC, conexion.getSolicitudInfo("END_USER_ID", Session["solicitud"].ToString()));

                string query2 = String.Format("UPDATE SOLICITUD_DISPENSADORES SET {0} = '{1}' WHERE CLIENT_ID = '{2}' AND END_USER_ID = '{3}'"
                    , campo, valor, clienteKC, conexion.getSolicitudInfo("END_USER_ID", Session["solicitud"].ToString()));

                //en la segunda conexion se usa la misma funcion de actualizar pero con diferente query
                if (conexion.updateClientesFinales(query) && conexion.updateClientesFinales(query2))
                    return true;
            }
            catch (Exception error)
            {
                RadAjaxManager1.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }
            
            return false;
        }

        private bool mandarActualizarDetalle(string campoDetalle, string valorDetalle, string dispensador, string producto)
        {

            try
            {
                Connection conexion = new Connection();

                //Modifica el valor de la solicitud si la cantidad de aprobados cambia
                if (campoDetalle.Equals("APPROVAL_QTY"))
                {
                    string pais = conexion.getUsersInfo("ID_COUNTRY", "USER_ID", Session.Contents["userid"].ToString());
                    string queryTemp = String.Format("SELECT DISPENSER_PRICE FROM DISPENSADOR_PAIS WHERE DISPENSER_ID = '{0}' AND COUNTRY_ID = '{1}'", dispensador, pais);
                    DataTable temporal = conexion.getGridDataSource(queryTemp);

                    double nuevoCosto = Convert.ToDouble(valorDetalle) * Convert.ToDouble(temporal.Rows[0]["DISPENSER_PRICE"].ToString());
                    conexion.updateClientesFinales(String.Format("UPDATE DESCRIPCION_DISPENSADORES SET INVERSION = {0} WHERE DR_ID = {1} AND DISPENSER_ID = '{2}' AND PRODUCT_ID = '{3}'",
                        nuevoCosto, Session.Contents["solicitud"].ToString(), dispensador, producto));
                }

                //Si el estado cambia a rechazado no se toma en cuenta su valor de lo contrario verifica si estaba en rechazado
                //recalcula el valor de esa solicitud
                if (campoDetalle.Equals("STATUS_ID"))
                {
                    string estadoActual = conexion.getDetalleSol("STATUS_ID", dispensador, producto, Session.Contents["solicitud"].ToString());
                    if (valorDetalle.Equals("3"))
                    {
                        conexion.updateClientesFinales(String.Format("UPDATE DESCRIPCION_DISPENSADORES SET INVERSION = 0 WHERE DR_ID = {0} AND DISPENSER_ID = '{1}' AND PRODUCT_ID = '{2}'",
                            Session.Contents["solicitud"].ToString(), dispensador, producto));
                    }
                    else if (estadoActual.Equals("3"))
                    {
                        string pais = conexion.getUsersInfo("ID_COUNTRY", "USER_ID", Session.Contents["userid"].ToString());
                        string queryTemp = String.Format("SELECT DISPENSER_PRICE FROM DISPENSADOR_PAIS WHERE DISPENSER_ID = '{0}' AND COUNTRY_ID = '{1}'", dispensador, pais);
                        double nuevoCosto = 0;

                        DataTable temporal = conexion.getGridDataSource(queryTemp);
                        queryTemp = String.Format("SELECT DISPENSER_QUANTITY, APPROVAL_QTY FROM DESCRIPCION_DISPENSADORES WHERE DR_ID = {0} AND DISPENSER_ID = '{1}' AND PRODUCT_ID = '{2}'",
                            Session.Contents["solicitud"].ToString(), dispensador, producto);
                        DataTable cantidades = conexion.getGridDataSource(queryTemp);

                        if (!cantidades.Rows[0]["APPROVAL_QTY"].ToString().Equals(String.Empty))
                            nuevoCosto = Convert.ToDouble(cantidades.Rows[0]["APPROVAL_QTY"].ToString()) * Convert.ToDouble(temporal.Rows[0]["DISPENSER_PRICE"].ToString());
                        else
                            nuevoCosto = Convert.ToDouble(cantidades.Rows[0]["DISPENSER_QUANTITY"].ToString()) * Convert.ToDouble(temporal.Rows[0]["DISPENSER_PRICE"].ToString());

                        conexion.updateClientesFinales(String.Format("UPDATE DESCRIPCION_DISPENSADORES SET INVERSION = {0} WHERE DR_ID = {1} AND DISPENSER_ID = '{2}' AND PRODUCT_ID = '{3}'",
                        nuevoCosto, Session.Contents["solicitud"].ToString(), dispensador, producto));
                    }
                }

                string query = String.Format("UPDATE DESCRIPCION_DISPENSADORES SET {0} = '{1}' WHERE DR_ID = '{2}' AND DISPENSER_ID = '{3}' AND PRODUCT_ID = '{4}'"
                    , campoDetalle, valorDetalle, Session["solicitud"].ToString(), dispensador, producto);

                //Uso la funcion de updateClientesFinales ya que esta acepta un string con el query :P
                if (conexion.updateClientesFinales(query))
                {
                    grdDescripciones.Rebind();
                    return true;
                }
            }
            catch (Exception error)
            {
                RadAjaxManager1.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }

            return false;
        }

        private bool crearCsv()
        {
            try
            {
                Connection conexion = new Connection();

                string query = String.Format("SELECT * FROM SOLICITUD_DISPENSADORES WHERE DR_ID = '{0}'", Session.Contents["solicitud"].ToString());
                string organizacionVentas = String.Empty;
                string tipoImpuesto = String.Empty;
                string nombrecliente = String.Empty;
                string roothpath = String.Empty;
                string alias = String.Empty;
                string clientID = String.Empty;

                DataTable temp = conexion.getGridDataSource(query);

                int noArchivo = 0;

                foreach (DataRow fila in temp.Rows)
                {
                    clientID = fila["CLIENT_ID"].ToString();
                    organizacionVentas = conexion.getCountryInfo("SALES_ORGANIZATION", "ID_COUNTRY", fila["ID_COUNTRY"].ToString());
                    tipoImpuesto = conexion.getCountryInfo("TAX_RATE", "ID_COUNTRY", fila["ID_COUNTRY"].ToString());
                    nombrecliente = conexion.getClientKCInfo("CLIENT_NAME", "CLIENT_ID", clientID);

                    string queryDispensador = String.Format("SELECT D.ID_DISPENSER, D.DESCRIPTION FROM DISPENSADORES AS D JOIN DISPENSADOR_PAIS AS DP" +
                    " ON D.ID_DISPENSER = DP.DISPENSER_ID  WHERE DP.COUNTRY_ID = '{0}'", fila["ID_COUNTRY"].ToString());
                    string queryProducto = String.Format("SELECT * FROM PRODUCTOS WHERE COUNTRY_ID = '{0}'", fila["ID_COUNTRY"].ToString());
                    string queryDetalle = String.Format("SELECT * FROM DESCRIPCION_DISPENSADORES WHERE DR_ID = '{0}'", Session.Contents["solicitud"].ToString());

                    DataTable dispensadores = conexion.getGridDataSource(queryDispensador);
                    DataTable productos = conexion.getGridDataSource(queryProducto);
                    DataTable descripcionDispensador = conexion.getGridDataSource(queryDetalle);
                    DataTable paraArchivo = new DataTable();

                    paraArchivo.Columns.Add("Codigo_Dispensador");
                    paraArchivo.Columns.Add("Descripcion_Dispensador");
                    paraArchivo.Columns.Add("Cantidad_Dispensador");
                    paraArchivo.Columns.Add("Codigo_Producto");
                    paraArchivo.Columns.Add("Descripcion_Producto");
                    paraArchivo.Columns.Add("Cantidad_Producto");

                    try
                    {
                        alias = conexion.getClientKCInfo("ALIAS", "CLIENT_ID", clientID);
                        noArchivo = conexion.getClientKCFile(fila["CLIENT_ID"].ToString());

                        if (alias.Equals("ERROR") || noArchivo < 0)
                            return false;

                        roothpath = Request.PhysicalApplicationPath;

                        string path = roothpath + @"DATA\";
                        DirectoryInfo dir = new DirectoryInfo(path);

                        if (!dir.Exists)
                            dir.Create();

                        string archivo = path + String.Format(@"SCS-{0}-{1}{2}.csv", fila["ID_COUNTRY"].ToString(), alias, ++noArchivo);
                        pathAbsoluto = archivo;

                        FileStream csv = new FileStream(archivo, FileMode.Create, FileAccess.Write);
                        StreamWriter guardar = new StreamWriter(csv, System.Text.Encoding.Default);

                        for (int i = 0; i < 2; i++)
                        {
                            guardar.Write("H;");
                            guardar.Write(tipoImpuesto + ";");
                            guardar.Write(txtCedulaJuridica.Text + ";");//Mandatorio
                            guardar.Write(txtRazonSocial.Text + ";");//Mandatorio
                            guardar.Write(txtNombreComercial.Text + ";");//Mandatorio
                            guardar.Write(txtDireccion.Text + ";");//Mandatorio
                            guardar.Write("0;");

                            if (fila["POSTAL_CODE"].ToString().Equals(String.Empty))
                                guardar.Write("N/A;");
                            else
                                guardar.Write(fila["POSTAL_CODE"].ToString() + ";");

                            guardar.Write("N/A;");
                            guardar.Write(fila["NEIGHBORHOOD"].ToString() + ";");//Mandatorio
                            guardar.Write(cmbCiudad.Text + ";");//Mandatorio
                            guardar.Write(cmbDepartamento.Text + ";");//Mandatorio
                            guardar.Write(fila["ID_COUNTRY"].ToString() + ";");
                            guardar.Write(txtTelefono.Text + ";");//Mandatorio

                            if (fila["CONTACT_EMAIL"].ToString().Equals(String.Empty))
                                guardar.Write("N/A;");
                            else
                                guardar.Write(fila["CONTACT_EMAIL"].ToString() + ";");

                            string correo = conexion.getEndClientInfo("E_MAIL", "CLIENT_ID", fila["CLIENT_ID"].ToString(), "END_USER_ID", fila["END_USER_ID"].ToString());

                            if (correo.Equals(String.Empty))
                                guardar.Write("N/A;");
                            else
                                guardar.Write(correo + ";");

                            if (i == 0)
                            {
                                guardar.Write(txtPersonaContacto.Text + ";");//Mandatorio
                                guardar.Write(txtTelefonoContacto.Text + ";");//Mandatorio

                                if (fila["CONTACT_EMAIL"].ToString().Equals(String.Empty))
                                    guardar.Write("N/A;");
                                else
                                    guardar.Write(fila["CONTACT_EMAIL"].ToString() + ";");

                                guardar.Write("N/A;");
                                guardar.Write("N/A;");
                                guardar.Write("0004 AGENTE\n");
                            }
                            else
                            {
                                guardar.Write(";;;");
                                guardar.Write(fila["NEIGHBORHOOD"].ToString() + ";");//Mandatorio
                                guardar.Write("N/A;\n");
                            }
                        }

                        guardar.Write("HH;");
                        guardar.Write(fila["PURCHASE_FREQUENCY"].ToString() + ";");
                        guardar.Write("N/A;");

                        //Para el tipo de trafico
                        string tipoTrafico = conexion.getSolicitudInfo("TRAFFIC_TYPE", Session.Contents["solicitud"].ToString());
                        if (tipoTrafico.Equals("Bajo"))
                            guardar.Write("01 " + tipoTrafico + ";");
                        else if (tipoTrafico.Equals("Medio"))
                            guardar.Write("02 " + tipoTrafico + ";");
                        else if (tipoTrafico.Equals("Alto"))
                            guardar.Write("03 " + tipoTrafico + ";");

                        //Para la limpieza tercerizada
                        string limpiezaTercerizada = conexion.getSolicitudInfo("TERTIARY_CLEANING", Session["solicitud"].ToString());
                        if (limpiezaTercerizada.Equals("False"))
                            guardar.Write("No;");
                        else
                            guardar.Write("Si;");

                        //Para el segmento
                        if (cmbSegmento.SelectedValue.Length < 2)
                            guardar.Write("0" + cmbSegmento.SelectedValue + " " + cmbSegmento.Text + ";");//Mandatorio
                        else
                            guardar.Write(cmbSegmento.SelectedValue + " " + cmbSegmento.Text + ";");//Mandatorio

                        //Para el subsegmento
                        if (cmbSubSegmento.Text.Equals(String.Empty))
                            guardar.Write("N/A;");
                        else if (cmbSubSegmento.SelectedValue.Length < 3)
                            guardar.Write("0" + cmbSubSegmento.SelectedValue + " " + cmbSubSegmento.Text + ";");
                        else
                            guardar.Write(cmbSubSegmento.SelectedValue + " " + cmbSubSegmento.Text + ";");

                        guardar.Write("N/A;N/A;");
                        guardar.Write(conexion.getPaymentConditionInfo("PAYMENT_CONDITION", "ID_PAYMENT_CONDITION", fila["ID_PAYMENT_CONDITION"].ToString()) + ";");

                        //Empleados
                        if (fila["EMPLOYEES"].ToString().Equals("0"))
                            guardar.Write("N/A;");
                        else
                            guardar.Write(fila["EMPLOYEES"].ToString() + ";");

                        //Visitantes
                        if (fila["VISITORS"].ToString().Equals("0"))
                            guardar.Write("N/A;");
                        else
                            guardar.Write(fila["VISITORS"].ToString() + ";");

                        //Cantidad Lavatorios
                        if (fila["WASHBASIN"].ToString().Equals("0"))
                            guardar.Write("N/A;");
                        else
                            guardar.Write(fila["WASHBASIN"].ToString() + ";");

                        //Cantidad de baños hombre
                        if (fila["MALE_BATHROOM"].ToString().Equals("0"))
                            guardar.Write("N/A;");
                        else
                            guardar.Write(fila["MALE_BATHROOM"].ToString() + ";");

                        //Cantidad de baños mujer
                        if (fila["FEMALE_BATHROOM"].ToString().Equals("0"))
                            guardar.Write("N/A;");
                        else
                            guardar.Write(fila["FEMALE_BATHROOM"].ToString() + ";");

                        guardar.Write(fila["SALES_ID"].ToString() + ";");
                        guardar.Write(conexion.getSalesInfo("SALES_NAME", "SALES_ID", fila["SALES_ID"].ToString(), "CLIENT_ID", fila["CLIENT_ID"].ToString()) + ";");
                        guardar.Write(fila["PURCHASE_FREQUENCY"].ToString() + ";");
                        guardar.Write("0 - NO;0;;\n");

                        guardar.Write("HI;");
                        guardar.Write(Session.Contents["solicitud"].ToString() + ";");

                        DateTime fechaTemp = DateTime.Parse(fila["DATE_REQUEST"].ToString());
                        string añoTemp = Convert.ToString(fechaTemp.Year);
                        string mesTemp = (fechaTemp.Month >= 1 && fechaTemp.Month <= 9) ? "0" + Convert.ToString(fechaTemp.Month) : Convert.ToString(fechaTemp.Month);
                        string diaTemp = (fechaTemp.Day >= 1 && fechaTemp.Day <= 9) ? "0" + Convert.ToString(fechaTemp.Day) : Convert.ToString(fechaTemp.Day);
                        string fechaTempConcatenada = añoTemp + mesTemp + diaTemp;

                        guardar.Write(fechaTempConcatenada + ";");
                        guardar.Write(fila["CLIENT_ID"].ToString() + ";");
                        guardar.Write(fila["CLIENT_ID"].ToString() + ";");
                        guardar.Write(nombrecliente + ";");
                        guardar.Write(conexion.consolidarCodigo(fila["END_USER_ID"].ToString()) + ";");
                        guardar.Write(txtRazonSocial.Text + ";");

                        string clienteEstrategico = conexion.getSolicitudInfo("STRATEGIC_CUSTOMER", Session.Contents["solicitud"].ToString());
                        if (clienteEstrategico.Equals("False"))
                            guardar.Write("2 - No;");
                        else
                            guardar.Write("1 - Si;");

                        guardar.Write(organizacionVentas + ";");
                        guardar.Write(conexion.getReason("REASON_DESCRIP", "ID_REASON", fila["REASON_ID"].ToString()) + ";");

                        fechaTemp = DateTime.Parse(fila["INSTALL_DATE"].ToString());
                        añoTemp = Convert.ToString(fechaTemp.Year);
                        mesTemp = (fechaTemp.Month >= 1 && fechaTemp.Month <= 9) ? "0" + Convert.ToString(fechaTemp.Month) : Convert.ToString(fechaTemp.Month);
                        diaTemp = (fechaTemp.Day >= 1 && fechaTemp.Day <= 9) ? "0" + Convert.ToString(fechaTemp.Day) : Convert.ToString(fechaTemp.Day);
                        fechaTempConcatenada = añoTemp + mesTemp + diaTemp;

                        guardar.Write(fechaTempConcatenada + ";");
                        guardar.Write(";;;;;;;;;\n");

                        string[] objeto = new string[6];
                        foreach (DataRow fila2 in dispensadores.Rows)
                        {
                            objeto[0] = fila2["ID_DISPENSER"].ToString();
                            objeto[1] = fila2["DESCRIPTION"].ToString();

                            foreach (GridDataItem item in grdDescripciones.MasterTableView.Items)
                            {
                                TableCell celda = item["CodigoDispensador"];
                                DataRow filaDetalle = descripcionDispensador.Rows[item.DataSetIndex];

                                if (celda.Text.Equals(fila2["ID_DISPENSER"].ToString()) &&
                                    filaDetalle["STATUS_ID"].ToString().Equals("2"))
                                {
                                    string sentencia = String.Empty;
                                    TableCell celda2 = item["CodigoProducto"];
                                    TableCell celda3 = item["CantidadProducto"];

                                    objeto[2] = (item.FindControl("txtDisAprobados") as RadNumericTextBox).Text;
                                    objeto[3] = celda2.Text;

                                    sentencia = String.Format("PRODUCT_ID = '{0}' AND DISPENSER_ID = '{1}'", celda2.Text, celda.Text);
                                    DataRow[] filaTemp = productos.Select(sentencia);

                                    objeto[4] = filaTemp[0]["PRODUCT_DESCRIP"].ToString();
                                    objeto[5] = celda3.Text;

                                    break;
                                }
                                else
                                {
                                    objeto[2] = "0";
                                    objeto[3] = "0";
                                    objeto[4] = "0";
                                    objeto[5] = "0";
                                }
                            }

                            paraArchivo.LoadDataRow(objeto, false);
                        }

                        foreach (DataRow imprimir in paraArchivo.Rows)
                        {
                            guardar.Write("I;");
                            guardar.Write(imprimir["Codigo_Dispensador"].ToString() + " - " + imprimir["Descripcion_Dispensador"] + ";");
                            guardar.Write(imprimir["Cantidad_Dispensador"].ToString() + ";");

                            if (imprimir["Codigo_Producto"].ToString().Equals("0"))
                                guardar.Write(imprimir["Codigo_Producto"].ToString() + ";");
                            else
                                guardar.Write(imprimir["Codigo_Producto"].ToString() + " - " + imprimir["Descripcion_Producto"].ToString() + ";");

                            guardar.Write(imprimir["Cantidad_Producto"].ToString() + ";");
                            guardar.Write("0;");
                            guardar.Write("ZAGN;");
                            guardar.Write(";;;;;;;;;;;;;;\n");
                        }

                        guardar.Close();

                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }

                conexion.updateClientKCFile(noArchivo, clientID);
                return true;
            }
            catch (Exception error)
            {
                RadAjaxManager1.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }

            return false;
        }

        //Para el correo electronico
        private DataTable fillDetalle()
        {
            try
            {
                Connection conexion = new Connection();

                DataTable tabla = new DataTable();
                tabla.Columns.Add("Codigo_Dispensador");
                tabla.Columns.Add("Descripcion_Dispensador");
                tabla.Columns.Add("Cantidad_Solicitada");
                tabla.Columns.Add("Cantidad_Aprobada");
                tabla.Columns.Add("Producto");
                tabla.Columns.Add("Estatus");
                tabla.Columns.Add("Comentarios");

                string[] campos = new string[7];

                foreach (GridDataItem item in grdDescripciones.MasterTableView.Items)
                {
                    TableCell celda = item["CodigoDispensador"];
                    campos[0] = celda.Text;//Codigo Dispensador
                    campos[1] = conexion.getDispenserInfo("DESCRIPTION", "ID_DISPENSER", celda.Text);//Descripcion Dispensador

                    celda = item["CantidadDispensador"];
                    campos[2] = celda.Text;// Cantidad solicitada
                    campos[3] = (item.FindControl("txtDisAprobados") as RadNumericTextBox).Text;//Cantidad Aprobada

                    TableCell celda2 = item["CodigoDispensador"];
                    celda = item["CodigoProducto"];
                    campos[4] = conexion.getProductInfo("PRODUCT_DESCRIP", "PRODUCT_ID", celda.Text, "DISPENSER_ID", celda2.Text);//Producto
                    campos[5] = (item.FindControl("cmbEstados") as RadComboBox).Text;//Estatus
                    campos[6] = (item.FindControl("txtComentariosGrid") as RadTextBox).Text;//Comentarios

                    tabla.LoadDataRow(campos, false);

                }

                return tabla;
            }
            catch (Exception error)
            {
                RadAjaxManager1.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }
            
            return new DataTable();

        }

        private bool validarControles()
        {
            try
            {
                if (txtNombreComercial.Text.Equals(String.Empty))
                {
                    RadAjaxManager1.ResponseScripts.Add(@"alert('El nombre comercial es requerido.');");
                    return false;
                }

                if (txtRazonSocial.Text.Equals(String.Empty))
                {
                    RadAjaxManager1.ResponseScripts.Add(@"alert('La razon social es requerida.');");
                    return false;
                }

                if (txtDireccion.Text.Equals(String.Empty))
                {
                    RadAjaxManager1.ResponseScripts.Add(@"alert('La direccion es requerida.');");
                    return false;
                }

                if (txtCedulaJuridica.Text.Equals(String.Empty))
                {
                    RadAjaxManager1.ResponseScripts.Add(@"alert('La cedula juridica es requerida.');");
                    return false;
                }

                if (txtTelefono.Text.Equals(String.Empty) || txtTelefono.Text.Equals("0"))
                {
                    RadAjaxManager1.ResponseScripts.Add(@"alert('El telefono es requerido.');");
                    return false;
                }

                if (txtPersonaContacto.Text.Equals(String.Empty))
                {
                    RadAjaxManager1.ResponseScripts.Add(@"alert('La persona de contacto es requerida.');");
                    return false;
                }

                if (txtTelefonoContacto.Text.Equals(String.Empty) || txtTelefonoContacto.Text.Equals("0"))
                {
                    RadAjaxManager1.ResponseScripts.Add(@"alert('El telefono del contacto es requerido.');");
                    return false;
                }

                foreach (GridDataItem fila in grdDescripciones.MasterTableView.Items)
                {
                    if ((fila.FindControl("txtDisAprobados") as RadNumericTextBox).Text.Equals(String.Empty))
                    {
                        RadAjaxManager1.ResponseScripts.Add(@"alert('La cantidad a aprobar no debe ser vacia.');");
                        return false;
                    }
                }

                return true;
            }
            catch (Exception error)
            {
                RadAjaxManager1.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }

            return false;
        }

        private void recalcularInversion()
        {
            try
            {
                Connection conexion = new Connection();

                double inversionDetalle = 0;
                DataTable detalle = conexion.getGridDataSource(String.Format("SELECT INVERSION FROM DESCRIPCION_DISPENSADORES WHERE DR_ID = {0}",
                    Session.Contents["solicitud"].ToString()));

                foreach (DataRow fila in detalle.Rows)
                    inversionDetalle += Convert.ToDouble(fila["INVERSION"].ToString());

                string idClienteKC = conexion.getSolicitudInfo("CLIENT_ID", Session["solicitud"].ToString());

                string queryTemp = String.Format("UPDATE SOLICITUD_DISPENSADORES SET INVER_APRO = {0} WHERE DR_ID = {1}",
                    inversionDetalle, Session.Contents["solicitud"].ToString());
                conexion.Actualizar(queryTemp);
            }
            catch (Exception error)
            {
                RadAjaxManager1.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }

        }
        #endregion

        #region Carga de controles del grid
        protected DataTable comboInGrid(string dispensador, string producto)
        {
            try
            {
                Connection conexion = new Connection();

                int estadoActual = Convert.ToInt32(conexion.getDetalleSol("STATUS_ID", dispensador, producto, Session["solicitud"].ToString()));

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
                RadAjaxManager1.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }

            return new DataTable();
        }

        protected double cantidad(string dispensador, string producto)
        {
            try
            {
                Connection conexion = new Connection();

                string cantidad = conexion.getDetalleSol("APPROVAL_QTY", dispensador, producto, Session["solicitud"].ToString());
                double cantidadAutorizada = 0;
                double cantidadDispensador = Convert.ToDouble(conexion.getDetalleSol("DISPENSER_QUANTITY", dispensador, producto, Session["solicitud"].ToString()));

                if (!cantidad.Equals(String.Empty))
                    cantidadAutorizada = Convert.ToDouble(cantidad);

                if (cantidadAutorizada > 0)
                    return cantidadAutorizada;
                else
                {
                    string query = String.Format("UPDATE DESCRIPCION_DISPENSADORES SET APPROVAL_QTY = {0} WHERE DR_ID = '{1}' AND DISPENSER_ID = '{2}' AND PRODUCT_ID = '{3}'",
                        cantidadDispensador, Session.Contents["solicitud"].ToString(), dispensador, producto);

                    conexion.Actualizar(query);
                    return cantidadDispensador;
                }
            }
            catch (Exception error)
            {
                RadAjaxManager1.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }

            return 0;
        }

        //Aqui trabajo en que la data del grid salga bien

        protected string comentarios(string dispensador, string producto)
        {
            try
            {
                Connection conexion = new Connection();

                return conexion.getDetalleSol("ADMIN_COMMENT", dispensador, producto, Session["solicitud"].ToString());
            }
            catch (Exception error)
            {
                RadAjaxManager1.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }

            return String.Empty;
        }
        #endregion

        #region Eventos
        protected void grdDescripciones_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Connection conexion = new Connection();

                /*Arreglar esto*/

                string query = String.Format("SELECT DD.PRODUCT_ID, DD.PRODUCT_QUANTITY, DD.DISPENSER_ID, D.DESCRIPTION, DD.DISPENSER_QUANTITY, DD.INVERSION, " +
                    "(SELECT DP.DISPENSER_PRICE FROM DISPENSADOR_PAIS AS DP WHERE DP.DISPENSER_ID = DD.DISPENSER_ID AND DP.COUNTRY_ID = '{0}') AS UNIDAD "
                + "FROM DESCRIPCION_DISPENSADORES AS DD JOIN DISPENSADORES AS D ON D.ID_DISPENSER = DD.DISPENSER_ID WHERE DD.DR_ID = {1}",
                conexion.getUsersInfo("ID_COUNTRY", "USER_ID", Session.Contents["userid"].ToString()), Session["solicitud"].ToString());
                grdDescripciones.DataSource = conexion.getGridDataSource(query);
            }
            catch (Exception error)
            {
                RadAjaxManager1.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }
        }

        protected void cmbSegmento_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                cmbSubSegmento.Items.Clear();

                campos.Add("SEGMENT_ID");
                valores.Add(cmbSegmento.SelectedValue);

                cargarSubSegmentos(cmbSegmento.SelectedValue);
            }
            catch (Exception error)
            {
                RadAjaxManager1.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }
        }

        protected void cmbDepartamento_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                cmbCiudad.Items.Clear();

                campos.Add("STATE");
                valores.Add(cmbDepartamento.Text);

                cargarCiudades(cmbDepartamento.SelectedValue);
            }
            catch (Exception error)
            {
                RadAjaxManager1.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }
        }

        protected void btNuevo_Click(object sender, EventArgs e)
        {
            try
            {
                btNuevo.Visible = false;
                btAprobar.Visible = false;
                btRechazar.Visible = false;
                btGuardar.Visible = false;
                btCerrarCita.Visible = false;

                pnlDescripcionSol.Visible = false;

                mostrarOcultar(true);
                cargarDispensador();
            }
            catch (Exception error)
            {
                RadAjaxManager1.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }
        }

        protected void cmbDispensador_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            cargarProducto(cmbDispensador.SelectedValue);
        }

        protected void btCancelar_Click(object sender, EventArgs e)
        {
            try
            {
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
            }
            catch (Exception error)
            {
                RadAjaxManager1.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }
        }

        protected void btCrear_Click(object sender, EventArgs e)
        {

            try
            {
                #region Validacion
                if (cmbDispensador.SelectedValue.Equals("0") || cmbProducto.SelectedValue.Equals("0"))
                {
                    RadAjaxManager1.ResponseScripts.Add(@"alert('Codigo de dispensador o de producto esta en vacio o no existe.');");
                    return;
                }

                if ((txtCantDis.Text.Equals("0") || txtCantDis.Text.Equals(String.Empty)) || (txtProducto.Text.Equals("0") || txtProducto.Text.Equals(String.Empty)))
                {
                    RadAjaxManager1.ResponseScripts.Add(@"alert('La cantidad de dispensador o de producto esta vacio.');");
                    return;
                }
                #endregion

                foreach (GridDataItem dataItem in grdDescripciones.MasterTableView.Items)
                {
                    TableCell celdaProducto = dataItem["CodigoProducto"];
                    TableCell celdaDispensador = dataItem["CodigoDispensador"];
                    string tempProducto = celdaProducto.Text;
                    string tempDispensador = celdaDispensador.Text;

                    if (cmbDispensador.SelectedValue.Equals(tempDispensador) && cmbProducto.SelectedValue.Equals(tempProducto))
                    {
                        RadAjaxManager1.ResponseScripts.Add(@"alert('Ya existe un detalle en la solicitud con estos requisitos.');");
                        return;
                    }
                }

                Connection conexion = new Connection();
                DataTable tabla = conexion.getGridDataSource(String.Format("SELECT DISPENSER_PRICE FROM DISPENSADOR_PAIS WHERE DISPENSER_ID = '{0}' AND COUNTRY_ID = '{1}'",
                    cmbDispensador.SelectedValue, conexion.getUsersInfo("ID_COUNTRY", "USER_ID", Session.Contents["userid"].ToString())));

                double precio = Convert.ToDouble(tabla.Rows[0]["DISPENSER_PRICE"].ToString());
                double inversion = precio * Convert.ToInt32(txtCantDis.Text);

                string query = String.Format("INSERT INTO DESCRIPCION_DISPENSADORES (DR_ID, DISPENSER_ID, PRODUCT_ID, DISPENSER_QUANTITY, PRODUCT_QUANTITY, STATUS_ID, INVERSION) VALUES " +
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

                grdDescripciones.Rebind();
            }
            catch (Exception error)
            {
                RadAjaxManager1.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }
        }

        protected void btGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                string query = String.Empty;
                string query2 = String.Empty;
                string estadoActual = String.Empty;
                Connection conexion = new Connection();

                if (campos.Count == 0 && camposDetalle.Count == 0)
                {
                    campos.Clear();
                    camposDetalle.Clear();
                    valores.Clear();
                    valoresDetalle.Clear();
                    codigoDispensador.Clear();
                    codigoProducto.Clear();

                    RadAjaxManager1.ResponseScripts.Add(@"alert('No hay datos a modificar.');");
                    return;
                }

                //Estas dos funciones actualizan la informacion general tanto como la detallada
                verificarUpdatesGeneral();
                verificarUpdatesDetalle();

                campos.Clear();
                camposDetalle.Clear();
                valores.Clear();
                valoresDetalle.Clear();
                codigoDispensador.Clear();
                codigoProducto.Clear();

                //Recalcula la inversion flotante si se modifica la cantidad autorizada o el estado del detalle
                recalcularInversion();

                estadoActual = conexion.getSolicitudInfo("STATUS_ID", Session.Contents["solicitud"].ToString());
                query = String.Format("UPDATE SOLICITUD_DISPENSADORES SET STATUS_ID = 5 WHERE DR_ID = '{0}'", Session.Contents["solicitud"].ToString());

                if (estadoActual.Equals("1"))
                {
                    if (conexion.Actualizar(query))
                    {
                        lblDescripcionEstado.Text = conexion.getStatusDescripInfo("STATUS_DESCRIP", "STATUS_ID", "5");
                        RadAjaxManager1.ResponseScripts.Add(@"alert('Cambios guardados con exito.');");
                    }
                    else
                        RadAjaxManager1.ResponseScripts.Add(@"alert('Error de conexion intentolo mas tarde o contactese con el adminsitador.');");
                }
                else
                    RadAjaxManager1.ResponseScripts.Add(@"alert('Cambios guardados con exito.');");

                grdDescripciones.Rebind();
            }
            catch (Exception error)
            {
                RadAjaxManager1.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }

        }

        protected void btAtras_Click(object sender, EventArgs e)
        {
            Response.Redirect("AutorizacionSolicitudes.aspx");
        }

        protected void btAprobar_Click(object sender, EventArgs e)
        {
            try
            {
                Connection conexion = new Connection();

                if (!validarControles())
                    return;

                if (dpFechaInstalacion.IsEmpty)
                {
                    RadAjaxManager1.ResponseScripts.Add(@"alert('La fecha de instalacion esta vacia.');");
                    return;
                }

                //Verifica si hubieron cambios no guardados los guarda de aqui
                if (campos.Count > 0)
                {
                    verificarUpdatesGeneral();
                    campos.Clear();
                    valores.Clear();
                }

                //Verifica que si hubieron cambios no guardados dentro del detalle(Grid) los guarda aqui
                if (camposDetalle.Count > 0)
                {
                    verificarUpdatesDetalle();
                    camposDetalle.Clear();
                    valoresDetalle.Clear();
                    recalcularInversion();
                }
                else
                    recalcularInversion();

                codigoDispensador.Clear();
                codigoProducto.Clear();

                string query = String.Format("SELECT DISPENSER_ID, PRODUCT_ID, STATUS_ID FROM DESCRIPCION_DISPENSADORES WHERE DR_ID = '{0}'", Session.Contents["solicitud"].ToString());
                DataTable temp = conexion.getGridDataSource(query);
                bool aprobados = false;//variable usada para saber si hay solicitudes pendientes o aprobadas en el detalle

                foreach (DataRow fila in temp.Rows)
                {
                    if (fila["STATUS_ID"].ToString().Equals("1"))
                    {
                        conexion.Actualizar(String.Format("UPDATE DESCRIPCION_DISPENSADORES SET STATUS_ID = 2 WHERE DR_ID = '{0}' AND DISPENSER_ID = '{1}' AND PRODUCT_ID = '{2}'",
                            Session.Contents["solicitud"].ToString(), fila["DISPENSER_ID"].ToString(), fila["PRODUCT_ID"].ToString()));
                        aprobados = true;
                    }
                    else if (fila["STATUS_ID"].ToString().Equals("2"))
                        aprobados = true;
                }

                if (!aprobados)
                {
                    RadAjaxManager1.ResponseScripts.Add(@"alert('No se puede aprobar una solicitud con todos los detalles rechazados.');");
                    return;
                }

                DateTime hoy = DateTime.Now;
                DateTime fechaInstalacion = DateTime.Parse(Convert.ToString(dpFechaInstalacion.SelectedDate));

                #region Movimientos de inversion e inversion flotante
                //double inversionFlotante = Convert.ToDouble(conexion.getClientKCInfo("INVERSION_FLOTANTE", "CLIENT_ID", conexion.getSolicitudInfo("CLIENT_ID", Session.Contents["solicitud"].ToString())));
                //double inversion = Convert.ToDouble(conexion.getClientKCInfo("INVERSION", "CLIENT_ID", conexion.getSolicitudInfo("CLIENT_ID", Session.Contents["solicitud"].ToString())));

                double inversionSolicitud = Convert.ToDouble(conexion.getSolicitudInfo("INVER_SOLICITADA", Session.Contents["solicitud"].ToString()));
                double inversionAproSol = Convert.ToDouble(conexion.getSolicitudInfo("INVER_APRO", Session.Contents["solicitud"].ToString()));

                //inversionFlotante -= inversionSolicitud;
                //inversion += inversionAproSol;

                //conexion.Actualizar(String.Format("UPDATE CLIENTES_KC SET INVERSION_FLOTANTE = (INVERSION_FLOTANTE - {0}), INVERSION = (INVERSION + {1}) WHERE CLIENT_ID = '{2}'",
                    //inversionFlotante, inversion, conexion.getSolicitudInfo("CLIENT_ID", Session.Contents["solicitud"].ToString())));

                conexion.Actualizar(String.Format("UPDATE CLIENTES_KC SET INVERSION_FLOTANTE = (INVERSION_FLOTANTE - {0}), INVERSION = (INVERSION + {1}) WHERE CLIENT_ID = '{2}'",
                inversionSolicitud, inversionAproSol, conexion.getSolicitudInfo("CLIENT_ID", Session.Contents["solicitud"].ToString())));
                #endregion

                query = String.Format("UPDATE SOLICITUD_DISPENSADORES SET STATUS_ID = 2, PROGRAMMING_DATE = '{0}', APPROVAL_DATE = '{1}'" +
                    " WHERE DR_ID = {2}", fechaInstalacion.ToString("yyyMMdd"), hoy.ToString("yyyMMdd"), Session.Contents["solicitud"].ToString());

                DataTable solicitante = conexion.getGridDataSource(String.Format("SELECT USER_NAME, E_MAIL FROM USUARIOS WHERE CLIENT_ID = '{0}'", conexion.getSolicitudInfo("CLIENT_ID", Session.Contents["solicitud"].ToString())));
                DataTable detalles = fillDetalle();
                DataTable customerCareInfo = conexion.getCustomerCareInfo(conexion.getUsersInfo("ID_COUNTRY", "USER_ID", Session.Contents["userid"].ToString()));

                if (conexion.Actualizar(query))
                {
                    if (crearCsv())
                    {

                        if (conexion.enviarMailAdmin(customerCareInfo, pathAbsoluto, Session.Contents["solicitud"].ToString(), txtNombreComercial.Text) &&
                            conexion.enviarEmailDistribuidor(solicitante, Session.Contents["solicitud"].ToString(), txtNombreComercial.Text, detalles))
                        {
                            configurarBotones();
                            lblDescripcionEstado.Text = conexion.getStatusDescripInfo("STATUS_DESCRIP", "STATUS_ID", "2");
                            RadAjaxManager1.ResponseScripts.Add(@"aprobadoRechazado('Aprobacion hecha con exito.');");
                        }
                        else
                        {
                            configurarBotones();
                            lblDescripcionEstado.Text = conexion.getStatusDescripInfo("STATUS_DESCRIP", "STATUS_ID", "2");
                            RadAjaxManager1.ResponseScripts.Add(@"aprobadoRechazado('Aprobacion hecha con exito; error de conexion no se pudo enviar el correo.');");
                        }
                    }
                    else
                    {
                        configurarBotones();
                        lblDescripcionEstado.Text = conexion.getStatusDescripInfo("STATUS_DESCRIP", "STATUS_ID", "2");
                        RadAjaxManager1.ResponseScripts.Add(@"aprobadoRechazado('Aprobacion hecha con exito; sin archivo informe sobre este error.');");
                    }
                }
                else
                {
                    RadAjaxManager1.ResponseScripts.Add(@"aprobadoRechazado('La aprobacion no se realizo, contacte a su administrador.');");
                }

                grdDescripciones.Rebind();
            }
            catch (Exception error)
            {
                RadAjaxManager1.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }
        }

        protected void btRechazar_Click(object sender, EventArgs e)
        {
            try
            {
                Connection conexion = new Connection();

                foreach (GridDataItem fila in grdDescripciones.MasterTableView.Items)
                {
                    if ((fila.FindControl("txtComentariosGrid") as RadTextBox).Text.Equals(String.Empty))
                    {
                        RadAjaxManager1.ResponseScripts.Add(@"alert('Una o varias descripciones no tienen comentarios.');");
                        return;
                    }
                }

                #region Inversion

                double inversionFlotante = Convert.ToDouble(conexion.getClientKCInfo("INVERSION_FLOTANTE", "CLIENT_ID", conexion.getSolicitudInfo("CLIENT_ID", Session.Contents["solicitud"].ToString())));
                double inversionSolicitada = Convert.ToDouble(conexion.getSolicitudInfo("INVER_SOLICITADA", Session.Contents["solicitud"].ToString()));

                inversionFlotante -= inversionSolicitada;
                conexion.Actualizar(String.Format("UPDATE CLIENTES_KC SET INVERSION_FLOTANTE = {0} WHERE CLIENT_ID = '{1}'",
                    inversionFlotante, conexion.getSolicitudInfo("CLIENT_ID", Session.Contents["solicitud"].ToString())));
                #endregion

                DateTime hoy = DateTime.Now;

                string querySolicitud = String.Format("UPDATE SOLICITUD_DISPENSADORES SET STATUS_ID = 3, APPROVAL_DATE = '{0}', PROGRAMMING_DATE = '{1}', INVER_APRO = 0 WHERE DR_ID = '{2}'",
                    hoy.ToString("yyyMMdd"), hoy.ToString("yyyMMdd"), Session.Contents["solicitud"]);
                string queryDetalle = String.Format("UPDATE DESCRIPCION_DISPENSADORES SET STATUS_ID = 3, INVERSION = 0 WHERE DR_ID = '{0}'", Session.Contents["solicitud"].ToString());

                DataTable solicitante = conexion.getGridDataSource(String.Format("SELECT USER_NAME, E_MAIL FROM USUARIOS WHERE CLIENT_ID = '{0}'", conexion.getSolicitudInfo("CLIENT_ID", Session.Contents["solicitud"].ToString())));
                DataTable detalles = fillDetalle();

                if (conexion.Actualizar(queryDetalle) && conexion.Actualizar(querySolicitud))
                {
                    if (conexion.enviarEmailDistribuidor(solicitante, Session.Contents["solicitud"].ToString(), txtNombreComercial.Text, detalles))
                    {
                        lblDescripcionEstado.Text = conexion.getStatusDescripInfo("STATUS_DESCRIP", "STATUS_ID", "3");
                        configurarBotones();
                        RadAjaxManager1.ResponseScripts.Add(@"aprobadoRechazado('Rechazo hecho con exito.');");
                    }
                    else
                    {
                        lblDescripcionEstado.Text = conexion.getStatusDescripInfo("STATUS_DESCRIP", "STATUS_ID", "3");
                        configurarBotones();
                        RadAjaxManager1.ResponseScripts.Add(@"aprobadoRechazado('Solicitud Rechazada; error de conexion no se envio correo al distribuidor.');");
                    }
                }
                else
                {
                    RadAjaxManager1.ResponseScripts.Add(@"aprobadoRechazado('Error de conexion al momento del rechazo, contactese con su administrador.');");
                    return;
                }

                grdDescripciones.Rebind();
            }
            catch (Exception error)
            {
                RadAjaxManager1.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }
        }

        protected void btCerrarCita_Click(object sender, EventArgs e)
        {
            try
            {
                Connection conexion = new Connection();

                if (dpFechaInstalacion.IsEmpty)
                {
                    RadAjaxManager1.ResponseScripts.Add("@alert('La fecha de cierre esta vacia');");
                    return;
                }

                DateTime cierre = DateTime.Parse(Convert.ToString(dpFechaInstalacion.SelectedDate));
                string fechaCierre = Convert.ToString(cierre.Year);
                fechaCierre += (cierre.Month >= 1 && cierre.Month <= 9) ? "0" + Convert.ToString(cierre.Month) : Convert.ToString(cierre.Month);
                fechaCierre += (cierre.Day >= 1 && cierre.Day <= 9) ? "0" + Convert.ToString(cierre.Day) : Convert.ToString(cierre.Day);

                DateTime sistema = DateTime.Now;
                string fechaSistema = Convert.ToString(sistema.Year);
                fechaSistema += (sistema.Month >= 1 && sistema.Month <= 9) ? "0" + Convert.ToString(sistema.Month) : Convert.ToString(sistema.Month);
                fechaSistema += (sistema.Day >= 1 && sistema.Day <= 9) ? "0" + Convert.ToString(sistema.Day) : Convert.ToString(sistema.Day);

                string query = String.Format("UPDATE SOLICITUD_DISPENSADORES SET STATUS_ID = 4, INSTALLATION_DATE = '{0}', CLOSING_DATE = '{1}' WHERE DR_ID = '{2}'",
                    fechaSistema, fechaSistema, Session.Contents["solicitud"].ToString());

                if (conexion.Actualizar(query))
                {
                    lblDescripcionEstado.Text = conexion.getStatusDescripInfo("STATUS_DESCRIP", "STATUS_ID", "4");
                    configurarBotones();
                    RadAjaxManager1.ResponseScripts.Add(@"aprobadoRechazado('Solicitud Cerrada.');");
                }
                else
                {
                    RadAjaxManager1.ResponseScripts.Add(@"aprobadoRechazado('Error de conexion al momento de cerrar, contactese con su administrador.');");
                    return;
                }
            }
            catch (Exception error)
            {
                RadAjaxManager1.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }

        }

        protected void RadScriptManager1_AsyncPostBackError(object sender, AsyncPostBackErrorEventArgs e)
        {
            RadScriptManager1.AsyncPostBackErrorMessage = "Error inesperado:\n" + e.Exception.Message;
        }
        #endregion

        #region Cambios

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
                RadAjaxManager1.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
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
                RadAjaxManager1.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
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
                RadAjaxManager1.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
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

        protected void txtDireccion_TextChanged(object sender, EventArgs e)
        {
            campos.Add("ADDRESS");
            valores.Add(txtDireccion.Text);
        }

        protected void txtCedulaJuridica_TextChanged(object sender, EventArgs e)
        {
            campos.Add("CORPORATE_ID");
            valores.Add(txtCedulaJuridica.Text);
        }

        protected void txtTelefono_TextChanged(object sender, EventArgs e)
        {
            campos.Add("TELEPHONE");
            valores.Add(txtTelefono.Text);
        }

        protected void cmbSubSegmento_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            campos.Add("SUB_SEGMENT_ID");
            valores.Add(cmbSubSegmento.SelectedValue);
        }

        protected void cmbCiudad_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            campos.Add("CITY");
            valores.Add(cmbCiudad.SelectedValue);
        }

        protected void txtPersonaContacto_TextChanged(object sender, EventArgs e)
        {
            campos.Add("CONTACT_PERSON");
            valores.Add(txtPersonaContacto.Text);
        }

        protected void txtTelefonoContacto_TextChanged(object sender, EventArgs e)
        {
            campos.Add("CONTACT_TELEPHONE");
            valores.Add(txtTelefonoContacto.Text);
        }
        #endregion
    }
}