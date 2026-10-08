using Microsoft.EntityFrameworkCore;
using WellDoneGarageApi.Models;

namespace WellDoneGarageApi.Data
{
    public class GarageDbContext : DbContext
    {
        public GarageDbContext(DbContextOptions<GarageDbContext> options) : base(options)
        {
            
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<StockTransaction> StockTransactions { get; set; }
        public DbSet<JobCard> JobCards { get; set; }
        public DbSet<JobItem> JobItems { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<VehicleOwnership> VehicleOwnerships { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Customer → Vehicle
            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.Customer)
                .WithMany()
                .HasForeignKey(v => v.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Vehicle → JobCard
            modelBuilder.Entity<JobCard>()
                .HasOne(j => j.Vehicle)
                .WithMany()
                .HasForeignKey(j => j.VehicleId)
                .OnDelete(DeleteBehavior.Restrict);

            // Customer → JobCard
            modelBuilder.Entity<JobCard>()
                .HasOne(j => j.Customer)
                .WithMany()
                .HasForeignKey(j => j.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // JobCard → JobItem
            modelBuilder.Entity<JobItem>()
                .HasOne(j => j.JobCard)
                .WithMany()
                .HasForeignKey(j => j.JobCardId)
                .OnDelete(DeleteBehavior.Cascade);

            // Item → JobItem
            modelBuilder.Entity<JobItem>()
                .HasOne(j => j.Item)
                .WithMany()
                .HasForeignKey(j => j.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            // Item → StockTransaction
            modelBuilder.Entity<StockTransaction>()
                .HasOne(s => s.Item)
                .WithMany()
                .HasForeignKey(s => s.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            // JobCard → Invoice
            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.JobCard)
                .WithOne()
                .HasForeignKey<Invoice>(i => i.JobCardId)
                .OnDelete(DeleteBehavior.Restrict);

            // Customer → Invoice
            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Customer)
                .WithMany()
                .HasForeignKey(i => i.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Invoice → Payment
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Invoice)
                .WithMany()
                .HasForeignKey(p => p.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

            // Unique vehicle number
            modelBuilder.Entity<Vehicle>()
                .HasIndex(v => v.VehicleNumber)
                .IsUnique();

            modelBuilder.Entity<Item>()
                .Property(i => i.UnitPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<JobItem>()
                .Property(j => j.PriceAtTime)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Invoice>()
                .Property(i => i.LabourCost)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Invoice>()
                .Property(i => i.PartsCost)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Invoice>()
                .Property(i => i.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasPrecision(18, 2);

            // Customer query filter to exclude deleted customers
            modelBuilder.Entity<Customer>()
                .HasQueryFilter(c => !c.IsDeleted);

            // Vehicle query filter
            modelBuilder.Entity<Vehicle>()
                .HasQueryFilter(v => !v.IsDeleted);

            // Item query filter
            modelBuilder.Entity<Item>()
                .HasQueryFilter(i => !i.IsDeleted);

            // Jobcard query filter
            modelBuilder.Entity<JobCard>()
                .HasQueryFilter(i => !i.IsDeleted);

            // Vehicle Ownership
            modelBuilder.Entity<VehicleOwnership>()
                .ToTable("VehicleOwnership");

            modelBuilder.Entity<VehicleOwnership>()
                .HasOne(vo => vo.Vehicle)
                .WithMany(v => v.OwnershipHistory)
                .HasForeignKey(vo => vo.VehicleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<VehicleOwnership>()
                .HasOne(vo => vo.Customer)
                .WithMany(c => c.VehicleOwnerships)
                .HasForeignKey(vo => vo.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Enddate in VehicleOwnership should be nullable
            modelBuilder.Entity<VehicleOwnership>()
                .HasIndex(vo => vo.VehicleId)
                .IsUnique()
                .HasFilter("[EndDate] IS NULL");
        }
    }
}
