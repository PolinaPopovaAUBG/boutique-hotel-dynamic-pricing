using HotelReservationSystem.Models;

namespace HotelReservationSystem.Data
{
    public class DbSeeder
    {
        public static void Seed(AppDbContext db)
        {
            //call this method if database is empty
            //add rooms types if empty
            if (!db.RoomTypes.Any())
            {
                var types = new List<RoomType>
                {
                    new RoomType
                    {
                        Name = "Standard King", View = "Standard", BedType = "King",
                        Capacity = 2, BasePrice = 120m
                    },
                    new RoomType
                    {
                        Name = "Pool View Two Queens", View = "Pool", BedType = "Two Queens",
                        Capacity = 4, BasePrice = 180m
                    },
                    new RoomType
                    {
                        Name = "Harbor View King", View = "harbor", BedType = "King",
                        Capacity = 2, BasePrice = 220m
                    },
                    new RoomType
                    {
                        Name = "Breakwater Two Doubles", View = "Breakwater", BedType = "Two Doubles",
                        Capacity = 4, BasePrice = 250m
                    }
                };
                db.RoomTypes.AddRange(types);
                db.SaveChanges();
            }
            //if none exist - add rooms
            if (!db.Rooms.Any())
            {
                //get the id's
                var standardKing = db.RoomTypes.First(t => t.Name == "Standard King").Id;
                var poolQueens = db.RoomTypes.First(t => t.Name == "Pool View Two Queens").Id;
                var harborKing = db.RoomTypes.First(t => t.Name == "Harbor View King").Id;
                var breakwaterDoubles = db.RoomTypes.First(t => t.Name == "Breakwater Two Doubles").Id;

                var rooms = new List<Room>();

                //add 5 Standard King rooms
                for (int i = 1; i <= 5; i++)
                    rooms.Add(new Room
                    {
                        RoomNumber = $"10{i}",
                        RoomTypeId = standardKing,
                        Status = "Available"
                    });

                for (int i = 1; i <= 5; i++)
                    rooms.Add(new Room
                    {
                        RoomNumber = $"20{i}",
                        RoomTypeId = poolQueens,
                        Status = "Available"
                    });

                for (int i = 1; i <= 4; i++)
                    rooms.Add(new Room
                    {
                        RoomNumber = $"30{i}",
                        RoomTypeId = harborKing,
                        Status = "Available"
                    });

                for (int i = 1; i <= 5; i++)
                    rooms.Add(new Room
                    {
                        RoomNumber = $"40{i}",
                        RoomTypeId = breakwaterDoubles,
                        Status = "Available"
                    });

                db.Rooms.AddRange(rooms);
                db.SaveChanges();

            }
        }
    }
}
