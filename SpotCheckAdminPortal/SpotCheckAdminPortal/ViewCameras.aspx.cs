using SpotCheckAdminPortal.Enums;
using SpotCheckAdminPortal.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
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

        private static ParkingLot _selectedParkingLot;
        private static ParkingLot selectedParkingLot
        {
            get => _selectedParkingLot;
            set
            {
                _selectedParkingLot = value;
            }
        }

        private static eDeviceStatus.DeviceStatus[] deployedStatues = { eDeviceStatus.DeviceStatus.Deployed, eDeviceStatus.DeviceStatus.ReadyForSpots };
        private static eDeviceStatus.DeviceStatus[] undeployedStatues = { eDeviceStatus.DeviceStatus.Undeployed };

        private List<Device> globalDeviceList;

        #endregion End Properties

        #region Events

        protected void Page_Init(object sender, EventArgs e)
        {
            ParkingLot pl = new ParkingLot();
            Device d = new Device();

            //get information
            company = IoC.CurrentCompany;
            parkingLots = pl.GetParkingLotListFromCompanyID(company.CompanyID);
            globalDeviceList = d.GetDeviceListFromCompanyID(company.CompanyID);

            if (IoC.ValidateInfo())
            {
                Response.Redirect("Dashboard.aspx");
            }

            populateDeployDropDownList();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            CreateDeviceListDivs(undeployedStatues);
            CreateDeploySubmitButtonWithServerAndClientControls();

            //on first load only
            //check for params in the url
            if (!IsPostBack)
            {
                //set label
                CompanyNameLiteral.Text = company.CompanyName;

                if (parkingLots.Count > 0)
                {
                    string paramParkingLot = Request.QueryString["ParkingLotID"];
                    LoadParkingLotComboBox(paramParkingLot);  //param parkinglot will either be null or be a valid parkinglotID               
                }
                else
                {
                    ShowMessage("warning", "You do not have any parking lots set up yet.");
                }
            }
            //on refresh
            else
            {
                if (selectedParkingLot != null)
                {
                    CreateDeviceListDivs(deployedStatues);
                    deployedCameraUpdatePanel.Update();    //update only deployed parking lots
                }
            }
        }

        private void btnOrderNewSubmit_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            string deviceID = button.CommandArgument;

            Device newDevice = new Device();
            string[] controlIDsToFind = { "addNameTextBox" };

            Control match = null;

            foreach (string controlID in controlIDsToFind)
            {
                match = IoC.FindControlRecursive(modalDiv, controlID);
                if (match != null)
                {
                    TextBox tb = match as TextBox;

                    switch (controlID)
                    {
                        case string a when controlID.Contains("Name"):
                            if (tb.Text != "")
                            {
                                newDevice.DeviceName = tb.Text;
                                tb.Text = "";
                            }
                            break;
                    }
                }
            }

            newDevice.CompanyID = company.CompanyID;
            newDevice = newDevice.Create();

            if (newDevice != null)
            {
                //reload undeployed camera list
                globalDeviceList = newDevice.GetDeviceListFromCompanyID(company.CompanyID);

                CreateDeviceListDivs(undeployedStatues);
                undeployedCameraUpdatePanel.Update();     //force update to only undeployed cameras (this also updates modals)
                ShowMessage("success", "Device successfully ordered.");
            }
            else
            {
                ShowMessage("danger", "Error Occurred. Device could not be ordered.");
            }
        }

        private void deployButton_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            int deviceID = Int32.Parse(button.CommandArgument);

            //Send Request to Camera to take image
            if (SendCameraImageRequest(deviceID))
            {
                //Attempt to retreive image
                Device currentDevice = new Device()
                {
                    DeviceID = deviceID
                };
                currentDevice.Fill();
                string encodedImageString = currentDevice.GetEncodedImageString();
                currentDevice.ClearImage();

                if(encodedImageString != null)
                {
                    hiddenImageStringField.Text = encodedImageString;
                    hiddenCameraIDField.Text = currentDevice.DeviceID.ToString();
                    hiddenSpotCoordJsonField.Text = "";
                    hiddenInfoPanel.Update();
                    //from here, javascript should take over and do the rest for us
                }
                else
                {
                    currentDevice.ClearImage();
                    ShowMessage("danger", "Could not retrieve image from camera.");
                    deployModalUpdatePanel.Update();    //this will cause the modal to close on the user.
                }
            }
            else
            {
                Device device = new Device();
                device.DeviceID = deviceID;
                device.ClearImage();
                ShowMessage("danger", "Could not send image request from camera.");
                deployModalUpdatePanel.Update();    //this will cause the modal to close on the user.
            }
        }


        private void btnEditSubmit_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            string deviceID = button.CommandArgument;

            Device editDevice = new Device();
            editDevice.DeviceID = Int32.Parse(deviceID);
            editDevice = editDevice.Fill();

            if (editDevice != null)
            {
                string[] controlIDsToFind = { "editNameTextBox" + deviceID };
                Control match = null;

                foreach (string controlID in controlIDsToFind)
                {
                    match = IoC.FindControlRecursive(modalDiv, controlID);

                    if (match != null)
                    {
                        TextBox tb = match as TextBox;

                        switch (controlID)
                        {
                            case string a when controlID.Contains("Name"):
                                if (tb.Text != "")
                                {
                                    editDevice.DeviceName = tb.Text;
                                    tb.Text = "";
                                }
                                break;
                        }
                    }
                }

                editDevice = editDevice.Update();
                if (editDevice != null)
                {
                    //reload undeployed camera list
                    globalDeviceList = editDevice.GetDeviceListFromCompanyID(company.CompanyID);

                    if (editDevice.IsDeployed())     //refresh the correct list
                    {
                        CreateDeviceListDivs(deployedStatues);
                        deployedCameraUpdatePanel.Update();     //force update to only deployed cameras (this also updates modals)
                    }
                    else
                    {
                        CreateDeviceListDivs(undeployedStatues);
                        undeployedCameraUpdatePanel.Update();     //force update to only undeployed cameras (this also updates modals)
                    }

                    ShowMessage("success", "Device successfully updated.");
                }
                else     //update failed
                {
                    ShowMessage("danger", "Error Occurred. Device could not be updated.");
                }
            }
            else     //fill failed
            {
                ShowMessage("danger", "Error Occurred. Device could not be found.");
            }
        }


        private void deploySubmitButton_Click(object sender, EventArgs e)
        {
            ShowMessage("danger", "wow");
        }

        private void btnUndeploySubmit_Click(object sender, EventArgs e)
        {
            Button undeployButton = sender as Button;
            string deviceID = undeployButton.CommandArgument;

            Device undeployDevice = new Device();
            undeployDevice.DeviceID = Int32.Parse(deviceID);
            undeployDevice = undeployDevice.Fill();

            if (undeployDevice != null)
            {
                bool undeployResult = undeployDevice.Undeploy();
                if (undeployResult)
                {
                    //reload both camera lists
                    globalDeviceList = undeployDevice.GetDeviceListFromCompanyID(company.CompanyID);
                    CreateDeviceListDivs(undeployedStatues);
                    CreateDeviceListDivs(deployedStatues);
                    //force update (this also updates modals)
                    undeployedCameraUpdatePanel.Update();
                    deployModalUpdatePanel.Update();

                    ShowMessage("success", "Device successfully undeployed.");
                }
                else     //updeploy failed
                {
                    ShowMessage("danger", "Error Occurred. Device could not be undeployed.");
                }
            }
            else     //fill failed
            {
                ShowMessage("danger", "Error Occurred. Device could not be found.");
            }
        }

        private void btnDeleteSubmit_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            string deviceID = button.CommandArgument;

            Device deleteDevice = new Device();
            deleteDevice.DeviceID = Int32.Parse(deviceID);
            deleteDevice = deleteDevice.Fill();

            if (deleteDevice != null)
            {
                bool? deleteResult = deleteDevice.Delete();
                if (deleteResult == true)
                {
                    //reload undeployed camera list
                    globalDeviceList = deleteDevice.GetDeviceListFromCompanyID(company.CompanyID);
                    CreateDeviceListDivs(undeployedStatues);
                    undeployedCameraUpdatePanel.Update();     //force update to only undeployed cameras (this also updates modals)                    

                    ShowMessage("success", "Device successfully deleted.");
                }
                else     //update failed
                {
                    ShowMessage("danger", "Error Occurred. Device could not be deleted.");
                }
            }
            else     //fill failed
            {
                ShowMessage("danger", "Error Occurred. Device could not be found.");
            }
        }

        protected void parkingLotDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if selected is the default value, do nothing
            DropDownList dropDownList = sender as DropDownList;
            if (dropDownList.SelectedValue != "0")
            {
                ParkingLot parkingLot = new ParkingLot()
                {
                    LotID = int.Parse(dropDownList.SelectedValue)
                };

                parkingLot = parkingLot.Fill();
                selectedParkingLot = parkingLot;

                CreateDeviceListDivs(deployedStatues);
                deployedCameraUpdatePanel.Update();    //update only deployed parking lots
            }
        }

        #endregion Events

        #region Methods

        /*private Image ConvertStringToImage(string encodedImage)
        {
            ImageConverter imageConverter = new ImageConverter();
            byte[] array = Convert.FromBase64String(encodedImage);
            
        }*/


        private bool SendCameraImageRequest(int deviceID)
        {
            Device device = new Device
            {
                DeviceID = deviceID
            };
            device = device.Fill();
            device.TakeNewImage = true;
            Device updatedDevice = device.Update();

            if (!(updatedDevice is null))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void LoadParkingLotComboBox(string selectedParkingLotID = null)
        {
            bool forcePageUpdate = false;

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
                List<Device> deployedCameras = getDeployedCamerasFromParkingLotID((int)parkingLot.LotID);

                ListItem parkingLotItem = new ListItem();
                parkingLotItem.Value = ((int)parkingLot.LotID).ToString();
                parkingLotItem.Text = parkingLot.LotName + " - " + deployedCameras.Count().ToString() + " Camera(s)";

                if (deployedCameras.Count < 1)
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

            if (forcePageUpdate)
            {
                //trigger page update
                parkingLotDropDownList_SelectedIndexChanged(parkingLotDropDownList, EventArgs.Empty);
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
            alertUpdatePanel.Update();
        }

        private List<Device> getDeployedCamerasFromParkingLotID(int parkingLotID)
        {
            return globalDeviceList.FindAll(f => f.ParkingLotID == parkingLotID && (f.DeviceStatusID == (int)eDeviceStatus.DeviceStatus.Deployed || f.DeviceStatusID == (int)eDeviceStatus.DeviceStatus.ReadyForSpots));
        }

        private List<Device> getUndeployedCameras()
        {
            return globalDeviceList.FindAll(f => f.DeviceStatusID == (int)eDeviceStatus.DeviceStatus.Undeployed || f.DeviceStatusID == (int)eDeviceStatus.DeviceStatus.WaitingForImage);
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

            Button btnOrderNewSubmit = new Button();
            btnOrderNewSubmit.ID = "btnAddSubmit";
            btnOrderNewSubmit.CssClass = "btn btn-primary";
            btnOrderNewSubmit.Text = "Order";
            btnOrderNewSubmit.Click += new EventHandler(btnOrderNewSubmit_Click);

            divFooter.Controls.Add(btnOrderNewSubmit);
            divFooter.Controls.Add(btnCloseFooter);

            div3.Controls.Add(divHeader);
            div3.Controls.Add(divBody);
            div3.Controls.Add(divFooter);

            div2.Controls.Add(div3);
            div1.Controls.Add(div2);

            modalDiv.Controls.Add(div1);
        }

        private void CreateDeployedEditModals()
        {
            if (globalDeviceList != null && selectedParkingLot != null)
            {
                List<Device> deployedDevices = getDeployedCamerasFromParkingLotID((int)selectedParkingLot.LotID);

                foreach (Device device in deployedDevices)
                {
                    HtmlGenericControl div1 = new HtmlGenericControl("div");
                    div1.Attributes.Add("class", "modal");
                    div1.Attributes.Add("tabindex", "-1");
                    div1.Attributes.Add("role", "dialog");
                    div1.Attributes.Add("id", "editModal" + device.DeviceID);
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
                    h5.InnerHtml = "Edit Camera";

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
                    nameLabel.ID = "editNameLabel" + device.DeviceID;
                    nameLabel.Text = "Name: ";
                    TextBox editNameTextBox = new TextBox();
                    editNameTextBox.ID = "editNameTextBox" + device.DeviceID;
                    editNameTextBox.Attributes.Add("placeholder", device.DeviceName);
                    editNameTextBox.Attributes.Add("style", "float: right;");

                    divBody.Controls.Add(nameLabel);
                    divBody.Controls.Add(editNameTextBox);
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
                    btnEditSubmit.ID = "btnEditSubmit" + device.DeviceID;
                    btnEditSubmit.CssClass = "btn btn-primary";
                    btnEditSubmit.Text = "Save";
                    btnEditSubmit.Click += new EventHandler(btnEditSubmit_Click);
                    btnEditSubmit.CommandArgument = Convert.ToString(device.DeviceID);

                    divFooter.Controls.Add(btnEditSubmit);
                    divFooter.Controls.Add(btnCloseFooter);

                    div3.Controls.Add(divHeader);
                    div3.Controls.Add(divBody);
                    div3.Controls.Add(divFooter);

                    div2.Controls.Add(div3);
                    div1.Controls.Add(div2);

                    modalDiv.Controls.Add(div1);
                }
            }
        }

        private void CreateDeployedUndeployModals()
        {
            if (globalDeviceList != null && selectedParkingLot != null)
            {
                List<Device> deployedDevices = getDeployedCamerasFromParkingLotID((int)selectedParkingLot.LotID);

                foreach (Device device in deployedDevices)
                {
                    HtmlGenericControl div1 = new HtmlGenericControl("div");
                    div1.Attributes.Add("class", "modal");
                    div1.Attributes.Add("tabindex", "-1");
                    div1.Attributes.Add("role", "dialog");
                    div1.Attributes.Add("id", "undeployModal" + device.DeviceID);
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
                    h5.InnerHtml = "Undeploy Camera";

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
                    nameLabel.ID = "undeployNameLabel" + device.DeviceID;
                    nameLabel.Text = device.DeviceName;
                    nameLabel.CssClass = "text-align: center;";

                    //warning
                    Label warningLabel = new Label();
                    nameLabel.ID = "warningUndeployLabel" + device.DeviceID;
                    nameLabel.Text = "Undeploying this camera from this parking lot will allow you to deploy it to a new parking lot or return it to SpotCheck.";
                    nameLabel.CssClass = "text-align: center;";

                    divBody.Controls.Add(nameLabel);
                    divBody.Controls.Add(new LiteralControl("<br />"));
                    divBody.Controls.Add(new LiteralControl("<br />"));
                    divBody.Controls.Add(warningLabel);

                    //footer controls
                    HtmlGenericControl divFooter = new HtmlGenericControl("div");
                    divFooter.Attributes.Add("class", "modal-footer");

                    HtmlGenericControl btnCloseFooter = new HtmlGenericControl("button");
                    btnCloseFooter.Attributes.Add("type", "button");
                    btnCloseFooter.Attributes.Add("class", "btn btn-secondary");
                    btnCloseFooter.Attributes.Add("data-dismiss", "modal");
                    btnCloseFooter.InnerHtml = "Cancel";

                    Button btnUndeploySubmit = new Button();
                    btnUndeploySubmit.ID = "btnUndeploySubmit" + device.DeviceID;
                    btnUndeploySubmit.CssClass = "btn btn-danger";
                    btnUndeploySubmit.Text = "Undeploy";
                    btnUndeploySubmit.Click += new EventHandler(btnUndeploySubmit_Click);
                    btnUndeploySubmit.CommandArgument = Convert.ToString(device.DeviceID);

                    divFooter.Controls.Add(btnUndeploySubmit);
                    divFooter.Controls.Add(btnCloseFooter);

                    div3.Controls.Add(divHeader);
                    div3.Controls.Add(divBody);
                    div3.Controls.Add(divFooter);

                    div2.Controls.Add(div3);
                    div1.Controls.Add(div2);

                    modalDiv.Controls.Add(div1);
                }
            }
        }

        private void CreateUnDeployedEditModals()
        {
            if (globalDeviceList != null)
            {
                List<Device> undeployedDevices = getUndeployedCameras();

                foreach (Device device in undeployedDevices)
                {
                    HtmlGenericControl div1 = new HtmlGenericControl("div");
                    div1.Attributes.Add("class", "modal");
                    div1.Attributes.Add("tabindex", "-1");
                    div1.Attributes.Add("role", "dialog");
                    div1.Attributes.Add("id", "editModal" + device.DeviceID);
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
                    h5.InnerHtml = "Edit Camera";

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
                    nameLabel.ID = "editNameLabel" + device.DeviceID;
                    nameLabel.Text = "Name: ";
                    TextBox editNameTextBox = new TextBox();
                    editNameTextBox.ID = "editNameTextBox" + device.DeviceID;
                    editNameTextBox.Attributes.Add("placeholder", device.DeviceName);
                    editNameTextBox.Attributes.Add("style", "float: right;");

                    divBody.Controls.Add(nameLabel);
                    divBody.Controls.Add(editNameTextBox);
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
                    btnEditSubmit.ID = "btnEditSubmit" + device.DeviceID;
                    btnEditSubmit.CssClass = "btn btn-primary";
                    btnEditSubmit.Text = "Save";
                    btnEditSubmit.Click += new EventHandler(btnEditSubmit_Click);
                    btnEditSubmit.CommandArgument = Convert.ToString(device.DeviceID);

                    divFooter.Controls.Add(btnEditSubmit);
                    divFooter.Controls.Add(btnCloseFooter);

                    div3.Controls.Add(divHeader);
                    div3.Controls.Add(divBody);
                    div3.Controls.Add(divFooter);

                    div2.Controls.Add(div3);
                    div1.Controls.Add(div2);

                    modalDiv.Controls.Add(div1);
                }
            }
        }

        private void CreateDeploySubmitButtonWithServerAndClientControls()
        {
            Button deploySubmitButton = new Button();
            deploySubmitButton.ID = "deploySubmit";
            deploySubmitButton.Attributes.Add("type", "button");
            deploySubmitButton.Attributes.Add("class", "btn btn-primary");
            deploySubmitButton.Click += new EventHandler(deploySubmitButton_Click);
            deploySubmitButton.Text = "Deploy";

            HtmlGenericControl cancel = new HtmlGenericControl("button");
            cancel.ID = "deployCloseFooter";
            cancel.Attributes.Add("type", "button");
            cancel.Attributes.Add("class", "btn btn-secondary");
            cancel.Attributes.Add("data-dismiss", "modal");
            cancel.InnerText = "Cancel";

            divFooterDeploy.Controls.Add(deploySubmitButton);
            divFooterDeploy.Controls.Add(cancel);
        }

        private void CreateUndeployedDeleteModals()
        {
            if (globalDeviceList != null)
            {
                List<Device> undeployedDevices = getUndeployedCameras();

                foreach (Device device in undeployedDevices)
                {
                    HtmlGenericControl div1 = new HtmlGenericControl("div");
                    div1.Attributes.Add("class", "modal");
                    div1.Attributes.Add("tabindex", "-1");
                    div1.Attributes.Add("role", "dialog");
                    div1.Attributes.Add("id", "deleteModal" + device.DeviceID);
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
                    h5.InnerHtml = "Delete Camera";

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
                    nameLabel.ID = "deleteNameLabel" + device.DeviceID;
                    nameLabel.Text = device.DeviceName;
                    nameLabel.CssClass = "text-align: center;";

                    //warning
                    Label warningLabel = new Label();
                    nameLabel.ID = "warningDeleteLabel" + device.DeviceID;
                    nameLabel.Text = "Deleting this camera will permanently remove it from your undeployed camera list. SpotCheck will ship you a box with an included shipping label to send the camera back.";
                    nameLabel.CssClass = "text-align: center;";

                    divBody.Controls.Add(nameLabel);
                    divBody.Controls.Add(new LiteralControl("<br />"));
                    divBody.Controls.Add(new LiteralControl("<br />"));
                    divBody.Controls.Add(warningLabel);

                    //footer controls
                    HtmlGenericControl divFooter = new HtmlGenericControl("div");
                    divFooter.Attributes.Add("class", "modal-footer");

                    HtmlGenericControl btnCloseFooter = new HtmlGenericControl("button");
                    btnCloseFooter.Attributes.Add("type", "button");
                    btnCloseFooter.Attributes.Add("class", "btn btn-secondary");
                    btnCloseFooter.Attributes.Add("data-dismiss", "modal");
                    btnCloseFooter.InnerHtml = "Cancel";

                    Button btnDeleteSubmit = new Button();
                    btnDeleteSubmit.ID = "btnUndeploySubmit" + device.DeviceID;
                    btnDeleteSubmit.CssClass = "btn btn-danger";
                    btnDeleteSubmit.Text = "Delete";
                    btnDeleteSubmit.Click += new EventHandler(btnDeleteSubmit_Click);
                    btnDeleteSubmit.CommandArgument = Convert.ToString(device.DeviceID);

                    divFooter.Controls.Add(btnDeleteSubmit);
                    divFooter.Controls.Add(btnCloseFooter);

                    div3.Controls.Add(divHeader);
                    div3.Controls.Add(divBody);
                    div3.Controls.Add(divFooter);

                    div2.Controls.Add(div3);
                    div1.Controls.Add(div2);

                    modalDiv.Controls.Add(div1);
                }
            }
        }

        private void LoadModals()
        {
            modalDiv.Controls.Clear();
            CreateDeployedEditModals();
            CreateDeployedUndeployModals();
            CreateUnDeployedEditModals();
            CreateUndeployedDeleteModals();
            CreateAddModal();
            modalUpdatePanel.Update();    //force update to update panel for modals
        }

        private void CreateDeviceListDivs(eDeviceStatus.DeviceStatus[] statuses)
        {
            string createType = "";

            //for deployed
            if (statuses.Count() == 2 && statuses.Contains(eDeviceStatus.DeviceStatus.Deployed) && statuses.Contains(eDeviceStatus.DeviceStatus.ReadyForSpots))
            {
                createType = "deployed";
            }
            else if (statuses.Count() == 1 && statuses.Contains(eDeviceStatus.DeviceStatus.Undeployed))
            {
                createType = "undeployed";
            }

            List<Device> devices = new List<Device>();

            //clear existing modals
            switch (createType)
            {
                case ("deployed"):
                    deployedCameraContainer.Controls.Clear();
                    devices = getDeployedCamerasFromParkingLotID((int)selectedParkingLot.LotID);
                    break;
                case ("undeployed"):
                    undeployedCameraContainer.Controls.Clear();
                    devices = getUndeployedCameras();
                    break;
            }

            foreach (Device device in devices)
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

                h6.InnerHtml = device.DeviceName;
                h6.Attributes.Add("class", "m-0 font-weight-bold text-primary");
                h6.Attributes.Add("style", "float:left; width:45%;");
                link.Controls.Add(h6);

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

                HtmlGenericControl outerButtonContainer = new HtmlGenericControl("div");
                outerButtonContainer.Attributes.Add("class", "container-fluid");

                HtmlGenericControl innerButtonContainer = new HtmlGenericControl("div");
                innerButtonContainer.Attributes.Add("class", "row no-gutters");

                //create different buttons for deployed and undeployed
                switch (createType)
                {
                    case ("deployed"):
                        HtmlGenericControl editDeployedButtonDiv = new HtmlGenericControl("div");
                        editDeployedButtonDiv.Attributes.Add("class", "col-6");
                        editDeployedButtonDiv.Attributes.Add("style", "padding-right: 5px; padding-bottom: 0px;");
                        HtmlGenericControl editDeployedButton = new HtmlGenericControl("button");
                        editDeployedButton.Attributes.Add("data-toggle", "modal");
                        editDeployedButton.Attributes.Add("data-target", "#editModal" + device.DeviceID);
                        editDeployedButton.Attributes.Add("type", "button");
                        editDeployedButton.Attributes.Add("class", "btn btn-primary btn-block");
                        editDeployedButton.Attributes.Add("data-backdrop", "false");
                        editDeployedButton.InnerHtml = "Edit";
                        editDeployedButtonDiv.Controls.Add(editDeployedButton);

                        HtmlGenericControl undeployButtonDiv = new HtmlGenericControl("div");
                        undeployButtonDiv.Attributes.Add("class", "col-6");
                        undeployButtonDiv.Attributes.Add("style", "padding-left: 5px; padding-bottom: 0px;");
                        HtmlGenericControl undeployButton = new HtmlGenericControl("button");
                        undeployButton.Attributes.Add("data-toggle", "modal");
                        undeployButton.Attributes.Add("data-target", "#undeployModal" + device.DeviceID);
                        undeployButton.Attributes.Add("type", "button");
                        undeployButton.Attributes.Add("class", "btn btn-warning btn-block");
                        undeployButton.Attributes.Add("data-backdrop", "false");
                        undeployButton.InnerHtml = "Undeploy";
                        undeployButtonDiv.Controls.Add(undeployButton);

                        innerButtonContainer.Controls.Add(editDeployedButtonDiv);
                        innerButtonContainer.Controls.Add(undeployButtonDiv);
                        break;

                    case ("undeployed"):
                        HtmlGenericControl deployButtonDiv = new HtmlGenericControl("div");
                        deployButtonDiv.Attributes.Add("class", "col-4");
                        deployButtonDiv.Attributes.Add("style", "padding-right: 2px; padding-bottom: 0px;");
                        Button deployButton = new Button();
                        deployButton.ID = "deployButton" + device.DeviceID.ToString();
                        deployButton.Attributes.Add("data-toggle", "modal");
                        deployButton.Attributes.Add("data-target", "#deployModal");
                        deployButton.Attributes.Add("type", "button");
                        deployButton.Attributes.Add("class", "btn btn-success btn-block");
                        deployButton.Attributes.Add("data-backdrop", "false");
                        deployButton.Click += new EventHandler(deployButton_Click);
                        deployButton.Text = "Deploy";
                        deployButton.CommandArgument = device.DeviceID.ToString();

                        deployButtonDiv.Controls.Add(deployButton);

                        HtmlGenericControl editButtonDiv = new HtmlGenericControl("div");
                        editButtonDiv.Attributes.Add("class", "col-4");
                        editButtonDiv.Attributes.Add("style", "padding-left: 2px; padding-right: 2px; padding-bottom: 0px;");
                        HtmlGenericControl editButton = new HtmlGenericControl("button");
                        editButton.Attributes.Add("data-toggle", "modal");
                        editButton.Attributes.Add("data-target", "#editModal" + device.DeviceID);
                        editButton.Attributes.Add("type", "button");
                        editButton.Attributes.Add("class", "btn btn-primary btn-block");
                        editButton.Attributes.Add("data-backdrop", "false");
                        editButton.InnerHtml = "Edit";
                        editButtonDiv.Controls.Add(editButton);

                        HtmlGenericControl deleteButtonDiv = new HtmlGenericControl("div");
                        deleteButtonDiv.Attributes.Add("class", "col-4");
                        deleteButtonDiv.Attributes.Add("style", "padding-left: 2px; padding-bottom: 0px;");
                        HtmlGenericControl deleteButton = new HtmlGenericControl("button");
                        deleteButton.Attributes.Add("data-toggle", "modal");
                        deleteButton.Attributes.Add("data-target", "#deleteModal" + device.DeviceID);
                        deleteButton.Attributes.Add("type", "button");
                        deleteButton.Attributes.Add("class", "btn btn-danger btn-block");
                        deleteButton.Attributes.Add("data-backdrop", "false");
                        deleteButton.InnerHtml = "Delete";
                        deleteButtonDiv.Controls.Add(deleteButton);

                        innerButtonContainer.Controls.Add(deployButtonDiv);
                        innerButtonContainer.Controls.Add(editButtonDiv);
                        innerButtonContainer.Controls.Add(deleteButtonDiv);
                        break;
                }

                outerButtonContainer.Controls.Add(innerButtonContainer);

                innerDiv.Controls.Add(extAddressDiv);
                innerDiv.Controls.Add(localAddressDiv);
                innerDiv.Controls.Add(macAddressDiv);
                innerDiv.Controls.Add(new LiteralControl("<br />"));
                innerDiv.Controls.Add(outerButtonContainer);

                middleDiv.Controls.Add(innerDiv);

                outerDiv.Controls.Add(link);
                outerDiv.Controls.Add(middleDiv);

                switch (createType)
                {
                    case ("deployed"):
                        deployedCameraContainer.Controls.Add(outerDiv);
                        break;
                    case ("undeployed"):
                        undeployedCameraContainer.Controls.Add(outerDiv);
                        break;
                }
            }

            LoadModals();
        }

        public void populateDeployDropDownList()
        {
            ListItem defaultItem = new ListItem();
            defaultItem.Value = "0";
            defaultItem.Text = "Select parking lot";
            defaultItem.Enabled = true;
            defaultItem.Selected = true;
            deployParkingLotDropDownList.Items.Add(defaultItem);

            foreach (ParkingLot parkingLot in parkingLots)
            {
                ListItem parkingLotItem = new ListItem();
                parkingLotItem.Value = ((int)parkingLot.LotID).ToString();
                parkingLotItem.Text = parkingLot.LotName;

                deployParkingLotDropDownList.Items.Add(parkingLotItem);
            }
        }

        #endregion Methods

    }
}