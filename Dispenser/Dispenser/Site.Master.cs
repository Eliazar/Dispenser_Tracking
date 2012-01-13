using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//Agregados por el programador
using System.Web.Security;

namespace Dispenser
{
    public partial class Site : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session.Contents["userid"] != null)
            {
                if (Session.Contents["rol"].ToString().Equals("DSTADM"))
                {
                    MenuItem childItem = new MenuItem();
                    MenuItem childItem1 = new MenuItem();
                    MenuItem childItem2 = new MenuItem();

                    childItem1.Text = "Clientes Existentes";
                    childItem1.NavigateUrl = "Dst/Solicitud_Dispensadores.aspx";

                    childItem2.Text = "Clientes Nuevos";
                    childItem2.NavigateUrl = "Dst/Solicitud_Dispensadores.aspx";

                    childItem.ChildItems.Add(childItem2);
                    childItem.ChildItems.Add(childItem1);

                    childItem.Text = "Solicitud Dispensadores";
                    childItem.Selectable = false;
                    //childItem.NavigateUrl = "Dst/Solicitud_Dispensadores.aspx";
                    NavigationMenu.Items[1].ChildItems.AddAt(0, childItem);
                    NavigationMenu.Items[1].ChildItems[1].NavigateUrl = "Dst/SeguimientoSolicitudes.aspx";

                    NavigationMenu.Items[2].ChildItems[0].ChildItems.RemoveAt(1);
                    NavigationMenu.Items[2].ChildItems[0].ChildItems.RemoveAt(0);
                    NavigationMenu.Items[2].ChildItems[1].Text = "Vendedores";
                    NavigationMenu.Items[2].ChildItems[1].ChildItems[0].NavigateUrl = "Mantenimiento/Agregar_Vendedor.aspx";
                    NavigationMenu.Items[2].ChildItems[1].ChildItems[1].NavigateUrl = "Mantenimiento/Editar_Vendedor.aspx";
                    NavigationMenu.Items[2].ChildItems.RemoveAt(2);
                    NavigationMenu.Items.RemoveAt(3);
                }

                NavigationMenu.Visible = true;
                HeadLoginView.Visible = true;
            }
            else
            {
                NavigationMenu.Visible = false;
                HeadLoginView.Visible = false;
            }
        }

        protected void HeadLoginStatus_LoggedOut(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();

            Session.Abandon();
            Session.RemoveAll();
        }
    }
}