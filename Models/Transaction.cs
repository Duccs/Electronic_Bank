namespace Electronic_Bank.Models
{
    public class Transaction
    {

        public int Id { get; set; }
        public int WalletId { get; set; }
        public virtual Wallet Wallet { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public TypeEnum type { get; set; }
    }
    public enum TypeEnum
    {
        Withdraw,
        Deposit,
        Transfer
    }
}
