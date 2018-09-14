//using SendGrid;
//using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace NetCoreSample.Helper
{
    public class SendGridHelper
    {


        //public static async void SendVerifyCodeAsync(string email, string userName, string code)
        //{
        //    var client = new SendGridClient("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");
        //    var msg = new SendGridMessage()
        //    {
        //        From = new EmailAddress("netcoresample@service.com", "netcoresample"),
        //        Subject = "Verify",
        //        HtmlContent = $"<a href='https://netcoresample.herokuapp.com/Home/VerifyAccount?email={email}&userName={userName}&code={code}>'"
        //    };
        //    msg.AddTo(new EmailAddress("ce678013@gmail.com", "Test User"));
        //    var response = await client.SendEmailAsync(msg);
        //}


        //public static async void SendEmailAsync () {
        //    var client = new SendGridClient("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");
        //    var msg = new SendGridMessage()
        //    {
        //        From = new EmailAddress("netcoresample@service.com", "netcoresample"),
        //        Subject = "Hello World from the SendGrid CSharp SDK!",
        //        PlainTextContent = "Hello, Email!",
        //        HtmlContent = "<strong>Hello, Email!</strong>"
        //    };
        //    msg.AddTo(new EmailAddress("ce678013@gmail.com", "Test User"));
        //    var response = await client.SendEmailAsync(msg);
        //}
    }
}
