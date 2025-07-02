# Hotel Management System
 A modern Hotel Management System built with ASP.NET Core, following best practices in clean architecture, domain-driven design, middleware-based global error handling, and robust transactional workflows.

🚀 Overview
This project is a work in progress backend for a hotel booking platform, inspired by large-scale systems.
It covers core hotel operations:

Room & Reservation Management.

1- Room Types & Facilities.

2- Offers & Promotions.

3- Advanced authorization with fine-grained Feature-based Access Control.

4- Centralized error handling and transaction safety via custom Middleware.

5- Clean separation of DTOs, Entities, Repositories, Services, and Controllers.


⚙️ Key Features

✅ Reservations

Create, update, cancel, and manage reservations

Prevent double-bookings with transactional integrity

Custom validation for dates and statuses

✅ Rooms

CRUD operations for rooms

Change room status dynamically on reservation or cancellation

Manage different room types

✅ Facilities

Add, edit, delete facilities tied to rooms

✅ Offers

Create & apply promotional offers to reservations

✅ Role-based & Feature-based Authorization

Secure endpoints using custom AuthorizeFilter with fine-grained permissions

✅ Robust Middleware

Global error handler returns consistent error DTOs

Transaction middleware wraps database operations in atomic units

✅ Exception Strategy

Uses domain-specific exceptions (NotFoundException, BusinessLogicException) instead of manual null checks

Clean service layer with clear responsibilities



📁 Project Structure

📦 Hotel-Management
 ┣ 📂 Controllers       # API entry points
 ┣ 📂 Services          # Business logic
 ┣ 📂 Repositories      # Data access layer
 ┣ 📂 Models            # Entities, Enums
 ┣ 📂 DTOs              # Data Transfer Objects
 ┣ 📂 Middlewares       # Global error handling, transaction control
 ┣ 📂 Filters           # Custom authorization filters
 ┣ 📜 Program.cs        # Application startup
 ┣ 📜 appsettings.json  # Configuration


