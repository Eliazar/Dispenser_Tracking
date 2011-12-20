using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//Agregados
using System.Data;

namespace Dispenser.Mantenimiento
{
    public partial class Nuevo_Socio : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Menu menu = Master.FindControl("NavigationMenu") as Menu;
            menu.Items[2].ChildItems[2].ChildItems.RemoveAt(0);
        }

        protected void btCrear_Click(object sender, EventArgs e)
        {
            Connection conexion = new Connection();
            string query = String.Empty;
            DateTime nuevaActualizacion = new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(1).Month, 1);

            query = String.Format("SELECT CLIENT_ID, ALIAS FROM CLIENTES_KC WHERE COUNTRY = '{0}'", conexion.getUserCountry(Session.Contents["userid"].ToString()));
            DataTable clientesExistentes = conexion.getGridDataSource(query);

            foreach (DataRow fila in clientesExistentes.Rows)
            {
                if (numCodigo.Text.Equals(fila["CLIENT_ID"].ToString()))
                {
                    mensajes.Text = String.Format("<script languaje='javascript'>" +
                                                    "alert('Codigo de socio ya existe.');" +
                                              "</script>");
                    return;
                }

                if (txtAlias.Text.ToUpper().Equals(fila["ALIAS"].ToString().ToUpper()))
                {
                    mensajes.Text = String.Format("<script languaje='javascript'>" +
                                                    "alert('Alias ya fue asignado a otro socio comercial.');" +
                                              "</script>");
                    return;
                }
            }

            query = String.Format("INSERT INTO CLIENTES_KC "
                + "(CLIENT_ID, SUBSIDIARY_ID, CLIENT_NAME, ADDRESS, TELEPHONE, COUNTRY, ALIAS, FILE_NUMBER, DIRECT_CUSTOMER, BUDGET_TP, INVERSION, INVERSION_FLOTANTE, NEXT_UPDATE) "
                + "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}')",
                numCodigo.Text, numCodigo.Text, txtNombre.Text, txtDireccion.Text, txtTelefono.Text, conexion.getUserCountry(Session.Contents["userid"].ToString()),
                txtAlias.Text.ToUpper(), 0, cmbClienteDirecto.SelectedValue, numPresupuesto.Text, 0, 0, nuevaActualizacion.ToString("yyyMMdd"));

            if (conexion.Actualizar(query))
            {
                mensajes.Text = String.Format("<script languaje='javascript'>" +
                                                    "alert('Socio comercial creado con exito.');" +
                                                    "window.location.href = '../Default.aspx';" +
                                              "</script>");
            }
            else
            {
                mensajes.Text = String.Format("<script languaje='javascript'>" +
                                                    "alert('Error de conexion intentelo mas tarde.');" +
                                              "</script>");
            }

        }

        #region Eventos de custom validator
        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = (args.Value.Length >= 7);
        }

        protected void CustomValidator3_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = (Convert.ToDouble(args.Value) >= 100);
        }
        #endregion
    }
}