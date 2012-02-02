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

                DateTime hoy = DateTime.Now;
                string clientId = conexion.getUsersInfo("CLIENT_ID", "USER_ID", Session["userid"].ToString());

                if (Session["rol"].Equals("KCPCCR") || Session["rol"].Equals("KCPADM"))
                {
                    parrafo1.Text = "Desde aqui podras autorizar, revisar y rechazar solicitudes de nuestros socios comerciales. " +
                        "¿Cómo dar seguimiento a las solicitudues? ahora te decimos:";

                    parrafo2.Visible = false;

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
                        double disponibilidad = tp - (inversion + inversionF);
                        double porcentaje = ((disponibilidad) / tp) * 100;

                        lblTP.Text = "<b>Presupuesto:</b> $" + tp.ToString();
                        lblInvAut.Text = "<b>Inversion Aprobada:</b> $" + inversion.ToString("0.00");
                        lblInvPend.Text = "<b>Inversion Pendiente:</b> $" + inversionF.ToString("0.00");

                        if (disponibilidad > 0)
                            lblRestante.Text = "<b>Presupuesto Restante:</b> $" + disponibilidad.ToString("0.00");
                        else
                        {
                            lblRestante.ForeColor = System.Drawing.Color.Red;
                            lblRestante.Text = "$0.0";
                        }

                        if (porcentaje > 0)
                            lblInvUsada.Text = "<b>% Disponibilidad:</b> " + porcentaje.ToString("0.00") + "%";
                        else
                        {
                            lblInvUsada.ForeColor = System.Drawing.Color.Red;
                            lblInvUsada.Text = "0%";
                        }

                        
                        Session.Add("presupuesto", tp);
                        Session.Add("aprobado", inversion);
                        Session.Add("pendiente", inversionF);
                        Session.Add("porcentaje", Convert.ToDouble(((inversion + inversionF) / tp) * 100));
                        Session.Add("correoBajo", false);
                        Session.Add("correoSobre", false);
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
            }
            catch (Exception error)
            {
                mensajes.Text = String.Format("<script languaje='javascript'>" +
                                        "alert('{0}');" +
                                        "window.location.href = 'About.aspx';" +
                                     "</script>", error.Message);
            }
        }
    }
}