<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Configuration.aspx.cs" Inherits="BRSi.Configuration" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <div id="menu" style="text-align:right; float:right;width:100%;background-color:#F57A01">
        <label id="lblPartizione" style="color: white; font-weight: bold; font-size: 14px;text-align: left;float:left"></label>
        <a href="default.aspx" style="color: white; font-weight: bold; font-size:14px">Home</a>
        <a href="Panel.aspx" style="color: white; font-weight: bold; font-size:14px">Monitors</a> 
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
      <link href="BOOTSTRAP/css/bootstrap.min.css" rel="stylesheet" />
   <style>
       body {
           font-size:10px
       }
       .tableIFS {
    border-collapse: collapse;
    background-color:white;
    border-color:lightgray;
    }

    .tableIFS, th, td {
        border: 1px solid lightgray;
         text-align: center;
         padding: 5px;
    }
    tr:hover{background-color:#f5f5f5}
   </style>

    
    <link href="KENDO/styles/kendo.common.min.css" rel="stylesheet">
    <link href="KENDO/styles/kendo.default.min.css" rel="stylesheet">
    <script src="KENDO/js/jquery.min.js"></script>
    <script src="KENDO/js/kendo.web.min.js"></script>
    <script src="KENDO/content/shared/js/console.js"></script> 
    
    
        <%-- Link a script e style jquery ui --%>    
    <script type="text/javascript" src="scripts/js/jquery-1.10.2.min.js"></script>
  
    <script type="text/javascript" src="scripts/js/jquery-ui-1.9.2.custom.min.js"></script>    
    <link type="text/css" href="Scripts/css/ui-lightness/jquery-ui-1.9.2.custom.css" rel="Stylesheet" />
     
    <script src="scripts/Configuration.js"></script>
   
    <script src="BOOTSTRAP/js/bootstrap.min.js"></script>
    <%-------------------------------------------------------------------------%>
 
 

    <div id="accordion" style="width: 100%; height: 100%">

        <h3>Librerie da Salvare</h3>

        <div style="height: 450px">

            <div id="EventMessage" class="ui-widget" style="display: none; float: left; clear: left; width: 100%; position: absolute; z-index: 10">
                <div class="ui-state-highlight ui-corner-all" style="padding: 0 .7em;">
                    <p id="message">
                        <span class="ui-icon ui-icon-info" style="float: left; margin-right: .3em;"></span>
                        <strong>Alert:</strong> Sample ui-state-error style.
                    </p>
                </div>
            </div>

            <div id="DivLibTitle" style="float: left; width: 48%; height: auto">LIBRERIE UTENTE</div>
            <div id="DivSaveTitle" style="float: right; width: 48%; height: auto">LIBRERIE DA SALVARE</div>
            <div id="GridLibraries" style="float: left; width: 48%; height: auto"></div>
            <div id="gridSelection" style="float: right; width: 48%; height: auto; clear: right"></div>

            <div style="float: left; width: 100%; height: auto; clear: left">

                <div id="BtnSchedx" style="float: right">Schedulazioni</div>
                <div id="BtnReload" style="float: right">Verifica</div>
                <div id="BtnSaveLib" style="float: right">Salva</div>
                <div id="wait" style="float: right">
                    <img src="Images/wait.gif" style="width: 24px; height: 24px" />
                </div>
            </div>
        </div>
        <h3>Cartelle IFS</h3>
        <div id="div-IFS">

            <div id="MsgIFS" class="ui-widget" style="display: none; float: left; clear: left; width: 100%; position: absolute; z-index: 10">
                <div class="ui-state-highlight ui-corner-all" style="padding: 0 .7em;">

                    <p id="messageIFS">
                        <span class="ui-icon ui-icon-info" style="float: left; margin-right: .3em;"></span>
                        <strong>INFO:</strong> Sample ui-state-error style.
                    </p>

                </div>
            </div>

            <input id="IFSDir" type="text" value="Inserire Percorso IFS" />
            <div id="btnCheckIFS">Check</div>
            <div id="btnAddIFS">Aggiungi</div>
            <a  href="/scheduledIFS.aspx">vai alle Schedulazioni</a>


            <table id="tbl-IFS" class="tableIFS">
            </table>
            <div id="waitIFS" style="display: none">
                <img src="Images/wait.gif" style="width: 24px; height: 24px" />
                Attendi....
            </div>

        </div>

        <h3>Impostazioni trasferimento FTP</h3>
       

        <div id="settingFtp" class="row">
            <div id="MsgFTP" class="ui-widget" style="display: none; float: left; clear: left; width: 100%; position: absolute; z-index: 10">
                <div class="ui-state-highlight ui-corner-all" style="padding: 0 .7em;">

                    <p id="messageFtp">
                        <span class="ui-icon ui-icon-info" style="float: left; margin-right: .3em;"></span>
                        <strong>INFO:</strong> Sample ui-state-error style.
                    </p>

                </div>
            </div>
            <div id="div-ftp" class="col-lg-6">

                <asp:Table ID="Table1" Style="width: 100%" runat="server">
                    <asp:TableRow ID="TableRow1" runat="server" Visible="false">
                        <asp:TableCell ID="TableCell1" runat="server">CCSID:</asp:TableCell>
                        <asp:TableCell ID="TableCell2" runat="server">
                            <asp:TextBox ID="TxtCCSID" runat="server" ReadOnly="True" class=" form-control"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="TableRow3" runat="server">
                        <asp:TableCell ID="TableCell5" runat="server">SFTP:</asp:TableCell>
                        <asp:TableCell ID="TableCell6" runat="server">
                            <asp:CheckBox ID="checkSftp" runat="server" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="TableRow4" runat="server">
                        <asp:TableCell ID="TableCell7" runat="server">Chiave:</asp:TableCell>
                        <asp:TableCell ID="TableCell8" runat="server">
                            <asp:TextBox ID="TxtKey" runat="server" class=" form-control" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="TableRow2" runat="server">
                        <asp:TableCell ID="TableCell3" runat="server">Sistema Operativo:</asp:TableCell><asp:TableCell ID="TableCell4" runat="server">
                            <asp:DropDownList ID="DDLSisOp" runat="server" class=" form-control"></asp:DropDownList>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow runat="server">
                        <asp:TableCell runat="server">Indirizzo Ftp:</asp:TableCell><asp:TableCell runat="server">
                            <asp:TextBox ID="TxtIndirizzo" runat="server" class=" form-control"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow runat="server">
                        <asp:TableCell runat="server">Percorso:</asp:TableCell><asp:TableCell runat="server">
                            <asp:TextBox ID="TxtPath" runat="server" class=" form-control"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow runat="server">
                        <asp:TableCell runat="server">Porta:</asp:TableCell><asp:TableCell runat="server">
                            <asp:TextBox ID="TxtPort" runat="server" class=" form-control"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow runat="server">
                        <asp:TableCell runat="server">User:</asp:TableCell><asp:TableCell runat="server">
                            <asp:TextBox ID="TxtUser" runat="server" class=" form-control"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow runat="server">
                        <asp:TableCell runat="server">Password:</asp:TableCell><asp:TableCell runat="server">
                            <asp:TextBox ID="TxtPwd" runat="server" TextMode="Password" class=" form-control"></asp:TextBox>

                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>

                <div id="BtnTestFtp" style="float: left">Verifica FTP</div>
                <div id="BtnSaveFtp" style="display: none">Salva</div>
                <div id='waitftp' style="float: left">
                    <img src="Images/wait.gif" style="width: 24px; height: 24px" />
                </div>
            </div>
            <div class="col-lg-6">

                <asp:TableRow runat="server">

                    <asp:TableCell runat="server">Ritenzione log (giorni):</asp:TableCell><asp:TableCell runat="server">
                        <div class="row">
                            <div class="col-lg-9">
                                <asp:TextBox ID="TxtggRetention" runat="server" class="form-control"></asp:TextBox>
                            </div>
                            <div class="col-lg-2">
                                <div id="btnSaveggRet" class="btn btn-success">Salva</div>
                            </div>
                        </div>


                    </asp:TableCell>
                </asp:TableRow>
            </div>
        </div>

    </div>

</asp:Content>