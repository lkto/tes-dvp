namespace InvoicesApp.Models;

public class InvoiceModel
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int CustomerId { get; set; }
    public int Number { get; set; }
    public int TotalNumberItems { get; set; }
    public InvoiceDetailModel[] Details { get; set; } = Array.Empty<InvoiceDetailModel>();
    public decimal SubTotal { get; set; }
    public decimal TaxTotal { get; set; }
    public decimal Total { get; set; }
}
