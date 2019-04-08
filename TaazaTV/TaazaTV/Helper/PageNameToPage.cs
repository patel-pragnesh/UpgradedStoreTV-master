using System;
using System.Collections.Generic;
using System.Text;
using TaazaTV.View.Eventpoll;
using TaazaTV.View.News;
using TaazaTV.View.Accounts;
using TaazaTV.View.Social;
using Xamarin.Forms;
using TaazaTV.View.Tools;
using TaazaTV.View.TaazaCash;

namespace TaazaTV.Helper
{
    class PageNameToPage
    {
        public static Page GetPage(string PageName, string Name, ImageSource Image)
        {
            switch (PageName)
            {
                case "breaking_news":
                    return new BreakingNewsPage(Name, Image);
                    break;
                case "news":
                    return new NewsCategoryPage(Name, Image);
                    break;
                case "live_tv":
                    return new NewVideoViewPage();
                        //VideoViewPage("http://www.zengatv.com/embed?v=5d9eebd0-313d-11e1-8f87-1231400424bd.html&t=live");
                    //http://www.zengatv.com/embed?v=5d9eebd0-313d-11e1-8f87-1231400424bd.html&t=live
                    break;
                case "contests":
                    if (AppData.UserId == "")
                    {
                        return new RegistrationPage();
                    }
                    else
                    {
                        return new PollContestListPage(Name, Image, "2");
                    }
                    break;
                case "social_network":
                    if (AppData.UserId == "")
                    {
                        return new RegistrationPage();
                    }
                    else
                    {
                        return new SocialNewsPage(Name, Image);
                    }
                    break;
                case "events":
                  
                        return new EventListPage(string.Empty, Name, Image);

                    break;
                case "shows":
                    return new ShowsPage(Name, Image);
                    break;
                case "opinion_poll":
                    if (AppData.UserId == "")
                    {
                        return new RegistrationPage();
                    }
                    else
                    {
                        return new PollContestListPage(Name, Image, "1");
                    }

                    break;

                case "citizen_journalist":
                    if (AppData.UserId == "")
                    {
                        return new RegistrationPage();
                    }
                    else
                    {
                        return new NewsSubmitPage();
                    }

                case "restaurant":

                    return new RestaurantListPage();

                case "taazadekho":

                    if (AppData.UserId == "")
                    {
                        return new RegistrationPage();
                    }
                    else
                    {
                        return new TaazaDekhoPage();
                    }

                default:
                    return new NewsCategoryPage(Name, Image);
                    break;
            }
        }

        private static void DisplayAlert(string v1, string v2, string v3)
        {
            throw new NotImplementedException();
        }
    }
}
