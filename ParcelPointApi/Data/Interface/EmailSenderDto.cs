using System.Net.Mail;
using System.Net;
using System.Text;
using DotNetEnv;

namespace ParcelPointApi.Data.Interface
{
    public interface IEmailSenderDto
    {
        void EmailSender(string email, string subject, string code, string name);
    }
    public class EmailSenderDto : IEmailSenderDto
    {
        public void EmailSender(string email, string subject, string code, string name = "Default") 
        {
            // Setup
            string[] creds = [Env.GetString("EMAIL_ADDRESS"), Env.GetString("EMAIL_PASSWORD")];
            Console.WriteLine($"CREDENTIALS: {creds[0]} | {creds[1]}");

            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(creds[0], creds[1]);

            // Create email message
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(creds[0]);
            mailMessage.To.Add(email);
            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true;

            string verificationCode = code; // Replace with actual generated code

            StringBuilder mailBody = new StringBuilder();
            mailBody.AppendFormat(@"
            <div style='font-family: Arial, sans-serif; max-width: 600px; margin: auto; padding: 20px; border: 1px solid #ddd; border-radius: 10px; background-color: #f9f9f9;'>
                <h2 style='color: #2C3E50; text-align: center;'>User Verification</h2>
                <p style='font-size: 16px; color: #555;'>Hello, {1}!</p>
                <p style='font-size: 16px; color: #555;'>Here is your 6-digit verification code:</p>
        
                <div style='text-align: center; font-size: 24px; font-weight: bold; color: #E74C3C; padding: 10px; background: #FFF3CD; border-radius: 5px;'>
                    {0}
                </div>

                <p style='font-size: 16px; color: #555;'>Please enter this code within <strong>10 minutes</strong> before it expires.</p>
                <p style='font-size: 16px; color: #555;'>If you did <strong>not</strong> request this verification, you can safely ignore this email. No changes will be made to your account.</p>
                <p style='font-size: 16px; color: #555;'>Thank you,</p>
                <p style='font-size: 16px; color: #555;'><strong>LOCKBOX INNOVATORS</strong></p>
        
                <p style='text-align: center; font-size: 14px; color: #888;'>This is an automated message. Please do not reply.</p>
            </div>", verificationCode, name);

            mailMessage.Body = mailBody.ToString();

            // Send email
            client.Send(mailMessage);
        }
    } 
}
