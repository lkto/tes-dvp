using InvoicesApp.Models;

namespace InvoicesApp.Repositories.Interfaces;

public interface ICustomerRepository
{
    Task<List<CustomerModel>> GetAllAsync();
}
