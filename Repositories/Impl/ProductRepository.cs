using System.Data;
using System.Data.SqlClient;
using InvoicesApp.Data;
using InvoicesApp.Models;
using InvoicesApp.Repositories.Interfaces;

namespace InvoicesApp.Repositories.Impl;

public class ProductRepository : IProductRepository
{
    private readonly DatabaseContext _databaseContext;

    public ProductRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<List<ProductModel>> GetAllAsync()
    {
        DataTable result = await _databaseContext.ExecuteQueryAsync("sp_ListarProductos");

        return BuildResult(result);
    }

    public async Task<ProductModel> GetByIdAsync(int id)
    {
        SqlParameter[] parameters = new SqlParameter[] {
            new("@Id", id),
        };

        DataTable result = await _databaseContext.ExecuteQueryAsync("sp_BuscarProductoPorId", parameters);

        List<ProductModel> products = BuildResult(result);

        if (products.Count == 0)
        {
            throw new Exception("No se encontró el producto"); // TODO: Create custom exception
        }

        return products[0];
    }

    private static List<ProductModel> BuildResult(DataTable data)
    {
        List<ProductModel> products = new();

        foreach (DataRow product in data.Rows)
        {
            products.Add(new ProductModel
            {
                Id = Convert.ToInt32(product["Id"]),
                Name = product["NombreProducto"]?.ToString() ?? "",
                Image = product["ImagenProducto"]?.ToString(),
                UnitPrice = Convert.ToDecimal(product["PrecioUnitario"]),
                Ext = product["Ext"]?.ToString()
            });
        }

        return products;
    }
}
