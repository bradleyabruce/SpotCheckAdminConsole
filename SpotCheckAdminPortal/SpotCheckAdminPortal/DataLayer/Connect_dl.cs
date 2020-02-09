using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;

namespace SpotCheckAdminPortal.DataLayer
{
   public static class Connect_dl
   {

      //lets use some reflection to get crazy in here
      public static string BuildJson(Object o, bool removeDateProperties = false)
      {
         string json = "{";
         List<Dictionary<string, object>> objectPropertyNameValuePairs = new List<Dictionary<string, object>>();

         foreach (PropertyInfo propertyInfo in o.GetType().GetProperties())
         {
            string propertyName = propertyInfo.Name;
            object propertyValue = propertyInfo.GetValue(o);
            if (propertyValue != null)
            {
               if(CheckDate(propertyValue) && removeDateProperties)  //remove dates if needed
               {
                  continue;
               }

               Dictionary<string, object> nameValuePair = new Dictionary<string, object>();
               propertyName = propertyName.Substring(0, 1).ToLower() + propertyName.Substring(1);
               nameValuePair.Add(propertyName, propertyValue);
               objectPropertyNameValuePairs.Add(nameValuePair);
            }
         }

         foreach (Dictionary<string, object> nameValuePair in objectPropertyNameValuePairs)
         {
            json += "\"" + nameValuePair.FirstOrDefault().Key + "\":\"" + nameValuePair.FirstOrDefault().Value + "\",";
         }

         json = json.Substring(0, json.Length - 1);
         json += "}";

         return json;
      }

      public static HttpWebRequest BuildRequest(string url, string method, string json)
      {
         try
         {
            Uri uri = new Uri(url);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = method;
            request.ContentType = "application/json";

            using (var streamWriter = new StreamWriter(request.GetRequestStream())) { streamWriter.Write(json); }
            return request;
         }
         catch (Exception e)
         {
            string error = e.Message;
            return null;
         }
      }

      public static Dictionary<HttpStatusCode, string> GetResponse(HttpWebRequest request)
      {
         string httpResponse;
         string httpError;

         Dictionary<HttpStatusCode, string> result = new Dictionary<HttpStatusCode, string>();

         try
         {
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
               //return json string
               using (var streamReader = new StreamReader(response.GetResponseStream()))
               {
                  httpResponse = streamReader.ReadToEnd();
                  result.Add(response.StatusCode, httpResponse);
                  return result;
               }
            }
            else
            {
               //return error string
               using (var streamReader = new StreamReader(response.GetResponseStream()))
               {
                  httpResponse = streamReader.ReadToEnd();
               }

               httpError = httpResponse.Substring(10);
               result.Add(response.StatusCode, httpError);
               return result;
            }
         }
         //(this will catch if the api returns a 400 and it does that a whole lot)
         catch (WebException wex)
         {
            //return error string
            if (wex.Response != null)
            {
               using (var errorResponse = (HttpWebResponse)wex.Response)
               {
                  using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                  {
                     httpError = reader.ReadToEnd().Substring(10);
                     result.Add(errorResponse.StatusCode, httpError);
                     return result;
                  }
               }
            }
            //server just doesnt feel like working today
            else
            {
               httpError = "Server offline.";
               result.Add(HttpStatusCode.InternalServerError, httpError);
               return result;
            }
         }
      }

      private static bool CheckDate(object o)
      {
         if(o.GetType() == typeof(DateTime))
         {
            return true;
         }
         else
         {
            return false;
         }
      }
   }
}
