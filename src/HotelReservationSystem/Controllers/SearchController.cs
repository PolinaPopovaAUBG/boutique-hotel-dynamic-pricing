using Microsoft.AspNetCore.Mvc;
using HotelReservationSystem.Data;
using HotelReservationSystem.Services;

namespace HotelReservationSystem.Controllers
{
    public class SearchController : Controller
    {
        private readonly AppDbContext _db;
        private readonly PricingService _pricing;


        //When controller is created, ASP.NET injects the database context and pricing service
        public SearchController(AppDbContext db, PricingService pricing)
        {
            _db = db;
            _pricing = pricing;
        }



        //Shows the search form to the guest: /Search
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Results(DateTime checkIn, DateTime checkOut, int guests)
        {
            if (checkOut <= checkIn)
            {
                ViewBag.Error = "Check-in must go before check-out.";
                return View();
            }

            int nights = (checkOut - checkIn).Days;

            //Filter room types for the number of people
            var roomTypes = _db.RoomTypes
                .Where(rt => rt.Capacity >= guests)
                .ToList();

            var results = new List<(string Name, int Capacity, decimal Price)>();

            foreach (var rt in roomTypes)
            {
                //Number of total rooms of this type
                int totalRooms = _db.Rooms.Count(r => r.RoomTypeId == rt.Id);
                if (totalRooms == 0) continue; // if all rooms of that type are taken

                //Count rooms of this type that are already booked during the range
                int bookedRooms = _db.Bookings
                    .Where(b => b.RoomTypeId == rt.Id &&
                           b.Status != "Cancelled" &&
                           b.CheckIn < checkOut &&
                           b.CheckOut > checkIn)
                    .Select(b => b.Id)
                    .Distinct()
                    .Count();

                double occupancyRate = (double)bookedRooms / totalRooms;

                int daysUntilCheckin = (checkIn - DateTime.Today).Days;
                if (daysUntilCheckin < 0) daysUntilCheckin = 0;

                //Calculate final price per night
                decimal pricePerNight = _pricing.CalculatePrice(
                    rt.BasePrice,
                    occupancyRate,
                    daysUntilCheckin,
                    checkIn,
                    nights);

                results.Add((rt.Name, rt.Capacity, pricePerNight));
            }

            //pass data to the view
            ViewBag.CheckIn = checkIn;
            ViewBag.CheckOut = checkOut;
            ViewBag.Guests = guests;
            ViewBag.Nights = nights;

            return View(results);
        }
    }
}
