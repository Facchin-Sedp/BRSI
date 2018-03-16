using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BRSi
{
    public partial class SiteMaster : MasterPage
    {
     

        protected void Page_Init(object sender, EventArgs e)
        {

          

            if (Session["user_" + ConfigurationManager.AppSettings["as400Server"]] != null)// faccio apparire il pannello se login
            {
                login.Visible = true;
                Page.Title = "BRSi - " + ConfigurationManager.AppSettings["as400Server"];
            }
            else
            {
                Page.Title = "BRSi";
                login.Visible = false;
            }


        }

      

        protected void Page_Load(object sender, EventArgs e)
        {
    
        }

        protected void LoginView1_ViewChanged(object sender, EventArgs e)
        {

        }

        protected void LoginView1_ViewChanging(object sender, EventArgs e)
        {

        }
    }
}