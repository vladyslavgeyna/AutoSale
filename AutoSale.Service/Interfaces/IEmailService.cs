using AutoSale.Domain.Response;

namespace AutoSale.Service.Interfaces
{
    public interface IEmailService
    {
        Task<IResponse<bool>> SendAsync(string emailTo, string subject, string body);
    }
}