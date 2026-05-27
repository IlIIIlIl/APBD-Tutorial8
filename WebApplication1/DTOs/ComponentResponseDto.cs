namespace WebApplication1.DTOs;

public class ComponentResponseDto
{
    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int Amount { get; set; }
}