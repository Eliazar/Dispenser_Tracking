<%@ Page Title="" Language="C#" MasterPageFile="~/Mantenimiento/Mantenimiento.Master" AutoEventWireup="true" CodeBehind="AgregarKAM.aspx.cs" Inherits="Dispenser.Mantenimiento.AgregarKAM" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/jscript" src="../Scripts/Advertencia.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
        
    <asp:Literal runat="server" ID="literal" />
    
    <div class="centrar">
        <fieldset>
            <legend>Ingreso de KAM</legend>

            <table>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblUsuario">Usuario KC:</asp:Label>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtUsuario" runat="server" Skin="Web20" MaxLength="6" Width="200px">
                        </telerik:RadTextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                            ErrorMessage="Usuario requerido" ControlToValidate="txtUsuario" 
                            CssClass="failureNotification" Display="Dynamic" 
                            ValidationGroup="TodoError">*
                        </asp:RequiredFieldValidator>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblNombre">Nombre:</asp:Label>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtNombre" runat="server" Skin="Web20" MaxLength="50" Width="200px">
                        </telerik:RadTextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                            ErrorMessage="Nombre requerido" ControlToValidate="txtNombre" 
                            CssClass="failureNotification" Display="Dynamic" ValidationGroup="TodoError">*
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblCorreo" runat="server">Correo:</asp:Label>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCorreo" runat="server" Skin="Web20" MaxLength="50" 
                            Width="200px" EmptyMessage="ejemplo@ejemplo.com">
                        </telerik:RadTextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                            ErrorMessage="Correo requerido" ControlToValidate="txtCorreo" 
                            CssClass="failureNotification" Display="Dynamic" ValidationGroup="TodoError">*
                        </asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                            ErrorMessage="Formato de correo no valido." ControlToValidate="txtCorreo" 
                            CssClass="failureNotification" Display="Dynamic" 
                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                            ValidationGroup="TodoError">*
                        </asp:RegularExpressionValidator>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td colspan="3">
                        <asp:CheckBox ID="chkCorreo" runat="server" Text="Habilitar envio de correo" 
                            Checked="True" />
                    </td>
                </tr>
                <tr>
                    <td colspan="7" style="text-align:center">
                        Asignacion de cuentas:
                    </td>
                </tr>
                <tr>
                    <td colspan="7" style="padding-left:18%">
                        <telerik:RadListBox ID="lbxCuentasFuente" runat="server" Skin="Web20" AllowTransfer="true"
                        TransferToID="lbxCuentasDestino" Width="230px" Height="200px" AllowTransferOnDoubleClick="True">
                        </telerik:RadListBox>
                  
                        <telerik:RadListBox ID="lbxCuentasDestino" runat="server" Skin="Web20" Width="200px" Height="200px">
                        </telerik:RadListBox>

                        <asp:Label runat="server" ID="lblMensajes"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="7" style="text-align:right">
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                            CssClass="failureNotification" HeaderText="Se encontraron conflictos:" ShowMessageBox="True" 
                            ShowSummary="False" ValidationGroup="TodoError" />
                        <asp:Button ID="btCrear" runat="server" Text="Crear" Width="120px" 
                            ValidationGroup="TodoError" onclick="btCrear_Click"/>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>

</asp:Content>