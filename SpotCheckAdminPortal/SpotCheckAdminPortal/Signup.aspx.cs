using System;
using System.Web;
using System.Web.UI;

namespace SpotCheckAdminPortal
{

    public partial class Signup : System.Web.UI.Page
    {
        protected void LoginInsteadButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("index.aspx");
        }

        protected void SignUpButton_Click(object sender, EventArgs e)
        {

        }
    }
}
