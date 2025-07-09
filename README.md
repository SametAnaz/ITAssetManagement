# IT Asset Management System

An ASP.NET Core application for managing IT assets in an organization.

## Project Structure

The project follows a layered architecture:

1. **ITAssetManagement.Core** (Core/Entities Layer)
   - Contains domain entities and interfaces
   - All other layers depend on this layer

2. **ITAssetManagement.DataAccess** (Data Access Layer)
   - Handles database operations
   - Contains repositories and DbContext
   - Depends only on Core layer

3. **ITAssetManagement.Business** (Business Logic Layer)
   - Contains business logic and services
   - Depends on Core and DataAccess layers

4. **ITAssetManagement.Web** (Presentation Layer)
   - MVC Controllers and Views
   - User interface components
   - Depends on Core and Business layers

## Getting Started

1. Clone the repository
2. Set up the database connection string in `appsettings.json`
3. Run migrations to create the database
4. Run the application

## Features

- Laptop inventory management
- Asset assignment tracking
- User management
- Detailed logging of asset changes