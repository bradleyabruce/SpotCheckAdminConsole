﻿<%@ Page Language="C#" Inherits="SpotCheckAdminPortal.index" %>
<!DOCTYPE html>
<html>
<head runat="server">
   <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">

    <title>SpotCheck</title>
    <meta name="description" content="Free Bootstrap 4 Template by uicookies.com">
    <meta name="keywords" content="Free website templates, Free bootstrap themes, Free template, Free bootstrap, Free website template">

    <link href="https://fonts.googleapis.com/css?family=Crimson+Text:400,400i,600|Montserrat:200,300,400" rel="stylesheet">

    <link rel="stylesheet" href="assets/css/bootstrap/bootstrap.css">
    <link rel="stylesheet" href="assets/fonts/ionicons/css/ionicons.min.css">
    <link rel="stylesheet" href="assets/fonts/law-icons/font/flaticon.css">

    <link rel="stylesheet" href="assets/fonts/fontawesome/css/font-awesome.min.css">


    <link rel="stylesheet" href="assets/css/slick.css">
    <link rel="stylesheet" href="assets/css/slick-theme.css">

    <link rel="stylesheet" href="assets/css/helpers.css">
    <link rel="stylesheet" href="assets/css/style.css">
    <link rel="stylesheet" href="assets/css/landing-2.css">
</head>
<body data-spy="scroll" data-target="#pb-navbar" data-offset="200">
 <form id="indexForm" runat="server">
            
            <!-- START Nav -->
            <nav class="navbar navbar-expand-lg navbar-dark pb_navbar pb_scrolled-light" id="pb-navbar">
                <div class="container">
                    <a class="navbar-brand" href="index.aspx">SpotCheck</a>
                    <button class="navbar-toggler ml-auto" type="button" data-toggle="collapse" data-target="#probootstrap-navbar" aria-controls="probootstrap-navbar" aria-expanded="false" aria-label="Toggle navigation">
                        <span><i class="ion-navicon"></i></span>
                    </button>
                    <div class="collapse navbar-collapse" id="probootstrap-navbar">
                        <ul class="navbar-nav ml-auto">
                            <li class="nav-item"><a class="nav-link" href="#section-home">Home</a></li>
                            <li class="nav-item"><a class="nav-link" href="#section-features">Features</a></li>
                            <li class="nav-item"><a class="nav-link" href="#section-faq">FAQ</a></li>
                        </ul>
                    </div>
                </div>
            </nav>
            <!-- END Nav -->
    
            <!-- START Section Home -->
            <section class="pb_cover_v3 overflow-hidden cover-bg-indigo cover-bg-opacity text-left pb_gradient_v1 pb_slant-light" id="section-home">
                <div class="container">
                    <div class="row align-items-center justify-content-center">
                        <div class="col-md-6">
                            <h2 class="heading mb-3">SpotCheck Helps You Get There Faster</h2>
                            <div class="sub-heading">
                                <p class="mb-4">Using computer vision SpotCheck helps parking lots visualize how many open spots they have available so drivers can get where they need to go faster </p>
                            </div>
                        </div>
                        <div class="col-md-5 relative align-self-center">
                            <div id="register" class="bg-white rounded pb_form_v1">
                                <h2 class="mb-4 mt-0 text-center">Admin Login</h2>
                                <div class="form-group">
                                    <asp:TextBox id="TextBoxCompanyUsername" CssClass="form-control pb_height-50 reverse" placeholder="Company Username" runat="server"/>
                                </div>
                                <div class="form-group">
                                    <asp:TextBox id="TextBoxCompanyPassword" CssClass="form-control pb_height-50 reverse" placeholder="Password" TextMode="Password" runat="server"/>
                                </div>
                                <div class="form-group">
                                    <asp:Button id="LoginButton" CssClass="btn btn-primary btn-lg btn-block pb_btn-pill  btn-shadow-blue" OnClick="LoginButton_Click" Text="Login" runat="server"/>
                                </div>                               
                                <div class="form-group">
                                    <asp:Label runat="server" id="ErrorLabel" Text="" CssClass="labelErrorMessage" ForeColor="Red"/>
                                </div>
                                <div class="form-group" style="text-align: center">
                                    <asp:Button id="SignUpInsteadButton"  CssClass="btn-form-switch" OnClick="SignUpInsteadButton_Click" Text="Sign Up Instead" runat="server"/>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
            <!-- END section Home -->
            
            <!-- START SECTION Features -->            
            <!--
            <section class="pb_section bg-light pb_slant-white pb_pb-250" id="section-features">
                <div class="container">
                    <div class="row justify-content-center mb-5">
                        <div class="col-md-6 text-center mb-5">
                            <h5 class="text-uppercase pb_font-15 mb-2 pb_color-dark-opacity-3 pb_letter-spacing-2"><strong>Features</strong></h5>
                            <h2>App Features</h2>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-4 col-md- col-sm-6">
                            <div class="media d-block pb_feature-v1 text-left">
                                <div class="pb_icon"><i class="ion-ios-speedometer-outline pb_icon-gradient"></i></div>
                                <div class="media-body">
                                    <h5 class="mt-0 mb-3 heading">User Friendly App</h5>
                                    <p class="text-sans-serif">Quick and responsive app that allows drivers to easily see nearby parking lots with open spots.</p>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-4 col-md- col-sm-6">
                            <div class="media d-block pb_feature-v1 text-left">
                                <div class="pb_icon"><i class="ion-ios-infinite-outline pb_icon-gradient"></i></div>
                                <div class="media-body">
                                    <h5 class="mt-0 mb-3 heading">Computer Vision</h5>
                                    <p class="text-sans-serif">Utilizng computer vision and a trained model we are able to detect when a parking spot is avialable and  update our database accordingly. </p>
                                </div>
                            </div>
                        </div>

                        <div class="col-lg-4 col-md- col-sm-6">
                            <div class="media d-block pb_feature-v1 text-left">
                                <div class="pb_icon"><i class="ion-ios-color-filter-outline pb_icon-gradient"></i></div>
                                <div class="media-body">
                                    <h5 class="mt-0 mb-3 heading">A Third Thing</h5>
                                    <p class="text-sans-serif">I guess we will need to think of a new third thing to enter</p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </section>-->
            <!-- END Section Features -->

            <!-- START Section FAQ -->
            <!--
            <section class="pb_section pb_slant-white" id="section-faq">
                <div class="container">
                    <div class="row justify-content-center mb-5">
                        <div class="col-md-6 text-center mb-5">
                            <h5 class="text-uppercase pb_font-15 mb-2 pb_color-dark-opacity-3 pb_letter-spacing-2"><strong>FAQ</strong></h5>
                            <h2>Frequently Ask Questions</h2>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md">
                            <div id="pb_faq" class="pb_accordion" data-children=".item">
                                <div class="item">
                                    <a data-toggle="collapse" data-parent="#pb_faq" href="#pb_faq1" aria-expanded="true" aria-controls="pb_faq1" class="pb_font-22 py-4">What is Instant?</a>
                                    <div id="pb_faq1" class="collapse show" role="tabpanel">
                                        <div class="py-3">
                                            <p>Pityful a rethoric question ran over her cheek, then she continued her way.</p>
                                            <p>Far far away, behind the word mountains, far from the countries Vokalia and Consonantia, there live the blind texts. Separated they live in Bookmarksgrove right at the coast of the Semantics, a large language ocean.</p>
                                        </div>
                                    </div>
                                </div>
                                <div class="item">
                                    <a data-toggle="collapse" data-parent="#pb_faq" href="#pb_faq2" aria-expanded="false" aria-controls="pb_faq2" class="pb_font-22 py-4">Is this available to my country?</a>
                                    <div id="pb_faq2" class="collapse" role="tabpanel">
                                        <div class="py-3">
                                            <p>A small river named Duden flows by their place and supplies it with the necessary regelialia. It is a paradisematic country, in which roasted parts of sentences fly into your mouth.</p>
                                        </div>
                                    </div>
                                </div>
                                <div class="item">
                                    <a data-toggle="collapse" data-parent="#pb_faq" href="#pb_faq3" aria-expanded="false" aria-controls="pb_faq3" class="pb_font-22 py-4">How do I use the features of Instant App?</a>
                                    <div id="pb_faq3" class="collapse" role="tabpanel">
                                        <div class="py-3">
                                            <p>Even the all-powerful Pointing has no control about the blind texts it is an almost unorthographic life One day however a small line of blind text by the name of Lorem Ipsum decided to leave for the far World of Grammar.</p>
                                        </div>
                                    </div>
                                </div>
                                <div class="item">
                                    <a data-toggle="collapse" data-parent="#pb_faq" href="#pb_faq4" aria-expanded="false" aria-controls="pb_faq4" class="pb_font-22 py-4">How much do the Instant App cost?</a>
                                    <div id="pb_faq4" class="collapse" role="tabpanel">
                                        <div class="py-3">
                                            <p>The Big Oxmox advised her not to do so, because there were thousands of bad Commas, wild Question Marks and devious Semikoli, but the Little Blind Text didn’t listen. She packed her seven versalia, put her initial into the belt and made herself on the way.</p>
                                        </div>
                                    </div>
                                </div>

                                <div class="item">
                                    <a data-toggle="collapse" data-parent="#pb_faq" href="#pb_faq5" aria-expanded="false" aria-controls="pb_faq5" class="pb_font-22 py-4">I have technical problem, who do I email?</a>
                                    <div id="pb_faq5" class="collapse" role="tabpanel">
                                        <div class="py-3">
                                            <p>On her way she met a copy. The copy warned the Little Blind Text, that where it came from it would have been rewritten a thousand times and everything that was left from its origin would be the word "and" and the Little Blind Text should turn around and return to its own, safe country. </p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </section> -->
            <!-- END SECTION FAQ -->
    </form> 
</body>
</html>

