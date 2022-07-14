namespace Electronic_Bank.Models
{
    public class Wallet
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public virtual Client? Client { get; set; }
        public decimal Amount { get; set; }
        public int CurrencyId { get; set; }
        public virtual Currency? Currency { get; set; }
        public virtual List<Transaction>? Transactions { get; set; }
    }
}
