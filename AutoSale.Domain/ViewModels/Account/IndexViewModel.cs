namespace AutoSale.Domain.ViewModels.Account
{
    public class IndexViewModel
    {
        public string Name { get; set; } = null!;

        public string Surname { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? UserImageName { get; set; }
    }
}