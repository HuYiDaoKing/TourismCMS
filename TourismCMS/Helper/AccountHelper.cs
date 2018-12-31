using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TourismCMS.Helper
{
    public static class AccountHelper
    {
        public static string FlashLoginErrorMessage(TempDataDictionary tmpData)
        {
            if (tmpData[AccountHashKeys.CaptchaErrorMessage] != null)
            {
                return Resources.AccountResource.CaptchaErrorMessage;
            }

            if (tmpData[AccountHashKeys.LoginModelStateErrorMessage] != null)
            {
                return Resources.AccountResource.LoginModelStateErrorMessage;
            }

            if (tmpData[AccountHashKeys.EmailOrPasswordErrorMessage] != null)
            {
                return Resources.AccountResource.EmailOrPasswordErrorMessage;
            }

            if (tmpData[AccountHashKeys.RegisterFailure] != null)
            {
                return Resources.AccountResource.RegisterFailure;
            }

            if (tmpData[AccountHashKeys.RegisterSuccess] != null)
            {
                return Resources.AccountResource.RegisterSuccess;
            }

            if (tmpData[AccountHashKeys.PasswordConflict] != null)
            {
                return Resources.AccountResource.PasswordConflict;
            }

            if (tmpData[AccountHashKeys.DuplicateRegister] != null)
            {
                return Resources.AccountResource.DuplicateRegister;
            }

            return null;
        }
    }

    public static class AccountHashKeys
    {
        // Login view model key.
        public static readonly string CaptchaErrorMessage = "CAPTCHA_ERROR";

        public static readonly string EmailOrPasswordErrorMessage = "EMAIL_OR_PASSWORD_ERROR";
        public static readonly string LoginModelStateErrorMessage = "MODEL_STATE_ERROR";

        // Cookie key.
        public static readonly string UserBrowserCookie = "USER_BROWSER_COOKIE";

        public static readonly string AdminUserBrowserCookie = "ADMIN_USER_BROWSER_COOKIE";

        public static readonly string RegisterSuccess = "RRGISTER_SUCCESS";
        public static readonly string RegisterFailure = "REGISTER_FAILURE!";
        public static readonly string PasswordConflict = "PASSWORD_CONFLICT!";
        public static readonly string DuplicateRegister = "DUPLICATE_REGISTER!";

        // Current user key.
        public static readonly string CurrentAccountUser = "CURRENT_ACCOUNT_USER";

        public static readonly string CurrentMerchantUser = "CURRENT_MERCHANT_USER";
        public static readonly string CurrentAdminUser = "CURRENT_ADMIN_USER";
    }
}