using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using VietWay.Repository.EntityModel;

namespace VietWay.Repository.DataAccessObject
{
    public class VietWayDbContext() : DbContext()
    {
        #region DbSets
        public DbSet<Account> Account { get; set; }
        public DbSet<Admin> Admin { get; set; }
        public DbSet<Attraction> Attraction { get; set; }
        public DbSet<AttractionImage> AttractionImage { get; set; }
        public DbSet<AttractionSchedule> AttractionSchedule { get; set; }
        public DbSet<AttractionType> AttractionType { get; set; }
        public DbSet<BookingPayment> BookingPayment { get; set; }
        public DbSet<CustomerFeedback> CustomerFeedback { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Image> Image { get; set; }
        public DbSet<Manager> Manager { get; set; }
        public DbSet<Province> Province { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<Tour> Tour { get; set; }
        public DbSet<TourBooking> TourBooking { get; set; }
        public DbSet<TourCategory> TourCategory { get; set; }
        public DbSet<TourDuration> TourDuration { get; set; }
        public DbSet<TourTemplate> TourTemplate { get; set; }
        public DbSet<TourTemplateImage> TourTemplateImage { get; set; }
        public DbSet<TourTemplateProvince> TourTemplateProvince { get; set; }
        public DbSet<TourTemplateSchedule> TourTemplateSchedule { get; set; }
        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(GetConnectionString(),option =>
            {
                option.EnableRetryOnFailure();
            });
        }
        private static string GetConnectionString()
        {
            string? environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .AddEnvironmentVariables()
                .Build();
            string? connectionString = environment == "Development" ?
                configuration.GetConnectionString("SQLDatabase") :
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
