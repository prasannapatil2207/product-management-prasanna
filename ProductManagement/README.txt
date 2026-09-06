PRODUCT MANAGEMENT - ASP.NET CORE MVC .NET 8

TECHNOLOGY
-----------
Frontend: HTML/Razor, CSS, JavaScript
Backend: ASP.NET Core MVC, C#
Database: SQL Server LocalDB
Database connection: Microsoft.Data.SqlClient (ADO.NET)

IMPORTANT
---------
This project targets .NET 8.0.
Visual Studio must have the .NET 8 SDK installed.

RUN STEPS
---------
1. Extract the ZIP file.
2. Open ProductManagement.csproj in Visual Studio 2026.
3. Wait for NuGet restore to finish.
4. Make sure SQL Server LocalDB is installed.
5. Open SQL Server Object Explorer.
6. Connect to (localdb)\MSSQLLocalDB.
7. Open Database/ProductDB.sql.
8. Execute the complete SQL script.
9. Build > Build Solution.
10. Run with Ctrl+F5 or the green Run button.
11. The application opens on /Product.

DATABASE CONNECTION
-------------------
Server=(localdb)\MSSQLLocalDB
Database=ProductDB
Trusted_Connection=True
TrustServerCertificate=True

If the database already exists, the SQL script will NOT delete it.
If Products already has rows, sample rows are not inserted again.

FEATURES
--------
- View products
- Add product
- Edit product
- Delete product
- Search product/category
- Stock status
