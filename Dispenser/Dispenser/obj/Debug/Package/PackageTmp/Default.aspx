<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Dispenser.Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    
    <asp:Label runat="server" ID="mensajes"></asp:Label>

    <div class="derecha">
        <div class="derecha-arriba">
            <div class="derecha-fondo">
                <div class="aplicaciones">
                    <h2>Estadisticas</h2>
                    <table style="width: 100%">
                        <tr>
                            <td>
                                <center><asp:Label runat="server" ID="lblTP"/></center>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <center><asp:Label runat="server" ID="lblInvAut"/></center>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <center><asp:Label runat="server" ID="lblInvPend"/></center>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <center><asp:Label runat="server" ID="lblRestante"/></center>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <center><asp:Label runat="server" ID="lblInvUsada"/></center>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
    
    <div class="izquierda">
        
        <div>
            
        </div>

        <h2>
            Bienvenidos!
        </h2>
        <p>
            <asp:Label runat="server" ID="parrafo1">Desde esta aqui podras hacer tus solicitudes de dispensador y tambien ver si ya estan autorizadas.
                ¿Cómo hacer una solicitud? A continuacion te decimos como:</asp:Label>
        </p>
        <p>
            <asp:Label runat="server" ID="parrafo2"><b>Solicitud de Dispensadores:</b> En el menu de servicios situado en la parte superior busca la opcion de
            solicitud de dispensadores y haz click para redireccinarte a la pagina de solicitud de dispensadores, llena el formulario de solicitud y listo.
            </asp:Label>
        </p>
        <p>
            <asp:Label runat="server" ID="parrafo3"><b>Seguimiento de Solicitudes:</b> En el menu de servicios situado en la parte superior busca la opcion de 
            seguimiento de solicitudes y haz click para redireccionarte a la pagina de seguimiento de dispensadores, desde alli podras dar seguimiento a las
            solicitudes.
            </asp:Label>
        </p>
    </div>
</asp:Content>