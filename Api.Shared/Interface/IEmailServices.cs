using Api.Shared.DTOs.Emails;

namespace Api.Shared.Interface
{
    public interface IEmailServices
    {
        string GenerateEmailTemplate<T>(string templateName, T mailTemplateModel);
        Task<bool> SendAsync(MailRequest request, CancellationToken cancellationToken = default);
    }
}