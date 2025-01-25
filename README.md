# E-Commerce Project - Backend Development

This is a comprehensive backend application for an e-commerce platform built using C#, ASP.NET Core, Entity Framework Core, and SQL Server. The project is designed to manage core e-commerce functionalities, including user management, product catalog, order management, and more. It provides the server-side architecture and API endpoints necessary to support the functionality of an online store.

## Features

### 1. **User Authentication & Authorization**
   - **Admin, Seller, and Customer Roles**: The platform supports multiple user roles, each with distinct permissions and capabilities.
     - **Admin**: Manages the overall platform, including user management and system settings.
     - **Seller**: Manages their own store, inventory, and sales.
     - **Customer**: Browses products, places orders, and manages their personal information.
   - **ASP.NET Core Identity** is used to handle user authentication and authorization, ensuring secure login and access control.

### 2. **Product Management**
   - **Product Catalog**: The system allows the creation, update, and deletion of products. Each product can have multiple associated images.
   - **Product Categories**: Products can be categorized for easier browsing and search filtering.

### 3. **Order Management**
   - **Order Creation**: Customers can add items to the cart and proceed to checkout, where an order is created.
   - **Order Status**: The system tracks the status of orders (e.g., Pending, Shipped, Delivered).
   - **Order Items**: Each order consists of multiple items with references to the corresponding products.

### 4. **Flexible Shipping & Billing**
   - **Multiple Addresses**: Customers can enter different shipping and billing addresses, providing flexibility for various delivery scenarios.

### 5. **Payment Integration**
   - Payment methods (Stripe) are supported to handle transaction processing securely.

### 6. **Database Architecture**
   - **Entity Framework Core** is used for data access, leveraging code-first migrations to handle schema evolution.
   - The database includes entities such as **Admin**, **Seller**, **Customer**, **Product**, **Order**, and **OrderItem** and others.

### 7. **Admin Panel**
   - Admins can manage users, products, and view platform analytics, including order summaries and sales data.

### 8. **Security**
   - The system implements robust security measures, including password hashing, user session management, and role-based access control.

## Technologies Used

- **C#**: Primary programming language for backend development.
- **ASP.NET Core**: Framework used for building the Web API.
- **Entity Framework Core**: ORM used to interact with SQL Server and manage database operations.
- **SQL Server**: Database management system.
- **JWT (JSON Web Tokens)**: Used for secure API authentication.
- **Swagger**: For API documentation and testing endpoints.

## Project Structure

- **Controllers**: Manages API endpoints and implements the business logic.
- **Models**: Contains classes that represent the database schema (e.g., Product, Order, User).
- **DTOs (Data Transfer Objects)**: Used for data exchange between layers of the application.
- **Services**: Encapsulates the business logic for handling operations like order processing and payment.
