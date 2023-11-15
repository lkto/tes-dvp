namespace InvoicesApp.Models;

public class CustomerModel
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public CustomerTypeModel CustomerType { get; set; } = new();
    public DateTime CreationDate { get; set; }
    public string RFC { get; set; } = "";
}
