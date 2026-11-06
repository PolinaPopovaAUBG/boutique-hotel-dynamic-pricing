using Microsoft.AspNetCore.Mvc;
using System;
using HotelReservationSystem.Data;
using HotelReservationSystem.Services;
using HotelReservationSystem.Models;

namespace HotelReservationSystem.Controllers
{
    public class BookingController : Controller
    {
        private readonly AppDbContext _db;
        private readonly PricingService _pricing;

        public BookingController(AppDbContext db, PricingService pricing)
        {
            _db = db;
            _pricing = pricing;
        }
        public IActionResult Create(int roomTypeId, DateTime checkIn, DateTime checkOut)
        {
            //redirect to login if not authentificated
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                //remember where the user wanted to go
                string returnUrl = $"/Booking/Create?roomTypeId={roomTypeId}&checkIn={checkIn:yyyy-MM-dd}&checkOut={checkOut:yyyy-MM-dd}";
                HttpContext.Session.SetString("ReturnUrl", returnUrl);
                return RedirectToAction("Login", "Account");
            }


            //find the room type
            var roomType = _db.RoomTypes.FirstOrDefault(rt => rt.Id == roomTypeId);
            if (roomType == null) return NotFound();

            //recalculate price
            int nights = (checkOut - checkIn).Days;
            if (nights <= 0) return BadRequest("Invalid dates.");

            //for now - estimnated occupancy
            double occupanceRate = 0.60;
            decimal finalPrice = _pricing.CalculatePrice(
                roomType.BasePrice,
                occupanceRate,
                (checkIn - DateTime.Today).Days,
                checkIn,
                nights);

            ViewBag.RoomType = roomType;
            ViewBag.CheckIn = checkIn;
            ViewBag.CheckOut = checkOut;
            ViewBag.Nights = nights;
            ViewBag.FinalPrice = finalPrice;


            return View();
        }

        //add Confirm action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Confirm(int roomTypeId, DateTime checkIn, DateTime checkOut)
        {
            //check whether user is logged in
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            //find the guest record
            var guest = _db.Guests.FirstOrDefault(g => g.UserId == userId);
            if (guest == null)
                return BadRequest("Guest record not found.");

            //find the room type
            var roomType = _db.RoomTypes.FirstOrDefault(rt => rt.Id == roomTypeId);
            if (roomType == null) 
                return NotFound();

            //validate dates
            int nights = (checkOut - checkIn).Days;
            if (nights <= 0)
                return BadRequest("Invalid dates.");

            //recalculate price - the locked price - final
            double occupanceRate = 0.60;
            decimal finalPrice = _pricing.CalculatePrice(
                roomType.BasePrice,
                occupanceRate,
                (checkIn - DateTime.Today).Days,
                checkIn,
                nights);

            //create booking record
            var booking = new Booking
            {
                GuestId = guest.Id,
                RoomTypeId = roomTypeId,
                CheckIn = checkIn,
                CheckOut = checkOut,
                BasePriceAtBooking = roomType.BasePrice,
                FinalPrice = finalPrice * nights,
                PriceLocked = true,
                Status = "Confirmed"
            };

            _db.Bookings.Add(booking);
            _db.SaveChanges();

            //create payment record

            var payment = new Payment
            {
                BookingId = booking.Id,
                Amount = finalPrice * nights,
                Status = "Completed",
                PaidAt = DateTime.UtcNow
            };

            _db.Payments.Add(payment);
            _db.SaveChanges();

            //redirect to success page
            return RedirectToAction("Success", new {id = booking.Id});


        }

        public IActionResult Success(int id)
        {
            var booking = _db.Bookings.FirstOrDefault(b => b.Id == id);
            if (booking == null) 
                return NotFound();
            return View(booking);
        }

        public IActionResult MyBookings()
        {
            //check if logged in
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            //find the guest record
            var guest = _db.Guests.FirstOrDefault(g => g.Id == userId);
            if (guest == null)
                return BadRequest("Guest record not found.");

            //get all bookings for this guest, newest first
            var bookings = _db.Bookings
                .Where(b => b.GuestId == guest.Id)
                .OrderByDescending(b => b.CheckIn)
                .ToList();

            return View(bookings);
        }


    }
}
