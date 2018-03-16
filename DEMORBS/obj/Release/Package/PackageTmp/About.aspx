<%@ Page Title="Informazioni" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="BRSi.About" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <hgroup class="title">
        <h1><%: Title %>.</h1>
        <h2>Pagina di descrizione dell'app.</h2>
    </hgroup>

    <article>
        <p>        
            Utilizzare questa area per fornire informazioni aggiuntive.
        
            Utilizzare questa area per fornire informazioni aggiuntive.
        </p>
    </article>

    <!--<aside>
        <h3>Titolo a parte</h3>
        <p>        
            Utilizzare questa area per fornire informazioni aggiuntive.
        </p>
        <ul>
            <li><a runat="server" href="~/">Home</a></li>
            <li><a runat="server" href="~/About.aspx">Informazioni</a></li>
            <li><a runat="server" href="~/Contact.aspx">Contatto</a></li>
        </ul>
    </aside>-->
</asp:Content>