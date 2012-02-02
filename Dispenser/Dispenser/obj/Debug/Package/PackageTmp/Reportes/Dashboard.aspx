<%@ Page Title="" Language="C#" MasterPageFile="~/Reportes/Reportes.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="Dispenser.Reportes.Dashboard" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript" src="../Scripts/Advertencia.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>

    <asp:Literal runat="server" ID="literal" />

    <div>
        <div style="width:100%;">
            <fieldset>
                <legend>Cantidades X Motivo</legend>
                <telerik:RadGrid ID="grdMotivoCantidad" runat="server" Skin="Web20" 
                    AutoGenerateColumns="False" onneeddatasource="grdMotivoCantidad_NeedDataSource" 
                    onexcelmlexportrowcreated="grdMotivoCantidad_ExcelMLExportRowCreated" >
                    
                    <ExportSettings HideStructureColumns="false" ExportOnlyData="true" OpenInNewWindow="true" Excel-Format="ExcelML" 
                        FileName="Cantidades_Motivo"/>
                    
                    <MasterTableView DataKeyNames="CLIENT_ID" CommandItemDisplay="Top">
                        <CommandItemSettings ShowExportToExcelButton="true" ShowAddNewRecordButton="false" ShowRefreshButton="false"/>
                        <Columns>
                            <telerik:GridBoundColumn DataField="CLIENT_ID" HeaderButtonType="None" HeaderText="Codigo Socio" UniqueName="Codigo"
                                HeaderStyle-Font-Bold="true">
                            </telerik:GridBoundColumn>
                        
                            <telerik:GridBoundColumn DataField="CLIENT_NAME" HeaderButtonType="None" HeaderText="Socio Comercial" UniqueName="Cliente"
                                HeaderStyle-Font-Bold="true">
                            </telerik:GridBoundColumn>

                            <telerik:GridNumericColumn DataField="NUEVOS" HeaderButtonType="None" HeaderText="Nuevo" UniqueName="SolNuevas"
                                HeaderStyle-Font-Bold="true">
                            </telerik:GridNumericColumn>

                            <telerik:GridNumericColumn DataField="AMPLIACION" HeaderButtonType="None" HeaderText="Ampliacion" UniqueName="SolAmpliacion"
                                HeaderStyle-Font-Bold="true">
                            </telerik:GridNumericColumn>

                            <telerik:GridNumericColumn DataField="DETERIORO" HeaderButtonType="None" HeaderText="Deterioro" UniqueName="SolDeterioro"
                                HeaderStyle-Font-Bold="true">
                            </telerik:GridNumericColumn>

                            <telerik:GridNumericColumn DataField="OTROS" HeaderButtonType="None" HeaderText="Otros" UniqueName="Otros"
                                HeaderStyle-Font-Bold="true">
                            </telerik:GridNumericColumn>

                            <telerik:GridNumericColumn DataField="TOTAL" HeaderButtonType="None" HeaderText="Total" UniqueName="Total"
                                HeaderStyle-Font-Bold="true">
                            </telerik:GridNumericColumn>

                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            </fieldset>
        </div>
    
        <div style="width:100%;">
            <fieldset>
                <legend>Inversion X Motivo</legend>
                <telerik:RadGrid ID="grdMotivoInversion" runat="server" Skin="Web20" 
                    AutoGenerateColumns="false" 
                    onneeddatasource="grdMotivoInversion_NeedDataSource" 
                    onexcelmlexportrowcreated="grdMotivoInversion_ExcelMLExportRowCreated" 
                    onexcelmlexportstylescreated="grdMotivoInversion_ExcelMLExportStylesCreated">
                    
                    <ExportSettings HideStructureColumns="false" ExportOnlyData="true" OpenInNewWindow="true" Excel-Format="ExcelML" 
                        FileName="Inversion_Motivo_US$"/>

                    <MasterTableView DataKeyNames="CLIENT_ID" CommandItemDisplay="Top" TableLayout="Auto">
                        <CommandItemSettings ShowExportToExcelButton="true" ShowAddNewRecordButton="false" ShowRefreshButton="false"/>
                        <Columns>

                            <telerik:GridBoundColumn DataField="CLIENT_ID" HeaderButtonType="None" HeaderText="Codigo Socio" UniqueName="Codigo"
                                HeaderStyle-Font-Bold="true">
                            </telerik:GridBoundColumn>
                        
                            <telerik:GridBoundColumn DataField="CLIENT_NAME" HeaderButtonType="None" HeaderText="Socio Comercial" UniqueName="Cliente"
                                HeaderStyle-Font-Bold="true">
                            </telerik:GridBoundColumn>
                            
                            <telerik:GridNumericColumn DataField="NUEVOS" HeaderButtonType="None" HeaderText="Nuevo" UniqueName="SolNuevas"
                                HeaderStyle-Font-Bold="true" DataFormatString="$ {0:N2}">
                            </telerik:GridNumericColumn>

                            <telerik:GridNumericColumn DataField="AMPLIACION" HeaderButtonType="None" HeaderText="Ampliacion" UniqueName="SolAmpliacion"
                                HeaderStyle-Font-Bold="true" DataFormatString="$ {0:N2}">
                            </telerik:GridNumericColumn>

                            <telerik:GridNumericColumn DataField="DETERIORO" HeaderButtonType="None" HeaderText="Deterioro" UniqueName="SolDeterioro"
                                HeaderStyle-Font-Bold="true" DataFormatString="$ {0:N2}">
                            </telerik:GridNumericColumn>

                            <telerik:GridNumericColumn DataField="OTROS" HeaderButtonType="None" HeaderText="Otros" UniqueName="Otros"
                                HeaderStyle-Font-Bold="true" DataFormatString="$ {0:N2}">
                            </telerik:GridNumericColumn>

                            <telerik:GridNumericColumn DataField="TOTAL" HeaderButtonType="None" HeaderText="Total" UniqueName="Total"
                                HeaderStyle-Font-Bold="true" DataFormatString="$ {0:N2}">
                            </telerik:GridNumericColumn>

                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            </fieldset>
        </div>

        <div style="width:100%;">
            <fieldset>
            <legend>Inversion en US$</legend>
            <telerik:RadGrid ID="grdInversiones" runat="server" Skin="Web20" AutoGenerateColumns="false" onneeddatasource="grdInversiones_NeedDataSource"
                onexcelmlexportrowcreated="grdInversiones_ExcelMLExportRowCreated" onexcelmlexportstylescreated="grdInversiones_ExcelMLExportStylesCreated">
        
                <ExportSettings HideStructureColumns="false" ExportOnlyData="true" OpenInNewWindow="true" Excel-Format="ExcelML" 
                    FileName="Inversion_En_US$"/>
        
                <MasterTableView DataKeyNames="CLIENT_ID" CommandItemDisplay="Top">
                    <CommandItemSettings ShowExportToExcelButton="true" ShowAddNewRecordButton="false" ShowRefreshButton="false"/>
                    <Columns>
                        <telerik:GridBoundColumn DataField="CLIENT_ID" HeaderButtonType="None" HeaderText="Codigo Socio" UniqueName="Codigo"
                            HeaderStyle-Font-Bold="true">
                        </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn DataField="CLIENT_NAME" HeaderButtonType="None" HeaderText="Socio Comercial" UniqueName="Cliente"
                            HeaderStyle-Font-Bold="true">
                        </telerik:GridBoundColumn>

                        <telerik:GridNumericColumn DataField="BUDGET_TP" HeaderButtonType="None" HeaderText="Presupuesto Mes" UniqueName="BudgetTP"
                            DataFormatString="$ {0:N2}" HeaderStyle-Font-Bold="true">
                        </telerik:GridNumericColumn>

                        <telerik:GridNumericColumn DataField="APROBADO" HeaderButtonType="None" HeaderText="Aprobado" UniqueName="Aprobado"
                            DataFormatString="$ {0:N2}" HeaderStyle-Font-Bold="true">
                        </telerik:GridNumericColumn>

                        <telerik:GridNumericColumn DataField="MOVIDAS_APROBADAS" HeaderButtonType="None" HeaderText="Carried Aprobadas" UniqueName="C_Aprobado"
                            DataFormatString="$ {0:N2}" HeaderStyle-Font-Bold="true">
                        </telerik:GridNumericColumn>

                        <telerik:GridNumericColumn DataField="PENDIENTE" HeaderButtonType="None" HeaderText="Pendiente" UniqueName="Pendiente"
                            DataFormatString="$ {0:N2}" HeaderStyle-Font-Bold="true">
                        </telerik:GridNumericColumn>

                        <telerik:GridNumericColumn DataField="MOVIDAS_PENDIENTE" HeaderButtonType="None" HeaderText="Carried Pendientes" UniqueName="C_Pendiente"
                            DataFormatString="$ {0:N2}" HeaderStyle-Font-Bold="true">
                        </telerik:GridNumericColumn>

                        <telerik:GridNumericColumn DataField="DISPONIBLE" UniqueName="Disponible" HeaderText="Disponibilidad" HeaderButtonType="None"
                            DataFormatString="$ {0:N2}" HeaderStyle-Font-Bold="true">
                        </telerik:GridNumericColumn>
                
                        <telerik:GridNumericColumn DataField="LIBRE" HeaderButtonType="None" HeaderText="% Disponible" UniqueName="PorceDisponible"
                            HeaderStyle-Font-Bold="true" DataFormatString="{0:P2}">
                        </telerik:GridNumericColumn>

                        <telerik:GridNumericColumn DataField="USADO" HeaderButtonType="None" HeaderText="% Usado" UniqueName="Usado"
                            HeaderStyle-Font-Bold="true" DataFormatString="{0:P2}">
                        </telerik:GridNumericColumn>

                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
            </fieldset>
        </div>

        <div style="width:100%;">
            <fieldset>
                <legend>Cantidad De Solcitudes</legend>
                <telerik:RadGrid ID="grdCantidad" runat="server" Skin="Web20" 
                    AutoGenerateColumns="false" onneeddatasource="grdCantidad_NeedDataSource" 
                    onexcelmlexportrowcreated="grdCantidad_ExcelMLExportRowCreated" 
                    onexcelmlexportstylescreated="grdCantidad_ExcelMLExportStylesCreated">
            
                    <ExportSettings Excel-Format="ExcelML" FileName="Cantidad_Solicitudes" HideStructureColumns="false" ExportOnlyData="true"
                        OpenInNewWindow="true" />

                    <MasterTableView DataKeyNames="CLIENT_ID" CommandItemDisplay="Top">
                        <CommandItemSettings ShowExportToExcelButton="true" ShowAddNewRecordButton="false" ShowRefreshButton="false"/>
                        <Columns>
                            <telerik:GridBoundColumn DataField="CLIENT_ID" HeaderText="Codigo Socio" HeaderButtonType="None" UniqueName="Codigo" HeaderStyle-Font-Bold="true">
                            </telerik:GridBoundColumn>
                    
                            <telerik:GridBoundColumn DataField="CLIENT_NAME" HeaderText="Socio Comercial" HeaderButtonType="None" UniqueName="Cliente" HeaderStyle-Font-Bold="true">
                            </telerik:GridBoundColumn>

                            <telerik:GridNumericColumn DataField="SOLICITUDES_APROBADAS" HeaderText="Cantidad Aprobadas" HeaderButtonType="None" UniqueName="Aprobadas"
                                HeaderStyle-Font-Bold="true">
                            </telerik:GridNumericColumn>
                    
                            <telerik:GridNumericColumn DataField="SOLICITUDES_PENDIENTES" HeaderText="Cantidad Pendientes" HeaderButtonType="None" UniqueName="Pendientes"
                                HeaderStyle-Font-Bold="true">
                            </telerik:GridNumericColumn>

                            <telerik:GridNumericColumn DataField="SOLICITUDES_RECHAZADAS" HeaderText="Cantidad Rechazadas" HeaderButtonType="None" UniqueName="Rechazadas"
                                HeaderStyle-Font-Bold="true">
                            </telerik:GridNumericColumn>

                            <telerik:GridNumericColumn DataField="PORCE_APRO" HeaderText="% Aprobado" HeaderButtonType="None" UniqueName="porceApro"
                                HeaderStyle-Font-Bold="true" DataFormatString="{0:P2}">
                            </telerik:GridNumericColumn>

                            <telerik:GridNumericColumn DataField="PORCE_PENDI" HeaderText="% Pendiente" HeaderButtonType="None" UniqueName="porcePend"
                                HeaderStyle-Font-Bold="true" DataFormatString="{0:P2}">
                            </telerik:GridNumericColumn>

                            <telerik:GridNumericColumn DataField="PORCE_RECH" HeaderText="% Rechazado" HeaderButtonType="None" UniqueName="porceRech"
                                HeaderStyle-Font-Bold="true" DataFormatString="{0:P2}">
                            </telerik:GridNumericColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            </fieldset>
        </div>
    </div>

</asp:Content>