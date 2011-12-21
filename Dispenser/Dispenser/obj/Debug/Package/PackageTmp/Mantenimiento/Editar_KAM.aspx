<%@ Page Title="" Language="C#" MasterPageFile="~/Mantenimiento/Mantenimiento.Master" AutoEventWireup="true" CodeBehind="Editar_KAM.aspx.cs" Inherits="Dispenser.Mantenimiento.Editar_KAM" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript" src="../Scripts/Advertencia.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>

    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Windows7">
    </telerik:RadAjaxLoadingPanel>
    
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="cmbKAM">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    
    <div class="centrar">
        <fieldset>
            <legend>Editar KAM</legend>
            
            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblKAM" runat="server">KAM:</asp:Label>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="style3">
                            <telerik:RadComboBox ID="cmbKAM" runat="server" Skin="Web20" Width="200px" 
                                AllowCustomText="true" Filter="Contains" AutoPostBack="True" 
                                onselectedindexchanged="cmbKAM_SelectedIndexChanged" MaxHeight="100px">
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server">Nombre:</asp:Label>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="style3">
                            <telerik:RadTextBox ID="txtNombreKAM" runat="server" 
                                Skin="Web20" Width="200px">
                            </telerik:RadTextBox>

                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                ControlToValidate="txtNombreKAM" CssClass="failureNotification" 
                                ErrorMessage="Nombre requerido" ValidationGroup="TodoError">*
                            </asp:RequiredFieldValidator>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:Label ID="lblCorreo" runat="server">Correo:</asp:Label>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="style1">
                            <telerik:RadTextBox ID="txtCorreo" runat="server" 
                                EmptyMessage="ejemplo@ejemplo.com" Skin="Web20" Width="200px">
                            </telerik:RadTextBox>

                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                ControlToValidate="txtCorreo" CssClass="failureNotification" 
                                ErrorMessage="Correo requerido" ValidationGroup="TodoError">*
                            </asp:RequiredFieldValidator>
                            
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                                ControlToValidate="txtCorreo" CssClass="failureNotification" 
                                ErrorMessage="Formato de correo invalido" 
                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                                ValidationGroup="TodoError">*
                            </asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:CheckBox ID="chkHabilitarCorreo" runat="server" 
                                Text="Habilitar envio de correo." />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td colspan="3">
                            <asp:CheckBox ID="chkHabilitarKam" runat="server" Text="KAM Habilitado." />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7" style="text-align:center">
                            Asignacion de cuentas:
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7" style="padding-left:18%">
                            <telerik:RadListBox ID="lbxCuentasFuente" runat="server" Skin="Web20" AllowTransfer="True"
                            TransferToID="lbxCuentasDestino" Width="230px" Height="200px" AllowTransferOnDoubleClick="True">
                            </telerik:RadListBox>
                            
                            <telerik:RadListBox ID="lbxCuentasDestino" runat="server" Skin="Web20" 
                                Width="200px" Height="200px">
                            </telerik:RadListBox>
                            <asp:Label ID="mensajes" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7" style="text-align:right">
                            <asp:Button runat="server" ID="btEditar" ValidationGroup="TodoError" 
                                Text="Aceptar" Enabled="False" onclick="btEditar_Click"/>
                        </td>
                    </tr>
                </table>

                <asp:Literal runat="server" ID="Errores" />
                <asp:ValidationSummary ID="Validaciones" runat="server" ValidationGroup="TodoError" CssClass="failureNotification" />
            </telerik:RadAjaxPanel>

        </fieldset>
    </div>

</asp:Content>

