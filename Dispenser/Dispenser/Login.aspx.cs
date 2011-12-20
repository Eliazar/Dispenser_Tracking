using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//Agregados
using System.Web.Security;

namespace Dispenser
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void LoginUser_Authenticate(object sender, AuthenticateEventArgs e)
        {
            try
            {
                string usuario = LoginUser.UserName.ToUpper();
                string contraseña = LoginUser.Password;
                string nombreUsuario = String.Empty;
                bool estadoCuenta = false;
                string logId = String.Empty;

                DateTime hoy = DateTime.Now;
                DateTime fechaVencimiento;

                Connection conexion = new Connection();
                if (conexion.Login(usuario, contraseña))
                {
                    Session.Add("userid", usuario);
                    Session.Add("rol", conexion.getUsersInfo("ID_ROL", "USER_ID", usuario));
                    Session.Add("eventos", String.Empty);

                    nombreUsuario = conexion.getUser(usuario);
                    estadoCuenta = Convert.ToBoolean(conexion.getStatus(usuario));
                }
                else
                    return;

                if (!estadoCuenta)
                {
                    LoginUser.FailureText = "La cuenta esta inactiva imposible continuar!.";
                    return;
                }

                fechaVencimiento = Convert.ToDateTime(conexion.getPExpiration(usuario));

                if (hoy.CompareTo(fechaVencimiento) == 0 || hoy.CompareTo(fechaVencimiento) == 1)
                {
                    //Actualizar el estatus a inactivo
                    if (conexion.updateState("USUARIOS", "STATUS", usuario, false))
                    {
                        LoginUser.FailureText = String.Format("Lo sentimos {0} su cuenta ha expirado y ha sido bloqueada", usuario);
                        return;
                    }
                }

                string loginID = conexion.getUserLogId(usuario);

                if (loginID.Equals("ERROR"))
                {
                    LoginUser.FailureText = "Error de conexion con el servidor, intentelo mas tarde";
                    return;
                }

                if (!conexion.storeLogin(loginID, nombreUsuario).Equals("true"))
                {
                    LoginUser.FailureText = "Error de conexion con el servidor, intentelo mas tarde.";
                    return;
                }

                logId = conexion.getLogID(usuario);
                Session.Add("logid", Convert.ToInt32(logId));

                FormsAuthentication.RedirectFromLoginPage(nombreUsuario, LoginUser.RememberMeSet);
            }
            catch (Exception error)
            {
                mensajes.Text = String.Format("<script languaje='javascript'>" +
                                    "alert('{0}');" +
                                    "window.location.href = 'Login.aspx';" +
                                 "</script>", error.Message);
            }

        }
    }
}