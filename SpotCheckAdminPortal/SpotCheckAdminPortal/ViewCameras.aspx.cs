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
      private List<ParkingLot> parkingLots = new List<ParkingLot>();


      private ParkingLot _selectedParkingLot;
      public ParkingLot selectedParkingLot
      {
         get { return _selectedParkingLot; }
         set
         {
            _selectedParkingLot = value;
            currentDeviceList = _selectedParkingLot.GetCamerasDeployed();
         }
      }

      private List<Device> _currentDeviceList;
      public List<Device> currentDeviceList
      {
         get { return _currentDeviceList; }
         set
         {
            _currentDeviceList = value;
            LoadCurrentDeviceListDivs();
         }
      }

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
            string paramParkingLot = Request.QueryString["ParkingLotID"];

            CreateAddModal();

            //if params are passed to page
            if (paramParkingLot != null)
            {
               LoadParkingLotComboBox(paramParkingLot);
            }
            else
            {
               LoadParkingLotComboBox();
            }

         }
         else
         {
            ShowMessage("warning", "You do not have any parking lots set up yet.");
         }
      }

      private void ParkingLotDropDownList_SelectedIndexChanged(object sender, EventArgs e)
      {
         DropDownList list = sender as DropDownList;
         if (list != null)
         {
            ListItem selectedItem = list.SelectedItem;
            if (selectedItem.Value != "0" && selectedItem.Text != "Select parking lot")   //default value
            {
               int parkingLotID = Int32.Parse(selectedItem.Value);
               ParkingLot selectedLot = new ParkingLot
               {
                  LotID = parkingLotID
               };

               selectedLot = selectedLot.Fill();
               if (selectedLot != null)
               {
                  selectedParkingLot = selectedLot;  //save class variable  
               }
            }
         }
      }

      private void btnAddSubmit_Click(object sender, EventArgs e)
      {
         Button addButton = sender as Button;
         //add new camera to database
      }

      #endregion Events

      #region Methods

      private void LoadParkingLotComboBox(string selectedParkingLotID = null)
      {
         bool forcePageUpdate = false;

         HtmlGenericControl comboBoxDiv = new HtmlGenericControl("div");
         DropDownList parkingLotDropDownList = new DropDownList();
         parkingLotDropDownList.ID = "parkingLotDropDownList";
         parkingLotDropDownList.Width = 275;
         parkingLotDropDownList.SelectedIndexChanged += new EventHandler(ParkingLotDropDownList_SelectedIndexChanged);
         parkingLotDropDownList.AutoPostBack = true;
         parkingLotDropDownList.ClientIDMode = ClientIDMode.Inherit;   //removed conflict with autopostback and updatepanel

         //only have default value if there was no passed param
         if (selectedParkingLotID == null)
         {
            ListItem defaultItem = new ListItem();
            defaultItem.Value = "0";
            defaultItem.Text = "Select parking lot";
            defaultItem.Enabled = true;
            defaultItem.Selected = true;
            parkingLotDropDownList.Items.Add(defaultItem);
         }

         foreach (ParkingLot parkingLot in parkingLots)
         {
            ListItem parkingLotItem = new ListItem();
            parkingLotItem.Value = ((int)parkingLot.LotID).ToString();
            parkingLotItem.Text = parkingLot.LotName;

            if (parkingLot.GetCamerasDeployed().Count < 1)
            {
               parkingLotItem.Attributes.Add("disabled", "disabled");
               parkingLotItem.Attributes.Add("title", "no cameras deployed here");
            }
            //if the parkinglot matches our passed param
            if (parkingLotItem.Value == selectedParkingLotID)
            {
               parkingLotItem.Selected = true;
               forcePageUpdate = true;
            }
            else
            {
               parkingLotItem.Selected = false;
            }
            parkingLotDropDownList.Items.Add(parkingLotItem);
            
         }

         HtmlGenericControl text = new HtmlGenericControl("p");
         text.InnerText = "Select parking lot to view cameras.";

         comboBoxDiv.Controls.Add(text);
         comboBoxDiv.Controls.Add(parkingLotDropDownList);
         cameraContainer.Controls.Add(comboBoxDiv);

         if (forcePageUpdate)
         {
            //trigger page update
            ParkingLotDropDownList_SelectedIndexChanged(parkingLotDropDownList, EventArgs.Empty);
         }
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

      public void CreateAddModal()
      {
         HtmlGenericControl div1 = new HtmlGenericControl("div");
         div1.Attributes.Add("class", "modal");
         div1.Attributes.Add("tabindex", "-1");
         div1.Attributes.Add("role", "dialog");
         div1.Attributes.Add("id", "addModal");
         div1.Attributes.Add("style", "z-index:1;");

         HtmlGenericControl div2 = new HtmlGenericControl("div");
         div2.Attributes.Add("class", "modal-dialog modal-dialog-centered");
         div2.Attributes.Add("role", "document");

         HtmlGenericControl div3 = new HtmlGenericControl("div");
         div3.Attributes.Add("class", "modal-content");

         //header controls
         HtmlGenericControl divHeader = new HtmlGenericControl("div");
         divHeader.Attributes.Add("class", "modal-header");

         HtmlGenericControl h5 = new HtmlGenericControl("h5");
         h5.Attributes.Add("class", "modal-title");
         h5.InnerHtml = "Order New Camera";

         HtmlGenericControl btnTopClose = new HtmlGenericControl("button");
         btnTopClose.Attributes.Add("type", "button");
         btnTopClose.Attributes.Add("class", "close");
         btnTopClose.Attributes.Add("data-dismiss", "modal");
         btnTopClose.Attributes.Add("aria-label", "Close");

         HtmlGenericControl btnCloseSpan = new HtmlGenericControl("span");
         btnCloseSpan.Attributes.Add("aria-hidden", "true");
         btnCloseSpan.InnerHtml = "&times;";

         btnTopClose.Controls.Add(btnCloseSpan);
         divHeader.Controls.Add(h5);
         divHeader.Controls.Add(btnTopClose);

         //body controls
         HtmlGenericControl divBody = new HtmlGenericControl("div");
         divBody.Attributes.Add("class", "modal-body");
         divBody.Attributes.Add("style", "display: flex; flex-direction: column;");

         //name
         Label nameLabel = new Label();
         nameLabel.ID = "addNameLabel";
         nameLabel.Text = "New Camera Name: ";
         TextBox addNameTextBox = new TextBox();
         addNameTextBox.ID = "addNameTextBox";
         addNameTextBox.Attributes.Add("style", "float: right;");

         //address
         Label addressLabel = new Label();
         addressLabel.ID = "addAddressLabel";
         addressLabel.Text = "Ship to Address: ";
         TextBox addAddressTextBox = new TextBox();
         addAddressTextBox.ID = "addAddressTextBox";
         addAddressTextBox.Attributes.Add("style", "float: right;");

         //Information Label
         Label infoLabel = new Label();
         infoLabel.ID = "addInfoLabel";
         infoLabel.Text = "New cameras will arrive in 3-5 business days. Follow the instructions included to physically deploy it at your parking lot.";
         infoLabel.Attributes.Add("style", "text-align: center; ");

         //Warning Label
         Label addWarning = new Label();
         addWarning.ID = "addWarningLabel";
         addWarning.Text = "After physically deploying your new camera, come back to this menu to finish deployment process.";
         addWarning.Attributes.Add("style", "text-align: center; color: red");

         divBody.Controls.Add(nameLabel);
         divBody.Controls.Add(addNameTextBox);
         divBody.Controls.Add(new LiteralControl("<br />"));
         divBody.Controls.Add(new LiteralControl("<br />"));

         divBody.Controls.Add(addressLabel);
         divBody.Controls.Add(addAddressTextBox);
         divBody.Controls.Add(new LiteralControl("<br />"));
         divBody.Controls.Add(new LiteralControl("<br />"));

         divBody.Controls.Add(infoLabel);
         divBody.Controls.Add(addWarning);
         divBody.Controls.Add(new LiteralControl("<br />"));
         divBody.Controls.Add(new LiteralControl("<br />"));

         //footer controls
         HtmlGenericControl divFooter = new HtmlGenericControl("div");
         divFooter.Attributes.Add("class", "modal-footer");

         HtmlGenericControl btnCloseFooter = new HtmlGenericControl("button");
         btnCloseFooter.Attributes.Add("type", "button");
         btnCloseFooter.Attributes.Add("class", "btn btn-secondary");
         btnCloseFooter.Attributes.Add("data-dismiss", "modal");
         btnCloseFooter.InnerHtml = "Cancel";

         Button btnAddSubmit = new Button();
         btnAddSubmit.ID = "btnAddSubmit";
         btnAddSubmit.CssClass = "btn btn-primary";
         btnAddSubmit.Text = "Add";
         btnAddSubmit.Click += new EventHandler(btnAddSubmit_Click);

         divFooter.Controls.Add(btnAddSubmit);
         divFooter.Controls.Add(btnCloseFooter);

         div3.Controls.Add(divHeader);
         div3.Controls.Add(divBody);
         div3.Controls.Add(divFooter);

         div2.Controls.Add(div3);
         div1.Controls.Add(div2);

         cameraContainer.Controls.Add(div1);
      }

      private void LoadCurrentDeviceListDivs()
      {
         HtmlGenericControl propertyCameraDiv = new HtmlGenericControl("div");
         propertyCameraDiv.Attributes.Add("style", "float: left; width: 40%; ");

         foreach (Device device in currentDeviceList)
         {
            //outer div
            HtmlGenericControl outerDiv = new HtmlGenericControl("div");
            outerDiv.Attributes.Add("class", "card shadow mb");
            outerDiv.Attributes.Add("data-parent", "cameraContainer");

            HyperLink link = new HyperLink();
            link.NavigateUrl = "#collapseCamera" + device.DeviceID;
            link.Attributes.Add("class", "d-block card-header py-3 collapsed");
            link.Attributes.Add("data-toggle", "collapse");
            link.Attributes.Add("role", "button");

            HtmlGenericControl h6 = new HtmlGenericControl("h6");
            /*HtmlGenericControl mobileIcon = new HtmlGenericControl("i");
            if (parkingLot.TotalSpots > 0)
            {
               mobileIcon.Attributes.Add("class", "fas fa-fw fa-mobile-alt");
               mobileIcon.Attributes.Add("style", "color: green; float: right; width:45%;");
               mobileIcon.Attributes.Add("Title", "Parking Lot is visible to SpotCheck users.");
            }
            else
            {
               mobileIcon.Attributes.Add("class", "fas fa-fw fa-mobile-alt");
               mobileIcon.Attributes.Add("style", "color: red; float: right; width:45%;");
               mobileIcon.Attributes.Add("Title", "Parking Lot is not visible to SpotCheck users.");
            }*/

            h6.InnerHtml = device.DeviceName;
            h6.Attributes.Add("class", "m-0 font-weight-bold text-primary");
            h6.Attributes.Add("style", "float:left; width:45%;");
            link.Controls.Add(h6);
            //link.Controls.Add(mobileIcon);

            HtmlGenericControl middleDiv = new HtmlGenericControl("div");
            middleDiv.Attributes.Add("class", "collapse hide");
            middleDiv.ID = "collapseCamera" + device.DeviceID;

            HtmlGenericControl innerDiv = new HtmlGenericControl("div");
            innerDiv.Attributes.Add("class", "card-body");

            HtmlGenericControl extAddressDiv = new HtmlGenericControl("div");
            extAddressDiv.InnerHtml = "<strong>External IP Address: </strong>" + device.ExternalIpAddress;

            HtmlGenericControl localAddressDiv = new HtmlGenericControl("div");
            localAddressDiv.InnerHtml = "<strong>Local IP Address: </strong>" + device.LocalIpAddress;

            HtmlGenericControl macAddressDiv = new HtmlGenericControl("div");
            macAddressDiv.InnerHtml = "<strong>Mac Address: </strong>" + device.MacAddress;

            //HtmlGenericControl visitsDiv = new HtmlGenericControl("div");
            //visitsDiv.InnerHtml = "<strong>Visits this month: </strong>0";

            /* HtmlGenericControl camerasDiv = new HtmlGenericControl("div");
             if (cameraHyperlink != null)
             {
                camerasDiv.InnerHtml = "<strong>Cameras Deployed: </strong>";
                camerasDiv.Controls.Add(cameraHyperlink);
             }
             else
             {
                camerasDiv.InnerHtml = "<strong>Cameras Deployed: </strong>0";
             }*/

            HtmlGenericControl editButton = new HtmlGenericControl("button");
            editButton.Attributes.Add("data-toggle", "modal");
            editButton.Attributes.Add("data-target", "#editModal" + device.DeviceID);
            editButton.Attributes.Add("type", "button");
            editButton.Attributes.Add("class", "btn btn-primary");
            editButton.Attributes.Add("data-backdrop", "false");
            editButton.Attributes.Add("style", "float: left; width: 49%;");
            editButton.InnerHtml = "Edit Camera";

            HtmlGenericControl deleteButton = new HtmlGenericControl("button");
            deleteButton.Attributes.Add("data-toggle", "modal");
            deleteButton.Attributes.Add("data-target", "#deleteModal" + device.DeviceID);
            deleteButton.Attributes.Add("type", "button");
            deleteButton.Attributes.Add("class", "btn btn-danger");
            deleteButton.Attributes.Add("data-backdrop", "false");
            deleteButton.Attributes.Add("style", "float: right; width: 49%;");
            deleteButton.InnerHtml = "Delete Camera";

            innerDiv.Controls.Add(extAddressDiv);
            innerDiv.Controls.Add(localAddressDiv);
            innerDiv.Controls.Add(macAddressDiv);
            innerDiv.Controls.Add(new LiteralControl("<br />"));
            innerDiv.Controls.Add(editButton);
            innerDiv.Controls.Add(deleteButton);
            innerDiv.Controls.Add(new LiteralControl("<br />"));

            middleDiv.Controls.Add(innerDiv);

            outerDiv.Controls.Add(link);
            outerDiv.Controls.Add(middleDiv);

            propertyCameraDiv.Controls.Add(outerDiv);
         }

         cameraContainer.Controls.Add(propertyCameraDiv);

      }
      #endregion Methods
   }
}