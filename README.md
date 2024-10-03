# MyTrailer Project

## Overview

MyTrailer is an online trailer rental system that allows customers to:
- Browse and book trailers for short-term and long-term rentals.
- Purchase insurance for trailer rentals.
- Receive notifications for late returns and any applicable fees.
- Book trailers for overnight use via the website.

## Entities

- **Trailer**
  - `Id`: Trailer identifier.
  - `LocationId`: Identifier for the location where the trailer is placed.
  - `Number`: Unique number of the trailer at the specific location.
  - `IsAvailable`: Boolean flag indicating whether the trailer is available for booking.

- **Location**
  - `Id`: Location identifier.
  - `LocationName`: Name of the location.
  - `Address`: Physical address of the location.
  - `PartnerCompany`: The company associated with the location.

- **Customer**
  - `Id`: Customer identifier.
  - `Name`: Full name of the customer.
  - `Email`: Customer’s email for notifications.
  - `PhoneNumber`: Customer’s phone number.
  - `Payments`: Collection of payment records.

- **Booking**
  - `Id`: Booking identifier.
  - `CustomerId`: Identifier for the customer making the booking.
  - `TrailerId`: Identifier for the trailer being booked.
  - `StartDateTime`: Start date and time of the booking.
  - `EndDateTime`: End date and time of the booking.
  - `IsInsured`: Boolean indicating whether insurance was purchased.
  - `IsOverdue`: Boolean indicating if the trailer was returned late.
  - **New Fields**:
    - `IsOverNight`: Boolean indicating if the booking is overnight.
    - `OverNightFee`: The fee for overnight rentals.

## Features

### 1. View Available Trailers at a Selected Location
- As a customer, you can view all available trailers at your selected location.

### 2. Book a Trailer for Short-Term Rental
- Book a trailer for a short-term rental and the system will mark it as unavailable during the booking period.

### 3. Purchase Insurance for Trailer
- You can add insurance to your trailer booking for a fee of 50 Kr.

### 4. Charge a Late Return Fee
- If a trailer is returned late, an excess rental fee is applied to the customer’s account automatically.

### 5. **Book Long-Term Rentals for Overnight Use**
- Customers attempting to book an overnight rental are allowed to complete the booking via the website.
- **New Fields**: Overnight bookings are detected and an `OverNightFee` is applied.
  
  #### Logic:
  - A booking that spans past midnight is flagged as `IsOverNight: true`.
  - An additional `OverNightFee` is applied based on the business logic.

### 6. Track Trailer Usage and Late Returns
- The system tracks trailer usage and automatically calculates any applicable late fees upon return.

### 7. Display Branding on Trailers
- Partner companies can have their branding displayed on the trailers located at their stores.

## Database Schema Changes

The following fields have been added to the **Booking** table to support long-term and overnight bookings:

```sql
ALTER TABLE Booking
ADD IsOverNight BIT NOT NULL DEFAULT 0,
    OverNightFee DECIMAL(18, 2) NOT NULL DEFAULT 0.00;
