using Microsoft.AspNetCore.Hosting;

namespace Api.Infrastructure.Services
{
    public class EmailServices : IEmailServices
    {
        private readonly MailSettings _settings;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EmailServices(IOptions<MailSettings> settings, IWebHostEnvironment webHostEnvironment)
        {
            _settings = settings.Value;
            _webHostEnvironment = webHostEnvironment;
        }

        public string GenerateEmailTemplate<T>(string templateName, T mailTemplateModel)
        {
            try
            {
                string baseDirectory = $"{_webHostEnvironment.WebRootPath}";

                string template = GetTemplate(templateName, baseDirectory);

                IRazorEngine razorEngine = new RazorEngine();
                IRazorEngineCompiledTemplate modifiedTemplate = razorEngine.Compile(template);

                return modifiedTemplate.Run(mailTemplateModel);
            }
            catch
            {
                return "";
            }
        }

        public static string GetTemplate(string templateName, string baseDirectory)
        {
            string tmplFolder = Path.Combine(baseDirectory, "Email Templates");
            string filePath = Path.Combine(tmplFolder, $"{templateName}.cshtml");

            using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var sr = new StreamReader(fs, Encoding.Default);
            string mailText2 = sr.ReadToEnd();
            sr.Close();
            return mailText2;
        }

        public async Task<bool> SendAsync(MailRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                var email = new MimeMessage();
                // From
                email.From.Add(new MailboxAddress(_settings.DisplayName, request.From ?? _settings.From));
                // To
                foreach (string address in request.To) email.To.Add(MailboxAddress.Parse(address));
                // Reply To
                if (!string.IsNullOrEmpty(request.ReplyTo)) email.ReplyTo.Add(new MailboxAddress(request.ReplyToName, request.ReplyTo));
                // Bcc
                if (request.Bcc != null)
                {
                    foreach (string address in request.Bcc.Where(bccValue => !string.IsNullOrWhiteSpace(bccValue)))
                        email.Bcc.Add(MailboxAddress.Parse(address.Trim()));
                }
                // Cc
                if (request.Cc != null)
                {
                    foreach (string? address in request.Cc.Where(ccValue => !string.IsNullOrWhiteSpace(ccValue)))
                        email.Cc.Add(MailboxAddress.Parse(address.Trim()));
                }
                // Headers
                if (request.Headers != null)
                {
                    foreach (var header in request.Headers)
                        email.Headers.Add(header.Key, header.Value);
                }
                // Content
                var builder = new BodyBuilder();
                email.Sender = new MailboxAddress(request.DisplayName ?? _settings.DisplayName, request.From ?? _settings.From);
                email.Subject = request.Subject;
                builder.HtmlBody = request.Body;
                // Create the file attachments for this e-mail message
                if (request.AttachmentData != null)
                {
                    foreach (var attachmentInfo in request.AttachmentData)
                        builder.Attachments.Add(attachmentInfo.Key, attachmentInfo.Value);
                }

                email.Body = builder.ToMessageBody();

                using var smtp = new MailKit.Net.Smtp.SmtpClient();
                await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls, cancellationToken);
                await smtp.AuthenticateAsync(_settings.UserName, _settings.Password, cancellationToken);
                await smtp.SendAsync(email, cancellationToken);
                await smtp.DisconnectAsync(true, cancellationToken);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
