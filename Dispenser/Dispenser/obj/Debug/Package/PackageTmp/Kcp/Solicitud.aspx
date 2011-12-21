<%@ Page Title="" Language="C#" MasterPageFile="~/Kcp/Autorizaciones.Master" AutoEventWireup="true" CodeBehind="Solicitud.aspx.cs" Inherits="Dispenser.Kcp.Solicitud" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <link href="../Styles/Site.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript" src="../Scripts/Advertencia.js"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server" 
        LoadScriptsBeforeUI="False">
    </telerik:RadScriptManager>

    <telerik:RadWindowManager ID="RadWindowManager1" runat="server" Skin="Web20">
    </telerik:RadWindowManager>

    <telerik:RadAjaxLoadingPanel runat="server" ID="Cargar" Skin="Web20">
    </telerik:RadAjaxLoadingPanel>

    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="cmbSegmento">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="pnlEncabezado" LoadingPanelID="Cargar"/>
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="cmbDepartamento">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="pnlEncabezado" LoadingPanelID="Cargar"/>
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="btNuevo">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="main" LoadingPanelID="Cargar" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="cmbDispensador">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="cmbProducto" LoadingPanelID="Cargar" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="btCancelar">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="main" LoadingPanelID="Cargar" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="btCrear">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="main" LoadingPanelID="Cargar" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="btGuardar">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="main" LoadingPanelID="Cargar" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="btAprobar">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="main" LoadingPanelID="Cargar" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="btRechazar">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="main" LoadingPanelID="Cargar" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="btCerrarCita">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="main" LoadingPanelID="Cargar" />
                </UpdatedControls>
            </telerik:AjaxSetting>

        </AjaxSettings>
    </telerik:RadAjaxManager>

    <asp:Panel runat="server" ID="main">
        <div class="centrar">
            <fieldset>
            <legend>Solcitud</legend>
            <asp:Panel runat="server" ID="pnlEncabezado">
                <table>
                    <tr>
                        <th colspan="5"> 
                            <asp:Label runat="server" ID="lblSolicitud"></asp:Label>
                            <br />
                            <asp:Label ID="lblCodigo" runat="server"></asp:Label>
                        </th>
                    </tr>
                    <tr>
                        <td colspan="5" class="clase1">
                            <asp:Button runat="server" ID="btAtras" Text="Atras" Width="120px" 
                                onclick="btAtras_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblEstado">Estado Actual:</asp:Label>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblDescripcionEstado" Font-Bold="true"></asp:Label>
                        </td>
                        <td colspan="3" class="clase1">
                            
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblMotivo" runat="server">Motivo:</asp:Label>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblDescripcionMotivo" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:Label ID="lblNombreComercial" runat="server">Nombre Comercial:</asp:Label>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtNombreComercial" Width="190px" 
                                Skin="Web20" ontextchanged="txtNombreComercial_TextChanged">
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblFechaSol" runat="server">Fecha De Servicio:</asp:Label>
                        </td>
                        <td>
                            <telerik:RadDateInput ID="txtFechaSol" Runat="server" DateFormat="d/M/yyyy" 
                                Enabled="False" Width="190px">
                            </telerik:RadDateInput>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblRazonSocial">Razón Social:</asp:Label>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtRazonSocial" skin="Web20" 
                                Width="190px" ontextchanged="txtRazonSocial_TextChanged">
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblFechaInstalacion">Fecha Instalacion:</asp:Label>
                        </td>
                        <td>
                            <telerik:RadDatePicker ID="dpFechaInstalacion" runat="server" Width="190px" 
                                Skin="Web20">
                            </telerik:RadDatePicker>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:Label ID="lblTelefono" runat="server">Telefono:</asp:Label>
                        </td>
                        <td>
                            <telerik:RadNumericTextBox ID="txtTelefono" runat="server" MaxLength="12" 
                                MinValue="0" ontextchanged="txtTelefono_TextChanged" Skin="Web20" Width="190px">
                                <NumberFormat DecimalDigits="0" GroupSeparator="" />
                            </telerik:RadNumericTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblCedulaJuridica" runat="server">Cedula Juridica:</asp:Label>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtCedulaJuridica" runat="server" Skin="Web20" 
                                Width="190px" ontextchanged="txtCedulaJuridica_TextChanged">
                            </telerik:RadTextBox>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:Label ID="lblDepartamento" runat="server">Estado:</asp:Label>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="cmbDepartamento" runat="server" AutoPostBack="True" 
                                MaxHeight="100px" onselectedindexchanged="cmbDepartamento_SelectedIndexChanged" 
                                Skin="Web20" Width="190px">
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblSegmento" runat="server">Segmento:</asp:Label>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="cmbSegmento" runat="server" Skin="Web20" Width="190px" 
                                onselectedindexchanged="cmbSegmento_SelectedIndexChanged" 
                                AutoPostBack="True" MaxHeight="100px">
                            </telerik:RadComboBox>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblCiudad">Ciudad:</asp:Label>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="cmbCiudad" runat="server" MaxHeight="100px" 
                                onselectedindexchanged="cmbCiudad_SelectedIndexChanged" Skin="Web20" 
                                Width="190px">
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td>    
                            <asp:Label ID="lblSubSegmento" runat="server">Sub Segmento:</asp:Label>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="cmbSubSegmento" runat="server" Skin="Web20" 
                                Width="190px" onselectedindexchanged="cmbSubSegmento_SelectedIndexChanged" 
                                MaxHeight="100px">
                            </telerik:RadComboBox>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:Label ID="lblTelefonoContacto" runat="server">Telefono Contacto:</asp:Label>
                        </td>
                        <td>
                            <telerik:RadNumericTextBox ID="txtTelefonoContacto" runat="server" 
                                MaxLength="12" MinValue="0" ontextchanged="txtTelefonoContacto_TextChanged" 
                                Skin="Web20" Width="190px">
                                <NumberFormat DecimalDigits="0" GroupSeparator="" />
                            </telerik:RadNumericTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblPersonaContacto">Persona Contacto:</asp:Label>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtPersonaContacto" Skin="Web20" 
                                Width="190px" ontextchanged="txtPersonaContacto_TextChanged">
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblDireccion" runat="server">Direccion:</asp:Label>
                        </td>
                        <td colspan="4">
                            <telerik:RadTextBox ID="txtDireccion" runat="server" Height="40px" 
                                ontextchanged="txtDireccion_TextChanged" Skin="Web20" TextMode="MultiLine" 
                                Width="500px">
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblComentarios" runat="server">Comentarios:</asp:Label>
                        </td>
                        <td colspan="4">
                            <asp:Label ID="lblDescripcionComentarios" runat="server" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            </fieldset>
        
            <asp:Panel runat="server" ID="pnlNuevo">
                <table>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblDispensador" Visible="False">Codigo Dispensador:</asp:Label>
                        </td>
                        <td>
                            <telerik:RadComboBox runat="server" ID="cmbDispensador" Skin="Web20" Width="190px"
                                AllowCustomText="True" Filter="Contains" Visible="False" 
                                AutoPostBack="True" 
                                onselectedindexchanged="cmbDispensador_SelectedIndexChanged" MaxHeight="100px">
                            </telerik:RadComboBox>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblProducto" Visible="False">Producto:</asp:Label>
                        </td>
                        <td>
                            <telerik:RadComboBox runat="server" ID="cmbProducto" Skin="Web20" Width="190px" 
                                Visible="False" MaxHeight="100px">
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblcantDisp" Visible="False">Cantidad Dispensador:</asp:Label>
                        </td>
                        <td>
                            <telerik:RadNumericTextBox runat="server" ID="txtCantDis" Skin="Web20" 
                                Width="190px" MinValue="0" Value="0" Visible="False">
                                <NumberFormat DecimalDigits="0" GroupSeparator="" />
                            </telerik:RadNumericTextBox>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblcantProd" Visible="False">Cantidad Producto:</asp:Label>
                        </td>
                        <td>
                            <telerik:RadNumericTextBox runat="server" Skin="Web20" Width="190px" 
                                MinValue="0" Value="0" Visible="False" ID="txtProducto">
                                <NumberFormat DecimalDigits="0" GroupSeparator="" />
                            </telerik:RadNumericTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="clase1">
                            <asp:Button runat="server" ID="btCrear" Text="Agregar" Width="120px" 
                                Visible="False" onclick="btCrear_Click" /> 
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td colspan="2" class="clase1">
                            <asp:Button runat="server" ID="btCancelar" Text="Cancelar" Width="120px" 
                                Visible="False" onclick="btCancelar_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5" class="clase1">
                            <asp:Button runat="server" ID="btNuevo" Text="Nuevo" Width="120px" 
                                onclick="btNuevo_Click"/>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </div>

        <div class="grid">
            <telerik:RadAjaxPanel runat="server" ID="pnlDescripcionSol">
                <telerik:RadGrid runat="server" ID="grdDescripciones" Skin="Web20" 
                    AutoGenerateColumns="false" ShowStatusBar="true" 
                    onneeddatasource="grdDescripciones_NeedDataSource">
                    <MasterTableView>
                        <Columns>
                            <telerik:GridBoundColumn DataField="PRODUCT_ID" HeaderButtonType="None" 
                                HeaderStyle-Font-Bold="true" HeaderText="Codigo Producto" 
                                UniqueName="CodigoProducto">
                                <HeaderStyle Font-Bold="True" />
                            </telerik:GridBoundColumn>

                            <telerik:GridBoundColumn DataField="PRODUCT_QUANTITY" HeaderButtonType="None" 
                                HeaderStyle-Font-Bold="true" HeaderText="Cantidad Producto" 
                                UniqueName="CantidadProducto">
                                <HeaderStyle Font-Bold="True" />
                            </telerik:GridBoundColumn>

                            <telerik:GridBoundColumn DataField="DISPENSER_ID" HeaderButtonType="None" 
                                HeaderStyle-Font-Bold="true" HeaderText="Codigo Dispensador" 
                                UniqueName="CodigoDispensador">
                                <HeaderStyle Font-Bold="True" />
                            </telerik:GridBoundColumn>

                            <telerik:GridBoundColumn DataField="DISPENSER_QUANTITY" HeaderButtonType="None" 
                                HeaderStyle-Font-Bold="true" HeaderText="Cantidad Dispensador" 
                                UniqueName="CantidadDispensador">
                                <HeaderStyle Font-Bold="True" />
                            </telerik:GridBoundColumn>

                            <telerik:GridTemplateColumn HeaderButtonType="None" 
                                HeaderStyle-Font-Bold="true" HeaderText="Dispensadores Aprobados" 
                                UniqueName="Aprobados">
                                <ItemTemplate>
                                    <telerik:RadNumericTextBox ID="txtDisAprobados" runat="server" MaxValue="999" 
                                        MinValue="0" ontextchanged="txtDisAprobados_TextChanged" Skin="Web20" 
                                        Value='<%# cantidad((string)DataBinder.Eval(Container.DataItem, "DISPENSER_ID"), (string)DataBinder.Eval(Container.DataItem, "PRODUCT_ID")) %>' 
                                        Width="120px">
                                        <NumberFormat DecimalDigits="0" />
                                    </telerik:RadNumericTextBox>
                                </ItemTemplate>
                                <HeaderStyle Font-Bold="True" />
                            </telerik:GridTemplateColumn>

                            <telerik:GridNumericColumn DataField="UNIDAD" HeaderButtonType="None" HeaderText="Costo Unitario"
                                UniqueName="dispenserPrice" DataType="System.Double" DataFormatString="${0:N2}" HeaderStyle-Font-Bold="true">
                            </telerik:GridNumericColumn>

                            <telerik:GridNumericColumn DataField="INVERSION" HeaderButtonType="None" HeaderText="Costo Detalle"
                                UniqueName="dispenserPrice" DataType="System.Double" DataFormatString="${0:N2}" HeaderStyle-Font-Bold="true">
                            </telerik:GridNumericColumn>

                            <telerik:GridTemplateColumn HeaderButtonType="None" 
                                HeaderStyle-Font-Bold="true" HeaderText="Estado" UniqueName="Estado">
                                <ItemTemplate>
                                    <telerik:RadComboBox ID="cmbEstados" runat="server" 
                                        DataSource='<%# comboInGrid( (string)DataBinder.Eval(Container.DataItem, "DISPENSER_ID"), (string)DataBinder.Eval(Container.DataItem, "PRODUCT_ID") ) %>' 
                                        DataTextField="STATUS_DESCRIP" DataValueField="STATUS_ID" 
                                        onselectedindexchanged="cmbEstados_SelectedIndexChanged" Skin="Web20" 
                                        Width="120px" MaxHeight="100px">
                                    </telerik:RadComboBox>
                                </ItemTemplate>
                                <HeaderStyle Font-Bold="True" />
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderButtonType="None" 
                                HeaderStyle-Font-Bold="true" HeaderText="Comanetarios" UniqueName="Comentarios">
                                <ItemTemplate>
                                    <telerik:RadTextBox ID="txtComentariosGrid" runat="server" 
                                        ontextchanged="txtComentariosGrid_TextChanged" Skin="Web20" 
                                        Text='<%# comentarios((string)DataBinder.Eval(Container.DataItem, "DISPENSER_ID"), (string)DataBinder.Eval(Container.DataItem, "PRODUCT_ID")) %>' 
                                        TextMode="MultiLine" Width="120px">
                                    </telerik:RadTextBox>
                                </ItemTemplate>
                                <HeaderStyle Font-Bold="True" />
                            </telerik:GridTemplateColumn>
                        </Columns>

                    </MasterTableView>
                </telerik:RadGrid>
            </telerik:RadAjaxPanel>
        </div>

        <div class="botones">
            <asp:Button runat="server" ID="btAprobar" Text="Programar" Width="120px" 
                onclick="btAprobar_Click"/>
            &nbsp;
            <asp:Button runat="server" ID="btRechazar" Text="Rechazar" Width="120px" 
                onclick="btRechazar_Click" />
            <br />
            <asp:Button runat="server" ID="btGuardar" Text="Guardar Cambios" Width="120px" 
                onclick="btGuardar_Click"/>
            &nbsp;
            <asp:Button runat="server" ID="btCerrarCita" Text="Cerrar Cita" Width="120px" 
                onclick="btCerrarCita_Click"/>
        </div>

    </asp:Panel>

</asp:Content>
