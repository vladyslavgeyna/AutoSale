using AutoSale.Domain.Models;
using AutoSale.Domain.Response;
using Microsoft.AspNetCore.Identity;

namespace AutoSale.Service.Interfaces
{
    public interface IUserService
    {
        Task<IResponse<IdentityResult>> RemoveAsync(User user);
    }
}