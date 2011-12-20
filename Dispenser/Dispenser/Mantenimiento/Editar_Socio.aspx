<%@ Page Title="" Language="C#" MasterPageFile="~/Mantenimiento/Mantenimiento.Master" AutoEventWireup="true" CodeBehind="Editar_Socio.aspx.cs" Inherits="Dispenser.Mantenimiento.Editar_Socio" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript" src="../Scripts/Advertencia.js">
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
    
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Web20">
    </telerik:RadAjaxLoadingPanel>
    
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="cmbCodigoSAP">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <div class="centrar">
        <fieldset>
            <legend>Editar Socio Comercial</legend>
            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblCodigoSAP" runat="server">Cliente:</asp:Label>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="cmbCodigoSAP" runat="server" Skin="Web20" 
                                AllowCustomText="True" Filter="Contains" Width="150px" AutoPostBack="true" 
                                MaxHeight="100px" 
                                onselectedindexchanged="cmbCodigoSAP_SelectedIndexChanged">
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblNombre" runat="server">Nombre Socio:</asp:Label>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtNombreSocio" runat="server" Skin="Web20" Width="150px">
                            </telerik:RadTextBox>
                            
                            <asp:RequiredFieldValidator ID="rfvNombreSocio" runat="server" 
                                ErrorMessage="Nombre requerido." ControlToValidate="txtNombreSocio" 
                                CssClass="failureNotification" ValidationGroup="TodoError">*
                            </asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:Label ID="lblTelefono" runat="server">Telefono Socio:</asp:Label>
                        </td>
                        <td>
                            <telerik:RadMaskedTextBox ID="txtTelefono" runat="server" Skin="Web20" 
                                Mask="####-####" Width="150px" PromptChar="" >
                            </telerik:RadMaskedTextBox>
                            
                            <asp:RequiredFieldValidator ID="rfvTelefono" runat="server" 
                                ErrorMessage="Telefono requerido." ControlToValidate="txtTelefono" 
                                CssClass="failureNotification" ValidationGroup="TodoError">*
                            </asp:RequiredFieldValidator>
                            
                            <asp:CustomValidator ID="cvTelefono" runat="server" 
                                ErrorMessage="Formato de telefono invalido." 
                                ClientValidationFunction="longitudTelefono" ControlToValidate="txtTelefono" 
                                CssClass="failureNotification" ValidationGroup="TodoError">*
                            </asp:CustomValidator>
                            
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblAlias" runat="server">Alias:</asp:Label>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtAlias" runat="server" Skin="Web20" Width="150px" MaxLength="10"
                                ToolTip="El alias es una descripcion corta par el archivo\nque se envia al customer care.">
                            </telerik:RadTextBox>

                            <asp:RequiredFieldValidator ID="rfvAlias" runat="server" 
                                ErrorMessage="Alias requerido." ControlToValidate="txtAlias" 
                                CssClass="failureNotification" ValidationGroup="TodoError">*
                            </asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:Label ID="lblClienteDirecto" runat="server">Cliente Directo:</asp:Label>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="cmbClienteDirecto" runat="server" Skin="Web20" Width="150px">
                                <Items>
                                    <telerik:RadComboBoxItem runat="server" Text="No" Value="FALSE" />
                                    <telerik:RadComboBoxItem runat="server" Text="Sí" Value="TRUE" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblDireccion" runat="server">Direccion:</asp:Label>
                        </td>
                        <td colspan="3">
                            <telerik:RadTextBox ID="txtDireccion" runat="server" Skin="Web20" Width="415px">
                            </telerik:RadTextBox>
                            
                            <asp:RequiredFieldValidator ID="rfvDireccion" runat="server" 
                                ErrorMessage="Direccion requerida." ControlToValidate="txtDireccion" 
                                CssClass="failureNotification" ValidationGroup="TodoError">*
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="text-align:right">
                            <asp:Button ID="btGuardar" runat="server" Text="Guardar Cambios" Enabled="false"
                                ValidationGroup="TodoError" onclick="btGuardar_Click" />
                        </td>
                    </tr>
                </table>
            </telerik:RadAjaxPanel>
        </fieldset>

        <asp:Literal runat="server" ID="Errores"></asp:Literal>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="TodoError" HeaderText="Conflictos" CssClass="failureNotification" />
    </div>

</asp:Content>