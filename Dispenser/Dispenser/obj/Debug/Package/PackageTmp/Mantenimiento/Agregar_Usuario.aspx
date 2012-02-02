<%@ Page Title="" Language="C#" MasterPageFile="~/Mantenimiento/Mantenimiento.Master" AutoEventWireup="true" CodeBehind="Agregar_Usuario.aspx.cs" Inherits="Dispenser.Mantenimiento.Agregar_Usuario" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript" src="../Scripts/Advertencia.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server" 
        onasyncpostbackerror="RadScriptManager1_AsyncPostBackError">
    </telerik:RadScriptManager>
    
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" 
        Skin="Windows7">
    </telerik:RadAjaxLoadingPanel>
    
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="cmbUbicacion">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    
    <div class="centrar">
        <fieldset>
            <legend>Agregar nuevos usuarios</legend>
            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="200px" 
                Width="600px" HorizontalAlign="NotSet" 
                LoadingPanelID="RadAjaxLoadingPanel1">
                <table>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblUbicacion">Usuario de:</asp:Label>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <telerik:RadComboBox ID="cmbUbicacion" runat="server" Skin="Web20" 
                                AllowCustomText="True" AutoPostBack="True" Filter="Contains" Width="200px" 
                                onselectedindexchanged="cmbUbicacion_SelectedIndexChanged" 
                                MaxHeight="100px">
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblCodigo" runat="server">Usuario:</asp:Label>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtCodigo" runat="server" Skin="Web20" Width="200px" 
                                Enabled="False" MaxLength="30">
                            </telerik:RadTextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                ErrorMessage="Codigo Mandatorio" CssClass="failureNotification" 
                                ValidationGroup="TodoError" ControlToValidate="txtCodigo">*</asp:RequiredFieldValidator>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:Label ID="lblUsuario" runat="server">Nombre:</asp:Label>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtUsuario" runat="server" Skin="Web20" Width="200px" 
                                Enabled="False" MaxLength="50">
                            </telerik:RadTextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                ErrorMessage="Usuario Mandatorio" CssClass="failureNotification" 
                                ValidationGroup="TodoError" ControlToValidate="txtUsuario">*</asp:RequiredFieldValidator>
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
                            <telerik:RadTextBox ID="txtCorreo" runat="server" Skin="Web20" Width="200px" 
                                EmptyMessage="ejemplo@ejemplo.com" Enabled="False" MaxLength="50">
                            </telerik:RadTextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                                ErrorMessage="Correo Mandatorio" CssClass="failureNotification" 
                                ValidationGroup="TodoError" ControlToValidate="txtCorreo">*</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Formato de correo no valido"
                                ControlToValidate="txtCorreo" ValidationGroup="TodoError" 
                                CssClass="failureNotification" 
                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">*</asp:RegularExpressionValidator>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblRol" Visible="False">Rol:</asp:Label>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <telerik:RadComboBox ID="cmbRol" runat="server" Skin="Web20" Width="200px" 
                                Visible="False">
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7" style="text-align: right">
                            <asp:Button ID="btProcesar" runat="server" Text="Procesar" Enabled="False" 
                                ValidationGroup="TodoError" onclick="btProcesar_Click"/>
                        </td>
                    </tr>
                </table>

                <asp:Literal runat="server" id="Errores" />
                <asp:ValidationSummary ID="Validaciones" runat="server" ValidationGroup="TodoError" CssClass="failureNotification"/>
            </telerik:RadAjaxPanel>
        </fieldset>
    </div>

</asp:Content>