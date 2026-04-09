# Inventory Management System

A desktop inventory management application built with WPF and .NET 8.0, designed to track CCTV and networking equipment. The system operates offline-first using a local SQL Server database, with Supabase as the cloud synchronization layer.

---

## Overview

This application provides a centralized interface for managing physical inventory across equipment categories including Cameras, DVR, NVR, POE, HDD, and Adaptors. It is built for internal use in security systems and IT environments where reliable offline access is essential.

---

## Tech Stack

| Layer | Technology |
|---|---|
| Framework | .NET 8.0, WPF (Windows Presentation Foundation) |
| Language | C# |
| Local Database | SQL Server (via SSMS) |
| ORM | Entity Framework Core 8.0 |
| Cloud Sync | Supabase (PostgreSQL, Singapore region) |
| IDE | Visual Studio 2022 |

---

## Features

- Secure login with role-based access
- Dashboard with weekly summary cards (New Stocks, Total Returns, Pending Requests, Defective Items)
- Stock management with category filtering and DataGrid views
- Add, track, and manage stock items including serial number, model, warranty, and assigned staff
- Request and return workflow management
- Offline-first architecture with cloud sync support

---

## Project Structure

```
Inventory/
├── Data/
│   ├── AppDbContext.cs
│   └── DatabaseService.cs
├── Models/
│   ├── StockEntity.cs
│   ├── RequestEntity.cs
│   └── ReturnEntity.cs
├── Views/
│   ├── LoginWindow.xaml
│   ├── MainWindow.xaml
│   ├── StocksPage.xaml
│   ├── AddStockPage.xaml
│   ├── RequestsPage.xaml
│   ├── AddRequestPage.xaml
│   ├── ReturnsPage.xaml
│   └── AddReturnPage.xaml
└── Inventory.csproj
```

---

## Prerequisites

- Windows OS
- Visual Studio 2022
- .NET 8.0 SDK
- SQL Server (local instance via SSMS)
- Internet connection for Supabase sync

---

## Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/your-username/inventory.git
cd inventory
```

### 2. Configure the Local Database

Open SQL Server Management Studio and ensure a database named `InventoryDB` exists on your local server instance. Update the connection string in `AppDbContext.cs` to match your server name if needed.

```
Server=YOUR_SERVER_NAME;Database=InventoryDB;Trusted_Connection=True;
```

### 3. Apply Migrations

Open the Package Manager Console in Visual Studio and run:

```
Update-Database
```

### 4. Configure Supabase (Optional)

If cloud sync is required, update the Supabase URL and anon key in the application configuration to point to your project instance.

### 5. Build and Run

Open `Inventory.sln` in Visual Studio 2022 and press `F5` to build and run the application.

---

## Default Credentials

```
Username: admin
Password: admin123
```

> It is recommended to update the credentials before deploying in a production environment.

---

## NuGet Packages

| Package | Version |
|---|---|
| Microsoft.EntityFrameworkCore.SqlServer | 8.0.25 |
| Microsoft.EntityFrameworkCore.Tools | 8.0.25 |
| Supabase | 1.1.1 |

---

## Equipment Categories

- Camera
- DVR
- NVR
- POE
- HDD
- Adaptor

---

## Roadmap

- Wire all UI pages to the local SQL Server database via EF Core
- Implement Supabase cloud sync layer
- Staff management module

---

## License

This project is for internal use only.
