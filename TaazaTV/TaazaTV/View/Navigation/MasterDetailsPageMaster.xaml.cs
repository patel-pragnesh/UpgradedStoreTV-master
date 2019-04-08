using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TaazaTV.Helper;
using TaazaTV.View.Accounts;
using TaazaTV.View.News;
using TaazaTV.View.Tools;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaazaTV.View.Navigation
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterDetailsPageMaster : ContentPage
    {
        public ListView ListView;

        public MasterDetailsPageMaster()
        {
            InitializeComponent();
            Icon = "menu.png";
            BindingContext = new MasterDetailsPageMasterViewModel();
            ListView = MenuItemsListView;
            if(AppData.UserId=="")
            {
                ulogin.IsVisible = true;

            }
            UserName.Text = AppData.UserName;
            if(AppData.UserName=="")
            {
                uRegister.IsVisible = true;
            }
            Avatar.Source = AppData.Avatar;

            
        }

        class MasterDetailsPageMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<MasterDetailsPageMenuItem> MenuItems { get; set; }

            public MasterDetailsPageMasterViewModel()
            {
                if (AppData.UserId == "")
                {
                    MenuItems = new ObservableCollection<MasterDetailsPageMenuItem>(new[]
                   {
                    //new MasterDetailsPageMenuItem { Id = 0, Icon="myprofile.png",  Title = "My Profile", TargetType=typeof(MyProfilePage) },
                    new MasterDetailsPageMenuItem { Id = 1, Icon="privacypolicy.png",  Title = "Privacy Policy", TargetType=typeof(PrivacyPolicyPage) },
                    new MasterDetailsPageMenuItem { Id = 3, Icon="terms.png",  Title = "Terms & Conditions", TargetType=typeof(TermsPage) },
                    //new MasterDetailsPageMenuItem { Id = 2, Icon="submission.png",  Title = "My Submission", TargetType=typeof(SubmitedNewsPage) }
                });
                }
                else
                {
                    MenuItems = new ObservableCollection<MasterDetailsPageMenuItem>(new[]
                    {
                    new MasterDetailsPageMenuItem { Id = 0, Icon="myprofile.png",  Title = "My Profile", TargetType=typeof(MyProfilePage) },
                    new MasterDetailsPageMenuItem { Id = 1, Icon="privacypolicy.png",  Title = "Privacy Policy", TargetType=typeof(PrivacyPolicyPage) },
                    new MasterDetailsPageMenuItem { Id = 3, Icon="terms.png",  Title = "Terms & Conditions", TargetType=typeof(TermsPage) },
                    new MasterDetailsPageMenuItem { Id = 2, Icon="submission.png",  Title = "My Submission", TargetType=typeof(SubmitedNewsPage) },
                    new MasterDetailsPageMenuItem { Id = 4, Icon="terms.png",  Title = "TaazaCash Privacy Policy", TargetType=typeof(TaazaCashTerms) }
                });
                }
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }

        private async void OnMenuItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            //var item = (MasterDetailsPageMenuItem)e.SelectedItem;
            // Type page = item.TargetType;

            //await Navigation.PushAsync((Page)Activator.CreateInstance(page));
            //await Navigation.PushAsync(new ProfilePage());
        }

        private void Logout_Tapped(object sender, EventArgs e)
        {
           // AppData.IsLogin = false;
           // App.Current.MainPage = new LoginPage();
        }
        
        private async void Login_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NewLoginPage());
        }

        private async void Register_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new RegistrationPage());
        }
    }
}