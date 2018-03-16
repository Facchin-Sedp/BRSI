<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Scheduled.aspx.cs" Inherits="BRSi.Scheduled" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">

    <div id="menu" style="text-align:right; float:right;width:100%;background-color:#F57A01">
        <label id="lblPartizione" style="color: white; font-weight: bold; font-size: 14px;text-align: left;float:left"></label>
        <a href="default.aspx" style="color: white; font-weight: bold; font-size:14px">Home</a>
        <a href="Configuration.aspx" style="color: white; font-weight: bold; font-size:14px">Configurazione</a>
        <a href="Panel.aspx" style="color: white; font-weight: bold; font-size:14px">Monitors</a> 
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

     <script type="text/javascript" src="scripts/js/jquery-1.8.3.js"></script>
    <script type="text/javascript" src="scripts/js/jquery-ui-1.9.2.custom.js"></script>
    <script type="text/javascript" src="scripts/js/jquery-ui-1.9.2.custom.min.js"></script>    
    <link type="text/css" href="Scripts/css/ui-lightness/jquery-ui-1.9.2.custom.css" rel="Stylesheet" />

    <link href="Content/switch.css" rel="stylesheet" />
    <script src="Scripts/timepicker/jquery.ui.timepicker.js"></script>
    <link href="Scripts/timepicker/jquery.ui.timepicker.css" rel="stylesheet" />
    <script src="Scripts/Schedule.js"></script>

  
    
    
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <img id="Waitpages" runat="server" src="../Images/wait.gif" style="width:24px;height:24px" visible="false" />
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="BtnNext" />
        <asp:AsyncPostBackTrigger ControlID="BtnPrev" />
    </Triggers>
</asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdPnl" runat="server">
        <ContentTemplate>
            <h3>SALVATAGGIO PARAMETRI DI SISTEMA</h3>
            <div id="div-ComandiSAV" style="float: left; padding-right: 10px; width: 100%">
                <h3 id="header-savsys">Parametri Sistema</h3>
                <div>
                    <div style="float: left; padding-right: 10px; width: 100%">

                        <h4>utenti e autorizzazioni</h4>
                        <div id="SYFL1" class="bool-slider false" style="float: left; clear: left">
                            <div class="inset">
                                <div id="sYFL1" class="control"></div>
                            </div>
                        </div>

                    </div>
                  

                    <div style="float: left; padding-right: 10px; width: 100%">
                         <h4>Configurazioni sistema </h4>
                        <div id="SYFL2" class="bool-slider false" style="float: left; clear: left">
                            <div class="inset">
                                <div id="sYFL2" class="control"></div>
                            </div>
                        </div>
                    </div>
                    <div style="float: left; padding-right: 10px; width: 100%">
                      <h4>Ora</h4> 
                   
                        <div>
                            <input type="text" style="width: 40px;font-size:medium" id='timepicker-SavSys' 
                                class="ui-timepicker" value="00:00" />
                        </div>
                    </div>

                    <div style="float: left; width: 100%; height: auto; clear: left">

                        <div id="BtnSaveSys"   style="float: right">Salva</div>

                        <div id='waitSaveSys' style="float: right">
                            <img src="Images/wait.gif" style="width: 24px; height: 24px; display: none" />
                        </div>

                    </div>
                </div>
            </div>

             <h3>SALVATAGGIO LIBRERIE</h3>

            <div id="div-saveLibs" style="float: left; padding-right: 10px; width: 100%;">
                <h3>Cerca Librerie</h3>
                <div>


                    <div id="div_ricerca" style="width: 100%">

                        <div>
                            <p>
                                <div id="Divsearch" style="float: left; width: 40%">
                                    <asp:TextBox ID="Txtsearch" runat="server" Style="width: 50%; padding-right: 10px"></asp:TextBox>
                                    <asp:Button ID="BtnSearch" runat="server" Text="Cerca" OnClick="BtnSearch_Click" />
                                    <asp:Label ID="lblNumLib" runat="server" Text=""></asp:Label>
                                </div>
                                <div id="DivCombo" style="float: left; width: 30%">
                                    <asp:DropDownList ID="DDLlibrary" runat="server"></asp:DropDownList>
                                    <asp:Button ID="BtnSel" runat="server" Text="Seleziona" OnClick="BtnSel_Click" />

                                </div>
                                <div id="Divpage" style="float: right">
                                    <asp:Button ID="BtnPrev" runat="server" Text="Previous" OnClick="BtnPrev_Click" />
                                    <asp:Button ID="BtnNext" runat="server" Text="Next" OnClick="BtnNext_Click" /><br />
                                    <asp:Label ID="Lblpage" runat="server" Text="page"></asp:Label>
                                </div>

                            </p>
                        </div>

                    </div>
                </div>
            </div>
                <div id="librerie" style="width: 100%; float: left">

                    <asp:Repeater ID="Repeater1" runat="server">
                        <ItemTemplate>
                            <h3 id="<%# Eval("nome") %>"><%# Eval("nome")  %> </h3>
                            <div>
                                <p id="par<%# Eval("nome")  %>">
                                    <strong><%# Eval("Descrizione") %> </strong>




                                    <div id="DivLibTitle" style="float: left; width: auto; height: auto; padding-right: 10px">Schedulazioni</div>

                                    <div style="float: left; padding-right: 10px; width: auto">
                                        <div id="sched1<%# Eval("nome") %>" class="bool-slider <%# Eval("scheds.enable1") %>" style="float: left">
                                            <div class="inset">
                                                <div id="Time1<%# Eval("nome") %>" class="control"></div>
                                            </div>
                                        </div>
                                        <div style="float: left; clear: left">
                                            <input type="text" style="width: 40px" id='timepicker1<%# Eval("nome") %>' class="ui-timepicker" value="<%# Eval("scheds.timeSched1") %>" />
                                        </div>
                                        <div id="type1<%# Eval("nome") %>" class="type-slider T" style="float: left; clear: left">
                                            <div class="inset">
                                                <div id="Type1<%# Eval("nome") %>" class="control"></div>
                                            </div>
                                        </div>


                                    </div>

                                    <div style="float: left; padding-right: 10px">
                                        <div id="sched2<%# Eval("nome") %>" class="bool-slider <%# Eval("scheds.enable2") %>" style="float: left">
                                            <div class="inset">
                                                <div id="Time2<%# Eval("nome") %>" class="control"></div>
                                            </div>
                                        </div>
                                        <div style="float: left; clear: left">
                                            <input type="text" style="width: 40px; display: none" id='timepicker2<%# Eval("nome") %>' class="ui-timepicker" value='<%# Eval("scheds.timeSched2") %>' />
                                        </div>
                                        <div id="type2<%# Eval("nome") %>" class="type-slider <%# Eval("scheds.tipo2") %>" style="float: left; clear: left; display: none">
                                            <div class="inset">
                                                <div id="Type2<%# Eval("nome") %>" class="control"></div>
                                            </div>
                                        </div>
                                    </div>

                                    <div style="float: left; padding-right: 10px">
                                        <div id="sched3<%# Eval("nome") %>" class="bool-slider <%# Eval("scheds.enable3") %>" style="float: left">
                                            <div class="inset">
                                                <div id="Time3<%# Eval("nome") %>" class="control"></div>
                                            </div>
                                        </div>
                                        <div style="float: left; clear: left">
                                            <input type="text" style="width: 40px; display: none" id='timepicker3<%# Eval("nome") %>' class="ui-timepicker" value='<%# Eval("scheds.timeSched3") %>' />
                                        </div>
                                        <div id="type3<%# Eval("nome") %>" class="type-slider <%# Eval("scheds.tipo3") %>" style="float: left; clear: left; display: none">
                                            <div class="inset">
                                                <div id="Type3<%# Eval("nome") %>" class="control"></div>
                                            </div>
                                        </div>


                                    </div>

                                    <div style="float: left; padding-right: 10px">
                                        <div id="sched4<%# Eval("nome") %>" class="bool-slider <%# Eval("scheds.enable4") %>" style="float: left">
                                            <div class="inset">
                                                <div id="Time4<%# Eval("nome") %>" class="control"></div>
                                            </div>
                                        </div>
                                        <div style="float: left; clear: left">
                                            <input type="text" style="width: 40px; display: none" id='timepicker4<%# Eval("nome") %>' class="ui-timepicker" value='<%# Eval("scheds.timeSched4") %>' />
                                        </div>
                                        <div id="type4<%# Eval("nome") %>" class="type-slider <%# Eval("scheds.tipo4") %>" style="float: left; clear: left; display: none">
                                            <div class="inset">
                                                <div id="Type4<%# Eval("nome") %>" class="control"></div>
                                            </div>
                                        </div>

                                    </div>

                                    <div style="float: left; padding-right: 10px">
                                        <div id="sched5<%# Eval("nome") %>" class="bool-slider <%# Eval("scheds.enable5") %>" style="float: left">
                                            <div class="inset">
                                                <div id="Time5<%# Eval("nome") %>" class="control"></div>
                                            </div>
                                        </div>
                                        <div style="float: left; clear: left">
                                            <input type="text" style="width: 40px; display: none" id='timepicker5<%# Eval("nome") %>' class="ui-timepicker" value='<%# Eval("scheds.timeSched5") %>' />
                                        </div>
                                        <div id="type5<%# Eval("nome") %>" class="type-slider <%# Eval("scheds.tipo5") %>" style="float: left; clear: left; display: none">
                                            <div class="inset">
                                                <div id="Type5<%# Eval("nome") %>" class="control"></div>
                                            </div>
                                        </div>

                                    </div>

                                    <div style="float: left; padding-right: 10px">
                                        <div id="sched6<%# Eval("nome") %>" class="bool-slider <%# Eval("scheds.enable6") %>" style="float: left">
                                            <div class="inset">
                                                <div id="Time6<%# Eval("nome") %>" class="control"></div>
                                            </div>
                                        </div>
                                        <div style="float: left; clear: left">
                                            <input type="text" style="width: 40px; display: none" id='timepicker6<%# Eval("nome") %>' class="ui-timepicker" value='<%# Eval("scheds.timeSched6") %>' />
                                        </div>
                                        <div id="type6<%# Eval("nome") %>" class="type-slider <%# Eval("scheds.tipo6") %>" style="float: left; clear: left; display: none">
                                            <div class="inset">
                                                <div id="Type6<%# Eval("nome") %>" class="control"></div>
                                            </div>
                                        </div>

                                    </div>

                                    <div style="float: left; padding-right: 10px">
                                        <div id="sched7<%# Eval("nome") %>" class="bool-slider <%# Eval("scheds.enable7") %>" style="float: left">
                                            <div class="inset">
                                                <div id="Time7<%# Eval("nome") %>" class="control"></div>
                                            </div>
                                        </div>
                                        <div style="float: left; clear: left">
                                            <input type="text" style="width: 40px; display: none" id='timepicker7<%# Eval("nome") %>' class="ui-timepicker" value='<%# Eval("scheds.timeSched7") %>' />
                                        </div>
                                        <div id="type7<%# Eval("nome") %>" class="type-slider <%# Eval("scheds.tipo7") %>" style="float: left; clear: left; display: none">
                                            <div class="inset">
                                                <div id="Type7<%# Eval("nome") %>" class="control"></div>
                                            </div>
                                        </div>

                                    </div>

                                    <div style="float: left; padding-right: 10px">
                                        <div id="sched8<%# Eval("nome") %>" class="bool-slider <%# Eval("scheds.enable8") %>" style="float: left">
                                            <div class="inset">
                                                <div id="Time8<%# Eval("nome") %>" class="control"></div>
                                            </div>
                                        </div>
                                        <div style="float: left; clear: left">
                                            <input type="text" style="width: 40px; display: none" id='timepicker8<%# Eval("nome") %>' class="ui-timepicker" value='<%# Eval("scheds.timeSched8") %>' />
                                        </div>
                                        <div id="type8<%# Eval("nome") %>" class="type-slider <%# Eval("scheds.tipo8") %>" style="float: left; clear: left; display: none">
                                            <div class="inset">
                                                <div id="Type8<%# Eval("nome") %>" class="control"></div>
                                            </div>
                                        </div>

                                    </div>

                                    <div style="float: left; padding-right: 10px">
                                        <div id="sched9<%# Eval("nome") %>" class="bool-slider <%# Eval("scheds.enable9") %>" style="float: left">
                                            <div class="inset">
                                                <div id="Time9<%# Eval("nome") %>" class="control"></div>
                                            </div>
                                        </div>
                                        <div style="float: left; clear: left">
                                            <input type="text" style="width: 40px; display: none" id='timepicker9<%# Eval("nome") %>' class="ui-timepicker" value='<%# Eval("scheds.timeSched9") %>' />
                                        </div>
                                        <div id="type9<%# Eval("nome") %>" class="type-slider <%# Eval("scheds.tipo9") %>" style="float: left; clear: left; display: none">
                                            <div class="inset">
                                                <div id="Type9<%# Eval("nome") %>" class="control"></div>
                                            </div>
                                        </div>

                                    </div>


                                    <div style="float: left; width: 100%; height: auto; clear: left">

                                        <div id="BtnSaveSched<%# Eval("nome") %>" class="btnSaveSched" style="float: right">Salva</div>

                                        <div id='wait<%# Eval("nome") %>' style="float: right">
                                            <img src="Images/wait.gif" style="width: 24px; height: 24px" />
                                        </div>

                                    </div>


                                </p>
                            </div>

                        </ItemTemplate>

                    </asp:Repeater>

                </div>




            

               
            


            <div id='EventMessage' class="ui-widget" style="display: none; float: left; clear: left; width: 100%; position: absolute; z-index: 10">
                <div class="ui-state-highlight ui-corner-all" style="padding: 0 .7em;">
                    <p id='message'>
                        <span class="ui-icon ui-icon-info" style="float: left; margin-right: .3em;"></span>
                        <strong>Alert:</strong> Sample ui-state-error style.
                    </p>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>



</asp:Content>
