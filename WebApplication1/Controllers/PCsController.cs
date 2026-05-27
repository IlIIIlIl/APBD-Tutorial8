using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTOs;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PCsController : ControllerBase
{
    private readonly AppDbContext _context;

    public PCsController(AppDbContext context)
    {
        _context = context;
    }

    // GET api/pcs
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var pcs = await _context.PCs
            .Select(p => new PcResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                Weight = p.Weight,
                Warranty = p.Warranty,
                CreatedAt = p.CreatedAt,
                Stock = p.Stock
            })
            .ToListAsync();

        return Ok(pcs);
    }

    // GET api/pcs/{id}/components
    [HttpGet("{id}/components")]
    public async Task<IActionResult> GetComponents(int id)
    {
        var pcExists = await _context.PCs.AnyAsync(p => p.Id == id);

        if (!pcExists)
            return NotFound();

        var components = await _context.PCComponents
            .Where(pc => pc.PCId == id)
            .Include(pc => pc.Component)
            .Select(pc => new ComponentResponseDto
            {
                Code = pc.Component.Code,
                Name = pc.Component.Name,
                Description = pc.Component.Description,
                Amount = pc.Amount
            })
            .ToListAsync();

        return Ok(components);
    }

    // POST api/pcs
    [HttpPost]
    public async Task<IActionResult> Create(CreatePcDto dto)
    {
        var pc = new PC
        {
            Name = dto.Name,
            Weight = dto.Weight,
            Warranty = dto.Warranty,
            CreatedAt = dto.CreatedAt,
            Stock = dto.Stock
        };

        _context.PCs.Add(pc);

        await _context.SaveChangesAsync();

        var response = new PcResponseDto
        {
            Id = pc.Id,
            Name = pc.Name,
            Weight = pc.Weight,
            Warranty = pc.Warranty,
            CreatedAt = pc.CreatedAt,
            Stock = pc.Stock
        };

        return CreatedAtAction(
            nameof(GetAll),
            new { id = pc.Id },
            response
        );
    }

    // PUT api/pcs/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdatePcDto dto)
    {
        var pc = await _context.PCs.FindAsync(id);

        if (pc == null)
            return NotFound();

        pc.Name = dto.Name;
        pc.Weight = dto.Weight;
        pc.Warranty = dto.Warranty;
        pc.CreatedAt = dto.CreatedAt;
        pc.Stock = dto.Stock;

        await _context.SaveChangesAsync();

        return Ok(new PcResponseDto
        {
            Id = pc.Id,
            Name = pc.Name,
            Weight = pc.Weight,
            Warranty = pc.Warranty,
            CreatedAt = pc.CreatedAt,
            Stock = pc.Stock
        });
    }

    // DELETE api/pcs/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var pc = await _context.PCs
            .Include(p => p.PCComponents)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (pc == null)
            return NotFound();

        _context.PCComponents.RemoveRange(pc.PCComponents);

        _context.PCs.Remove(pc);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}