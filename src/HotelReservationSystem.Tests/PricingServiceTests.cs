using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelReservationSystem.Services;
using Xunit;

// unit tests test if each piece of the algorithm works as expected

namespace HotelReservationSystem.Tests
{
    public class PricingServiceTests
    {
        // One service will be used in all tests

        private readonly PricingService _pricing = new PricingService();
        
        [Fact]
        public void Test_Standard_price()
        {
            //Final_price = base_price * occupancy_factor * lead_time_factor * seasonal_factor * length_of_stay_factor

            decimal result = _pricing.CalculatePrice(100m, 0.60, 14, new DateTime(2026, 6, 1), 2);

            Assert.Equal(105.00m, result);

        }

        [Fact]
        public void Test_High_Occupancy()
        {
            decimal result = _pricing.CalculatePrice(100m, 0.95, 14, new DateTime(2026, 6, 1), 2);

            Assert.Equal(157.50m, result);
        }

        [Fact]
        public void Test_Last_Minute_Booking()
        {
            decimal result = _pricing.CalculatePrice(100m, 0.60, 2, new DateTime(2026, 6, 1), 2);

            Assert.Equal(126.00m, result);
        }

        [Fact]
        public void Test_High_Season()
        {
            decimal result = _pricing.CalculatePrice(100m, 0.60, 14, new DateTime(2026, 7, 15), 2);

            Assert.Equal(125.00m, result);
        }

        [Fact]
        public void Test_Long_Stay()
        {
            decimal result = _pricing.CalculatePrice(100m, 0.60, 14, new DateTime(2026, 6, 1), 7);

            Assert.Equal(89.25m, result);
        }

        [Fact]
        public void Test_Price_Floor()
        {
            //The algorithm calculates a price of 58.52, but the price floor is 70.00, so should give out 70.00 as a result
            decimal result = _pricing.CalculatePrice(100m, 0.30, 45, new DateTime(2026, 2, 10), 7);

            Assert.Equal(70.00m, result);
        }

        [Fact]
        public void Test_Price_Ceiling()
        {
            //The algorithm calculates a price of 225.00, whereas the price ceiling is 300.00, so it should not exceed it 
            decimal result = _pricing.CalculatePrice(100m, 0.95, 2, new DateTime(2026, 7, 15), 1);

            Assert.True(result <= 300m);
        }

        [Fact]
        public void Test_Rounding()
        {
            decimal result = _pricing.CalculatePrice(99.99m, 0.60, 14, new DateTime(2026, 6, 1), 1);

            Assert.Equal(104.99m, result);
        }

        [Fact]
        public void Test_December_20_Is_High_Season()
        {
            decimal result = _pricing.CalculatePrice(100m, 0.60, 14, new DateTime(2026, 12, 20), 2);

            Assert.Equal(125.00m, result);
        }

        [Fact]
        public void Test_January_6_Is_Low_Season()
        {
            decimal result = _pricing.CalculatePrice(100m, 0.60, 14, new DateTime(2026, 1, 6), 2);

            Assert.Equal(90.00m, result);
        }


    }
}
