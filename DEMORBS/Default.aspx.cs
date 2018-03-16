using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BRSi
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user_" + ConfigurationManager.AppSettings["as400Server"]] != null)// faccio apparire il pannello se login
            {
                PnlLogin.Visible = true;
                PnlLogout.Visible = false;
            }
            else
            {
                PnlLogin.Visible = false;
                PnlLogout.Visible = true;
            }
        }
    }
}