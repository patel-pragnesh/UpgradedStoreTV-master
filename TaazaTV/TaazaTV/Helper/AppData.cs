using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaazaTV.Helper
{
    class AppData
    {
        private static ISettings AppSettings =>  CrossSettings.Current;

        public static string UserName
        {
            get => AppSettings.GetValueOrDefault(nameof(UserName), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(UserName), value);
        }

        public static string MerchantUserName
        {
            get => AppSettings.GetValueOrDefault(nameof(MerchantUserName), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(MerchantUserName), value);
        }

        public static string UserReferralCode
        {
            get => AppSettings.GetValueOrDefault(nameof(UserReferralCode), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(UserReferralCode), value);
        }

        public static string MerchantUserId
        {
            get => AppSettings.GetValueOrDefault(nameof(MerchantUserId), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(MerchantUserId), value);
        }

        public static string UserId
        {
            get => AppSettings.GetValueOrDefault(nameof(UserId), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(UserId), value);
        }

        public static string MobileNo
        { 
            get => AppSettings.GetValueOrDefault(nameof(MobileNo), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(MobileNo), value);
        }

        public static string UserCity
        {
            get => AppSettings.GetValueOrDefault(nameof(UserCity), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(UserCity), value);
        }

        public static string UserCityId
        {
            get => AppSettings.GetValueOrDefault(nameof(UserCityId), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(UserCityId), value);
        }

        public static bool IsLogin
        {
            get => AppSettings.GetValueOrDefault(nameof(IsLogin), false);
            set => AppSettings.AddOrUpdateValue(nameof(IsLogin), value);
        }

        public static bool PickerProblem
        {
            get => AppSettings.GetValueOrDefault(nameof(PickerProblem), false);
            set => AppSettings.AddOrUpdateValue(nameof(PickerProblem), value);
        }

        public static bool IsMerchantLogin
        {
            get => AppSettings.GetValueOrDefault(nameof(IsMerchantLogin), false);
            set => AppSettings.AddOrUpdateValue(nameof(IsMerchantLogin), value);
        }
        
        public static string Avatar
        {
            get => AppSettings.GetValueOrDefault(nameof(Avatar), "logo.png");
            set => AppSettings.AddOrUpdateValue(nameof(Avatar), value);
        }

        public static string DeviceToken
        {
            get => AppSettings.GetValueOrDefault(nameof(DeviceToken), "1234567890");
            set => AppSettings.AddOrUpdateValue(nameof(DeviceToken), value);
        }

        public static bool IsSkip
        {
            get => AppSettings.GetValueOrDefault(nameof(IsSkip), false);
            set => AppSettings.AddOrUpdateValue(nameof(IsSkip), value);
        }

        //public static bool IsPreniumAdOpen
        //{
        //    get => AppSettings.GetValueOrDefault(nameof(IsPreniumAdOpen), false);
        //    set => AppSettings.AddOrUpdateValue(nameof(IsPreniumAdOpen), value);
        //}

        public static string TaazaCash
        {
            get => AppSettings.GetValueOrDefault(nameof(TaazaCash), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(TaazaCash), value);
        }

        public static string CartCount
        {
            get => AppSettings.GetValueOrDefault(nameof(CartCount), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(CartCount), value);
        }
    }
}
