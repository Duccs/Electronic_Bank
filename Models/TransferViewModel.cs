namespace Electronic_Bank.Models
{
    public class TransferViewModel
    {
        public int Id { get; set; }
        public int target { get; set; }
        public decimal amount { get; set; }
    }
}
