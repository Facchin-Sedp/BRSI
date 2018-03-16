using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using com.ibm.as400.access;
using java.util;
using System.Web.Services;
using System.Web.Script.Serialization;

namespace BRSi.Account
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {


            headerTitolo.InnerText = "Login al server "+ ConfigurationManager.AppSettings["as400Server"];
        }
       
/*
        protected void Login1_Authenticate(object sender, AuthenticateEventArgs e)
        {
            AS400 server = new AS400();
            Boolean isc = false;

            server.setSystemName(ConfigurationManager.AppSettings["as400Server"]);
            server.setUserId(Login1.UserName); //user del login
            server.setPassword(Login1.Password); //pwd del login

            try { server.connectService(0); } //se si connette allora l'utente di login esiste ed entro
            catch { isc = false; }

            isc = server.isConnected();
            //isc = true;
            if (isc)
            {
                e.Authenticated = true;
                //creo la connessione all'as400 da usare dopo per il monitoring
                Boolean res=LoadConfiguration();
                Boolean Multipar=Convert.ToBoolean(ConfigurationManager.AppSettings["MultiPartition"]);
                if (Multipar)
                {
                    if (!res)
                        Response.Redirect("/partitions/" + ConfigurationManager.AppSettings["as400Server"] + "/configuration.aspx", true);
                    Session.Add("user_" + ConfigurationManager.AppSettings["as400Server"], Login1.UserName);
                    Response.Redirect("/partitions/" + ConfigurationManager.AppSettings["as400Server"] + "/default.aspx", true);
                }
                else
                {
                    if (!res)
                        Response.Redirect( "/configuration.aspx", true);
                    Session.Add("user_" + ConfigurationManager.AppSettings["as400Server"], Login1.UserName);
                    Response.Redirect(  "/default.aspx", true);

                }

            }
            else
                e.Authenticated = false;
        }
        */
        public static Boolean LoadConfiguration()
        {
            AS400 system = new AS400();
            //se l'utente si è connesso allora creo una sessione di connessione as400 con un utente *ALLOBJ, server per poter reperire il monitor del server
            system.setSystemName(ConfigurationManager.AppSettings["as400Server"]);
            system.setUserId(ConfigurationManager.AppSettings["as400User"]); //utente con diritti *ALLOBJ
            system.setPassword(ConfigurationManager.AppSettings["as400Pwd"]);

            HttpContext.Current.Session.Add("iseries_"+ ConfigurationManager.AppSettings["as400Server"], system);

            //Creo la connessione all'as400 via JDBC da usare per le fasi di lettura e scrittura dei dati
            AS400JDBCRowSet rowset = FunzioniComuni.connectJdbcAS400(ConfigurationManager.AppSettings["as400Server"],
                ConfigurationManager.AppSettings["as400User"], ConfigurationManager.AppSettings["as400Pwd"]);

            if (rowset != null)
            {
                HttpContext.Current.Session.Add("iseriesJDBC_" + ConfigurationManager.AppSettings["as400Server"], rowset);

                //carico i dati per il server FTP
                string FTPAddress = "";

                FtpServer ftpServer = new FtpServer();
                rowset.setCommand("SELECT FTIIP,FTCSI,FTPOR,FTUSE,FTPSW,FTTYP,FTPAT,FTFTP,FTKEY FROM PMBRS.FTPCF00F");
                rowset.execute();


                while (rowset.next())
                {
                    string port = rowset.getString(3).Trim();
                    if (port != "" && port != "21")// 21 è default e non lo metto
                        FTPAddress = "ftp://" + rowset.getString(1).Trim() + ":" + rowset.getString(3).Trim();
                    else
                        FTPAddress = "ftp://" + rowset.getString(1).Trim();

                    ftpServer.ftpAddressURI = FTPAddress;
                    ftpServer.ftpAddress = rowset.getString(1).Trim();
                    ftpServer.user = rowset.getString(4).Trim();
                    ftpServer.pwd = rowset.getString(5).Trim();
                    ftpServer.ccsid = rowset.getString(2).Trim();
                    ftpServer.ftpType = rowset.getString(6).Trim();
                    ftpServer.ftpPath = rowset.getString(7).Trim();
                    ftpServer.port = port;// porta
                    ftpServer.ftpSecure = rowset.getString("FTFTP").Trim();
                    ftpServer.ftpSecureKey = rowset.getString("FTKEY").Trim();

                    HttpContext.Current.Session.Add("ftpServer_" + ConfigurationManager.AppSettings["as400Server"], ftpServer);
                    return true;// ho trovato la configurazione
                }

                return false;// la configurazione è vuota se esce qui

            }
            else
                return false;


        }




        protected void Login1_LoggedIn(object sender, EventArgs e)
        {
           // Session.Add("user_" + ConfigurationManager.AppSettings["as400Server"], Login1.UserName);
           
        }


        [WebMethod(Description = "Login", EnableSession = true)]
        public static String LoginServer(String UserName,String Password)
        {
            
            String res = String.Empty;
           
            AS400 server = new AS400();
            Boolean isc = false;

            server.setSystemName(ConfigurationManager.AppSettings["as400Server"]);
            server.setUserId(UserName); //user del login
            server.setPassword(Password); //pwd del login

            try
            {
                try { server.connectService(0); } //se si connette allora l'utente di login esiste ed entro
                catch { isc = false; }

                isc = server.isConnected();
               
                if (isc)
                {
                    HttpContext.Current.Session.Add("user_" + ConfigurationManager.AppSettings["as400Server"], UserName);

                    //creo la connessione all'as400 da usare dopo per il monitoring
                    Boolean conf = LoadConfiguration();
                    Boolean Multipar = Convert.ToBoolean(ConfigurationManager.AppSettings["MultiPartition"]);
                    if (Multipar)
                    {
                        String url = "/partitions/" + ConfigurationManager.AppSettings["as400Server"];
                        if (!conf)
                        {
                            url += "/configuration.aspx";
                        }
                        else
                        {
                            url += "/default.aspx";
                        }
                        return new JavaScriptSerializer().Serialize(url);


                    }
                    else
                    {
                        if (!conf)
                            return new JavaScriptSerializer().Serialize("/configuration.aspx");
                        else
                            return new JavaScriptSerializer().Serialize("/default.aspx");

                    }

                }
                else
                    return new JavaScriptSerializer().Serialize("Errore: user inesistente o password sbagliata!");
            }
            catch (Exception ex)
            {

                return new JavaScriptSerializer().Serialize("Errore: " + ex.Message);
            }



            
        }






        ///////////////////FINE CLASSE

    }
}