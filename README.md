# 🚗 Car Rental API

A modern ASP.NET Core Web API for managing car rentals. It supports user registration, authentication with JWT, car listing, and reservation management. Built with Entity Framework Core and follows RESTful principles using `ActionResult<T>` responses.

## ✨ Features

- 🔐 User Registration & Login (JWT Authentication)
- ✉️ Email Verification / Sending
- 🚘 Car Management (Add, List, Filter)
- 📅 Car Reservations
- 🛡️ Role-based Access (Admin, User)
- 📦 EF Core & Code-First Migrations
- ⚙️ Structured API Responses using `ActionResult<T>`
- 🧯 Centralized Error Logging Middleware

---

## 🧱 Tech Stack

- **ASP.NET Core Web API**
- **Entity Framework Core**
- **SQL Server** (or other EF-compatible DBs)
- **AutoMapper**
- **JWT Authentication**
- **Swagger / OpenAPI**

---

## 🚀 Getting Started

### 📦 Prerequisites

- [.NET SDK 7.0 or newer](https://dotnet.microsoft.com/)
- SQL Server or SQLite
- Visual Studio / VS Code

### 🔧 Setup

```bash
# Clone the repo
git clone https://github.com/tseretelii/CarRental.git
cd car-rental-api

# Restore dependencies
dotnet restore

# Apply EF Core migrations
dotnet ef database update

# Run the application
dotnet run
```

API will be available at: `https://localhost:5001` or `http://localhost:5000`  
Swagger UI: `https://localhost:5001/swagger`

---

## 🔐 Authentication

Authentication is handled via **JWT Bearer Tokens**.

1. Register a user: `POST /api/users/register`
2. Login: `POST /api/users/login`
3. Use the token in headers:
   ```
   Authorization: Bearer <your_token>
   ```

---

## ⚠️ Error Logging Middleware

This API uses a **custom error-handling middleware** that:
- Catches **unhandled exceptions** across the API.
- Logs the exception details in a `.txt` file.
- Creates a folder on the **Desktop** named:

```
/Desktop/ExceptionsLog/
```

Each error is logged as a file named after the timestamp and endpoint.  
If logging fails (e.g. due to missing permission), the error is silently ignored.

---

## ✉️ Email Sending Setup

The API uses **SMTP (Gmail)** to send email verifications.  
To enable this, create a folder on your desktop:

```
/Desktop/EmailConfig/
```

Inside this folder, create a file named:

```
EmailAcc.json
```

With the following structure:

```json
{
  "MailAddress": "your-email@gmail.com",
  "Password": "your-app-password"
}
```

> ⚠️ You must use a valid Gmail address and **app password** (if 2FA is enabled).  
> If this file is missing or contains invalid credentials:
- Email will not be sent
- An error will be logged to the `ExceptionsLog` folder

---

## 📚 API Endpoints Overview

> Explore and test all endpoints via Swagger UI.

---

## 🛠 Configuration

Edit `appsettings.json` to configure your DB and JWT settings:

```json
"ConnectionStrings": {
  "DefaultConnection": "Your SQL Server connection string"
},
"JWTOptions": {
  "Secret": "your-secret-key",
  "Issuer": "your-app",
  "Audience": "your-app-users"
}
```

---

## 📂 Project Structure

```
CarRentalAPI/
├── Controllers/
├── Models/
│   ├── Entities/
│   └── DTOs/
├── Services/
├── Interfaces/
├── Middleware/
├── Configs/
├── Program.cs
└── README.md
```

---

## 🧪 Testing

Test the API using:

- Swagger UI (`/swagger`)
- Postman / Thunder Client
- Optional: Write and run unit/integration tests

---

## 🙌 Credits

Created with ❤️ by Gigi Tsereteli  
Includes custom error logging and email verification utilities built for real-world deployment needs.
