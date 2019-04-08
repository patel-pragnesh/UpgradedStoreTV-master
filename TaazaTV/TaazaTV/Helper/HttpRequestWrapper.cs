using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http;

using System.IO;
using System.Linq;
using Plugin.Connectivity;

namespace TaazaTV.Helper
{
    class HttpRequestWrapper
    {


        public async Task<string> GetResponseAsync(string URL, string Method)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                string responseText = "";
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(
                        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


                    var response = await client.GetAsync(URL);

                    responseText = await response.Content.ReadAsStringAsync();
                }
                return responseText;
            }
            else
            {
                // write your code if there is no Internet available 
                return "NoInternet";
            }

        }

        public async Task<string> GetResponseAsync(string URL, IEnumerable<KeyValuePair<string, string>> postData)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    string responseText = "";

                    using (var httpClient = new HttpClient())
                    {
                        using (var content = new FormUrlEncodedContent(postData))
                        {
                            content.Headers.Clear();
                            content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                            HttpResponseMessage response = await httpClient.PostAsync(URL, content);

                            return await response.Content.ReadAsStringAsync();

                        }

                    }
                }
                else
                {
                    // write your code if there is no Internet available 
                    return "NoInternet";
                }

            }
            catch (Exception ex) { return "NoInternet"; }
        }

        public async Task<string> PostFormDataAsync(string URL, MultipartFormDataContent postData)
        {
            try
            {
                string responseText = "";
                if (CrossConnectivity.Current.IsConnected)
                {
                    using (var httpClient = new HttpClient())
                    {

                        using (postData)
                        {
                            HttpResponseMessage response = await httpClient.PostAsync(URL, postData);

                            return await response.Content.ReadAsStringAsync();
                        }
                    }
                }
                else
                {
                    // write your code if there is no Internet available 
                    return "NoInternet";
                }
            }
            catch
            {
                return "NoInternet";
            }
        }
    }



    public class HttpHeader
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
