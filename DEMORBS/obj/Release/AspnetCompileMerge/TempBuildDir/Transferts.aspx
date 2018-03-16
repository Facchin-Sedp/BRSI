<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Transferts.aspx.cs" Inherits="BRSi.Transferts" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
        <section id="StartPage" class="featured">
        
        <div id="errorMessage" class="ui-widget" style="display:none">
	        <div class="ui-state-error ui-corner-all" style="padding: 0 .7em;">
		        <p id="message"><span class="ui-icon ui-icon-alert" style="float: left; margin-right: .3em;"></span>
		        <strong>Alert:</strong> Sample ui-state-error style.</p>
	        </div>
        </div>
        <div id="iseriesdata" style="width:100%; display:none"> 
        <div id="iSeriesValueDIV" style="width:100%;background-color:#F57A01;font-size:18px"></div>
        <div id="menu" style="text-align:right; float:right;width:100%;background-color:#F57A01">

            <a href="Scheduled.aspx" style="color: white; font-weight: bold; font-size:14px">Schedulazioni</a>
            <a href="Configuration.aspx" style="color: white; font-weight: bold; font-size:14px">Configurazione</a>
            <a href="#msgQueue" style="color:white;font-weight:bold; font-size:14px">Messaggi Coda PMBRS</a>  

        </div>

        </div>
    </section>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <link href="BOOTSTRAP/css/bootstrap.min.css" rel="stylesheet" />
    <script src="BOOTSTRAP/js/bootstrap.min.js"></script>


    <div class="container container-fluid">

        <div class="row">
            <div class="col-lg-6">
                <input class="form-control" value="" placeholder="Inserisci nome file" />
                
            </div>
            <div class="col-lg-6">
                <button class="btn btn-success">Avvia Trasferimento</button>
            </div>
        </div>
    </div>

</asp:Content>
