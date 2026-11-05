using Microsoft.AspNetCore.Mvc;
using System;
using HotelReservationSystem.Data;
using HotelReservationSystem.Services;

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
    }
}
