using System.Diagnostics.Eventing.Reader;

namespace HotelReservationSystem.Services
{

    //This class contains rule-based dynamic pricing algorithm
    //It does not touch the batabase
    //Calculated the final price from the base price
    public class PricingService
    {
        public decimal CalculatePrice(
            decimal basePrice,
            double accupancyRate,
            int daysUntilCheckin,
            DateTime checkInDate,
            int nights)
        {
            decimal price = basePrice;

            // occupancy factor
            if (accupancyRate <= 0.50)
                price = price * 0.85m;
            else if (accupancyRate <= 0.80)
                price = price * 1.00m;
            else if (accupancyRate <= 0.90)
                price = price * 1.25m;
            else
                price = price * 1.50m;

            // lead time factor
            if (daysUntilCheckin >= 60)
                price = price * 0.90m;
            else if (daysUntilCheckin >= 30)
                price = price * 0.95m;
            else if ((daysUntilCheckin >= 7))
                price = price * 1.00m;
            else
                price = price * 1.20m;

            // seasonal factor
            int month = checkInDate.Month;
            int day = checkInDate.Day;

            bool isHighSeason = (month == 7 || month == 8) ||
                                (month == 12 && day >= 20) ||
                                (month == 1 && day <= 5);
            bool isMidSeason = (month >= 5 && month <= 6) ||
                               (month >= 9 && month <= 10);

            if (isHighSeason) 
                price = price * 1.25m;
             else if (isMidSeason)
                    price = price * 1.05m;
                else
                    price = price * 0.90m;

                // length of stay factor
                if (nights >= 7)
                    price = price * 0.85m;
                else if (nights >= 4)
                    price = price * 0.90m;

                // price floor and ceiling
                decimal floor = basePrice * 0.70m;
                decimal ceiling = basePrice * 3.00m;

                if (price < floor)
                    price = floor;
                if (price > ceiling)
                    price = ceiling;

                return Math.Round(price, 2);
        }
    }
}
