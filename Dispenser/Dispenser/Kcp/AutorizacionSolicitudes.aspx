<%@ Page Title="" Language="C#" MasterPageFile="~/Kcp/Autorizaciones.Master" AutoEventWireup="true" CodeBehind="AutorizacionSolicitudes.aspx.cs" Inherits="Dispenser.Kcp.AutorizacionSolicitudes" %>

<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <script type="text/javascript" src="../Scripts/Advertencia.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" 
        LoadScriptsBeforeUI="False">
    </telerik:RadScriptManager>

    <telerik:RadWindowManager ID="RadWindowManager1" runat="server" Skin="Web20">
    </telerik:RadWindowManager>
    
    <telerik:RadAjaxLoadingPanel runat="server" ID="Cargar" Skin="Web20">
    </telerik:RadAjaxLoadingPanel>

    <telerik:RadAjaxManager ID="radajaxmanager" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="grdAutorizaciones">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="pnlGrid" LoadingPanelID="Cargar" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btCrear">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="pnlNuevo" LoadingPanelID="Cargar" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="cmbDispensador">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="pnlNuevo" LoadingPanelID="Cargar" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
   
    <telerik:RadAjaxPanel runat="server" ID="pnlGrid" Width="100%">
        <telerik:RadTabStrip ID="RadTabStrip1" runat="server" Skin="Web20" SelectedIndex="0" MultiPageID="RadMultiPage1" Enabled="false"
            Align="Right" Width="100%">
            <Tabs>
                <telerik:RadTab Text="Tab1" Selected="True" Font-Bold="true" Visible="false">
                </telerik:RadTab>
                <telerik:RadTab Text="Tab2" Font-Bold="true" Visible="false">
                </telerik:RadTab>
            </Tabs>
        </telerik:RadTabStrip>
    
        <telerik:RadMultiPage runat="server" ID="RadMultiPage1" SelectedIndex="0">
            <telerik:RadPageView ID="RadPageView1" runat="server" Width="100%">
                <table width="100%">
                    <tr>
                        <td>
                            <telerik:RadGrid runat="server" ID="grdAutorizaciones" Skin="Web20" AutoGenerateColumns="False"
                                AllowFilteringByColumn="True" PageSize="15" AllowPaging="True" 
                                ondetailtabledatabind="grdAutorizaciones_DetailTableDataBind" 
                                onneeddatasource="grdAutorizaciones_NeedDataSource">
                                <GroupingSettings CaseSensitive="false" />
                                <MasterTableView DataKeyNames="DR_ID" TableLayout="Fixed">
                                
                                    <GroupByExpressions>
                                        <telerik:GridGroupByExpression>
                                            <SelectFields>
                                                <telerik:GridGroupByField FieldAlias="Fecha" FieldName="DATE_REQUEST" FormatString="{0:d}" />
                                            </SelectFields>
                                            <GroupByFields>
                                                <telerik:GridGroupByField FieldName="DATE_REQUEST" SortOrder="Descending" />
                                            </GroupByFields>
                                        </telerik:GridGroupByExpression>
                                    </GroupByExpressions>

                                    <DetailTables>
                                        <telerik:GridTableView DataKeyNames="DISPENSER_ID" Name="Solicitud" IsFilterItemExpanded="false">
                                            <CommandItemSettings ExportToPdfText="Export to Pdf" />
                                            <Columns>
                                                <telerik:GridBoundColumn DataField="DISPENSER_ID" HeaderButtonType="None" 
                                                    HeaderText="Codigo Dispensador" UniqueName="CodigoDispensador">
                                                </telerik:GridBoundColumn>

                                                <telerik:GridBoundColumn DataField="PRODUCT_ID" HeaderButtonType="None" 
                                                    HeaderText="Codigo Producto" UniqueName="CodigoProducto">
                                                </telerik:GridBoundColumn>

                                                <telerik:GridBoundColumn DataField="DISPENSER_QUANTITY" HeaderButtonType="None" 
                                                    HeaderText="Cantidad Dispensadores" UniqueName="CantidadDispensadores">
                                                </telerik:GridBoundColumn>

                                                <telerik:GridBoundColumn DataField="PRODUCT_QUANTITY" HeaderButtonType="None" 
                                                    HeaderText="Cantidad Producto (En Cajas)" UniqueName="CantidadProducto">
                                                </telerik:GridBoundColumn>

                                                <telerik:GridBoundColumn DataField="APPROVAL_QTY" HeaderButtonType="None" 
                                                    HeaderText="Cantidad Dispensadores (Oficial)" UniqueName="CantidadDispensadoresAprobados">
                                                </telerik:GridBoundColumn>

                                                <telerik:GridNumericColumn DataField="INVERSION" HeaderButtonType="None" HeaderText="Costo Detalle"
                                                    UniqueName="dispenserPrice" DataType="System.Double" DataFormatString="${0:N2}">
                                                </telerik:GridNumericColumn>

                                                <telerik:GridBoundColumn DataField="STATUS_DESCRIP" HeaderButtonType="None" 
                                                    HeaderText="Estado Actual" UniqueName="EstadoSolicitud">
                                                </telerik:GridBoundColumn>
                                            </Columns>
                                        </telerik:GridTableView>
                                    </DetailTables>

                                    <Columns>
                                        <telerik:GridBoundColumn AutoPostBackOnFilter="true" 
                                            CurrentFilterFunction="EqualTo" DataField="DR_ID" FilterControlWidth="100%" 
                                            HeaderButtonType="None" HeaderStyle-Font-Bold="true" 
                                            HeaderText="Numero Solicitud" ShowFilterIcon="false" UniqueName="Codigo">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn AutoPostBackOnFilter="true" 
                                            CurrentFilterFunction="Contains" DataField="CLIENT_ID" HeaderButtonType="None" 
                                            HeaderStyle-Font-Bold="true" HeaderText="Codigo Distribuidor" 
                                            ShowFilterIcon="false" FilterControlWidth="100%" UniqueName="CodigoDistribuidor">
                                        </telerik:GridBoundColumn>
                                
                                        <telerik:GridBoundColumn AutoPostBackOnFilter="true" 
                                            CurrentFilterFunction="Contains" DataField="CLIENT_NAME" 
                                            HeaderButtonType="None" HeaderStyle-Font-Bold="true" HeaderText="Distribuidor" 
                                            ShowFilterIcon="false" FilterControlWidth="100%" UniqueName="nombreDistribuidor">
                                        </telerik:GridBoundColumn>
                                
                                        <telerik:GridBoundColumn AutoPostBackOnFilter="true" 
                                            CurrentFilterFunction="Contains" DataField="TRADE_NAME" 
                                            FilterControlWidth="100%" HeaderButtonType="None" HeaderStyle-Font-Bold="true" 
                                            HeaderText="Cliente final" ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>
                                
                                        <telerik:GridDateTimeColumn DataField="PROGRAMMING_DATE" DataFormatString="{0:d}" 
                                            FilterControlWidth="70%" HeaderButtonType="None" HeaderStyle-Font-Bold="true" 
                                            HeaderText="Fecha A Instalar" PickerType="DatePicker">
                                        </telerik:GridDateTimeColumn>
                                
                                        <telerik:GridBoundColumn AutoPostBackOnFilter="true" 
                                            CurrentFilterFunction="Contains" DataField="REASON_DESCRIP" 
                                            FilterControlWidth="100%" HeaderButtonType="None" HeaderStyle-Font-Bold="true" 
                                            HeaderText="Razon Instalacion" ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>
                                
                                        <telerik:GridBoundColumn AutoPostBackOnFilter="true" 
                                            CurrentFilterFunction="Contains" DataField="STATUS_DESCRIP" 
                                            HeaderButtonType="None" HeaderStyle-Font-Bold="true" FilterControlWidth="100%"
                                            HeaderText="Estado Solicitud" ShowFilterIcon="false" UniqueName="Estado">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridNumericColumn DataField="INVER_SOLICITADA" HeaderButtonType="None" HeaderText="Inversion Solicitada"
                                            UniqueName="inverSol" DataType="System.Double" DataFormatString="${0:N2}" HeaderStyle-Font-Bold="true"
                                            FilterControlWidth="60%">
                                        </telerik:GridNumericColumn>

                                        <telerik:GridNumericColumn DataField="INVER_APRO" HeaderButtonType="None" HeaderText="Inversion Aprobada"
                                            UniqueName="inverApro" DataType="System.Double" DataFormatString="${0:N2}" HeaderStyle-Font-Bold="true"
                                            FilterControlWidth="60%">
                                        </telerik:GridNumericColumn>

                                        <telerik:GridCheckBoxColumn HeaderText="Siguiente Mes" HeaderButtonType="None" HeaderStyle-Font-Bold="true"
                                            CurrentFilterFunction="EqualTo" ShowFilterIcon="false" AutoPostBackOnFilter="true" DataField="NEXT_MONTH">
                                        </telerik:GridCheckBoxColumn>

                                        <telerik:GridBoundColumn AllowFiltering="false" DataField="COMMENTS" 
                                            HeaderButtonType="None" HeaderStyle-Font-Bold="true" HeaderText="Comentarios" 
                                            UniqueName="Comentario">
                                        </telerik:GridBoundColumn>
                                
                                        <telerik:GridTemplateColumn AllowFiltering="false" HeaderStyle-Font-Bold="true" 
                                            HeaderText="Ver" UniqueName="Ver">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btVer" runat="server" Font-Size="10.5px" 
                                                    onclick="btVer_Click" Text="Ver Solicitud" Width="90px" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                        </td>
                    </tr>
                </table>
            </telerik:RadPageView>

            <telerik:RadPageView ID="RadPageView2" runat="server" Width="100%">
                
                <asp:SqlDataSource ID="sqlEstadoCiudad" runat="server"></asp:SqlDataSource>
                <asp:SqlDataSource ID="sqlSegmentos" runat="server"></asp:SqlDataSource>
                <asp:SqlDataSource ID="sqlDispensadorProducto" runat="server"></asp:SqlDataSource>

                <div class="centrar">
                    <fieldset>
                        <legend>Solicitud</legend>
                        <asp:Panel runat="server" ID="pnlEncabezado">
                            <table width="100%">
                                <tr>
                                    <th colspan="4">
                                        <asp:Label runat="server" ID="lblSolicitud"></asp:Label>
                                        <br />
                                        <asp:Label ID="lblCodigo" runat="server"></asp:Label>
                                    </th>
                                </tr>
                                <tr>
                                    <td colspan="4" class="clase1">
                                        <asp:Button runat="server" ID="btAtras" Text="Atras" Width="120px" 
                                            onclick="btAtras_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="lblDistribuidor">Solicitante:</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblNombreDistribuidor" runat="server" Font-Bold="true"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lblEstado">Estado Actual:</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lblDescripcionEstado" Font-Bold="true"></asp:Label>
                                    </td>
                                
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:Label ID="lblNumeroSol" runat="server" Visible="False"></asp:Label>
                                        <asp:Label ID="lblcodigoDis" runat="server" Visible="false"></asp:Label>
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
                                        <asp:Label ID="lblNombreComercial" runat="server">Nombre Comercial:</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNombreComercial" runat="server" 
                                            ontextchanged="txtNombreComercial_TextChanged" Width="190px" 
                                            CssClass="requerido" MaxLength="60"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFechaSol" runat="server">Fecha De Servicio:</asp:Label>
                                    </td>
                                    <td>
                                        <telerik:RadDateInput ID="txtFechaSol" Runat="server" DateFormat="d/M/yyyy" 
                                            Enabled="False" Width="190px" Skin="Office2010Black" 
                                            style="top: 0px; left: 0px">
                                        </telerik:RadDateInput>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lblRazonSocial">Razón Social:</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRazonSocial" runat="server" CssClass="requerido" 
                                            ontextchanged="txtRazonSocial_TextChanged" Width="190px" MaxLength="60"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="lblFechaInstalacion">Fecha Instalacion:</asp:Label>
                                    </td>
                                    <td>
                                        <telerik:RadDatePicker ID="dpFechaInstalacion" runat="server" Width="190px" 
                                            Skin="Metro" CssClass="requerido">
                                            <Calendar Skin="Metro" UseColumnHeadersAsSelectors="False" 
                                                UseRowHeadersAsSelectors="False" ViewSelectorText="x">
                                            </Calendar>
                                            <DateInput DateFormat="M/d/yyyy" DisplayDateFormat="M/d/yyyy" 
                                                EnableSingleInputRendering="True" LabelWidth="64px">
                                            </DateInput>
                                            <DatePopupButton HoverImageUrl="" ImageUrl="" />
                                        </telerik:RadDatePicker>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblTelefono" runat="server">Telefono:</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTelefono" runat="server" CssClass="requerido" 
                                            ontextchanged="txtTelefono_TextChanged" Width="190px" MaxLength="12"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblCedulaJuridica" runat="server">Cedula Juridica:</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCedulaJuridica" runat="server" CssClass="requerido" 
                                            ontextchanged="txtCedulaJuridica_TextChanged" Width="190px" MaxLength="15"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblSegmento" runat="server">Segmento:</asp:Label>
                                    </td>
                                    <td>
                                        <telerik:RadComboBox ID="cmbSegmento" runat="server" AllowCustomText="true" 
                                            DataSourceID="sqlSegmentos" EnableLoadOnDemand="true" Filter="Contains" 
                                            Height="100px" MaxHeight="100px" onitemdatabound="cmbSegmento_ItemDataBound" 
                                            onselectedindexchanged="cmbSegmento_SelectedIndexChanged" Skin="Web20" 
                                            Width="190px" CssClass="requerido">
                                        </telerik:RadComboBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblPersonaContacto" runat="server">Nombre Contacto:</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPersonaContacto" runat="server" CssClass="requerido" 
                                            MaxLength="50" ontextchanged="txtPersonaContacto_TextChanged" 
                                            Width="190px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblApellido" runat="server" Text="Apellido Contacto:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtApellido" runat="server" CssClass="requerido" 
                                            MaxLength="50" Width="190px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>    
                                        <asp:Label ID="lblTelefonoContacto" runat="server">Telefono Contacto:</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTelefonoContacto" runat="server" CssClass="requerido" 
                                            MaxLength="12" ontextchanged="txtTelefonoContacto_TextChanged" 
                                            Width="190px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblExtension" runat="server">Ext.:</asp:Label>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="RadTextBox1" Runat="server" 
                                            EnableSingleInputRendering="True" LabelWidth="64px" MaxLength="6" Skin="Web20" 
                                            Text="0" Width="40px">
                                        </telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblDepartamento" runat="server">Estado:</asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <telerik:RadComboBox ID="cmbDepartamento" runat="server" AllowCustomText="True" 
                                            CssClass="requerido" DataSourceID="sqlEstadoCiudad" EnableLoadOnDemand="True" 
                                            Filter="Contains" Height="190px" HighlightTemplatedItems="True" 
                                            MarkFirstMatch="True" onitemdatabound="cmbDepartamento_ItemDataBound" 
                                            onitemsrequested="cmbDepartamento_ItemsRequested" Skin="Web20" 
                                            Sort="Ascending" Width="265px" >
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
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblDireccion" runat="server">Direccion:</asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtDireccion" runat="server" CssClass="requerido" 
                                            Height="40px" ontextchanged="txtDireccion_TextChanged" TextMode="MultiLine" 
                                            Width="450px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblComentarios" runat="server">Comentarios:</asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:Label ID="lblDescripcionComentarios" runat="server" Font-Bold="true"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </fieldset>

                    <fieldset>
                        <legend>Ingresar Nuevo Dispensador</legend>
                        <asp:Panel runat="server" ID="pnlNuevo" Width="100%">
                            <table width="100%">
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="lblDispensador">Dispensador:</asp:Label>
                                    </td>
                                    <td>
                                        <telerik:RadComboBox runat="server" ID="cmbDispensador" Skin="Web20" Width="190px"
                                            EmptyMessage="Seleccione un dispensador" MaxHeight="100px"
                                            HighlightTemplatedItems="True" DataSourceID="sqlDispensadorProducto" 
                                            onitemdatabound="cmbDispensador_ItemDataBound" DropDownWidth="400px" AllowCustomText="true"
                                            Filter="Contains" MarkFirstMatch="true" AutoPostBack="True" 
                                            onselectedindexchanged="cmbDispensador_SelectedIndexChanged">
                                            <ItemTemplate>
                                                <ul>
                                                    <li class="col1"><telerik:RadBinaryImage ID="RadBinaryImage1" runat="server" DataValue='<%# Eval("DISPENSER_FACE") %>'/></li>
                                                    <li class="col1"><%# DataBinder.Eval(Container.DataItem, "ID_DISPENSER")%></li>
                                                </ul>
                                            </ItemTemplate>
                                        </telerik:RadComboBox>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lblProducto">Producto:</asp:Label>
                                    </td>
                                    <td>
                                        <telerik:RadComboBox runat="server" ID="cmbProducto" Skin="Web20" Width="190px" DropDownWidth="400px"
                                            EmptyMessage="Antes seleccione un dispensador">
                                        </telerik:RadComboBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="lblcantDisp">Cantidad Dispensador:</asp:Label>
                                    </td>
                                    <td>
                                        <telerik:RadNumericTextBox runat="server" ID="txtCantDis" Skin="Web20" 
                                            Width="190px" MinValue="0" Value="0">
                                            <NumberFormat DecimalDigits="0" GroupSeparator="" />
                                        </telerik:RadNumericTextBox>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lblcantProd">Cantidad Producto:</asp:Label>
                                    </td>
                                    <td>
                                        <telerik:RadNumericTextBox runat="server" Skin="Web20" Width="190px" 
                                            MinValue="0" Value="0" ID="txtProducto">
                                            <NumberFormat DecimalDigits="0" GroupSeparator="" />
                                        </telerik:RadNumericTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" class="clase1">
                                        <asp:Button ID="btCrear" runat="server" onclick="btCrear_Click" Text="Agregar" 
                                            Width="120px" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </fieldset>
                </div>

                <div class="grid">
                    <asp:Panel runat="server" ID="pnlDescripcionSol">
                        <telerik:RadGrid runat="server" ID="grdDescripciones" Skin="Web20" AutoGenerateColumns="false" ShowStatusBar="true" 
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

                                    <telerik:GridBoundColumn DataField="DESCRIPTION" HeaderButtonType="None" HeaderStyle-Font-Bold="true"
                                        HeaderText="Descripcion" UniqueName="Descripcion">
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
                    </asp:Panel>    
                </div>

                <div class="botones">
                    <asp:Button runat="server" ID="btAprobar" Text="Aprobar Cita" Width="120px" 
                        ValidationGroup="TodoError"/>
                    &nbsp;
                    <asp:Button runat="server" ID="btRechazar" Text="Rechazar Cita" Width="120px" />
                    <br />
                    <asp:Button runat="server" ID="btGuardar" Text="Guardar Cambios" Width="120px"/>
                    &nbsp;
                    <asp:Button runat="server" ID="btCerrarCita" Text="Cerrar Cita" Width="120px"/>
                </div>

            </telerik:RadPageView>
        </telerik:RadMultiPage>
    </telerik:RadAjaxPanel>
</asp:Content>