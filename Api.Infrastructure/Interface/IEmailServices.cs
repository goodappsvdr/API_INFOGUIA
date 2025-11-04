namespace Api.Infrastructure.Services.Interface
{
    public interface IEmailServices
    {
        string GenerateEmailTemplate<T>(string templateName, T mailTemplateModel);
        Task<bool> SendAsync(MailRequest request, CancellationToken cancellationToken = default);
    }
}