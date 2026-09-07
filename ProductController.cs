using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ProductManagement.Models;

namespace ProductManagement.Controllers;

public class ProductController : Controller
{
    private readonly IConfiguration _configuration;

    public ProductController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    private string ConnectionString =>
        _configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Database connection string not found.");

    public IActionResult Index(string? search)
    {
        var products = new List<Product>();

        using var connection = new SqlConnection(ConnectionString);
        var sql = "SELECT Id, Name, Category, Price, Quantity FROM Products";

        if (!string.IsNullOrWhiteSpace(search))
            sql += " WHERE Name LIKE @Search OR Category LIKE @Search";

        sql += " ORDER BY Id DESC";

        using var command = new SqlCommand(sql, connection);

        if (!string.IsNullOrWhiteSpace(search))
            command.Parameters.AddWithValue("@Search", "%" + search.Trim() + "%");

        connection.Open();

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            products.Add(new Product
            {
                Id = Convert.ToInt32(reader["Id"]),
                Name = Convert.ToString(reader["Name"]) ?? "",
                Category = Convert.ToString(reader["Category"]) ?? "",
                Price = Convert.ToDecimal(reader["Price"]),
                Quantity = Convert.ToInt32(reader["Quantity"])
            });
        }

        ViewBag.Search = search;
        return View(products);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Product product)
    {
        ValidateProduct(product);

        if (!ModelState.IsValid)
            return View(product);

        using var connection = new SqlConnection(ConnectionString);
        const string sql = """
            INSERT INTO Products (Name, Category, Price, Quantity)
            VALUES (@Name, @Category, @Price, @Quantity)
            """;

        using var command = new SqlCommand(sql, connection);
        AddProductParameters(command, product);

        connection.Open();
        command.ExecuteNonQuery();

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var product = GetProduct(id);
        return product is null ? NotFound() : View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Product product)
    {
        ValidateProduct(product);

        if (!ModelState.IsValid)
            return View(product);

        using var connection = new SqlConnection(ConnectionString);
        const string sql = """
            UPDATE Products
            SET Name = @Name, Category = @Category, Price = @Price, Quantity = @Quantity
            WHERE Id = @Id
            """;

        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Id", product.Id);
        AddProductParameters(command, product);

        connection.Open();
        command.ExecuteNonQuery();

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        using var connection = new SqlConnection(ConnectionString);
        const string sql = "DELETE FROM Products WHERE Id = @Id";

        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Id", id);

        connection.Open();
        command.ExecuteNonQuery();

        return RedirectToAction(nameof(Index));
    }

    private Product? GetProduct(int id)
    {
        using var connection = new SqlConnection(ConnectionString);
        const string sql = """
            SELECT Id, Name, Category, Price, Quantity
            FROM Products
            WHERE Id = @Id
            """;

        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Id", id);

        connection.Open();

        using var reader = command.ExecuteReader();

        if (!reader.Read())
            return null;

        return new Product
        {
            Id = Convert.ToInt32(reader["Id"]),
            Name = Convert.ToString(reader["Name"]) ?? "",
            Category = Convert.ToString(reader["Category"]) ?? "",
            Price = Convert.ToDecimal(reader["Price"]),
            Quantity = Convert.ToInt32(reader["Quantity"])
        };
    }

    private static void AddProductParameters(SqlCommand command, Product product)
    {
        command.Parameters.AddWithValue("@Name", product.Name.Trim());
        command.Parameters.AddWithValue("@Category", product.Category.Trim());
        command.Parameters.AddWithValue("@Price", product.Price);
        command.Parameters.AddWithValue("@Quantity", product.Quantity);
    }

    private void ValidateProduct(Product product)
    {
        if (string.IsNullOrWhiteSpace(product.Name))
            ModelState.AddModelError(nameof(product.Name), "Product name is required.");

        if (string.IsNullOrWhiteSpace(product.Category))
            ModelState.AddModelError(nameof(product.Category), "Category is required.");

        if (product.Price < 0)
            ModelState.AddModelError(nameof(product.Price), "Price cannot be negative.");

        if (product.Quantity < 0)
            ModelState.AddModelError(nameof(product.Quantity), "Quantity cannot be negative.");
    }
}
