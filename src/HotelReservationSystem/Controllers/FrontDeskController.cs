using Microsoft.AspNetCore.Mvc;
using HotelReservationSystem.Data;
using HotelReservationSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelReservationSystem.Controllers
{
    public class FrontDeskController : Controller
    {
        private readonly AppDbContext _db;

        public FrontDeskController(AppDbContext db)
        {
            _db = db;
        }

        //only allow logged-in staff / admin

        private bool IsStaffLoggedIn()
        {
            var role = HttpContext.Session.GetString("Role");
            return role == "FrontDesk" || role == "Admin";
        }

        //get: front desk

        public IActionResult Index()
        {
            if (!IsStaffLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            var today = DateTime.Today;

            //arrivals: if check-in is today, status is confirmed
            var arrivals = _db.Bookings
                .Where(b => b.CheckIn.Date == today && b.Status == "Confirmed")
                .ToList();

            //stayovers - checked-in reservations
            var inHouse = _db.Bookings
                .Include(b => b.AssignedRoom)
                .Where(b => b.CheckOut.Date > today && b.Status == "CheckedIn")
                .ToList();

            //departures: if check-out is today, status is checked-in
            var departures = _db.Bookings
                .Include(b => b.AssignedRoom) // Keep the fix from earlier!
                .Where(b => b.CheckOut.Date <= today && b.Status == "CheckedIn")
                .ToList();

            ViewBag.Arrivals = arrivals;
            ViewBag.InHouse = inHouse;
            ViewBag.Departures = departures;

            return View();
        }

        //get: /frontdesk/checkin/{id}
        public IActionResult CheckIn(int id)
        {

            if (!IsStaffLoggedIn())
                return RedirectToAction("Login", "Account");

            var booking = _db.Bookings.FirstOrDefault(b=> b.Id == id);
            if (booking == null)
                return NotFound();

            //available rooms of the same room type
            var availableRooms = _db.Rooms
                .Where(r => r.RoomTypeId == booking.RoomTypeId && r.Status == "Available")
                .ToList();

            ViewBag.Booking = booking;
            ViewBag.RoomTypeName = _db.RoomTypes
                .Where(rt => rt.Id == booking.RoomTypeId)
                .Select(rt => rt.Name)
                .FirstOrDefault();
            ViewBag.AvailableRooms = availableRooms;

            return View();
        }

        //post: /frontdesk/checkin/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CheckIn(int id, int selectedRoomId)
        {
            if (!IsStaffLoggedIn())
                return RedirectToAction("Login", "Account");

            var booking = _db.Bookings.FirstOrDefault(b => b.Id == id);
            if (booking == null)
                return NotFound();

            var room = _db.Rooms.FirstOrDefault(r=> r.Id == selectedRoomId);
            if (room == null)
                return BadRequest("Room not found.");

            //assign room and mark booking as check-in
            booking.AssignedRoomId = room.Id;
            booking.Status = "CheckedIn";
            room.Status = "Occupied";

            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        //post: /frontdesk/checkout/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult CheckOut(int id)
        {
            if (!IsStaffLoggedIn())
                return RedirectToAction("Login", "Account");

            var booking = _db.Bookings.FirstOrDefault(b => b.Id == id);
            if (booking == null)
                return NotFound();

            //mark booking as check-out
            booking.Status = "CheckedOut";

            //mark room as dirty, so housekeeping cleans it
            if (booking.AssignedRoomId != null )
            {
                var room = _db.Rooms.FirstOrDefault(r => r.Id == booking.AssignedRoomId);
                if (room != null)
                    room.Status = "Dirty";
            }

            _db.SaveChanges();

            return RedirectToAction("Index");
        }


    }
}
