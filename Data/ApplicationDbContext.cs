using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Electronic_Bank.Models;

namespace Electronic_Bank.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Electronic_Bank.Models.Client>? Client { get; set; }
        public DbSet<Electronic_Bank.Models.Currency>? Currency { get; set; }
        public DbSet<Electronic_Bank.Models.Transaction>? Transaction { get; set; }
        public DbSet<Electronic_Bank.Models.Wallet>? Wallet { get; set; }
    }
}