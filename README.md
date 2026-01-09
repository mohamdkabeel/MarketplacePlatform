#  E-Commerce Platform API

A scalable and secure E-Commerce RESTful API built using **ASP.NET Core Web API (.NET 8)**.
The project is designed following **Clean Architecture principles** and provides a complete backend solution
for managing products, categories, orders, reviews, users and real time notifications.

---

##  Tech Stack
- ASP.NET Core Web API (.NET 8)
- Entity Framework Core
- SQL Server
- ASP.NET Core Identity
- JWT Authentication & Authorization
- SignalR (Real time Notifications)
- Swagger / OpenAPI

---

##  Features
- User Authentication & Authorization using JWT
- Role Management (Admin / Seller / Customer)
- Product Management (CRUD)
- Category Management
- Shopping Cart & Order System
- Product Reviews & Ratings
- Email Notifications
- Real-time Notifications using SignalR
- Pagination, Search & Filtering
- Clean Architecture (Repository & Service Pattern)

---

##  Project Architecture
The project follows Clean Architecture with clear separation of concerns:

- **Domain Layer**
  - Entities
  - Interfaces

- **Application Layer**
  - DTOs
  - Service Contracts
  - Business Logic

- **Infrastructure Layer**
  - Entity Framework Core
  - Database Context
  - Services (Email, Notifications, etc.)

- **API Layer**
  - Controllers
  - Middleware
  - Authentication & Authorization

---

##  API Documentation (Swagger)
All API endpoints are documented using **Swagger** for easy testing and integration.

---

##  API Endpoints Preview (Swagger)

> Below are some screenshots of the API endpoints documented in Swagger.

### Authentication & Users & Cart
(<img width="1505" height="919" alt="Screenshot 2026-01-09 134010" src="https://github.com/user-attachments/assets/c44d2e8c-d4a4-4ed7-81ff-e2d110a39026" />)


### Categories & Orders & Products 
(<img width="1263" height="908" alt="Screenshot 2026-01-09 134034" src="https://github.com/user-attachments/assets/32c32316-59da-45e8-945b-fa8063dd953b" />)


### Reviews & WishList
(<img width="1267" height="341" alt="Screenshot 2026-01-09 134051" src="https://github.com/user-attachments/assets/67bc58de-732f-4262-85d5-ded129031a23" />)

---

##  Getting Started

### Prerequisites
- .NET 8 SDK
- SQL Server
- Visual Studio or VS Code

---


