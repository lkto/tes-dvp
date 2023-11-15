namespace InvoicesApp.Models;

public class InvoiceViewModel
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Number { get; set; }
    public decimal Total { get; set; }
}
