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
            MenuItem item2 = new MenuItem();
            MenuItem item3 = new MenuItem();

            item3.Text = "Clientes Nuevos";
            item3.NavigateUrl = "Solicitud_Clientes_Nuevos.aspx";
            
            item2.Text = "Clientes Existentes";
            item2.NavigateUrl = "Solicitud_Dispensadores.aspx";

            item.ChildItems.Add(item3);
            item.ChildItems.Add(item2);
            item.Text = "Solicitud Dispensadores";
            item.Selectable = false;
            
            menu.Items[1].ChildItems.Add(item);

            if (Session.Contents["rol"].ToString().Equals("KCPADM") || Session.Contents["rol"].ToString().Equals("KCPCCR"))
                Response.Redirect("../Default.aspx");
        }

        protected void grdConsulta_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            Connection conexion = new Connection();

            string userId = Session["userid"].ToString();
            string clientCodeKC = conexion.getUsersInfo("CLIENT_ID", "USER_ID", userId);

            int mesActual = DateTime.Now.Month;
            int mesHistorico = 0;
            int anioHistorico = 0;

            switch (mesActual)
            {
                case 1:
                    mesHistorico = 11;  //Mes de noviembre del año anterior
                    anioHistorico = DateTime.Now.Year - 1;
                    break;
                case 2:
                    mesHistorico = 12; //Mes de diciembre del año anterior
                    anioHistorico = DateTime.Now.Year - 1;
                    break;
                default:
                    mesHistorico = mesActual - 2; //Se calculan dos meses anterirores 
                    anioHistorico = DateTime.Now.Year;
                    break;
            }

            //Se crea la fecha de inicio
            DateTime fechaHistorico = new DateTime(anioHistorico, mesHistorico, 1);

            string query = String.Format("SELECT SD.DR_ID, SD.DATE_REQUEST, R.REASON_DESCRIP,"
            + " (SELECT CF.TRADE_NAME FROM CLIENTES_FINALES AS CF WHERE CF.END_USER_ID = SD.END_USER_ID AND CF.CLIENT_ID = '{0}') AS TRADE_NAME,"
            + " S.SEGMENT_NAME, ST.STATUS_DESCRIP, SD.PROGRAMMING_DATE, SD.INVER_SOLICITADA, SD.INVER_APRO, SD.NEXT_MONTH"
            + " FROM SOLICITUD_DISPENSADORES AS SD JOIN RAZONES AS R ON SD.REASON_ID = R.ID_REASON"
            + " JOIN SEGMENTOS AS S ON SD.SEGMENT_ID = S.SEGMENT_ID JOIN STATUS_DESCRIP AS ST ON SD.STATUS_ID = ST.STATUS_ID"
            + " WHERE SD.CLIENT_ID = '{0}' AND SD.DATE_REQUEST BETWEEN '{1}' AND '{2}' ORDER BY DR_ID DESC", clientCodeKC,
            fechaHistorico.ToString("yyyMMdd"), DateTime.Now.ToString("yyyMMdd"));

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