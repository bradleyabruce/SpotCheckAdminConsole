using System;
using System.IO;
using System.Net;
using System.Web.Configuration;
using SpotCheckAdminPortal.Models;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SpotCheckAdminPortal.DataLayer
{
    public class Company_dl : Company
    {
        string API_URL = "http://173.91.255.135:8080/SpotCheckServer-2.1.8.RELEASE/";

        public Company_dl(Company company)
        {
            this.CompanyID = company.CompanyID;
            this.CompanyName = company.CompanyName;
            this.Address = company.Address;
            this.City = company.City;
            this.ZipCode = company.ZipCode;
            this.State = company.State;
            this.CompanyUsername = company.CompanyUsername;
            this.CompanyPassword = company.CompanyPassword;
        }

        #region Login

        public new bool? Login()
        {
            string url = API_URL + "company/login";
            string json = Connect_dl.BuildJson(this);

            HttpWebRequest request = Connect_dl.BuildRequest(url, "POST", json);

            if (request != null)
            {
                Dictionary<HttpStatusCode, string> response = Connect_dl.GetResponse(request);
                HttpStatusCode code = response.FirstOrDefault().Key;
                string httpResponse = response.FirstOrDefault().Value;

                if (code == HttpStatusCode.OK)
                {
                    //save company in IoC, return true
                    Company currentCompany = Newtonsoft.Json.JsonConvert.DeserializeObject<Company>(httpResponse);
                    IoC.CurrentCompany = currentCompany;
                    return true;
                }
                else
                {
                    if (httpResponse == "Incorrect Username or Password")
                    {
                        //clear company, return false for incorrect username/password
                        Company company = new Company();
                        IoC.CurrentCompany = company;
                        return false;
                    }
                    else
                    {
                        //clear company, return null for server error
                        Company company = new Company();
                        IoC.CurrentCompany = company;
                        return null;
                    }
                }
            }
            else
            {
                //clear company, return null for server error
                Company company = new Company();
                IoC.CurrentCompany = company;
                return null;
            }
        }

        #endregion Login

        #region SignUp

        public new bool? SignUp()
        {
            string url = API_URL + "company/signUp";
            string json = Connect_dl.BuildJson(this);

            HttpWebRequest request = Connect_dl.BuildRequest(url, "POST", json);
            if (request != null)
            {
                Dictionary<HttpStatusCode, string> response = Connect_dl.GetResponse(request);
                HttpStatusCode code = response.FirstOrDefault().Key;
                string httpResponse = response.FirstOrDefault().Value;

                if (code == HttpStatusCode.OK)
                {
                    //clear company in IoC, return true
                    Company company = new Company();
                    IoC.CurrentCompany = company;
                    return true;
                }
                else
                {
                    if (httpResponse == "Company already exists with specified username.")
                    {
                        //clear company, return false for incorrect username/password
                        Company company = new Company();
                        IoC.CurrentCompany = company;
                        return false;
                    }
                    else
                    {
                        //clear company, return null for server error
                        Company company = new Company();
                        IoC.CurrentCompany = company;
                        return null;
                    }
                }
            }
            else
            {
                //clear company, return null for server error
                Company company = new Company();
                IoC.CurrentCompany = company;
                return null;
            }
        }

        #endregion SignUp
    }
}
