using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//Agregados
using Telerik.Web.UI;

namespace Dispenser.Kcp
{
    public partial class AutorizacionSolicitudes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string historial = Session.Contents["historial"].ToString();
                historial += "Autorizacion de Solicitudes; ";
                Session.Contents["historial"] = historial;
            }
        }

        #region Todo del grid
        protected void grdAutorizaciones_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            Connection conexion = new Connection();

            string userId = Session["userid"].ToString();
            string Pais = conexion.getUsersInfo("ID_COUNTRY", "USER_ID", userId);

            string query = String.Format("SELECT SD.DR_ID, SD.DATE_REQUEST, R.REASON_DESCRIP, SD.INSTALL_DATE, SD.CLIENT_ID, CKC.CLIENT_NAME," +
                    " (SELECT CF.TRADE_NAME FROM CLIENTES_FINALES AS CF WHERE CF.END_USER_ID = SD.END_USER_ID AND CF.CLIENT_ID = SD.CLIENT_ID) AS TRADE_NAME, " +
                    " S.SEGMENT_NAME, ST.STATUS_DESCRIP, SD.COMMENTS, SD.PROGRAMMING_DATE, SD.NEXT_MONTH, SD.INVER_SOLICITADA, SD.INVER_APRO " +
                    " FROM SOLICITUD_DISPENSADORES AS SD JOIN RAZONES AS R ON SD.REASON_ID = R.ID_REASON" +
                    " JOIN SEGMENTOS AS S ON SD.SEGMENT_ID = S.SEGMENT_ID JOIN STATUS_DESCRIP AS ST ON SD.STATUS_ID = ST.STATUS_ID " +
                    " JOIN CLIENTES_KC AS CKC ON SD.CLIENT_ID = CKC.CLIENT_ID WHERE SD.ID_COUNTRY = '{0}' ORDER BY DR_ID, STATUS_DESCRIP DESC", Pais);


            grdAutorizaciones.DataSource = conexion.getGridDataSource(query);
        }

        protected void grdAutorizaciones_DetailTableDataBind(object source, Telerik.Web.UI.GridDetailTableDataBindEventArgs e)
        {
            Connection conexion = new Connection();
            GridDataItem dataitem = (GridDataItem)e.DetailTableView.ParentItem;

            switch (e.DetailTableView.Name)
            {
                case "Solicitud":
                    {
                        string requestId = dataitem.GetDataKeyValue("DR_ID").ToString();
                        string query = String.Format("SELECT DD.DISPENSER_ID, DD.PRODUCT_ID, DD.DISPENSER_QUANTITY, DD.PRODUCT_QUANTITY, DD.APPROVAL_QTY," +
                        " SD.STATUS_DESCRIP, DD.INVERSION FROM DESCRIPCION_DISPENSADORES AS DD JOIN STATUS_DESCRIP AS SD ON DD.STATUS_ID = SD.STATUS_ID " +
                            " WHERE DR_ID = '{0}'", requestId);

                        e.DetailTableView.DataSource = conexion.getGridDataSource(query);
                        break;
                    }
            }
        }

        protected void btVer_Click(object sender, EventArgs e)
        {
            GridDataItem fila;

            fila = (sender as LinkButton).Parent.Parent as GridDataItem;
            TableCell celda = fila["Codigo"];
            TableCell celda2 = fila["Estado"];
            Session.Add("solicitud", celda.Text);
            Session.Add("estado", celda2.Text);

            Response.Redirect("Solicitud.aspx");
        }
        #endregion
    }
}