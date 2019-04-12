using Xamarin.Forms;

namespace TaazaTV.Helper
{
    class Constant
    {
        //staging URL
        private static string HostName = "http://dev2.maxmobility.in/taazatv/public/api/V2";
        public static string ServerURL = "http://dev2.maxmobility.in/taazatv/public";

        //Live URL 
      //  private static string HostName = "http://52.221.138.138/taazatv/public/api/V2";
      //  public static string ServerURL = "http://52.221.138.138/taazatv/public";

        //Live URL 
    //    private static string HostName = "http://api.taazatv.com/public/api/V2";
    //    public static string ServerURL = "http://api.taazatv.com/public";

        #region Public Constant        
        public static string CompanyID = "COM1000";
        public static string DeviceType = Device.RuntimePlatform;
        public static string DeviceToken = AppData.DeviceToken;
        public static string AppVersion = "1.6";
        #endregion

        public static string[] APIs = new string[]
        {
            HostName + "/user/login",
            HostName + "/user/registration",
            HostName + "/location/citys",
            HostName + "/user/otp-validate",
            HostName + "/user/profile",
            HostName + "/user/profile-update",
            HostName + "/news/details",
            HostName + "/news/list",
            HostName + "/news/list/user",
            HostName + "/news/submit",
            HostName + "/news/category",
            HostName + "/dashboard",
            HostName + "/dashboard/unread-data-count",
            HostName + "/user/change-password",
            HostName + "/user/forgot-password-sentotp-one",
            HostName + "/user/forgot-password-checkotp-two",
            HostName + "/user/forgot-update-password-three",
            HostName + "/news/category",
            HostName + "/youtube/playlists",
            HostName + "/youtube/playlist/items",
            HostName + "/youtube/videos",
            HostName + "/poll-or-contest/question-list",
            HostName + "/poll-or-contest/question-details",
            HostName + "/poll-or-contest/submit",
            HostName + "/event/list",
            HostName + "/event/details",
            HostName + "/group/list/user",
            HostName + "/group/feed",
            HostName + "/user/social-login-registration",
            HostName + "/user/update-device-token",
            HostName + "/user/resend-otp",
            HostName + "/user/check-phone-no",

            // Taazat Dekho Lakho Jeeto second Phase

            HostName + "/ad/premium/list",
            HostName + "/ad/general/list",
            HostName + "/poll-or-contest/taazadekho-question",
            HostName + "/poll-or-contest/taazadekho-submit",
            HostName + "/event/book-event",
            HostName + "/restaurant/book",
            HostName + "/restaurant/list",
            HostName + "/restaurant/details",
            HostName + "/user/transactions/history",
            HostName + "/booking/list",
            HostName + "/user/merchant-login",
            HostName + "/user/merchant-ticket-verivication",
            HostName + "/event/location-list",
            HostName + "/restaurant/location-list",
            HostName + "/event-booking/booking-worldline?event_id=",
            HostName + "/restaurant-booking/booking-worldline?restaurant_id=",

            // Taazat Store Final Phase

             HostName + "/product/category/list",
             HostName + "/product/item/list",
             HostName + "/product/item/details",
             HostName + "/product/filter-option/list",
             HostName + "/customer/customer-address-list",
             HostName + "/customer/customer-address-all-state",
             HostName + "/customer/customer-address-set-default",
             HostName + "/customer/customer-address-add",
             HostName + "/customer/customer-address-edit-data",
             HostName + "/customer/customer-address-do-edit",
             HostName + "/customer/customer-address-delete",
             HostName + "/info/get-seller-list",
             HostName + "/info/get-seller-details",
             HostName + "/restaurant-transaction/otp-generate",
             HostName + "/cart/history",

        };


        public enum APIName : int
        {
            Login,
            Registration,
            Cities,
            OTP,
            Profile,
            ProfileUpdate,
            NewsDetails,
            NewsList,
            UserwiseNewsList,
            NewsPost,
            Category,
            Dashboard,
            DashboardUnreadDataCount,
            ChangePassword,
            ForgotPasswordSentOTP,
            ForgotPasswordCheckOTP,
            ForgotPasswordUpdatePassword,
            NewsCategory,
            ShowsPlaylist,
            ShowsPlaylistVideos,
            ShowsTopVideos,
            PollContestQuestionList,
            PollContestQuestionDetails,
            PollContestAnswarSubmit,
            Eventlist,
            EventDetails,
            GroupList,
            FeedList,
            socialLogin,
            UpdateDeviceToken,
            ResendOTP,
            NewLogincheck,

            // Taazat Dekho Lakho Jeeto second Phase

            PreniumUrl,
            GeneralUrl,
            TaazaDekhoUrl,
            TaazaDekhoSubmitUrl,
            BookEvent,
            BookRestaurant,
            RestaurantList,
            RestaurantDetails,
            TaazaTransactions,
            AllTransactionsAPI,
            MerchantLoginCheck,
            MerchantCodeVerification,
            EventLocationList,
            RestaurantLoactionList,
            PaymentEventGateway,
            PaymentRestGateway,

            // Taazat Store Final Phase

            FirstLevelCategoryListAPI,
            GeneralProductListAPI,
            ProductDetailsAPI,
            FilterOptionsAPI,
            UserAddressListAPI,
            GetStateListAPI,
            SetDefaultAddressAPI,
            AddUserNewAddressAPI,
            UserAddressDetailsViewAPI,
            UpdateUserAddressAPI,
            DeleteUserAddressAPI,
            GetSellerListAPI,
            GetSellerDetailsAPI,
            GetOfflinePayOTPAPI,
            GetCartHistoryAPI,
        }
    }
}
