using Microsoft.EntityFrameworkCore;
using Web_App_BMS.Model;

// the connection to the database will be handled by the
// ASP.Net Core Service

namespace Web_App_BMS.Data
{
    public class BMS_DBContext : DbContext
    { 
        public DbSet<Customer> Customer {get; set;}
        public DbSet<Banker> Banker { get; set; }
        public DbSet<Branch> Branch { get; set; }
        public DbSet<Loan> Loan { get; set; }
        public DbSet<CreditCard> CreditCard { get; set; }
        public DbSet<Account> Account { get; set; }
        public DbSet<Loan_Payment> Loan_Payment { get; set; }
        public DbSet<Transaction> Transaction { get; set; }

        public BMS_DBContext(DbContextOptions options): base(options)
        {
            
        }
        public BMS_DBContext()
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // make the relationships between the entities
            modelBuilder.Entity<Customer>()
                .HasOne(c => c.Account)
                .WithOne(a => a.Customer);

            modelBuilder.Entity<Banker>()
                .HasOne(b => b.Branch)
                .WithOne(br => br.Banker);
            
            modelBuilder.Entity<Loan>()
                .HasOne(l => l.Account)
                .WithOne(a => a.Loan);

            modelBuilder.Entity<Loan>()
                .HasOne(l => l.Branch)
                .WithMany(br => br.Loans)
                .OnDelete(DeleteBehavior.Cascade); //Deletes a loan when there is a
                                                   //foreign key of the deleted branch

            modelBuilder.Entity<Account>()
                .HasOne(a => a.Branch)
                .WithMany(br => br.Accounts);

            modelBuilder.Entity<Transaction>()
                .HasOne(tr => tr.Customer)
                .WithMany(c => c.Transactions)
                .OnDelete(DeleteBehavior.Cascade); //Deletes a transaction when there is a
                                                    //foreign key of the deleted customer

            modelBuilder.Entity<Transaction>()
                .HasOne(tr => tr.Account)
                .WithMany(a => a.Transactions);

            modelBuilder.Entity<CreditCard>()
                .HasOne(cr => cr.Customer)
                .WithOne(c => c.Card)
                .OnDelete(DeleteBehavior.Cascade); //Deletes a credit card when there is a
                                                    //foreign key of the deleted customer

            modelBuilder.Entity<CreditCard>()
                .HasOne(cr => cr.Account)
                .WithOne(a => a.Card);

            modelBuilder.Entity<Loan_Payment>()
                .HasOne(lp => lp.Loan)
                .WithOne(l => l.Loan_Payment);


        }
    }
}
