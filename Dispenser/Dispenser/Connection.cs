using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//Agregados
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Net;
using System.Net.Mail;
using System.Configuration;
using System.Text;

namespace Dispenser
{
    public class Connection
    {
        private string connection = String.Empty;

        public Connection()
        {
            //connection = ConfigurationManager.ConnectionStrings["Casa"].ConnectionString;
            connection = ConfigurationManager.ConnectionStrings["KCPConexion"].ConnectionString;
            //connection = ConfigurationManager.ConnectionStrings["ServerConexion"].ConnectionString;
        }

        public string getConnectionString()
        {
            return connection;
        }

        #region Seccion Login
        //Funcion del login
        public bool Login(string userId, string userPass)
        {
            //se crea la conexion con la viariable globla conection
            SqlConnection bridge = new SqlConnection(getConnectionString());

            string Query = "SELECT * FROM USUARIOS";
            //variables temporales para la comprobacion del usuario
            string idTemp;
            string passTemp;

            //habre la conexion y valida el usuario
            try
            {
                bridge.Open();

                SqlCommand statement = new SqlCommand(Query, bridge);
                SqlDataReader reader = statement.ExecuteReader();

                while (reader.Read())
                {
                    idTemp = Convert.ToString(reader["USER_ID"]);
                    passTemp = Convert.ToString(reader["PASSWORD"]);

                    if (userId.Equals(idTemp) && userPass.Equals(passTemp))
                    {
                        bridge.Close();
                        return true;
                    }
                }

                bridge.Close();
            }
            catch (SqlException)
            {
                return false;
            }

            return false;
        }

        //Funcion del login sobrecargada
        public bool Login(string userId)
        {
            //se crea la conexion con la viariable globla conection
            SqlConnection bridge = new SqlConnection(getConnectionString());

            string Query = "SELECT * FROM USUARIOS";

            //variables temporales para la comprobacion del usuario
            string idTemp;

            //habre la conexion y valida el usuario
            try
            {
                bridge.Open();

                SqlCommand statement = new SqlCommand(Query, bridge);
                SqlDataReader reader = statement.ExecuteReader();

                while (reader.Read())
                {
                    idTemp = Convert.ToString(reader["USER_ID"]);

                    if (userId.Equals(idTemp))
                    {
                        bridge.Close();
                        return true;
                    }
                }

                bridge.Close();
            }
            catch (SqlException)
            {
                return false;
            }

            return false;
        }

        #endregion

        #region Seccion Datos De Usuario
        //Funcion para ver el usuario
        public string getUser(string userId)
        {

            SqlConnection bridge = new SqlConnection(getConnectionString());

            string query = String.Format("SELECT USER_NAME FROM USUARIOS WHERE USER_ID = '{0}'", userId);
            string userName;

            try
            {
                bridge.Open();
                SqlCommand statement = new SqlCommand(query, bridge);
                SqlDataReader reader = statement.ExecuteReader();

                while (reader.Read())
                {
                    userName = Convert.ToString(reader["USER_NAME"]);
                    return userName;
                }

                bridge.Close();
            }
            catch (SqlException)
            {
                bridge.Close();

            }

            return "Unexpected error";

        }

        //Funcion para saber el estatus de la cuenta
        public string getStatus(string userId)
        {
            SqlConnection bridge = new SqlConnection(getConnectionString());

            string query = String.Format("SELECT STATUS FROM USUARIOS WHERE USER_ID = '{0}'", userId);
            string estatus;

            try
            {
                bridge.Open();
                SqlCommand statement = new SqlCommand(query, bridge);
                SqlDataReader reader = statement.ExecuteReader();

                while (reader.Read())
                {
                    estatus = Convert.ToString(reader["STATUS"]);
                    return estatus;
                }

                bridge.Close();
            }
            catch (SqlException)
            {
                return "Connection Error";
            }

            return "Unexpected Error";

        }

        //Funcion para obtener la fecha
        public string getPExpiration(string userId)
        {
            SqlConnection bridge = new SqlConnection(getConnectionString());

            string query = String.Format("SELECT P_EXPIRATION FROM USUARIOS WHERE USER_ID = '{0}'", userId);
            string fecha;

            try
            {
                bridge.Open();
                SqlCommand statement = new SqlCommand(query, bridge);
                SqlDataReader reader = statement.ExecuteReader();

                while (reader.Read())
                {
                    fecha = Convert.ToString(reader["P_EXPIRATION"]);
                    return fecha;
                }

                bridge.Close();
            }
            catch (SqlException)
            {
                return "Connection Error";
            }

            return "Unexpected Error";

        }

        //actualiza el estado de la cuenta del usuario
        public bool updateState(string table, string field, string userID, bool status)
        {
            SqlConnection bridge = new SqlConnection(getConnectionString());
            string estado = String.Empty;

            if (status)
                estado = "TRUE";
            else
                estado = "FALSE";

            string query = String.Format("UPDATE {0} SET {1} = '{2}' WHERE USER_ID = '{3}'", table, field, estado, userID);

            try
            {
                bridge.Open();
                SqlCommand sentencia = new SqlCommand(query, bridge);
                SqlDataReader reader = sentencia.ExecuteReader();
                bridge.Close();

                return true;
            }
            catch (SqlException)
            {
                return false;
            }
        }

        //Funcion para obtener datos de Usuarios
        public string getUsersInfo(string field, string conditionL, string conditionR)
        {
            SqlConnection bridge = new SqlConnection(getConnectionString());
            string query = String.Format("SELECT {0} FROM USUARIOS WHERE {1} = '{2}'", field, conditionL, conditionR);

            try
            {
                bridge.Open();
                SqlCommand sentencia = new SqlCommand(query, bridge);
                SqlDataReader reader = sentencia.ExecuteReader();

                while (reader.Read())
                {
                    return Convert.ToString(reader[field]);
                }

                bridge.Close();
            }
            catch (SqlException)
            {
                return "ERROR";
            }

            return "ERROR";
        }
        #endregion

        #region Seccion De Logs de la pagina

        //actualiza los ultimos campos del login log
        public bool updateLog(string query)
        {
            SqlConnection bridge = new SqlConnection(getConnectionString());

            try
            {
                bridge.Open();
                SqlCommand sentencia = new SqlCommand(query, bridge);
                SqlDataReader reader = sentencia.ExecuteReader();
                bridge.Close();

                return true;
            }
            catch (SqlException)
            {
                return false;
            }
        }

        //funcion para obtener el log id correspondiente al usuario
        public string getLogID(string userID)
        {
            SqlConnection bridge = new SqlConnection(getConnectionString());

            string query = String.Format("SELECT LOG_ID FROM LOGIN_LOG WHERE LOGIN_ID = '{0}' AND LOGOUT IS NULL", userID);

            try
            {
                bridge.Open();
                SqlCommand sentencia = new SqlCommand(query, bridge);
                SqlDataReader reader = sentencia.ExecuteReader();

                while (reader.Read())
                {
                    return Convert.ToString(reader["LOG_ID"]);
                }

                bridge.Close();
            }
            catch (SqlException)
            {
                return "ERROR";
            }

            return "ERROR";

        }

        //funcion para obtener el logid del usuario
        public string getUserLogId(string userID)
        {
            SqlConnection bridge = new SqlConnection(getConnectionString());

            string query = String.Format("SELECT LOGIN_ID FROM USUARIOS WHERE USER_ID = '{0}'", userID);

            try
            {
                bridge.Open();
                SqlCommand sentencia = new SqlCommand(query, bridge);
                SqlDataReader reader = sentencia.ExecuteReader();

                while (reader.Read())
                {
                    return Convert.ToString(reader["LOGIN_ID"]);
                }

                bridge.Close();
            }
            catch (SqlException)
            {
                return "ERROR";
            }

            return "ERROR";
        }

        //Funcion de insertar en el log 1ra vez
        public string storeLogin(string userLogID, string userName)
        {
            SqlConnection bridge = new SqlConnection(getConnectionString());
            string query = String.Format("INSERT INTO LOGIN_LOG (LOGIN_ID, USER_NAME, LOGIN) VALUES ('{0}', '{1}', GETDATE())", userLogID, userName);

            try
            {
                bridge.Open();
                SqlCommand sentencia = new SqlCommand(query, bridge);
                SqlDataReader reader = sentencia.ExecuteReader();
                bridge.Close();

                return "true";
            }
            catch (SqlException error)
            {
                return error.GetType().ToString();
            }

        }

        #endregion

        #region Seccion De Pais

        //conseguir el id del pais donde se encuentra por medio del id
        public string getUserCountry(string userId)
        {
            SqlConnection bridge = new SqlConnection(getConnectionString());
            string query = String.Format("SELECT ID_COUNTRY FROM USUARIOS WHERE USER_ID = '{0}'", userId);

            try
            {
                bridge.Open();
                SqlCommand sentencia = new SqlCommand(query, bridge);
                SqlDataReader reader = sentencia.ExecuteReader();

                while (reader.Read())
                {
                    return Convert.ToString(reader["ID_COUNTRY"]);
                }

                bridge.Close();
            }
            catch (SqlException)
            {
                return "ERROR";
            }

            return "ERROR";
        }

        //devuelve el nombre del pais
        public string getCountryInfo(string field, string conditionL, string conditionR)
        {

            SqlConnection bridge = new SqlConnection(getConnectionString());
            string query = String.Format("SELECT {0} FROM PAISES WHERE {1} = '{2}'", field, conditionL, conditionR);

            try
            {
                bridge.Open();
                SqlCommand sentencia = new SqlCommand(query, bridge);
                SqlDataReader reader = sentencia.ExecuteReader();

                while (reader.Read())
                {
                    return Convert.ToString(reader[field]);
                }

                bridge.Close();
            }
            catch (SqlException)
            {
                return "ERROR";
            }

            return "ERROR";
        }

        public string getDivTerrInfo(string field, string conditionL, string conditionR)
        {

            SqlConnection bridge = new SqlConnection(getConnectionString());
            string query = String.Format("SELECT {0} FROM DIVISION_TERRITORIAL WHERE {1} = '{2}'", field, conditionL, conditionR);

            try
            {
                bridge.Open();
                SqlCommand sentencia = new SqlCommand(query, bridge);
                SqlDataReader reader = sentencia.ExecuteReader();

                while (reader.Read())
                {
                    return Convert.ToString(reader[field]);
                }

                bridge.Close();
            }
            catch (SqlException)
            {
                return "ERROR";
            }

            return "ERROR";
        }

        #endregion

        #region Distribuidores
        //Funcion para obtener datos varios de los clientes KC
        public string getClientKCInfo(string field, string conditionL, string conditionR)
        {
            SqlConnection bridge = new SqlConnection(getConnectionString());
            string query = String.Format("SELECT {0} FROM CLIENTES_KC WHERE {1} = '{2}'", field, conditionL, conditionR);

            try
            {
                bridge.Open();
                SqlCommand sentencia = new SqlCommand(query, bridge);
                SqlDataReader reader = sentencia.ExecuteReader();

                while (reader.Read())
                {
                    return Convert.ToString(reader[field]);
                }

                bridge.Close();
            }
            catch (SqlException)
            {
                return "ERROR";
            }
            finally
            {
                bridge.Close();
            }

            return "ERROR";
        }

        public int getClientKCFile(string id)
        {
            SqlConnection bridge = new SqlConnection(getConnectionString());
            string query = String.Format("SELECT FILE_NUMBER FROM CLIENTES_KC WHERE CLIENT_ID = '{0}'", id);

            try
            {
                bridge.Open();
                SqlCommand sentencia = new SqlCommand(query, bridge);
                SqlDataReader reader = sentencia.ExecuteReader();

                while (reader.Read())
                    return Convert.ToInt32(reader["FILE_NUMBER"]);

            }
            catch (SqlException)
            {
                return -1;
            }

            return -1;
        }

        public bool updateClientKCFile(int codigo, string clientId)
        {
            SqlConnection bridge = new SqlConnection(getConnectionString());
            string query = String.Format("UPDATE CLIENTES_KC SET FILE_NUMBER = {0} WHERE CLIENT_ID = '{1}'", codigo, clientId);

            try
            {
                bridge.Open();
                SqlCommand sentencia = new SqlCommand(query, bridge);
                SqlDataReader reader = sentencia.ExecuteReader();
                bridge.Close();

                return true;
            }
            catch (SqlException)
            {
                return false;
            }
            finally
            {
                bridge.Close();
            }
        }
        #endregion

        #region Seccion Varias
        //conseguir el supportid mediante el id del pais
        public string getSupportId(string countryId)
        {
            SqlConnection bridge = new SqlConnection(getConnectionString());
            string query = String.Format("SELECT SUPPORT_ID FROM SOPORTE WHERE SUPPORT_COUNTRY = '{0}'", countryId);

            try
            {
                bridge.Open();
                SqlCommand sentencia = new SqlCommand(query, bridge);
                SqlDataReader reader = sentencia.ExecuteReader();

                while (reader.Read())
                {
                    return Convert.ToString(reader["SUPPORT_ID"]);
                }

                bridge.Close();
            }
            catch (SqlException)
            {
                return "ERROR";
            }

            return "ERROR";
        }

        //consigue la situacion por medio de la descripcion
        public string getSituationId(string situationDesc)
        {
            SqlConnection bridge = new SqlConnection(getConnectionString());
            string query = String.Format("SELECT ID_SITUATION FROM SITUACIONES WHERE SITUATION_DESC = '{0}'", situationDesc);

            try
            {
                bridge.Open();
                SqlCommand sentencia = new SqlCommand(query, bridge);
                SqlDataReader reader = sentencia.ExecuteReader();

                while (reader.Read())
                {
                    return Convert.ToString(reader["ID_SITUATION"]);
                }

                bridge.Close();
            }
            catch (SqlException)
            {
                return "ERROR";
            }

            return "ERROR";
        }

        public bool setHelp(string userId, string situationId, string comments, string support_id)
        {
            SqlConnection bridge = new SqlConnection(getConnectionString());
            string query = String.Format("INSERT INTO AYUDA (USER_ID, SITUATION_ID, COMMENTS, DATE_REQUEST, STATUS_REQUEST, SUPPORT_ID)" +
                " VALUES ('{0}', '{1}', '{2}', SYSDATETIME(), 'FALSE', '{3}')", userId, situationId, comments, support_id);

            try
            {
                bridge.Open();
                SqlCommand sentencia = new SqlCommand(query, bridge);
                SqlDataReader reader = sentencia.ExecuteReader();
                bridge.Close();

                return true;
            }
            catch (SqlException)
            {
                return false;
            }
        }

        //Funcion para obtener datos de las condiciones de pago
        public string getPaymentConditionInfo(string field, string conditionL, string conditionR)
        {
            SqlConnection bridge = new SqlConnection(getConnectionString());
            string query = String.Format("SELECT {0} FROM CONDICIONES_PAGO WHERE {1} = '{2}'", field, conditionL, conditionR);

            try
            {
                bridge.Open();
                SqlCommand sentencia = new SqlCommand(query, bridge);
                SqlDataReader reader = sentencia.ExecuteReader();

                while (reader.Read())
                {
                    return Convert.ToString(reader[field]);
                }

                bridge.Close();
            }
            catch (SqlException)
            {
                return "ERROR";
            }

            return "ERROR";
        }

        //Funcion para obtener el id del vendedor
        public string getSalesInfo(string field, string conditionL, string conditionR)
        {
            SqlConnection bridge = new SqlConnection(getConnectionString());
            string query = String.Format("SELECT {0} FROM VENDEDORES WHERE {1} = '{2}'", field, conditionL, conditionR);

            try
            {
                bridge.Open();
                SqlCommand sentencia = new SqlCommand(query, bridge);
                SqlDataReader reader = sentencia.ExecuteReader();

                while (reader.Read())
                {
                    return Convert.ToString(reader[field]);
                }

                bridge.Close();
            }
            catch (SqlException)
            {
                return "ERROR";
            }

            return "ERROR";
        }

        public string getSalesInfo(string field, string conditionL, string conditionR, string conditionL2, string conditionR2)
        {
            SqlConnection bridge = new SqlConnection(getConnectionString());
            string query = String.Format("SELECT {0} FROM VENDEDORES WHERE {1} = '{2}' AND {3} = '{4}'", field, conditionL, conditionR, conditionL2, conditionR2);

            try
            {
                bridge.Open();
                SqlCommand sentencia = new SqlCommand(query, bridge);
                SqlDataReader reader = sentencia.ExecuteReader();

                while (reader.Read())
                {
                    return Convert.ToString(reader[field]);
                }

                bridge.Close();
            }
            catch (SqlException)
            {
                return "ERROR";
            }

            return "ERROR";
        }

        public string getStatusDescripInfo(string field, string conditionL, string conditionR)
        {
            SqlConnection bridge = new SqlConnection(getConnectionString());
            string query = String.Format("SELECT {0} FROM STATUS_DESCRIP WHERE {1} = '{2}'", field, conditionL, conditionR);

            try
            {
                bridge.Open();
                SqlCommand sentencia = new SqlCommand(query, bridge);
                SqlDataReader reader = sentencia.ExecuteReader();

                while (reader.Read())
                {
                    return Convert.ToString(reader[field]);
                }

                bridge.Close();
            }
            catch (SqlException)
            {
                return "ERROR";
            }

            return "ERROR";
        }

        public string consolidarCodigo(string codigo)
        {
            char[] data = codigo.ToCharArray();
            char[] consolidado = new char[15];

            int longitud = data.Length;
            int longitudMaxima = 15;
            int resultado = longitudMaxima - longitud;
            int posicion = 0;
            int posicion2 = 0;

            codigo = String.Empty;

            for (int i = 0; i < resultado; i++)
            {
                consolidado[i] = '0';
                posicion = i;
            }

            for (int j = posicion + 1; j < longitudMaxima; j++)
            {
                consolidado[j] = data[posicion2];
                ++posicion2;
            }

            for (int k = 0; k < 15; k++)
                codigo += consolidado[k];

            return codigo;

        }

        public string getReason(string field, string conditionL, string conditionR)
        {
            SqlConnection bridge = new SqlConnection(getConnectionString());
            string query = String.Format("SELECT {0} FROM RAZONES WHERE {1} = '{2}'", field, conditionL, conditionR);

            try
            {
                bridge.Open();
                SqlCommand sentencia = new SqlCommand(query, bridge);
                SqlDataReader reader = sentencia.ExecuteReader();

                while (reader.Read())
                {
                    return Convert.ToString(reader[field]);
                }

                bridge.Close();
            }
            catch (SqlException)
            {
                return "ERROR";
            }
            finally
            {
                bridge.Close();
            }

            return "ERROR";
        }
        #endregion

        #region Solicitud de Dispensadores
        //Funcion de almacenamiento de la informacion general de la solicitud de dispensador
        public bool setDispenserGeneral(string query)
        {
            SqlConnection bridge = new SqlConnection(getConnectionString());

            try
            {
                bridge.Open();
                SqlCommand sentencia = new SqlCommand(query, bridge);
                SqlDataReader reader = sentencia.ExecuteReader();
                bridge.Close();

                return true;
            }
            catch (SqlException)
            {
                return false;
            }
            finally
            {
                bridge.Close();
            }

        }

        //Funcion para obtener el id de la solicitud
        public string getdispenserReqId(string fecha, string clientId)
        {
            SqlConnection bridge = new SqlConnection(getConnectionString());
            string query = String.Format("SELECT DR_ID FROM SOLICITUD_DISPENSADORES WHERE DATE_REQUEST = '{0}' AND CLIENT_ID = '{1}' ORDER BY DR_ID", fecha, clientId);
            string codigo = "";

            try
            {
                bridge.Open();
                SqlCommand sentencia = new SqlCommand(query, bridge);
                SqlDataReader reader = sentencia.ExecuteReader();

                while (reader.Read())
                {
                    codigo = Convert.ToString(reader["DR_ID"]);
                }

                bridge.Close();

                return codigo;
            }
            catch (SqlException)
            {
                return "ERROR";
            }

        }

        public bool setDescripcionDis(string drId, List<string> codigoDis, List<string> codigoProd, List<int> cantidadDis, List<int> cantidadProd, List<double> costoDetalle)
        {
            SqlConnection bridge = new SqlConnection(getConnectionString());
            StringBuilder insertar = new StringBuilder();

            for (int i = 0; i < codigoDis.Count; i++)
            {
                string query = String.Format("INSERT INTO DESCRIPCION_DISPENSADORES (DR_ID, DISPENSER_ID, PRODUCT_ID, DISPENSER_QUANTITY, PRODUCT_QUANTITY, STATUS_ID, INVERSION)" +
                    " VALUES ('{0}', '{1}', '{2}', {3}, {4}, 1, {5}); ", drId, codigoDis[i], codigoProd[i], cantidadDis[i], cantidadProd[i], costoDetalle[i]);

                insertar.Append(query);
            }

            try
            {
                bridge.Open();
                SqlCommand sentencia = new SqlCommand(insertar.ToString(), bridge);
                SqlDataReader reader = sentencia.ExecuteReader();
                bridge.Close();
            }
            catch (SqlException)
            {
                return false;
            }

            return true;
        }

        public DataTable getGridDataSource(string query)
        {
            SqlConnection bridge = new SqlConnection(getConnectionString());
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(query, bridge);
            DataTable myDataTable = new DataTable();

            bridge.Open();

            try
            {
                adapter.Fill(myDataTable);
            }
            catch (SqlException)
            {
            }
            finally
            {
                bridge.Close();
            }

            return myDataTable;

        }

        public string getSolicitudInfo(string field, string idSolicitud)
        {
            SqlConnection bridge = new SqlConnection(getConnectionString());
            string query = String.Format("SELECT {0} FROM SOLICITUD_DISPENSADORES WHERE DR_ID = '{1}'", field, idSolicitud);
            string valor = String.Empty;

            try
            {
                bridge.Open();
                SqlCommand sentencia = new SqlCommand(query, bridge);
                SqlDataReader reader = sentencia.ExecuteReader();

                while (reader.Read())
                {
                    valor = Convert.ToString(reader[field]);
                }

                bridge.Close();
                return valor;
            }
            catch (SqlException)
            {
                return "ERROR";
            }
        }

        public string getDetalleSol(string field, string dispensador, string producto, string idSolicitud)
        {
            SqlConnection bridge = new SqlConnection(getConnectionString());
            string query = String.Format("SELECT {0} FROM DESCRIPCION_DISPENSADORES WHERE DR_ID = '{1}' AND DISPENSER_ID = '{2}' AND PRODUCT_ID = '{3}'"
                , field, idSolicitud, dispensador, producto);
            string valor = String.Empty;

            try
            {
                bridge.Open();
                SqlCommand sentencia = new SqlCommand(query, bridge);
                SqlDataReader reader = sentencia.ExecuteReader();

                while (reader.Read())
                    valor = Convert.ToString(reader[field]);

                bridge.Close();
                return valor;
            }
            catch (SqlException)
            {
                return "ERROR";
            }
        }
        #endregion

        #region ClientesFinales

        public bool updateClientesFinales(string query)
        {

            SqlConnection bridge = new SqlConnection(getConnectionString());

            try
            {
                bridge.Open();
                SqlCommand sentencia = new SqlCommand(query, bridge);
                SqlDataReader reader = sentencia.ExecuteReader();
                bridge.Close();

                return true;
            }
            catch (SqlException)
            {
                return false;
            }
            finally
            {
                bridge.Close();
            }

        }

        //Funcion para obtener informacion del cliente final
        public string getEndClientInfo(string field, string conditionL, string conditionR, string conditionL2, string conditionR2)
        {
            SqlConnection bridge = new SqlConnection(getConnectionString());
            string query = String.Format("SELECT {0} FROM CLIENTES_FINALES WHERE {1} = '{2}' AND {3} = '{4}'", field, conditionL, conditionR, conditionL2, conditionR2);

            try
            {
                bridge.Open();
                SqlCommand sentencia = new SqlCommand(query, bridge);
                SqlDataReader reader = sentencia.ExecuteReader();

                while (reader.Read())
                {
                    return Convert.ToString(reader[field]);
                }

                bridge.Close();
            }
            catch (SqlException)
            {
                return "ERROR";
            }

            return "ERROR";
        }

        public bool setNewEndUser(string query)
        {
            SqlConnection bridge = new SqlConnection(getConnectionString());

            try
            {
                bridge.Open();
                SqlCommand sentencia = new SqlCommand(query, bridge);
                SqlDataReader reader = sentencia.ExecuteReader();
                bridge.Close();
            }
            catch (SqlException)
            {
                return false;
            }

            return true;
        }

        public bool seekEndUser(string clientId, string codigo)
        {
            SqlConnection conexion = new SqlConnection(getConnectionString());
            string query = String.Format("SELECT END_USER_ID FROM CLIENTES_FINALES WHERE CLIENT_ID = '{0}'", clientId);
            string codigoTemp;

            try
            {
                conexion.Open();
                SqlCommand statement = new SqlCommand(query, conexion);
                SqlDataReader reader = statement.ExecuteReader();

                while (reader.Read())
                {
                    codigoTemp = Convert.ToString(reader["END_USER_ID"]);

                    if (codigo.Equals(codigoTemp))
                    {
                        conexion.Close();
                        return true;
                    }
                }

            }
            catch (SqlException)
            {
                return false;
            }

            return false;
        }

        #endregion

        #region Segmentos

        //Funcion para obtener informacion de los segmentos
        public string getSegmentsInfo(string field, string conditionL, string conditionR)
        {
            SqlConnection bridge = new SqlConnection(getConnectionString());
            string query = String.Format("SELECT {0} FROM SEGMENTOS WHERE {1} = '{2}'", field, conditionL, conditionR);

            try
            {
                bridge.Open();
                SqlCommand sentencia = new SqlCommand(query, bridge);
                SqlDataReader reader = sentencia.ExecuteReader();

                while (reader.Read())
                {
                    return Convert.ToString(reader[field]);
                }

                bridge.Close();
            }
            catch (SqlException)
            {
                return "ERROR";
            }

            return "ERROR";
        }

        //Funcio para obtener la informacion de los subsegmentos
        public string getSubSegmentInfo(string field, string conditionL1, string conditionR1, string conditionL2, string conditionR2)
        {
            SqlConnection bridge = new SqlConnection(getConnectionString());
            string query = String.Format("SELECT {0} FROM SUB_SEGMENTOS WHERE {1} = '{2}' AND {3} = '{4}'", field, conditionL1, conditionR1, conditionL2, conditionR2);

            try
            {
                bridge.Open();
                SqlCommand sentencia = new SqlCommand(query, bridge);
                SqlDataReader reader = sentencia.ExecuteReader();

                while (reader.Read())
                {
                    return Convert.ToString(reader[field]);
                }

                bridge.Close();
            }
            catch (SqlException)
            {
                return "ERROR";
            }

            return "ERROR";
        }

        #endregion

        #region Query de Administrador
        public bool Actualizar(string query)
        {
            SqlConnection bridge = new SqlConnection(getConnectionString());

            try
            {
                bridge.Open();
                SqlCommand sentencia = new SqlCommand(query, bridge);
                SqlDataReader reader = sentencia.ExecuteReader();
                bridge.Close();

                return true;
            }
            catch (SqlException)
            {
                return false;
            }
            finally
            {
                bridge.Close();
            }
        }

        public string getEstadoSolicitud(string query)
        {
            SqlConnection bridge = new SqlConnection(getConnectionString());

            try
            {
                bridge.Open();
                SqlCommand sentencia = new SqlCommand(query, bridge);
                SqlDataReader reader = sentencia.ExecuteReader();

                while (reader.Read())
                {
                    return Convert.ToString(reader["STATUS_DESCRIP"]);
                }

                bridge.Close();
            }
            catch (SqlException)
            {
                return "Error";
            }
            finally
            {
                bridge.Close();
            }

            return "Error";
        }
        #endregion

        #region Correos
        public string getDispenserInfo(string field, string condicionL, string condicionR)
        {
            SqlConnection bridge = new SqlConnection(getConnectionString());
            string query = String.Format("SELECT {0} FROM DISPENSADORES WHERE {1} = '{2}'", field, condicionL, condicionR);

            try
            {
                bridge.Open();
                SqlCommand sentencia = new SqlCommand(query, bridge);
                SqlDataReader reader = sentencia.ExecuteReader();

                while (reader.Read())
                {
                    return Convert.ToString(reader[field]);
                }

                bridge.Close();
            }
            catch (SqlException)
            {
                return "ERROR";
            }

            return "ERROR";
        }

        public string getProductInfo(string field, string condicionL, string condicionR, string condicionL2, string condicionR2)
        {
            SqlConnection bridge = new SqlConnection(getConnectionString());
            string query = String.Format("SELECT {0} FROM PRODUCTOS WHERE {1} = '{2}' AND {3} = '{4}'", field, condicionL, condicionR, condicionL2, condicionR2);

            try
            {
                bridge.Open();
                SqlCommand sentencia = new SqlCommand(query, bridge);
                SqlDataReader reader = sentencia.ExecuteReader();

                while (reader.Read())
                {
                    return Convert.ToString(reader[field]);
                }

                bridge.Close();
            }
            catch (SqlException)
            {
                return "ERROR";
            }

            return "ERROR";
        }

        public DataTable getCustomerCareInfo(string idPais)
        {
            SqlConnection bridge = new SqlConnection(getConnectionString());
            string query = String.Format("SELECT USER_NAME, E_MAIL FROM USUARIOS WHERE ID_COUNTRY = '{0}' AND ID_ROL = 'KCPCCR' AND STATUS = 1", idPais);
            DataTable tabla = new DataTable();

            try
            {
                bridge.Open();
                SqlCommand sentencia = new SqlCommand(query, bridge);
                SqlDataAdapter dataAdapter = new SqlDataAdapter(sentencia);

                dataAdapter.Fill(tabla);
                bridge.Close();
                return tabla;
            }
            catch (SqlException)
            {
                return tabla;
            }
        }

        public bool enviarEmail(List<string> datosGrl, List<string> codigosDis, List<string> codigoPro, List<int> cantidadDis, List<int> cantidadPro,
            string idPais, string filePath, string clientId, bool nextMonth)
        {
            #region Cuerpo del correo
            StringBuilder cuerpo = new StringBuilder();

            cuerpo.Append("<table>");

            cuerpo.Append("<tr>");
            cuerpo.Append("<td>");
            cuerpo.Append("Distribuidor");
            cuerpo.Append("</td>");
            cuerpo.Append("<td>");
            cuerpo.Append(datosGrl[0]);
            cuerpo.Append("</td>");
            cuerpo.Append("</tr>");

            cuerpo.Append("<tr>");
            cuerpo.Append("<td>");
            cuerpo.Append("Usuario Final:");
            cuerpo.Append("</td>");
            cuerpo.Append("<td>");
            cuerpo.Append(datosGrl[1]);
            cuerpo.Append("</td>");
            cuerpo.Append("</tr>");

            cuerpo.Append("<tr>");
            cuerpo.Append("<td>");
            cuerpo.Append("Enviado por:");
            cuerpo.Append("</td>");
            cuerpo.Append("<td>");
            cuerpo.Append(datosGrl[2]);
            cuerpo.Append("</td>");
            cuerpo.Append("</tr>");

            cuerpo.Append("<tr>");
            cuerpo.Append("<td>");
            cuerpo.Append("&nbsp;");
            cuerpo.Append("</td>");
            cuerpo.Append("</tr>");

            cuerpo.Append("<tr>");
            cuerpo.Append("<td>");
            cuerpo.Append("Fecha de solicitud:");
            cuerpo.Append("</td>");
            cuerpo.Append("<td>");
            cuerpo.Append(datosGrl[3]);
            cuerpo.Append("</td>");
            cuerpo.Append("</tr>");

            cuerpo.Append("<tr>");
            cuerpo.Append("<td>");
            cuerpo.Append("Fecha Programada:");
            cuerpo.Append("</td>");
            cuerpo.Append("<td>");
            cuerpo.Append(datosGrl[4]);
            cuerpo.Append("</td>");
            cuerpo.Append("</tr>");

            cuerpo.Append("<tr>");
            cuerpo.Append("<td>");
            cuerpo.Append("Razon de la instalacion:");
            cuerpo.Append("</td>");
            cuerpo.Append("<td>");
            cuerpo.Append(datosGrl[5]);
            cuerpo.Append("</td>");
            cuerpo.Append("</tr>");

            cuerpo.Append("<tr>");
            cuerpo.Append("<td>");
            cuerpo.Append("Direccion:");
            cuerpo.Append("</td>");
            cuerpo.Append("<td>");
            cuerpo.Append(datosGrl[6]);
            cuerpo.Append("</td>");
            cuerpo.Append("</tr>");

            cuerpo.Append("<tr>");
            cuerpo.Append("<td>");
            cuerpo.Append("Contacto:");
            cuerpo.Append("</td>");
            cuerpo.Append("<td>");
            cuerpo.Append(datosGrl[7]);
            cuerpo.Append("</td>");
            cuerpo.Append("</tr>");

            cuerpo.Append("<tr>");
            cuerpo.Append("<td>");
            cuerpo.Append("Telefono:");
            cuerpo.Append("</td>");
            cuerpo.Append("<td>");
            cuerpo.Append(datosGrl[8]);
            cuerpo.Append("</td>");
            cuerpo.Append("</tr>");

            cuerpo.Append("<tr>");
            cuerpo.Append("<td>");
            cuerpo.Append("Comentarios:");
            cuerpo.Append("</td>");
            cuerpo.Append("<td>");
            cuerpo.Append(datosGrl[9]);
            cuerpo.Append("</td>");
            cuerpo.Append("</tr>");

            cuerpo.Append("<tr>");
            cuerpo.Append("<td>");
            cuerpo.Append("&nbsp;");
            cuerpo.Append("</td>");
            cuerpo.Append("</tr>");

            cuerpo.Append("<tr>");
            cuerpo.Append("<td>");
            cuerpo.Append("&nbsp;");
            cuerpo.Append("</td>");
            cuerpo.Append("</tr>");

            if (nextMonth)
            {
                cuerpo.Append("<tr>");
                cuerpo.Append("<td>");
                cuerpo.Append("<b>ESTA SOLICITUD EXCEDIO EL PRESUPUESTO DEL DISTRIBUIDOR<br />ESTARA HABILITADA HASTA EL SIGUIENTE MES</b>");
                cuerpo.Append("</td>");
                cuerpo.Append("</tr>");
            }

            cuerpo.Append("<tr>");
            cuerpo.Append("<td collspan=\"2\">");
            cuerpo.Append("***** Detalle de la solicitud *****");
            cuerpo.Append("</td>");
            cuerpo.Append("</tr>");

            cuerpo.Append("</table>");

            cuerpo.Append("<table border = 1>");

            cuerpo.Append("<tr>");
            cuerpo.Append("<th>");
            cuerpo.Append("Codigo Dispensador");
            cuerpo.Append("</th>");
            cuerpo.Append("<th>");
            cuerpo.Append("Descripcion");
            cuerpo.Append("</th>");
            cuerpo.Append("<th>");
            cuerpo.Append("Cantidad");
            cuerpo.Append("</th>");
            cuerpo.Append("<th>");
            cuerpo.Append("Producto");
            cuerpo.Append("</th>");
            cuerpo.Append("<th>");
            cuerpo.Append("Cantidad");
            cuerpo.Append("</th>");
            cuerpo.Append("</tr>");

            for (int i = 0; i < codigoPro.Count; i++)
            {
                cuerpo.Append("<tr>");
                cuerpo.Append("<td>");
                cuerpo.Append(codigosDis[i]);
                cuerpo.Append("</td>");
                cuerpo.Append("<td>");
                cuerpo.Append(getDispenserInfo("DESCRIPTION", "ID_DISPENSER", codigosDis[i]));
                cuerpo.Append("</td>");
                cuerpo.Append("<td>");
                cuerpo.Append(cantidadDis[i]);
                cuerpo.Append("</td>");
                cuerpo.Append("<td>");
                cuerpo.Append(getProductInfo("PRODUCT_DESCRIP", "PRODUCT_ID", codigoPro[i], "DISPENSER_ID", codigosDis[i]));
                cuerpo.Append("</td>");
                cuerpo.Append("<td>");
                cuerpo.Append(cantidadPro[i]);
                cuerpo.Append("</td>");
                cuerpo.Append("</tr>");
            }

            cuerpo.Append("</table>");
            #endregion

            string query = String.Format("SELECT K.KAM_NAME, K.KAM_MAIL FROM KAM AS K JOIN CUENTAS_KAM AS CK ON CK.KAM_ID = K.KAM_ID WHERE CK.CLIENT_ID = '{0}' AND K.KAM_ACTIVE = 1"
                , clientId);
            DataTable kamInfo = getGridDataSource(query);

            DataTable customerCareInfo = getCustomerCareInfo(idPais);

            if (customerCareInfo.Rows.Count == 0)
                return false;

            MailMessage mensaje = new MailMessage();

            //Se ingresa primero la copia ya que no se quiere mandar mas de una vez el correo en caso de que existieran varis customer care
            MailAddress copia = new MailAddress(kamInfo.Rows[0]["KAM_MAIL"].ToString(), kamInfo.Rows[0]["KAM_NAME"].ToString());
            mensaje.CC.Add(copia);

            foreach (DataRow fila in customerCareInfo.Rows)
            {
                try
                {
                    MailAddress de = new MailAddress("dispenser@analitica-b2b.com", "Dispenser Tracking");//Solicitud dispensadores
                    MailAddress para = new MailAddress(fila["E_MAIL"].ToString(), fila["USER_NAME"].ToString());//Administrador

                    mensaje.From = de;
                    mensaje.To.Add(para);
                    mensaje.Subject = "Nueva solicitud";
                    mensaje.BodyEncoding = System.Text.Encoding.Default;
                    mensaje.IsBodyHtml = false;
                    mensaje.Body = cuerpo.ToString();
                    mensaje.IsBodyHtml = true;

                    SmtpClient cliente = new SmtpClient("relay-hosting.secureserver.net");
                    cliente.Credentials = new System.Net.NetworkCredential("dispenser@analitica-b2b.com", "dispensador2011");
                    cliente.EnableSsl = false;

                    cliente.Send(mensaje);
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return true;
        }

        public bool enviarMailAdmin(DataTable customerCare, string filePath, string numeroSolicitud, string clienteFinal)
        {
            #region Cuerpo del correo
            StringBuilder cuerpo = new StringBuilder();
            cuerpo.Append("<h3>Correo de confirmacion</h3>");
            cuerpo.Append("<p>Solicitud aprobada con el siguiente detalle:</p>");
            cuerpo.Append("<table border = 1>");
            cuerpo.Append("<tr>");
            cuerpo.Append("<td>");
            cuerpo.Append("<b>Solicitud No.</b>");
            cuerpo.Append("</td>");
            cuerpo.Append("<td>");
            cuerpo.Append(numeroSolicitud);
            cuerpo.Append("</td>");
            cuerpo.Append("</tr>");
            cuerpo.Append("<tr>");
            cuerpo.Append("<td>");
            cuerpo.Append("<b>Cliente Final:</b>");
            cuerpo.Append("</td>");
            cuerpo.Append("<td>");
            cuerpo.Append(clienteFinal);
            cuerpo.Append("</td>");
            cuerpo.Append("</tr>");
            cuerpo.Append("</table>");
            #endregion

            foreach (DataRow fila in customerCare.Rows)
            {
                try
                {
                    MailAddress de = new MailAddress("dispenser@analitica-b2b.com", "Dispenser Tracking");//Solicitud dispensadores
                    MailAddress para = new MailAddress(fila["E_MAIL"].ToString(), fila["USER_NAME"].ToString());//Administrador
                    MailMessage mensaje = new MailMessage(de, para);
                    mensaje.Attachments.Add(new Attachment(filePath));

                    // mensaje.Bcc.Add(txtEmail.Text);
                    mensaje.Subject = "Confirmacion";
                    mensaje.BodyEncoding = System.Text.Encoding.Default;
                    mensaje.IsBodyHtml = false;
                    mensaje.Body = cuerpo.ToString();
                    mensaje.IsBodyHtml = true;

                    SmtpClient cliente = new SmtpClient("relay-hosting.secureserver.net");
                    cliente.Credentials = new System.Net.NetworkCredential("dispenser@analitica-b2b.com", "dispensador2011");
                    cliente.EnableSsl = false;

                    cliente.Send(mensaje);
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return true;
        }

        public bool enviarEmailDistribuidor(DataTable solicitante, string numeroSolicitud, string clienteFinal, DataTable detalle)
        {
            #region Desarrollo del cuerpo
            StringBuilder cuerpo = new StringBuilder();
            cuerpo.Append("<h2>Correo de actualizacion de solicitud.</h2>");
            cuerpo.Append("<p></p>");
            cuerpo.Append("<table>");
            cuerpo.Append("<tr>");
            cuerpo.Append("<td>");
            cuerpo.Append("Numero de solicitud:");
            cuerpo.Append("</td>");
            cuerpo.Append("<td>");
            cuerpo.Append(numeroSolicitud);
            cuerpo.Append("</td>");
            cuerpo.Append("</tr>");

            cuerpo.Append("<tr>");
            cuerpo.Append("<td>");
            cuerpo.Append("Cliente:");
            cuerpo.Append("</td>");
            cuerpo.Append("<td>");
            cuerpo.Append(clienteFinal);
            cuerpo.Append("</td>");
            cuerpo.Append("</tr>");
            cuerpo.Append("</table>");

            cuerpo.Append("<h3>********** Detalles **********</h3>");
            cuerpo.Append("<p></p>");
            cuerpo.Append("<table border = 1>");
            cuerpo.Append("<tr>");
            cuerpo.Append("<th>");
            cuerpo.Append("Codigo Dispensador");
            cuerpo.Append("</th>");
            cuerpo.Append("<th>");
            cuerpo.Append("Descripcion Dispensador");
            cuerpo.Append("</th>");
            cuerpo.Append("<th>");
            cuerpo.Append("Cantidad Solicitada");
            cuerpo.Append("</th>");
            cuerpo.Append("<th>");
            cuerpo.Append("Cantidad Real");
            cuerpo.Append("</th>");
            cuerpo.Append("<th>");
            cuerpo.Append("Producto");
            cuerpo.Append("</th>");
            cuerpo.Append("<th>");
            cuerpo.Append("Estado");
            cuerpo.Append("</th>");
            cuerpo.Append("<th>");
            cuerpo.Append("Comentarios");
            cuerpo.Append("</th>");
            cuerpo.Append("</tr>");

            foreach (DataRow fila in detalle.Rows)
            {
                cuerpo.Append("<tr>");
                cuerpo.Append("<td>");
                cuerpo.Append(fila["Codigo_Dispensador"].ToString());
                cuerpo.Append("</td>");
                cuerpo.Append("<td>");
                cuerpo.Append(fila["Descripcion_Dispensador"].ToString());
                cuerpo.Append("</td>");
                cuerpo.Append("<td>");
                cuerpo.Append(fila["Cantidad_Solicitada"].ToString());
                cuerpo.Append("</td>");
                cuerpo.Append("<td>");
                cuerpo.Append(fila["Cantidad_Aprobada"].ToString());
                cuerpo.Append("</td>");
                cuerpo.Append("<td>");
                cuerpo.Append(fila["Producto"].ToString());
                cuerpo.Append("</td>");
                cuerpo.Append("<td>");
                cuerpo.Append(fila["Estatus"].ToString());
                cuerpo.Append("</td>");
                cuerpo.Append("<td>");
                cuerpo.Append(fila["Comentarios"].ToString());
                cuerpo.Append("</td>");
                cuerpo.Append("</tr>");
            }

            cuerpo.Append("</table>");
            #endregion

            foreach (DataRow fila in solicitante.Rows)
            {
                try
                {
                    MailAddress de = new MailAddress("dispenser@analitica-b2b.com", "Dispenser Tracking");//solicitud dispensadores
                    MailAddress para = new MailAddress(fila["E_MAIL"].ToString(), fila["USER_NAME"].ToString());//Distribuidor
                    MailMessage mensaje = new MailMessage(de, para);

                    // mensaje.Bcc.Add(txtEmail.Text);
                    mensaje.Subject = "Actualizacion";
                    mensaje.BodyEncoding = System.Text.Encoding.Default;
                    mensaje.IsBodyHtml = false;
                    mensaje.Body = cuerpo.ToString();
                    mensaje.IsBodyHtml = true;

                    /*SmtpClient cliente = new SmtpClient("smtp.gmail.com");
                    cliente.Port = 587;
                    cliente.Credentials = new System.Net.NetworkCredential("eliazar.melendez@gmail.com", "finalmele8916");
                    cliente.EnableSsl = true;*/

                    SmtpClient cliente = new SmtpClient("relay-hosting.secureserver.net");
                    cliente.Credentials = new System.Net.NetworkCredential("dispenser@analitica-b2b.com", "dispensador2011");
                    cliente.EnableSsl = false;

                    cliente.Send(mensaje);
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return true;
        }

        public bool enviarActualizacionTP(DataTable correos, string clientID)
        {

            string query = String.Format("SELECT K.KAM_NAME, K.KAM_MAIL FROM KAM AS K JOIN CUENTAS_KAM AS CK ON CK.KAM_ID = K.KAM_ID WHERE CK.CLIENT_ID = '{0}' AND K.KAM_ACTIVE = 1"
                , clientID);

            DataTable kamInfo = getGridDataSource(query);
            
            StringBuilder cuerpo = new StringBuilder();
            cuerpo.Append("<h3>Correo de aviso!</h3>");
            cuerpo.Append("<p>Se ingreso el nuevo presupuesto para dispensadores.<br /><a href=\"http://www.analitica-b2b.com/dispenser/login.aspx\">Ingrese al portal!!</a></p>");

            MailMessage mensaje = new MailMessage();

            //Se ingresa primero la copia ya que no se quiere mandar mas de una vez el correo en caso de que existieran varis customer care
            MailAddress copia = new MailAddress(kamInfo.Rows[0]["KAM_MAIL"].ToString(), kamInfo.Rows[0]["KAM_NAME"].ToString());
            mensaje.CC.Add(copia);

            if (correos.Rows.Count == 0)
                return false;

            foreach (DataRow fila in correos.Rows)
            {
                try
                {
                    MailAddress de = new MailAddress("dispenser@analitica-b2b.com", "Dispenser Tracking");//Solicitud dispensadores
                    MailAddress para = new MailAddress(fila["E_MAIL"].ToString(), fila["USER_NAME"].ToString());//Administrador

                    //mensaje.Bcc.Add(txtEmail.Text);
                    mensaje.From = de;
                    mensaje.To.Add(para);
                    mensaje.Subject = "Actualizacion TP";
                    mensaje.BodyEncoding = System.Text.Encoding.Default;
                    mensaje.IsBodyHtml = false;
                    mensaje.Body = cuerpo.ToString();
                    mensaje.IsBodyHtml = true;

                    SmtpClient cliente = new SmtpClient("relay-hosting.secureserver.net");
                    cliente.Credentials = new System.Net.NetworkCredential("dispenser@analitica-b2b.com", "dispensador2011");
                    cliente.EnableSsl = false;

                    cliente.Send(mensaje);
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return true;

        }

        public bool notificacionNuevoUsuario(List<string> datos)
        {

            string query = String.Format("SELECT K.KAM_NAME, K.KAM_MAIL FROM KAM AS K JOIN CUENTAS_KAM AS CK ON CK.KAM_ID = K.KAM_ID WHERE CK.CLIENT_ID = '{0}' AND K.KAM_ACTIVE = 1"
                , datos[0]);

            DataTable kamInfo = getGridDataSource(query);
            StringBuilder cuerpo = new StringBuilder();

            #region Cuerpo del correo
            cuerpo.Append("<h3>Correo de aviso!</h3>");
            cuerpo.Append("<p>Se ha creado su usuario exitosamente:</p>");
            
            cuerpo.Append("<table>");
                cuerpo.Append("<tr>");
                    cuerpo.Append("<td>");
                        cuerpo.Append("Usuario:");
                    cuerpo.Append("</td>");
                    cuerpo.Append("<td>");
                        cuerpo.Append(datos[1]);
                    cuerpo.Append("</td>");
                cuerpo.Append("</tr>");

                cuerpo.Append("<tr>");
                    cuerpo.Append("<td>");
                        cuerpo.Append("Password:");
                    cuerpo.Append("</td>");
                    cuerpo.Append("<td>");
                        cuerpo.Append(datos[3]);
                    cuerpo.Append("</td>");
                cuerpo.Append("</tr>");
            cuerpo.Append("</table>");

            cuerpo.Append("<p>click <a href=\"http://www.analitica-b2b.com/dispenser/login.aspx\">aqui</a> para redireccionarte.</p>");
            #endregion

            MailMessage mensaje = new MailMessage();
            
            try
            {
                MailAddress de = new MailAddress("dispenser@analitica-b2b.com", "Dispenser Tracking");//Solicitud dispensadores
                MailAddress para = new MailAddress(datos[4], datos[2]);//Administrador
                MailAddress copia = new MailAddress(kamInfo.Rows[0]["KAM_MAIL"].ToString(), kamInfo.Rows[0]["KAM_NAME"].ToString());

                //mensaje.Bcc.Add(txtEmail.Text);
                mensaje.From = de;
                mensaje.To.Add(para);
                mensaje.CC.Add(copia);
                mensaje.Subject = "Nuevo usuario";
                mensaje.BodyEncoding = System.Text.Encoding.Default;
                mensaje.IsBodyHtml = false;
                mensaje.Body = cuerpo.ToString();
                mensaje.IsBodyHtml = true;

                SmtpClient cliente = new SmtpClient("relay-hosting.secureserver.net");
                cliente.Credentials = new System.Net.NetworkCredential("dispenser@analitica-b2b.com", "dispensador2011");
                cliente.EnableSsl = false;

                cliente.Send(mensaje);
            }
            catch (Exception)
            {
                return false;
            }
            

            return true;

        }

        public bool correoPresupuesto(string codigoDistribuidor, double porcentaje)
        {
            string query = String.Format("SELECT K.KAM_NAME, K.KAM_MAIL FROM KAM AS K JOIN CUENTAS_KAM AS CK ON CK.KAM_ID = K.KAM_ID WHERE CK.CLIENT_ID = '{0}' AND K.KAM_ACTIVE = 1"
                , codigoDistribuidor);

            DataTable kamInfo = getGridDataSource(query);
            DataTable customerCare = getCustomerCareInfo(getClientKCInfo("COUNTRY", "CLIENT_ID", codigoDistribuidor));
            string distribuidor = getClientKCInfo("CLIENT_NAME", "CLIENT_ID", codigoDistribuidor);
            
            StringBuilder cuerpo = new StringBuilder();

            #region Cuerpo del correo
            
            cuerpo.Append("<h3>Correo de aviso!</h3>");
            if(porcentaje >= 80 && porcentaje < 100)
                cuerpo.Append(String.Format("<p>Atencion presupuesto bajo para el distribuidor {0}</p>", distribuidor));
            else if(porcentaje >= 100)
                cuerpo.Append(String.Format("<p>Atencion presupuesto agotado para el distribuidor {0}</p>", distribuidor));

            #endregion

            MailMessage mensaje = new MailMessage();
            MailAddress copia = new MailAddress(kamInfo.Rows[0]["KAM_MAIL"].ToString(), kamInfo.Rows[0]["KAM_NAME"].ToString());
            mensaje.CC.Add(copia);

            foreach (DataRow fila in customerCare.Rows)
            {
                try
                {
                    MailAddress de = new MailAddress("dispenser@analitica-b2b.com", "Dispenser Tracking");//Solicitud dispensadores
                    MailAddress para = new MailAddress(fila["E_MAIL"].ToString(), fila["USER_NAME"].ToString());//Administrador

                    //mensaje.Bcc.Add(txtEmail.Text);
                    mensaje.From = de;
                    mensaje.To.Add(para);
                    mensaje.Subject = "Presupuesto";
                    mensaje.BodyEncoding = System.Text.Encoding.Default;
                    mensaje.IsBodyHtml = false;
                    mensaje.Body = cuerpo.ToString();
                    mensaje.IsBodyHtml = true;

                    SmtpClient cliente = new SmtpClient("relay-hosting.secureserver.net");
                    cliente.Credentials = new System.Net.NetworkCredential("dispenser@analitica-b2b.com", "dispensador2011");
                    cliente.EnableSsl = false;

                    cliente.Send(mensaje);
                }
                catch (Exception)
                {
                    return false;
                }
            }


            return true;
        }
        #endregion
    }
}