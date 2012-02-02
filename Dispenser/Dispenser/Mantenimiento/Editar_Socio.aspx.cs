using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//Agregados
using Telerik.Web.UI;
using System.Data;

namespace Dispenser.Mantenimiento
{
    public partial class Editar_Socio : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Menu menu = Master.FindControl("NavigationMenu") as Menu;
                menu.Items[2].ChildItems[2].ChildItems.RemoveAt(1);

                if (!IsPostBack)
                {
                    if (Session.Contents["rol"].ToString().Equals("DSTADM"))
                        Response.Redirect("../Default.aspx");
                    
                    Connection conexion = new Connection();
                    string query = String.Format("SELECT CLIENT_ID, CLIENT_NAME FROM CLIENTES_KC WHERE COUNTRY = '{0}'",
                        conexion.getUserCountry(Session.Contents["userid"].ToString()));

                    DataTable resultset = conexion.getGridDataSource(query);

                    foreach (DataRow fila in resultset.Rows)
                    {
                        RadComboBoxItem item = new RadComboBoxItem();

                        item.Text = fila["CLIENT_ID"].ToString() + " | " + fila["CLIENT_NAME"].ToString();
                        item.Value = fila["CLIENT_ID"].ToString();

                        item.DataBind();
                        cmbCodigoSAP.Items.Add(item);
                    }
                }
            }
            catch (Exception error)
            {
                RadAjaxManager1.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }
        }

        protected void cmbCodigoSAP_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                if (cmbCodigoSAP.SelectedValue.Equals(String.Empty))
                {
                    RadAjaxManager1.ResponseScripts.Add("alert('Seleccion no valida');");
                    btGuardar.Enabled = false;
                    return;
                }

                Connection conexion = new Connection();

                string query = String.Format("SELECT CLIENT_NAME, ADDRESS, TELEPHONE, ALIAS, DIRECT_CUSTOMER FROM CLIENTES_KC WHERE CLIENT_ID = '{0}'",
                    cmbCodigoSAP.SelectedValue);

                DataTable resultset = conexion.getGridDataSource(query);

                txtNombreSocio.Text = resultset.Rows[0]["CLIENT_NAME"].ToString();
                txtDireccion.Text = resultset.Rows[0]["ADDRESS"].ToString();
                txtTelefono.Text = resultset.Rows[0]["TELEPHONE"].ToString();
                txtAlias.Text = resultset.Rows[0]["ALIAS"].ToString();
                cmbClienteDirecto.FindItemByValue(resultset.Rows[0]["DIRECT_CUSTOMER"].ToString(), true).Selected = true;

                btGuardar.Enabled = true;
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
                Connection conexion = new Connection();

                string query = String.Format("UPDATE CLIENTES_KC SET CLIENT_NAME = '{0}', ADDRESS = '{1}', TELEPHONE = '{2}', ALIAS = '{3}', DIRECT_CUSTOMER = '{4}' " +
                    "WHERE CLIENT_ID = '{5}'", txtNombreSocio.Text, txtDireccion.Text, txtTelefono.Text, txtAlias.Text, cmbClienteDirecto.SelectedValue, cmbCodigoSAP.SelectedValue);

                if (conexion.Actualizar(query))
                    RadAjaxManager1.ResponseScripts.Add("esditarSocio();");
                else
                    RadAjaxManager1.ResponseScripts.Add("alert('Error de conexion, intentelo mas tarde.');");

            }
            catch (Exception error)
            {
                RadAjaxManager1.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }
        }
    }
}