using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//Agregados
using System.Web.Security;

namespace Dispenser.Mantenimiento
{
    public partial class Mantenimiento : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session.Contents["userid"] != null)
            {
                if (Session.Contents["rol"].ToString().Equals("DSTADM"))
                {
                    MenuItem childItem = new MenuItem();
                    MenuItem childItem2 = new MenuItem();
                    MenuItem childItem3 = new MenuItem();

                    childItem2.Text = "Clientes Existentes";
                    childItem2.NavigateUrl = "../Dst/Solicitud_Dispensadores.aspx";

                    childItem3.Text = "Clientes Nuevos";
                    childItem3.NavigateUrl = "../Dst/Solicitud_Clientes_Nuevos.aspx";

                    childItem.ChildItems.Add(childItem3);
                    childItem.ChildItems.Add(childItem2);
                    childItem.Text = "Solicitud Dispensadores";
                    childItem.Selectable = false;
                    NavigationMenu.Items[1].ChildItems.AddAt(0, childItem);
                    NavigationMenu.Items[1].ChildItems[1].NavigateUrl = "../Dst/SeguimientoSolicitudes.aspx";

                    NavigationMenu.Items[2].ChildItems[0].ChildItems.RemoveAt(1);
                    NavigationMenu.Items[2].ChildItems[0].ChildItems.RemoveAt(0);
                    NavigationMenu.Items[2].ChildItems[1].Text = "Vendedores";
                    NavigationMenu.Items[2].ChildItems.RemoveAt(2);
                    NavigationMenu.Items.RemoveAt(3);
                }
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