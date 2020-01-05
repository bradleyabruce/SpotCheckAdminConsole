using System;
using System.IO;
using System.Net;
using System.Web.Configuration;
using SpotCheckAdminPortal.Models;
using System.Configuration;

namespace SpotCheckAdminPortal.DataLayer
{
    public class Company_dl : Company
    {
        string API_URL = "http://173.91.255.135:8080/SpotCheckServer-2.1.8.RELEASE/";

        private string httpError;
        private bool isResponseOK;


        public Company_dl(Company company)
        {
            this.CompanyID = company.CompanyID;
            this.CompanyName = company.CompanyName;
            this.Address = company.Address;
            this.City = company.City;
            this.State = company.State;
            this.CompanyUsername = company.CompanyUsername;
            this.CompanyPassword = company.CompanyPassword;
        }

        #region Login

        public new bool? Login()
        {
            string url = API_URL + "company/login";
            HttpWebRequest request = this.BuildRequest(url);

            if (request != null)
            {
                string response = GetResponse(request);

                if (isResponseOK)
                {
                    //save company in IoC
                    Company currentCompany = Newtonsoft.Json.JsonConvert.DeserializeObject<Company>(response);
                    IoC.CurrentCompany = currentCompany;
                    return true;
                }
                else
                {
                    if (httpError == "Incorrect Username or Password")
                    {
                        Company company = new Company();
                        IoC.CurrentCompany = company;
                        return false;
                    }
                    else
                    {
                        Company company = new Company();
                        IoC.CurrentCompany = company;
                        return null;
                    }
                }
            }
            else
            {
                Company company = new Company();
                IoC.CurrentCompany = company;
                return null;
            }
        }

        private HttpWebRequest BuildRequest(string url)
        {
            try
            {
                Uri uri = new Uri(url);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "POST";
                request.ContentType = "application/json";

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    string json = "";
                    json += "{\"companyUsername\":\"" + this.CompanyUsername + "\",";
                    json += "\"companyPassword\":\"" + this.CompanyPassword + "\"}";

                    streamWriter.Write(json);
                }

                return request;
            }
            catch (Exception e)
            {
                string error = e.Message;
                return null;
            }

        }

        private string GetResponse(HttpWebRequest request)
        {
            string httpResponse = "";

            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    //return json
                    isResponseOK = true;
                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        httpResponse = streamReader.ReadToEnd();
                        return httpResponse;
                    }
                }
                else
                {
                    //return error string
                    isResponseOK = false;
                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        httpResponse = streamReader.ReadToEnd();
                    }

                    httpError = httpResponse.Substring(10);
                    return httpError;
                }
            }
            //(this will catch if the api returns a 400 and it does that a whole lot)
            catch (WebException wex)
            {
                //return error message
                isResponseOK = false;
                if (wex.Response != null)
                {
                    using (var errorResponse = (HttpWebResponse)wex.Response)
                    {
                        using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                        {
                            httpError = reader.ReadToEnd().Substring(10);
                            return httpError;
                        }
                    }
                }
                else
                {
                    httpError = "Server offline.";
                    return httpError;
                }
            }
        }

        #endregion Login
    }
}
