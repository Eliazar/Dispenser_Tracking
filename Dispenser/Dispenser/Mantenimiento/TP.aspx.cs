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


namespace Dispenser.Mantenimiento
{
    public partial class TP : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Menu menu = Master.FindControl("NavigationMenu") as Menu;
                menu.Items[2].ChildItems[2].ChildItems.RemoveAt(2);

                if (!IsPostBack)
                {
                    Connection conexion = new Connection();

                    string paisUsuario = conexion.getUserCountry(Session.Contents["userid"].ToString());
                    string query = String.Format("SELECT CLIENT_ID, CLIENT_NAME FROM CLIENTES_KC WHERE COUNTRY = '{0}' AND NEXT_UPDATE <= '{1}' AND DIRECT_CUSTOMER = 0",
                        paisUsuario, DateTime.Now.ToString("yyyMMdd"));

                    DataTable tabla = conexion.getGridDataSource(query);

                    foreach (DataRow fila in tabla.Rows)
                    {
                        RadComboBoxItem item = new RadComboBoxItem();
                        item.Text = fila["CLIENT_ID"].ToString() + " | " + fila["CLIENT_NAME"].ToString();
                        item.Value = fila["CLIENT_ID"].ToString();

                        item.DataBind();
                        cmbSocioComercial.Items.Add(item);
                    }
                }
            }
            catch (Exception error)
            {
                literal.Text = String.Format("<script languaje='javascript'>" +
                    "alert('Sucedio el siguiente error: {0}');" +
                    "window.location.href = '../Default.aspx';" +
                    "</script>", error.Message);
            }
        }

        protected void btProcesar_Click(object sender, EventArgs e)
        {
            if (cmbSocioComercial.SelectedValue.Equals(String.Empty))
            {
                mensajes.Text = String.Format("<script languaje='javascript'>" +
                                                    "alert('Seleccion invalida, elija un socio comercial de la lista.');" +
                                              "</script>");
                return;
            }

            Connection conexion = new Connection();

            //Consigo la fecha de ultima actualizacion y la fecha de hoy
            DateTime fechaActualizacion = DateTime.Parse(conexion.getClientKCInfo("NEXT_UPDATE", "CLIENT_ID", cmbSocioComercial.SelectedValue));
            DateTime hoy = DateTime.Now;

            double inversion = Convert.ToDouble(nmcPresupuesto.Value);

            //Cuento cuantas solicitudes hay desde la ultima vez que se actualizo el TP
            string query = String.Format("SELECT COUNT(DR_ID) AS CANTIDAD FROM SOLICITUD_DISPENSADORES WHERE (CLIENT_ID = '{0}' AND (STATUS_ID = 1 OR STATUS_ID = 5)) AND (DATE_REQUEST BETWEEN '{1}' AND '{2}')",
                cmbSocioComercial.SelectedValue, fechaActualizacion.ToString("yyyMMdd"), hoy.ToString("yyyMMdd"));
            DataTable solicitudesMes = conexion.getGridDataSource(query);

            if (Convert.ToInt32(solicitudesMes.Rows[0]["CANTIDAD"].ToString()) > 0)
            {
                mensajes.Text = String.Format("<script languaje='javascript'>" +
                                                    "alert('Existen {0} solicitudes pendientes de el ultimo presupuesto. No se puede continuar');" +
                                              "</script>", solicitudesMes.Rows[0]["CANTIDAD"].ToString());
                return;
            }

            hoy = hoy.AddMonths(1);
            query = String.Format("UPDATE CLIENTES_KC SET BUDGET_TP = {0}, NEXT_UPDATE = '{1}', INVERSION = 0 WHERE CLIENT_ID = '{2}'",
                inversion, new DateTime(hoy.Year, hoy.Month, 1).ToString("yyyMMdd"), cmbSocioComercial.SelectedValue);

            if (conexion.Actualizar(query))
            {

                //Se buscan todas las solicitudes corridas.
                query = String.Format("SELECT DR_ID, INVER_SOLICITADA FROM SOLICITUD_DISPENSADORES WHERE NEXT_MONTH = 1 AND IS_EDITABLE = 0 AND CLIENT_ID = '{0}'",
                cmbSocioComercial.SelectedValue);
                DataTable solicitudesCorridas = conexion.getGridDataSource(query);

                double acumulador = 0;
                int pendientes = 0;
                int transferidas = 0;

                //Se recorren todas las filas del resultado de la busqueda y compara si el precio cabe dentro del nuevo presupuesto caso contrario
                //se vuelve a correr al siguiente mes.
                foreach (DataRow fila in solicitudesCorridas.Rows)
                {
                    acumulador += Convert.ToDouble(fila["INVER_SOLICITADA"].ToString());

                    if (acumulador <= inversion)
                    {
                        query = String.Format("UPDATE SOLICITUD_DISPENSADORES SET IS_EDITABLE = 1 WHERE DR_ID = '{0}'", fila["DR_ID"].ToString());
                        conexion.Actualizar(query);
                        transferidas++;
                    }
                    else
                    {
                        query = String.Format("UPDATE SOLICITUD_DISPENSADORES SET INSTALL_DATE = '{0}' WHERE DR_ID = '{1}'"
                            , new DateTime(hoy.Year, hoy.Month, 1).ToString("yyyMMdd"), fila["DR_ID"].ToString());
                        conexion.Actualizar(query);
                        pendientes++;
                    }
                }


                query = String.Format("SELECT USER_NAME, E_MAIL FROM USUARIOS WHERE CLIENT_ID = '{0}'", cmbSocioComercial.SelectedValue);
                DataTable correos = conexion.getGridDataSource(query);
                
                if (conexion.enviarActualizacionTP(correos, cmbSocioComercial.SelectedValue))
                    mensajes.Text = String.Format("<script languaje='javascript'>" +
                                                    "alert('TP actualizado con exito. Con solicitudes pendientes habilitadas: {0} y corridas: {1}');" +
                                              "</script>", transferidas, pendientes);
                else
                    mensajes.Text = String.Format("<script languaje='javascript'>" +
                                                    "alert('TP actualizado con exito. Con solicitudes pendientes habilitadas: {0} y corridas: {1}." +
                                                    " Pero no se envio la notificacion por problemas con el servidor.');" +
                                              "</script>", transferidas, pendientes);
                
                nmcPresupuesto.Text = String.Empty;
                cmbSocioComercial.SelectedItem.Remove();
                cmbSocioComercial.Text = String.Empty;
            }
            else
            {
                mensajes.Text = String.Format("<script languaje='javascript'>" +
                                                    "alert('Error de conexion, no se pudo actualizar el TP.');" +
                                              "</script>");
            }
        }

    }
}