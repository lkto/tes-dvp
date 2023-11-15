using System.Data;
using InvoicesApp.Data;
using InvoicesApp.Models;
using InvoicesApp.Repositories.Interfaces;

namespace InvoicesApp.Repositories.Impl;

public class CustomerRepository : ICustomerRepository
{
    private readonly DatabaseContext _databaseContext;

    public CustomerRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<List<CustomerModel>> GetAllAsync()
    {
        List<CustomerModel> products = new();

        DataTable result = await _databaseContext.ExecuteQueryAsync("sp_ListarClientes");

        foreach (DataRow product in result.Rows)
        {
            products.Add(new CustomerModel
            {
                Id = Convert.ToInt32(product["Id"]),
                Name = product["RazonSocial"]?.ToString() ?? "",
                RFC = product["RFC"]?.ToString() ?? "",
                CustomerType = new()
                {
                    Id = Convert.ToInt32(product["IdTipoCliente"]),
                    Name = product["TipoCliente"]?.ToString() ?? ""

                },
                CreationDate = Convert.ToDateTime(product["FechaCreacion"]),
            });
        }

        return products;
    }
}
