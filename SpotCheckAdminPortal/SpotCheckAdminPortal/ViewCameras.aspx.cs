using SpotCheckAdminPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SpotCheckAdminPortal
{
   public partial class ViewCameras : System.Web.UI.Page
   {
      #region Properties

      private Company company = new Company();
      private List<Device> devices = new List<Device>();
      private List<ParkingLot> parkingLots = new List<ParkingLot>();

      #endregion End Properties

      #region Events

      protected void Page_Load(object sender, EventArgs e)
      {
         ParkingLot pl = new ParkingLot();

         //get information
         company = IoC.CurrentCompany;
         parkingLots = pl.GetParkingLotListFromCompanyID((int)company.CompanyID);

         bool validate = IoC.ValidateInfo();
         if (validate)
         {
            Response.Redirect("Dashboard.aspx");
         }

         //set label
         CompanyNameLiteral.Text = company.CompanyName;
         if (parkingLots.Count > 0)
         {
            LoadParkingLotComboBox();            
         }
         else
         {
            ShowMessage("warning", "You do not have any parking lots set up yet.");
         }
      }

      #endregion Events

      #region Methods

      private void LoadParkingLotComboBox()
      {
         HtmlGenericControl comboBoxDiv = new HtmlGenericControl("div");
         DropDownList parkingLotDropDownList = new DropDownList();

         ListItem defaultItem = new ListItem();
         defaultItem.Value = null;
         defaultItem.Text = "Select parking lot";
         defaultItem.Enabled = true;
         defaultItem.Selected = true;
         parkingLotDropDownList.Items.Add(defaultItem);

         foreach (ParkingLot parkingLot in parkingLots)
         {
            ListItem parkingLotItem = new ListItem();
            parkingLotItem.Value = ((int)parkingLot.LotID).ToString();
            parkingLotItem.Text = parkingLot.LotName;
            if(parkingLot.GetCamerasDeployed().Count < 1)
            {
               parkingLotItem.Attributes.Add("disabled", "disabled");
               parkingLotItem.Attributes.Add("title", "no cameras deployed here");
            }
            parkingLotItem.Selected = false;
            parkingLotDropDownList.Items.Add(parkingLotItem);
         }         

         HtmlGenericControl text = new HtmlGenericControl("p");
         text.InnerText = "Select parking lot to view cameras.";

         comboBoxDiv.Controls.Add(text);
         comboBoxDiv.Controls.Add(parkingLotDropDownList);
         cameraContainer.Controls.Add(comboBoxDiv);
      }

      private void ShowMessage(string type, string message)
      {
         HtmlGenericControl outerDiv = new HtmlGenericControl("div");
         if (type == "success")
         {
            outerDiv.Attributes.Add("class", "alert alert-success alert-dismissible fade show");
            outerDiv.InnerHtml = message;
         }
         if (type == "warning")
         {
            outerDiv.Attributes.Add("class", "alert alert-warning alert-dismissible fade show");
            outerDiv.InnerHtml = message;
         }
         if (type == "danger")
         {
            outerDiv.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
            outerDiv.InnerHtml = message;
         }

         outerDiv.Attributes.Add("role", "alert");

         HtmlGenericControl btnDismiss = new HtmlGenericControl("button");
         btnDismiss.Attributes.Add("type", "button");
         btnDismiss.Attributes.Add("class", "close");
         btnDismiss.Attributes.Add("data-dismiss", "alert");
         btnDismiss.Attributes.Add("aria-label", "Close");

         HtmlGenericControl span = new HtmlGenericControl("span");
         span.Attributes.Add("aria-hidden", "true");
         span.InnerHtml = "&times;";

         btnDismiss.Controls.Add(span);
         outerDiv.Controls.Add(btnDismiss);

         alertDiv.Controls.Add(outerDiv);
      }

      #endregion Methods


   }
}