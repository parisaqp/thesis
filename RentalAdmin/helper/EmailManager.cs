using System;
using System.Collections.Generic;
using RestSharp;
using RentalAdmin.Models;

namespace RentalAdmin.helper
{
    public class EmailManager
    {
        private RentalAdmin.Models.RentalEntities db;//= new limonadeEntities();
        public EmailManager(RentalEntities DB)
        {
            this.db = DB;

        }
        public bool sendDirectMessage(string to, string subject, string title, string templateName,
            object model, string shortTtile35char, string EmailReason,
             byte EmailLayoutID, System.Guid DirecMessageID,
            bool ForceEmail = false, bool isbulk = false
            , Nullable<DateTime> DirectMessageExpire = null,
            List<System.Net.Mail.LinkedResource> linkedResources = null, List<System.Net.Mail.Attachment> attachments = null,
            string unsubscribeUrl = null, string replyTo = null
            , byte Priority = 1)
        {
            try
            {
                string DirecMessageDataModel = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                DirectMessage dm = new DirectMessage()
                {
                    DirecMessageID = DirecMessageID,
                    DirecMessageDataModel = DirecMessageDataModel,
                    // DirecMessageEmailSent=false,
                    DirecMessageEmailTo = to,
                    //DirecMessageEmailTry=0,
                    DirecMessageInsertDate = DateTime.UtcNow,
                    DirecMessageIsBulk = isbulk,
                    DirecMessagePriority = Priority,
                    DirecMessageShortTitle = shortTtile35char,
                    DirecMessageStatus = 0,
                    DirecMessageSubject = subject,
                    DirecMessageTitle = title,
                    EmailLayoutID = EmailLayoutID,
                    DirectMessageExpire = DirectMessageExpire != null ? (DateTime)DirectMessageExpire : DateTime.UtcNow.AddMonths(1),
                    DirectMessageForceEmail = true,
                    DirectMessageEmailReason = EmailReason
                };
                db.DirectMessages.Add(dm);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public async System.Threading.Tasks.Task<bool> sendDirectMessageasync(string to, string subject, string title, string templateName,
            object model, string shortTtile35char, string EmailReason,
            byte EmailLayoutID, System.Guid DirecMessageID,
            bool ForceEmail = false, bool isbulk = false
            , Nullable<DateTime> DirectMessageExpire = null,
            List<System.Net.Mail.LinkedResource> linkedResources = null, List<System.Net.Mail.Attachment> attachments = null,
            string unsubscribeUrl = null, string replyTo = null
            , byte Priority = 1)
        {
            try
            {
                string DirecMessageDataModel = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                DirectMessage dm = new DirectMessage()
                {
                    DirecMessageID = DirecMessageID,
                    DirecMessageDataModel = DirecMessageDataModel,
                    // DirecMessageEmailSent=false,
                    DirecMessageEmailTo = to,
                    //DirecMessageEmailTry=0,
                    DirecMessageInsertDate = DateTime.UtcNow,
                    DirecMessageIsBulk = isbulk,
                    DirecMessagePriority = Priority,
                    DirecMessageShortTitle = shortTtile35char,
                    DirecMessageStatus = 0,
                    DirecMessageSubject = subject,
                    DirecMessageTitle = title,
                    EmailLayoutID = EmailLayoutID,
                    DirectMessageExpire = DirectMessageExpire != null ? (DateTime)DirectMessageExpire : DateTime.UtcNow.AddMonths(1),
                    DirectMessageForceEmail = true,
                    DirectMessageEmailReason = EmailReason
                };
                db.DirectMessages.Add(dm);
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
    }
    //    public class EmailManager
    //    {
    //        public void
    //sendEmail(string MessageStr, string fromEmail, string nameStr)
    //        {
    //            try
    //            {
    //                //var client = new RestClient("http://example.com");
    //                //// client.Authenticator = new HttpBasicAuthenticator(username, password);
    //                //var request = new RestRequest("resource/{id}");
    //                //request.AddParameter("thing1", "Hello");
    //                //request.AddParameter("thing2", "world");
    //                //request.AddHeader("header", "value");
    //                //request.AddFile("file", path);
    //                //var response = client.Post(request);
    //                //var content = response.Content; // Raw content as string
    //                //var response2 = client.Post<Person>(request);
    //                //var name = response2.Data.Name;


    //                //                MailMessage mail = new MailMessage();
    //                //                SmtpClient SmtpServer = new SmtpClient("smtp-relay.sendinblue.com");

    //                //                mail.From = new MailAddress("pouriya.ghasemi1@gmail.com");
    //                //                mail.To.Add("ghyahoo.com");
    //                //                mail.Subject = "new message";
    //                //                mail.Body = MessageStr;

    //                //                SmtpServer.Port = 587;
    //                //                SmtpServer.Credentials = new System.Net.NetworkCredential("pouriya.ghasemi1@gmail.com", "xsmtpsib-e8d29da7f23d95518f1d833555c7280ccf0a6fe3cc3542a9373871e272217bf2-GYdxNUTCnv7bV2Xa");
    //                //                //SmtpServer.EnableSsl = true;

    //                //                SmtpServer.Send(mail);
    //                //                //MessageBox.Show("mail Send");
    //                //                HttpClient client = new HttpClient();
    //                //                var values = new Dictionary<string, string>
    //                //{
    //                //    { "application/json", "{\"sender\":{\"name\":\"test\",\"email\":\"pouriya.ghasemi1@gmail.com\"},\"to\":[{\"email\":\"ghpouriya@yahoo.com\",\"name\":\"pouriya\"}],\"replyTo\":{\"email\":\"pouriya.ghasemi1@gmail.com\"},\"htmlContent\":\"<h1>test</h1>\",\"textContent\":\"test\",\"subject\":\"test subject\"}"},

    //                //};

    //                //                var content = new FormUrlEncodedContent(values);


    //                //                var response = await client.PostAsync("https://api.sendinblue.com/v3/smtp/email", content);
    //                //                // response.Headers.Add("application/json", "{\"sender\":{\"name\":\"test\",\"email\":\"pouriya.ghasemi1@gmail.com\"},\"to\":[{\"email\":\"ghpouriya@yahoo.com\",\"name\":\"pouriya\"}],\"replyTo\":{\"email\":\"pouriya.ghasemi1@gmail.com\"},\"htmlContent\":\"<h1>test</h1>\",\"textContent\":\"test\",\"subject\":\"test subject\"}");

    //                //                response.Headers.Add("accept", "application/json");
    //                //                response.Headers.Add("content-type", "application/json");
    //                //                response.Headers.Add("api-key", "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");
    //                //                var responseString = await response.Content.ReadAsStringAsync();

    //                var client = new RestClient("https://api.sendinblue.com/v3/smtp/email");
    //                var request = new RestRequest(Method.POST);
    //                request.AddHeader("accept", "application/json");
    //                request.AddHeader("content-type", "application/json");
    //                request.AddHeader("api-key", "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");
    //                request.AddParameter("application/json", "{\"sender\":{\"name\":\" "+"rentalir-"+ nameStr+"\",\"email\":\""+fromEmail+"\"},\"to\":[{\"email\":\"pouriya.ghasemi1@gmail.com\",\"name\":\""+ nameStr + "\"}],\"replyTo\":{\"email\":\""+fromEmail+ "\"},\"htmlContent\":\"<p>name sender:"+nameStr+"</p>" + "<p>"+MessageStr+"</p>\",\"textContent\":\""+ MessageStr + "\",\"subject\":\""+"rental contact"+"\"}", ParameterType.RequestBody);
    //                IRestResponse response = client.Execute(request);

    //            }
    //            catch (Exception ex)
    //            {
    //                //MessageBox.Show(ex.ToString());
    //            }
    //        }

    //    }
}