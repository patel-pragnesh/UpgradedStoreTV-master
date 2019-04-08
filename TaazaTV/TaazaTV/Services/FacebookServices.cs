using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TaazaTV.Model;
using Newtonsoft.Json;
using TaazaTV.Helper;
using Xamarin.Forms;

namespace TaazaTV.Services
{
    public class FacebookServices
    {
        public async Task<FacebookProfile> GetFacebookProfileAsync(string accessToken)
        {
            var requestUrl =
                "https://graph.facebook.com/v2.7/me/?fields=name,picture,location,email,first_name,last_name&access_token="
                + accessToken;

            var httpClient = new HttpClient();

            var userJson = await httpClient.GetStringAsync(requestUrl);

            var facebookProfile = JsonConvert.DeserializeObject<FacebookProfile>(userJson);

            HttpRequestWrapper wrapper = new HttpRequestWrapper();
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            // FacebookProfile FacebookProfile = new FacebookProfile();
            //  facebooklogin Facebooklogin = new facebooklogin();
            parameters.Add(new KeyValuePair<string, string>("company_code", Constant.CompanyID));
            parameters.Add(new KeyValuePair<string, string>("f_name", facebookProfile.FirstName));
            parameters.Add(new KeyValuePair<string, string>("l_name", facebookProfile.LastName));

            parameters.Add(new KeyValuePair<string, string>("phone_no", facebookProfile.PhoneNo));
            parameters.Add(new KeyValuePair<string, string>("email_id", facebookProfile.Emailid));
            parameters.Add(new KeyValuePair<string, string>("social_login_type", "1"));
            parameters.Add(new KeyValuePair<string, string>("social_user_id", facebookProfile.Id));

            parameters.Add(new KeyValuePair<string, string>("device_type", Constant.DeviceType));
            parameters.Add(new KeyValuePair<string, string>("device_token", AppData.DeviceToken));
            parameters.Add(new KeyValuePair<string, string>("app_version", Constant.AppVersion));

            try
            {
                string data = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.socialLogin], parameters);

                var des = JsonConvert.DeserializeObject<facebooklogin>(data);
                if (des.responseText == "Success")
                {
                    AppData.IsLogin = true;
                    AppData.UserName = des.data.user_data.name;
                    AppData.UserId = des.data.user_data.user_id;
                    AppData.UserCityId = des.data.user_data.city_id;
                    AppData.UserCity = des.data.user_data.city_name;
                    AppData.Avatar = facebookProfile.Picture.Data.Url;

                    // Update Device Token
                    Task.Run( async() =>
                    {
                        try
                        {
                            HttpRequestWrapper wra = new HttpRequestWrapper();
                            List<KeyValuePair<string, string>> paras = new List<KeyValuePair<string, string>>();

                            paras.Add(new KeyValuePair<string, string>("company_code", Constant.CompanyID));

                            paras.Add(new KeyValuePair<string, string>("user_id", AppData.UserId));

                            paras.Add(new KeyValuePair<string, string>("device_type", Constant.DeviceType));
                            paras.Add(new KeyValuePair<string, string>("device_token", AppData.DeviceToken));
                            paras.Add(new KeyValuePair<string, string>("app_version", Constant.AppVersion));

                            string result = await wra.GetResponseAsync(Constant.APIs[(int)Constant.APIName.UpdateDeviceToken], paras);
                        }
                        catch(Exception e) { }
                    });


                    App.Current.MainPage = new MasterDetailPage();
                }
            }
            catch (Exception ex)
            {

            }
            return facebookProfile;            
        }
    }
}
