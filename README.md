# 🏨 Hotel Management System

## 📌 Overview
*Hotel Management System* is a full-featured web application that simulates a real-world hotel booking experience similar to platforms like *Booking.com*. It allows users to:
- Search and book rooms with available services (Wi-Fi, Sauna, etc.).
- Modify their booking dates.
- Submit reviews about the hotel and its services after checkout.
- View dynamic room pricing based on the current season (summer, winter, etc.).
- Explore different user roles (Admin, Staff, Customer) with dynamic access control.

---

## 🧩 Key Features

### 🔐 Authentication & Authorization
- Fully implemented login and registration system.
- Role-based access control (Admin, Hotel Staff, Customer).
- Dynamic role management.

### 🏨 Room Booking System
- Book available rooms and select optional services.
- Modify or cancel booking dates.
- Automatic room availability updates.

### 🧾 Room Services
Each room can include:
- Free or paid Wi-Fi.
- Sauna.
- Room service.
- Additional customizable amenities.

### 💸 Seasonal Pricing System
- Room pricing is dynamically calculated based on the current *season/weather*.
- Example: Higher prices in *summer, lower in **winter*.

### 👥 Profiles
- *User Profile:* View and manage personal bookings and reviews.
- *Staff Profile:* Hotel employees can manage room services and booking requests.
- *Hotel Profile:* Hotel overview, services, reviews, and room status.

### 📝 Review System
- Users can review their *stay experience* and rate hotel services after checkout.

---

## 🧪 Architecture & Code Practices

- *Clean Architecture:* Clear separation of concerns using:
  - DTOs (Data Transfer Objects)
  - Entities
  - Repositories
  - Services
  - Controllers
- *Middleware:* Centralized error handling and transaction safety.
- *Scalable Design:* Easy to extend with new features or services.

---

## 🛠️ Tech Stack
- *Backend:* .NET (ASP.NET Core)
- *Database:* SQL Server (or any relational DB)
- *Authentication:* JWT / Identity
- *Architecture Style:* RESTful API

---

## 🧑‍💻 Contributors
- [Mero0077](https://github.com/Mero0077)

---

## 📷 Screenshots (Optional)
> Add images or GIFs here to show room booking, pricing, review, etc.

---

## 🚀 How to Run the Project

1. Clone the repository:
   ```bash
   git clone https://github.com/Mero0077/Hotel-Management.git
