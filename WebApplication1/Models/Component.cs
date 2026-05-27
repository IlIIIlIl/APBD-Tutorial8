namespace WebApplication1.Models;

public class Component
{
    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;
    
    public int ComponentManufacturersId { get; set; }
    
    public int ComponentTypesId { get; set; }
    
    public ComponentManufacturers ComponentManufacturers { get; set; } = null!;

    public ComponentTypes ComponentTypes { get; set; } = null!;

    public ICollection<PCComponent> PCComponents { get; set; } = new List<PCComponent>();
}