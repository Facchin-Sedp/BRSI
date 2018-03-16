using com.ibm.as400.access;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BRSi
{
    public partial class Configuration : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DDLSisOp.Items.Add("WINDOWS");
            DDLSisOp.Items.Add("BRS");
            DDLSisOp.Items.Add("BRSI");

            if (Session["ftpServer_" + ConfigurationManager.AppSettings["as400Server"]] != null) // all'apertura della pagina verifico se ho in sessione i parftp
            {
                FtpServer ftpServer = new FtpServer();
                ftpServer= (FtpServer)Session["ftpServer_" + ConfigurationManager.AppSettings["as400Server"]];

                TxtIndirizzo.Text = ftpServer.ftpAddress;
                TxtUser.Text= ftpServer.user ;
                TxtPwd.Text = ftpServer.pwd;                
                TxtCCSID.Text = ftpServer.ccsid;                
                TxtPath.Text = ftpServer.ftpPath;
                TxtPort.Text = ftpServer.port;
                TxtKey.Text = ftpServer.ftpSecureKey;
                if(ftpServer.ftpSecure =="S")
                    checkSftp.Checked =true;

                DDLSisOp.Text = DDLSisOp.Items.FindByText(ftpServer.ftpType.ToUpper()).Text;
                 
            }

            GiorniMantenimentoLog();


          
        }

        private void GiorniMantenimentoLog()
        {
            AS400JDBCRowSet rowset = FunzioniComuni.connectJdbcAS400(ConfigurationManager.AppSettings["as400Server"],
            ConfigurationManager.AppSettings["as400User"], ConfigurationManager.AppSettings["as400Pwd"]);
            String giorni = String.Empty;
            if (rowset != null)
            {


                rowset.setCommand("SELECT DAYRT FROM PMBRS.SAVDP00F");// solo ifs

                rowset.execute();// lettura

                // lettura rowset
                while (rowset.next())
                {

                   giorni= rowset.getString("DAYRT");

                }
                if(giorni !=String.Empty)
                    TxtggRetention.Text = giorni;
                else
                    TxtggRetention.Text = ConfigurationManager.AppSettings["GGRetentionDefault"];



            }
        }

        protected void checkSftp_CheckedChanged(object sender, EventArgs e)
        {
            if (checkSftp.Checked)
            {
                TxtPort.Text = "22";
                TxtKey.Visible=true;
            }
            else
            {
                TxtPort.Text = "21";
                TxtKey.Visible = false;
            }
        }

        [WebMethod]
        public static String get_IFS_on_Table()
        {
            List<String> ls_ifs = new List<string> { };
            AS400JDBCRowSet rowset = FunzioniComuni.connectJdbcAS400(ConfigurationManager.AppSettings["as400Server"],
            ConfigurationManager.AppSettings["as400User"], ConfigurationManager.AppSettings["as400Pwd"]);

            if (rowset != null)
            {


                rowset.setCommand("SELECT TRIM(CFIFS) FROM PMBRS.SAVCF00F where CFOBJ='I'");// solo ifs

                rowset.execute();// lettura

                // lettura rowset
                while (rowset.next())
                {

                    ls_ifs.Add(rowset.getString(1));

                }


            }
                return new JavaScriptSerializer().Serialize(ls_ifs);
        }

        [WebMethod]
        public static Boolean check_IFS(String rootDir)
        {
           

            String machine = ConfigurationManager.AppSettings["as400Server"];
            String user = ConfigurationManager.AppSettings["as400User"];
            String password = ConfigurationManager.AppSettings["as400Pwd"];

            AS400 server = new AS400();

            server.setSystemName(machine);
            server.setUserId(user);
            server.setPassword(password);
            IFSFile dir = new IFSFile(server, rootDir);

            if (dir.exists()){return true;}



            return false;
        }

        [WebMethod]
        public static Boolean save_IFS(string dirIFS)
        {

            String query = String.Empty;

            if (dirIFS == "/") dirIFS = "/*";// la vuole così

            #region QUERY DI INSERT LIBRERIE

            AS400JDBCRowSet rowset = FunzioniComuni.connectJdbcAS400(ConfigurationManager.AppSettings["as400Server"],
                    ConfigurationManager.AppSettings["as400User"], ConfigurationManager.AppSettings["as400Pwd"]);
            AS400JDBCRowSet rowsetSelect = FunzioniComuni.connectJdbcAS400(ConfigurationManager.AppSettings["as400Server"],
                    ConfigurationManager.AppSettings["as400User"], ConfigurationManager.AppSettings["as400Pwd"]);


            String data = DateTime.Now.ToString("yyyyMMdd");
            String ora = DateTime.Now.ToString("HHmmss");


            if (rowset != null)
            {
               
                // per ogni lib vado a vedere se esiste già
                // se esiste non la tocco se non c'è la aggiungo con la schedulazione di default
               
                    rowsetSelect.setCommand("select count(*) as cont from pmbrs.savcf00f where CFIFS ='" +  dirIFS + "'");
                    rowsetSelect.execute();

                    rowsetSelect.next();
                    int exist = rowsetSelect.getInt("cont");// verifico se ho la lib e decido se fare update o insert

   

                    if (exist == 0)// se non esiste allora insert  CFTS1       CFHS1             

                    {
                        query = "INSERT INTO PMBRS.SAVCF00F (CFEDT,CFETM,CFIFS,CFHS1,CFOBJ) "
                        + "values('" + data + "', '"
                        + ora + "','"
                        + dirIFS + "'," 
                        + "'2359',"// orasched                        
                        + "'I')";

                        rowset.setCommand(query);
                        rowset.execute();
                    }
                    else
                    {
                        query = "UPDATE PMBRS.SAVCF00F "
                        + "SET CFEDT='" + data + "',"
                        + " CFETM='" + ora + "'"                       
                        + "WHERE  CFLIB = '" + dirIFS + "'";
                        // inutile mettere il campo CFOBJ a L nell'update



                        rowset.setCommand(query);
                        rowset.execute();
                    }
                 


                rowset.close();
            }
            else
                return false;

            return true;

            #endregion

        }


        [WebMethod]
        public static Boolean delete_IFS(string dirIFS)
        {

            String query = String.Empty; 

            #region QUERY DI INSERT LIBRERIE

            AS400JDBCRowSet rowset = FunzioniComuni.connectJdbcAS400(ConfigurationManager.AppSettings["as400Server"],
                    ConfigurationManager.AppSettings["as400User"], ConfigurationManager.AppSettings["as400Pwd"]);
            

            if (rowset != null)
            {

                // per ogni lib vado a vedere se esiste già
                // se esiste non la tocco se non c'è la aggiungo con la schedulazione di default

              
                    query = "DELETE FROM PMBRS.SAVCF00F WHERE CFIFS='"+dirIFS+"'";

                    rowset.setCommand(query);
                    rowset.execute();
                



                rowset.close();
            }
            else
                return false;

            return true;

            #endregion

        }


        [WebMethod]
        public static Boolean save_GG_Ret(string ggRet)
        {

            String query = String.Empty;

           

            #region QUERY DI INSERT LIBRERIE

            AS400JDBCRowSet rowset = FunzioniComuni.connectJdbcAS400(ConfigurationManager.AppSettings["as400Server"],
                    ConfigurationManager.AppSettings["as400User"], ConfigurationManager.AppSettings["as400Pwd"]);
           
             

            if (rowset != null)
            {

                // per ogni lib vado a vedere se esiste già
                // se esiste non la tocco se non c'è la aggiungo con la schedulazione di default


                String qD = "DELETE FROM PMBRS.SAVDP00F";
                rowset.setCommand(qD);
                rowset.execute();
                String q = "INSERT INTO PMBRS.SAVDP00F (DAYRT) values('" + ggRet + "')";
                rowset.setCommand(q);
                rowset.execute();
                rowset.close();

            }
            else
                return false;

            return true;

            #endregion

        }






    }
}