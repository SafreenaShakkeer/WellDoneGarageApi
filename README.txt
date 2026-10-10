# Garage Management System

A full-stack web application designed to simplify and manage the daily operations of a bike service garage. The system aims to centralize customer and vehicle records, inventory management, service job cards, billing, and reporting in one place.

The project is being developed using **ASP.NET Core Web API** for the backend and **Angular** for the frontend, with **SQL Server** as the database.

## Technology Stack

- **Backend:** C#, ASP.NET Core Web API
- **ORM:** Entity Framework Core
- **Database:** Microsoft SQL Server
- **Frontend:** Angular
- **API Testing:** Swagger / OpenAPI
- **Version Control:** Git and GitHub

## Implemented Features

### Customer Management
- Create, retrieve, update, and soft-delete customer records.
- Validate customer data through DTOs.
- Automatically track creation and update timestamps.
- Apply global query filters to exclude soft-deleted customers.

### Vehicle Management
- Manage vehicles associated with customers.
- Maintain vehicle registration numbers and model details.
- Prevent duplicate vehicle registration numbers.
- Validate customer relationships before creating or updating vehicles.
- Support vehicle ownership transfer.

### Inventory Management
- Create, retrieve, update, and soft-delete inventory items.
- Validate item details and prevent negative unit prices.
- Track current stock quantities.

### Stock Transaction Management
- Record stock additions and stock deductions.
- Support stock transaction types: `IN` and `OUT`.
- Associate transactions with relevant references, such as purchases or jobs.
- Prevent stock deductions when sufficient inventory is unavailable.
- Keep stock transaction records immutable through the API.

### Job Card Management
- Job card API development is in progress.
- Define relationships between customers, vehicles, and job cards.
- Track service status using `Pending`, `InProgress`, and `Completed` states.

## Features Under Development

- **Job Card Management:** Complete service job card workflows, mechanic assignment, complaint tracking, and completion details.
- **Searching and Filtering:** Search records and filter results based on relevant fields.
- **Pagination:** Implement server-side pagination for efficient retrieval as the number of records grows.
- **Billing and Invoicing:** Generate invoices for garage services and spare parts.
- **Payment Management:** Track payments against invoices.
- **Reporting:** Provide useful summaries of garage operations, services, sales, and inventory.
- **Angular Frontend:** Develop responsive interfaces and integrate them with the backend APIs.
- **Authentication and Authorization:** Introduce secure login and role-based access for administrators and mechanics.
- **WhatsApp Notifications:** Explore service completion notifications, vehicle collection reminders, and periodic maintenance reminders.

## Project Architecture

The application follows a client-server architecture.

1. **Angular Client:** Provides the user interface and sends HTTP requests.
2. **ASP.NET Core Web API:** Handles incoming requests, validation, and application operations.
3. **Entity Framework Core:** Manages communication between the API and the database.
4. **SQL Server:** Stores customers, vehicles, inventory items, stock transactions, job cards, and related records.

## Design and Development Practices

- RESTful API design
- DTO-based request and response handling
- Entity Framework Core relationships and database migrations
- Data validation and appropriate HTTP status codes
- Soft deletion where applicable
- Foreign key constraints and unique constraints
- Separation of API models from database entities
- Git-based version control

## Project Goals

- Build a practical garage management solution based on real-world business requirements.
- Strengthen backend development skills using C# and ASP.NET Core.
- Understand database design, entity relationships, and inventory consistency.
- Implement maintainable APIs and efficient data retrieval.
- Gain hands-on experience integrating Angular with a .NET backend.

## Current Status

**Actively in development.**

The core Customer, Vehicle, Item, and Stock Transaction APIs have been implemented. Job Card Management and additional application features are being developed incrementally.

This project is intended as a practical learning and portfolio project, with an emphasis on applying software development principles to a real-world use case.