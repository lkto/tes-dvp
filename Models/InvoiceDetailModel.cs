namespace InvoicesApp.Models;

public class InvoiceDetailModel
{
    public int Id { get; set; }
    public int InvoiceId { get; set; }
    public int ProductId { get; set; }
    public int ProductQuantity { get; set; }
    public decimal ProductPrice { get; set; }
    public decimal SubTotal { get; set; }
    public string? Observations { get; set; }
}
