using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//Agregados
using System.Data;

namespace Dispenser.Mantenimiento
{
    public partial class Agregar_Vendedor : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Menu menu = Master.FindControl("NavigationMenu") as Menu;
            menu.Items[2].ChildItems[1].ChildItems.RemoveAt(0);
            menu.Items[2].ChildItems[1].ChildItems[0].NavigateUrl = "Editar_Vendedor.aspx";

            if (Session.Contents["rol"].ToString().Equals("KCPADM") || Session.Contents["rol"].ToString().Equals("KCPCCR"))
                Response.Redirect("../Default.aspx");
        }

        protected void btGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                Connection conexion = new Connection();

                string clientId = conexion.getUsersInfo("CLIENT_ID", "USER_ID", Session.Contents["userid"].ToString());
                string query = String.Format("SELECT SALES_ID FROM VENDEDORES WHERE CLIENT_ID = '{0}'", clientId);

                //Se valida que el codigo de vendedor no este en la base de datos.
                DataTable tabla = conexion.getGridDataSource(query);
                foreach (DataRow fila in tabla.Rows)
                {
                    if (fila["SALES_ID"].ToString().ToUpper().Equals(txtCodigo.Text.ToUpper()))
                    {
                        lblMensajes.Text = String.Format("<script languaje='javascript'>" +
                                        "alert('Codigo de vendedor ya existe.');" +
                                     "</script>");
                        return;
                    }

                }

                query = String.Format("INSERT INTO VENDEDORES (SALES_ID, CLIENT_ID, SALES_NAME) VALUES ('{0}', '{1}', '{2}')",
                    txtCodigo.Text.ToUpper(), clientId, txtNombre.Text);

                if (conexion.Actualizar(query))
                    lblMensajes.Text = String.Format("<script languaje='javascript'>" +
                                        "alert('Vendedor ingresado con exito.');" +
                                        "window.location.href = '../Default.aspx';" +
                                     "</script>");
                else
                    lblMensajes.Text = String.Format("<script languaje='javascript'>" +
                                        "alert('Error de conexion, intente refrescar la pagina.');" +
                                     "</script>");
            }
            catch (Exception error)
            {
                literal.Text = String.Format("<script languaje='javascript'>" +
                    "alert('Sucedio el siguiente error: {0}');" +
                    "window.location.href = '../Default.aspx';" +
                    "</script>", error.Message);
            }
        }
    }
}