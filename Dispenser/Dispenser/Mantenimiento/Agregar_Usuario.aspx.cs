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
    public partial class Agregar_Usuario : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Menu menu = Master.FindControl("NavigationMenu") as Menu;
                menu.Items[2].ChildItems[0].ChildItems.RemoveAt(0);
                if (!IsPostBack)
                {
                    Connection conexion = new Connection();

                    string query = String.Format("SELECT CLIENT_ID, CLIENT_NAME FROM CLIENTES_KC WHERE COUNTRY = '{0}' AND DIRECT_CUSTOMER = 0",
                        conexion.getUserCountry(Session.Contents["userid"].ToString()));

                    DataTable clientes = conexion.getGridDataSource(query);

                    RadComboBoxItem temp = new RadComboBoxItem();
                    temp.Value = "0";
                    temp.Text = "Kimberly-Clark";

                    temp.DataBind();
                    cmbUbicacion.Items.Add(temp);

                    foreach (DataRow fila in clientes.Rows)
                    {
                        RadComboBoxItem item = new RadComboBoxItem();
                        item.Text = fila["CLIENT_ID"].ToString() + " | " + fila["CLIENT_NAME"].ToString();
                        item.Value = fila["CLIENT_ID"].ToString();

                        item.DataBind();
                        cmbUbicacion.Items.Add(item);
                    }

                    cargarRoles();
                }
            }
            catch (Exception error)
            {
                RadAjaxManager1.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }
        }

        protected void cmbUbicacion_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
        {

            try
            {
                txtCorreo.Enabled = true;
                txtUsuario.Enabled = true;
                btProcesar.Enabled = true;

                if (cmbUbicacion.Text.Equals(String.Empty) || cmbUbicacion.SelectedValue.Equals(String.Empty))
                {
                    txtCorreo.Enabled = false;
                    txtUsuario.Enabled = false;
                    txtCodigo.Enabled = false;
                    btProcesar.Enabled = false;
                    lblRol.Visible = false;
                    cmbRol.Visible = false;

                    txtCorreo.Text = String.Empty;
                    txtUsuario.Text = String.Empty;
                    txtCodigo.Text = String.Empty;

                    RadAjaxManager1.ResponseScripts.Add(String.Format("alert('Seleccion invalida.');"));

                    return;
                }


                if (cmbUbicacion.SelectedValue.Equals("0"))
                {
                    txtCodigo.Enabled = true;
                    lblRol.Visible = true;
                    cmbRol.Visible = true;
                    txtCodigo.Text = String.Empty;
                }
                else
                {
                    lblRol.Visible = false;
                    cmbRol.Visible = false;
                    crearUsuarioDst();
                }
            }
            catch (Exception error)
            {
                RadAjaxManager1.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }
        }

        protected void btProcesar_Click(object sender, EventArgs e)
        {
            try
            {
                Connection conexion = new Connection();
                string query = String.Empty;
                string password = String.Empty;

                Random random = new Random(DateTime.Now.Millisecond);

                for (int i = 0; i < 5; i++)
                    password += (char)random.Next(97, 123);

                for (int j = 0; j < 5; j++)
                    password += Convert.ToString(random.Next(0, 9));

                if (cmbUbicacion.SelectedValue.Equals("0"))
                {
                    query = String.Format("SELECT USER_ID FROM USUARIOS WHERE ID_COUNTRY = '{0}' AND CLIENT_ID IS NULL",
                        conexion.getUserCountry(Session.Contents["userid"].ToString()));

                    DataTable usuariosExistentes = conexion.getGridDataSource(query);
                    foreach (DataRow fila in usuariosExistentes.Rows)
                    {
                        if (fila["USER_ID"].ToString().Equals(txtCodigo.Text.ToUpper()))
                        {
                            RadAjaxManager1.ResponseScripts.Add(String.Format("alert('El codigo ya existe.');"));
                            return;
                        }
                    }

                    query = String.Format("SELECT KAM_ID FROM KAM WHERE COUNTRY = '{0}' AND KAM_ACTIVE = 1", conexion.getUserCountry(Session.Contents["userid"].ToString()));
                    usuariosExistentes = conexion.getGridDataSource(query);

                    foreach (DataRow fila in usuariosExistentes.Rows)
                    {
                        if (fila["KAM_ID"].ToString().Equals(txtCodigo.Text.ToUpper()))
                        {
                            RadAjaxManager1.ResponseScripts.Add(String.Format("alert('El codigo ya existe para un KAM.');"));
                            return;
                        }
                    }

                    query = String.Format("INSERT INTO USUARIOS (USER_ID, USER_NAME, PASSWORD, P_EXPIRATION, ID_ROL, LOGIN_ID, STATUS, ID_COUNTRY, E_MAIL) "
                    + "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', 1, '{6}', '{7}')"
                    , txtCodigo.Text, txtUsuario.Text, password, DateTime.Now.AddMonths(1).ToString("yyyMMdd"), cmbRol.SelectedValue, txtCodigo.Text,
                    conexion.getUserCountry(Session.Contents["userid"].ToString()), txtCorreo.Text);
                }
                else
                    query = String.Format("INSERT INTO USUARIOS (USER_ID, USER_NAME, PASSWORD, P_EXPIRATION, ID_ROL, LOGIN_ID, STATUS, ID_COUNTRY, E_MAIL, CLIENT_ID) "
                    + "VALUES ('{0}', '{1}', '{2}', '{3}', 'DSTADM', '{4}', 1, '{5}', '{6}', '{7}')", txtCodigo.Text, txtUsuario.Text, password,
                    DateTime.Now.AddMonths(1).ToString("yyyMMdd"), txtCodigo.Text, conexion.getUserCountry(Session.Contents["userid"].ToString()),
                    txtCorreo.Text, cmbUbicacion.SelectedValue);


                if (conexion.Actualizar(query))
                    if(conexion.notificacionNuevoUsuario(cmbUbicacion.SelectedValue))
                        RadAjaxManager1.ResponseScripts.Add(String.Format("exitoNuevoUsuario(1);"));
                    else
                        RadAjaxManager1.ResponseScripts.Add(String.Format("exitoNuevoUsuario(0);"));
                else
                    RadAjaxManager1.ResponseScripts.Add(String.Format("alert('Error inesperado de conexion, intentelo mas tarde');"));
            }
            catch (Exception error)
            {
                RadAjaxManager1.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }
        }

        private void crearUsuarioDst()
        {

            try
            {
                Connection conexion = new Connection();
                string usuario = String.Empty;
                string alias = conexion.getClientKCInfo("ALIAS", "CLIENT_ID", cmbUbicacion.SelectedValue);
                int correlativo = 0;

                DataTable resultSet = conexion.getGridDataSource(String.Format("SELECT USER_ID FROM USUARIOS WHERE CLIENT_ID = '{0}'", cmbUbicacion.SelectedValue));

                if (resultSet.Rows.Count > 0)
                    foreach (DataRow fila in resultSet.Rows)
                        correlativo = Convert.ToInt32(fila["USER_ID"].ToString().Substring(alias.Length, 1));

                usuario = alias + Convert.ToString(++correlativo);
                if (usuario.Length > 30)
                {
                    RadAjaxManager1.ResponseScripts.Add(String.Format("alert('Se ha llegado al limite de usuarios para este ditribuidor.');"));
                    return;
                }

                txtCodigo.Enabled = false;
                txtCodigo.Text = usuario;
            }
            catch (Exception error)
            {
                RadAjaxManager1.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }

        }

        private void cargarRoles()
        {
            try
            {
                Connection conexion = new Connection();

                string query = String.Format("SELECT * FROM ROLES WHERE ID_ROL <> 'DSTADM' ORDER BY ID_ROL DESC");
                DataTable resultSet = conexion.getGridDataSource(query);

                foreach (DataRow fila in resultSet.Rows)
                {
                    RadComboBoxItem item = new RadComboBoxItem();
                    item.Value = fila["ID_ROL"].ToString();
                    item.Text = fila["NOMBRE_ROL"].ToString();

                    cmbRol.Items.Add(item);
                }
            }
            catch (Exception error)
            {
                RadAjaxManager1.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }

        }

        protected void RadScriptManager1_AsyncPostBackError(object sender, AsyncPostBackErrorEventArgs e)
        {
            RadScriptManager1.AsyncPostBackErrorMessage = "Error inesperado:\n" + e.Exception.Message;
        }
    }
}