using System.ComponentModel.DataAnnotations.Schema;

namespace Electronic_Bank.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public virtual List<Wallet>? Wallets { get; set; }
        [NotMapped]
        public IFormFile Image { get; set; }
        public string ImagePath { get; set; }
    }
}
