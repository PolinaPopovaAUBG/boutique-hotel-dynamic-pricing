#Boutique Hotel Reservation System with Rule-Based Dynamic Pricing and Revenue Reporting

#Problem Statement 

Boutique hotels struggle to price rooms competitively because static room rates ignore real-time demand, 
seasonality, and booking lead time. Dynamic Pricing system was also intended to increase revenues - collect
more charges when demand increases, and discount room rates during low demand times to incentivize
sales. This system provides a web-based reservation platform where room prices adjust automatically 
based on occupancy, lead time, season, and length of stay.

#User Roles and Services - Functional Requirements

1. Guest

GR-01: Register a new account with email and password
GR-02: Log in and log out securely
GR-03: Search available rooms by date range and room type
GR-04: See dynamically calculated prices in search results
GR-05: Book a room: price is locked at the moment of booking
GR-06: Pay for a booking (simulated payment gateway)
GR-07: Receive email confirmation
GR-08: View one's booking history and upcoming reservations
GR-09: Cancel a reservation (cancellation policy: refund deposit if cancelled 7 days prior reservation)
GR-10: Leave a review and rating after checkout

2. Front Desk

FDR-01: Log in to the staff dashboard
FDR-02: View today's arrivals and departures
FDR-03: Check-out a guest - room is marked as dirty automatically
FDR-04: Assign a room to each arrival
FDR-05: Mark arrival's room as clean
FDR-06: Check-in guest - assign a room
FDR-07: Make a reservation: process on-site payments online
FDR-08: Mark a room out for maintenance
FDR-09: Extend a stay
FDR-10: Add add-ons, such as 2.00pm early arrival, 12.00pm late check-out, pet fee

3. Administrator

AR-01: Log in to the admin dashboard
AR-02: Manage room inventory (add/edit/delete rooms and room types)
AR-03: Set and edit base prices per room type
AR-04: View dynamic pricing breakdown (base × each factor = final)
AR-05: Override a calculated price for a specific booking
AR-06: View revenue reports (daily, weekly, monthly) - export to CSV
AR-07: View occupancy reports (occupancy % over time) - export to CSV
AR-08: Manage staff accounts (create, deactivate, reset password)
AR-09: Refund a reservation - special requests for out of cancellation policy
AR-10: Post charges to a reservation, such as room upgrade charge.

#Non-Functional Requirements

NFR-01: Performance: Search results and price calculation must return in under 2 seconds for up to 100 rooms.
NFR-02: Security: All DB queries parameterized to prevent SQL injection
NFR-03: Usability: Every role has its own dashboard; navigation requires no more than 3 clicks to reach any core service.
NFR-04: Reliability: Booking transactions are atomic - a failed payment must not create a booking.
NFR-05: Maintainability: Architecture follows MVC; each layer (Controller / Service / Data) is independently testable.

#Dynamic=Pricing Algorithm

final_price = base_price
× occupancy_factor
× lead_time_factor
× seasonal_factor
× length_of_stay_factor

#Database Tables

1. Users
2. Guests
3. Staff
4. RoomTypes
5. Rooms
6. Bookings
7. Payments
8. Reviews
9. SeasonalRates
10. PricingFactors


