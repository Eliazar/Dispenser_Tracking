<%@ Page Title="" Language="C#" MasterPageFile="~/Dst/Clientes.Master" AutoEventWireup="true" CodeBehind="SeguimientoSolicitudes.aspx.cs" Inherits="Dispenser.Dst.SeguimientoSolicitudes" %>

<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>

    <telerik:RadAjaxLoadingPanel ID="Cargar" Runat="server" Skin="Web20">
    </telerik:RadAjaxLoadingPanel>

    <telerik:RadAjaxManager ID="radajaxmanager" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="grdConsulta">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="pnlGrid" LoadingPanelID="Cargar" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <div>
        <telerik:RadAjaxPanel runat="server" ID="pnlGrid" Width="100%">
            <telerik:RadGrid ID="grdConsulta" runat="server" Skin="Web20" AutoGenerateColumns="false"
                AllowFilteringByColumn="true" PageSize="15" AllowPaging="true" 
                onneeddatasource="grdConsulta_NeedDataSource" 
                ondetailtabledatabind="grdConsulta_DetailTableDataBind">
                <GroupingSettings CaseSensitive="false" />
                <MasterTableView DataKeyNames="DR_ID" TableLayout="Fixed">
                                
                    <GroupByExpressions>
                        <telerik:GridGroupByExpression>
                            <SelectFields>
                                <telerik:GridGroupByField FieldAlias="Fecha" FieldName="DATE_REQUEST" FormatString="{0:D}" />
                            </SelectFields>
                            <GroupByFields>
                                <telerik:GridGroupByField FieldName="DATE_REQUEST" SortOrder="Descending" />
                            </GroupByFields>
                        </telerik:GridGroupByExpression>
                    </GroupByExpressions>

                    <DetailTables>
                        <telerik:GridTableView DataKeyNames="DISPENSER_ID" Name="Solicitud" IsFilterItemExpanded="false">
                            <Columns>
                                <telerik:GridBoundColumn HeaderText="Codigo Dispensador" HeaderButtonType="None" DataField="DISPENSER_ID">
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn HeaderText="Codigo Producto" HeaderButtonType="None" DataField="PRODUCT_ID">
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn HeaderText="Cantidad Dispensadores" HeaderButtonType="None" DataField="DISPENSER_QUANTITY">
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn HeaderText="Cantidad Producto (En cajas)" HeaderButtonType="None" DataField="PRODUCT_QUANTITY">
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn HeaderText="Cantidad Dispensadores (Oficial)" HeaderButtonType="None" DataField="APPROVAL_QTY">
                                </telerik:GridBoundColumn>

                                <telerik:GridNumericColumn HeaderText="Inverion Detalle" HeaderButtonType="None" DataField="INVERSION"
                                    UniqueName="dispenserPrice" DataType="System.Decimal" DataFormatString="${0:N2}">
                                </telerik:GridNumericColumn>

                                <telerik:GridBoundColumn HeaderText="Estado de la solicitud" HeaderButtonType="None" DataField="STATUS_DESCRIP">
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn HeaderButtonType="None" HeaderText="Comentarios" DataField="ADMIN_COMMENT">
                                </telerik:GridBoundColumn>
                            </Columns>
                        </telerik:GridTableView>
                    </DetailTables>

                    <Columns>
                        <telerik:GridBoundColumn HeaderText="Numero Solicitud" HeaderButtonType="None" DataField="DR_ID" AutoPostBackOnFilter="true"
                            ShowFilterIcon="false" CurrentFilterFunction="Contains">
                        </telerik:GridBoundColumn>

                        <telerik:GridDateTimeColumn HeaderText="Fecha Solicitud" HeaderButtonType="None" DataField="DATE_REQUEST" DataFormatString="{0:d}"
                            PickerType="DatePicker" FilterControlWidth="70%">
                        </telerik:GridDateTimeColumn>

                        <telerik:GridBoundColumn HeaderText="Cliente" HeaderButtonType="None" DataField="TRADE_NAME" ShowFilterIcon="false"
                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                        </telerik:GridBoundColumn>

                        <telerik:GridDateTimeColumn HeaderText="Fecha Programada" HeaderButtonType="None" DataField="PROGRAMMING_DATE" DataFormatString="{0:d}"
                            PickerType="DatePicker" FilterControlWidth="70%">
                        </telerik:GridDateTimeColumn>

                        <telerik:GridBoundColumn HeaderText="Razon de instalacion" HeaderButtonType="None" DataField="REASON_DESCRIP" ShowFilterIcon="false"
                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                        </telerik:GridBoundColumn>

                        <telerik:GridNumericColumn HeaderText="Pendiente Total" HeaderButtonType="None" DataField="INVER_SOLICITADA" DataFormatString="${0:N2}"
                            FilterControlWidth="70%">
                        </telerik:GridNumericColumn>

                        <telerik:GridNumericColumn HeaderText="Aprobado Total" HeaderButtonType="None" DataField="INVER_APRO" DataFormatString="${0:N2}"
                            FilterControlWidth="70%">
                        </telerik:GridNumericColumn>

                        <telerik:GridBoundColumn HeaderText="Estado de la Solicitud" HeaderButtonType="None" DataField="STATUS_DESCRIP" AutoPostBackOnFilter="true"
                            ShowFilterIcon="false" CurrentFilterFunction="Contains">
                        </telerik:GridBoundColumn>

                        <telerik:GridCheckBoxColumn HeaderText="Para siguiente mes" HeaderButtonType="None" DataField="NEXT_MONTH" AutoPostBackOnFilter="true"
                            CurrentFilterFunction="EqualTo" ShowFilterIcon="false">
                        </telerik:GridCheckBoxColumn>
                    </Columns>

                </MasterTableView>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </div>
</asp:Content>
