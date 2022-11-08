using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RentalAdmin.Models;

namespace RentalAdmin.helper.GoogleRecaptcha
{
    public static class GoogleRecaptcha
    {
        public static IHtmlString ReCaptchaHidden(this HtmlHelper helper)
        {
            var mvcHtmlString = new TagBuilder("input")
            {
                Attributes =
               {
                    new KeyValuePair<string, string>("type", "hidden"),
                    new KeyValuePair<string, string>("id", StaticList.GoogleRecaptchaInputName),
                    new KeyValuePair<string, string>("name", StaticList.GoogleRecaptchaInputName)
               }
            };
            string renderedReCaptchaInput = mvcHtmlString.ToString(TagRenderMode.Normal);
            return MvcHtmlString.Create($"{renderedReCaptchaInput}");
        }

        public static IHtmlString ReCaptchaJS(this HtmlHelper helper, string useCase = "homepage")
        {
            string reCaptchaSiteKey = StaticList.GoogleRecaptchaSiteKey;
            string reCaptchaApiScript = "<script src='https://www.google.com/recaptcha/api.js?render=" + reCaptchaSiteKey + "'></script>;";
            string reCaptchaTokenResponseScript = "<script>$('form').submit(function(e) { e.preventDefault(); grecaptcha.ready(function() { grecaptcha.execute('" + reCaptchaSiteKey + "', {action: '" + useCase + "'}).then(function(token) { $('#" + StaticList.GoogleRecaptchaInputName + "').val(token); $('form').unbind('submit').submit(); }); }); }); </script>;";
            return MvcHtmlString.Create($"{reCaptchaApiScript}{reCaptchaTokenResponseScript}");
        }
        public static IHtmlString ReCaptchaValidationMessage(this HtmlHelper helper, string errorText = null)
        {
            var invalidReCaptchaObj = helper.ViewContext.Controller.TempData["InvalidCaptcha"];
            var invalidReCaptcha = invalidReCaptchaObj?.ToString();
            if (string.IsNullOrWhiteSpace(invalidReCaptcha)) return MvcHtmlString.Create("");
            var buttonTag = new TagBuilder("span")
            {
                Attributes = {
               new KeyValuePair<string, string>("class", "text-danger")
          },
                InnerHtml = errorText ?? invalidReCaptcha
            };
            return MvcHtmlString.Create(buttonTag.ToString(TagRenderMode.Normal));
        }
    }
}