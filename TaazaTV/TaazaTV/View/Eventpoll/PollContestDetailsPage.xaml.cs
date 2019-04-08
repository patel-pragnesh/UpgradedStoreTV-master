using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaazaTV.Helper;
using TaazaTV.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XLabs.Forms.Controls;

namespace TaazaTV.View.Eventpoll
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PollContestDetailsPage : ContentPage
    {
        PollContestModel Items = new PollContestModel();
        string Poll_Id = "";
        static bool IsLoad = true;
        int SelectedIndex =-1;
        public PollContestDetailsPage(string PollId, string PollTitle, ImageSource Image, string Type)
        {
            InitializeComponent();
            if (Device.RuntimePlatform == Device.iOS)
                this.Title = "Taaza TV Contests";
            TitleText.Text = PollTitle;
            TitleImage.Source = Image;
            Poll_Id = PollId;
            IsLoad = true;
        }

        protected override async void OnAppearing()
        {
           SelectedIndex = -1;
            //MainFrame.IsVisible = false;            
            if (IsLoad)
            {
                loadListdata(Poll_Id);
                IsLoad = false;
                //PollOptions.SelectedIndex = SelectedIndex;
            }
            base.OnAppearing();
        }

        private async void loadListdata(string PollId)
        {
           // SelectedIndex = -1;
            NoInternet.IsVisible = false;
            Loader.IsVisible = true;
            MainFrames.IsVisible = false;
            await Task.Delay(1000);

            try
            {
                HttpRequestWrapper wrapper = new HttpRequestWrapper();
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
                parameters.Add(new KeyValuePair<string, string>("company_code", Constant.CompanyID));
                parameters.Add(new KeyValuePair<string, string>("poll_id", PollId));
                parameters.Add(new KeyValuePair<string, string>("user_id", AppData.UserId));
                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.PollContestQuestionDetails], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {
                    NoInternet.IsVisible = true;
                    MainFrames.IsVisible = false;
                    //header.IsVisible = false;
                    //bar.IsVisible = false;
                    //TitleText.IsVisible=false;
                    //TitleImage.IsVisible=false;
                    //LineBar.IsVisible = false;
                    //Anslbl.IsVisible = false;
                    //qlbl.IsVisible = false;
                    //Submit.IsVisible = false;
                    //header.IsVisible = false;
                    bar.IsVisible = false;
                }
                else
                {
                    try
                    {
                        Items = JsonConvert.DeserializeObject<PollContestModel>(jsonstr);
                    }
                    catch
                    {
                        NoInternet.IsVisible = false;
                        MainFrames.IsVisible = false;
                        bar.IsVisible = false;
                    }
                 
                    if (Items.data.poll_details == null)
                    {
                        MainFrames.IsVisible = false;
                        //header.IsVisible = false;
                        //bar.IsVisible = false;
                        //TitleText.IsVisible = false;
                        //TitleImage.IsVisible = false;
                        //LineBar.IsVisible = false;
                        //qlbl.IsVisible = false;
                        //Anslbl.IsVisible = false;
                        //Submit.IsVisible = false;
                        //header.IsVisible = false;
                        bar.IsVisible = false;
                    }
                    BindingContext = Items.data.poll_details;
                    if (!Items.data.poll_details.is_Enabled)
                    {
                        Submit.Text = Items.data.poll_details.type_text + " submitted";
                        IsAnswer.Text = "Your Answer is:";
                        PollOptions.ItemsSource = Items.data.poll_details.poll_options.Where(y => y.Is_Selected).Select(x => x.option_value).ToList();
                        PollOptions.SelectedIndex = 1;
                        PollOptions.SelectedIndex = 0;
                    }
                    else
                    {
                        IsAnswer.Text = "Your Options is:";
                        PollOptions.ItemsSource = Items.data.poll_details.poll_options.Select(x => x.option_value).ToList();
                    }

                }
            }
            catch (Exception ex)
            {
            }

            Loader.IsVisible = false;
            MainFrames.IsVisible = true;
            header.IsVisible = true;
            bar.IsVisible = true;
            //TitleText.IsVisible = true;
            //TitleImage.IsVisible = true;
            //LineBar.IsVisible = true;
            //Anslbl.IsVisible = true;
            //qlbl.IsVisible = true;
            //Submit.IsVisible = true;
        }

        private void lstView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Poll_Options option = (Poll_Options)e.Item;

            foreach (var item in Items.data.poll_details.poll_options.Where(x => x.Is_Selected == true))
            {
                item.Is_Selected = false;
            }

            option.Is_Selected = true;

        }

        private void DoSomething(object sender, EventArgs e)
        {
            loadListdata(Poll_Id);
        }

        private async void NoDataDoSomething(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }


        private async void SubmitButtonClicked(object sender, EventArgs e)
        {
            MainFrames.IsVisible = false;
            Loader.IsVisible = true;
            await Task.Delay(1000);

            try
            {
                HttpRequestWrapper wrapper = new HttpRequestWrapper();
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
                parameters.Add(new KeyValuePair<string, string>("company_code", Constant.CompanyID));
                parameters.Add(new KeyValuePair<string, string>("poll_id", Poll_Id));
                if (SelectedIndex>=0)
                {
                    parameters.Add(new KeyValuePair<string, string>("poll_option_id", Items.data.poll_details.poll_options[SelectedIndex].poll_option_id.ToString()));
                }
                else
                {
                    await DisplayAlert("Error", "Blank option not submitted!", "OK");
                }
                parameters.Add(new KeyValuePair<string, string>("user_id", AppData.UserId));
                var jsonstr = await wrapper.GetResponseAsync(Constant.APIs[(int)Constant.APIName.PollContestAnswarSubmit], parameters);
                if (jsonstr.ToString() == "NoInternet")
                {
                    NoInternet.IsVisible = true;
                    MainFrames.IsVisible = false;
                    header.IsVisible = false;
                    bar.IsVisible = false;
                }
                else
                {
                    var result = JsonConvert.DeserializeObject<SubmitContestOrPoll>(jsonstr);

                    if (result.responseText == "Success")
                    {
                        await DisplayAlert("Success", "Your answer has been submitted!", "OK");
                        await Task.Delay(1000);
                        loadListdata(Poll_Id);
                    }
                    else
                    {
                        await DisplayAlert("Error", "Your answer not submitted!", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
            }
            MainFrames.IsVisible = true;
            Loader.IsVisible = false;
            header.IsVisible = true;
            bar.IsVisible = true;
        }

        private void PollOptions_CheckedChanged(object sender, int e)
        {

            SelectedIndex = e;

        }
    }
}