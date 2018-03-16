<%@ Page Title="Contatto" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="BRSi.Contact" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <hgroup class="title">
        <h1><%: Title %></h1><br />
        <h2>Soluzioni EDP</h2>
    </hgroup>

    <section class="contact">
        <header>
            <h3>Telefono:</h3>
        </header>
        <p>
            <span class="label">Principale:</span>
            <span>0161.56922</span>
        </p>
 
    </section>

    <section class="contact">
        <header>
            <h3>Posta elettronica:</h3>
        </header>
        <p>
            <span class="label">Supporto:</span>
            <span><a href="mailto:sedpdistribution@soluzioniedp.it">Support@example.com</a></span>
        </p>
        <p>
            <span class="label">Generale:</span>
            <span><a href="mailto:info@soluzioniedp.it">General@example.com</a></span>
        </p>
    </section>

    <section class="contact">
        <header>
            <h3>Indirizzo:</h3>
        </header>
        <p>
            viale Garibaldi, 51<br />
            13100 Vercelli
        </p>
    </section>
</asp:Content>