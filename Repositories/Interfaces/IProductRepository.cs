using InvoicesApp.Models;

namespace InvoicesApp.Repositories.Interfaces;

public interface IProductRepository
{
    Task<List<ProductModel>> GetAllAsync();
    Task<ProductModel> GetByIdAsync(int id);
}
