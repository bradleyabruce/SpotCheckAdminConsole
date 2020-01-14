using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SpotCheckAdminPortal.Models;

namespace SpotCheckAdminPortal
{

    public partial class ViewParkingLots : System.Web.UI.Page
    {
        #region Properties

        private Company company = new Company();
        private List<Device> devices = new List<Device>();
        private List<ParkingLot> parkingLots = new List<ParkingLot>();

        #endregion End Properties

        #region Events


        protected void Page_Load(object sender, EventArgs e)
        {
            //get information
            company = IoC.CurrentCompany;

            Device d = new Device();
            devices = d.GetDeviceListFromCompanyID((int)company.CompanyID);

            ParkingLot pl = new ParkingLot();
            parkingLots = pl.GetParkingLotListFromCompanyID((int)company.CompanyID);

            //Validate info
            bool validate = IoC.ValidateInfo();
            if (validate)
            {
                Response.Redirect("index.aspx");
            }

            //set label
            CompanyNameLiteral.Text = company.CompanyName;

            LoadPage();

        }

        private void btnEditSubmit_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            string lotID = button.CommandArgument.Substring(7);

            ParkingLot editLot = new ParkingLot();
            editLot.LotID = int.Parse(lotID);
            editLot = editLot.Fill();

            string[] controlIDsToFind = { "editNameTextBox" + lotID, "editAddressTextBox" + lotID, "editCityTextBox" + lotID, "editStateTextBox" + lotID, "editZipCodeTextBox" + lotID };

            Control match = null;

            foreach (string controlID in controlIDsToFind)
            {
                match = FindControlRecursive(parkingLotContainer, controlID);
                if (match != null)
                {
                    TextBox tb = match as TextBox;

                    switch (controlID)
                    {
                        case string a when controlID.Contains("Name"):
                            if (tb.Text != "")
                            {
                                editLot.LotName = tb.Text;
                                tb.Text = "";
                            }
                            break;
                        case string a when controlID.Contains("Address"):
                            if (tb.Text != "")
                            {
                                editLot.Address = tb.Text;
                                tb.Text = "";
                            }
                            break;
                        case string a when controlID.Contains("City"):
                            if (tb.Text != "")
                            {
                                editLot.City = tb.Text;
                                tb.Text = "";
                            }
                            break;
                        case string a when controlID.Contains("State"):
                            if (tb.Text != "")
                            {
                                editLot.State = tb.Text;
                                tb.Text = "";
                            }
                            break;
                        case string a when controlID.Contains("ZipCode"):
                            if (tb.Text != "")
                            {
                                editLot.ZipCode = tb.Text;
                                tb.Text = "";
                            }
                            break;
                    }
                }
            }

            //update Lot
            editLot = editLot.UpdateParkingLot();

            if(editLot != null)
            {
                //reload parkingLotList
                parkingLots = editLot.GetParkingLotListFromCompanyID((int)IoC.CurrentCompany.CompanyID);
                LoadPage();
                ShowMessage("success");
            }
            else
            {
                ShowMessage("danger");
                //ShowErrorMessage();
            }            
        }

        #endregion

        #region Methods

        private void LoadPage()
        {
            parkingLotContainer.Controls.Clear();
            CreateParkingLotDropDowns();
            CreateEditModals();
        }

        private void ShowMessage(string type)
        {
            HtmlGenericControl outerDiv = new HtmlGenericControl("div");
            if(type == "success")
            {
                outerDiv.Attributes.Add("class", "alert alert-success alert-dismissible fade show");
                outerDiv.InnerHtml = "Parking lot successfully updated!";
            }
            if(type == "warning")
            {
                outerDiv.Attributes.Add("class", "alert alert-warning alert-dismissible fade show");
                outerDiv.InnerHtml = "Warning!";
            }
            if(type == "danger")
            {
                outerDiv.Attributes.Add("class", "alert alert-danger alert-dismissible fade show");
                outerDiv.InnerHtml = "Error Occurred. Parking lot was not updated!";
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


        private void CreateParkingLotDropDowns()
        {
            foreach (ParkingLot parkingLot in parkingLots)
            {
                List<Device> deployedCameras = parkingLot.GetCamerasDeployed();
                int cameraCount = 0;
                if(deployedCameras != null)
                {
                    cameraCount = deployedCameras.Count;
                }

                //outer div
                HtmlGenericControl outerDiv = new HtmlGenericControl("div");
                outerDiv.Attributes.Add("class", "card shadow mb");
                outerDiv.Attributes.Add("style", "width: 30%");
                outerDiv.Attributes.Add("data-parent", "parkingLotContainer");

                HyperLink link = new HyperLink();
                link.NavigateUrl = "#collapseParkingLot" + parkingLot.LotID;
                link.Attributes.Add("class", "d-block card-header py-3");
                link.Attributes.Add("data-toggle", "collapse");
                link.Attributes.Add("role", "button");

                HtmlGenericControl h6 = new HtmlGenericControl("h6");
                h6.InnerHtml = parkingLot.LotName;
                h6.Attributes.Add("class", "m-0 font-weight-bold text-primary");
                link.Controls.Add(h6);

                HtmlGenericControl middleDiv = new HtmlGenericControl("div");
                middleDiv.Attributes.Add("class", "collapse hide");
                middleDiv.ID = "collapseParkingLot" + parkingLot.LotID;

                HtmlGenericControl innerDiv = new HtmlGenericControl("div");
                innerDiv.Attributes.Add("class", "card-body");
                string innerHtml = "<p><strong>" + parkingLot.Address + ", " + parkingLot.City + ", " + parkingLot.State + " " + parkingLot.ZipCode + "</strong></p> ";
                innerHtml += "<p>Total Spots: " + parkingLot.TotalSpots + "</p>";
                innerHtml += "<p>Open Spots: " + parkingLot.OpenSpots + "</p>";
                innerHtml += "<p> Cameras Deployed: " + cameraCount + "</p>";
                innerDiv.InnerHtml = innerHtml;

                HtmlGenericControl editButton = new HtmlGenericControl("button");
                editButton.Attributes.Add("data-toggle", "modal");
                editButton.Attributes.Add("data-target", "#editModal" + parkingLot.LotID);
                editButton.Attributes.Add("type", "button");
                editButton.Attributes.Add("class", "btn btn-primary");
                editButton.InnerHtml = "Edit Parking Lot";
                innerDiv.Controls.Add(editButton);

                middleDiv.Controls.Add(innerDiv);

                outerDiv.Controls.Add(link);
                outerDiv.Controls.Add(middleDiv);

                parkingLotContainer.Controls.Add(outerDiv);
            }
        }

        public void CreateEditModals()
        {
            foreach (ParkingLot parkingLot in parkingLots)
            {
                HtmlGenericControl div1 = new HtmlGenericControl("div");
                div1.Attributes.Add("class", "modal");
                div1.Attributes.Add("tabindex", "-1");
                div1.Attributes.Add("role", "dialog");
                div1.Attributes.Add("id", "editModal" + parkingLot.LotID);

                HtmlGenericControl div2 = new HtmlGenericControl("div");
                div2.Attributes.Add("class", "modal-dialog");
                div2.Attributes.Add("role", "document");

                HtmlGenericControl div3 = new HtmlGenericControl("div");
                div3.Attributes.Add("class", "modal-content");

                //header controls
                HtmlGenericControl divHeader = new HtmlGenericControl("div");
                divHeader.Attributes.Add("class", "modal-header");

                HtmlGenericControl h5 = new HtmlGenericControl("h5");
                h5.Attributes.Add("class", "modal-title");
                h5.InnerHtml = "Edit Parking Lot";

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

                //name
                Label nameLabel = new Label();
                nameLabel.ID = "editNameLabel" + parkingLot.LotID;
                nameLabel.Text = "Name: ";
                TextBox editNameTextBox = new TextBox();
                editNameTextBox.ID = "editNameTextBox" + parkingLot.LotID;
                editNameTextBox.Attributes.Add("placeholder", parkingLot.LotName);

                //address
                Label addressLabel = new Label();
                addressLabel.ID = "editAddressLabel" + parkingLot.LotID;
                addressLabel.Text = "Address: ";
                TextBox editAddressTextBox = new TextBox();
                editAddressTextBox.ID = "editAddressTextBox" + parkingLot.LotID;
                editAddressTextBox.Attributes.Add("placeholder", parkingLot.Address);

                //city
                Label cityLabel = new Label();
                cityLabel.ID = "editCityLabel" + parkingLot.LotID;
                cityLabel.Text = "City: ";
                TextBox editCityTextBox = new TextBox();
                editCityTextBox.ID = "editCityTextBox" + parkingLot.LotID;
                editCityTextBox.Attributes.Add("placeholder", parkingLot.City);

                //state
                Label stateLabel = new Label();
                stateLabel.ID = "editStateLabel" + parkingLot.LotID;
                stateLabel.Text = "State: ";
                TextBox editStateTextBox = new TextBox();
                editStateTextBox.ID = "editStateTextBox" + parkingLot.LotID;
                editStateTextBox.Attributes.Add("placeholder", parkingLot.State);

                //zip
                Label zipCodeLabel = new Label();
                zipCodeLabel.ID = "editZipCodeLabel" + parkingLot.LotID;
                zipCodeLabel.Text = "Zip Code: ";
                TextBox editZipCodeTextBox = new TextBox();
                editZipCodeTextBox.ID = "editZipCodeTextBox" + parkingLot.LotID;
                editZipCodeTextBox.Attributes.Add("placeholder", parkingLot.ZipCode);

                divBody.Controls.Add(nameLabel);
                divBody.Controls.Add(editNameTextBox);
                divBody.Controls.Add(new LiteralControl("<br />"));
                divBody.Controls.Add(new LiteralControl("<br />"));

                divBody.Controls.Add(addressLabel);
                divBody.Controls.Add(editAddressTextBox);
                divBody.Controls.Add(new LiteralControl("<br />"));
                divBody.Controls.Add(new LiteralControl("<br />"));

                divBody.Controls.Add(cityLabel);
                divBody.Controls.Add(editCityTextBox);
                divBody.Controls.Add(new LiteralControl("<br />"));
                divBody.Controls.Add(new LiteralControl("<br />"));

                divBody.Controls.Add(stateLabel);
                divBody.Controls.Add(editStateTextBox);
                divBody.Controls.Add(new LiteralControl("<br />"));
                divBody.Controls.Add(new LiteralControl("<br />"));

                divBody.Controls.Add(zipCodeLabel);
                divBody.Controls.Add(editZipCodeTextBox);
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

                Button btnEditSubmit = new Button();
                btnEditSubmit.ID = "btnSubmit" + parkingLot.LotID;
                btnEditSubmit.CssClass = "btn btn-primary";
                btnEditSubmit.Text = "Save";
                btnEditSubmit.Click += new EventHandler(btnEditSubmit_Click);
                btnEditSubmit.CommandArgument = Convert.ToString("btnEdit" + parkingLot.LotID);

                divFooter.Controls.Add(btnEditSubmit);
                divFooter.Controls.Add(btnCloseFooter);

                div3.Controls.Add(divHeader);
                div3.Controls.Add(divBody);
                div3.Controls.Add(divFooter);

                div2.Controls.Add(div3);
                div1.Controls.Add(div2);

                parkingLotContainer.Controls.Add(div1);              
            }
        }

        private Control FindControlRecursive(Control rootControl, string controlID)
        {
            if (rootControl.ID == controlID) return rootControl;

            foreach (Control controlToSearch in rootControl.Controls)
            {
                Control controlToReturn = FindControlRecursive(controlToSearch, controlID);
                if (controlToReturn != null) return controlToReturn;
            }
            return null;
        }    

        #endregion
    }
}
