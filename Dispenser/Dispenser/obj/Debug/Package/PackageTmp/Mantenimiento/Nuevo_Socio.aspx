<%@ Page Title="" Language="C#" MasterPageFile="~/Mantenimiento/Mantenimiento.Master" AutoEventWireup="true" CodeBehind="Nuevo_Socio.aspx.cs" Inherits="Dispenser.Mantenimiento.Nuevo_Socio" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/jscript" src="../Scripts/Advertencia.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
    
    <div class="centrar">
        <fieldset>
            <legend>Agregar Nuevo Socio Comercial</legend>
            <table>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblCodigoSap">Codigo SAP:</asp:Label>
                    </td>
                    <td>
                        <telerik:RadNumericTextBox ID="numCodigo" runat="server" Width="150px" 
                            Skin="Web20" MaxLength="8" DataType="System.Int32" MinValue="0">
                            <NumberFormat AllowRounding="False" DecimalDigits="0" GroupSeparator="" />
                        </telerik:RadNumericTextBox>

                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="TodoError"
                            ErrorMessage="Codigo Requerido" ControlToValidate="numCodigo" CssClass="failureNotification">*
                        </asp:RequiredFieldValidator>

                        <asp:CustomValidator ID="CustomValidator2" runat="server" 
                            ErrorMessage="Codigo SAP incorrecto." 
                            ClientValidationFunction="longitudTelefono" ControlToValidate="numCodigo" 
                            CssClass="failureNotification" 
                            onservervalidate="CustomValidator1_ServerValidate" ValidationGroup="TodoError">*</asp:CustomValidator>
                        
                    </td>
                    <td>
                        <asp:Label ID="lblNombre" runat="server">Nombre Socio:</asp:Label>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtNombre" runat="server" Skin="Web20" Width="150px">
                        </telerik:RadTextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                            ErrorMessage="Nombre Requerido" ValidationGroup="TodoError" 
                            ControlToValidate="txtNombre" CssClass="failureNotification">*
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblTelefono" runat="server">Telefono:</asp:Label>
                    </td>
                    <td>
                        <telerik:RadMaskedTextBox ID="txtTelefono" runat="server" 
                            Mask="########" PromptChar=" " Skin="Web20" Width="150px">
                        </telerik:RadMaskedTextBox>
                        
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                            ErrorMessage="Telefono requerido." ControlToValidate="txtTelefono" 
                            CssClass="failureNotification" ValidationGroup="TodoError">*
                        </asp:RequiredFieldValidator>

                        <asp:CustomValidator ID="CustomValidator1" runat="server" 
                            ErrorMessage="Telefono incorrecto" 
                            ClientValidationFunction="longitudTelefono" ControlToValidate="txtTelefono" 
                            CssClass="failureNotification" 
                            onservervalidate="CustomValidator1_ServerValidate" ValidationGroup="TodoError">*</asp:CustomValidator>
                    </td>
                    <td>                       
                        <asp:Label ID="lblAlias" runat="server">Alias:</asp:Label>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtAlias" runat="server" Skin="Web20" Width="150px" 
                            MaxLength="10" ToolTip="El alias es una descripcion corta par el archivo\nque se envia al customer care.">
                        </telerik:RadTextBox>
                        
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                            ErrorMessage="Alias Requerido" ControlToValidate="txtAlias" 
                            CssClass="failureNotification" ValidationGroup="TodoError">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblClienteDirecto" runat="server">Cliente Directo:</asp:Label>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="cmbClienteDirecto" runat="server" Skin="Web20" 
                            Width="150px">
                            <Items>
                                <telerik:RadComboBoxItem runat="server" Text="No" Value="0" />
                                <telerik:RadComboBoxItem runat="server" Text="Si" Value="1" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td>                       
                        <asp:Label ID="lblPresupuesto" runat="server">Presupuesto:</asp:Label>
                    </td>
                    <td>
                        <telerik:RadNumericTextBox ID="numPresupuesto" runat="server" Skin="Web20" 
                            Width="150px" EmptyMessage="Presupuesto en US$" MaxLength="7" MaxValue="9999" 
                            MinValue="0">
                            <IncrementSettings InterceptArrowKeys="False" InterceptMouseWheel="False" />
                            <NumberFormat AllowRounding="False" />
                        </telerik:RadNumericTextBox>
                        
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                            ErrorMessage="Presupuesto Requerido" ControlToValidate="numPresupuesto" 
                            CssClass="failureNotification" ValidationGroup="TodoError">*
                        </asp:RequiredFieldValidator>
                        
                        <asp:CustomValidator ID="CustomValidator3" runat="server" 
                            ErrorMessage="Presupuesto demasiado bajo." ClientValidationFunction="inversion" 
                            ControlToValidate="numPresupuesto" CssClass="failureNotification" 
                            onservervalidate="CustomValidator3_ServerValidate" ValidationGroup="TodoError">*
                        </asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblDireccion" runat="server">Direccion:</asp:Label>
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtDireccion" runat="server" Width="415px" Skin="Web20">
                        </telerik:RadTextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
                            ErrorMessage="RequiredFieldValidator" ControlToValidate="txtDireccion" 
                            CssClass="failureNotification" ValidationGroup="TodoError">*
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" style="text-align:right">
                        <asp:Label ID="mensajes" runat="server"></asp:Label>
                        <asp:Button ID="btCrear" runat="server" Text="Crear" Width="100px" 
                            ValidationGroup="TodoError" onclick="btCrear_Click"/>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>

    <asp:Literal runat="server" ID="errores" />
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="TodoError" CssClass="failureNotification" 
        HeaderText="Conflictos"/>
</asp:Content>