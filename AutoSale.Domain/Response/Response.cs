using AutoSale.Domain.Enum;

namespace AutoSale.Domain.Response
{
    public class Response<T> : IResponse<T>
    {
        public string Description { get; set; }
        
        public ResponseCode Code { get; set; }
        
        public T Data { get; set; }
    }
}