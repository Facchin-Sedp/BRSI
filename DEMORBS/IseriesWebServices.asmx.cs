using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Configuration;
using com.ibm.as400.access;
using java.util;
using java.text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using java.sql;
using WinSCP;
using System.Globalization;
using System.Reflection;

namespace BRSi
{
    /// <summary>
    /// Descrizione di riepilogo per IseriesWebServices
    /// </summary>
    [WebService(Namespace = "http://BRSWEB/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Per consentire la chiamata di questo servizio Web dallo script utilizzando ASP.NET AJAX, rimuovere il commento dalla riga seguente. 
    [System.Web.Script.Services.ScriptService]
    

    public class IseriesWebServices : System.Web.Services.WebService
    {

        #region RICERCA OGGETTI
        [WebMethod(Description = "ricerca oggetti salvati", EnableSession = false)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]        
        public String search_Items(String txtSearch,String txtLibreria,String txtTipo)
        {
            txtSearch = txtSearch.ToUpper();

            List<result_search> ls_result_search = new List<result_search> { };

            AS400JDBCRowSet rowset = FunzioniComuni.connectJdbcAS400(ConfigurationManager.AppSettings["as400Server"],
            ConfigurationManager.AppSettings["as400User"], ConfigurationManager.AppSettings["as400Pwd"]);
            String where = String.Empty;

            String querySearch = @" SELECT NOMEFA,SRONAM,SROTYP,SROLIB,SROSVT FROM PMBRS.STLIB00F
                                    WHERE 
                ";

            if (txtSearch != String.Empty && txtLibreria != String.Empty && txtTipo != String.Empty)
                where += @"
                                    (UPPER(NOMEFA) LIKE '%@TXTSEARCH@%'
                                    OR
                                    UPPER(SRONAM) LIKE '%@TXTSEARCH@%'
                                    OR
                                    UPPER(SROSVT) LIKE '%@TXTSEARCH@%')
                        ";
            else if(txtSearch != String.Empty && txtLibreria != String.Empty && txtTipo == String.Empty)
                where += @"
                                    (UPPER(NOMEFA) LIKE '%@TXTSEARCH@%'
                                    OR
                                    UPPER(SRONAM) LIKE '%@TXTSEARCH@%'
                                    OR
                                    UPPER(SROTYP) LIKE '%@TXTSEARCH@%'
                                    OR
                                    UPPER(SROSVT) LIKE '%@TXTSEARCH@%')
                        ";
            else if (txtSearch != String.Empty && txtLibreria == String.Empty && txtTipo != String.Empty)
                where += @"
                                    (UPPER(NOMEFA) LIKE '%@TXTSEARCH@%'
                                    OR
                                    UPPER(SRONAM) LIKE '%@TXTSEARCH@%'
                                    OR
                                    UPPER(SROLIB) LIKE '%@TXTSEARCH@%'
                                    OR
                                    UPPER(SROSVT) LIKE '%@TXTSEARCH@%')
                        ";
            else if(txtSearch != String.Empty && txtLibreria == String.Empty && txtTipo == String.Empty)
                where += @"
                                    (UPPER(NOMEFA) LIKE '%@TXTSEARCH@%'
                                    OR
                                    UPPER(SRONAM) LIKE '%@TXTSEARCH@%'
                                    OR
                                    UPPER(SROTYP) LIKE '%@TXTSEARCH@%'
                                    OR
                                    UPPER(SROLIB) LIKE '%@TXTSEARCH@%'
                                    OR
                                    UPPER(SROSVT) LIKE '%@TXTSEARCH@%')
                        ";

            if (txtLibreria != String.Empty)
                where += @" AND UPPER(SROLIB) LIKE '%" + txtLibreria+@"%'";
            if (txtTipo != String.Empty)
                where += @" AND UPPER(SROTYP) LIKE '%" + txtTipo + @"%'";

            // se inizia con AND questo è da rimuovere
            if (where.Trim().StartsWith("AND"))
                where = where.Remove(0, 4);

            querySearch += where.Replace("@TXTSEARCH@",txtSearch);
            querySearch += @"       ORDER BY SROSVT DESC
                                    FETCH FIRST 100 ROW ONLY";
            rowset.setCommand(querySearch);
            rowset.execute();// prima cancello poi inserisco
            while (rowset.next())
            {
                String NOMEFA = rowset.getString("NOMEFA");
                String SROMNM = rowset.getString("SRONAM");
                String SROTYP = rowset.getString("SROTYP");
                String SROLIB = rowset.getString("SROLIB");
                String SROSVT = rowset.getString("SROSVT");

                ls_result_search.Add(new result_search
                {
                    NOMEFA = NOMEFA.Trim(),
                    SROLIB = SROLIB.Trim(),
                    SROMNM = SROMNM.Trim(),
                    SROSVT = SROSVT.Trim(),
                    SROTYP = SROTYP.Trim()
                });

            }
            JavaScriptSerializer jser = new JavaScriptSerializer();
            jser.MaxJsonLength = 2147483647;
            return jser.Serialize(ls_result_search);
        }


        [WebMethod(Description = "ricerca oggetti salvati IFS", EnableSession = false)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String search_Items_IFS(String txtSearch, String txtOggetto, String txtTipo)
        {
            txtSearch = txtSearch.ToUpper();

            List<result_search_IFS> ls_result_search = new List<result_search_IFS> { };

            AS400JDBCRowSet rowset = FunzioniComuni.connectJdbcAS400(ConfigurationManager.AppSettings["as400Server"],
            ConfigurationManager.AppSettings["as400User"], ConfigurationManager.AppSettings["as400Pwd"]);
            String where = String.Empty;

            String querySearch = @" SELECT NOMEFA,SRDIR,SRDAT,SRTIM,SRTIP,SROBJ FROM PMBRS.STIFS00F
                                    WHERE 
                ";

            if (txtSearch != String.Empty && txtOggetto != String.Empty && txtTipo != String.Empty)
                where += @"
                                    (UPPER(NOMEFA) LIKE '%@TXTSEARCH@%'
                                    OR
                                    UPPER(SRDIR) LIKE '%@TXTSEARCH@%')                                 
                        ";
            else if (txtSearch != String.Empty && txtOggetto != String.Empty && txtTipo == String.Empty)
                where += @"
                                    (UPPER(NOMEFA) LIKE '%@TXTSEARCH@%'
                                    OR
                                    UPPER(SRDIR) LIKE '%@TXTSEARCH@%'
    
                                    OR
                                    UPPER(SROBJ) LIKE '%@TXTSEARCH@%')
                        ";
            else if (txtSearch != String.Empty && txtOggetto == String.Empty && txtTipo != String.Empty)// TIPO
                where += @"
                                    (UPPER(NOMEFA) LIKE '%@TXTSEARCH@%'
                                    OR
                                    UPPER(SRDIR) LIKE '%@TXTSEARCH@%'                                
                                    OR
                                    UPPER(SROBJ) LIKE '%@TXTSEARCH@%')
                        ";
            else if (txtSearch != String.Empty && txtOggetto == String.Empty && txtTipo == String.Empty)
                where += @"
                                    (UPPER(NOMEFA) LIKE '%@TXTSEARCH@%'
                                    OR
                                    UPPER(SRDIR) LIKE '%@TXTSEARCH@%'
                                    OR
                                    UPPER(SRTIP) LIKE '%@TXTSEARCH@%'
                                    OR
                                    UPPER(SROBJ) LIKE '%@TXTSEARCH@%')
                                  
                        ";
           
            if (txtOggetto != String.Empty)
                where += @" AND UPPER(SROBJ) LIKE '%" + txtOggetto + @"%'";
            if (txtTipo != String.Empty)
                where += @" AND UPPER(SRTIP) LIKE '%" + txtTipo + @"%'";

            // se inizia con AND questo è da rimuovere
            if (where.Trim().StartsWith("AND"))
                where = where.Remove(0, 4);

            querySearch += where.Replace("@TXTSEARCH@", txtSearch);
            querySearch += @"       ORDER BY SRDAT||SRTIM DESC
                                    FETCH FIRST 100 ROW ONLY";
            rowset.setCommand(querySearch);
            rowset.execute();// prima cancello poi inserisco
            while (rowset.next())
            {
                String NOMEFA = rowset.getString("NOMEFA");
                String SRDIR = rowset.getString("SRDIR");
                String SRTIP = rowset.getString("SRTIP");
                String SROBJ = rowset.getString("SROBJ");
                String SRDAT = rowset.getString("SRDAT");
                String SRTIM = rowset.getString("SRTIM");

                ls_result_search.Add(new result_search_IFS
                {
                    NOMEFA = NOMEFA.Trim().ToUpper(),
                    SRDIR = SRDIR.Trim().ToUpper(),
                    SRTIP = SRTIP.Trim().ToUpper(),
                    SROBJ = SROBJ.Trim().ToUpper(),
                    SRDAT = SRDAT.Trim().ToUpper(),
                    SRTIM = SRTIM.Trim().ToUpper()
                });

            }
            JavaScriptSerializer jser = new JavaScriptSerializer();
            jser.MaxJsonLength = 2147483647;
            if (ls_result_search.Count == 0)
            { 
                ls_result_search.Add(new result_search_IFS
                {
                    NOMEFA =querySearch
                    
                });
            }

            return jser.Serialize(ls_result_search);

        }

        [WebMethod(Description = "ricerca oggetti salvati", EnableSession = false)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String search_Items_QDLS(String txtSearch, String txtOggetto, String Tipo)
        {
            txtSearch = txtSearch.ToUpper();

            List<result_search_QDLS> ls_result_search = new List<result_search_QDLS> { };

            AS400JDBCRowSet rowset = FunzioniComuni.connectJdbcAS400(ConfigurationManager.AppSettings["as400Server"],
            ConfigurationManager.AppSettings["as400User"], ConfigurationManager.AppSettings["as400Pwd"]);
            String where = String.Empty;

            String querySearch = @" SELECT NOMEFA,SDLDOC,SDLPTH,SDLDSC,SDLDAT,SDLTIM,SDLTYP FROM PMBRS.STDOC00F
                                    WHERE 
                ";
 

            if (txtOggetto != String.Empty)
                where += @" AND (UPPER(SDLDOC) LIKE '%" + txtOggetto + @"%' OR UPPER(SDLPTH) LIKE '%" + txtOggetto + @"%')";
            if (Tipo != "0")
                where += @" AND UPPER(SDLTYP) = " + Tipo ;

            // se inizia con AND questo è da rimuovere
            if (where.Trim().StartsWith("AND"))
                where = where.Remove(0, 4);

            if (where == String.Empty)
                querySearch = querySearch.Replace("WHERE", "");// elimino la clausola where se non esiste un where

            querySearch += where.Replace("@TXTSEARCH@", txtSearch);
            querySearch += @"       ORDER BY SDLDAT||SDLTIM DESC
                                    FETCH FIRST 100 ROW ONLY";
            rowset.setCommand(querySearch);
            rowset.execute();// prima cancello poi inserisco
            while (rowset.next())
            {
                //NOMEFA,SDLDOC,SDLPTH,SDLDSC,SDLDAT,SDLTIM 
                String NOMEFA = rowset.getString("NOMEFA");
                String SDLDOC = rowset.getString("SDLDOC");
                String SDLPTH = rowset.getString("SDLPTH");
                String SDLDSC = rowset.getString("SDLDSC");
                String SDLDAT = rowset.getString("SDLDAT");
                String SDLTIM = rowset.getString("SDLTIM");
                String SDLTYP = rowset.getString("SDLTYP");

                ls_result_search.Add(new result_search_QDLS
                {
                    NOMEFA = NOMEFA.Trim(),
                    SDLDOC = SDLDOC.Trim(),
                    SDLPTH = SDLPTH.Trim(),
                    SDLDSC = SDLDSC.Trim(),
                    SDLTYP = SDLTYP.Trim(),
                    SDLDAT = SDLDAT.Trim(),
                    SDLTIM = SDLTIM.Trim()
            });

            }
            JavaScriptSerializer jser = new JavaScriptSerializer();
            jser.MaxJsonLength = 2147483647;
            return jser.Serialize(ls_result_search);
        }


        [WebMethod(Description = "ricerca Errori oggetti salvati", EnableSession = false)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String search_Items_ERRORILIB(String libInErrore)
        {

            List<result_search_ErroriLib> ls_result_search = new List<result_search_ErroriLib> { };

            // passo la stringa e da questa cerco il nome libreria secondo il pattern seguente
            String pattern = @".*libreria\s(.*)\ssono.*";
            Regex re = new Regex(pattern);

            Match m= re.Match(libInErrore);
            if (m.Success)
            {
                libInErrore = m.Groups[1].Value;
            }
            else
            {
                JavaScriptSerializer jserX = new JavaScriptSerializer();
                jserX.MaxJsonLength = 2147483647;
                return jserX.Serialize(ls_result_search);
            }




            AS400JDBCRowSet rowset = FunzioniComuni.connectJdbcAS400(ConfigurationManager.AppSettings["as400Server"],
            ConfigurationManager.AppSettings["as400User"], ConfigurationManager.AppSettings["as400Pwd"]);
            String where = String.Empty;

            String querySearch = @" SELECT SROSVT,SROLIB,SRONAM,SROTYP,SROCMD,NOMEFA FROM PMBRS.STLIBE0F
                                    WHERE SROLIB='"+ libInErrore + "'";
                         


            
            rowset.setCommand(querySearch);
            rowset.execute();// prima cancello poi inserisco
            while (rowset.next())
            {
                //NOMEFA,SDLDOC,SDLPTH,SDLDSC,SDLDAT,SDLTIM 
                String SROSVT = rowset.getString("SROSVT");
                String SROLIB = rowset.getString("SROLIB");
                String SRONAM = rowset.getString("SRONAM");
                String SROTYP = rowset.getString("SROTYP");
                String SROCMD = rowset.getString("SROCMD");
                String NOMEFA = rowset.getString("NOMEFA");
                 

                ls_result_search.Add(new result_search_ErroriLib
                {
                    SROSVT = SROSVT.Trim(),
                    SROLIB = SROLIB.Trim(),
                    SRONAM = SRONAM.Trim(),
                    SROTYP = SROTYP.Trim(),
                    SROCMD = SROCMD.Trim(),
                    NOMEFA = NOMEFA.Trim() 
                });

            }
            JavaScriptSerializer jser = new JavaScriptSerializer();
            jser.MaxJsonLength = 2147483647;
            return jser.Serialize(ls_result_search);
        }


        #endregion


        #region PERFORMANCE ISERIES
        [WebMethod(Description="Lettura proprieta di sistema",EnableSession=false)]
        // [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String GetServerProperties()
        {
            iSeriesProperties iseries = new iSeriesProperties();
            AS400 server = null;
            try
            {
                server = (AS400)Session["iseries_" + ConfigurationManager.AppSettings["as400Server"]];
            }
            catch (Exception)
            { }

            if (server == null)
            {
                server = new AS400();
                server.setSystemName(ConfigurationManager.AppSettings["as400Server"]);
                server.setUserId(ConfigurationManager.AppSettings["as400User"]); //utente con diritti *ALLOBJ
                server.setPassword(ConfigurationManager.AppSettings["as400Pwd"]);
               // Session.Add("iseries_" + ConfigurationManager.AppSettings["as400Server"], server);
            }

            SystemValue sv = new SystemValue();
            sv.setSystem(server);
            sv.setName("QSRLNBR");
            iseries.Serial = (String)sv.getValue();
            iseries.Name = server.getSystemName();

            //return iseries;
            return new JavaScriptSerializer().Serialize(iseries); //non è chiamato con oggetti kendo ma con normale json quindi devo serializzare il risultato
        }

        [WebMethod(Description = "Lettura CPU", EnableSession = false)]      
        public float GetServerPerfRealTime()
        {
            float cpu = 0;
            try
            {
                AS400 server = null;
                try
                {
                    server = (AS400)Session["iseries_" + ConfigurationManager.AppSettings["as400Server"]];
                }
                catch (Exception)
                { }

                if (server == null)
                {
                    server = new AS400();
                    server.setSystemName(ConfigurationManager.AppSettings["as400Server"]);
                    server.setUserId(ConfigurationManager.AppSettings["as400User"]); //utente con diritti *ALLOBJ
                    server.setPassword(ConfigurationManager.AppSettings["as400Pwd"]);
                   // Session.Add("iseries_"+ ConfigurationManager.AppSettings["as400Server"], server);
                }

                SystemStatus st = new SystemStatus();
                st.setSystem(server);
                cpu = st.getPercentProcessingUnitUsed();// cpu in %
                server.disconnectAllServices();
                return cpu;
            }
            catch (Exception)
            {
                return cpu;
            }
     
        }


        [WebMethod(Description = "Lettura ASP%", EnableSession = false)]
        public float GetASPPercentage()
        {
            float asp = 0;
            try
            {
                AS400 server = null;
                try
                {
                    server = (AS400)Session["iseries_" + ConfigurationManager.AppSettings["as400Server"]];
                }
                catch (Exception)
                { }

                if (server == null)
                {
                    server = new AS400();
                    server.setSystemName(ConfigurationManager.AppSettings["as400Server"]);
                    server.setUserId(ConfigurationManager.AppSettings["as400User"]); //utente con diritti *ALLOBJ
                    server.setPassword(ConfigurationManager.AppSettings["as400Pwd"]);
                   // Session.Add("iseries_" + ConfigurationManager.AppSettings["as400Server"], server);
                }

                SystemStatus st = new SystemStatus();
                st.setSystem(server);

                asp = st.getPercentSystemASPUsed();

                server.disconnectAllServices();
                return asp;
            }
            catch (Exception)
            {
                return asp;
            }
    
        }

        [WebMethod(Description = "Lettura n Users", EnableSession = false)]
        public int GetUsersSignedOn()
        {
            int users = 0;
            try
            {
                AS400 server = null;
                try
                {
                    server = (AS400)Session["iseries_" + ConfigurationManager.AppSettings["as400Server"]];
                }
                catch (Exception)
                { }

                if (server == null)
                {
                    server = new AS400();
                    server.setSystemName(ConfigurationManager.AppSettings["as400Server"]);
                    server.setUserId(ConfigurationManager.AppSettings["as400User"]); //utente con diritti *ALLOBJ
                    server.setPassword(ConfigurationManager.AppSettings["as400Pwd"]);
                   // Session.Add("iseries_" + ConfigurationManager.AppSettings["as400Server"], server);
                }

                SystemStatus st = new SystemStatus();
                st.setSystem(server);

                users = st.getUsersCurrentSignedOn();

                server.disconnectAllServices();
                return users;
            }
            catch (Exception)
            {
                return users;
            }

        }


        [WebMethod(Description = "Lettura JOB SUMMARY", EnableSession = false)]
        public String GetJobsSummary()
        {
            AS400 server = null;
            try
            {
                server = (AS400)Session["iseries_" + ConfigurationManager.AppSettings["as400Server"]];
            }
            catch (Exception)
            { }

            if (server == null)
            {
                server = new AS400();
                server.setSystemName(ConfigurationManager.AppSettings["as400Server"]);
                server.setUserId(ConfigurationManager.AppSettings["as400User"]); //utente con diritti *ALLOBJ
                server.setPassword(ConfigurationManager.AppSettings["as400Pwd"]);
               // Session.Add("iseries_" + ConfigurationManager.AppSettings["as400Server"], server);
            }

            SystemStatus st = new SystemStatus();
            st.setSystem(server);

            List<JobsSummary> jsumList = new List<JobsSummary> { };
            JobsSummary ja = new JobsSummary();

            ja.Status = "Active";
            ja.Number = st.getActiveJobsInSystem();

            jsumList.Add(ja);

            JobsSummary jt = new JobsSummary();

            jt.Status = "Total";
            jt.Number = st.getJobsInSystem();

            jsumList.Add(jt);

            JobsSummary jb = new JobsSummary();

            jb.Status = "Batch";
            jb.Number = st.getBatchJobsRunning();

            jsumList.Add(jb);

            JobsSummary jbw = new JobsSummary();

            jbw.Status = "Batch waiting";
            jbw.Number = st.getBatchJobsWaitingForMessage();

            jsumList.Add(jbw);

            JobsSummary jat = new JobsSummary();

            jat.Status = "Active Thread";
            jat.Number = st.getActiveThreadsInSystem();

            jsumList.Add(jat);
            server.disconnectAllServices();
            return new JavaScriptSerializer().Serialize(jsumList);
        }

   

        [WebMethod(Description = "Lettura users", EnableSession = false)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<iSeriesUser> GetUserList()
        {
            AS400 server = null;
            try
            {
                server = (AS400)Session["iseries_" + ConfigurationManager.AppSettings["as400Server"]];
            }
            catch (Exception)
            { }

            if (server == null)
            {
                server = new AS400();
                server.setSystemName(ConfigurationManager.AppSettings["as400Server"]);
                server.setUserId(ConfigurationManager.AppSettings["as400User"]); //utente con diritti *ALLOBJ
                server.setPassword(ConfigurationManager.AppSettings["as400Pwd"]);
               // Session.Add("iseries_" + ConfigurationManager.AppSettings["as400Server"], server);
            }
            UserList ul = new UserList(server);

            Enumeration users = ul.getUsers();
            List<iSeriesUser> ls_user = new List<iSeriesUser> { };

            while (users.hasMoreElements())
            {

                User u = (User)users.nextElement();
                u.loadUserInformation();

                iSeriesUser utente = new iSeriesUser();
                

                utente.Name = u.getName();
                utente.Descrizione = u.getDescription();
                java.util.Date dt=u.getPreviousSignedOnDate();
                if (dt != null)
                {
                    SimpleDateFormat dateformat = new SimpleDateFormat("yyyy/MM/dd");

                    String lastlogon = dateformat.format(dt);
                    utente.lastlogon = lastlogon;
                }
                else
                    utente.lastlogon = "NA";
                
                ls_user.Add(utente);
        
            
            }

            server.disconnectAllServices();
            return ls_user;
           // return new JavaScriptSerializer().Serialize(ls_user);
        }

        [WebMethod(Description="Lista messaggi coda QEDD",EnableSession=false)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json,UseHttpGet=false)]
        public List<MsgCoda> GetMsgCoda(string datax,String Severity)       
        {
            AS400 server = null;

            try
            {
                server = (AS400)Session["iseries_" + ConfigurationManager.AppSettings["as400Server"]];
            }
            catch (Exception)
            { }

            if (server == null)
            {
                server = new AS400();
                server.setSystemName(ConfigurationManager.AppSettings["as400Server"]);
                server.setUserId(ConfigurationManager.AppSettings["as400User"]); //utente con diritti *ALLOBJ
                server.setPassword(ConfigurationManager.AppSettings["as400Pwd"]);
               // Session.Add("iseries_" + ConfigurationManager.AppSettings["as400Server"], server);
            }

            List<MsgCoda> ls_msg= new List<MsgCoda>{};

            try
            {
                DateTime datafiltered = Convert.ToDateTime(datax);// QDFTOWN

                QSYSObjectPathName path = new QSYSObjectPathName("PMBRS", "PMBRS", "MSGQ");//("PMEDDUSR", "PMEDD", "MSGQ");

                MessageQueue q = new MessageQueue(server, path.getPath());// esiste anche MessageQueue.CURRENT
                
                q.setListDirection(false);// dal + nuovo al vecchio
                q.setHelpTextFormatting(MessageFile.SUBSTITUTE_FORMATTING_CHARACTERS);// dovrebbe sostituire i char con spazi
                q.setSeverity(Convert.ToInt32(Severity));
                Enumeration enq = q.getMessages();

                while (enq.hasMoreElements())
                {

                    QueuedMessage mes = (QueuedMessage)enq.nextElement();

                    String datatimestr = mes.getDate().get(java.util.Calendar.DAY_OF_MONTH).ToString().PadLeft(2, '0')
                    + "/"
                    + (mes.getDate().get(java.util.Calendar.MONTH) + 1).ToString().PadLeft(2, '0')
                    + "/"
                    + mes.getDate().get(java.util.Calendar.YEAR).ToString().PadLeft(2, '0')
                    + " "
                    + mes.getDate().get(java.util.Calendar.HOUR_OF_DAY).ToString().PadLeft(2, '0')
                    + ":"
                    + mes.getDate().get(java.util.Calendar.MINUTE).ToString().PadLeft(2, '0')
                    + ":"
                    + mes.getDate().get(java.util.Calendar.SECOND).ToString().PadLeft(2, '0');

                    String msgid = mes.getID();
                    String Testo = mes.getText().Replace("\0", "").Trim();
                    String help = mes.getHelp().Replace("\0", "");
                    Int32 severity = mes.getSeverity();
                    DateTime convdata = Convert.ToDateTime(datatimestr);

                    if (datafiltered != null)
                        if (datafiltered.ToShortDateString() == convdata.ToShortDateString() && severity >= 0)
                            ls_msg.Add(new MsgCoda { Msgid = msgid, Testo = Testo, Help = help, severity = severity, datatime = datatimestr });

                }

                q.close();// chiudo la coda
                server.disconnectAllServices();
            }
            catch (Exception)
            {

            }
            return ls_msg;
        }

        public class MsgCoda
        {
            public String Msgid { get; set; }
            public String datatime {get;set;}
            public String Testo { get; set; }
            public String Help { get; set; }
            public Int32 severity { get; set; } 
        }

     

        public class JobDetails
        {
            public string Status { get; set; }
            public string ActiveStatus { get; set; }
            public string Number { get; set; }
            public string Name { get; set; }
            public string User { get; set; }
            public string Subsystem { get; set; }
        }

        [WebMethod(Description = "Lista dei JOBS", EnableSession = false)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<JobDetails> JOBLIST()
        {
            List<JobDetails> results = new List<JobDetails> { };
            AS400 server = null;

            try
            {
                server = (AS400)Session["iseries_" + ConfigurationManager.AppSettings["as400Server"]];
            }
            catch (Exception)
            { }

            if (server == null)
            {
                server = new AS400();
                server.setSystemName(ConfigurationManager.AppSettings["as400Server"]);
                server.setUserId(ConfigurationManager.AppSettings["as400User"]); //utente con diritti *ALLOBJ
                server.setPassword(ConfigurationManager.AppSettings["as400Pwd"]);
                //Session.Add("iseries_" + ConfigurationManager.AppSettings["as400Server"], server);
            }


            JobList list = new JobList(server);
            /// gettype:
            /// " " - The job is not a valid job. 
            ///    "A" - The job is an autostart job. 
            ///    "B" - The job is a batch job. 
            ///    "I" - The job is an interactive job. 
            ///    "M" - The job is a subsystem monitor job. 
            ///    "R" - The job is a spooled reader job. 
            ///    "S" - The job is a system job. 
            ///    "W" - The job is a spooled writer job. 
            ///    "X" - The job is a SCPF system job.
            ///    "V" - SLIC task - check product licenses
            ///    
            // list.addJobSelectionCriteria(JobList.SELECTION_JOB_NAME,jobName);// ritorna gli *active
            // Valori multipli
            list.addJobSelectionCriteria(JobList.SELECTION_ACTIVE_JOB_STATUS, Job.ACTIVE_JOB_STATUS_WAIT_MESSAGE);// msgw
            list.addJobSelectionCriteria(JobList.SELECTION_ACTIVE_JOB_STATUS, "DLYW");
            //list.addJobSelectionCriteria(JobList.SELECTION_ACTIVE_JOB_STATUS, "SIGW");

            list.addJobSelectionCriteria(JobList.SELECTION_PRIMARY_JOB_STATUS_ACTIVE, java.lang.Boolean.TRUE);
            list.addJobSelectionCriteria(JobList.SELECTION_PRIMARY_JOB_STATUS_JOBQ, java.lang.Boolean.FALSE);
            list.addJobSelectionCriteria(JobList.SELECTION_PRIMARY_JOB_STATUS_OUTQ, java.lang.Boolean.FALSE);
            Enumeration items = list.getJobs();

            while (items.hasMoreElements())
            {
                Job job = (Job)items.nextElement();

                job.loadInformation();

                String Active_Status = job.getValue(Job.ACTIVE_JOB_STATUS).ToString();// stato attuale tipo MSGW e altri
                String variable = job.getValue(Job.CURRENT_LIBRARY).ToString();

                var details = new JobDetails
                {
                    Name = job.getName().Trim(),
                    User=job.getUser().Trim(),
                    Number = job.getNumber().Trim(),
                    Status= job.getStatus(),
                    ActiveStatus = Active_Status,
                    Subsystem=job.getSubsystem()
                };

                string[] p = details.Subsystem.Split('/');
                String soloSBS = p[p.Length - 1].Split('.')[0];

                details.Subsystem = soloSBS;

                results.Add(details);

            }
            server.disconnectAllServices();
            return results;
        }


        [WebMethod(Description = "Job Logs", EnableSession = false)]
        public String JOBLOG(String par)
        {
            AS400 server = null;

            try
            {
                server = (AS400)Session["iseries_" + ConfigurationManager.AppSettings["as400Server"]];
            }
            catch (Exception)
            { }

            if (server == null)
            {
                server = new AS400();
                server.setSystemName(ConfigurationManager.AppSettings["as400Server"]);
                server.setUserId(ConfigurationManager.AppSettings["as400User"]); //utente con diritti *ALLOBJ
                server.setPassword(ConfigurationManager.AppSettings["as400Pwd"]);
              //  Session.Add("iseries_" + ConfigurationManager.AppSettings["as400Server"], server);
            }

            String[] parsplit = par.Split(',');//name,user,number
            JobLog jlog = new JobLog(server, parsplit[0], parsplit[1], parsplit[2]);
            String log= String.Empty;

            try
            {
                Enumeration enlog = jlog.getMessages();
                while (enlog.hasMoreElements()) // joblog
                {
                    QueuedMessage l = (QueuedMessage)enlog.nextElement();


                    String dataMessaggio = l.getDate().get(java.util.Calendar.DAY_OF_MONTH).ToString().PadLeft(2, '0')
                        + "/"
                        + (l.getDate().get(java.util.Calendar.MONTH) + 1).ToString().PadLeft(2, '0')
                        + "/"
                        + l.getDate().get(java.util.Calendar.YEAR).ToString()
                        + " "
                        + l.getDate().get(java.util.Calendar.HOUR).ToString().PadLeft(2, '0')
                        + ":"
                        + l.getDate().get(java.util.Calendar.MINUTE).ToString().PadLeft(2, '0')
                        + ":"
                        + l.getDate().get(java.util.Calendar.SECOND).ToString().PadLeft(2, '0')
                        + "<br>";

                    log += dataMessaggio + l.getText();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            server.disconnectAllServices();
            return log;
        }


#endregion


        #region LISTA LIBRERIE e SALVATAGGIO
        [WebMethod(Description = "Enumerazione Librerie", EnableSession = false)]
        public List<Library> Get_libraries_list()
        {
            AS400 server = null;

            try
            {
                server = (AS400)Session["iseries_" + ConfigurationManager.AppSettings["as400Server"]];
            }
            catch (Exception)
            { }

            if (server == null)
            {
                server = new AS400();
                server.setSystemName(ConfigurationManager.AppSettings["as400Server"]);
                server.setUserId(ConfigurationManager.AppSettings["as400User"]); //utente con diritti *ALLOBJ
                server.setPassword(ConfigurationManager.AppSettings["as400Pwd"]);
               // Session.Add("iseries_" + ConfigurationManager.AppSettings["as400Server"], server);
            }
            List<Library> result = new List<Library> { };

            CommandCall command = new CommandCall(server);
            try
            {
                // run command
                if (command.run("DSPOBJD OBJ(*ALLUSR) OBJTYPE(*LIB) OUTPUT(*OUTFILE) OUTFILE(PMBRS/LIBRARY)") != true)
                {
                    // Note that there was an error.
                    return null;
                }
                # region lettura messaggio di ritorno se serve
                // messaggio as400 del comando
                //AS400Message[] messageList = command.getMessageList();
                //for (int i = 0; i < messageList.Length; ++i)
                //{
                //    // Show each message.


                //}
                #endregion


                result = Read_Libraries_list();

            }
            catch (Exception e)
            {
                String err = "Command " + command.getCommand() + " issued an exception! " + e.Message;
                return null;

            }

            return result;

        }

        private List<Library> Read_Libraries_list()
        {
            List<Library> rs = new List<Library> { };

            AS400JDBCRowSet rowset = FunzioniComuni.connectJdbcAS400(ConfigurationManager.AppSettings["as400Server"],
                ConfigurationManager.AppSettings["as400User"], ConfigurationManager.AppSettings["as400Pwd"]);

            if (rowset != null)
            {
   
                rowset.setCommand("SELECT ODOBNM,ODOBAT,ODOBTX FROM PMBRS.LIBRARY " +
                                  "WHERE TRIM(ODOBNM) NOT IN (SELECT TRIM(CFLIB) FROM PMBRS.SAVCF00F)");
                rowset.execute();

                // lettura rowset
                while (rowset.next())
                {
                    Library l = new Library();
                    l.nome = rowset.getString("ODOBNM").Trim();
                    l.Descrizione = rowset.getString("ODOBTX").Trim();
                    l.attributo = rowset.getString("ODOBAT").Trim();
                    l.compressione = "false";
                    l.zip = "false";
                    rs.Add(l);
                }
                rowset.close();
            }
            else
                return null;
            return rs;
        }

        [WebMethod(Description = "Salvataggio librerie", EnableSession = false)]
        public String Save_Libraries_list(string JsonLibs)
        {

            Library[] libs = new JavaScriptSerializer().Deserialize<Library[]>(JsonLibs);// non funziona con le liste ma con gli array

            List<Library> libraries = libs.ToList();// converto in lista perchè + comodo da utilizzare

            String format_libs = String.Empty;

            foreach (Library lib in libraries)// formatto la lista per utilizzarla nella query
            {
                format_libs +="'" + lib.nome + "',";
            }

            format_libs = format_libs.Remove(format_libs.LastIndexOf(","),1);

            #region QUERY DI INSERT LIBRERIE

            AS400JDBCRowSet rowset = FunzioniComuni.connectJdbcAS400(ConfigurationManager.AppSettings["as400Server"],
                    ConfigurationManager.AppSettings["as400User"], ConfigurationManager.AppSettings["as400Pwd"]);
            AS400JDBCRowSet rowsetSelect = FunzioniComuni.connectJdbcAS400(ConfigurationManager.AppSettings["as400Server"],
                    ConfigurationManager.AppSettings["as400User"], ConfigurationManager.AppSettings["as400Pwd"]);

            
            String data = DateTime.Now.ToString("yyyyMMdd");
            String ora = DateTime.Now.ToString("HHmmss");


            if (rowset != null)
            {
                rowset.setCommand("DELETE FROM PMBRS.SAVCF00F Where CFLIB not in (" + format_libs + ") and CFOBJ='L'");
                rowset.execute();// prima cancello le lib che non sono nella nuova lista poi inserisco le altre


                // per ogni lib vado a vedere se esiste già
                // se esiste non la tocco se non c'è la aggiungo con la schedulazione di default
                foreach (Library lib in libraries)
                {
                    rowsetSelect.setCommand("select count(*) as cont from pmbrs.savcf00f where CFLIB ='"+lib.nome+"'");
                    rowsetSelect.execute();

                    rowsetSelect.next();
                    int exist= rowsetSelect.getInt("cont");// verifico se ho la lib e decido se fare update o insert
                   
                    
                    String comp = String.Empty;
                    String zip = String.Empty;
                     String query=String.Empty;

                    if (Convert.ToBoolean(lib.compressione)) comp = "Y"; else comp = "N";
                    if (Convert.ToBoolean(lib.zip)) zip = "Y"; else zip = "N";

                    if (exist == 0)// se non esiste allora insert  CFTS1       CFHS1             

                    {
                        query = "INSERT INTO PMBRS.SAVCF00F (CFEDT,CFETM,CFLIB,CFCPR,CFZIP,CFDES,CFTS1,CFHS1,CFTYP,CFOBJ) "
                        + "values('" + data + "','"
                        + ora + "','"
                        + lib.nome + "','"
                        + comp + "','"
                        + zip + "','"
                        + lib.Descrizione + "','"
                        + "T" + "','"// totale
                        + "2359" + "','"// orasched
                        + lib.attributo + "','"
                        + "L')";

                        rowset.setCommand(query);
                        rowset.execute();
                    }
                    else
                    {
                        query = "UPDATE PMBRS.SAVCF00F "
                        + "SET CFEDT='" + data + "',"
                        + " CFETM='" + ora + "',"                       
                        + " CFCPR='" + comp + "',"
                        + " CFZIP='" + zip + "',"
                        + " CFDES='" + lib.Descrizione + "',"
                        + " CFTYP='" + lib.attributo + "' "
                        + "WHERE  CFLIB = '" + lib.nome + "'";
                        // inutile mettere il campo CFOBJ a L nell'update
                         
                       

                        rowset.setCommand(query);
                        rowset.execute();
                    }
                } 
  

                rowset.close();
            }
            else
                return "Error";

            return "Ok";

            #endregion

        }

        [WebMethod(Description = "Salvataggio Schedulazione LIBRERIE", EnableSession = false)]
        public String Save_Sched_lib(string JsonSched)
        {

            Sched Sched = new JavaScriptSerializer().Deserialize<Sched>(JsonSched);// non funziona con le liste ma con gli array


            Sched lib = Sched;

            #region QUERY DI INSERT LIBRERIE

            AS400JDBCRowSet rowset = FunzioniComuni.connectJdbcAS400(ConfigurationManager.AppSettings["as400Server"],
                    ConfigurationManager.AppSettings["as400User"], ConfigurationManager.AppSettings["as400Pwd"]);
         


            String data = DateTime.Now.ToString("yyyyMMdd");
            String ora = DateTime.Now.ToString("HHmmss");
            String query = String.Empty;

            if (rowset != null)
            {
                

               // faccio l'update delle schedulazioni


                query = " UPDATE PMBRS.SAVCF00F SET"
                + " CFTS1='" + lib.tipo1 + "',"
                + " CFHS1='" + lib.timeSched1.Replace(":","") + "',"
                + " CFTS2='" + lib.tipo2 + "',"
                + " CFHS2='" + lib.timeSched2.Replace(":", "") + "',"
                + " CFTS3='" + lib.tipo3 + "',"
                + " CFHS3='" + lib.timeSched3.Replace(":", "") + "',"
                + " CFTS4='" + lib.tipo4 + "',"
                + " CFHS4='" + lib.timeSched4.Replace(":", "") + "',"
                + " CFTS5='" + lib.tipo5 + "',"
                + " CFHS5='" + lib.timeSched5.Replace(":", "") + "',"
                + " CFTS6='" + lib.tipo6 + "',"
                + " CFHS6='" + lib.timeSched6.Replace(":", "") + "',"
                + " CFTS7='" + lib.tipo7 + "',"
                + " CFHS7='" + lib.timeSched7.Replace(":", "") + "',"
                + " CFTS8='" + lib.tipo8 + "',"
                + " CFHS8='" + lib.timeSched8.Replace(":", "") + "',"
                + " CFTS9='" + lib.tipo9 + "',"
                + " CFHS9='" + lib.timeSched9.Replace(":", "") + "' "
                + "WHERE CFLIB ='" + lib.libreria + "'";
      

                rowset.setCommand(query);
                rowset.execute(); 
                rowset.close();
            }
            else
                return "Error";

            return "Ok";

            #endregion

        }

        [WebMethod(Description = "Salvataggio Schedulazione IFS", EnableSession = false)]
        public String Save_Sched_IFS(string JsonSched)
        {

            Sched Sched = new JavaScriptSerializer().Deserialize<Sched>(JsonSched);// non funziona con le liste ma con gli array


            Sched ifsSched = Sched;

            #region QUERY DI INSERT LIBRERIE

            AS400JDBCRowSet rowset = FunzioniComuni.connectJdbcAS400(ConfigurationManager.AppSettings["as400Server"],
                    ConfigurationManager.AppSettings["as400User"], ConfigurationManager.AppSettings["as400Pwd"]);



            String data = DateTime.Now.ToString("yyyyMMdd");
            String ora = DateTime.Now.ToString("HHmmss");
            String query = String.Empty;

            if (rowset != null)
            {


                // faccio l'update delle schedulazioni


                query = " UPDATE PMBRS.SAVCF00F SET"
                + " CFTS1='" + ifsSched.tipo1 + "',"
                + " CFHS1='" + ifsSched.timeSched1.Replace(":", "") + "',"
                + " CFTS2='" + ifsSched.tipo2 + "',"
                + " CFHS2='" + ifsSched.timeSched2.Replace(":", "") + "',"
                + " CFTS3='" + ifsSched.tipo3 + "',"
                + " CFHS3='" + ifsSched.timeSched3.Replace(":", "") + "',"
                + " CFTS4='" + ifsSched.tipo4 + "',"
                + " CFHS4='" + ifsSched.timeSched4.Replace(":", "") + "',"
                + " CFTS5='" + ifsSched.tipo5 + "',"
                + " CFHS5='" + ifsSched.timeSched5.Replace(":", "") + "',"
                + " CFTS6='" + ifsSched.tipo6 + "',"
                + " CFHS6='" + ifsSched.timeSched6.Replace(":", "") + "',"
                + " CFTS7='" + ifsSched.tipo7 + "',"
                + " CFHS7='" + ifsSched.timeSched7.Replace(":", "") + "',"
                + " CFTS8='" + ifsSched.tipo8 + "',"
                + " CFHS8='" + ifsSched.timeSched8.Replace(":", "") + "',"
                + " CFTS9='" + ifsSched.tipo9 + "',"
                + " CFHS9='" + ifsSched.timeSched9.Replace(":", "") + "' "
                + "WHERE CFIFS ='" + ifsSched.libreria + "'";


                rowset.setCommand(query);
                rowset.execute();
                rowset.close();
            }
            else
                return "Error";

            return "Ok";

            #endregion

        }

        [WebMethod(Description = "Lettura Schedulazioni", EnableSession = false)]
        public String Get_Sched_lib(string JsonSched)
        {

            Sched Sched = new JavaScriptSerializer().Deserialize<Sched>(JsonSched);// non funziona con le liste ma con gli array


            Sched lib = Sched;

            #region QUERY DI SELECT LIBRERIE

            AS400JDBCRowSet rowset = FunzioniComuni.connectJdbcAS400(ConfigurationManager.AppSettings["as400Server"],
                    ConfigurationManager.AppSettings["as400User"], ConfigurationManager.AppSettings["as400Pwd"]);



            String data = DateTime.Now.ToString("yyyyMMdd");
            String ora = DateTime.Now.ToString("HHmmss");
            String query = String.Empty;

            if (rowset != null)
            {


                // faccio l'update delle schedulazioni


                query = " SELECT CFTS1, CFHS1, CFTS2, CFHS2,CFTS3, CFHS3, CFTS4, CFHS4, CFTS5, CFHS5,CFTS6,CFHS6"
                        + ",CFTS7,CFHS7,CFTS8,CFHS8,CFTS9,CFHS9"
                        + " FROM PMBRS.SAVCF00F "
                        + "WHERE CFLIB ='" + lib.libreria + "'";


                rowset.setCommand(query);
                rowset.execute();

                rowset.close();
            }
            else
                return "Error";

            return "Ok";

            #endregion

        }

        [WebMethod(Description = "Lettura librerie salvate", EnableSession = false)]
        public List<Library> Read_Saved_libraries()
        {
            List<Library> rs = new List<Library> { };

            AS400JDBCRowSet rowset = FunzioniComuni.connectJdbcAS400(ConfigurationManager.AppSettings["as400Server"],
                ConfigurationManager.AppSettings["as400User"], ConfigurationManager.AppSettings["as400Pwd"]);

            if (rowset != null)
            {


                rowset.setCommand("SELECT CFLIB,CFCPR,CFZIP,CFDES,CFTYP FROM PMBRS.SAVCF00F where CFOBJ='L'");
                rowset.execute();// inserisco

                // lettura rowset
                while (rowset.next())
                {

                    Library l = new Library();
                    l.nome = rowset.getString("CFLIB");
                    l.Descrizione = rowset.getString("CFDES");
                    l.attributo = rowset.getString("CFTYP");

                    if (rowset.getString("CFCPR").ToUpper().Trim() == "Y") l.compressione = "checked"; else l.compressione = "";
                    if (rowset.getString("CFZIP").ToUpper().Trim() == "Y") l.zip = "checked"; else l.zip = "";

                    rs.Add(l);
                }

                rowset.close();
            }
            else
                return null;
            return rs;
        }

        #endregion
         

        #region INTERACTIVE USERS
        [WebMethod(Description = "Lettura proprietà utenti connessi", EnableSession = false)]
        // [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<InteractiveUser> GetInteractiveUsers()
        {
            List<InteractiveUser> results = new List<InteractiveUser> { };
            AS400 server = null;
            try
            {
                server = (AS400)Session["iseries_" + ConfigurationManager.AppSettings["as400Server"]];
            }
            catch (Exception)
            { }

            if (server == null)
            {
                server = new AS400();
                server.setSystemName(ConfigurationManager.AppSettings["as400Server"]);
                server.setUserId(ConfigurationManager.AppSettings["as400User"]); //utente con diritti *ALLOBJ
                server.setPassword(ConfigurationManager.AppSettings["as400Pwd"]);
               // Session.Add("iseries_" + ConfigurationManager.AppSettings["as400Server"], server);
            }

            JobList list = new JobList(server);
            /// gettype:
            /// " " - The job is not a valid job. 
            ///    "A" - The job is an autostart job. 
            ///    "B" - The job is a batch job. 
            ///    "I" - The job is an interactive job. 
            ///    "M" - The job is a subsystem monitor job. 
            ///    "R" - The job is a spooled reader job. 
            ///    "S" - The job is a system job. 
            ///    "W" - The job is a spooled writer job. 
            ///    "X" - The job is a SCPF system job.
            ///    "V" - SLIC task - check product licenses
            ///    
            // list.addJobSelectionCriteria(JobList.SELECTION_JOB_NAME,jobName);// ritorna gli *active
            // Valori multipli
            list.addJobSelectionCriteria(JobList.SELECTION_JOB_TYPE, Job.JOB_TYPE_INTERACTIVE);
            list.addJobSelectionCriteria(JobList.SELECTION_PRIMARY_JOB_STATUS_OUTQ, java.lang.Boolean.FALSE);

       
            Enumeration items = list.getJobs();

            while (items.hasMoreElements())
            {
                Job job = (Job)items.nextElement();

                job.loadInformation();

                String Active_Status = job.getValue(Job.ACTIVE_JOB_STATUS).ToString();// stato attuale tipo MSGW e altri
                String variable = job.getValue(Job.CURRENT_LIBRARY).ToString();
                
                // formattazione data
                SimpleDateFormat ft = new SimpleDateFormat("dd.MM.yyyy hh:mm:ss");
                String dataAccesso=ft.format(job.getJobEnterSystemDate());
                //////

                var user = new InteractiveUser
                {
                    JobUser = job.getName().Trim(),
                    UserName = job.getUser().Trim(),
                    Status = job.getStatus(),
                    ActiveStatus = Active_Status,
                    LogonDateTime = dataAccesso
                };





                results.Add(user);

            }
            

           // con lista ordinata per nome
            return results.OrderBy(o => o.UserName).ToList(); //non è chiamato con oggetti kendo ma con normale json quindi devo serializzare il risultato
        }
        #endregion

          
        #region FUNZIONI PER FTP
        [WebMethod(Description = "Lista file in FTP", EnableSession = false)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //string FTPAddress, string SourcefilePath, string username, string password
        public List<FileStruct> FTP_LISTFILE()
        {



            FtpServer ftpServer = Get_FTP_Parameters(); ;// (FtpServer)Session["ftpServer_" + ConfigurationManager.AppSettings["as400Server"]];
            FileStruct f_error = new FileStruct();

            List<FileStruct> strList_ibm = new List<FileStruct> { };
            List<FileStruct> strList_zip = new List<FileStruct> { };
            List<FileStruct> strList_inf = new List<FileStruct> { };

            List<FileStruct> strList_merge = new List<FileStruct> { };


            try
            {
                String BinDirectory = System.AppDomain.CurrentDomain.BaseDirectory + "bin";
                FileInfo WinSCP = new FileInfo(BinDirectory + "winscp.exe");
                if (!WinSCP.Exists)
                {

                    String DirParent = Directory.GetParent(BinDirectory).FullName;

                    DirectoryInfo dirWinSCP = new DirectoryInfo(DirParent + "\\" + "winscp");
                    FileInfo WinSCPinBin = new FileInfo(BinDirectory + "\\" + "winscp.exe");
                    if (dirWinSCP.Exists && !WinSCPinBin.Exists)
                    {
                        File.Copy(dirWinSCP + "\\" + "WinSCP.exe", BinDirectory + "\\" + "WinSCP.exe");

                    }


                }
            }
            catch (Exception ex)
            {

                strList_merge.Clear();
                f_error.Name = f_error.Name + " " + ex.Message;
                strList_merge.Add(f_error);
                return strList_merge;
            }

            // Setup session options
            SessionOptions sessionOptions = new SessionOptions
            {
                PortNumber = Convert.ToInt32(ftpServer.port),
                HostName = ftpServer.ftpAddress,
                UserName = ftpServer.user,
                Password = ftpServer.pwd
            };

            try
            {

                if (ftpServer.ftpSecure == "F")
                {
                    sessionOptions.Protocol = Protocol.Ftp;
                }
                else
                {
                    sessionOptions.Protocol = Protocol.Sftp;
                    sessionOptions.SshHostKeyFingerprint = ftpServer.ftpSecureKey;
                }
                RemoteFileInfo firstFileCatch=null;
                using (Session session = new Session())
                {
                    // Connect
                    session.Open(sessionOptions);

                    RemoteDirectoryInfo directory = session.ListDirectory(ftpServer.ftpPath);
                    string nameFileIBM = String.Empty; //per controllare la coppia di file .ibm e .inf
                    string nameFileINF = String.Empty; //per controllare la coppia di file .ibm e .inf


                    int count = 0;
                    //Console.WriteLine("{0} with size {1}, permissions {2} and last modification at {3}",
                    //    fileInfo.Name, fileInfo.Length, fileInfo.FilePermissions, fileInfo.LastWriteTime);
                    DateTime UltimoGG = DateTime.Now.Date;

                    var dirInf = directory.Files.Where(item => Path.GetExtension(item.Name).ToLower() == ".inf");

                    foreach (RemoteFileInfo fileInfo in dirInf.OrderByDescending(item => item.LastWriteTime))
                    {

                        FileStruct f = ParseFileStructFromWindowsStyleRecord(fileInfo);
                        // ricavo l'ultimo gg dal primo file essendo in ordine se  la data + recente è minore della data attuale
                        if (fileInfo.LastWriteTime.Date < DateTime.Now.Date && count == 0)
                            UltimoGG = fileInfo.LastWriteTime.Date;// DateTime.Now.AddDays(-1).Date;

                        if (fileInfo.LastWriteTime.Date == UltimoGG)
                        {

                            //if (Path.GetExtension(f.Name).ToLower() == ".ibm")
                            //{
                            //    strList_ibm.Add(f);
                            //}
                            //if (Path.GetExtension(f.Name).ToLower() == ".zip")
                            //{
                            //    strList_zip.Add(f);
                            //}
                            if (Path.GetExtension(fileInfo.Name).ToLower() == ".inf" && f!= null)
                            {
                                strList_inf.Add(f);
                            }

                        }
                        else
                        {
                            firstFileCatch = fileInfo;
                            break;
                        }
                        count++;
                        //if (count > 99) break;
                    }
                    
                }// fine ricerca file


                #region ACCOPPIO I FILE
                /*
                 // accoppio i file
                 foreach(FileStruct f in strList_inf.OrderByDescending(item => item.TransferTime))
                 {
                     FileStruct f_ibm = strList_ibm.Find(item => Path.GetFileNameWithoutExtension(item.Name) == Path.GetFileNameWithoutExtension(f.Name));

                     if (f_ibm != null)
                     {

                         strList_merge.Add(f_ibm);
                         strList_merge.Add(f);

                     }
                     else
                     {

                         FileStruct f_zip = strList_zip.Find(item => Path.GetFileNameWithoutExtension(item.Name) == Path.GetFileNameWithoutExtension(f.Name));
                         if(f_zip!= null)
                         {
                             strList_merge.Add(f_zip);
                             strList_merge.Add(f);
                         }
                         else
                         {
                             f_error.Name = "Errore: Manca il file " + Path.GetFileNameWithoutExtension(f.Name) + ".ibm o .zip";
                             strList_merge.Add(f_error);
                         }


                     }

                 }

     */
                #endregion

                if(strList_inf.Count==0)
                    strList_inf.Add(new FileStruct {Name="No Data. Last file has data: " + firstFileCatch.LastWriteTime.ToString() });
                return strList_inf;

            }
            catch (Exception ex)
            {
                strList_merge.Clear();
                f_error.Name = f_error.Name + " " + ex.Message + "\n" + sessionOptions.HostName.ToString();
                strList_merge.Add(f_error);
                return strList_merge;
            }
        }

        [WebMethod(Description = "Check FTP", EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String FTP_CHECK(String JsonFtp)
        {

            FtpParameters parFtp = new JavaScriptSerializer().Deserialize<FtpParameters>(JsonFtp);

            try
            {
                // Setup session options
                SessionOptions sessionOptions = new SessionOptions
                {
                    PortNumber = Convert.ToInt32(parFtp.Port),
                    //PortNumber = 22,
                    HostName = parFtp.Address,
                    UserName = parFtp.User,
                    Password = parFtp.Pwd

                };
                sessionOptions.Protocol = Protocol.Ftp;
                if (parFtp.Secure == "F")
                {
                    sessionOptions.Protocol = Protocol.Ftp;
                }
                else
                {
                    sessionOptions.Protocol = Protocol.Sftp;
                    sessionOptions.SshHostKeyFingerprint = parFtp.Key;
                    // sessionOptions.SshHostKeyFingerprint = "ssh-rsa 1024 ab:8e:df:af:93:d1:5a:e0:04:85:59:92:19:d3:a3:ad";
                }

                using (Session session = new Session())
                {
                    // Connect
                    session.Open(sessionOptions);
                    return "OK";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        public class FileStruct
        {
            public string Flags { get; set; }
            public string Owner { get; set; }
            public string Group { get; set; }
            public bool IsDirectory { get; set; }
            public string TransferTime { get; set; }
            public string Name { get; set; }
            public string Size { get; set; }
            public string CreateTime { get; set; }
            public string CreateTimeFrom { get; set; }
        }

        // Parsifica il risultato della lista FTP e ne estrae i file e i loro paramentri necessari
        private FileStruct ParseFileStructFromWindowsStyleRecord(RemoteFileInfo fileInfo)
        {
            FileStruct f = new FileStruct();
            Regex r_data = null;

            try
            {


                f.TransferTime = Convert.ToString(fileInfo.LastWriteTime);
                f.IsDirectory = fileInfo.IsDirectory;
                f.Size = Convert.ToString(Math.Round(Convert.ToDecimal(fileInfo.Length) / 1024, 3));// in Kb
                f.Name = fileInfo.Name;

                string dateFileCreate = string.Empty;
                string timeFileCreate = string.Empty;
                string dateFileFrom = string.Empty;
                string timeFileFrom = string.Empty;

                //es. astaldi_01-20130506-145300-T
                r_data = new Regex(@"^(.*)-(\d+)-(\d+)-T\.(inf|ibm|INF|IBM|ZIP|zip)$");
                Match createTime = r_data.Match(f.Name);
                if (createTime.Success)
                {
                    dateFileCreate = createTime.Groups[2].Value;
                    timeFileCreate = createTime.Groups[3].Value;
                    f.CreateTime = dateFileCreate.Substring(0, 4) + "-" + dateFileCreate.Substring(4, 2) + "-" + dateFileCreate.Substring(6, 2) + " " + timeFileCreate.Substring(0, 2) + " " + timeFileCreate.Substring(2, 2) + " " + timeFileCreate.Substring(4, 2);
                    f.CreateTimeFrom = "";
                    return f;
                }
                else
                {
                    r_data = new Regex(@"^(.*)-(\d+)-(\d+)-I-(\d+)-(\d+)\.(inf|ibm|INF|IBM|ZIP|zip)$");// T I .ibm .inf .zip
                    createTime = r_data.Match(f.Name);
                    if (createTime.Success)
                    {
                        dateFileCreate = createTime.Groups[2].Value;
                        timeFileCreate = createTime.Groups[3].Value;
                        dateFileFrom = createTime.Groups[4].Value;
                        timeFileFrom = createTime.Groups[5].Value;

                        f.CreateTime = dateFileCreate.Substring(0, 4) + "-" + dateFileCreate.Substring(4, 2) + "-" + dateFileCreate.Substring(6, 2) + " " + timeFileCreate.Substring(0, 2) + " " + timeFileCreate.Substring(2, 2) + " " + timeFileCreate.Substring(4, 2);
                        f.CreateTimeFrom = dateFileFrom.Substring(0, 4) + "-" + dateFileFrom.Substring(4, 2) + "-" + dateFileFrom.Substring(6, 2) + " " + timeFileFrom.Substring(0, 2) + " " + timeFileFrom.Substring(2, 2) + " " + timeFileFrom.Substring(4, 2);
                        return f;
                    }
                    else
                        return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }


        //CREATE AN FTP REQUEST WITH THE DOMAIN AND CREDENTIALS
        [WebMethod(Description = "Read file FTP content", EnableSession = true)]
        public string ReadFtpFile(string file)
        {
            List<FileDetails> righe = new List<FileDetails> { }; ;
            try
            {
                FtpServer ftpServer = (FtpServer)Session["ftpServer_" + ConfigurationManager.AppSettings["as400Server"]];

                System.Net.FtpWebRequest tmpReq = (System.Net.FtpWebRequest)System.Net.FtpWebRequest.Create(ftpServer.ftpAddressURI + ftpServer.ftpPath + "/" + file);
                tmpReq.Credentials = new System.Net.NetworkCredential(ftpServer.user, ftpServer.pwd);

                //GET THE FTP RESPONSE
                using (System.Net.WebResponse tmpRes = tmpReq.GetResponse())
                {
                    //GET THE STREAM TO READ THE RESPONSE FROM
                    using (System.IO.Stream tmpStream = tmpRes.GetResponseStream())
                    {
                        //CREATE A TXT READER (COULD BE BINARY OR ANY OTHER TYPE YOU NEED)
                        using (StreamReader tmpReader = new StreamReader(tmpStream))
                        {

                            String linea = String.Empty;

                            while (!tmpReader.EndOfStream)
                            {
                                linea = tmpReader.ReadLine();
                                //0,10,20,44,139,172,180,190,215,236,326,336,346

                                righe.Add(new FileDetails
                                {
                                    Campo1 = linea.Substring(0, 10), //SAVLIB 
                                    Campo2 = linea.Substring(10, 10), //*OBJ
                                    Campo3 = linea.Substring(20, 8), //Serial Number o nome server
                                    Campo4 = linea.Substring(28, 6), //versione so
                                    Campo5 = linea.Substring(34, 10), //lib (2)
                                    Campo6 = linea.Substring(44, 95), //
                                    //data e ora (1) - es. 1130506144610 linea.Substring(139, 13)
                                    Campo7 = "20" + linea.Substring(140, 2) + "-" + linea.Substring(142, 2) + "-" + linea.Substring(144, 2) + " " + linea.Substring(146, 2) + ":" + linea.Substring(148, 2) + ":" + linea.Substring(150, 2),
                                    Campo8 = linea.Substring(152, 20), //file (3)
                                    Campo9 = linea.Substring(172, 8), //tipo oggetto (4)
                                    Campo10 = linea.Substring(180, 10), // tipo oggetto (5)
                                    Campo11 = linea.Substring(190, 25), //
                                    Campo12 = linea.Substring(215, 21), //
                                    Campo13 = linea.Substring(236, 90), //testo (6)
                                    Campo14 = linea.Substring(326, 10), //
                                    Campo15 = linea.Substring(336, 10), //
                                    Campo16 = linea.Substring(346, linea.Length - 1 - 346) //
                                });
                            }
                            tmpReader.Close();
                        }
                    }
                }
                return new JavaScriptSerializer().Serialize(righe);
            }
            catch (Exception ex)
            {
                //return ex.Message;
                return "Errore: File " + file + " not found or format error (" + ex.Message + ")";
            }
        }



        /// <summary>
        /// download del file e lettura da finire
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>  
        [WebMethod(Description = "Read file FTP and SFTP content", EnableSession = false)]
        public string ReadFtpFile2(string file)
        {
            List<FileDetails> righe = new List<FileDetails> { };
            try
            {
                FtpServer ftpServer =Get_FTP_Parameters();// (FtpServer)Session["ftpServer_" + ConfigurationManager.AppSettings["as400Server"]];
                // Setup session options
                SessionOptions sessionOptions = new SessionOptions
                {
                    PortNumber = Convert.ToInt32(ftpServer.port),
                    HostName = ftpServer.ftpAddress,
                    UserName = ftpServer.user,
                    Password = ftpServer.pwd
                };

                if (ftpServer.ftpSecure == "F")
                {
                    sessionOptions.Protocol = Protocol.Ftp;
                }
                else
                {
                    sessionOptions.Protocol = Protocol.Sftp;
                    sessionOptions.SshHostKeyFingerprint = ftpServer.ftpSecureKey;
                    // sessionOptions.SshHostKeyFingerprint = "ssh-rsa 1024 ab:8e:df:af:93:d1:5a:e0:04:85:59:92:19:d3:a3:ad";
                }


                using (Session session = new Session())
                {
                    session.DisableVersionCheck=true;
                    // Connect
                    session.Open(sessionOptions);

                    string stamp = DateTime.Now.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
                    string fileName = file;
                    string remotePath = ftpServer.ftpPath + "/" + fileName;
                    string localPath = Server.MapPath(".") + "\\tmpInf\\" + fileName;


                    // Download the file and throw on any error
                    session.GetFiles(remotePath, localPath).Check();

                    //CREATE A TXT READER (COULD BE BINARY OR ANY OTHER TYPE YOU NEED)
                    using (StreamReader tmpReader = new StreamReader(localPath))
                    {

                        String linea = String.Empty;

                        while (!tmpReader.EndOfStream)
                        {
                            linea = tmpReader.ReadLine();
                            //0,10,20,44,139,172,180,190,215,236,326,336,346

                            righe.Add(new FileDetails
                            {
                                Campo1 = linea.Substring(0, 10), //SAVLIB 
                                Campo2 = linea.Substring(10, 10), //*OBJ
                                Campo3 = linea.Substring(20, 8), //Serial Number o nome server
                                Campo4 = linea.Substring(28, 6), //versione so
                                Campo5 = linea.Substring(34, 10), //lib (2)
                                Campo6 = linea.Substring(44, 95), //
                                //data e ora (1) - es. 1130506144610 linea.Substring(139, 13)
                                Campo7 = "20" + linea.Substring(140, 2) + "-" + linea.Substring(142, 2) + "-" + linea.Substring(144, 2) + " " + linea.Substring(146, 2) + ":" + linea.Substring(148, 2) + ":" + linea.Substring(150, 2),
                                Campo8 = linea.Substring(152, 20), //file (3)
                                Campo9 = linea.Substring(172, 8), //tipo oggetto (4)
                                Campo10 = linea.Substring(180, 10), // tipo oggetto (5)
                                Campo11 = linea.Substring(190, 25), //
                                Campo12 = linea.Substring(215, 21), //
                                Campo13 = linea.Substring(236, 90), //testo (6)
                                Campo14 = linea.Substring(326, 10), //
                                Campo15 = linea.Substring(336, 10), //
                                Campo16 = linea.Substring(346, linea.Length - 1 - 346) //
                            });
                            if (righe.Count > 500) break;
                        }
                        tmpReader.Close();
                    }
                    return new JavaScriptSerializer().Serialize(righe);
                }
            }
            catch (Exception e)
            {
                return "Error: " + e.Message;
            }

        }


        public class FileDetails
        {
            public string Campo1 { get; set; }
            public string Campo2 { get; set; }
            public string Campo3 { get; set; }
            public string Campo4 { get; set; }
            public string Campo5 { get; set; }
            public string Campo6 { get; set; }
            public string Campo7 { get; set; }
            public string Campo8 { get; set; }
            public string Campo9 { get; set; }
            public string Campo10 { get; set; }
            public string Campo11 { get; set; }
            public string Campo12 { get; set; }
            public string Campo13 { get; set; }
            public string Campo14 { get; set; }
            public string Campo15 { get; set; }
            public string Campo16 { get; set; }
        }


        // salvataggio parametri FTP
        [WebMethod(Description = "Salvataggio ftp parametri", EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String Save_FTP_Parameters(String JsonFtp)
        {


            FtpParameters parFtp = new JavaScriptSerializer().Deserialize<FtpParameters>(JsonFtp);
            if (!parFtp.Path.StartsWith("/"))
                parFtp.Path = "/" + parFtp.Path;



            FtpServer ftpServer = new FtpServer();
            ftpServer.ftpAddressURI = parFtp.Address;
            ftpServer.ftpAddress = parFtp.Address;
            ftpServer.ftpPath = parFtp.Path;
            ftpServer.port = parFtp.Port;
            ftpServer.user = parFtp.User;
            ftpServer.pwd = parFtp.Pwd;
            ftpServer.ccsid = parFtp.CCSID;
            ftpServer.ftpType = parFtp.Tipo;
            ftpServer.ftpSecureKey = parFtp.Key;
            ftpServer.ftpSecure = parFtp.Secure;


             

            Session.Add("ftpServer_" + ConfigurationManager.AppSettings["as400Server"], ftpServer);// metto in sessione

            #region QUERY DI INSERT FTP

            AS400JDBCRowSet rowset = FunzioniComuni.connectJdbcAS400(ConfigurationManager.AppSettings["as400Server"],
                                    ConfigurationManager.AppSettings["as400User"], ConfigurationManager.AppSettings["as400Pwd"]);
            String data = DateTime.Now.ToString("yyyyMMdd");
            String ora = DateTime.Now.ToString("HHmmss");


            if (rowset != null)
            {
                rowset.setCommand("DELETE FROM PMBRS.FTPCF00F");
                rowset.execute();// prima cancello poi inserisco

                String query = "INSERT INTO PMBRS.FTPCF00F (FTEDT,FTETM,FTIIP,FTCSI,FTPOR,FTUSE,FTPSW,FTPAT,FTTYP,FTFTP,FTKEY) "
                + "VALUES (" + data
                + "," + ora
                + ",'" + parFtp.Address
                + "','" + parFtp.CCSID
                + "','" + parFtp.Port
                + "','" + parFtp.User
                + "','" + parFtp.Pwd
                + "','" + parFtp.Path
                + "','" + parFtp.Tipo
                + "','" + parFtp.Secure
                + "','" + parFtp.Key
                + "')";

                rowset.setCommand(query);
                try
                {
                    rowset.execute();
                }
                catch (Exception ex)
                {

                    return "Error: "  +ex.Message + "  campo FTEDT:" + data.ToString();
                }
               
            }
            else
                return "Error";

 


            return "Ok";

            #endregion

        }



        public FtpServer Get_FTP_Parameters()
        {
            FtpServer Ftp = new FtpServer();

            #region QUERY DI LETTURA PAR FTP

            AS400JDBCRowSet rowset = FunzioniComuni.connectJdbcAS400(ConfigurationManager.AppSettings["as400Server"],
            ConfigurationManager.AppSettings["as400User"], ConfigurationManager.AppSettings["as400Pwd"]);

            String query = "SELECT FTIIP,FTPOR,FTUSE,FTPSW,FTPAT,FTTYP,FTFTP,FTKEY FROM PMBRS.FTPCF00F";

            try
            {
                rowset.setCommand(query);
                rowset.execute();// prima cancello poi inserisco
                while (rowset.next())
                {
                    Ftp.ftpAddress = rowset.getString("FTIIP").Trim();
                    Ftp.port = rowset.getString("FTPOR").Trim();
                    Ftp.user = rowset.getString("FTUSE").Trim();
                    Ftp.pwd = rowset.getString("FTPSW").Trim();
                    Ftp.ftpPath = rowset.getString("FTPAT").Trim();
                    Ftp.ftpType = rowset.getString("FTTYP").Trim();
                    Ftp.ftpSecure = rowset.getString("FTFTP").Trim();
                    Ftp.ftpSecureKey = rowset.getString("FTKEY").Trim();

                }
            }
            catch (Exception ex)
            {
                Ftp.ftpAddress = ex.Message;

                return Ftp;
            }

                #endregion

                return Ftp;
        }
        #endregion


        #region TRASFERIMENTO FILE
        [WebMethod(Description = "Trasferimento FTP", EnableSession = false)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //string FTPAddress, string SourcefilePath, string username, string password
        public String putFile(String FILE)
        {
            String res = String.Empty;
            // chiamo il programma per trasferire il file in oggetto 
            String SERVER=ConfigurationManager.AppSettings["as400Server"];
            String USER= ConfigurationManager.AppSettings["as400User"];
            String PWD= ConfigurationManager.AppSettings["as400Pwd"];

            res=FunzioniComuni.LaunchCommand(SERVER,USER,PWD,FILE);

            return new JavaScriptSerializer().Serialize(res);
        }
        #endregion





            #region CLASSI
            /// <summary>
            /// ///////////////////// CLASSI
            /// </summary>
        public class iSeriesProperties
        {
            public String Name { get; set; }
            public String Serial { get; set; }
        }

        public class iSeriesUser
        {
            public String Name { get; set; }
            public String Descrizione { get; set; }
            public String lastlogon { get; set; }
        }

        public class InteractiveUser
        {
            public String UserName { get; set; }
            public String JobUser { get; set; }
            public String Status { get; set; }
            public String ActiveStatus { get; set; }
            public String LogonDateTime { get; set; }
        }

        public class JobsSummary
        {
            public String Status { get; set; }
            public float Number { get; set; }
        }

        public class FtpFile
        {
            public String Name { get; set; }       
        }

        public class Library
        {
            public String nome { get; set; }
            public String Descrizione { get; set; }
            public String attributo { get; set; }
            public String compressione { get; set; }
            public String zip { get; set; }
            public Sched scheds { get; set; }
        }

        public class Sched
        {
            public String libreria { get; set; }
            public String timeSched1 { get; set; }
            public String tipo1 { get; set; }
            public String timeSched2 { get; set; }
            public String tipo2 { get; set; }
            public String timeSched3 { get; set; }
            public String tipo3 { get; set; }
            public String timeSched4 { get; set; }
            public String tipo4 { get; set; }
            public String timeSched5 { get; set; }
            public String tipo5 { get; set; }
            public String timeSched6 { get; set; }
            public String tipo6 { get; set; }
            public String timeSched7 { get; set; }
            public String tipo7 { get; set; }
            public String timeSched8 { get; set; }
            public String tipo8 { get; set; }
            public String timeSched9 { get; set; }
            public String tipo9 { get; set; }
        }

        public class FtpParameters
        {
            public String Address { get; set; }
            public String Port { get; set; }
            public String User { get; set; }
            public String Pwd { get; set; }
            public String Path { get; set; }
            public String Tipo { get; set; }
            public String CCSID { get; set; }
            public String Secure { get; set; }
            public String Key { get; set; }
            
        }

        public class result_search
        {
            public String NOMEFA;
            public String SROMNM;
            public String SROTYP;
            public String SROLIB;
            public String SROSVT; 
        }
        public class result_search_IFS
        {
            public String NOMEFA;
            public String SRDIR;
            public String SRTIP;
            public String SROBJ;
            public String SRDAT;
            public String SRTIM;
        }

        public class result_search_QDLS
        {//NOMEFA,SDLDOC,SDLPTH,SDLDSC,SDLDAT,SDLTIM 
            public String NOMEFA;
            public String SDLDOC;
            public String SDLPTH;
            public String SDLDSC;
            public String SDLTYP;
            public String SDLDAT;
            public String SDLTIM;
        }

        public class result_search_ErroriLib
        {
            public String SROSVT;
            public String SROLIB;
            public String SRONAM;
            public String SROTYP;
            public String SROCMD;
            public String NOMEFA;

        }
        #endregion

    }
}
