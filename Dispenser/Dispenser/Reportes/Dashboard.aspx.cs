using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//Agregados
using System.Data;
using Telerik.Web.UI;
using Telerik.Web.UI.GridExcelBuilder;
using System.Drawing;

namespace Dispenser.Reportes
{
    public partial class Dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region Grid de inversion
        protected void grdInversiones_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                Connection conexion = new Connection();
                string query = String.Format("select ckc.CLIENT_ID, ckc.CLIENT_NAME, ckc.BUDGET_TP, " +
                                                "(select case when SUM(sd.INVER_APRO) > 0 then SUM(sd.INVER_APRO) else 0 end as suma " +
                                                "from SOLICITUD_DISPENSADORES as sd " +
                                                "where (sd.STATUS_ID = 2 or sd.STATUS_ID = 4) and sd.CLIENT_ID = ckc.CLIENT_ID and sd.DATE_REQUEST between '{0}' and '{1}') " +
                                                "as APROBADO, " +

                                                "(select case when SUM(sd.INVER_APRO) > 0 then SUM(sd.INVER_APRO) else 0 end as suma " +
                                                "from SOLICITUD_DISPENSADORES as sd " +
                                                "where (sd.STATUS_ID = 2 or sd.STATUS_ID = 4) and sd.CLIENT_ID = ckc.CLIENT_ID and (sd.DATE_REQUEST between '{0}' and '{1}') and sd.NEXT_MONTH = 1 and sd.IS_EDITABLE = 1) " +
                                                "as CARRIED_APROBADAS, " +

                                                "(select case when SUM(sd.INVER_SOLICITADA) > 0 then SUM(sd.INVER_SOLICITADA) else 0 end as suma " +
                                                "from SOLICITUD_DISPENSADORES as sd " +
                                                "where (sd.STATUS_ID = 1 or sd.STATUS_ID = 5) and sd.CLIENT_ID = ckc.CLIENT_ID and sd.DATE_REQUEST between '{0}' and '{1}') " +
                                                "as PENDIENTE, " +

                                                "(select case when SUM(sd.INVER_SOLICITADA) > 0 then SUM(sd.INVER_SOLICITADA) else 0 end as suma " +
                                                "from SOLICITUD_DISPENSADORES as sd " +
                                                "where (sd.STATUS_ID = 1 or sd.STATUS_ID = 5) and sd.CLIENT_ID = ckc.CLIENT_ID and (sd.DATE_REQUEST between '{0}' and '{1}') and sd.NEXT_MONTH = 1 and sd.IS_EDITABLE = 1) " +
                                                "as CARRIED_PENDIENTE " +

                                                "from CLIENTES_KC as ckc " +
                                                "where ckc.COUNTRY = '{2}' AND ckc.CLIENT_ID <> '40000000'", new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyMMdd"),
                                                DateTime.Now.ToString("yyyMMdd"), conexion.getUsersInfo("ID_COUNTRY", "USER_ID", Session.Contents["userid"].ToString()));


                DataTable resultset = conexion.getGridDataSource(query);
                DataTable tabla = new DataTable();

                tabla.Columns.Add("CLIENT_ID", typeof(string));
                tabla.Columns.Add("CLIENT_NAME", typeof(string));
                tabla.Columns.Add("BUDGET_TP", typeof(double));
                tabla.Columns.Add("APROBADO", typeof(double));
                tabla.Columns.Add("MOVIDAS_APROBADAS", typeof(double));
                tabla.Columns.Add("PENDIENTE", typeof(double));
                tabla.Columns.Add("MOVIDAS_PENDIENTE", typeof(double));
                tabla.Columns.Add("DISPONIBLE", typeof(double));
                tabla.Columns.Add("LIBRE", typeof(double));
                tabla.Columns.Add("USADO", typeof(double));

                string[] campos = new string[10];
                double totalBudget = 0;
                double totalAprobado = 0;
                double totalCarriedApro = 0;
                double totalPendiente = 0;
                double totalCarriedPend = 0;
                double disponibleTotal = 0;
                double porcetajeDispoTot = 0;

                foreach (DataRow fila in resultset.Rows)
                {
                    campos[0] = fila["CLIENT_ID"].ToString();
                    campos[1] = fila["CLIENT_NAME"].ToString();

                    double tp = Convert.ToDouble(fila["BUDGET_TP"].ToString());
                    double aprobado = Convert.ToDouble(fila["APROBADO"].ToString());
                    double carriedAprobadas = Convert.ToDouble(fila["CARRIED_APROBADAS"].ToString());
                    double pendientes = Convert.ToDouble(fila["PENDIENTE"].ToString());
                    double carriedPendientes = Convert.ToDouble(fila["CARRIED_PENDIENTE"].ToString());

                    campos[2] = Math.Round(tp, 2).ToString();
                    campos[3] = Math.Round(aprobado, 2).ToString();
                    campos[4] = Math.Round(carriedAprobadas, 2).ToString();
                    campos[5] = Math.Round(pendientes, 2).ToString();
                    campos[6] = Math.Round(carriedPendientes, 2).ToString();

                    double disponible = tp - (aprobado + carriedAprobadas + pendientes + carriedPendientes);
                    double porcentajeDispo = (tp > 0) ? (disponible / tp) : 0;
                    double porcentajeUsado = (1 - porcentajeDispo);

                    campos[7] = Math.Round(disponible, 2).ToString();
                    campos[8] = porcentajeDispo.ToString();
                    campos[9] = porcentajeUsado.ToString();

                    totalBudget += tp;
                    totalAprobado += aprobado;
                    totalPendiente += pendientes;
                    totalCarriedApro += carriedAprobadas;
                    totalCarriedPend += carriedPendientes;

                    tabla.LoadDataRow(campos, false);
                }

                disponibleTotal = totalBudget - (totalAprobado + totalCarriedApro + totalPendiente + totalCarriedPend);
                porcetajeDispoTot = (totalBudget > 0) ? (disponibleTotal / totalBudget) : 0;

                campos[0] = String.Empty;
                campos[1] = "Total Pais:";
                campos[2] = Math.Round(totalBudget, 2).ToString();
                campos[3] = Math.Round(totalAprobado, 2).ToString();
                campos[4] = Math.Round(totalCarriedApro, 2).ToString();
                campos[5] = Math.Round(totalPendiente, 2).ToString();
                campos[6] = Math.Round(totalCarriedPend, 2).ToString();
                campos[7] = Math.Round(disponibleTotal, 2).ToString();
                campos[8] = porcetajeDispoTot.ToString();
                campos[9] = (1 - porcetajeDispoTot).ToString();

                tabla.LoadDataRow(campos, false);

                grdInversiones.DataSource = tabla;

            }
            catch (Exception error)
            {
                literal.Text = String.Format("<script languaje='javascript'>" +
                    "alert('Sucedio el siguiente error: {0}');" +
                    "window.location.href = '../Default.aspx';" +
                    "</script>", error.Message);
            }
        }

        protected void grdInversiones_ExcelMLExportRowCreated(object source, GridExportExcelMLRowCreatedArgs e)
        {
            try
            {

                if (e.RowType == GridExportExcelMLRowType.DataRow)
                {

                    CellElement cell = e.Row.Cells.GetCellByName("BudgetTP");
                    cell.StyleValue = "Presupuesto";

                    cell = e.Row.Cells.GetCellByName("Aprobado");
                    cell.StyleValue = "Aprobado";

                    cell = e.Row.Cells.GetCellByName("C_Aprobado");
                    cell.StyleValue = "C_Aprobado";

                    cell = e.Row.Cells.GetCellByName("Pendiente");
                    cell.StyleValue = "Pendiente";

                    cell = e.Row.Cells.GetCellByName("C_Pendiente");
                    cell.StyleValue = "C_Pendiente";

                    cell = e.Row.Cells.GetCellByName("Disponible");
                    cell.StyleValue = "Disponible";

                    cell = e.Row.Cells.GetCellByName("PorceDisponible");
                    cell.StyleValue = "PorceDisponible";

                    cell = e.Row.Cells.GetCellByName("Usado");
                    cell.StyleValue = "Usado";

                    e.Worksheet.Name = "Inversion en US$";

                    //Set Page options
                    PageSetupElement pageSetup = e.Worksheet.WorksheetOptions.PageSetup;
                    pageSetup.PageLayoutElement.IsCenteredVertical = true;
                    pageSetup.PageLayoutElement.IsCenteredHorizontal = true;
                    pageSetup.PageMarginsElement.Left = 0.5;
                    pageSetup.PageMarginsElement.Top = 0.5;
                    pageSetup.PageMarginsElement.Right = 0.5;
                    pageSetup.PageMarginsElement.Bottom = 0.5;
                    pageSetup.PageLayoutElement.PageOrientation = PageOrientationType.Landscape;

                    //Freeze panes
                    e.Worksheet.WorksheetOptions.AllowFreezePanes = true;
                    e.Worksheet.WorksheetOptions.LeftColumnRightPaneNumber = 2;
                    e.Worksheet.WorksheetOptions.TopRowBottomPaneNumber = 1;
                    e.Worksheet.WorksheetOptions.SplitHorizontalOffset = 1;
                    e.Worksheet.WorksheetOptions.SplitVerticalOffest = 2;
                }

            }
            catch (Exception error)
            {
                literal.Text = String.Format("<script languaje='javascript'>" +
                    "alert('Sucedio el siguiente error: {0}');" +
                    "window.location.href = '../Default.aspx';" +
                    "</script>", error.Message);
            }
        }

        protected void grdInversiones_ExcelMLExportStylesCreated(object source, GridExportExcelMLStyleCreatedArgs e)
        {
            try
            {
                StyleElement Presupuesto = new StyleElement("Presupuesto");
                Presupuesto.NumberFormat.FormatType = NumberFormatType.Currency;
                e.Styles.Add(Presupuesto);

                StyleElement Aprobado = new StyleElement("Aprobado");
                Aprobado.NumberFormat.FormatType = NumberFormatType.Currency;
                e.Styles.Add(Aprobado);

                StyleElement C_Aprobado = new StyleElement("C_Aprobado");
                C_Aprobado.NumberFormat.FormatType = NumberFormatType.Currency;
                e.Styles.Add(C_Aprobado);

                StyleElement Pendiente = new StyleElement("Pendiente");
                Pendiente.NumberFormat.FormatType = NumberFormatType.Currency;
                e.Styles.Add(Pendiente);

                StyleElement C_Pendiente = new StyleElement("C_Pendiente");
                C_Pendiente.NumberFormat.FormatType = NumberFormatType.Currency;
                e.Styles.Add(C_Pendiente);

                StyleElement Disponible = new StyleElement("Disponible");
                Disponible.NumberFormat.FormatType = NumberFormatType.Currency;
                e.Styles.Add(Disponible);

                StyleElement PorceDisponible = new StyleElement("PorceDisponible");
                PorceDisponible.NumberFormat.FormatType = NumberFormatType.Percent;
                PorceDisponible.FontStyle.Italic = true;
                e.Styles.Add(PorceDisponible);

                StyleElement Usado = new StyleElement("Usado");
                Usado.NumberFormat.FormatType = NumberFormatType.Percent;
                Usado.FontStyle.Italic = true;
                e.Styles.Add(Usado);
            }
            catch (Exception error)
            {
                literal.Text = String.Format("<script languaje='javascript'>" +
                    "alert('Sucedio el siguiente error: {0}');" +
                    "window.location.href = '../Default.aspx';" +
                    "</script>", error.Message);
            }
        }
        #endregion

        #region Grid cantidades
        protected void grdCantidad_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Connection conexion = new Connection();

                string query = String.Format("select CLIENTES_KC.CLIENT_ID, CLIENTES_KC.CLIENT_NAME, " +
                    "(select COUNT(SOLICITUD_DISPENSADORES.DR_ID) " +
                    "from SOLICITUD_DISPENSADORES " +
                    "where (SOLICITUD_DISPENSADORES.STATUS_ID = 2 and SOLICITUD_DISPENSADORES.CLIENT_ID = CLIENTES_KC.CLIENT_ID) and SOLICITUD_DISPENSADORES.DATE_REQUEST between '{0}' and '{1}') " +
                    "as SOLICITUDES_APROBADAS, " +

                    "(select COUNT(SOLICITUD_DISPENSADORES.DR_ID) " +
                    "from SOLICITUD_DISPENSADORES " +
                    "where ((SOLICITUD_DISPENSADORES.STATUS_ID = 1 OR SOLICITUD_DISPENSADORES.STATUS_ID = 5) and SOLICITUD_DISPENSADORES.CLIENT_ID = CLIENTES_KC.CLIENT_ID) and SOLICITUD_DISPENSADORES.DATE_REQUEST between '{0}' and '{1}') " +
                    "as SOLICITUDES_PENDIENTES, " +

                    "(select COUNT(SOLICITUD_DISPENSADORES.DR_ID) " +
                    "from SOLICITUD_DISPENSADORES " +
                    "where (SOLICITUD_DISPENSADORES.STATUS_ID = 3 and SOLICITUD_DISPENSADORES.CLIENT_ID = CLIENTES_KC.CLIENT_ID) and SOLICITUD_DISPENSADORES.DATE_REQUEST between '{0}' and '{1}') " +
                    "as SOLICITUDES_RECHAZADAS " +

                    "from CLIENTES_KC " +
                    "where CLIENTES_KC.COUNTRY='{2}' and CLIENTES_KC.CLIENT_ID <> '40000000'",
                    new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyMMdd"), DateTime.Now.ToString("yyyMMdd"), conexion.getUsersInfo("ID_COUNTRY", "USER_ID", Session.Contents["userid"].ToString()));

                DataTable resultset = conexion.getGridDataSource(query);
                DataTable tablaGrid = new DataTable();

                tablaGrid.Columns.Add("CLIENT_ID", typeof(string));
                tablaGrid.Columns.Add("CLIENT_NAME", typeof(string));
                tablaGrid.Columns.Add("SOLICITUDES_APROBADAS", typeof(int));
                tablaGrid.Columns.Add("SOLICITUDES_PENDIENTES", typeof(int));
                tablaGrid.Columns.Add("SOLICITUDES_RECHAZADAS", typeof(int));
                tablaGrid.Columns.Add("PORCE_APRO", typeof(double));
                tablaGrid.Columns.Add("PORCE_PENDI", typeof(double));
                tablaGrid.Columns.Add("PORCE_RECH", typeof(double));

                string[] campos = new string[8];

                double aprobadosTotal = 0;
                double pendientesTotal = 0;
                double rechazadoTotal = 0;
                double totales = 0;

                double porceAproTot = 0;
                double porcePendTot = 0;
                double porceRechTot = 0;

                foreach (DataRow fila in resultset.Rows)
                {
                    campos[0] = fila["CLIENT_ID"].ToString();
                    campos[1] = fila["CLIENT_NAME"].ToString();

                    double aprobados = Convert.ToInt64(fila["SOLICITUDES_APROBADAS"].ToString());
                    double pendientes = Convert.ToInt64(fila["SOLICITUDES_PENDIENTES"].ToString());
                    double rechazados = Convert.ToInt64(fila["SOLICITUDES_RECHAZADAS"].ToString());
                    double totalTipos = (aprobados + pendientes + rechazados);

                    double porceApro = 0;
                    double porcePend = 0;
                    double porceRech = 0;

                    porceApro = (totalTipos > 0) ? aprobados / totalTipos : 0;
                    porcePend = (totalTipos > 0) ? pendientes / totalTipos : 0;
                    porceRech = (totalTipos > 0) ? rechazados / totalTipos : 0;

                    campos[2] = aprobados.ToString();
                    campos[3] = pendientes.ToString();
                    campos[4] = rechazados.ToString();
                    campos[5] = Math.Round(porceApro, 2).ToString();
                    campos[6] = Math.Round(porcePend, 2).ToString();
                    campos[7] = Math.Round(porceRech, 2).ToString();

                    aprobadosTotal += aprobados;
                    pendientesTotal += pendientes;
                    rechazadoTotal += rechazados;

                    tablaGrid.LoadDataRow(campos, false);

                }

                totales = (aprobadosTotal + pendientesTotal + rechazadoTotal);
                porceAproTot = (totales > 0) ? aprobadosTotal / totales : 0;
                porcePendTot = (totales > 0) ? pendientesTotal / totales : 0;
                porceRechTot = (totales > 0) ? rechazadoTotal / totales : 0;

                campos[0] = String.Empty;
                campos[1] = "Total Pais";
                campos[2] = aprobadosTotal.ToString();
                campos[3] = pendientesTotal.ToString();
                campos[4] = rechazadoTotal.ToString();

                campos[5] = Math.Round(porceAproTot, 2).ToString();
                campos[6] = Math.Round(porcePendTot, 2).ToString();
                campos[7] = Math.Round(porceRechTot, 2).ToString();

                tablaGrid.LoadDataRow(campos, false);
                grdCantidad.DataSource = tablaGrid;

            }
            catch (Exception error)
            {
                literal.Text = String.Format("<script languaje='javascript'>" +
                    "alert('Sucedio el siguiente error: {0}');" +
                    "window.location.href = '../Default.aspx';" +
                    "</script>", error.Message);
            }
        }

        protected void grdCantidad_ExcelMLExportRowCreated(object source, GridExportExcelMLRowCreatedArgs e)
        {

            try
            {
                if (e.RowType == GridExportExcelMLRowType.DataRow)
                {

                    CellElement cell = e.Row.Cells.GetCellByName("porceApro");
                    cell.StyleValue = "porceApro";

                    cell = e.Row.Cells.GetCellByName("porcePend");
                    cell.StyleValue = "porcePend";

                    cell = e.Row.Cells.GetCellByName("porceRech");
                    cell.StyleValue = "porceRech";

                    e.Worksheet.Name = "Cantidades por estatus.";

                    //Set Page options
                    PageSetupElement pageSetup = e.Worksheet.WorksheetOptions.PageSetup;
                    pageSetup.PageLayoutElement.IsCenteredVertical = true;
                    pageSetup.PageLayoutElement.IsCenteredHorizontal = true;
                    pageSetup.PageMarginsElement.Left = 0.5;
                    pageSetup.PageMarginsElement.Top = 0.5;
                    pageSetup.PageMarginsElement.Right = 0.5;
                    pageSetup.PageMarginsElement.Bottom = 0.5;
                    pageSetup.PageLayoutElement.PageOrientation = PageOrientationType.Landscape;

                    //Freeze panes
                    e.Worksheet.WorksheetOptions.AllowFreezePanes = true;
                    e.Worksheet.WorksheetOptions.LeftColumnRightPaneNumber = 2;
                    e.Worksheet.WorksheetOptions.TopRowBottomPaneNumber = 1;
                    e.Worksheet.WorksheetOptions.SplitHorizontalOffset = 1;
                    e.Worksheet.WorksheetOptions.SplitVerticalOffest = 2;
                }
            }
            catch (Exception error)
            {
                literal.Text = String.Format("<script languaje='javascript'>" +
                    "alert('Sucedio el siguiente error: {0}');" +
                    "window.location.href = '../Default.aspx';" +
                    "</script>", error.Message);
            }
        }

        protected void grdCantidad_ExcelMLExportStylesCreated(object source, GridExportExcelMLStyleCreatedArgs e)
        {

            try
            {
                StyleElement porceApro = new StyleElement("porceApro");
                porceApro.NumberFormat.FormatType = NumberFormatType.Percent;
                porceApro.FontStyle.Italic = true;
                e.Styles.Add(porceApro);

                StyleElement porcePend = new StyleElement("porcePend");
                porcePend.NumberFormat.FormatType = NumberFormatType.Percent;
                porcePend.FontStyle.Italic = true;
                e.Styles.Add(porcePend);

                StyleElement porceRech = new StyleElement("porceRech");
                porceRech.NumberFormat.FormatType = NumberFormatType.Percent;
                porceRech.FontStyle.Italic = true;
                e.Styles.Add(porceRech);
            }
            catch (Exception error)
            {
                literal.Text = String.Format("<script languaje='javascript'>" +
                    "alert('Sucedio el siguiente error: {0}');" +
                    "window.location.href = '../Default.aspx';" +
                    "</script>", error.Message);
            }
        }
        #endregion

        #region Grid Cantidad por motivos
        protected void grdMotivoCantidad_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Connection conexion = new Connection();

                string query = String.Format("SELECT CLIENTES_KC.CLIENT_ID, CLIENTES_KC.CLIENT_NAME, "
                    + "(SELECT COUNT(SOLICITUD_DISPENSADORES.DR_ID) "
                    + "FROM SOLICITUD_DISPENSADORES "
                    + "WHERE (SOLICITUD_DISPENSADORES.REASON_ID = 3 AND SOLICITUD_DISPENSADORES.CLIENT_ID = CLIENTES_KC.CLIENT_ID) AND (SOLICITUD_DISPENSADORES.DATE_REQUEST BETWEEN '{0}' AND '{1}')) AS NUEVOS, "

                    + "(SELECT COUNT(SOLICITUD_DISPENSADORES.DR_ID) "
                    + "FROM SOLICITUD_DISPENSADORES "
                    + "WHERE (SOLICITUD_DISPENSADORES.REASON_ID = 6 AND SOLICITUD_DISPENSADORES.CLIENT_ID = CLIENTES_KC.CLIENT_ID) AND (SOLICITUD_DISPENSADORES.DATE_REQUEST BETWEEN '{0}' AND '{1}')) AS AMPLIACION, "

                    + "(SELECT COUNT(SOLICITUD_DISPENSADORES.DR_ID) "
                    + "FROM SOLICITUD_DISPENSADORES "
                    + "WHERE (SOLICITUD_DISPENSADORES.REASON_ID = 4 AND SOLICITUD_DISPENSADORES.CLIENT_ID = CLIENTES_KC.CLIENT_ID) AND (SOLICITUD_DISPENSADORES.DATE_REQUEST BETWEEN '{0}' AND '{1}')) AS DETERIORO, "

                    + "(SELECT COUNT(SOLICITUD_DISPENSADORES.DR_ID) "
                    + "FROM SOLICITUD_DISPENSADORES "
                    + "WHERE ((SOLICITUD_DISPENSADORES.REASON_ID = 1 OR SOLICITUD_DISPENSADORES.REASON_ID = 2 OR SOLICITUD_DISPENSADORES.REASON_ID = 5) AND SOLICITUD_DISPENSADORES.CLIENT_ID = CLIENTES_KC.CLIENT_ID) AND ((SOLICITUD_DISPENSADORES.DATE_REQUEST BETWEEN '{0}' AND '{1}'))) AS OTROS "

                    + "FROM CLIENTES_KC "
                    + "WHERE CLIENTES_KC.COUNTRY = '{2}' AND CLIENTES_KC.CLIENT_ID <> '40000000'",
                    new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyMMdd"), DateTime.Now.ToString("yyyMMdd"), conexion.getUsersInfo("ID_COUNTRY", "USER_ID", Session.Contents["userid"].ToString()));

                DataTable resultset = conexion.getGridDataSource(query);
                DataTable data = new DataTable();

                data.Columns.Add("CLIENT_ID", typeof(string));
                data.Columns.Add("CLIENT_NAME", typeof(string));
                data.Columns.Add("NUEVOS", typeof(int));
                data.Columns.Add("AMPLIACION", typeof(int));
                data.Columns.Add("DETERIORO", typeof(int));
                data.Columns.Add("OTROS", typeof(int));
                data.Columns.Add("TOTAL", typeof(int));

                int totalNuevos = 0;
                int totalAmpliacion = 0;
                int totalDeterioro = 0;
                int totalOtros = 0;
                int totalPais = 0;

                string[] campos = new string[7];

                foreach (DataRow fila in resultset.Rows)
                {
                    campos[0] = fila["CLIENT_ID"].ToString();
                    campos[1] = fila["CLIENT_NAME"].ToString();
                    campos[2] = fila["NUEVOS"].ToString();
                    campos[3] = fila["AMPLIACION"].ToString();
                    campos[4] = fila["DETERIORO"].ToString();
                    campos[5] = fila["OTROS"].ToString();

                    int nuevos = Convert.ToInt32(fila["NUEVOS"].ToString());
                    int ampliacion = Convert.ToInt32(fila["AMPLIACION"].ToString());
                    int deterioro = Convert.ToInt32(fila["DETERIORO"].ToString());
                    int otros = Convert.ToInt32(fila["OTROS"].ToString());

                    totalNuevos += nuevos;
                    totalAmpliacion += ampliacion;
                    totalDeterioro += deterioro;
                    totalOtros += otros;

                    int total = nuevos + ampliacion + deterioro + otros;
                    campos[6] = total.ToString();

                    data.LoadDataRow(campos, false);

                }

                totalPais = totalNuevos + totalAmpliacion + totalDeterioro + totalOtros;

                campos[0] = String.Empty;
                campos[1] = "Total Pais:";
                campos[2] = totalNuevos.ToString();
                campos[3] = totalAmpliacion.ToString();
                campos[4] = totalDeterioro.ToString();
                campos[5] = totalOtros.ToString();
                campos[6] = totalPais.ToString();

                data.LoadDataRow(campos, false);
                grdMotivoCantidad.DataSource = data;

            }
            catch (Exception error)
            {
                literal.Text = String.Format("<script languaje='javascript'>" +
                    "alert('Sucedio el siguiente error: {0}');" +
                    "window.location.href = '../Default.aspx';" +
                    "</script>", error.Message);
            }
        }

        protected void grdMotivoCantidad_ExcelMLExportRowCreated(object source, GridExportExcelMLRowCreatedArgs e)
        {
            try
            {
                //Set Page options
                PageSetupElement pageSetup = e.Worksheet.WorksheetOptions.PageSetup;
                pageSetup.PageLayoutElement.IsCenteredVertical = true;
                pageSetup.PageLayoutElement.IsCenteredHorizontal = true;
                pageSetup.PageMarginsElement.Left = 0.5;
                pageSetup.PageMarginsElement.Top = 0.5;
                pageSetup.PageMarginsElement.Right = 0.5;
                pageSetup.PageMarginsElement.Bottom = 0.5;
                pageSetup.PageLayoutElement.PageOrientation = PageOrientationType.Landscape;

                //Freeze panes
                e.Worksheet.WorksheetOptions.AllowFreezePanes = true;
                e.Worksheet.WorksheetOptions.LeftColumnRightPaneNumber = 2;
                e.Worksheet.WorksheetOptions.TopRowBottomPaneNumber = 1;
                e.Worksheet.WorksheetOptions.SplitHorizontalOffset = 1;
                e.Worksheet.WorksheetOptions.SplitVerticalOffest = 2;
            }
            catch (Exception error)
            {
                literal.Text = String.Format("<script languaje='javascript'>" +
                    "alert('Sucedio el siguiente error al configurar el archivo de cantidad x motivo: {0}');" +
                    "window.location.href = '../Default.aspx';" +
                    "</script>", error.Message);
            }
        }
        #endregion

        #region Grid Plata por motivo
        protected void grdMotivoInversion_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Connection conexion = new Connection();

                string query = String.Format("SELECT CLIENTES_KC.CLIENT_ID, CLIENTES_KC.CLIENT_NAME, "

                    + "(SELECT CASE WHEN SUM(SOLICITUD_DISPENSADORES.INVER_APRO) > 0 THEN SUM(SOLICITUD_DISPENSADORES.INVER_APRO) ELSE 0 END AS SUMA "
                    + "FROM SOLICITUD_DISPENSADORES "
                    + "WHERE (SOLICITUD_DISPENSADORES.REASON_ID = 3 AND SOLICITUD_DISPENSADORES.CLIENT_ID = CLIENTES_KC.CLIENT_ID) AND (SOLICITUD_DISPENSADORES.DATE_REQUEST "
                    + "BETWEEN '{0}' AND '{1}')) AS NUEVOS, "

                    + "(SELECT CASE WHEN SUM(SOLICITUD_DISPENSADORES.INVER_APRO) > 0 THEN SUM(SOLICITUD_DISPENSADORES.INVER_APRO) ELSE 0 END AS SUMA "
                    + "FROM SOLICITUD_DISPENSADORES "
                    + "WHERE (SOLICITUD_DISPENSADORES.REASON_ID = 6 AND SOLICITUD_DISPENSADORES.CLIENT_ID = CLIENTES_KC.CLIENT_ID) AND (SOLICITUD_DISPENSADORES.DATE_REQUEST "
                    + "BETWEEN '{0}' AND '{1}')) AS AMPLIACION, "

                    + "(SELECT CASE WHEN SUM(SOLICITUD_DISPENSADORES.INVER_APRO) > 0 THEN SUM(SOLICITUD_DISPENSADORES.INVER_APRO) ELSE 0 END AS SUMA "
                    + "FROM SOLICITUD_DISPENSADORES "
                    + "WHERE (SOLICITUD_DISPENSADORES.REASON_ID = 4 AND SOLICITUD_DISPENSADORES.CLIENT_ID = CLIENTES_KC.CLIENT_ID) AND (SOLICITUD_DISPENSADORES.DATE_REQUEST "
                    + "BETWEEN '{0}' AND '{1}')) AS DETERIORO, "

                    + "(SELECT CASE WHEN SUM(SOLICITUD_DISPENSADORES.INVER_APRO) > 0 THEN SUM(SOLICITUD_DISPENSADORES.INVER_APRO) ELSE 0 END AS SUMA "
                    + "FROM SOLICITUD_DISPENSADORES "
                    + "WHERE ((SOLICITUD_DISPENSADORES.REASON_ID = 1 OR SOLICITUD_DISPENSADORES.REASON_ID = 2 OR SOLICITUD_DISPENSADORES.REASON_ID = 5) AND "
                    + "SOLICITUD_DISPENSADORES.CLIENT_ID = CLIENTES_KC.CLIENT_ID) AND ((SOLICITUD_DISPENSADORES.DATE_REQUEST BETWEEN '{0}' AND '{1}'))) AS OTROS "

                    + "FROM CLIENTES_KC "
                    + "WHERE CLIENTES_KC.COUNTRY = '{2}' AND CLIENTES_KC.CLIENT_ID <> '40000000'",
                    new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyMMdd"), DateTime.Now.ToString("yyyMMdd"), conexion.getUsersInfo("ID_COUNTRY", "USER_ID", Session.Contents["userid"].ToString()));

                DataTable resultset = conexion.getGridDataSource(query);
                DataTable data = new DataTable();

                data.Columns.Add("CLIENT_ID", typeof(string));
                data.Columns.Add("CLIENT_NAME", typeof(string));
                data.Columns.Add("NUEVOS", typeof(double));
                data.Columns.Add("AMPLIACION", typeof(double));
                data.Columns.Add("DETERIORO", typeof(double));
                data.Columns.Add("OTROS", typeof(double));
                data.Columns.Add("TOTAL", typeof(double));

                double totalNuevos = 0;
                double totalAmpliacion = 0;
                double totalDeterioro = 0;
                double totalOtros = 0;
                double totalPais = 0;

                string[] campos = new string[7];

                foreach (DataRow fila in resultset.Rows)
                {
                    campos[0] = fila["CLIENT_ID"].ToString();
                    campos[1] = fila["CLIENT_NAME"].ToString();
                    campos[2] = fila["NUEVOS"].ToString();
                    campos[3] = fila["AMPLIACION"].ToString();
                    campos[4] = fila["DETERIORO"].ToString();
                    campos[5] = fila["OTROS"].ToString();

                    double nuevos = Convert.ToDouble(fila["NUEVOS"].ToString());
                    double ampliacion = Convert.ToDouble(fila["AMPLIACION"].ToString());
                    double deterioro = Convert.ToDouble(fila["DETERIORO"].ToString());
                    double otros = Convert.ToDouble(fila["OTROS"].ToString());

                    totalNuevos += nuevos;
                    totalAmpliacion += ampliacion;
                    totalDeterioro += deterioro;
                    totalOtros += otros;

                    double total = nuevos + ampliacion + deterioro + otros;
                    campos[6] = total.ToString();

                    data.LoadDataRow(campos, false);
                }

                totalPais = totalNuevos + totalAmpliacion + totalDeterioro + totalOtros;

                campos[0] = String.Empty;
                campos[1] = "Total Pais:";
                campos[2] = totalNuevos.ToString();
                campos[3] = totalAmpliacion.ToString();
                campos[4] = totalDeterioro.ToString();
                campos[5] = totalOtros.ToString();
                campos[6] = totalPais.ToString();

                data.LoadDataRow(campos, false);
                grdMotivoInversion.DataSource = data;
            }
            catch (Exception error)
            {
                literal.Text = String.Format("<script languaje='javascript'>" +
                    "alert('Sucedio el siguiente error al cargar el motivo por inversion: {0}');" +
                    "window.location.href = '../Default.aspx';" +
                    "</script>", error.Message);
            }

        }

        protected void grdMotivoInversion_ExcelMLExportRowCreated(object source, GridExportExcelMLRowCreatedArgs e)
        {
            try
            {
                if (e.RowType == GridExportExcelMLRowType.DataRow)
                {

                    CellElement cell = e.Row.Cells.GetCellByName("SolNuevas");
                    cell.StyleValue = "SolNuevas";

                    cell = e.Row.Cells.GetCellByName("SolAmpliacion");
                    cell.StyleValue = "SolAmpliacion";

                    cell = e.Row.Cells.GetCellByName("SolDeterioro");
                    cell.StyleValue = "SolDeterioro";

                    cell = e.Row.Cells.GetCellByName("Otros");
                    cell.StyleValue = "Otros";

                    cell = e.Row.Cells.GetCellByName("Total");
                    cell.StyleValue = "Total";

                    e.Worksheet.Name = "Cantidades por estatus";

                    //Set Page options
                    PageSetupElement pageSetup = e.Worksheet.WorksheetOptions.PageSetup;
                    pageSetup.PageLayoutElement.IsCenteredVertical = true;
                    pageSetup.PageLayoutElement.IsCenteredHorizontal = true;
                    pageSetup.PageMarginsElement.Left = 0.5;
                    pageSetup.PageMarginsElement.Top = 0.5;
                    pageSetup.PageMarginsElement.Right = 0.5;
                    pageSetup.PageMarginsElement.Bottom = 0.5;
                    pageSetup.PageLayoutElement.PageOrientation = PageOrientationType.Landscape;

                    //Freeze panes
                    e.Worksheet.WorksheetOptions.AllowFreezePanes = true;
                    e.Worksheet.WorksheetOptions.LeftColumnRightPaneNumber = 2;
                    e.Worksheet.WorksheetOptions.TopRowBottomPaneNumber = 1;
                    e.Worksheet.WorksheetOptions.SplitHorizontalOffset = 1;
                    e.Worksheet.WorksheetOptions.SplitVerticalOffest = 2;
                }
            }
            catch (Exception error)
            {
                literal.Text = String.Format("<script languaje='javascript'>" +
                    "alert('Sucedio el siguiente error al configurar el archivo de inversion x motivo: {0}');" +
                    "window.location.href = '../Default.aspx';" +
                    "</script>", error.Message);
            }
        }

        protected void grdMotivoInversion_ExcelMLExportStylesCreated(object source, GridExportExcelMLStyleCreatedArgs e)
        {
            try
            {
                StyleElement SolNuevas = new StyleElement("SolNuevas");
                SolNuevas.NumberFormat.FormatType = NumberFormatType.Currency;
                SolNuevas.FontStyle.Italic = true;
                e.Styles.Add(SolNuevas);

                StyleElement SolAmpliacion = new StyleElement("SolAmpliacion");
                SolAmpliacion.NumberFormat.FormatType = NumberFormatType.Currency;
                SolAmpliacion.FontStyle.Italic = true;
                e.Styles.Add(SolAmpliacion);

                StyleElement SolDeterioro = new StyleElement("SolDeterioro");
                SolDeterioro.NumberFormat.FormatType = NumberFormatType.Currency;
                SolDeterioro.FontStyle.Italic = true;
                e.Styles.Add(SolDeterioro);

                StyleElement Otros = new StyleElement("Otros");
                Otros.NumberFormat.FormatType = NumberFormatType.Currency;
                Otros.FontStyle.Italic = true;
                e.Styles.Add(Otros);

                StyleElement Total = new StyleElement("Total");
                Total.NumberFormat.FormatType = NumberFormatType.Currency;
                Total.FontStyle.Italic = true;
                e.Styles.Add(Total);
            }
            catch (Exception error)
            {
                literal.Text = String.Format("<script languaje='javascript'>" +
                    "alert('Sucedio el siguiente error al exportar inversion x motivo: {0}');" +
                    "window.location.href = '../Default.aspx';" +
                    "</script>", error.Message);
            }
        }
        #endregion
    }
}