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

FootCap/
│
├── Controllers/ # Handles user requests
│ ├── AccountController.cs # Login, Register, Logout
│ ├── CartController.cs # Add/Remove from Cart
│ ├── OrderController.cs # Confirm Orders
│ └── ProdcController.cs # Display Products
│
├── Models/ # Application entities
│ ├── Product.cs
│ ├── Category.cs
│ ├── Cart.cs
│ ├── CartItem.cs
│ ├── Order.cs
│ └── OrderItem.cs
│
├── Views/ # UI pages (Razor)
│ ├── Cart/
│ ├── Order/
│ ├── Account/
│ ├── Prodc/
│ └── Shared/
│
├── wwwroot/ # Static files
│ ├── css/
│ ├── js/
│ └── images/
│
├── Data/
│ └── Context.cs # DB context and configuration
│
├── appsettings.json # DB connection & configs
└── Program.cs # App entry point

yaml
Copy
Edit
