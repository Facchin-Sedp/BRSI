<%@ Page Title="Accesso" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="BRSi.Account.Login" %>
<%@ Register Src="~/Account/OpenAuthProviders.ascx" TagPrefix="uc" TagName="OpenAuthProviders" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">

    <script src="/Scripts/Roundabout/jquery.roundabout.js"></script>
    <script type="text/javascript" src="scripts/js/jquery-1.8.3.js"></script>
    <script type="text/javascript" src="scripts/js/jquery-ui-1.9.2.custom.js"></script>
    <script type="text/javascript" src="scripts/js/jquery-ui-1.9.2.custom.min.js"></script>
	<script>
	    $(document).ready(function () {
	        $("#ulbrand").roundabout({
	            autoplay: true,// gira in automatico
	            autoplayDuration: 3000,// ogni 3 secondi
	            autoplayPauseOnHover:true// con mousover si ferma in pausa
	        });
	    });
	</script>

    <hgroup class="title">
        <h1><%: Title %>.</h1>
    </hgroup>


            <section id="loginForm" style="width:50%">


                <asp:UpdatePanel runat="server" ID="updpnllogin">
                <ContentTemplate>
                <h2>Utilizzare un account iSeries per eseguire l'accesso.</h2>
                <asp:Login runat="server" ViewStateMode="Disabled" RenderOuterTable="false" ID="Login1" OnAuthenticate="Login1_Authenticate" OnLoggedIn="Login1_LoggedIn">
                    <LayoutTemplate>
                        <p class="validation-summary-errors">
                            <asp:Literal runat="server" ID="FailureText" />
                        </p>
                        <fieldset>
                            <legend>Form di accesso</legend>
                            <ol>
                                <li>
                                    <asp:Label runat="server" AssociatedControlID="UserName">Nome utente</asp:Label>
                                    <asp:TextBox runat="server" ID="UserName" />
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="UserName" CssClass="field-validation-error" ErrorMessage="Il campo Nome utente è obbligatorio." />
                                </li>
                                <li>
                                    <asp:Label runat="server" AssociatedControlID="Password">Password</asp:Label>
                                    <asp:TextBox runat="server" ID="Password" TextMode="Password" />
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="Password" CssClass="field-validation-error" ErrorMessage="Il campo Password è obbligatorio." />
                                </li>
                                <li>
                                    <asp:CheckBox runat="server" ID="RememberMe" />
                                    <asp:Label runat="server" AssociatedControlID="RememberMe" CssClass="checkbox">Memorizza account</asp:Label>
                                </li>
                            </ol>
                            <asp:Button runat="server" CommandName="Login" Text="Accedi" ID="BtnLogin"/>
  
                            <asp:UpdateProgress ID="ProgressLogin" runat="server" AssociatedUpdatePanelID="updpnllogin">
                                <ProgressTemplate>
                                    <img src="../Images/wait.gif" style="width:32px;height:32px"/>Login in corso...
                                </ProgressTemplate>
                            </asp:UpdateProgress>                  
                             

                        </fieldset>
                    </LayoutTemplate>
                </asp:Login>
            </ContentTemplate>
            </asp:UpdatePanel> 
 
            </section>

            <div id="brand" style="float:left; width:30%;padding-left:10%;padding-top:10%">
		        <ul id="ulbrand" style=" list-style:none">
			        <li><img src="/images/brand/logo_tango.png" style="width:100px;height:100px" alt="Tango04" /></li>
			        <li><img src="/images/brand/cbt.png" alt="Cosmic Blue Team" style="width:200px;height:50px"/></li>
			        <li><img src="/images/brand/ibm.png" style="width:170px;height:100px" alt="IBM" /></li>
			        <li><img src="/images/brand/logo-QSL-GROUP.png"  style="width:200px;height:80px" alt="QSL" /></li>
			        <li><img src="/images/brand/acg.png" alt="ACG IBM" /></li>
 
		        </ul>
            </div>

	 


</asp:Content>
