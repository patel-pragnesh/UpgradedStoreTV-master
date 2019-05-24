﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaazaTV.Helper;
using TaazaTV.Model.TaazaStoreModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaazaTV.View.TaazaStore
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewAddressPage : ContentPage
    {
        HttpRequestWrapper wrapper = new HttpRequestWrapper();
        Address AddressModel = new Address();
        string cust_addr_id, jsonstr;
        public NewAddressPage(string AddrID)
        {
            InitializeComponent();
            cust_addr_id = AddrID;
            GetStatesList();
            if (!String.IsNullOrEmpty(cust_addr_id))
            {
                UserAddressDetailsView(cust_addr_id); //Set Something to distinguish as upadte
            }
            else
            {
                this.BindingContext = AddressModel;
            }
        }

        private async void UserAddressDetailsView(string addr_id)
        {
            try
            {
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("customer_address_id", addr_id),
                };

                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.UserAddressDetailsViewAPI], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {
                }
                else
                {
                    var Item = JsonConvert.DeserializeObject<StoreUserAddressModel>(jsonstr);
                    AddressModel = Item.data.address;
                    this.BindingContext = AddressModel;
                    AddressType.Items[AddressModel.type - 1].Checked = true;
                }
            }
            catch (Exception ex)
            {
                var x = ex.Message;
            }
        }

        private async void GetStatesList()
        {
            try
            {
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();

                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.GetStateListAPI], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {
                }
                else
                {
                    var Items = JsonConvert.DeserializeObject<AddressStateModel>(jsonstr);
                }
            }
            catch (Exception ex)
            {
                var x = ex.Message;
            }
        }

        private async void SubmitNewAddress(object sender, EventArgs e)
        {
            var SelectedType = AddressType.Items.Where(z => z.Checked == true).FirstOrDefault();
          
            if (!String.IsNullOrEmpty(AddressModel.full_name) 
                && !String.IsNullOrEmpty(AddressModel.address_1)
                && !String.IsNullOrEmpty(AddressModel.pincode.ToString())
                && !String.IsNullOrEmpty(AddressModel.phone)
                && !String.IsNullOrEmpty(AddressModel.city_name)
                && !String.IsNullOrEmpty(AddressModel.state_name)
                && AddressModel.pincode.ToString().Count() == 6
                && AddressModel.phone.Count() == 10
                && !String.IsNullOrEmpty(AddressModel.address_2)
                && SelectedType != null
                )
            {
                AddressModel.type = Convert.ToInt32(SelectedType.Id) + 1;
                try
                {
                    List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("user_id", AppData.UserId),
                    new KeyValuePair<string, string>("full_name", AddressModel.full_name),
                    new KeyValuePair<string, string>("phone", AddressModel.phone),
                    new KeyValuePair<string, string>("address_1", AddressModel.address_1),
                    new KeyValuePair<string, string>("address_2", AddressModel.address_2),
                    new KeyValuePair<string, string>("address_3", AddressModel.address_3),
                    new KeyValuePair<string, string>("pincode", AddressModel.pincode.ToString()),
                    new KeyValuePair<string, string>("city_name", AddressModel.city_name),
                    new KeyValuePair<string, string>("state_name", AddressModel.state_name),
                    new KeyValuePair<string, string>("type", AddressModel.type.ToString()),
                };

                    if(!string.IsNullOrEmpty(cust_addr_id))
                    {
                        parameters.Add(new KeyValuePair<string, string>("customer_address_id", cust_addr_id));
                        jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.UpdateUserAddressAPI], parameters);
                    }
                    else
                    {
                        jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.AddUserNewAddressAPI], parameters);
                    }
                  

                    if (jsonstr.ToString() == "NoInternet")
                    {
                    }

                    else
                    {
                        var Items = JsonConvert.DeserializeObject<SuccessResponseModel>(jsonstr);
                        if (Items.responseText == "Success")
                            await Navigation.PopAsync();
                    }
                }
                catch (Exception ex)
                {
                }
            }

            else
            {
                //Show all fields are mandatory!!
            }
           
        }

    }
}