using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//Agregados
using System.Data;
using System.Data.Common;
using Telerik.Web.UI;

namespace Dispenser.Mantenimiento
{
    public partial class Editar_Usuario : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Menu menu = Master.FindControl("NavigationMenu") as Menu;
                menu.Items[2].ChildItems[0].ChildItems.RemoveAt(1);

                Connection conexion = new Connection();
                string query = String.Empty;

                SqlDataSource1.ConnectionString = conexion.getConnectionString();

                if (Session.Contents["rol"].ToString().Equals("KCPADM"))
                    query = String.Format("SELECT USER_ID, USER_NAME, STATUS, E_MAIL FROM USUARIOS AS U WHERE USER_ID <> '{0}' AND ID_COUNTRY = '{1}' AND 0 = (SELECT count(Kam.KAM_NAME) from Kam where Kam.KAM_ID = U.USER_ID AND Kam.KAM_ACTIVE = 1)",
                        Session.Contents["userid"].ToString(), conexion.getUserCountry(Session.Contents["userid"].ToString()));
                else if (Session.Contents["rol"].ToString().Equals("KCPCCR"))
                    query = String.Format("SELECT USER_ID, USER_NAME, STATUS, E_MAIL FROM USUARIOS WHERE USER_ID <> '{0}' AND ID_COUNTRY = '{1}' AND (ID_ROL <> 'KCPADM' AND ID_ROL <> 'KCPCCR')",
                        Session.Contents["userid"].ToString(), conexion.getUserCountry(Session.Contents["userid"].ToString()));

                SqlDataSource1.SelectCommand = query;
                SqlDataSource1.ConflictDetection = ConflictOptions.CompareAllValues;
            }
            catch (Exception error)
            {
                RadAjaxManager1.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }
        }

        protected void grdUsuarios_ItemUpdated(object source, GridUpdatedEventArgs e)
        {
            try
            {
                GridEditableItem item = e.Item as GridEditableItem;
                string userId = item.GetDataKeyValue("USER_ID").ToString();

                Connection conexion = new Connection();
                conexion.Actualizar(String.Format("UPDATE USUARIOS SET P_EXPIRATION = '{0}' WHERE USER_ID = '{1}'", DateTime.Now.AddMonths(1).ToString("yyyMMdd"), userId));

                if (e.Exception != null)
                {
                    e.KeepInEditMode = true;
                    e.ExceptionHandled = true;
                    RadAjaxManager1.ResponseScripts.Add(String.Format("errorEdicion('{0}');", e.Exception.Message.ToString()));
                }
                else
                {
                    RadAjaxManager1.ResponseScripts.Add(String.Format("alert('Usuario actualizado.');"));
                }
            }
            catch (Exception error)
            {
                RadAjaxManager1.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }
        }
    }
}