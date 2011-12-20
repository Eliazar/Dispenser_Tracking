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

            <telerik:AjaxSetting AjaxControlID="btAprobar">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="pnlGrid" LoadingPanelID="Cargar" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="btCerrar">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="pnlGrid" LoadingPanelID="Cargar" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            
            <telerik:AjaxSetting AjaxControlID="btCancelar">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="pnlGrid" LoadingPanelID="Cargar" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="btContinuar">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="pnlGrid" LoadingPanelID="Cargar" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="btCancelarRechazo">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="pnlGrid" LoadingPanelID="Cargar" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    
    <telerik:RadAjaxPanel runat="server" ID="pnlGrid">
        <table>
            <tr>
                <td>
                    <telerik:RadGrid runat="server" ID="grdAutorizaciones" Skin="Web20" AutoGenerateColumns="False"
                        AllowFilteringByColumn="True" PageSize="15" AllowPaging="True" 
                        ondetailtabledatabind="grdAutorizaciones_DetailTableDataBind" 
                        onneeddatasource="grdAutorizaciones_NeedDataSource" BorderStyle="Outset" BorderColor="#70B1D2">
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
                                    CurrentFilterFunction="EqualTo" DataField="DR_ID" FilterControlWidth="50px" 
                                    HeaderButtonType="None" HeaderStyle-Font-Bold="true" 
                                    HeaderText="Numero Solicitud" ShowFilterIcon="false" UniqueName="Codigo">
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn AutoPostBackOnFilter="true" 
                                    CurrentFilterFunction="Contains" DataField="CLIENT_ID" HeaderButtonType="None" 
                                    HeaderStyle-Font-Bold="true" HeaderText="Codigo Distribuidor" 
                                    ShowFilterIcon="false">
                                </telerik:GridBoundColumn>
                                
                                <telerik:GridBoundColumn AutoPostBackOnFilter="true" 
                                    CurrentFilterFunction="Contains" DataField="CLIENT_NAME" 
                                    HeaderButtonType="None" HeaderStyle-Font-Bold="true" HeaderText="Distribuidor" 
                                    ShowFilterIcon="false">
                                </telerik:GridBoundColumn>
                                
                                <telerik:GridBoundColumn AutoPostBackOnFilter="true" 
                                    CurrentFilterFunction="Contains" DataField="TRADE_NAME" 
                                    FilterControlWidth="200px" HeaderButtonType="None" HeaderStyle-Font-Bold="true" 
                                    HeaderText="Cliente final" ShowFilterIcon="false">
                                </telerik:GridBoundColumn>
                                
                                <telerik:GridDateTimeColumn DataField="PROGRAMMING_DATE" DataFormatString="{0:d}" 
                                    FilterControlWidth="70%" HeaderButtonType="None" HeaderStyle-Font-Bold="true" 
                                    HeaderText="Fecha A Instalar" PickerType="DatePicker">
                                </telerik:GridDateTimeColumn>
                                
                                <telerik:GridBoundColumn AutoPostBackOnFilter="true" 
                                    CurrentFilterFunction="Contains" DataField="REASON_DESCRIP" 
                                    FilterControlWidth="150px" HeaderButtonType="None" HeaderStyle-Font-Bold="true" 
                                    HeaderText="Razon Instalacion" ShowFilterIcon="false">
                                </telerik:GridBoundColumn>
                                
                                <telerik:GridBoundColumn AutoPostBackOnFilter="true" 
                                    CurrentFilterFunction="Contains" DataField="STATUS_DESCRIP" 
                                    HeaderButtonType="None" HeaderStyle-Font-Bold="true" 
                                    HeaderText="Estado Solicitud" ShowFilterIcon="false" UniqueName="Estado">
                                </telerik:GridBoundColumn>

                                <telerik:GridNumericColumn DataField="INVER_SOLICITADA" HeaderButtonType="None" HeaderText="Inversion Solicitada"
                                    UniqueName="inverSol" DataType="System.Double" DataFormatString="${0:N2}" HeaderStyle-Font-Bold="true">
                                </telerik:GridNumericColumn>

                                <telerik:GridNumericColumn DataField="INVER_APRO" HeaderButtonType="None" HeaderText="Inversion Aprobada"
                                    UniqueName="inverApro" DataType="System.Double" DataFormatString="${0:N2}" HeaderStyle-Font-Bold="true">
                                </telerik:GridNumericColumn>

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

                                <telerik:GridCheckBoxColumn HeaderText="Siguiente Mes" HeaderButtonType="None" HeaderStyle-Font-Bold="true"
                                    CurrentFilterFunction="EqualTo" ShowFilterIcon="false" AutoPostBackOnFilter="true" DataField="NEXT_MONTH">
                                </telerik:GridCheckBoxColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </td>
            </tr>
        </table>
    </telerik:RadAjaxPanel>
</asp:Content>
