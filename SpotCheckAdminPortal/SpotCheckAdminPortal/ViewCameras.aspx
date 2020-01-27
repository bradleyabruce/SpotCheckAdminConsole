﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewCameras.aspx.cs" Inherits="SpotCheckAdminPortal.ViewCameras" %>

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

   <!-- Custom styles for this template-->
   <link href="css/sb-admin-2.min.css" rel="stylesheet" />
</head>

<body id="viewCamerasBody" runat="server">

   <form id="cameraForm" runat="server">

      <div id="loadingCamerasDiv" runat="server">

         <!--Loading Window -->
         <asp:UpdateProgress runat="server" ID="cameraUpdateProgress" AssociatedUpdatePanelID="cameraUpdatePanel">
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
                  <asp:UpdatePanel runat="server" ID="cameraUpdatePanel">
                     <ContentTemplate runat="server">

                        <!-- Alert Div -->
                        <div runat="server" id="alertDiv"></div>

                        <!-- Page Heading -->
                        <div class="d-sm-flex align-items-center justify-content-between mb-4">
                           <h1 class="h3 mb-0 text-gray-800">Cameras</h1>
                           <button type="button" class="d-none d-sm-inline-block btn btn-sm btn-primary shadow-sm" data-toggle="modal" data-backdrop="false" data-target="#addModal"><i class="fas fa-plus fa-sm text-white-50"></i> Order New Camera</button>
                        </div>

                        <!-- Container to Hold all Programmatically Created HTML -->
                        <div runat="server" id="cameraContainer" style="width: 30%;"></div><!-- overflow-y: scroll; overflow-x: hidden; height: 75%; position: page;-->

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
