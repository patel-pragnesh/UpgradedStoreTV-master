using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TaazaTV.Helper;
using TaazaTV.Model;

namespace TaazaTV.Services
{
    public static class CommonFunc
    {
        public static async Task<PreniumAdList[]> FooterAdFunc(string pagename)
        {
            try
            {
                MultipartFormDataContent formdata = new MultipartFormDataContent();
                formdata.Add(new StringContent(pagename), "page_name");
                formdata.Add(new StringContent(AppData.UserId), "user_id");
                HttpRequestWrapper wrapper = new HttpRequestWrapper();
                string items = await wrapper.PostFormDataAsync(Constant.APIs[(int)Constant.APIName.GeneralUrl], formdata);

                if (items.ToString() == "NoInternet")
                {
                    return null;
                }

                else
                {
                    var deobj = JsonConvert.DeserializeObject<PreniumAdModel>(items);
                    var footersrc = deobj.data.Ad_list.ToArray();
                    if (footersrc != null && footersrc.Count() > 0)
                    {
                        return footersrc;
                    }
                    else
                    {
                       return null;
                    }

                }
            }

            catch
            {
                throw new NotImplementedException();
            }
        }
    }
}
