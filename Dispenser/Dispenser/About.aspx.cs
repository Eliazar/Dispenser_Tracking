using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Dispenser
{
    public partial class About : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            (Master.FindControl("NavigationMenu") as System.Web.UI.WebControls.Menu).Visible = true;
        }
    }
}