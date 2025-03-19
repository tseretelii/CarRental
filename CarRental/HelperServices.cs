using System.Net.Mail;
using System.Net;

namespace CarRental
{
    public static class HelperServices
    {
        private static readonly string _folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ExceptionsLog");

        public static string GetFullException (this Exception ex)
        {
            var exs = new List<string>();

            while (ex != null)
            {
                exs.Add (ex.Message);

                ex = ex.InnerException;
            }

            return string.Join ("=>", exs);
        }

        public static void SendEmail(string userName, string userEmail, string code)
        {
            var fromAddress = new MailAddress("emailformycsharpproject@gmail.com", "Car Rental Service");
            var toAddress = new MailAddress(userEmail, "Recipient Name");
            const string fromPassword = "qcca kyno bwkt ukzy";
            const string subject = "Verification Code";
            string body = $"Dear {userName} Thank you for signing up at Car Rental Service! We’re thrilled to have you on board. " +
                $"Your registration needs verification to complete, please visit URL and input this verification code: {code}";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }

            Console.WriteLine("Email sent successfully!");
        }

        public static async void WriteExceptionToFile(Exception ex, string endpoint)
        {
            var filePath = Path.Combine(_folderPath, DateTime.Now.ToString("yyyy-MM-dd HHmmss") + " -- " + endpoint + ".txt");

            if (!Directory.Exists(_folderPath))
                Directory.CreateDirectory(_folderPath);

            if (!File.Exists(filePath))
            {
                using (var fileStream = File.Create(filePath)) { }
            }

            using (var writer = new StreamWriter(filePath, append: true))
            {
                await writer.WriteLineAsync(ex.GetFullException());
            }
        }
    }
}
