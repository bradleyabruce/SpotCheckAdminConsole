<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewCameras.aspx.cs" Inherits="SpotCheckAdminPortal.ViewCameras" %>

<!DOCTYPE html>

<html>
<head runat="server">
   <meta charset="utf-8" />
   <meta http-equiv="X-UA-Compatible" content="IE=edge" />
   <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
   <meta name="description" content="" />
   <meta name="author" content="" />

   <title>View Cameras</title>

   <!-- Custom fonts for this template-->
   <link href="vendor/fontawesome-free/css/all.min.css" rel="stylesheet" type="text/css" />
   <link href="https://fonts.googleapis.com/css?family=Nunito:200,200i,300,300i,400,400i,600,600i,700,700i,800,800i,900,900i" rel="stylesheet" />

   <!--Javascript -->
   <script type="text/javascript" src="js/ViewCameras.js"></script>

   <!-- Custom styles for this template-->
   <link href="css/sb-admin-2.min.css" rel="stylesheet" />
</head>

<body id="viewCamerasBody" runat="server">

   <form id="cameraForm" runat="server">

      <!--<canvas id="imageCanvas" width="640" height="360" style="border: 1px solid black"></canvas>-->

      <div id="loadingCamerasDiv" runat="server">

         <!--Loading Windows -->
         <asp:UpdateProgress runat="server" ID="deployedCameraUpdateProgress" AssociatedUpdatePanelID="deployedCameraUpdatePanel">
            <ProgressTemplate>
               <div style="opacity: 0.5; background: #000; width: 100%; height: 100%; z-index: 10; top: 0; left: 0; position: fixed;"></div>
            </ProgressTemplate>
         </asp:UpdateProgress>
         <asp:UpdateProgress runat="server" ID="undeployedCameraUpdateProgress" AssociatedUpdatePanelID="undeployedCameraUpdatePanel">
            <ProgressTemplate>
               <div style="opacity: 0.5; background: #000; width: 100%; height: 100%; z-index: 10; top: 0; left: 0; position: fixed;"></div>
            </ProgressTemplate>
         </asp:UpdateProgress>
         <asp:UpdateProgress runat="server" ID="modalUpdateProgress" AssociatedUpdatePanelID="modalUpdatePanel">
            <ProgressTemplate>
               <div style="opacity: 0.5; background: #000; width: 100%; height: 100%; z-index: 10; top: 0; left: 0; position: fixed;"></div>
            </ProgressTemplate>
         </asp:UpdateProgress>
         <asp:UpdateProgress runat="server" ID="deployModalUpdateProgress" AssociatedUpdatePanelID="deployModalUpdatePanel">
            <ProgressTemplate>
               <div style="opacity: 0.5; background: #000; width: 100%; height: 100%; z-index: 10; top: 0; left: 0; position: fixed;"></div>
            </ProgressTemplate>
         </asp:UpdateProgress>

      </div>

      <!-- Page Wrapper -->
      <div id="wrapper">

         <!-- Sidebar -->
         <ul class="navbar-nav bg-gradient-primary sidebar sidebar-dark accordion" id="accordionSidebar">
            <!-- Sidebar - Brand -->
            <a class="sidebar-brand d-flex align-items-center justify-content-center" href="Dashboard.aspx">
               <div>S</div>
               <div class="rotate-n-15">
                  <i class="fas fa-fw fa-parking"></i>
               </div>
               <div>otCheck</div>
            </a>

            <!-- Nav - Dashboard Section -->
            <hr class="sidebar-divider my-0">
            <!-- Nav Item - Dashboard -->
            <li class="nav-item">
               <a class="nav-link" href="Dashboard.aspx">
                  <i class="fas fa-fw fa-tachometer-alt"></i>
                  <span>Dashboard</span>
               </a>
            </li>

            <!-- Divider -->
            <hr class="sidebar-divider my-0">

            <!-- Nav - Parking Lots Section -->
            <li class="nav-item">
               <a class="nav-link" href="ViewParkingLots.aspx">
                  <i class="fas fa-fw fa-building"></i>
                  <span>Parking Lots</span></a>
            </li>

            <!-- Nav Item - Cameras -->
            <li class="nav-item active">
               <a class="nav-link" href="ViewCameras.aspx">
                  <i class="fas fa-fw fa-video"></i>
                  <span>Cameras</span>
               </a>
            </li>

            <hr class="sidebar-divider my-0">
         </ul>

         <!-- Content Wrapper -->
         <div id="content-wrapper" class="d-flex flex-column">

            <!-- Main Content -->
            <div id="content">

               <!-- Topbar -->
               <nav class="navbar navbar-expand navbar-light bg-white topbar mb-4 static-top shadow">

                  <!-- Topbar Navbar -->
                  <ul class="navbar-nav ml-auto">

                     <div class="topbar-divider d-none d-sm-block"></div>

                     <!-- Nav Item - User Information -->
                     <li class="nav-item dropdown no-arrow">
                        <a class="nav-link dropdown-toggle" href="#" id="userDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                           <span class="mr-2 d-none d-lg-inline text-gray-600 small">
                              <asp:Literal ID="CompanyNameLiteral" runat="server" Text="Error" /></span>
                           <!--<img class="img-profile rounded-circle" src="https://source.unsplash.com/QAB-WJcbgJk/60x60">-->
                        </a>
                        <!-- Dropdown - User Information -->
                        <div class="dropdown-menu dropdown-menu-right shadow animated--grow-in" aria-labelledby="userDropdown">
                           <a class="dropdown-item" href="#">
                              <i class="fas fa-user fa-sm fa-fw mr-2 text-gray-400"></i>
                              Profile
                           </a>
                           <a class="dropdown-item" href="#">
                              <i class="fas fa-cogs fa-sm fa-fw mr-2 text-gray-400"></i>
                              Settings
                           </a>

                           <div class="dropdown-divider"></div>
                           <a class="dropdown-item" href="#" data-toggle="modal" data-target="#logoutModal">
                              <i class="fas fa-sign-out-alt fa-sm fa-fw mr-2 text-gray-400"></i>
                              Logout
                           </a>
                        </div>
                     </li>
                  </ul>
               </nav>
               <!-- End of Topbar -->

               <!-- Begin Page Content -->
               <div class="container-fluid" runat="server" id="camerasBodyDiv" style="position: relative">
                  <asp:ScriptManager ID="cameraScriptManager" runat="server"></asp:ScriptManager>

                  <!-- Alert Div -->
                  <asp:UpdatePanel runat="server" ID="alertUpdatePanel" UpdateMode="Conditional" ChildrenAsTriggers="false">
                     <ContentTemplate runat="server">
                        <div runat="server" id="alertDiv"></div>
                     </ContentTemplate>
                  </asp:UpdatePanel>

                  <!-- Page Heading -->
                  <div class="d-sm-flex align-items-center justify-content-between mb-4">
                     <h1 class="h3 mb-0 text-gray-800">Cameras</h1>
                     <button type="button" class="d-none d-sm-inline-block btn btn-sm btn-primary shadow-sm" data-toggle="modal" data-backdrop="false" data-target="#addModal"><i class="fas fa-plus fa-sm text-white-50"></i>&nbsp;Order New Camera</button>
                  </div>

                  <!-- Container to Hold all Programmatically Created HTML -->
                  <div id="LeftRightContainer" runat="server" style="width: 100%; margin: auto; padding: 10px;">

                     <asp:UpdatePanel runat="server" ID="modalUpdatePanel" UpdateMode="Conditional" ChildrenAsTriggers="false">
                        <ContentTemplate runat="server">
                           <div id="modalDiv" runat="server"></div>
                        </ContentTemplate>
                     </asp:UpdatePanel>


                     <!-- Separate Div just for deploy modals as they need to be handled differently -->
                     <div id="deployCameraContainerDiv" runat="server"></div>

                     <!-- Left -->
                     <asp:UpdatePanel runat="server" ID="deployedCameraUpdatePanel" UpdateMode="Conditional" ChildrenAsTriggers="false">
                        <ContentTemplate runat="server">
                           <div id="left" runat="server" style="width: 48%; float: left;">
                              <div id="comboBoxDiv" runat="server">
                                 <h3 class="h4 mb-0 text-gray-800"><u>Deployed Cameras</u></h3>
                                 <p>Select a parking lot to view cameras.</p>
                                 <asp:DropDownList ID="parkingLotDropDownList" runat="server" AutoPostBack="true" ClientIDMode="Inherit" Style="width: 80%" OnSelectedIndexChanged="parkingLotDropDownList_SelectedIndexChanged"></asp:DropDownList>
                              </div>
                              <br />
                              <div runat="server" id="deployedCameraContainer"></div>
                           </div>
                        </ContentTemplate>
                     </asp:UpdatePanel>
                     <!-- End Left -->

                     <!-- Right -->
                     <asp:UpdatePanel runat="server" ID="undeployedCameraUpdatePanel" UpdateMode="Conditional" ChildrenAsTriggers="false">
                        <ContentTemplate runat="server">
                           <div id="right" runat="server" style="margin-left: 52%">
                              <div id="undeployedCameraDiv" runat="server" style="flex: 1;">
                                 <h3 class="h4 mb-0 text-gray-800"><u>Undeployed Cameras</u></h3>
                              </div>
                              <br />
                              <br />
                              <p></p>
                              <br />
                              <div runat="server" id="undeployedCameraContainer" style="flex: 1;">
                              </div>
                           </div>
                        </ContentTemplate>
                     </asp:UpdatePanel>
                     <!-- End Right -->
                  </div>

                  <!-- Deploy Camera Modal -->
                  <asp:UpdatePanel ID="deployModalUpdatePanel" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                     <ContentTemplate>
                        <div id="deployModal" class="modal" tabindex="-1" role="dialog" style="z-index: 1">
                           <div id="div2Deploy" class="modal-dialog modal-dialog-centered modal-xl" role="document">
                              <div id="div3Deploy" class="modal-content">
                                 <div id="divHeaderDeploy" class="modal-header">
                                    <h5 class="modal-title">Deploy Camera
                                    </h5>
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close" id="deployCloseHeader">
                                       <span aria-hidden="true">&times;</span>
                                    </button>
                                 </div>
                                 <div id="divBodyDeploy" class="modal-body" style="width: 100%; margin: auto;">
                                    <!-- left side -->
                                    <div id="leftBodyDeploy" style="width: 35%; float: left;">
                                       <label id="parkingLotDropDownDeployLabel">Select parking lot to deploy camera at:</label>
                                       <br />
                                       <asp:DropDownList id="deployParkingLotDropDownList" runat="server" Width="250"></asp:DropDownList>
                                       <br />
                                       <br />
                                       <!-- buttons -->
                                       <label id="buttonDeployLabel">SpotCheck Vision requires that you set the parking spots on the image. These parking spots will count towards the parking lot's total spot count. Use the add button to begin drawing parking spots with the mouse.</label>
                                       <br />
                                        <button type="button" id="addParkingSpotCoordinateButton" class="btn btn-success" title="Add new parking space" name="options" autocomplete="off">
                                          <i class="fas fa-crop-alt text-white"></i>
                                       </button>

                                       <button type="button" name="options" class="btn btn-warning" id="removeParkingSpotCoordinateButton" title="Remove single parking space" autocomplete="off">
                                            <i class="fas fa-times text-white"></i> 
                                       </button>

                                                        
                                       <button id="removeAllParkingSpotCoordinatesButton" type="button" class="btn btn-danger" title="Clear all parking spaces">
                                          <i class="fas fa-trash-alt text-white"></i>
                                       </button>
                                                        
                                       <button id="deployDone" class="btn btn-primary" style="width: 180px; visibility: hidden;" type="button">Done</button>
                                    </div>
                                    <!-- right side -->
                                    <div id="rightBodyDeploy" style="margin-left: 35%;">
                                       <br />
                                       <canvas id="imageCanvas" width="640" height="360" style="border: 1px solid black"></canvas>
                                    </div>
                                 </div>
                                 <div id="divFooterDeploy" class="modal-footer" runat="server">
                                    
                                 </div>
                              </div>
                           </div>
                        </div>
                     </ContentTemplate>
                  </asp:UpdatePanel>

               </div>
               <!-- /.container-fluid -->

            </div>
            <!-- End of Main Content -->

            <!-- Footer -->
            <footer class="sticky-footer bg-white">
               <div class="container my-auto">
                  <div class="copyright text-center my-auto">
                     <span>Copyright &copy; SpotCheck 2020</span>
                                <asp:UpdatePanel id="hiddenInfoPanel" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                                    <ContentTemplate>
                                        <div style="display:none;">
                                            <asp:TextBox id="hiddenImageStringField" runat="server" ></asp:TextBox>
                                            <asp:TextBox id="hiddenCameraIDField" runat="server"></asp:TextBox>
                                            <asp:TextBox id="hiddenSpotCoordJsonField" runat="server"></asp:TextBox>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                  </div>
               </div>
            </footer>
            <!-- End of Footer -->

         </div>
         <!-- End of Content Wrapper -->

      </div>
      <!-- End of Page Wrapper -->

      <!-- Scroll to Top Button-->
      <a class="scroll-to-top rounded" href="#page-top">
         <i class="fas fa-angle-up"></i>
      </a>

      <!-- Logout Modal-->
      <div class="modal fade" id="logoutModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
         <div class="modal-dialog" role="document">
            <div class="modal-content">
               <div class="modal-header">
                  <h5 class="modal-title" id="exampleModalLabel">Ready to Leave?</h5>
                  <button class="close" type="button" data-dismiss="modal" aria-label="Close">
                     <span aria-hidden="true">×</span>
                  </button>
               </div>
               <div class="modal-body">Select "Logout" below if you are ready to end your current session.</div>
               <div class="modal-footer">
                  <button class="btn btn-secondary" type="button" data-dismiss="modal">Cancel</button>
                  <a class="btn btn-primary" href="login.html">Logout</a>
               </div>
            </div>
         </div>
      </div>
   </form>

   <script src="vendor/jquery/jquery.min.js"></script>
   <script src="vendor/bootstrap/js/bootstrap.bundle.min.js"></script>
   <script src="vendor/jquery-easing/jquery.easing.min.js"></script>
   <script src="js/sb-admin-2.min.js"></script>

</body>
</html>
