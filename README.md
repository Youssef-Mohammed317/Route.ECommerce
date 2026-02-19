# Route E-Commerce API Project

A robust **E-Commerce REST API** built with a clean architecture following the **Onion Architecture** pattern (**7 layers**).  
This project demonstrates a professional approach to building scalable, maintainable, and secure backend systems using modern .NET practices.

🎥 **Video (API Endpoints + Angular Testing):** https://drive.google.com/file/d/1l2i7VBHb2nSRPGbqnWYJm_NIqfqFp3Zf/view?usp=drive_link  
📜 **Certificate:** https://drive.google.com/file/d/19RqIrDpleaDBigK_zqCGFvM50ogRDtXL/view?usp=drive_link  

---

## 🏗 Architecture (Onion Architecture – 7 Layers)

- **Domain:** Entities and repository interfaces  
- **Persistence:** Data layer with EF Core and **two separate Identity contexts**  
- **API:** Main project hosting endpoints  
- **Presentation:** Controllers  
- **Service Abstraction:** Service interfaces  
- **Service Implementation:** Service implementations, mappings, exception specifications  
- **Shared:** DTOs and **Result Pattern**

---

## ✅ Key Features & Technologies

- **Entity Framework Core** for database interactions
- **Redis** for Basket (Cart) entity
- **Repository + Unit of Work** patterns (Generic Repository implementation)
- Dedicated **service layer** for Identity context
- **Specification Pattern** for flexible querying
- **Global Exception Handling**, later refactored into a consistent **Result Pattern**
- **JWT Authentication** for secure access
- **Stripe Integration** for card payments
- Fully tested with an **Angular front-end**  
  *(Video includes both the API endpoints and the Angular project testing flow)*

---

## 🎯 Project Goal

Build a scalable and maintainable E-Commerce backend using clean architecture principles, strong separation of concerns, secure authentication, and production-style integrations (Redis, Stripe, Specifications, Result Pattern).
