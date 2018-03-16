<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Panel.aspx.cs" Inherits="BRSi.Panel" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <section id="StartPage" class="featured">
        <script type="text/javascript" src="Scripts/rbsScript.js"></script>  
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

 
    <%-------------------------------------------------------------------------%> 
        
    <%-- Link a script e style kendo --%>
    <link href="KENDO/content/shared/styles/examples-offline.css" rel="stylesheet">
    <link href="KENDO/styles/kendo.common.min.css" rel="stylesheet">
    <link href="KENDO/styles/kendo.default.min.css" rel="stylesheet">
    <script src="KENDO/js/jquery.min.js"></script>
    <script src="KENDO/js/kendo.web.min.js"></script>
    <script src="KENDO/content/shared/js/console.js"></script>
    <script src="Scripts/rbsScript.js"></script>
    <%-------------------------------------------------------------------------%> 

    <%-- Link a script chart jquery --------------------------------------------%>
    <script type="text/javascript" src="Scripts/jshighchart/js/highcharts.js"></script>
    <%-------------------------------------------------------------------------%> 


  <%-----------------Style Table-----------------------------%>   
 
    <link href="Content/table.css" rel="stylesheet" />


    <%-----------------Style OVERLAY DIV-----------------------------%> 
    <style>
    .chiudi{ font-size:18px; color:#888; font-weight:bold; position:absolute; right:2%; top:0%;  cursor:pointer;} 
    .overlay
    {
        background:#000;
        position:fixed;
        top:0px;
        bottom:0px;
        left:0px;
        right:0px;
        z-index:100;
        cursor:pointer;
        /*Trasperenza cross browser*/
        opacity: .7; filter: alpha(opacity=60);
        -ms-filter:"progid:DXImageTransform.Microsoft.Alpha(Opacity=60)";   
    }
 
    #box{ width:1000px; height:600px; background-color:#FFF; display:none; z-index:+300; position:absolute; left:15%; top:15%; -moz-border-radius: 15px;  -webkit-border-radius: 15px;
    border-radius: 15px;}
    #editor{width:95%; height:100%; overflow:auto;float:left}

    </style>
    <%-------------------------------------------------------------------------%> 
    <%-- Link a script custom RBS --------------------------------------------%>
    <script type="text/javascript" src="Scripts/rbsScript.js"></script>   
    <%-------------------------------------------------------------------------%>

   
    <div id="arrow" style="float:left;width:100%;height:100%;vertical-align:middle;text-align:center;background-image:url('Images/arrow_grey_sfoc.png');background-repeat:no-repeat">
        <div id="DivAlertJobs" style="float:left;width:35%;font-size:20px;font-weight:bold;text-align:center"><asp:Image ID="ImgStatJobs" runat="server" ImageUrl="~/Images/yellow_16.png" /> JOBS MONITOR MSGW</div> 
        <div id="DivAlertFtp" style="float:right;width:40%;font-size:20px;font-weight:bold;text-align:center"><asp:Image ID="ImgStatFtp" runat="server" ImageUrl="~/Images/yellow_16.png" /> REMOTE DESTINATION </div>
        <div id="GridJobs" style="float:left;width:49%"></div>
         

        <div id="GridFTPList" style="float:right;width:50%;height:60%"></div>

        <div id="chartCPU" style="width:35%;height:200px;float:left;padding-top:15px;padding-bottom:10px"></div>
        <div id="ChartASP" style="width:13%;height:200px;float:left;padding-left:13px;padding-top:15px;padding-bottom:0px"></div>
        <div id="ChartJobs" style="width:35%;height:200px;float:left;padding-top:10px;padding-bottom:10px" ></div>
        <div id="ChartUsers" style="width:13%;height:200px;float:left;padding-left:13px;padding-top:10px;padding-bottom:0px"></div>
    
        <div id="title" style="width:100%;height:auto;float:left; clear:left;text-align:left;font-size:20px;font-weight:bold">
            <a name="msgQueue" onclick="scrollwinUp()"><img id="upMsgq"src ="Images/up18px.png" style="border-width:0px"/></a>CODA MESSAGGI PMBRS
        </div>
        
        <div id ="divStatusCoda" style="float:right;width:auto"><asp:Image ID="ImgStatCoda" runat="server" ImageUrl="~/Images/yellow_16.png" /></div>
        <div id="DivData" style="float:right;width:auto;clear:left">   <input id ="datetimepicker" /></div>

        <div id="GridMsgCoda" style="float:left;width:100%;background-color:white"></div>
 
        <div class="console" style="width:300px;height:100px;float:left;left:0;visibility:collapse"></div> 
    </div> 
   <%-- Ligth Box JQUERY OVERLAY --------------------------------------------%>
    <div class="overlay" id="overlay" style="display:none;"></div>
    <div id="box">
        <h1 id="titoloOverlay" class="titolo_box" style="text-align:center;color:#808080">Content</h1>
        <%--<p class="testo-box">Message:</p>--%>
              
        <div id="editor" style="float:left;width:100%;background-color:white"></div>
                
        <hr />
        <p class="chiudi" onclick="chiudiOverlay()">X</p>
    </div>  
    <!--fine box-->
    

</asp:Content>
