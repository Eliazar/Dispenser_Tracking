<%@ Page Title="" Language="C#" MasterPageFile="~/Mantenimiento/Mantenimiento.Master" AutoEventWireup="true" CodeBehind="Editar_Usuario.aspx.cs" Inherits="Dispenser.Mantenimiento.Editar_Usuario" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript" src="../Scripts/Advertencia.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>

    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdUsuarios" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Windows7">
    </telerik:RadAjaxLoadingPanel>
    
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" UpdateCommand="UPDATE USUARIOS SET [USER_NAME] = @USER_NAME, [STATUS] = @STATUS, [E_MAIL] = @E_MAIL 
    WHERE [USER_ID] = @USER_ID" OldValuesParameterFormatString="original_{0}" ProviderName="System.Data.SqlClient">
        <UpdateParameters> 
            <asp:Parameter Name="USER_NAME" Type="String"/>
            <asp:Parameter Name="STATUS" Type="Boolean" />
            <asp:Parameter Name="E_MAIL" Type="String" />
            <asp:Parameter Name="USER_ID" Type="String" />
            <asp:Parameter Name="original_USER_NAME" Type="String" />
            <asp:Parameter Name="original_STATUS" Type="Boolean" />
            <asp:Parameter Name="original_E_MAIL" Type="String" />
            <asp:Parameter Name="original_USER_ID" Type="String" />
        </UpdateParameters>
    </asp:SqlDataSource>
    
    <div class="centrar">
        <fieldset>
            <legend>Editar Usuario</legend>
            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
                <telerik:RadGrid ID="grdUsuarios" runat="server" Skin="Web20" AutoGenerateColumns="false" ShowStatusBar="true"
                AllowAutomaticUpdates="true" ValidationSettings-ValidationGroup="TodoError" onitemupdated="grdUsuarios_ItemUpdated"
                DataSourceID="SqlDataSource1">
                    <MasterTableView DataKeyNames="USER_ID">
                        
                        <Columns>
                            <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="Editar">
                            </telerik:GridEditCommandColumn>

                            <telerik:GridTemplateColumn HeaderText="ID Usuario" UniqueName="Codigo" HeaderButtonType="None" HeaderStyle-Font-Bold="true">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblCodigo" Text='<%# Eval("USER_ID", "{0:C}") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadTextBox ID="txtCodigo" runat="server" Text='<%# Bind("USER_ID") %>' Enabled="false">
                                    </telerik:RadTextBox>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderButtonType="None" HeaderText="Nombre Usuario" UniqueName="Usuario" HeaderStyle-Font-Bold="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblUsuario" runat="server" Text='<%# Eval("USER_NAME", "{0:C}") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadTextBox ID="txtUsuario" runat="server" Skin="Web20" Text='<%# Bind("USER_NAME") %>'>
                                    </telerik:RadTextBox>
                                    <span style="color: Red">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtUsuario" Display="Dynamic" 
                                        ErrorMessage="Usuario Mandatorio." ValidationGroup="TodoError">*</asp:RequiredFieldValidator>
                                    </span>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn ColumnEditorID="txtCorreo" HeaderButtonType="None" HeaderText="Correo Electronico" UniqueName="Correo"
                            HeaderStyle-Font-Bold="true" EditFormColumnIndex="1">
                                <ItemTemplate>
                                    <asp:Label ID="lblCorreo" runat="server" Text='<%# Eval("E_MAIL", "{0:C}") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadTextBox ID="txtCorreo" runat="server" Skin="Web20" Text='<%# Bind("E_MAIL") %>'>
                                    </telerik:RadTextBox>
                                    <span style="color: Red">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtCorreo" Display="Dynamic" 
                                        ErrorMessage="Correo Mandatorio." ValidationGroup="TodoError">*</asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtCorreo"
                                        ErrorMessage="Formato de correo invalido." ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                        Display="Dynamic" ValidationGroup="TodoError">*</asp:RegularExpressionValidator>
                                    </span>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridCheckBoxColumn DataField="STATUS" EditFormColumnIndex="1" HeaderButtonType="None" HeaderText="Codigo Activo"
                            UniqueName="Activo" HeaderStyle-Font-Bold="true">
                            </telerik:GridCheckBoxColumn>

                        </Columns>

                        <EditFormSettings CaptionDataField="USER_NAME" CaptionFormatString="Edición de usuario {0}" ColumnNumber="2">
                            <FormTableItemStyle Wrap="false" />
                            <FormCaptionStyle ForeColor="Brown" />
                            <FormMainTableStyle CellPadding="3" CellSpacing="0" GridLines="None" Width="100%" />
                            <FormTableStyle CellPadding="2" CellSpacing="0" Height="110px" />
                            <FormTableAlternatingItemStyle Wrap="false" />
                            <EditColumn ButtonType="ImageButton" CancelText="Cancelar" HeaderButtonType="None" UpdateText="Actualizar" />
                            <FormTableButtonRowStyle HorizontalAlign="Right" />
                        </EditFormSettings>
                    </MasterTableView>
                </telerik:RadGrid>

                <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="failureNotification" ValidationGroup="TodoError" 
                    HeaderText="Conflictos" ShowMessageBox="true" ShowSummary="false" />
            </telerik:RadAjaxPanel>
        </fieldset>
    </div>
</asp:Content>
