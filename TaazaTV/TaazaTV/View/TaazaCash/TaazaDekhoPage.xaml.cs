using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaazaTV.Helper;
using TaazaTV.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaazaTV.View.TaazaCash
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TaazaDekhoPage : ContentPage
    {
        HttpRequestWrapper wrapper = new HttpRequestWrapper();
        public string Poll_Id { get; set; }
        public string Poll_OptionId { get; set; }
        int SelectedIndex = -1;
        TaazaDekhoModel Items = new TaazaDekhoModel();

        public TaazaDekhoPage()
        {
            InitializeComponent();
#if __IOS__
            //Set Padding For the top of the page
            this.Padding = new Thickness(0,20,0,0);
#endif
            TaazaCashAmount.Text = AppData.TaazaCash;
            LoadOptions();
        }

        private async void LoadOptions()
        {
            Loader.IsVisible = true;
            try
            {
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("company_code", Constant.CompanyID),
                    new KeyValuePair<string, string>("user_id", AppData.UserId)
                };

                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.TaazaDekhoUrl], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {
                    NoInternet.IsVisible = true;
                    MainFrames.IsVisible = false;
                    Loader.IsVisible = false;
                }
                else
                {
                    Items = JsonConvert.DeserializeObject<TaazaDekhoModel>(jsonstr);
                    if (Items.data.details.poll_options.Count() == 0)
                    {
                        MainFrames.IsVisible = false;
                        NodataPage.IsVisible = true;
                    }
                      
                    else
                    {
                        PollOptions.ItemsSource = Items.data.details.poll_options.Select(x => x.option_value).ToList();
                        Poll_Id = Items.data.details.poll_id.ToString();
                        MainFrames.IsVisible = true;

                        if (Items.data.details == null)
                        {
                            MainFrames.IsVisible = false;
                            NodataPage.IsVisible = true;
                        }

                        if (!Items.data.details.is_Enabled)
                        {
                            Submit.Text = Items.data.details.type_text + " submitted";
                            PollOptions.ItemsSource = Items.data.details.poll_options.Select(x => x.option_value).ToList();
                            int i = 0, j = 0;
                            foreach (var x in Items.data.details.poll_options)
                            {
                                if (x.Is_Selected)
                                {
                                    j = i;
                                }
                                i++;
                            }
                            PollOptions.SelectedIndex = j;
                            Submit.IsEnabled = false;
                        }
                        else
                        {
                            PollOptions.ItemsSource = Items.data.details.poll_options.Select(x => x.option_value).ToList();
                            PollOptions.SelectedIndex = -1;
                        }
                    }
                    Loader.IsVisible = false;
                }
                   
            }

            catch(Exception ex)
            {
                var a = ex.Message;
                MainFrames.IsVisible = false;
                Loader.IsVisible = false;
                NodataPage.IsVisible = true;
            }
        }
        private async void SubmitButtonClicked(object sender, EventArgs e)
        {
            MainFrames.IsVisible = false;
            Loader.IsVisible = true;
            await Task.Delay(200);

            try
            {
                HttpRequestWrapper wrapper = new HttpRequestWrapper();
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
                parameters.Add(new KeyValuePair<string, string>("company_code", Constant.CompanyID));
                parameters.Add(new KeyValuePair<string, string>("poll_id", Poll_Id));
                parameters.Add(new KeyValuePair<string, string>("user_id", AppData.UserId));
                if (PollOptions.SelectedIndex >= 0)
                {
                    parameters.Add(new KeyValuePair<string, string>("poll_option_id", Items.data.details.poll_options[SelectedIndex].poll_option_id.ToString()));
                    var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.TaazaDekhoSubmitUrl], parameters);

                    if (jsonstr.ToString() == "NoInternet")
                    {
                        NoInternet.IsVisible = true;
                        MainFrames.IsVisible = false;
                        header.IsVisible = false;
                    }
                    else
                    {
                        var result = JsonConvert.DeserializeObject<SubmitContestOrPollTazzaDekho>(jsonstr);

                        if (result.responseText == "Success")
                        {
                            if(result.data.is_submitted_correct_answer==1)
                            {
                                await DisplayAlert("Success", "You have submitted the right answer!", "OK");
                            }
                            else
                            {
                                await DisplayAlert("Failure", "You have submitted the wrong answer!", "OK");
                            }
                            //await DisplayAlert("Success", "Your answer has been submitted!", "OK");
                            Submit.IsEnabled = false;
                            AppData.TaazaCash = result.data.current_wallet_balance;
                        }
                        else
                        {
                            await DisplayAlert("Error", "Your answer not submitted!", "OK");
                        }
                    }
                }
                else
                {
                    await DisplayAlert("Error", "Blank option not submitted!", "OK");
                }

            }
            catch (Exception ex)
            {
                Loader.IsVisible = false;
                NodataPage.IsVisible = true;
            }
            MainFrames.IsVisible = true;
            Loader.IsVisible = false;
            header.IsVisible = true;
        }

        private void DoSomething(object sender, EventArgs e)
        {
            NoInternet.IsVisible = false;
            NodataPage.IsVisible = false;
            LoadOptions();
        }

        private void PollOptions_CheckedChanged(object sender, int e)
        {
            SelectedIndex = e;
        }

        private async void NavToTransactionDetails(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TaazaTransactionPage());
        }

        private async void BackBtn_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}