using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//Agregados
using System.Web.Security;

namespace Dispenser.Reportes
{
    public partial class Reportes : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();

            Session.Abandon();
            Session.RemoveAll();
        }
    }
}