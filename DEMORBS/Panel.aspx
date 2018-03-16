<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Panel.aspx.cs" Inherits="BRSi.Panel" %>


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
    
    <%-------------------------------------------------------------------------%> 
        
    <%-- Link a script e style kendo --%>
    
     
    <link href="KENDO/styles/kendo.common.min.css" rel="stylesheet">
    <link href="KENDO/styles/kendo.default.min.css" rel="stylesheet">
    <script src="KENDO/js/jquery.min.js"></script>
    <script src="KENDO/js/kendo.web.min.js"></script>
    <script src="KENDO/content/shared/js/console.js"></script>


 

    <script src="BOOTSTRAP/js/bootstrap.min.js"></script>
   
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
    
    thead tr th{
        font-size:x-small
    }
    tbody tr td{
        font-size:x-small;
    }

    </style>
    <%-------------------------------------------------------------------------%> 
 
    <div>
        <div  class="container container-fluid">
 
        <div class="row">

            <div class="col-lg-6">
                <div class="row" style="padding-right:2px">
                    <div class="col-lg-12">
                        <div class="row" >
                            <div class="col-lg-12"  style="min-height: 74px">
                                <h4>JOBS MONITOR MSGW
                                    <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/yellow_16.png" /></h4>

                            </div>
                        </div>
                        <div class="row">
                            <div id="GridJobs" style="min-height:200px;overflow-y:scroll;background-color:white">
                                <table id="table-JOBEDD" class="table table-bordered table-hover" style="background-color: white; margin-top: 0px">
                                    <thead>
                                        <tr>
                                            <th>Subsystem</th>
                                            <th>Name</th>
                                            <th>Number</th>
                                            <th>Status</th>
                                            <th>ActiveStatus</th>
                                            <th>User</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                    </tbody>

                                </table>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-lg-9" style="padding-left:0px;">
                        <div id="chartCPU" style="height: 200px; padding-bottom: 5px; padding-top: 5px;"></div>
                    </div>
                    <div class="col-lg-3">
                        <div id="ChartASP" style="height: 200px; padding-bottom: 5px; padding-top: 5px;"></div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-lg-9"  style="padding-left:0px;">
                        <div id="ChartJobs" style="height: 200px;"></div>
                    </div>
                    <div class="col-lg-3">
                        <div id="ChartUsers" style="height: 200px;"></div>
                    </div>
                </div>

            </div>

            <div class="col-lg-6">
                <div class="row">
                    <div class="col-lg-12">
                        <h4>REMOTE DESTINATION <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/yellow_16.png" /></h4>
                        <div id="btnRefresh" class="btn btn-success"><i class="glyphicon glyphicon-refresh"></i></div>
                        <div class="btn-group">
                            <button id="btnSearch" class="btn btn-info">Cerca LIB</button>
                            <button id="btnSearch-IFS" class="btn btn-primary">Cerca IFS</button>
                           <!-- <button id="btnSearch-QDLS" class="btn btn-warning">Cerca QDLS</button>-->
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div id="GridFTPList" style="font-size: small;overflow-y:scroll;height:600px;background-color:white">
                        <table id="table-FTP_File" class="table table-bordered table-hover" style=" margin-top:0px">
                            <thead>
                                <tr>
                                    <th>Name</th>
                                    <th>CreationTime</th>
                                    <th>Size</th>
                                </tr>
                            </thead>
                            <tbody>

                            </tbody>

                        </table>

                    </div>
                </div>
            </div>


        </div>
            <hr />
        <div class="row">
            <div class="col-lg-12">
                <div id="title" style="width: 100%; height: auto; float: left; clear: left; text-align: left; font-size: 20px; font-weight: bold">
                    <a name="msgQueue" onclick="scrollwinUp()">
                        <img id="upMsgq" src="Images/up18px.png" style="border-width: 0px" /></a>CODA MESSAGGI PMBRS
                </div>
                <div id ="divStatusCoda" style="float:right;width:auto"><asp:Image ID="ImgStatCoda" runat="server" ImageUrl="~/Images/yellow_16.png" />
                    <button id="btnMsgQError" style="font-size: small; margin: 0,0,0,0; border-width: 0; border-radius: 4px; background-color: red; color: white" onclick="get_Message_by_Severity('10')">Errors</button>
                    <button id="btnMsgQAll" style="font-size: small; margin: 0,0,0,0; border-width: 0; border-radius: 4px; background-color: green; color: white" onclick="get_Message_by_Severity('0')">All</button>
                </div>
                <div id="DivData" style="float:right;width:auto;clear:left">   <input id ="datetimepicker" /></div>

                <div id="GridMsgCoda" style="float:left;width:100%;background-color:white"></div>
            </div>
        </div>
        <div class="console" style="width:300px;height:100px;float:left;left:0;visibility:collapse"></div> 
    </div>
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
    
     <!-- Modal RICERCA -->
    <div id="modal-RICERCA" class="modal fade " role="dialog" style="z-index: 100000">

        <div class="modal-dialog" style="width: 60%; height: 100%">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <h4 id="Titolo-RICERCA" class="modal-title">SEARCH ITEMS</h4>
                </div>
                <div class="modal-body">
                    <div class="row">

                        <div class="col-lg-4">
                            <label>Testo libero</label>
                            <input id="txtRicerca" type="text" class="form-control" />

                        </div>
                        <div class="col-lg-4">
                            <label>Libreria</label>
                            <input id="txtLibRicerca" type="text" class="form-control" />

                        </div>
                        <div class="col-lg-4">
                            <label>Tipo Oggetto</label>
                            <input id="txtTipoRicerca" type="text" class="form-control" />

                        </div>
                    </div>
                    <div class="row  text-right">
                        <div class="col-lg-1">
                            <img id="imgWait" style="display: none" src="Images/wait.gif" />
                        </div>
                        <div class="col-lg-11">

                            <button id="btnSearchItems" class="btn btn-success">Cerca</button>
                        </div>

                    </div>
                    <div class="row">
                        <div class="col-lg-12">
                            <h5>Results:</h5>
                            <table class="table table-bordered table-condensed table-hover" id="tableSearch">
                                <!---tabella risultati---->
                            </table>
                        </div>
                    </div>

                </div>

                <div class="modal-footer">
                    <button id="btnSearchClose" type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>


        </div>

    </div>


         <!-- Modal RICERCA IFS -->
    <div id="modal-RICERCA-IFS" class="modal fade " role="dialog" style="z-index: 100000">

        <div class="modal-dialog" style="width: 60%; height: 100%">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <h4 id="Titolo-RICERCA-IFS" class="modal-title">SEARCH IFS ITEMS</h4>
                </div>
                <div class="modal-body">
                    <div class="row">

                        <div class="col-lg-4">
                            <label>Testo libero</label>
                            <input id="txtRicerca-IFS" type="text" class="form-control" />

                        </div>
                        <div class="col-lg-4">
                            <label>Oggetto</label>
                            <input id="txtObjRicerca-IFS" type="text" class="form-control" />

                        </div>
                        <div class="col-lg-4">
                            <label>Tipo Oggetto</label>
                            <input id="txtTipoRicerca-IFS" type="text" class="form-control" />

                        </div>
                    </div>
                    <div class="row  text-right">
                        <div class="col-lg-1">
                            <img id="imgWait-IFS" style="display: none" src="Images/wait.gif" />
                        </div>
                        <div class="col-lg-11">

                            <button id="btnSearchItems-IFS" class="btn btn-success">Cerca</button>
                        </div>

                    </div>
                    <div class="row">
                        <div class="col-lg-12">
                            <h5>Results:</h5>
                            <table class="table table-bordered table-condensed table-hover" id="tableSearch-IFS">
                                <!---tabella risultati---->
                            </table>
                        </div>
                    </div>

                </div>

                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>


        </div>




    </div>



             <!-- Modal RICERCA QDSL -->
    <div id="modal-RICERCA-QDLS" class="modal fade " role="dialog" style="z-index: 100000">

        <div class="modal-dialog" style="width: 60%; height: 100%">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <h4 id="Titolo-RICERCA-QDLS" class="modal-title">SEARCH QDLS ITEMS</h4>
                </div>
                <div class="modal-body">
                    <div class="row">

                        <div class="col-lg-4">

                            <label>Tipo Oggetto</label>
                            <select id="comboTipo" class="form-control selectpicker">
                                <option selected value='0'>Tutto</option>
                                <option value='1'>Cartella</option>
                                <option value='2'>Documento</option>
                            </select>

                        </div>
                        <div class="col-lg-4">
                            <label>Oggetto</label>
                            <input id="txtObjRicerca-QDLS" type="text" class="form-control" />

                        </div>
                 
                    </div>
                    <div class="row  text-right">
                        <div class="col-lg-1">
                            <img id="imgWait-QDLS" style="display: none" src="Images/wait.gif" />
                        </div>
                        <div class="col-lg-11">

                            <button id="btnSearchItems-QDLS" class="btn btn-success">Cerca</button>
                        </div>

                    </div>
                    <div class="row">
                        <div class="col-lg-12">
                            <h5>Results:</h5>
                            <table class="table table-bordered table-condensed table-hover" id="tableSearch-QDLS">
                                <!---tabella risultati---->
                            </table>
                        </div>
                    </div>

                </div>

                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>


        </div>




    </div>

             <!-- Modal RICERCA ERRORI -->
    <div id="modal-RICERCA-ERRORILIB" class="modal fade " role="dialog" style="z-index: 100000">

        <div class="modal-dialog" style="width: 60%; height: 100%">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <h4  class="modal-title">Error Saved Libraries</h4>
                </div>
                <div class="modal-body">
  
                    <div class="row">
                        <div class="col-lg-12">
                            <h5>Results:</h5>
                            <table class="table table-bordered table-condensed table-hover" id="tableSearch-ERRORELIB">
                                <!---tabella risultati---->
                            </table>
                        </div>
                    </div>

                </div>

                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>


        </div>




    </div>





 <script src="Scripts/rbsScript.js?<%=DateTime.Now.Ticks.ToString()%>"></script>
</asp:Content>
