using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.ibm.as400.access;
using java.util;
using java.text;
using System.Net;
using System.IO;
using java.sql;
using System.Configuration;
using System.Threading;
using System.Web.Services;
using System.Web.Script.Serialization;

namespace BRSi
{
    public partial class Scheduled : System.Web.UI.Page
    {

        public List<Library> lista_tot;
        public List<Library> lista_parziale = new List<Library> { };


        protected void Page_Load(object sender, EventArgs e) 
        { 
            if (!IsPostBack) 
            {
                lista_tot = Read_Saved_libraries(" where cflib<>''");// leggo tutte le librerie salvate
                DDLlibrary.DataSource = lista_tot;
                DDLlibrary.DataTextField = "nome";
                DDLlibrary.DataValueField = "nome";
                DDLlibrary.DataBind();

                lblNumLib.Text = lista_tot.Count.ToString() + " Librerie trovate";
                ViewState.Add("ListaLibrerie",lista_tot);
                ViewState.Add("ListaParziale", lista_parziale);
                BindRepeater(true);
            } 
            else
            {
                lista_tot = (List<Library>)ViewState["ListaLibrerie"];
                lista_parziale = (List<Library>)ViewState["ListaParziale"];               
            }

        }

        private void BindRepeater(Boolean forward)
        {



            int pag = 5;
            int from=0;

            int to = from + pag;
 
            switch (forward)
            {
                case true:// avanti 
                    if (lista_parziale.Count > 0)
                    {
                        from = lista_parziale[lista_parziale.Count - 1].id;
                        to = from + pag;
                        if (to > lista_tot.Count - 1)
                        {
                            to = lista_tot.Count - 1;
                            BtnNext.Enabled = false;
                        }
                        else
                            BtnNext.Enabled = true;

                        lista_parziale = lista_tot.Where(Library => Library.id >= from && Library.id < to).ToList();// primi 10
                        BtnPrev.Enabled = true;
                    }
                    else
                        lista_parziale = lista_tot.Where(Library => Library.id >= from && Library.id < to).ToList();// primi 10
                    break;
                case false:// indietro
                    if (lista_parziale.Count > 0)
                    {
                        from = lista_parziale[0].id;
                        to = from - pag;// al contrario
                        if (to < 0)
                        {
                            to = 0;
                            BtnPrev.Enabled = false;
                        }
                        else
                            BtnPrev.Enabled = true;

                        lista_parziale = lista_tot.Where(Library => Library.id < from && Library.id >= to).ToList();// primi n librerie
                    }
                  
                       
                    

                    break;
            }



            if (lista_parziale.Count > 0)
            {
                String LibTo = String.Empty;
                String LibFrom = lista_parziale[0].nome;

                if (lista_parziale.Count < pag - 2)
                    LibTo = lista_parziale[lista_parziale.Count - 1].nome;
                else
                    LibTo = lista_parziale[pag - 2].nome;

                Lblpage.Text = "da " + LibFrom + " a " + LibTo;

                Repeater1.DataSource = lista_parziale;
                Repeater1.DataBind();
                ViewState["ListaParziale"] = lista_parziale;
            }
           
        }



        protected void BtnNext_Click(object sender, EventArgs e)
        {
           
            lista_tot = Read_Saved_libraries(" where cflib<>''");// leggo tutte le librerie salvate
            ViewState["ListaLibrerie"] = lista_tot;
            BindRepeater(true) ;           

            ScriptManager.RegisterClientScriptBlock(UpdPnl, UpdPnl.GetType(), "r", "reload2()", true);

            Waitpages.Visible = false;
        }
        protected void BtnPrev_Click(object sender, EventArgs e)
        {
            lista_tot = Read_Saved_libraries(" where CFLIB <>''");// leggo tutte le librerie salvate
            ViewState["ListaLibrerie"] = lista_tot;
            BindRepeater(false);

            ScriptManager.RegisterClientScriptBlock(UpdPnl, UpdPnl.GetType(),"r", "reload2()", true);
        }

        public List<Library> Read_Saved_libraries(String Where)
        {
            List<Library> rs = new List<Library> { };
            String nome = String.Empty;

            AS400JDBCRowSet rowset = FunzioniComuni.connectJdbcAS400(ConfigurationManager.AppSettings["as400Server"],
                ConfigurationManager.AppSettings["as400User"], ConfigurationManager.AppSettings["as400Pwd"]);

            if (rowset != null)
            {


                rowset.setCommand("SELECT * FROM PMBRS.SAVCF00F " + Where);

                rowset.execute();// inserisco
                int cont = 0;
                // lettura rowset
                while (rowset.next())
                {

                    Library l = new Library();

                    l.nome = rowset.getString("CFLIB").Trim();
                    l.Descrizione = rowset.getString("CFDES").Trim();
                    l.scheds = new Sched();
                    l.scheds.tipo1 = rowset.getString("CFTS1").Trim();
                    l.scheds.tipo2 = rowset.getString("CFTS2").Trim();
                    if (l.scheds.tipo2 == String.Empty) l.scheds.tipo2 = "I";

                    l.scheds.tipo3 = rowset.getString("CFTS3").Trim();
                    if (l.scheds.tipo3 == String.Empty) l.scheds.tipo3 = "I";

                    l.scheds.tipo4 = rowset.getString("CFTS4").Trim();
                    if (l.scheds.tipo4 == String.Empty) l.scheds.tipo4 = "I";

                    l.scheds.tipo5 = rowset.getString("CFTS5").Trim();
                    if (l.scheds.tipo5 == String.Empty) l.scheds.tipo5 = "I";

                    l.scheds.tipo6 = rowset.getString("CFTS6").Trim();
                    if (l.scheds.tipo6 == String.Empty) l.scheds.tipo6 = "I";

                    l.scheds.tipo7 = rowset.getString("CFTS7").Trim();
                    if (l.scheds.tipo7 == String.Empty) l.scheds.tipo7 = "I";

                    l.scheds.tipo8 = rowset.getString("CFTS8").Trim();
                    if (l.scheds.tipo8 == String.Empty) l.scheds.tipo8 = "I";

                    l.scheds.tipo9 = rowset.getString("CFTS9").Trim();
                    if (l.scheds.tipo9 == String.Empty) l.scheds.tipo9 = "I";

                    l.scheds.timeSched1 = rowset.getString("CFHS1").Substring(0, 2) + ":" + rowset.getString("CFHS1").Substring(2, 2);
                    if (l.scheds.timeSched1 == String.Empty) { l.scheds.timeSched1 = "00:00"; l.scheds.enable1 = "false"; }
                    else l.scheds.enable1 = "true";

                    l.scheds.timeSched2 = rowset.getString("CFHS2").Substring(0, 2) + ":" + rowset.getString("CFHS2").Substring(2, 2);
                    if (l.scheds.timeSched2.Replace(":", "").Trim() == String.Empty) { l.scheds.timeSched2 = "00:00"; l.scheds.enable2 = "false"; }
                    else l.scheds.enable2 = "true";

                    l.scheds.timeSched3 = rowset.getString("CFHS3").Substring(0, 2) + ":" + rowset.getString("CFHS3").Substring(2, 2);
                    if (l.scheds.timeSched3.Replace(":", "").Trim() == String.Empty) { l.scheds.timeSched3 = "00:00"; l.scheds.enable3 = "false"; }
                    else l.scheds.enable3 = "true";

                    l.scheds.timeSched4 = rowset.getString("CFHS4").Substring(0, 2) + ":" + rowset.getString("CFHS4").Substring(2, 2); 
                    if (l.scheds.timeSched4.Replace(":","").Trim() == String.Empty) { l.scheds.timeSched4 = "00:00";l.scheds.enable4 = "false"; }
                    else l.scheds.enable4 = "true";

                    l.scheds.timeSched5 = rowset.getString("CFHS5").Substring(0, 2) + ":" + rowset.getString("CFHS5").Substring(2, 2);
                    if (l.scheds.timeSched5.Replace(":", "").Trim() == String.Empty) { l.scheds.timeSched5 = "00:00"; l.scheds.enable5 = "false"; }
                    else l.scheds.enable5 = "true";

                    l.scheds.timeSched6 = rowset.getString("CFHS6").Substring(0, 2) + ":" + rowset.getString("CFHS6").Substring(2, 2);
                    if (l.scheds.timeSched6.Replace(":", "").Trim() == String.Empty) { l.scheds.timeSched6 = "00:00"; l.scheds.enable6 = "false"; }
                    else l.scheds.enable6 = "true";

                    l.scheds.timeSched7 = rowset.getString("CFHS7").Substring(0, 2) + ":" + rowset.getString("CFHS7").Substring(2, 2);
                    if (l.scheds.timeSched7.Replace(":", "").Trim() == String.Empty) { l.scheds.timeSched7 = "00:00"; l.scheds.enable7 = "false"; }
                    else l.scheds.enable7 = "true";

                    l.scheds.timeSched8 = rowset.getString("CFHS8").Substring(0, 2) + ":" + rowset.getString("CFHS8").Substring(2, 2);
                    if (l.scheds.timeSched8.Replace(":", "").Trim() == String.Empty) { l.scheds.timeSched8 = "00:00"; l.scheds.enable8 = "false"; }
                    else l.scheds.enable8 = "true";

                    l.scheds.timeSched9 = rowset.getString("CFHS9").Substring(0, 2) + ":" + rowset.getString("CFHS9").Substring(2, 2);
                    if (l.scheds.timeSched9.Replace(":", "").Trim() == String.Empty) { l.scheds.timeSched9 = "00:00"; l.scheds.enable9 = "false"; }
                    else l.scheds.enable9 = "true";

                    l.id = cont;// mi serve per conteggiare le librerie e filtrarle successivamente per non caricare la pagina
                    rs.Add(l);
                    cont++;
                }

                rowset.close();
            }
            else
                return null;
            return rs;
        }



        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            lista_tot = Read_Saved_libraries("where CFLIB like '" + Txtsearch.Text.ToUpper() +"%'");// leggo tutte le librerie salvate
            ViewState["ListaLibrerie"] = lista_tot;
           // lista_parziale = lista_tot.Where(Library => Library.nome.Contains(Txtsearch.Text)).ToList();
            Repeater1.DataSource = lista_tot;//lista_parziale;
            Repeater1.DataBind();

            lista_parziale.Clear();

            ScriptManager.RegisterClientScriptBlock(UpdPnl, UpdPnl.GetType(), "r", "reload2()", true);
        }

       

        protected void BtnSel_Click(object sender, EventArgs e)
        {
            lista_tot = Read_Saved_libraries("where trim(CFLIB) = '" + DDLlibrary.Text + "'");// leggo tutte le librerie salvate
            ViewState["ListaLibrerie"] = lista_tot;
           // lista_parziale = lista_tot.Where(Library => Library.nome == DDLlibrary.Text).ToList();
            Repeater1.DataSource = lista_tot;//lista_parziale;
            Repeater1.DataBind();

            lista_parziale.Clear();

            ScriptManager.RegisterClientScriptBlock(UpdPnl, UpdPnl.GetType(), "r", "reload2()", true);
        }


        [WebMethod]
        public static void Save_Parameter_SavSys(String SYFL1,String SYFL2,String SYHS1)
        {
            AS400JDBCRowSet rowset = FunzioniComuni.connectJdbcAS400(ConfigurationManager.AppSettings["as400Server"],
            ConfigurationManager.AppSettings["as400User"], ConfigurationManager.AppSettings["as400Pwd"]);

            if (rowset != null)
            {
                rowset.setCommand("Update PMBRS.SAVSY00F SET SYFL1='"+SYFL1
                    + "',SYFL2='" + SYFL2 + "',SYHS1='" + SYHS1 + "'");

                try
                {
                    rowset.execute();// update
                }
                catch (Exception ex)
                {

                     
                }

            }
        }

        [WebMethod]
        public static String get_Parameter_SavSys()
        {
            COMANDISAV cmdSav = new COMANDISAV();

            AS400JDBCRowSet rowset = FunzioniComuni.connectJdbcAS400(ConfigurationManager.AppSettings["as400Server"],
            ConfigurationManager.AppSettings["as400User"], ConfigurationManager.AppSettings["as400Pwd"]);

            if (rowset != null)
            {


                rowset.setCommand("SELECT SYFL1,SYFL2,SYHS1 FROM PMBRS.SAVSY00F");

                rowset.execute();// lettura

                // lettura rowset
                while (rowset.next())
                {
                    cmdSav.SYFL1 = rowset.getString(1);
                    cmdSav.SYFL2 = rowset.getString(2);
                    cmdSav.SYHS1= rowset.getString(3).Substring(0,2)+":"+ rowset.getString(3).Substring(2, 2);

                }


                
            }


            return new JavaScriptSerializer().Serialize(cmdSav);
        }



        [Serializable]
        public class Library
        {
            public int id { get; set; }
            public String nome { get; set; }
            public String Descrizione { get; set; }
            public String attributo { get; set; }
            public String compressione { get; set; }
            public String zip { get; set; }
            public Sched scheds { get; set; }
             
        }
        [Serializable]
        public class Sched
        {

            public String libreria { get; set; }
            public String timeSched1 { get; set; }
            public String enable1 { get; set; }
            public String tipo1 { get; set; }
            public String enable2 { get; set; }
            public String timeSched2 { get; set; }
            public String tipo2 { get; set; }
            public String enable3 { get; set; }
            public String timeSched3 { get; set; }
            public String tipo3 { get; set; }
            public String enable4 { get; set; }
            public String timeSched4 { get; set; }
            public String tipo4 { get; set; }
            public String enable5 { get; set; }
            public String timeSched5 { get; set; }
            public String tipo5 { get; set; }
            public String enable6 { get; set; }
            public String timeSched6 { get; set; }
            public String tipo6 { get; set; }
            public String enable7 { get; set; }
            public String timeSched7 { get; set; }
            public String tipo7 { get; set; }
            public String enable8 { get; set; }
            public String timeSched8 { get; set; }
            public String tipo8 { get; set; }
            public String enable9 { get; set; }
            public String timeSched9 { get; set; }
            public String tipo9 { get; set; }
        }

        // questi comandi sono per tutte le librerie
        public class COMANDISAV
        {
            public String SYFL1 { get; set; }// SAVSECDTA
            public String SYFL2 { get; set; }// SAVCFG
            public String SYHS1 { get; set; }
        }

    

        ///////////////////////



    }
}