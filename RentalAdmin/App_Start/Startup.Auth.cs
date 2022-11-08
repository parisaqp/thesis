using System;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using RentalAdmin.Models;

namespace RentalAdmin
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit https://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(60),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                },
                ExpireTimeSpan = TimeSpan.FromDays(61)
            });            
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");

            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = StaticList.GoogleApiClientID,
                ClientSecret = StaticList.GoogleApiSecretID
                     ,
                Caption = "به رسانه آموزشی لیموناد خوش آمدید"

                ,
                Provider = new GoogleOAuth2AuthenticationProvider()
                {
                    OnAuthenticated = context =>
                    {
                        var userDetail = context.User;
                        context.Identity.AddClaim(new System.Security.Claims.Claim(ClaimTypes.Name, context.Identity.FindFirstValue(ClaimTypes.Name)));
                        context.Identity.AddClaim(new Claim(ClaimTypes.Email, context.Identity.FindFirstValue(ClaimTypes.Email)));
                        //context.Identity.AddClaim(new Claim(ClaimTypes.DateOfBirth, context.Identity.FindFirstValue(ClaimTypes.DateOfBirth)));
                        //context.Identity.AddClaim(new Claim(ClaimTypes.Gender, context.Identity.FindFirstValue(ClaimTypes.Gender)));
                        //context.Identity.AddClaim(new Claim(ClaimTypes.Country, context.Identity.FindFirstValue(ClaimTypes.Country)));
                        //context.Identity.AddClaim(new Claim(ClaimTypes.MobilePhone, context.Identity.FindFirstValue(ClaimTypes.MobilePhone)));
                        string Image = "";
                        string Gender = "";
                        string FName = "";
                        string LName = "";
                        string googlePlus = "";
                        string PersonalWebPage = "";
                        if (userDetail["name"] != null)
                        {
                            FName = (string)userDetail["givenName"];
                            LName = (string)userDetail["familyName"];
                        }
                        //Gender = (string)userDetail["gender"];
                        if (userDetail["picture"] != null)
                        {
                            Image = (string)userDetail["picture"];
                        }
                        //googlePlus = (string)userDetail["url"];
                        //if (userDetail["urls"] != null)
                        //{
                        //    PersonalWebPage = (string)userDetail["urls"][0]["value"];
                        //}
                        //byte urlcount = 0;//ta chek shavad tedad noe url ke ersal shode ast.
                        //if (Gender != "" && Gender != null)
                        //{
                        //    context.Identity.AddClaim(new Claim(ClaimTypes.Gender, Gender));
                        //}
                        //if (PersonalWebPage != "" && PersonalWebPage != null)
                        //{
                        //    urlcount += 1;
                        //    context.Identity.AddClaim(new Claim(ClaimTypes.Webpage, PersonalWebPage));
                        //}
                        if (FName != "" && FName != null)
                        {
                            context.Identity.AddClaim(new Claim(ClaimTypes.GivenName, FName));
                        }
                        if (LName != "" && LName != null)
                        {
                            context.Identity.AddClaim(new Claim(ClaimTypes.StreetAddress, LName));
                        }
                        //if (googlePlus != "" && googlePlus != null)
                        //{
                        //    try
                        //    {
                        //        googlePlus = googlePlus.Substring(googlePlus.LastIndexOf('/') + 1, googlePlus.Length - (googlePlus.LastIndexOf('/') + 1));
                        //    }
                        //    catch 
                        //    {}
                        //    context.Identity.AddClaim(new Claim(ClaimTypes.Webpage, googlePlus));
                        //    //urlcount += 3;
                        //}
                        //در این قسمت در ادامه ادرس متن زیر وجود دارد که عکس را کوچک می کند
                        //?sz=50
                        //پس باید حذف شود
                        if (Image != "" && Image != null)
                        {
                            if (Image.Contains("?"))
                                Image = Image.Substring(0, Image.LastIndexOf('?'));
                            context.Identity.AddClaim(new Claim(ClaimTypes.Webpage, Image));
                            //  urlcount += 7;
                        }
                        // context.Identity.AddClaim(new Claim(ClaimTypes.SerialNumber, urlcount.ToString()));
                        //var aaa = context.Identity.FindFirst(ClaimTypes.Webpage);//FindFirstValue(ClaimTypes.Uri);
                        return System.Threading.Tasks.Task.FromResult(0);
                    }
                }
            });
        }
        class ImageGoogle
        {
            public string isDefault { get; set; }
            public string url { get; set; }

        }
    }
}