# IT Asset Management System

An ASP.NET Core MVC application for managing IT assets in an organization. This system provides comprehensive laptop inventory management, user management, and assignment tracking capabilities.

## 🏗️ Project Architecture

The project follows a clean layered architecture with separation of concerns:

### Architecture Layers:
- **Controllers** (Presentation Layer) - MVC Controllers handling HTTP requests
- **Services** (Business Logic Layer) - Business logic and data processing
- **Repositories** (Data Access Layer) - Data persistence using Repository pattern
- **Models** (Domain Layer) - Entity models and data structures

### Design Patterns:
- **Repository Pattern** for data access abstraction
- **Service Layer Pattern** for business logic encapsulation
- **Dependency Injection** for loose coupling
- **Soft Delete Pattern** for data safety

## 🛠️ Technology Stack

- **Framework**: ASP.NET Core 8.0 MVC
- **Database**: SQL Server (LocalDB for development)
- **ORM**: Entity Framework Core 8.0.11
- **UI**: Bootstrap 5, FontAwesome Icons
- **Barcode Generation**: ZXing.Net with ImageSharp integration
- **Image Processing**: SixLabors.ImageSharp

## 📁 Project Structure

```
ITAssetManagement.Web/
├── Controllers/              # MVC Controllers
│   ├── LaptopsController.cs
│   ├── UsersController.cs
│   ├── AssignmentsController.cs
│   └── HomeController.cs
├── Models/                   # Entity Models
│   ├── Laptop.cs
│   ├── User.cs
│   ├── Assignment.cs
│   ├── LaptopPhoto.cs
│   └── LaptopLog.cs
├── Services/                 # Business Logic Services
│   ├── Interfaces/
│   ├── LaptopService.cs
│   ├── UserService.cs
│   ├── AssignmentService.cs
│   └── BarcodeService.cs
├── Data/                     # Data Access Layer
│   ├── Repositories/
│   └── ApplicationDbContext.cs
├── Views/                    # Razor Views
├── Extensions/               # Helper Extensions
├── Migrations/               # EF Core Migrations
└── wwwroot/                 # Static Files
```


## 🚀 Getting Started

### Prerequisites
- .NET 8.0 SDK
- SQL Server or SQL Server Express LocalDB
- Visual Studio 2022 or VS Code

### Installation Steps

1. **Clone the repository**
   ```bash
   git clone [repository-url]
   cd ITAssetManagement
   ```

2. **Configure Database Connection**
   - Update connection string in `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=ITAssetManagementDB_Dev;Trusted_Connection=True;MultipleActiveResultSets=true"
     }
   }
   ```

3. **Run Database Migrations**
   ```bash
   cd ITAssetManagement.Web
   dotnet ef database update
   ```

4. **Build and Run**
   ```bash
   dotnet build
   dotnet run
   ```

5. **Access the Application**
   - Open browser and navigate to `https://localhost:5001` or `http://localhost:5000`

## ✨ Features

### 💻 Laptop Management
- **CRUD Operations**: Create, view, edit, and delete laptop records
- **Soft Delete**: Safe deletion with restoration capability
- **Search & Filter**: Search by brand, model, tag number, or ID
- **Pagination**: Efficient data browsing with 10 items per page
- **Photo Management**: Upload and manage laptop photos
- **Barcode Generation**: Generate CODE_128 barcodes for asset tracking
- **Notes System**: Add detailed notes to laptop records
- **Status Tracking**: Monitor laptop availability and condition

### 👥 User Management
- **User Profiles**: Manage user information including contact details
- **Department & Position**: Track organizational structure
- **Email Validation**: Ensure valid email addresses
- **Assignment History**: View user's laptop assignment history

### 📋 Assignment Management
- **Asset Assignment**: Assign laptops to users with date tracking
- **Return Management**: Process laptop returns with timestamps
- **Assignment History**: Complete audit trail of all assignments
- **Available Assets**: Track which laptops are currently available

### 📊 Additional Features
- **Audit Logging**: Comprehensive logging of all asset changes
- **Responsive UI**: Mobile-friendly Bootstrap interface
- **Data Validation**: Server-side and client-side validation
- **Error Handling**: Graceful error handling with user feedback
- **TempData Messaging**: Success, error, and info notifications

## 🗄️ Database Schema

### Core Entities:
- **Laptops**: Asset information with soft delete support
- **Users**: Employee/user information
- **Assignments**: Laptop-user assignment relationships
- **LaptopPhotos**: Photo attachments for laptops
- **LaptopLogs**: Audit trail for asset changes

### Key Features:
- **Soft Delete**: Laptops marked as inactive instead of physical deletion
- **Foreign Key Relationships**: Proper relational integrity
- **Data Annotations**: Validation rules and display formatting
- **Cascade Delete**: Automatic cleanup of related records

## 🔧 Development Guidelines

### Code Standards:
- Follow C# naming conventions (PascalCase for classes, camelCase for variables)
- Use async/await for all database operations
- Implement proper error handling with try-catch blocks
- Apply SOLID principles in design
- Use dependency injection for service registration

### Database Guidelines:
- Use Entity Framework Core migrations for schema changes
- Implement proper data annotations for validation
- Use soft delete for important entities
- Include audit fields where necessary

## 🚦 Migration History

- **AddNotesToLaptop** (2025-07-29): Added Notes field to Laptop entity
- **RemoveUnnecessaryColumns** (2025-07-22): Cleaned up unused columns
- **AddDateFieldsToAssignment** (2025-07-22): Enhanced assignment date tracking
- **SoftDeleteForLaptops** (2025-07-15): Implemented soft delete functionality
- **AddDeletedLaptopsTable** (2025-07-10): Initial deleted laptops tracking

## 🔮 Future Enhancements

- **Authentication & Authorization**: User login and role-based access
- **API Endpoints**: RESTful API for external integrations
- **Email Notifications**: Automated notifications for assignments
- **Advanced Reporting**: Dashboard and analytics
- **File Upload Security**: Enhanced security for photo uploads
- **Caching Strategy**: Performance optimization
- **Unit Testing**: Comprehensive test coverage

## 📝 Contributing

1. Fork the repository
2. Create a feature branch
3. Follow the coding standards
4. Write tests for new features
5. Submit a pull request

## 📄 License

This project is for internal organizational use.