using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.ModelEntity;

namespace VietWay.Repository.DataAccessObject
{
    public class VietWayDbContext() : DbContext()
    {
        #region DbSets
        public DbSet<Account> Account { get; set; }
        public DbSet<AdminInfo> AdminInfo { get; set; }
        public DbSet<Attraction> Attraction { get; set; }
        public DbSet<AttractionImage> AttractionImage { get; set; }
        public DbSet<AttractionSchedule> AttractionSchedule { get; set; }
        public DbSet<AttractionType> AttractionType { get; set; }
        public DbSet<BookingPayment> BookingPayment { get; set; }
        public DbSet<CustomerFeedback> CustomerFeedback { get; set; }
        public DbSet<CustomerInfo> CustomerInfo { get; set; }
        public DbSet<Image> Image { get; set; }
        public DbSet<ManagerInfo> ManagerInfo { get; set; }
        public DbSet<Province> Province { get; set; }
        public DbSet<StaffInfo> StaffInfo { get; set; }
        public DbSet<Tour> Tour { get; set; }
        public DbSet<TourBooking> TourBooking { get; set; }
        public DbSet<TourCategory> TourCategory { get; set; }
        public DbSet<TourTemplate> TourTemplate { get; set; }
        public DbSet<TourTemplateImage> TourTemplateImage { get; set; }
        public DbSet<TourTemplateProvince> TourTemplateProvince { get; set; }
        public DbSet<TourTemplateSchedule> TourTemplateSchedule { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(GetConnectionString());
        }
        private string GetConnectionString()
        {
            string? environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .AddEnvironmentVariables()
                .Build();
            string? connectionString = environment == "Development" ?
                configuration.GetConnectionString("SQLDatabase") :
                configuration.GetConnectionString("ProdSQLDatabase");
            return connectionString ?? throw new Exception("Cannot get connection string");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            modelBuilder.Entity<AttractionSchedule>()
                .HasKey(kas => new { kas.AttractionId, kas.DayNumber });
            modelBuilder.Entity<TourTemplateSchedule>()
                .HasKey(kts => new { kts.TourTemplateId, kts.DayNumber });
            modelBuilder.Entity<AttractionSchedule>()
            .HasOne<TourTemplateSchedule>()
            .WithMany()
            .HasForeignKey(fk => new { fk.TourTemplateId, fk.DayNumber });
        }
    }
}
