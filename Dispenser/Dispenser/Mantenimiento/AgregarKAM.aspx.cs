using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//Agregados
using Telerik.Web.UI;
using System.Data;

namespace Dispenser.Mantenimiento
{
    public partial class AgregarKAM : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Menu menu = Master.FindControl("NavigationMenu") as Menu;
                menu.Items[2].ChildItems[1].ChildItems.RemoveAt(0);

                if (Session.Contents["rol"].ToString().Equals("DSTADM"))
                    Response.Redirect("../Default.aspx");
                
                Connection conexion = new Connection();
                string query = String.Empty;

                if (!IsPostBack)
                {
                    query = String.Format("select ckc.CLIENT_ID, ckc.CLIENT_NAME from CLIENTES_KC as ckc where ckc.COUNTRY = '{0}' and ckc.DIRECT_CUSTOMER = 0 and 0 = (select COUNT(ck.KAM_ID) from CUENTAS_KAM as ck where ck.CLIENT_ID = ckc.CLIENT_ID)",
                        conexion.getUserCountry(Session.Contents["userid"].ToString()));

                    DataTable resultado = conexion.getGridDataSource(query);

                    foreach (DataRow fila in resultado.Rows)
                    {
                        RadListBoxItem item = new RadListBoxItem();
                        item.Text = fila["CLIENT_ID"].ToString() + "|" + fila["CLIENT_NAME"].ToString();
                        item.Value = fila["CLIENT_ID"].ToString();

                        item.DataBind();
                        lbxCuentasFuente.Items.Add(item);
                    }
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

        protected void btCrear_Click(object sender, EventArgs e)
        {

            try
            {
                if (lbxCuentasDestino.Items.Count == 0)
                {
                    lblMensajes.Text = String.Format("<script languaje='javascript'>" +
                                        "alert('No se encontraron cuentas asignadas para este KAM.');" +
                                     "</script>");
                    return;
                }

                Connection conexion = new Connection();
                string query = String.Empty;
                DataTable tabla = new DataTable();

                query = String.Format("SELECT USER_ID FROM USUARIOS WHERE ID_COUNTRY = '{0}' AND CLIENT_ID IS NULL AND STATUS = 1",
                    conexion.getUserCountry(Session.Contents["userid"].ToString()));
                tabla = conexion.getGridDataSource(query);

                foreach (DataRow fila in tabla.Rows)
                {
                    if (txtUsuario.Text.ToUpper().Equals(fila["USER_ID"].ToString()))
                    {
                        lblMensajes.Text = String.Format("<script languaje='javascript'>" +
                                        "alert('El usuario KC corresponde a un CTM o Customer Care imposible continuar.');" +
                                     "</script>");
                        return;
                    }
                }

                query = String.Format("SELECT KAM_ID FROM KAM WHERE KAM_ACTIVE = 1 AND COUNTRY = '{0}'",
                    conexion.getUserCountry(Session["userid"].ToString()));
                tabla = conexion.getGridDataSource(query);

                foreach (DataRow fila in tabla.Rows)
                {
                    if (txtUsuario.Text.ToUpper().Equals(fila["KAM_ID"].ToString()))
                    {
                        lblMensajes.Text = String.Format("<script languaje='javascript'>" +
                                        "alert('El usuario KC ya corresponde a un KAM imposible continuar.');" +
                                     "</script>");
                        return;
                    }
                }

                query = String.Format("INSERT INTO KAM (KAM_ID, KAM_NAME, KAM_MAIL, KAM_ACTIVE, RECEIVE_MAIL, COUNTRY) VALUES ('{0}', '{1}', '{2}', 1, '{3}', '{4}')",
                    txtUsuario.Text.ToUpper(), txtNombre.Text, txtCorreo.Text, chkCorreo.Checked, conexion.getUserCountry(Session.Contents["userid"].ToString()));

                int contador = 0;

                if (conexion.Actualizar(query))
                {
                    foreach (RadListBoxItem item in lbxCuentasDestino.Items)
                    {
                        query = String.Format("INSERT INTO CUENTAS_KAM (KAM_ID, CLIENT_ID) VALUES ('{0}', '{1}')", txtUsuario.Text.ToUpper(), item.Value);

                        if (!conexion.Actualizar(query))
                        {
                            lblMensajes.Text = String.Format("<script languaje='javascript'>" +
                                        "alert('Error inesperado de inserccion imposible continuar. Datos insertados {0}');" +
                                     "</script>", contador);
                            return;
                        }

                        contador++;

                    }

                    lblMensajes.Text = String.Format("<script languaje='javascript'>" +
                                        "alert('Exito al guardar. Datos de cuenta insertados {0}');" +
                                        "window.location.href = '../Default.aspx';" +
                                     "</script>", contador);

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
    }
}