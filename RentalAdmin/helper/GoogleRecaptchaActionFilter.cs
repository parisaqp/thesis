using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using RentalAdmin.Models;

namespace RentalAdmin.helper
{
    public class GoogleRecaptchaActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string reCaptchaToken = filterContext.HttpContext.Request.Form[StaticList.GoogleRecaptchaInputName];
            string reCaptchaResponse = ReCaptchaVerify(reCaptchaToken);
            ResponseToken response = new ResponseToken();
            if (reCaptchaResponse != null)
            {
                response = JsonConvert.DeserializeObject<ResponseToken>(reCaptchaResponse);
            }
            if (!response.Success)
            {
                AddErrorAndRedirectToGetAction(filterContext);
            }
            base.OnActionExecuting(filterContext);
        }

        public string ReCaptchaVerify(string responseToken)
        {
            const string apiAddress = "https://www.google.com/recaptcha/api/siteverify";
            string recaptchaSecretKey = StaticList.GoogleRecaptchaSecretKey;
            string urlToPost = $"{apiAddress}?secret={recaptchaSecretKey}&response={responseToken}";
            string responseString = null;
            using (var httpClient = new HttpClient())
            {
                try
                {
                    responseString = httpClient.GetStringAsync(urlToPost).Result;
                }
                catch
                {
                    //Todo: Error handling process goes here  
                }
            }
            return responseString;
        }

        private static void AddErrorAndRedirectToGetAction(ActionExecutingContext filterContext, string message = null)
        {
            filterContext.Controller.TempData["InvalidCaptcha"] = message ?? "Invalid Captcha! The form cannot be submitted.";
            filterContext.Result = new RedirectToRouteResult(filterContext.RouteData.Values);
        }

        internal class ResponseToken
        {
            public bool Success { get; set; }
            public float Score { get; set; }
            public string Action { get; set; }
            public DateTime Challenge_TS { get; set; }
            public string HostName { get; set; }
            public List<int> ErrorCodes { get; set; }
        }
    }
}