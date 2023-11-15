using InvoicesApp.Models;

namespace InvoicesApp.Repositories.Interfaces
{
    public interface IInvoiceRepository
    {
        Task<List<InvoiceViewModel>> GetAllAsync();
        Task<InvoiceViewModel> GetByIdAsync(int id);
        Task<List<InvoiceViewModel>> GetAllByCustomerAsync(int customerId);
        Task<InvoiceViewModel> GetByNumberAsync(int invoiceNumber);
        Task<InvoiceModel> CreateAsync(InvoiceModel invoice);
        Task<InvoiceModel> UpdateAsync(InvoiceModel invoice);
        Task<InvoiceModel> DeleteAsync(int id);
    }
}
