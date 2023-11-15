namespace InvoicesApp.Models;

public class ProductModel
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string? Image { get; set; }
    public decimal UnitPrice { get; set; }
    public string? Ext { get; set; }
}
