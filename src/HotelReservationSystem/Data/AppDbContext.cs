using Microsoft.EntityFrameworkCore;
using HotelReservationSystem.Models;

namespace HotelReservationSystem.Data
{
    //This class connects my classes and the MySQL database: which classes to assign into database, and how they relate to each other.
    //My connection string is in appsettings.json.
   
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        //Assigning table names.
        public DbSet<User> Users { get; set; }
        public DbSet<Guest> Guests { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<SeasonalRate> SeasonalRates { get; set; }
        public DbSet<PricingFactor> PricingFactors { get; set; }
        public DbSet<AddOn> AddOns { get; set; }
        public DbSet<BookingAddOn> BookingAddOns { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        //We specify details, such as unique relationships and one-to-one relationships.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //No two Users can have the same email.
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            //Guest belongs to exactly one User, on the other side - User can be at most one Guest.
            modelBuilder.Entity<Guest>()
                .HasOne(g => g.User)
                .WithOne(u => u.Guest)
                .HasForeignKey<Guest>(g => g.UserId);

            //Staff member belongs to exactly one User, on the other side - User can be at most one Staff member.
            modelBuilder.Entity<Staff>()
                .HasOne(s => s.User)
                .WithOne(u => u.Staff)
                .HasForeignKey<Staff>(s => s.UserId);

            //One-to-many relationship
            //Booking is for a RoomType, not a specific room - Front Desk assigns the room later.
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.AssignedRoom)
                .WithMany()
                .HasForeignKey(b => b.AssignedRoomId)
                .OnDelete(DeleteBehavior.SetNull);

            //Decimals

            modelBuilder.Entity<RoomType>()
                .Property(r => r.BasePrice).HasPrecision(10, 2);

            modelBuilder.Entity<Booking>()
                .Property(b => b.BasePriceAtBooking).HasPrecision(10, 2);
            modelBuilder.Entity<Booking>()
                .Property(b => b.FinalPrice).HasPrecision(10, 2);

            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount).HasPrecision(10, 2);
            modelBuilder.Entity<Payment>()
                .Property(p => p.RefundAmount).HasPrecision(10, 2);

            modelBuilder.Entity<AddOn>()
                .Property(a => a.Price).HasPrecision(10, 2);
            modelBuilder.Entity<BookingAddOn>()
                .Property(ba => ba.PriceAtBooking).HasPrecision(10, 2);

            modelBuilder.Entity<SeasonalRate>()
                .Property(s => s.Multiplier).HasPrecision(5, 2);
            modelBuilder.Entity<PricingFactor>()
                .Property(p => p.Multiplier).HasPrecision(5, 2);
        }
    }
}