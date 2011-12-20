using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Dispenser.Mantenimiento
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Menu menu = Master.FindControl("NavigationMenu") as Menu;
            menu.Items[2].ChildItems[0].ChildItems.RemoveAt(2);
            menu.Items[2].ChildItems[1].ChildItems[0].NavigateUrl = "Agregar_Vendedor.aspx";
            menu.Items[2].ChildItems[1].ChildItems[1].NavigateUrl = "Editar_Vendedor.aspx";
        }

        protected void btAplicar_Click(object sender, EventArgs e)
        {
            Connection conexion = new Connection();

            string contraActual = conexion.getUsersInfo("PASSWORD", "USER_ID", Session.Contents["userid"].ToString());

            if (contraActual.Equals(txtContraseñaActual.Text))
            {
                DateTime hoy = DateTime.Now.AddMonths(1);
                string query = String.Format("UPDATE USUARIOS SET PASSWORD = '{0}', P_EXPIRATION = '{1}' WHERE USER_ID = '{2}'",
                    txtNuevaContraseña.Text, hoy.ToString("yyyMMdd"), Session.Contents["userid"].ToString());

                if (conexion.Actualizar(query))
                    mensajes.Text = String.Format("<script languaje='javascript'>" +
                                    "alert('Contraseña actualizada.');" +
                                    "window.location.href = '../Default.aspx';" +
                                 "</script>");
                else
                    mensajes.Text = String.Format("<script languaje='javascript'>" +
                                    "alert('Ocurrio un error de conexion, no se pudo actualizar la contraseña.');" +
                                 "</script>");
            }
            else
            {
                mensajes.Text = String.Format("<script languaje='javascript'>" +
                                    "alert('La contraseña actual no coincide.');" +
                                 "</script>");
            }
        }
    }
}