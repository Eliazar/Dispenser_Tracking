using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//Agregados
using System.Data;

namespace Dispenser
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Connection conexion = new Connection();
                Session.Add("historial", String.Empty);

                DateTime hoy = DateTime.Now;
                string clientId = conexion.getUsersInfo("CLIENT_ID", "USER_ID", Session["userid"].ToString());

                if (Session["rol"].Equals("KCPCCR") || Session["rol"].Equals("KCPADM"))
                {
                    parrafo1.Text = "Desde aqui podras autorizar, revisar y rechazar solicitudes de nuestros socios comerciales. " +
                        "¿Cómo dar seguimiento a las solicitudues? ahora te decimos:";

                    parrafo2.Visible = false;
                    /*string query = String.Format("SELECT DISTINCT(R.REASON_DESCRIP), SUM(SD.INVER_SOLICITADA) AS INVERSION_TOTAL "
                    + "FROM SOLICITUD_DISPENSADORES AS SD "
                    + "INNER JOIN RAZONES AS R ON R.ID_REASON = SD.REASON_ID "
                    + "WHERE (SD.STATUS_ID = 1 OR STATUS_ID = 5) AND (SD.DATE_REQUEST BETWEEN '{1}' AND '{2}') "
                    + "GROUP BY R.REASON_DESCRIP"
                    , clientId, new DateTime(hoy.Year, hoy.Month, 1).ToString("yyyMMdd"), hoy.ToString("yyyMMdd"));

                    chrtConsumo.DataSource = conexion.getGridDataSource(query);
                    chrtConsumo.Visible = true;*/

                    string query = String.Format("SELECT COUNT(DR_ID) AS PENDIENTE "
                    + "FROM SOLICITUD_DISPENSADORES "
                    + "WHERE ID_COUNTRY = '{0}' AND (STATUS_ID = 1 OR STATUS_ID = 5) AND CLIENT_ID <> '40000000'"
                    , conexion.getUsersInfo("ID_COUNTRY", "USER_ID", Session.Contents["userid"].ToString()));

                    DataTable dataset = conexion.getGridDataSource(query);
                    lblTP.Text = "<b>Pendientes Total:</b> " + Convert.ToString(dataset.Rows[0]["PENDIENTE"]);

                    query = String.Format("SELECT COUNT(DR_ID) AS APROBADO "
                    + "FROM SOLICITUD_DISPENSADORES "
                    + "WHERE ID_COUNTRY = '{0}' AND STATUS_ID = 2 AND CLIENT_ID <> '40000000'"
                    , conexion.getUsersInfo("ID_COUNTRY", "USER_ID", Session.Contents["userid"].ToString()));

                    dataset = conexion.getGridDataSource(query);
                    lblInvAut.Text = "<b>Aprobados Total:</b> " + Convert.ToString(dataset.Rows[0]["APROBADO"]);

                    query = String.Format("SELECT COUNT(DR_ID) AS RECHAZADO "
                    + "FROM SOLICITUD_DISPENSADORES "
                    + "WHERE ID_COUNTRY = '{0}' AND STATUS_ID = 3 AND CLIENT_ID <> '40000000'"
                    , conexion.getUsersInfo("ID_COUNTRY", "USER_ID", Session.Contents["userid"].ToString()));

                    dataset = conexion.getGridDataSource(query);
                    lblInvPend.Text = "<b>Rechazados Total:</b> " + Convert.ToString(dataset.Rows[0]["RECHAZADO"]);

                    query = String.Format("SELECT COUNT(DR_ID) AS CERRADA "
                    + "FROM SOLICITUD_DISPENSADORES "
                    + "WHERE ID_COUNTRY = '{0}' AND STATUS_ID = 4 AND CLIENT_ID <> '40000000'"
                    , conexion.getUsersInfo("ID_COUNTRY", "USER_ID", Session.Contents["userid"].ToString()));

                    dataset = conexion.getGridDataSource(query);
                    lblInvUsada.Text = "<b>Cerradas Total:</b> " + Convert.ToString(dataset.Rows[0]["CERRADA"]);
                }

                DateTime vencimiento = Convert.ToDateTime(conexion.getPExpiration(Session["userid"].ToString()));

                if (!clientId.Equals(String.Empty))
                {
                    string query = String.Format("SELECT BUDGET_TP, INVERSION, INVERSION_FLOTANTE FROM CLIENTES_KC WHERE CLIENT_ID = '{0}'", clientId);
                    DataTable tabla = conexion.getGridDataSource(query);

                    foreach (DataRow fila in tabla.Rows)
                    {
                        double tp = Convert.ToDouble(fila["BUDGET_TP"]);
                        double inversion = Convert.ToDouble(fila["INVERSION"]);
                        double inversionF = Convert.ToDouble(fila["INVERSION_FLOTANTE"]);

                        lblTP.Text = "<b>Presupuesto:</b> $" + tp.ToString();
                        lblInvAut.Text = "<b>Inversion:</b> $" + inversion.ToString();
                        lblInvPend.Text = "<b>Inversion pendiente:</b> $" + inversionF.ToString();
                        lblInvUsada.Text = "<b>% Uso de Presupuesto:</b> " + Convert.ToDouble(((inversion + inversionF) / tp) * 100).ToString("0.00") + "%";

                        if (inversionF > tp)
                            lblInvPend.ForeColor = System.Drawing.Color.Red;

                        if (Convert.ToDouble(((inversion + inversionF) / tp) * 100) > 100)
                            lblInvUsada.ForeColor = System.Drawing.Color.Red;

                        Session.Add("presupuesto", tp);
                        Session.Add("aprobado", inversion);
                        Session.Add("pendiente", inversionF);
                        Session.Add("porcentaje", Convert.ToDouble(((inversion + inversionF) / tp) * 100));
                    }
                }


                int diferencia = vencimiento.DayOfYear - hoy.DayOfYear;

                //para cambio de año
                if (diferencia < 0)
                    diferencia = diferencia * -1;


                if (diferencia <= 5)
                {
                    mensajes.Text = String.Format("<script languaje='javascript'>" +
                                        "alert('Su contraseña vencera dentro de {0} dias');" +
                                     "</script>", diferencia);
                }

                string historial = Session["historial"].ToString();
                historial += "Default; ";
                Session["historial"] = historial;
            }
            catch (Exception error)
            {
                mensajes.Text = String.Format("<script languaje='javascript'>" +
                                        "alert('{0}');" +
                                        "window.location.href = 'Default.aspx';" +
                                     "</script>", error.Message);
            }
        }
    }
}