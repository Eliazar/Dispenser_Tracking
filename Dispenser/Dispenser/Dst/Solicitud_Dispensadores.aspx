<%@ Page Title="" Language="C#" MasterPageFile="~/Dst/Clientes.Master" AutoEventWireup="true" CodeBehind="Solicitud_Dispensadores.aspx.cs" Inherits="Dispenser.Dst.Solicitud_Dispensadores" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <script type="text/javascript" src="../Scripts/Advertencia.js">
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server"> 
    
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server" 
        LoadScriptsBeforeUI="False">
    </telerik:RadScriptManager>

    <telerik:RadWindowManager ID="RadWindowManager1" runat="server" Skin="Web20">
    </telerik:RadWindowManager>
    
    <asp:SqlDataSource ID="sqlCiudad" runat="server"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlMotivos" runat="server"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlClientes" runat="server"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlVendedores" runat="server"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlCondicionesPago" runat="server"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlSegmento" runat="server"></asp:SqlDataSource>
    
    <telerik:RadAjaxLoadingPanel runat="server" ID="Cargar" Skin="Web20">
    </telerik:RadAjaxLoadingPanel>

    <telerik:RadAjaxManager ID="radajaxmanager" runat="server" 
        DefaultLoadingPanelID="Cargar">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="cmbMotivos">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="main" LoadingPanelID="Cargar" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="cmbCodigoClienteFinal">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="main" LoadingPanelID="Cargar" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="cmbDepartamento">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="cmbCiudad" LoadingPanelID="Cargar" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="cmbSegmento">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="cmbSubSegmento" LoadingPanelID="Cargar" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="btEnviar">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="main" LoadingPanelID="Cargar" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="txtCodigoClienteFinal">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="main" LoadingPanelID="Cargar" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="cmbNombreComercial">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="main" LoadingPanelID="Cargar" />
                </UpdatedControls>
            </telerik:AjaxSetting>

        </AjaxSettings>
    </telerik:RadAjaxManager>
    
    <asp:Panel ID="main" runat="server">
        <div class="centrar">
            <fieldset>
                <legend>Motivos</legend>
                <asp:Panel ID="header" runat="server">
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lblMotivo">Motivo de<br />Instalacion:</asp:Label>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="cmbMotivos" runat="server" Width="200px" 
                                    Skin="Web20" 
                                    LoadingMessage="Cargando..." DataSourceID="sqlMotivos" 
                                    onitemdatabound="cmbMotivos_ItemDataBound">
                                    
                                </telerik:RadComboBox>
                            </td>
                            <td>
                                <asp:Label ID="lblFechaRequerida" runat="server">Fecha de<br />Servicio:</asp:Label>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" 
                                    ControlToValidate="dpFechaSolicitada" CssClass="failureNotification" 
                                    ErrorMessage="Fecha de servicio requerida." ValidationGroup="TodoError" 
                                    Display="Dynamic">*</asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <telerik:RadDatePicker ID="dpFechaSolicitada" runat="server" Culture="es-HN" 
                                    Skin="Web20" Width="200px" CssClass="requerido">
                                    <Calendar UseColumnHeadersAsSelectors="False" 
                                        UseRowHeadersAsSelectors="False" ViewSelectorText="x" Skin="Web20">
                                    </Calendar>
                                    <DateInput DateFormat="dd/MM/yyyy" DisplayDateFormat="dd/MM/yyyy">
                                    </DateInput>
                                    <DatePopupButton HoverImageUrl="" ImageUrl="" />
                                </telerik:RadDatePicker>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblComentario" runat="server">Comentarios:</asp:Label>
                            </td>
                            <td colspan="4">

                                <telerik:RadTextBox ID="txtComentarios" runat="server" Skin="Web20" 
                                    TextMode="MultiLine" Width="100%">
                                </telerik:RadTextBox>

                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </fieldset>

            <div class="centrar">
                <div style="padding-left: 20%">
                    <asp:TextBox ID="txtMuestra" runat="server" CssClass="requerido" 
                        Enabled="False" Font-Size="X-Small" Width="100px">Campo Requerido</asp:TextBox>
                </div>
            </div>
                                                               
            <fieldset>
                <legend>Informacion Especifica Del Cliente</legend>
                <asp:Panel ID="infoCliente" runat="server">
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:Label ID="lblCodigoCliente" runat="server">Nombre Cliente:</asp:Label>
                            </td>
                            <td colspan="3">
                                                        
                                <telerik:RadComboBox ID="cmbNombreComercial" Runat="server" 
                                    AllowCustomText="True" AutoPostBack="True" Filter="Contains" 
                                    onselectedindexchanged="cmbNombreComercial_SelectedIndexChanged" Skin="Web20" 
                                    Width="100%" DataSourceID="sqlClientes" EnableLoadOnDemand="True" 
                                    Height="190px" HighlightTemplatedItems="True" 
                                    onitemdatabound="cmbNombreComercial_ItemDataBound" 
                                    onitemsrequested="cmbNombreComercial_ItemsRequested" Sort="Ascending" 
                                    EnableVirtualScrolling="True" ShowMoreResultsBox="True">
                                    <HeaderTemplate>
                                        <ul>
                                            <li class="col1"><b>Codigo</b></li>
                                            <li class="col2"><b>Cliente</b></li>
                                        </ul>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <ul>
                                            <li class="col1"><%# DataBinder.Eval(Container.DataItem, "END_USER_ID")%></li>
                                            <li class="col2"><%# DataBinder.Eval(Container.DataItem, "TRADE_NAME")%></li>
                                        </ul>
                                    </ItemTemplate>
                                </telerik:RadComboBox>
                                                        
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lblCedulaJuridica">Cedula Juridica:</asp:Label>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                    ErrorMessage="Cedula juridica requerida" ControlToValidate="txtCedulaJuridica" 
                                    CssClass="failureNotification" ValidationGroup="TodoError" 
                                    Display="Dynamic">*
                                </asp:RequiredFieldValidator>
                                
                                
                            </td>
                            <td>
                                <asp:TextBox ID="txtCedulaJuridica" runat="server" CssClass="requerido" 
                                    Enabled="False" MaxLength="15" ontextchanged="txtCedulaJuridica_TextChanged1" 
                                    Width="190px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblVendedor">Vendedor:</asp:Label>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                    ControlToValidate="cmbVendedor" CssClass="failureNotification" 
                                    ValidationGroup="TodoError" ErrorMessage="Vendedor Requerido" 
                                    Display="Dynamic">*</asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="cmbVendedor" runat="server" Enabled="False" 
                                    onselectedindexchanged="cmbVendedor_SelectedIndexChanged" Skin="Web20" 
                                    Width="190px" AllowCustomText="True" Filter="Contains" 
                                    DataSourceID="sqlVendedores" onitemdatabound="cmbVendedor_ItemDataBound" 
                                    CssClass="requerido">
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lblNombreComercial">Nombre Comercial:</asp:Label>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                                    ErrorMessage="Nombre comercial requerido" ControlToValidate="txtNombreComercial" 
                                    CssClass="failureNotification" ValidationGroup="TodoError" 
                                    Display="Dynamic">*
                                </asp:RequiredFieldValidator>
                            </td>
                            <td>

                                <asp:TextBox ID="txtNombreComercial" runat="server" CssClass="requerido" 
                                    Enabled="False" MaxLength="60" ontextchanged="txtNombreComercial_TextChanged2" 
                                    Width="190px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblSubSegmento">Segmento:</asp:Label>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                                    ErrorMessage="Segmento requerido" ControlToValidate="cmbSubSegmento" 
                                    CssClass="failureNotification" ValidationGroup="TodoError" 
                                    Display="Dynamic">*
                                </asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="cmbSubSegmento" runat="server" Enabled="False" 
                                    onselectedindexchanged="cmbSubSegmento_SelectedIndexChanged" Skin="Web20" 
                                    Width="190px" AllowCustomText="True" Filter="Contains" 
                                    DataSourceID="sqlSegmento" EnableLoadOnDemand="True" Height="100px" 
                                    onitemdatabound="cmbSubSegmento_ItemDataBound" CssClass="requerido">
                                </telerik:RadComboBox>

                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lblRazonSocial">Razon Social:</asp:Label>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                                    ErrorMessage="Razon social requerida" ControlToValidate="txtRazonSocial" 
                                    CssClass="failureNotification" ValidationGroup="TodoError" 
                                    Display="Dynamic">*
                                </asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRazonSocial" runat="server" CssClass="requerido" 
                                    Enabled="False" MaxLength="60" ontextchanged="txtRazonSocial_TextChanged1" 
                                    Width="190px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td colspan="2">
                                <asp:CustomValidator ID="CustomValidator2" runat="server" 
                                    ControlToValidate="txtNombreComercial" CssClass="failureNotification"     
                                    ErrorMessage="No se pemiten simbolos en el campo &quot;Nombre Comercial&quot;" 
                                    ClientValidationFunction="validarSimbolos" 
                                    onservervalidate="CustomValidator_ServerValidate" 
                                    ValidationGroup="TodoError"></asp:CustomValidator>
                                <br />
                                <asp:CustomValidator ID="CustomValidator3" runat="server" 
                                    ControlToValidate="txtRazonSocial" CssClass="failureNotification"     
                                    ErrorMessage="No se pemiten simbolos en el campo &quot;Razon social&quot;" 
                                    ClientValidationFunction="validarSimbolos" 
                                    onservervalidate="CustomValidator_ServerValidate" 
                                    ValidationGroup="TodoError"></asp:CustomValidator>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </fieldset>
                                
            <fieldset>
                <legend>Informacion General Del Cliente</legend>
                <asp:Panel runat="server" ID="Direccion">
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lblDireccion">Direccion:</asp:Label>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
                                    ErrorMessage="Direccion requerida" ControlToValidate="txtDireccion" 
                                    CssClass="failureNotification" ValidationGroup="TodoError" 
                                    Display="Dynamic">*
                                </asp:RequiredFieldValidator>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtDireccion" runat="server" CssClass="requerido" 
                                    Enabled="false" Width="99%" ontextchanged="txtDireccion_TextChanged1">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblCiudad" runat="server">Estado:</asp:Label>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" 
                                    ControlToValidate="cmbCiudad" CssClass="failureNotification" Display="Dynamic" 
                                    ErrorMessage="Ciudad requerida" ValidationGroup="TodoError">*
                                </asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="cmbCiudad" Runat="server" AllowCustomText="True" 
                                    CssClass="requerido" DataSourceID="sqlCiudad" Enabled="False" 
                                    EnableLoadOnDemand="True" Filter="Contains" Height="190px" 
                                    HighlightTemplatedItems="True" MarkFirstMatch="True" 
                                    onitemdatabound="cmbCiudad_ItemDataBound" 
                                    onitemsrequested="cmbCiudad_ItemsRequested" 
                                    onselectedindexchanged="cmbCiudad_SelectedIndexChanged" Skin="Web20" 
                                    Sort="Ascending" Width="265px">
                                    <HeaderTemplate>
                                        <ul>
                                            <li class="col2"><b>Estado</b></li>
                                            <li class="col1"><b>Ciudad</b></li>
                                        </ul>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <ul>
                                            <li class="col2"><%# DataBinder.Eval(Container.DataItem, "DIVISION_NAME")%></li>
                                            <li class="col1"><%# DataBinder.Eval(Container.DataItem, "CITY_NAME")%></li>
                                        </ul>
                                    </ItemTemplate>
                                </telerik:RadComboBox>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblTelefono">Telefono:</asp:Label>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" 
                                    ErrorMessage="Telefono requerido" ControlToValidate="txtTelefono" 
                                    CssClass="failureNotification" ValidationGroup="TodoError" 
                                    Display="Dynamic">*
                                </asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTelefono" runat="server" CssClass="requerido" MaxLength="12" 
                                    Width="190px" Enabled="False" ontextchanged="txtTelefono_TextChanged1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblBarrio" runat="server">Barrio Distrito:</asp:Label>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" 
                                    ControlToValidate="txtBarrio" CssClass="failureNotification" Display="Dynamic" 
                                    ErrorMessage="Barrio/Distrito requerido" ValidationGroup="TodoError">*
                                </asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBarrio" runat="server" CssClass="requerido" Enabled="False" 
                                    MaxLength="50" ontextchanged="txtBarrio_TextChanged" Width="265px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblEMail">Correo<br />Electronico:</asp:Label>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" 
                                    ErrorMessage="Correo requerido" ControlToValidate="txtEMail" 
                                    CssClass="failureNotification" ValidationGroup="TodoError" 
                                    Display="Dynamic">*
                                </asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtEMail" runat="server" CssClass="requerido" Enabled="False" 
                                    ontextchanged="txtEMail_TextChanged1" Rows="30" Width="190px" 
                                    MaxLength="50"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblCodigoPostal" runat="server">Codigo Postal:</asp:Label>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtCodigoPostal" runat="server" Enabled="False" 
                                    MaxLength="10" ontextchanged="txtCodigoPostal_TextChanged" Skin="Web20" 
                                    Width="190px">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td colspan="2">
                                <asp:RegularExpressionValidator ID="expresion" runat="server" 
                                    ControlToValidate="txtEMail" CssClass="failureNotification" 
                                    ErrorMessage="Formato de correo no valido" SetFocusOnError="True" 
                                    style="Color:#FF0000" 
                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                                    ValidationGroup="TodoError"></asp:RegularExpressionValidator>
                                <br />
                                <asp:CustomValidator ID="CustomValidator1" runat="server" 
                                    ControlToValidate="txtDireccion" CssClass="failureNotification" 
                                    ErrorMessage="No se pemiten simbolos en campo &quot;Direccion&quot;" 
                                    ClientValidationFunction="validarSimbolos" 
                                    onservervalidate="CustomValidator_ServerValidate" 
                                    ValidationGroup="TodoError"></asp:CustomValidator>
                                <br />
                                <asp:CustomValidator ID="CustomValidator4" runat="server" 
                                    ControlToValidate="txtBarrio" CssClass="failureNotification" 
                                    ErrorMessage="No se pemiten simbolos en el campo &quot;Barrio o distrito&quot;" 
                                    ClientValidationFunction="validarSimbolos" 
                                    onservervalidate="CustomValidator_ServerValidate" 
                                    ValidationGroup="TodoError"></asp:CustomValidator>
                                <br />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                                    ErrorMessage="No se aceptan letras en el telefono" 
                                    ControlToValidate="txtTelefono" CssClass="failureNotification" 
                                    ValidationExpression="^\d+$" ValidationGroup="TodoError"></asp:RegularExpressionValidator>
                                
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </fieldset>
                                
            <fieldset>
                <legend>Datos Adicionales Del Cliente</legend>
                <asp:Panel runat="server" ID="DatosAdicionales">
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lblCondicionesPago">Condicion<br />De pago:</asp:Label>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="cmbCondicionPago" runat="server" Width="210px"
                                    Skin="Web20" Enabled="False" 
                                    onselectedindexchanged="cmbCondicionPago_SelectedIndexChanged" 
                                    DataSourceID="sqlCondicionesPago" Height="100px" 
                                    onitemdatabound="cmbCondicionPago_ItemDataBound">
                                </telerik:RadComboBox>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblCantidadEmpleados">Cantidad<br />Empleado:</asp:Label>
                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="txtCantidadEmpleados" runat="server" Width="210px"
                                    Skin="Web20" MinValue="0" Value="0" DataType="System.Int32" 
                                    Enabled="False" ontextchanged="txtCantidadEmpleados_TextChanged" 
                                    MaxLength="5">
                                    <IncrementSettings InterceptArrowKeys="False" InterceptMouseWheel="False" />
                                    <NumberFormat DecimalDigits="0" />
                                </telerik:RadNumericTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lblFrecuenciaCompra">Frecuencia<br />Compra:</asp:Label>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="cmbFrecuenciaCompra" runat="server" Width="210px"
                                    Skin="Web20" Enabled="False" 
                                    onselectedindexchanged="cmbFrecuenciaCompra_SelectedIndexChanged">
                                    <Items>
                                        <telerik:RadComboBoxItem runat="server" Text="30 días" Value="30 días" />
                                        <telerik:RadComboBoxItem runat="server" Text="7 días" Value="7 días" />
                                        <telerik:RadComboBoxItem runat="server" Text="15 días" Value="15 días" />
                                        <telerik:RadComboBoxItem runat="server" Text="45 días" Value="45 días" />
                                        <telerik:RadComboBoxItem runat="server" Text="60 días" Value="60 días" />
                                        <telerik:RadComboBoxItem runat="server" Text="75 días" Value="75 días" />
                                        <telerik:RadComboBoxItem runat="server" Text="90 días" Value="90 días" />
                                        <telerik:RadComboBoxItem runat="server" Text="105 días" Value="105 días" />
                                        <telerik:RadComboBoxItem runat="server" Text="120 días" Value="120 días" />
                                        <telerik:RadComboBoxItem runat="server" Text="135 días" Value="135 días" />
                                        <telerik:RadComboBoxItem runat="server" Text="150 días" Value="150 días" />
                                        <telerik:RadComboBoxItem runat="server" Text="165 días" Value="165 días" />
                                        <telerik:RadComboBoxItem runat="server" Text="180 días" Value="180 días" />
                                        <telerik:RadComboBoxItem runat="server" Text="365 días" Value="365 días" />
                                    </Items>
                                </telerik:RadComboBox>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblCantidadVisitantes">Cantidad<br />Visitantes:</asp:Label>
                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="txtCantidadVisitantes" runat="server" Width="210px"
                                    Skin="Web20" MinValue="0" Value="0" DataType="System.Int32" 
                                    Enabled="False" ontextchanged="txtCantidadVisitantes_TextChanged" 
                                    MaxLength="5">
                                    <IncrementSettings InterceptArrowKeys="False" InterceptMouseWheel="False" />
                                    <NumberFormat DecimalDigits="0" />
                                </telerik:RadNumericTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lblFrecuenciaMantenimiento">Frecuencia<br />Mantenimiento:</asp:Label>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="cmbFrecuenciaMantenimiento" runat="server" Width="210px"
                                    Skin="Web20" Enabled="False" 
                                    onselectedindexchanged="cmbFrecuenciaMantenimiento_SelectedIndexChanged">
                                    <Items>
                                        <telerik:RadComboBoxItem runat="server" Text="30 días" Value="30 días" />
                                        <telerik:RadComboBoxItem runat="server" Text="7 días" Value="7 días" />
                                        <telerik:RadComboBoxItem runat="server" Text="15 días" Value="15 días" />
                                        <telerik:RadComboBoxItem runat="server" Text="45 días" Value="45 días" />
                                        <telerik:RadComboBoxItem runat="server" Text="60 días" Value="60 días" />
                                        <telerik:RadComboBoxItem runat="server" Text="75 días" Value="75 días" />
                                        <telerik:RadComboBoxItem runat="server" Text="90 días" Value="90 días" />
                                    </Items>
                                </telerik:RadComboBox>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblCantidadLavatorios">Cantidad<br />Lavatorios:</asp:Label>
                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="txtCantidadLavatorios" runat="server" Width="210px"
                                    Skin="Web20" MinValue="0" Value="0" DataType="System.Int32" 
                                    Enabled="False" ontextchanged="txtCantidadLavatorios_TextChanged" 
                                    MaxLength="5">
                                    <IncrementSettings InterceptArrowKeys="False" InterceptMouseWheel="False" />
                                    <NumberFormat DecimalDigits="0" />
                                </telerik:RadNumericTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lblClienteEstrategico">Cliente<br />Estrategico:</asp:Label>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="cmbClienteEstrategico" runat="server" Width="210px"
                                    Skin="Web20" Enabled="False" 
                                    onselectedindexchanged="cmbClienteEstrategico_SelectedIndexChanged">
                                    <Items>
                                        <telerik:RadComboBoxItem runat="server" Text="SI" Value="TRUE" />
                                        <telerik:RadComboBoxItem runat="server" Text="NO" Value="FALSE" />
                                    </Items>
                                </telerik:RadComboBox>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblBañoHombre">Cantidad<br />Baños(H):</asp:Label>
                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="txtBañoHombre" runat="server" Width="210px"
                                    Skin="Web20" MinValue="0" Value="0" DataType="System.Int32" 
                                    Enabled="False" ontextchanged="txtBañoHombre_TextChanged" MaxLength="5">
                                    <IncrementSettings InterceptArrowKeys="False" InterceptMouseWheel="False" />
                                    <NumberFormat DecimalDigits="0" />
                                </telerik:RadNumericTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lblTipoTrafico">Tipo de<br />trafico:</asp:Label>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="cmbTipoTrafico" runat="server" Width="210px"
                                    Skin="Web20" Enabled="False" 
                                    onselectedindexchanged="cmbTipoTrafico_SelectedIndexChanged">
                                    <Items>
                                        <telerik:RadComboBoxItem runat="server" Text="Bajo" Value="Bajo" />
                                        <telerik:RadComboBoxItem runat="server" Text="Medio" Value="Medio" />
                                        <telerik:RadComboBoxItem runat="server" Text="Alto" Value="Alto" />
                                    </Items>
                                </telerik:RadComboBox>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblBañoMujer">Cantidad<br />Baños(M):</asp:Label>
                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="txtBañoMujer" runat="server" Width="210px"
                                    Skin="Web20" MinValue="0" Value="0" DataType="System.Int32" 
                                    Enabled="False" ontextchanged="txtBañoMujer_TextChanged" MaxLength="5">
                                    <IncrementSettings InterceptArrowKeys="False" InterceptMouseWheel="False" />
                                    <NumberFormat DecimalDigits="0" />
                                </telerik:RadNumericTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lblLimpiezaTercerizada">Limpieza<br />Tercerizada:</asp:Label>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="cmbLimpiezaTercerizada" runat="server" Width="210px"
                                    Skin="Web20" Enabled="False" 
                                    onselectedindexchanged="cmbLimpiezaTercerizada_SelectedIndexChanged">
                                    <Items>
                                        <telerik:RadComboBoxItem runat="server" Text="NO" Value="FALSE" />
                                        <telerik:RadComboBoxItem runat="server" Text="SI" Value="TRUE" />
                                    </Items>
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </fieldset>
                                
            <fieldset>
                <legend>Informacion De La Persona De Contacto</legend>
                <asp:Panel runat="server" ID="PersonaContacto">
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lblPersonaContacto">Nombre:</asp:Label>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" 
                                    ErrorMessage="Contacto requerido" ControlToValidate="txtPersonaContacto" 
                                    CssClass="failureNotification" ValidationGroup="TodoError" 
                                    Display="Dynamic">*
                                </asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPersonaContacto" runat="server" CssClass="requerido" 
                                    Enabled="False" MaxLength="50" ontextchanged="txtPersonaContacto_TextChanged1" 
                                    Width="190px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblTelefonoContacto">Telefono:</asp:Label>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" 
                                    ErrorMessage="Telefono del contacto requerido" ControlToValidate="txtTelefonoContacto" 
                                    CssClass="failureNotification" ValidationGroup="TodoError" 
                                    Display="Dynamic">*
                                </asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTelefonoContacto" runat="server" CssClass="requerido" 
                                    Enabled="False" MaxLength="12" ontextchanged="txtTelefonoContacto_TextChanged" 
                                    Width="190px"></asp:TextBox>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtExtension" runat="server" MaxLength="6" Width="40px" 
                                    EmptyMessage="EXT" Skin="Web20" LabelWidth=""></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblApellidoContacto" runat="server">Apellido:</asp:Label>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" 
                                    ErrorMessage="Apellido del contacto requerido." CssClass="failureNotification" 
                                    ValidationGroup="TodoError" ControlToValidate="txtApellido">*
                                </asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtApellido" runat="server" CssClass="requerido" 
                                    MaxLength="50" Width="190px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lblCorreoContacto" runat="server">Correo:</asp:Label>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" 
                                    ControlToValidate="txtCorreoContacto" CssClass="failureNotification" 
                                    Display="Dynamic" ErrorMessage="Correo del contacto requerido" 
                                    ValidationGroup="TodoError">*
                                </asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCorreoContacto" runat="server" CssClass="requerido" 
                                    Enabled="False" MaxLength="50" ontextchanged="txtCorreoContacto_TextChanged1" 
                                    Width="190px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblPosicion" runat="server">Cargo:</asp:Label>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtPosicion" runat="server" Enabled="False" 
                                    MaxLength="50" ontextchanged="txtPosicion_TextChanged" Skin="Web20" 
                                    Width="190px">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td colspan="4">
                                <asp:RegularExpressionValidator ID="validadorEMailContacot" runat="server"
                                    ControlToValidate="txtCorreoContacto" ErrorMessage="Formato de correo no valido"
                                    ForeColor="#FF0000" 
                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                                    SetFocusOnError="True" ValidationGroup="TodoError" 
                                    CssClass="failureNotification"></asp:RegularExpressionValidator>

                                <br />
                                <asp:CustomValidator ID="CustomValidator5" runat="server" 
                                    ControlToValidate="txtPersonaContacto" CssClass="failureNotification" 
                                    ErrorMessage="No se permiten simbolos en el campo &quot;Nombre&quot;" 
                                    ClientValidationFunction="validarSimbolos" 
                                    onservervalidate="CustomValidator_ServerValidate" 
                                    ValidationGroup="TodoError"></asp:CustomValidator>
                                <br />
                                <asp:CustomValidator ID="CustomValidator7" runat="server" 
                                    ControlToValidate="txtApellido" CssClass="failureNotification" 
                                    ErrorMessage="No se permiten simbolos en el campo &quot;Apellido&quot;" 
                                    ClientValidationFunction="validarSimbolos" 
                                    onservervalidate="CustomValidator_ServerValidate" 
                                    ValidationGroup="TodoError"></asp:CustomValidator>
                                <br />
                                <asp:CustomValidator ID="CustomValidator6" runat="server" 
                                    ControlToValidate="txtPosicion" CssClass="failureNotification" 
                                    ErrorMessage="No se permiten simbolos en el campo &quot;cargo&quot;" 
                                    ClientValidationFunction="validarSimbolos" 
                                    onservervalidate="CustomValidator_ServerValidate" 
                                    ValidationGroup="TodoError"></asp:CustomValidator>
                                <br />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                                    ErrorMessage="No se aceptan letras en el telefono" 
                                    CssClass="failureNotification" ValidationExpression="^\d+$" 
                                    ValidationGroup="TodoError" ControlToValidate="txtTelefonoContacto"></asp:RegularExpressionValidator>
                                <br />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" 
                                    ControlToValidate="txtExtension" CssClass="failureNotification" 
                                    ErrorMessage="No se aceptan letras en la extension" ValidationExpression="^\d+$" 
                                    ValidationGroup="TodoError"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </fieldset>
        </div>
                                
        <div class="centrarGrid">
            <fieldset>
                <legend>Dispensadores y Productos</legend>
                <telerik:RadAjaxPanel runat="server" ID="panelGrid">
                    <telerik:RadGrid runat="server" ID="grdDispensadoresProducto" AutoGenerateColumns="false"
                        ShowStatusBar="true" Skin="Web20" Enabled="False" 
                        onneeddatasource="grdDispensadoresProducto_NeedDataSource">
                        <MasterTableView DataKeyNames="ID_DISPENSER">
                                                
                            <GroupByExpressions>
                                <telerik:GridGroupByExpression>
                                    <SelectFields>
                                        <telerik:GridGroupByField FieldAlias="Categoria" FieldName="DISPENSER_TYPE" />
                                    </SelectFields>
                                    <GroupByFields>
                                        <telerik:GridGroupByField FieldName="DISPENSER_TYPE" SortOrder="Ascending" />
                                    </GroupByFields>
                                </telerik:GridGroupByExpression>
                            </GroupByExpressions>

                            <Columns>
                                <telerik:GridBinaryImageColumn DataField="DISPENSER_FACE" 
                                    HeaderButtonType="None" HeaderText="Vista Previa" UniqueName="Preview">
                                </telerik:GridBinaryImageColumn>

                                <telerik:GridBoundColumn DataField="ID_DISPENSER" HeaderButtonType="None" 
                                    HeaderText="Codigo Del Dispensador" UniqueName="Codigo">
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn DataField="DESCRIPTION" HeaderButtonType="None" 
                                    HeaderText="Descripcion" UniqueName="Descripciones">
                                </telerik:GridBoundColumn>

                                <telerik:GridNumericColumn DataField="DISPENSER_PRICE" HeaderButtonType="None"
                                    HeaderText="Costo Dispensador" UniqueName="dispenserPrice" DataType="System.Decimal"
                                    DataFormatString="${0:N2}">
                                </telerik:GridNumericColumn>

                                <telerik:GridTemplateColumn HeaderText="Cantidad" UniqueName="Cantidades">
                                    <ItemTemplate>
                                        <telerik:RadNumericTextBox ID="txtCantidadDispensadores" runat="server" 
                                            DataType="System.Int32" MaxLength="3" MinValue="0" Skin="Web20" Value="0">
                                            <NumberFormat DecimalDigits="0" />
                                        </telerik:RadNumericTextBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="Producto" UniqueName="Productos">
                                    <ItemTemplate>
                                        <telerik:RadComboBox ID="cmbProducto" runat="server" Skin="Web20"
                                            DataTextField="PRODUCT_DESCRIP" onitemdatabound="cmbProducto_ItemDataBound"
                                            DataSource='<%# funcion( (string)DataBinder.Eval(Container.DataItem, "ID_DISPENSER")) %>'
                                            HighlightTemplatedItems="true" DropDownWidth="300px" Sort="Ascending" Height="190px"
                                            EnableVirtualScrolling="true">
                                            <HeaderTemplate>
                                                <ul>
                                                    <li class="col1"><b>Codigo</b></li>
                                                    <li class="col2"><b>Producto</b></li>
                                                </ul>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <ul>
                                                    <li class="col1"><%# DataBinder.Eval(Container.DataItem, "PRODUCT_ID")%></li>
                                                    <li class="col2"><%# DataBinder.Eval(Container.DataItem, "PRODUCT_DESCRIP")%></li>
                                                </ul>
                                            </ItemTemplate>
                                        </telerik:RadComboBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="Cantidad (en cajas)" 
                                    UniqueName="CantidadProducto">
                                    <ItemTemplate>
                                        <telerik:RadNumericTextBox ID="txtCantidadProducto" runat="server" 
                                            DataType="System.Int32" MaxLength="3" MinValue="0" Skin="Web20" Value="0">
                                            <NumberFormat DecimalDigits="0" />
                                        </telerik:RadNumericTextBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </telerik:RadAjaxPanel>
            </fieldset>
        </div>
                                
        <div class="centrar">
            <asp:Button runat="server" ID="btEnviar" Text="Enviar Solicitud" 
                onclick="btEnviar_Click" Enabled="False" ValidationGroup="TodoError"/>

            <br />
                        
            <asp:Literal runat="server" ID="Errores" />
            <asp:ValidationSummary ID="vsErrores" runat="server" 
                CssClass="failureNotification" HeaderText="----- Campos Requeridos -----"
                ValidationGroup="TodoError" ShowMessageBox="True" ShowSummary="False" />
        </div>
    </asp:Panel>
</asp:Content>
