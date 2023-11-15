using System.Data;
using System.Data.SqlClient;
using InvoicesApp.Data;
using InvoicesApp.Models;
using InvoicesApp.Repositories.Interfaces;

namespace InvoicesApp.Repositories.Impl;

public class InvoiceRepository : IInvoiceRepository
{
    private readonly DatabaseContext _databaseContext;

    public InvoiceRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<List<InvoiceViewModel>> GetAllAsync()
    {
        DataTable result = await _databaseContext.ExecuteQueryAsync("sp_ListarFacturas");

        return BuildResult(result);
    }

    public async Task<InvoiceViewModel> GetByIdAsync(int id)
    {
        SqlParameter[] parameters = new SqlParameter[] {
            new("@Id", id),
        };

        DataTable result = await _databaseContext.ExecuteQueryAsync("sp_BuscarFactura", parameters);

        List<InvoiceViewModel> invoices = BuildResult(result);

        if (invoices.Count == 0)
        {
            throw new Exception("No se encontró la factura"); // TODO: Create custom exception
        }

        return invoices[0];
    }

    public async Task<List<InvoiceViewModel>> GetAllByCustomerAsync(int customerId)
    {
        SqlParameter[] parameters = new SqlParameter[] {
            new("@IdCliente", customerId),
        };

        DataTable result = await _databaseContext.ExecuteQueryAsync("sp_BuscarFacturaPorCliente", parameters);

        return BuildResult(result);
    }

    public async Task<InvoiceViewModel> GetByNumberAsync(int invoiceNumber)
    {
        SqlParameter[] parameters = new SqlParameter[] {
            new("@NumeroFactura", invoiceNumber),
        };

        DataTable result = await _databaseContext.ExecuteQueryAsync("sp_BuscarFacturaPorNumero", parameters);

        List<InvoiceViewModel> invoices = BuildResult(result);

        if (invoices.Count == 0)
        {
            throw new Exception("No existe ningúna factura con este número"); // TODO: Create custom exception
        }

        return invoices[0];
    }

    public async Task<InvoiceModel> CreateAsync(InvoiceModel invoice)
    {
        using SqlConnection connection = _databaseContext.getConnection;
        connection.Open();

        using SqlTransaction transaction = connection.BeginTransaction();

        try
        {
            invoice.Id = await CreateInvoice(invoice, connection, transaction);

            foreach (var item in invoice.Details)
            {
                item.Id = await CreateInvoiceDetail(invoice, connection, transaction, item);
            }

            transaction.Commit();
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }

        return invoice;
    }

    public Task<InvoiceModel> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<InvoiceModel> UpdateAsync(InvoiceModel invoice)
    {
        throw new NotImplementedException();
    }

    private static List<InvoiceViewModel> BuildResult(DataTable data)
    {
        List<InvoiceViewModel> invoices = new();

        foreach (DataRow product in data.Rows)
        {
            invoices.Add(new InvoiceViewModel
            {
                Id = Convert.ToInt32(product["Id"]),
                Date = Convert.ToDateTime(product["FechaEmision"]),
                Number = Convert.ToInt32(product["NumeroFactura"]),
                Total = Convert.ToDecimal(product["TotalFactura"])
            });
        }

        return invoices;
    }

    private static async Task<int> CreateInvoice(InvoiceModel invoice, SqlConnection connection, SqlTransaction transaction)
    {
        using SqlCommand command = new("sp_InsertarFactura", connection, transaction);

        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.AddWithValue("@IdCliente", invoice.CustomerId);
        command.Parameters.AddWithValue("@NumeroFactura", invoice.Number);
        command.Parameters.AddWithValue("@NumeroTotalArticulos", invoice.TotalNumberItems);
        command.Parameters.AddWithValue("@SubTotalFactura", invoice.SubTotal);
        command.Parameters.AddWithValue("@TotalImpuesto", invoice.TaxTotal);
        command.Parameters.AddWithValue("@TotalFactura", invoice.Total);

        int invoiceId = (int)(await command.ExecuteScalarAsync() ?? 0);

        if (invoiceId == 0)
        {
            throw new Exception("No se pudo crear la factura"); // TODO: Create custom exception
        }

        return invoiceId;
    }

    private static async Task<int> CreateInvoiceDetail(InvoiceModel invoice, SqlConnection connection, SqlTransaction transaction, InvoiceDetailModel item)
    {
        SqlCommand command = new("sp_InsertarDetalleFactura", connection, transaction);

        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.AddWithValue("@IdFactura", invoice.Id);
        command.Parameters.AddWithValue("@IdProducto", item.ProductId);
        command.Parameters.AddWithValue("@CantidadDeProducto", item.ProductQuantity);
        command.Parameters.AddWithValue("@PrecioUnitarioProducto", item.ProductPrice);
        command.Parameters.AddWithValue("@SubTotalProducto", item.SubTotal);
        command.Parameters.AddWithValue("@Notas", item.Observations ?? "");

        int itemId = (int)(await command.ExecuteScalarAsync() ?? 0);

        if (itemId == 0)
        {
            throw new Exception("No se pudo crear el detalle"); // TODO: Create custom exception
        }

        return itemId;
    }
}
