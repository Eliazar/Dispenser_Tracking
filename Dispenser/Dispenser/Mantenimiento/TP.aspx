<%@ Page Title="" Language="C#" MasterPageFile="~/Mantenimiento/Mantenimiento.Master" AutoEventWireup="true" CodeBehind="TP.aspx.cs" Inherits="Dispenser.Mantenimiento.TP" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript" src="../Scripts/Advertencia.js"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>

    <div class="centrar">
        <fieldset>
            <legend>Ingreso de presupuesto mensual</legend>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="Distribuidor" runat="server">Socio Comercial:</asp:Label>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <telerik:RadComboBox ID="cmbSocioComercial" runat="server" Skin="Web20" Width="300px"
                            AllowCustomText="True" Filter="Contains" MaxHeight="100px">
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                            ErrorMessage="No ha seleccionado un socio comercial" ValidationGroup="TodoError"
                            ControlToValidate="cmbSocioComercial" ForeColor="#FF3300">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblPresupuesto">Presupuesto este mes en $:</asp:Label>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <telerik:RadNumericTextBox ID="nmcPresupuesto" runat="server" MaxValue="9999.99" 
                            MinValue="0" Skin="Web20" Width="300px" MaxLength="7">
                        </telerik:RadNumericTextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                            ErrorMessage="No ha ingresado la inversion correpondiente al mes." ValidationGroup="TodoError"
                            ControlToValidate="nmcPresupuesto" ForeColor="#FF3300">*</asp:RequiredFieldValidator>
                        
                        <asp:CustomValidator ID="CustomValidator1" runat="server" 
                            ErrorMessage="Presupuesto demasiado bajo." CssClass="failureNotification" 
                            ClientValidationFunction="inversion" ControlToValidate="nmcPresupuesto" 
                            ValidationGroup="TodoError">*
                        </asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="mensajes" runat="server"></asp:Label>
                    </td>
                    <td  style="text-align:right">
                        <asp:Button runat="server" ID="btProcesar" Text="Procesar" ValidationGroup="TodoError"
                            onclick="btProcesar_Click"/>
                    </td>
                </tr>
            </table>

            <asp:Literal runat="server" ID="Errores" />
            <asp:ValidationSummary ID="Validaciones" runat="server" ValidationGroup="TodoError" CssClass="failureNotification"/>
        </fieldset>
    </div>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
    </telerik:RadAjaxManager>
</asp:Content>

