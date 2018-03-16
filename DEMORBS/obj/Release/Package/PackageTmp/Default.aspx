<%@ Page Title="BRS" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BRSi._Default" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    <section class="featured">
        <div class="content-wrapper" style="background-image:url('images/big_img.jpg');background-repeat:no-repeat;background-position:center">
            <hgroup class="title">
                <h1><%: Title %>.</h1>
                <h2>Backup Remote Service</h2>
            </hgroup>
            <p></p>
        </div>
    </section>
</asp:Content>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <asp:Panel ID="PnlLogin" runat="server" Visible="false">
    <h3>Opzioni disponibili:</h3>
        <ol class="round">
            <li class="one">
                <h2>Pannello di monitoraggio</h2>            
                <a href="/panel.aspx">Clicca qui</a> per visualizzare il pannello di monitoraggio
            </li>
            <li class="two">
                <h2>Configurazione</h2>
                Se è a prima volta che accedi verifica se esiste una configurazione. In caso contrario è necessario configurare alcuni parametri nella
                <a href="/configuration.aspx">Pagina di configurazione</a>
            </li>
            <li class="three">
                <h2>Schedulazioni</h2>
                Gestisci le schedulazioni dei trasferimenti (S)FTP programmati                
                <a href="/Scheduled.aspx">Gestione Schedulazioni</a>
            </li>
            <li class="four">
                <h2>Trasferimenti FTP</h2>
                Avvia Trasferimento (S)FTP  di files                
                <a href="/Transferts.aspx">Gestione Trasferimenti</a>
            </li>
        </ol>
    </asp:Panel>
    <asp:Panel ID="PnlLogout" runat="server" Visible="true" style="text-align:left;height:auto">
        <h3>Selezionare la Partizione</h3>
        <div>
            <ol class="round">
                <li class="one">
                    <h2>iSeries</h2>
                    <a href="/Account/Login.aspx">Accedi a iSeries</a>
                </li>
               <%-- <li class="two">
                    <h2>Blade</h2>
                  <a href="/partitions/perseo/Account/Login.aspx">Accedi a Perseo</a>
                </li>
                <li class="three">
                    <h2>Altra</h2>
                    <a href="/partitions/Test3/Account/Login.aspx">Accedi a Altra</a>
                </li>
                <li class="four">
                    <h2>Altra</h2>
                    <a href="/partitions/Test4/Account/Login.aspx">Accedi a Altra</a>
                </li>--%>
            </ol>
        </div>
        <div style="background-color:#C6C6C6">
        <img src="Images/soluzioni.PNG" />
        </div>
    </asp:Panel>
</asp:Content>
