using Microsoft.AspNetCore.Mvc;
using HotelReservationSystem.Data;
using HotelReservationSystem.Models;
using System.Text;

namespace HotelReservationSystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _db;

        public AdminController(AppDbContext db)
        {
            _db = db;
        }

        //only allow admin 
        private bool IsAdminLoggedIn()
        {
            var role = HttpContext.Session.GetString("Role");
            return role == "Admin";
        }

       //get/admin
        public IActionResult Index()
        {
            if (!IsAdminLoggedIn())
                return RedirectToAction("Login", "Account");

            //calculate revenue = sum of all reservations (completed payments)
            var totalRevenue = _db.Payments
                .Where(p => p.Status == "Completed")
                .Sum(p => p.Amount);

            //recent bookings displayed
            var recentBookings = _db.Bookings
                .OrderByDescending(b=> b.CreatedAt)
                .Take(10)
                .ToList();

            ViewBag.TotalRevenue = totalRevenue;
            ViewBag.RecentBookings = recentBookings;


            return View();
        }

        //get/admin/export revenue
        public IActionResult ExportRevenue()
        {
            if (!IsAdminLoggedIn())
                return RedirectToAction("Login", "Account");

            //get all payments to export
            var payments = _db.Payments.OrderByDescending(p => p.PaidAt).ToList();

            //build the csv string
            var csv = new StringBuilder();
            csv.AppendLine("PaymentID,BookingID,Amount,Status.PaidAt");

            foreach (var p in payments)
            {
                csv.AppendLine($"{p.Id},{p.BookingId},{p.Amount},{p.Status},{p.PaidAt}");

            }

            //return as a downloanable file
            var bytes = Encoding.UTF8.GetBytes(csv.ToString());
            return File(bytes, "text/csv", "Hotel_Revenue_Report.csv");
        }

        //get/admin/PricingBreakdown
        public IActionResult PricingBreakdown()
        {
            if (!IsAdminLoggedIn())
                return RedirectToAction("Login", "Account");

            //get room types to show their base prices
            var roomTypes = _db.RoomTypes.ToList();

            //get pricing factors to display lookup tables
            var factors = _db.PricingFactors.ToList();
            var seasonrates = _db.SeasonalRates.ToList();

            ViewBag.RoomTypes = roomTypes;
            ViewBag.Factors = factors;
            ViewBag.SeasonalRates = seasonrates;

            return View();
        }
    }
}
