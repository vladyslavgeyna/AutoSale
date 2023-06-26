using AutoSale.Domain.Enum;

namespace AutoSale.Domain.Response
{
    public interface IResponse<T>
    {
        string Description { get; }
        
        ResponseCode Code { get; }
        
        T Data { get; }        
    }
}