<%@ Page Title="" Language="C#" MasterPageFile="~/Mantenimiento/Mantenimiento.Master" AutoEventWireup="true" CodeBehind="Agregar_Vendedor.aspx.cs" Inherits="Dispenser.Mantenimiento.Agregar_Vendedor" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript" src="../Scripts/Advertencia.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>

    <asp:Literal runat="server" ID="literal" />
    
    <div class="centrar">
        <fieldset>
            <legend>Ingreso Vendedor</legend>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblCodigo" runat="server">Codigo Vendedor:</asp:Label>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCodigo" runat="server" Skin="Web20" Width="150px" 
                            MaxLength="50">
                        </telerik:RadTextBox>
                        
                        <asp:RequiredFieldValidator ID="rfvCodigo" runat="server" 
                            ErrorMessage="Codigo requerido." ControlToValidate="txtCodigo" 
                            CssClass="failureNotification" ValidationGroup="TodoError">*
                        </asp:RequiredFieldValidator>
                    </td>
                    <td>
                        <asp:Label ID="lblNombre" runat="server">Nombre Vendedor:</asp:Label>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtNombre" runat="server" Skin="Web20" Width="150px" 
                            MaxLength="50">
                        </telerik:RadTextBox>
                        
                        <asp:RequiredFieldValidator ID="rfvNombre" runat="server" 
                            ErrorMessage="Nombre requerido." ControlToValidate="txtNombre" 
                            CssClass="failureNotification" ValidationGroup="TodoError">*
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblMensajes" runat="server" />
                    </td>
                    <td colspan="3" style="text-align:right">
                        <asp:Button ID="btGuardar" runat="server" Text="Crear" Width="100px" 
                            onclick="btGuardar_Click" ValidationGroup="TodoError"/>
                    </td>
                </tr>
            </table>
        </fieldset>

        <asp:Literal runat="server" ID="errores" />
        <asp:ValidationSummary ID="vsErrores" runat="server" 
            CssClass="failureNotification" HeaderText="Conflictos" 
            ValidationGroup="TodoError" />

    </div>

</asp:Content>