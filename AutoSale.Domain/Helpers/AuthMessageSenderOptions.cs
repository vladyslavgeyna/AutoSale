namespace AutoSale.Domain.Helpers
{
    public class AuthMessageSenderOptions
    {
        public string Email { get; set; } = null!;
        
        public string Password { get; set; } = null!;

        public string Host { get; set; } = null!;

        public int Port { get; set; }
    }
}