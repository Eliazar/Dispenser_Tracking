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
    public partial class Editar_KAM : System.Web.UI.Page
    {

        static List<string> valoresInicio;
        static List<string> valoresFinales;
        
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                Menu menu = Master.FindControl("NavigationMenu") as Menu;
                menu.Items[2].ChildItems[1].ChildItems.RemoveAt(1);

                if (!IsPostBack)
                {
                    if (Session.Contents["rol"].ToString().Equals("DSTADM"))
                        Response.Redirect("../Default.aspx");
                    
                    Connection conexion = new Connection();
                    string query = String.Format("SELECT K.KAM_ID, K.KAM_NAME FROM KAM AS K WHERE K.COUNTRY = '{0}' AND 0 = (SELECT COUNT(U.USER_ID) FROM USUARIOS AS U WHERE U.USER_ID = K.KAM_ID AND U.STATUS = 1)"
                        , conexion.getUserCountry(Session.Contents["userid"].ToString()));

                    DataTable resultset = conexion.getGridDataSource(query);
                    foreach (DataRow fila in resultset.Rows)
                    {
                        RadComboBoxItem item = new RadComboBoxItem();

                        item.Text = fila["KAM_NAME"].ToString();
                        item.Value = fila["KAM_ID"].ToString();

                        item.DataBind();
                        cmbKAM.Items.Add(item);
                    }

                    cargarCuentasDisponibles(conexion);
                }
            }
            catch (Exception error)
            {
                RadAjaxManager1.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }
        }

        protected void cmbKAM_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
        {

            try
            {
                #region Desbloqueos y más

                if (cmbKAM.Text.Equals(String.Empty) || cmbKAM.SelectedValue.Equals(String.Empty))
                {
                    RadAjaxManager1.ResponseScripts.Add(String.Format("alert('Seleccion invalida, seleccione un KAM de la lista.');"));
                    btEditar.Enabled = false;
                    return;
                }

                btEditar.Enabled = true;
                lbxCuentasFuente.Items.Clear();
                lbxCuentasDestino.Items.Clear();

                #endregion

                valoresInicio = new List<string>();

                Connection conexion = new Connection();
                string query = String.Format("SELECT KAM_NAME, KAM_MAIL, KAM_ACTIVE, RECEIVE_MAIL FROM KAM WHERE KAM_ID = '{0}'", cmbKAM.SelectedValue);
                DataTable Kaminfo = conexion.getGridDataSource(query);

                //Carga los controles con la info del KAM devuleta por el query
                txtNombreKAM.Text = Kaminfo.Rows[0]["KAM_NAME"].ToString();
                txtCorreo.Text = Kaminfo.Rows[0]["KAM_MAIL"].ToString();
                chkHabilitarKam.Checked = Convert.ToBoolean(Kaminfo.Rows[0]["KAM_ACTIVE"].ToString());
                chkHabilitarCorreo.Checked = Convert.ToBoolean(Kaminfo.Rows[0]["RECEIVE_MAIL"].ToString());

                cargarCuentasDisponibles(conexion);

                query = String.Format("SELECT CK.CLIENT_ID, CK.CLIENT_NAME FROM CLIENTES_KC AS CK JOIN CUENTAS_KAM AS C ON CK.CLIENT_ID = C.CLIENT_ID WHERE C.KAM_ID = '{0}'",
                    cmbKAM.SelectedValue);
                Kaminfo = conexion.getGridDataSource(query);

                foreach (DataRow fila in Kaminfo.Rows)
                {
                    RadListBoxItem item = new RadListBoxItem();

                    valoresInicio.Add(fila["CLIENT_ID"].ToString());
                    item.Text = fila["CLIENT_NAME"].ToString();
                    item.Value = fila["CLIENT_ID"].ToString();

                    item.DataBind();
                    lbxCuentasDestino.Items.Add(item);
                }
            }
            catch (Exception error)
            {
                RadAjaxManager1.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }
        }
    
        protected void btEditar_Click(object sender, EventArgs e)
        {

            try
            {
                if (lbxCuentasDestino.Items.Count == 0 && chkHabilitarKam.Checked)
                {
                    RadAjaxManager1.ResponseScripts.Add("alert('No se encontraron cuentas asignadas, un KAM activo debe tener por lo menos una cuenta.');");
                    return;
                }

                Connection conexion = new Connection();
                string query = String.Format("UPDATE KAM SET KAM_NAME = '{0}', KAM_MAIL = '{1}', KAM_ACTIVE = '{2}', RECEIVE_MAIL = '{3}' WHERE KAM_ID = '{4}'",
                    txtNombreKAM.Text, txtCorreo.Text, chkHabilitarKam.Checked, chkHabilitarCorreo.Checked, cmbKAM.SelectedValue);

                if (conexion.Actualizar(query))
                {
                    valoresFinales = new List<string>();
                    bool encontrado = false;

                    foreach (RadListBoxItem item in lbxCuentasDestino.Items)
                        valoresFinales.Add(item.Value);

                    for (int i = 0; i < valoresInicio.Count; i++)
                    {
                        for (int j = 0; j < valoresFinales.Count; j++)
                        {
                            if (valoresInicio[i].Equals(valoresFinales[j]))
                            {
                                valoresFinales.RemoveAt(j);
                                encontrado = true;
                            }
                        }

                        if (!encontrado)
                        {
                            query = String.Format("DELETE FROM CUENTAS_KAM WHERE KAM_ID = '{0}' AND CLIENT_ID = '{1}'", cmbKAM.SelectedValue, valoresInicio[i]);
                            conexion.Actualizar(query);
                        }

                        encontrado = false;
                    }

                    for (int i = 0; i < valoresFinales.Count; i++)
                    {
                        query = String.Format("INSERT INTO CUENTAS_KAM (KAM_ID, CLIENT_ID) VALUES ('{0}', '{1}')", cmbKAM.SelectedValue, valoresFinales[i]);
                        conexion.Actualizar(query);
                    }

                    if (!chkHabilitarKam.Checked)
                    {
                        foreach (RadListBoxItem item in lbxCuentasDestino.Items)
                        {
                            query = String.Format("DELETE FROM CUENTAS_KAM WHERE KAM_ID = '{0}' AND CLIENT_ID = '{1}'", cmbKAM.SelectedValue, item.Value);
                            conexion.Actualizar(query);
                        }
                    }

                    RadAjaxManager1.ResponseScripts.Add("editarKam();");
                }
                else
                {
                    RadAjaxManager1.ResponseScripts.Add("alert('Error inesperado de conexion intentelo de nuevo.');");
                    return;
                }
            }
            catch (Exception error)
            {
                RadAjaxManager1.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }
        }

        #region Funciones
        private void cargarCuentasDisponibles(Connection conexion)
        {
            try
            {
                string query = String.Format("select ckc.CLIENT_ID, ckc.CLIENT_NAME from CLIENTES_KC as ckc where ckc.COUNTRY = '{0}' and ckc.DIRECT_CUSTOMER = 0 and 0 = (select COUNT(ck.KAM_ID) from CUENTAS_KAM as ck where ck.CLIENT_ID = ckc.CLIENT_ID)",
                        conexion.getUserCountry(Session.Contents["userid"].ToString()));

                DataTable resultset = conexion.getGridDataSource(query);
                foreach (DataRow fila in resultset.Rows)
                {
                    RadListBoxItem item = new RadListBoxItem();

                    item.Text = fila["CLIENT_NAME"].ToString();
                    item.Value = fila["CLIENT_ID"].ToString();

                    item.DataBind();
                    lbxCuentasFuente.Items.Add(item);
                }
            }
            catch (Exception error)
            {
                RadAjaxManager1.ResponseScripts.Add(String.Format("errorEnvio('{0}');", error.Message));
            }
        }
        #endregion
    }
}