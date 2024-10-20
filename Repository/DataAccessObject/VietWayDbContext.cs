using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VietWay.Repository.EntityModel;

namespace VietWay.Repository.DataAccessObject
{
    public class VietWayDbContext() : DbContext()
    {
        #region DbSets
        public DbSet<Account> Account { get; set; }
        public DbSet<Admin> Admin { get; set; }
        public DbSet<Attraction> Attraction { get; set; }
        public DbSet<AttractionCategory> AttractionCategory { get; set; }
        public DbSet<AttractionImage> AttractionImage { get; set; }
        public DbSet<AttractionSchedule> AttractionSchedule { get; set; }
        public DbSet<Booking> Booking { get; set; }
        public DbSet<BookingPayment> BookingPayment { get; set; }
        public DbSet<BookingTourParticipant> BookingTourParticipant { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<EntityHistory> EntityHistory { get; set; }
        public DbSet<EntityStatusHistory> EntityStatusHistory { get; set; }
        public DbSet<Event> Event { get; set; }
        public DbSet<EventCategory> EventCategory { get; set; }
        public DbSet<Feedback> Feedback { get; set; }
        public DbSet<Manager> Manager { get; set; }
        public DbSet<Post> Post { get; set; }
        public DbSet<PostCategory> PostCategory { get; set; }
        public DbSet<Province> Province { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<Tour> Tour { get; set; }
        public DbSet<TourCategory> TourCategory { get; set; }
        public DbSet<TourDuration> TourDuration { get; set; }
        public DbSet<TourTemplate> TourTemplate { get; set; }
        public DbSet<TourTemplateImage> TourTemplateImage { get; set; }
        public DbSet<TourTemplateProvince> TourTemplateProvince { get; set; }
        public DbSet<TourTemplateSchedule> TourTemplateSchedule { get; set; }
        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(GetConnectionString(),option =>
                {
                    option.EnableRetryOnFailure();
                })
                .LogTo(Console.WriteLine, LogLevel.Information);
        }
        public static string GetConnectionString()
        {
            string connectionString = Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING")
                ?? throw new Exception("SQL_CONNECTION_STRING is not set in environment variables");
            return connectionString;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Account>()
                .HasIndex(x => x.Email)
                .IsUnique();
            modelBuilder.Entity<Account>()
                .HasIndex(x => x.PhoneNumber)
                .IsUnique();
        }
    }
}
