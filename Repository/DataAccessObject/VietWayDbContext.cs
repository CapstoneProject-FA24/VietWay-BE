using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Repository.ModelEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.DataAccessObject
{
    public class VietWayDbContext(IConfiguration configuration) : DbContext()
    {
        private readonly IConfiguration _configuration = configuration;
        #region DbSets
        public DbSet<Account> Account { get; set; }
        public DbSet<Attraction> Attraction { get; set; }
        public DbSet<AttractionImage> AttractionImage { get; set; }
        public DbSet<AttractionType> AttractionType { get; set; }
        public DbSet<BookingPayment> BookingPayment { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<CompanyStaffInfo> CompanyStaffInfo { get; set;  }
        public DbSet<CustomerFeedback> CustomerFeedback { get; set; }
        public DbSet<CustomerInfo> CustomerInfo { get; set; }
        public DbSet<Image> Image { get; set; }
        public DbSet<ManagerInfo> ManagerInfo { get; set; }
        public DbSet<Province> Province { get; set; }
        public DbSet<Tour> Tour { get; set; }
        public DbSet<TourBooking> TourBooking { get; set; }
        public DbSet<TourCategory> TourCategory { get; set; }
        public DbSet<TourGuideAssignment> TourGuideAssignment { get; set; }
        public DbSet<TourGuideInfo> TourGuideInfo { get; set; }
        public DbSet<TourTemplate> TourTemplate { get; set; }
        public DbSet<TourTemplateAttraction> TourTemplateAttraction { get;set; }
        public DbSet<TourTemplateImage> TourTemplateImage { get; set; }
        public DbSet<TourTemplateProvince> TourTemplateProvince { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(GetConnectionString());
        }
        private string GetConnectionString()
        {
            string? environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            string? connectionString = environment == "Development" ?
                _configuration.GetConnectionString("SQLDatabase") :
                Environment.GetEnvironmentVariable("VietWayDB_ConnectionString");
            return connectionString ?? throw new Exception("Cannot get connection string");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}
