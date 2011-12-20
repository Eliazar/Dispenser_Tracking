<%@ Page Title="" Language="C#" MasterPageFile="~/Mantenimiento/Mantenimiento.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="Dispenser.Mantenimiento.ChangePassword" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>

    <div class="centrar">

        <asp:Literal ID="Errores" runat="server" />
        <asp:ValidationSummary ID="Validaciones" runat="server" ValidationGroup="TodoError" CssClass="failureNotification"/>

        <fieldset>
            <legend>Cambio de contraseña</legend>

            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblContraseñaActual" runat="server">Contraseña Actual:</asp:Label>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtContraseñaActual" runat="server" TextMode="Password" 
                            Skin="Web20" Width="200px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Contraseña Requerida"
                            CssClass="failureNotification" ControlToValidate="txtContraseñaActual" ValidationGroup="TodoError">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblNuevaContraseña">Nueva Contraseña:</asp:Label>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtNuevaContraseña" runat="server" TextMode="Password" Skin="Web20" Width="200px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Nueva contraseña requerida"
                            CssClass="failureNotification" ControlToValidate="txtNuevaContraseña" ValidationGroup="TodoError">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblRepetirContraseña">Confirmar Contraseña:</asp:Label>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtConfirmarContraseña" runat="server" Skin="Web20" TextMode="Password" Width="200px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Confirmacion de contraseña requerida"
                            CssClass="failureNotification" ControlToValidate="txtConfirmarContraseña" ValidationGroup="TodoError">*</asp:RequiredFieldValidator>
                        
                        <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Las contraseñas no coinciden." 
                            ControlToCompare="txtNuevaContraseña" 
                            ControlToValidate="txtConfirmarContraseña" CssClass="failureNotification" 
                            ValidationGroup="TodoError">*</asp:CompareValidator>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right" colspan="3">
                        <asp:Button runat="server" ID="btAplicar" Text="Aplicar" 
                            ValidationGroup="TodoError" onclick="btAplicar_Click"/>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="mensajes"></asp:Label>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
</asp:Content>

