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

