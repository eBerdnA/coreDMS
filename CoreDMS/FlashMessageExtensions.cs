using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreDMS
{
    // https://10consulting.com/2011/08/29/asp-dot-net-mvc-flash-messages-with-razor-view-engine/
    internal static class FlashMessageExtensions
    {
        private enum Notification
        {
            Danger,
            Warning,
            Success,
            Info
        }

        public static ActionResult Danger(this ActionResult result, HttpResponse response, string message)
        {
            CreateCookieWithFlashMessage(Notification.Danger, message, response);
            return result;
        }

        public static ActionResult Information(this ActionResult result, HttpResponse response, string message)
        {
            CreateCookieWithFlashMessage(Notification.Info, message, response);
            return result;
        }

        public static ActionResult Warning(this ActionResult result, HttpResponse response, string message)
        {
            CreateCookieWithFlashMessage(Notification.Warning, message, response);
            return result;
        }

        public static ActionResult Success(this ActionResult result, HttpResponse response, string message)
        {
            CreateCookieWithFlashMessage(Notification.Success, message, response);

            return result;
        }
        private static void CreateCookieWithFlashMessage(Notification notification, string message, HttpResponse response)
        {
            response.Cookies.Append(string.Format("Flash.{0}", notification), message);
        }
    }
}
