<%@ Page Title="" Language="C#" MasterPageFile="~/Mantenimiento/Mantenimiento.Master" AutoEventWireup="true" CodeBehind="Editar_Vendedor.aspx.cs" Inherits="Dispenser.Mantenimiento.Editar_Vendedor" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript" src="../Scripts/Advertencia.js"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>

    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Web20">
    </telerik:RadAjaxLoadingPanel>

    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="cmbCodigo">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
        
    <div class="centrar">
        <fieldset>
            <legend>Editar Vendedor</legend>
            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblCodigo" runat="server">Codigo/Nombre Vendedor:</asp:Label>
                        </td>
                        <td>
                        
                            <telerik:RadComboBox ID="cmbCodigo" runat="server" Skin="Web20" Width="150px" 
                                AllowCustomText="True" AutoPostBack="True" Filter="Contains" 
                                onselectedindexchanged="cmbCodigo_SelectedIndexChanged">
                            </telerik:RadComboBox>

                        </td>
                        <td>
                            <asp:Label ID="lblNombre" runat="server">Nombre Vendedor:</asp:Label>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtNombre" runat="server" Skin="Web20" Width="130px" 
                                MaxLength="50">
                            </telerik:RadTextBox>
                        
                            <asp:RequiredFieldValidator ID="rfvNombre" runat="server" 
                                ErrorMessage="Nombre requerido." ControlToValidate="txtNombre" 
                                CssClass="failureNotification" ValidationGroup="TodoError">*
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="text-align:right">
                            <asp:Button ID="btEditar" runat="server" Text="Editar" Width="100px" 
                                ValidationGroup="TodoError" Enabled="False" onclick="btEditar_Click"/>
                        </td>
                    </tr>
                </table>
            </telerik:RadAjaxPanel>
        </fieldset>

        <asp:Literal runat="server" ID="errores" />
        <asp:ValidationSummary ID="vsErrores" runat="server" 
            CssClass="failureNotification" HeaderText="Conflictos" 
            ValidationGroup="TodoError" />

    </div>
</asp:Content>