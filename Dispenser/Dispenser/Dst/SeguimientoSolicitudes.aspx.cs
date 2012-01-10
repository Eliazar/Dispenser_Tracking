using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//Agregados
using Telerik.Web.UI;

namespace Dispenser.Dst
{
    public partial class SeguimientoSolicitudes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Menu menu = Master.FindControl("NavigationMenu") as Menu;
            MenuItem item = new MenuItem();
            item.Text = "Solicitud Dispensadores";
            item.NavigateUrl = "Solicitud_Dispensadores.aspx";
            menu.Items[1].ChildItems.Add(item);
        }

        protected void grdConsulta_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            Connection conexion = new Connection();

            string userId = Session["userid"].ToString();
            string clientCodeKC = conexion.getUsersInfo("CLIENT_ID", "USER_ID", userId);

            string query = String.Format("SELECT SD.DR_ID, SD.DATE_REQUEST, R.REASON_DESCRIP,"
            + " (SELECT CF.TRADE_NAME FROM CLIENTES_FINALES AS CF WHERE CF.END_USER_ID = SD.END_USER_ID AND CF.CLIENT_ID = '{0}') AS TRADE_NAME,"
            + " S.SEGMENT_NAME, ST.STATUS_DESCRIP, SD.PROGRAMMING_DATE, SD.INVER_SOLICITADA, SD.INVER_APRO, SD.NEXT_MONTH"
            + " FROM SOLICITUD_DISPENSADORES AS SD JOIN RAZONES AS R ON SD.REASON_ID = R.ID_REASON"
            + " JOIN SEGMENTOS AS S ON SD.SEGMENT_ID = S.SEGMENT_ID JOIN STATUS_DESCRIP AS ST ON SD.STATUS_ID = ST.STATUS_ID"
            + " WHERE SD.CLIENT_ID = '{0}' ORDER BY DR_ID DESC", clientCodeKC);

            grdConsulta.DataSource = conexion.getGridDataSource(query);
        }

        protected void grdConsulta_DetailTableDataBind(object source, Telerik.Web.UI.GridDetailTableDataBindEventArgs e)
        {
            Connection conexion = new Connection();
            GridDataItem dataitem = (GridDataItem)e.DetailTableView.ParentItem;

            switch (e.DetailTableView.Name)
            {
                case "Solicitud":
                    {
                        string requestId = dataitem.GetDataKeyValue("DR_ID").ToString();
                        string query = String.Format("SELECT DD.DISPENSER_ID, DD.PRODUCT_ID, DD.DISPENSER_QUANTITY, DD.PRODUCT_QUANTITY, DD.APPROVAL_QTY," +
                        " DD.INVERSION, SD.STATUS_DESCRIP, DD.ADMIN_COMMENT FROM DESCRIPCION_DISPENSADORES AS DD" +
                        " JOIN STATUS_DESCRIP AS SD ON DD.STATUS_ID = SD.STATUS_ID WHERE DR_ID = '{0}'", requestId);
                        e.DetailTableView.DataSource = conexion.getGridDataSource(query);
                        break;
                    }

            }
        }
    }
}