namespace Electronic_Bank.Models
{
    public class Currency
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public decimal ValuetoUSD { get; set; }
        public virtual List<Wallet>? Wallets { get; set; }
    }
}
