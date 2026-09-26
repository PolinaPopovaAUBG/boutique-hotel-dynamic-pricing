# Dynamic Pricing Algorithm Specification

#Purpose

The purpose is to calculate the price of a room for a given date range based on 
real-time demand and booking conditions.

The goal is to maximize revenue during high demand times and to incentivize sales
during low demand times.

#Formula

final_price = base_price
× occupancy_factor
× lead_time_factor
× seasonal_factor
× length_of_stay_factor

where base-price is the standard nightly rate for the room type set by the Administrator
and each factor is a decimal multiplier derived from lookup tables below.

- Price increases with demand (occupancy, lead time).
- Price increases with seasonal peak.
- Discounts incentivize longer stays and early bookings.

The final price is rounded to 2 decimal places as a currency.

1. Occupancy % -> Multiplier 
0–50% -> 0.85
50–70% -> 1.00
70–85% -> 1.15
85–95% -> 1.30
95–100% -> 1.50

2. Lead Time Factor
Days until check-in -> Multiplier
60+ -> 0.90
30–59 -> 0.95
7–29 -> 1.00
0–6 -> 1.20

3. Seasonal Factor
Period -> Multiplier
High: Jul–Aug, Dec 20 – Jan 5 -> 1.25 -> high demand due to holidays
Normal: May–Jun, Sep–Oct -> 1.05 -> moderate demand 
Low: Nov–Mar (excl. holidays) -> 0.90 -> off-season discount

4. Length of stay factor
Nights -> Multiplier
1–3 -> 1.00
4–6 -> 0.90 
7+ -> 0.85

5. Edge Cases

1)Price floor: final price cannot fall below 70% of base price. 
2)Price ceiling: final price cannot exceed 300% of base price. 
3)Override: administrator can override the calculated price for a specific booking; override is logged in AuditLog. 
4)Cancellation: if cancelled >=7 days before check-in, deposit is refunded; otherwise forfeited.
5)Occupancy calculation: occupancy is calculated per date across all bookable rooms of the same room type.

6. Where the Algorithm Appears in the System

1)Guest search results: live price shown next to each room type.
2)Booking confirmation page: price is recalculated, shown, and locked on submit.
3)Administrator pricing breakdown: shows base × each factor = final.
4)Administrator reports: average price realized vs. base price, to show revenue impact.




