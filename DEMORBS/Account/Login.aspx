<%@ Page Title="Accesso" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="BRSi.Account.Login" %>
 

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">


    <script src="../Scripts/js/jquery-1.8.3.js"></script>
    <script src="../Scripts/js/jquery-ui-1.9.2.custom.js"></script>
    <script src="../Scripts/js/jquery-ui-1.9.2.custom.min.js"></script>
    <script src="../Scripts/Roundabout/jquery.roundabout.js"></script>
    <link href="../BOOTSTRAP/css/bootstrap.min.css" rel="stylesheet" />
    <style>
 

        .form-login {
            background-color: #FFFFFF;
            padding-top: 10px;
            padding-bottom: 20px;
            padding-left: 20px;
            padding-right: 20px;
            border-radius: 15px;
            border-color: #d2d2d2;
            border-width: 5px;
            box-shadow: 0 1px 0 #FFFFFF;
        }

        h4 {
            border: 0 solid #fff;
            border-bottom-width: 1px;
            padding-bottom: 10px;
            text-align: center;
        }

        .form-control {
            border-radius: 10px;
        }

        .wrapper {
            text-align: center;
        }
    </style>
    <script>
        $(document).ready(function () {
            $("#ulbrand").roundabout({
                autoplay: true,// gira in automatico
                autoplayDuration: 5000,// ogni 3 secondi
                autoplayPauseOnHover: false// con mousover si ferma in pausa
            });

            $("#btnLogin").click(function (e) {
                e.preventDefault();

                LoginServer();

            });

        });

        function LoginServer() {

            var UserName = $("#userName").val();
            var Password = $("#Password").val();

            var command = {UserName:UserName,Password:Password};

            var jsonData = JSON.stringify(command);

            $.ajax({
            type: "POST",
                url: 'Login.aspx/LoginServer',
            data:jsonData,
            dataType: "json",
            contentType: "application/json; charset=utf-8",

            success: function (res) {
                var data = res.d;

                var url = $.parseJSON(data);

                if (url.indexOf("Errore:") == -1)
                    window.location.replace(url);
                else
                    alert(url);
            

            },
            error: function (response) {

                //alert("Err: "+response.responseText);

            },
            cache: false
    });


            

        }

    </script>



    <div class="container">

        <div class="row">
            <div class="col-md-offset-5  col-md-3">
                <div class="form-login text-center">
                    <h2 id="headerTitolo" runat="server" class="label label-primary">Utilizzare un account iSeries per eseguire l'accesso.</h2>
                    <hr />
                    <input type="text" id="userName" class="form-control input-sm chat-input" placeholder="username" />
                    <br>

                    <input type="password" id="Password" class="form-control input-sm chat-input" placeholder="password" />
                    <br>

                    <div class="wrapper">
                        <span class="group-btn">
                            <button id="btnLogin" class="btn btn-success">login <i class="glyphicon glyphicon-log-in"></i></button>
                        </span>
                    </div>
                </div>

            </div>
        </div>
        
        <div class="row" style="min-height:100px;margin-top:100px">
            <div class="col-md-offset-3 col-md-5">
                
                    <ul id="ulbrand" style="list-style: none">
                        <li>
                            <img src="/images/brand/logo_tango.png" style="width: 100px; height: 100px" alt="Tango04" /></li>
                        <li>
                            <img src="/images/brand/cbt.png" alt="Cosmic Blue Team" style="width: 200px; height: 50px" /></li>
                        <li>
                            <img src="/images/brand/ibm.png" style="width: 170px; height: 100px" alt="IBM" /></li>
                        <li>
                            <img src="/images/brand/logo-QSL-GROUP.png" style="width: 200px; height: 80px" alt="QSL" /></li>
                        <li>
                            <img src="/images/brand/acg.png" alt="ACG IBM" /></li>

                    </ul>
                
            </div>
        </div>

    </div>
   
<%--    <section id="loginForm" style="width: 50%">


        <asp:UpdatePanel runat="server" ID="updpnllogin">
            <ContentTemplate>
                <h2 id="headerTitolo" runat="server">Utilizzare un account iSeries per eseguire l'accesso.</h2>
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
                            <asp:Button runat="server" CommandName="Login" Text="Accedi" ID="BtnLogin" CssClass="btn btn-success" />

                            <asp:UpdateProgress ID="ProgressLogin" runat="server" AssociatedUpdatePanelID="updpnllogin">
                                <ProgressTemplate>
                                    <img src="../Images/wait.gif" style="width: 32px; height: 32px" />Login in corso...
                                </ProgressTemplate>
                            </asp:UpdateProgress>


                        </fieldset>
                    </LayoutTemplate>
                </asp:Login>
            </ContentTemplate>
        </asp:UpdatePanel>

    </section>--%>






</asp:Content>
