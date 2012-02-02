using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//Agregados
using System.Data;
using Telerik.Web.UI;

namespace Dispenser.Mantenimiento
{
    public partial class Editar_Vendedor : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Menu menu = Master.FindControl("NavigationMenu") as Menu;
                menu.Items[2].ChildItems[1].ChildItems.RemoveAt(1);
                menu.Items[2].ChildItems[1].ChildItems[0].NavigateUrl = "Agregar_Vendedor.aspx";

                if (!IsPostBack)
                {
                    if (Session.Contents["rol"].ToString().Equals("KCPADM") || Session.Contents["rol"].ToString().Equals("KCPCCR"))
                        Response.Redirect("../Default.aspx");
                    
                    Connection conexion = new Connection();
                    string clientId = conexion.getUsersInfo("CLIENT_ID", "USER_ID", Session.Contents["userid"].ToString());
                    string query = String.Format("SELECT SALES_ID, SALES_NAME FROM VENDEDORES WHERE CLIENT_ID = '{0}'", clientId);

                    DataTable resultset = conexion.getGridDataSource(query);
                    foreach (DataRow fila in resultset.Rows)
                    {
                        RadComboBoxItem item = new RadComboBoxItem();

                        item.Text = fila["SALES_ID"].ToString() + " | " + fila["SALES_NAME"].ToString();
                        item.Value = fila["SALES_ID"].ToString();

                        item.DataBind();
                        cmbCodigo.Items.Add(item);
                    }
                }
            }
            catch (Exception error)
            {
                RadAjaxManager1.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }
        }

        protected void cmbCodigo_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                if (cmbCodigo.SelectedValue.Equals(String.Empty))
                {
                    RadAjaxManager1.ResponseScripts.Add("alert('Seleccion invalida, seleccione un elemento de la lista.');");
                    btEditar.Enabled = false;
                    return;
                }

                btEditar.Enabled = true;
                Connection conexion = new Connection();
                string clientId = conexion.getUsersInfo("CLIENT_ID", "USER_ID", Session.Contents["userid"].ToString());

                txtNombre.Text = conexion.getSalesInfo("SALES_NAME", "SALES_ID", cmbCodigo.SelectedValue, "CLIENT_ID", clientId);
            }
            catch (Exception error)
            {
                RadAjaxManager1.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }
        }

        protected void btEditar_Click(object sender, EventArgs e)
        {

            try
            {
                Connection conexion = new Connection();
                string clientId = conexion.getUsersInfo("CLIENT_ID", "USER_ID", Session.Contents["userid"].ToString());
                string query = String.Format("UPDATE VENDEDORES SET SALES_NAME = '{0}' WHERE SALES_ID = '{1}' AND CLIENT_ID = '{2}'",
                    txtNombre.Text, cmbCodigo.SelectedValue, clientId);

                if (conexion.Actualizar(query))
                    RadAjaxManager1.ResponseScripts.Add("vendedorEditado();");
                else
                    RadAjaxManager1.ResponseScripts.Add("alert('Error de conexion, refresque la pagina e intentelo de nuevo.');");
            }
            catch (Exception error)
            {
                RadAjaxManager1.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }

        }

    }
}