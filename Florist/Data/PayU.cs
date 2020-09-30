using System;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.IO;
using System.Text;
using Florist.Models.ViewModel;

namespace Florist.Data
{
    public class PayU
    {
        public static async Task<string> GetAccessToken()
        {
            var baseAddress = new Uri("https://secure.snd.payu.com/");

            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {


                using (var content = new StringContent("grant_type=client_credentials&client_id=395609&client_secret=1c1d2e6551384b4e6e9c58c412746a24", System.Text.Encoding.Default, "application/x-www-form-urlencoded"))
                {
                    using (var response = await httpClient.PostAsync("pl/standard/user/oauth/authorize", content))
                    {
                        string responseData = await response.Content.ReadAsStringAsync();
                        return responseData;
                    }
                }
            }
        }

        public static async Task<string> CreateNewOrder(string accessToken, string tokenType, OrderDetailsCard detailsCard, string ip)
        {
            StringBuilder sb = new StringBuilder();
            foreach(var item in detailsCard.listCart)
            {
                sb.Append("{\"name\":");
                sb.Append("\"" + item.Flower.Name + "\",  ");
                sb.Append("\"unitPrice\":");
                sb.Append("\"" + item.Flower.Price * 100 + "\",  ");
                sb.Append("\"quantity\":");
                sb.Append("\"" + item.Count + "\"  },");
            }
            sb.Length--;
            string products = sb.ToString();
            
            var baseAddress = new Uri("https://secure.snd.payu.com/");

            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {


                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("authorization", tokenType + " " + accessToken);

                using (var content = new StringContent(
                    "{  \"notifyUrl\": \"https://florist20200914221430.azurewebsites.net/Customer/Cart/Summary\", " +
                    " \"customerIp\": \"" + ip + "\",  " +
                    "\"merchantPosId\": \"395609\",  " +
                    "\"description\": \"Florist shop\",  " +
                    "\"currencyCode\": \"PLN\",  " +
                    " \"extOrderId\": \"" + detailsCard.OrderHeader.Id.ToString() + "\",  " +
                    "\"totalAmount\": \"" + (detailsCard.OrderHeader.OrderTotal * 100).ToString() + "\",  " +
                    " \"continueUrl\": \"https://florist20200914221430.azurewebsites.net/Customer/Order/Confirm/"+detailsCard.OrderHeader.Id+"\",  " +
                    "\"buyer\": " +
                    "{" +
                    "\"email\": \"" + detailsCard.OrderHeader.ApplicationUser.Email + "\"," +
                    "\"phone\": \"" + detailsCard.OrderHeader.PhoneNumber + "\"," +
                    "\"firstName\": \"" + detailsCard.OrderHeader.ApplicationUser.FirstName + "\"," +
                    "\"lastName\": \"" + detailsCard.OrderHeader.ApplicationUser.LastName + "\"" +
                    "}," +
                    "\"products\": " +
                    "[" + products + "]}", System.Text.Encoding.Default, "application/json"))
                {
                    using (var response = await httpClient.PostAsync("api/v2_1/orders/", content))
                    {
                        string Url = response.RequestMessage.RequestUri.ToString();
                        return Url;
                    }
                }
            }
        }

        //second method but not finished
        public static string CreateNewOrder(string accessToken, string tokenType)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://secure.snd.payu.com/api/v2_1/orders/");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Accept = "application/json";
            httpWebRequest.AllowAutoRedirect = false;
            httpWebRequest.Method = "POST";
            httpWebRequest.Headers.Add(HttpRequestHeader.Authorization, tokenType + " " + accessToken);

            string stringData = "{  \"notifyUrl\": \"https://florist20200914221430.azurewebsites.net/Customer/Cart/Summary\",  \"customerIp\": \"127.0.0.1\",  \"merchantPosId\": \"395609\",  \"description\": \"RTV market\",  \"currencyCode\": \"PLN\",  \"totalAmount\": \"12345\",  \"products\": [    {      \"name\": \"Wireless mouse\",      \"unitPrice\": \"12345\",      \"quantity\": \"1\"    },    {      \"name\": \"HDMI cable\",      \"unitPrice\": \"12345\",      \"quantity\": \"1\"    }  ]}"; 

            byte[] requestBytes = new UTF8Encoding().GetBytes(stringData);
            Stream requestStream = httpWebRequest.GetRequestStream();
            requestStream.Write(requestBytes, 0, requestBytes.Length);

            WebResponse response;
            try
            {
                response = httpWebRequest.GetResponse();
                return null;
            }
            catch (WebException e)  
            {
                if (e.Message.Contains("302"))
                {
                    response = e.Response;
                    using (Stream stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                        string responseString = reader.ReadToEnd();
                        return responseString;
                    }
                }    
            }
            return null;
        }

    }
}
