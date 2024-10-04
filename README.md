# MyTrailer Documentation

## Domain

### Entities

- **Trailer**
  - `Id`: Trailer identifier.
  - `LocationId`: Identifier for the location where the trailer is placed.
  - `Number`: Unique number of the trailer at the specific location.
  - `IsAvailable`: Boolean flag indicating whether the trailer is available for booking.

- **Location**
  - `Id`: Location identifier.
  - `LocationName`: Name of the location (e.g., "Jem og Fix").
  - `Address`: Physical address of the location.
  - `PartnerCompany`: The company that is associated with the location (for branding and other business agreements).

- **Customer**
  - `Id`: Customer identifier.
  - `Name`: Full name of the customer.
  - `Email`: Customer’s email for notifications and communication.
  - `PhoneNumber`: Customer’s phone number.
  - `Payments`: Collection of payment records for the customer.

- **Booking**
  - `Id`: Booking identifier.
  - `CustomerId`: Identifier for the customer making the booking.
  - `TrailerId`: Identifier for the trailer being booked.
  - `StartDateTime`: Start date and time of the booking.
  - `EndDateTime`: End date and time of the booking.
  - `IsInsured`: Boolean indicating whether insurance was purchased.
  - `IsOverdue`: Boolean indicating if the trailer was returned late.

We started out with the Customer entity, but as we started to code we decided to use the Asp Net Identity which basically holds the same user information as the Customer. 

## Value Objects

- **PaymentDetails**
  - `PaymentMethod`: Describes the method of payment used (e.g., "Credit Card").
  - `PaymentHistory`: Stores past payment transactions as a list.
  - **Behavior**: Tracks payment history and processes customer payments.

- **Insurance**
  - `BookingId`: The booking associated with this insurance.
  - `Fee`: Fixed fee for the insurance (50 Kr).
  - `CoveragePeriod`: The period for which the insurance is valid (e.g., 24 hours).
  - **Behavior**: Tracks insurance status and calculates coverage.

## Aggregates

- **Customer Aggregate**:
  - Attributes: Contains Customer, Booking, and Payments.
  - **Behavior**: Allows a customer to manage their bookings, payments, and insurance purchases.

- **Location Aggregate**:
  - Attributes: Contains Location and its associated Trailers.
  - **Behavior**: Manages the available trailers for a location, and checks availability based on bookings.

- **Trailer Aggregate**:
  - Attributes: Contains Trailer and its Bookings.
  - **Behavior**: Tracks trailer availability and manages bookings.

![Trailer Rental System](./TrailerDiagram.png)

## User Stories

### User Story 1: View Available Trailers at a Selected Location

- **As a customer**, I want to view available trailers at my selected location, so that I can choose one for rental.
- **Given** the customer has opened the trailer booking page in the app,
- **When** they select a location,
- **Then** the system should display all available trailers for that location.

### User Story 2: Book a Trailer for Short-Term Rental

- **As a customer**, I want to book a trailer for short-term rental, so that I can move my items.
- **Given** a customer has selected a trailer and a valid time slot,
- **When** they confirm the booking,
- **Then** the system should register the booking and mark the trailer as unavailable for the chosen period.

### User Story 3: Purchase Insurance for the Trailer

- **As a customer**, I want to purchase insurance when booking a trailer, so that I am covered in case of damage.
- **Given** a customer is booking a trailer,
- **When** they are prompted to add insurance,
- **Then** the system should allow them to add insurance for a fee of 50 Kr.

### User Story 4: Charge a Late Return Fee

- **As a customer**, I want to receive a late return fee if I return the trailer after the scheduled time, so that I know the extra cost.
- **Given** a customer has returned a trailer after the booking's end time,
- **When** the system processes the return,
- **Then** it should apply an excess rental fee to the customer’s account.

### User Story 5: Book Long-Term Rentals via Website

- **As a customer**, I want to book long-term rentals for overnight use via the website, so that I can use the trailer overnight.
- **Given** a customer is trying to book an overnight rental,
- **When** they attempt to do this via the app,
- **Then** the system should direct them to the website to complete the booking.

### User Story 6: Display Branding on Trailers

- **As a location partner**, I want to have my branding displayed on trailers, so that customers can recognize my store.
- **Given** a trailer is located at a partner’s site,
- **When** a customer views the trailer details in the app,
- **Then** the system should display the partner’s branding along with MyTrailer branding.

### User Story 7: Track Trailer Usage and Late Returns

- **As a system administrator**, I want to track trailer usage and late returns, so that I can ensure trailers are available and customers are charged correctly.
- **Given** a trailer has been rented out,
- **When** the trailer is returned,
- **Then** the system should log the return time and calculate any late fees if applicable.




# OLA 4 - Microservice Architecture for MyTrailer System

For OLA 4, we started by building the system as a monolith to get a complete working application. Then, we created two separate Web API projects to show how we could transition to a microservice-based architecture. However, simply splitting it into microservices isn't practical without significant changes. Each microservice would need its own dedicated database, and we would have to redesign the system to ensure proper communication between services, handle data consistency, and implement distributed transactions. Next week, when we receive the exam project, we'll focus on building services from the ground up with this in mind.

## 1. Booking Service
**Responsibilities:** Manages trailer bookings and returns.  
**Database:** A database specifically for storing booking information such as `Bookings`, `TrailerId`, `UserId`, `StartDateTime`, `EndDateTime`, `LateFees`, and `ActualReturnTime`.

**Endpoints:**
- `POST /bookings`: Create a booking.
- `PUT /bookings/{id}`: Update or return a trailer.
- `GET /bookings/user/{userId}`: Fetch all bookings for a user.

## 2. Trailer Service
**Responsibilities:** Manages trailer availability, location, and status.  
**Database:** Stores information about `Trailers`, `LocationId`, `IsAvailable`, and `Number`.

**Endpoints:**
- `GET /trailers/{locationId}`: Get available trailers at a location.
- `PUT /trailers/{id}`: Update trailer availability.

## 3. Location Service
**Responsibilities:** Manages trailer locations and partner information.  
**Database:** Stores information about `Locations`, `PartnerCompanies`, and branding images.

**Endpoints:**
- `GET /locations`: Fetch all locations.
- `GET /locations/{id}`: Fetch a specific location.

## 4. User Service
**Responsibilities:** Manages user authentication and roles using ASP.NET Identity.  
**Database:** User management will be handled via the ASP.NET Identity tables. You wouldn't create a new database here but rely on the existing Identity schema.

**Endpoints:**
- `POST /users/register`: User registration.
- `POST /users/login`: User login.

## 5. Payment Service
**Responsibilities:** Manages payments related to bookings (insurance or late fees).  
**Database:** Stores payment-related data such as `Payments`, `BookingId`, `Amount`, and `PaymentMethod`.

**Endpoints:**
- `POST /payments`: Record a new payment.
- `GET /payments/{bookingId}`: Fetch payment details for a booking.

