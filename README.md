# 👟 FootCap - Full Stack Shoe Store (ASP.NET Core MVC)

FootCap is a responsive full stack e-commerce web application for managing and selling shoes online. Built with ASP.NET Core MVC and Entity Framework Core, it features user authentication, product browsing, shopping cart, and order management.

---

## 🌐 Tech Stack

**Frontend:**
- HTML5, CSS3, JavaScript
- Bootstrap 5
- Boxicons

**Backend:**
- ASP.NET Core MVC
- Entity Framework Core (Code First)
- SQL Server
- Identity (User Authentication and Roles)

**Tools & Libraries:**
- Visual Studio 2022
- jQuery
- Git & GitHub

---

## 🚀 Features

- 🧑‍💼 User Registration & Login
- 🔐 Identity Roles: `Admin` and `User`
- 📦 Browse Products by Category (Nike, Adidas, etc.)
- 🛒 Add to Cart with Quantity Control
- ✅ Confirm Orders with Live Stock Deduction
- 📉 Stock Management After Orders
- 🗑 Remove Items from Cart
- 💬 Bootstrap Alert Cards for Notifications
- 📱 Fully Responsive UI
🎯 Design Principles
This project follows the SOLID principles of object-oriented design to ensure a clean, maintainable, and scalable codebase. Applying these principles helps in creating software that is easier to understand, test, and extend.

Single Responsibility Principle (SRP): Each class and module has one clear responsibility, which simplifies debugging and future changes.

Open/Closed Principle (OCP): The system is open for extension but closed for modification, allowing new features to be added without changing existing code.

Liskov Substitution Principle (LSP): Derived classes can be used interchangeably with their base classes without affecting the correctness of the program.

Interface Segregation Principle (ISP): Clients are provided with specific interfaces rather than a single general-purpose interface, promoting decoupling.

Dependency Inversion Principle (DIP): High-level modules depend on abstractions rather than concrete implementations, enhancing flexibility and testability.

By adhering to these principles, the FootCap application achieves a modular architecture that supports ongoing development and maintenance with minimal risk of regression.



## 📁 Project Structure

```plaintext
FootCap/
│
├── Controllers/          # Contains MVC controllers handling user requests and app logic
│   ├── AccountController.cs
│   ├── CartController.cs
│   ├── OrderController.cs
│   └── ProdcController.cs
│
├── Models/               # Contains entity classes representing data models
│   ├── Product.cs
│   ├── Category.cs
│   ├── Cart.cs
│   ├── CartItem.cs
│   ├── Order.cs
│   └── OrderItem.cs
│
├── Views/                # Razor views for UI pages organized by feature
│   ├── Cart/
│   ├── Order/
│   ├── Account/
│   ├── Prodc/
│   └── Shared/
│
├── wwwroot/              # Static files (CSS, JavaScript, images)
│   ├── css/
│   ├── js/
│   └── images/
│
├── Data/                 # Database context and migrations
│   └── Context.cs
│
├── appsettings.json      # Configuration file including DB connection strings
└── Program.cs            # Application startup and configuration

