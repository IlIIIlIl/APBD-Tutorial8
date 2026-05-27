using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data;

public class AppDbContext : DbContext
{
    public DbSet<PC> PCs => Set<PC>();
    public DbSet<Component> Components => Set<Component>();
    public DbSet<PCComponent> PCComponents => Set<PCComponent>();
    public DbSet<ComponentManufacturers> ComponentManufacturers => Set<ComponentManufacturers>();
    public DbSet<ComponentTypes> ComponentTypes => Set<ComponentTypes>();

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Component>()
        .HasKey(c => c.Code);

    modelBuilder.Entity<PCComponent>()
        .HasKey(pc => new { pc.PCId, pc.ComponentCode });

    modelBuilder.Entity<PCComponent>()
        .HasOne(pc => pc.PC)
        .WithMany(p => p.PCComponents)
        .HasForeignKey(pc => pc.PCId);

    modelBuilder.Entity<PCComponent>()
        .HasOne(pc => pc.Component)
        .WithMany(c => c.PCComponents)
        .HasForeignKey(pc => pc.ComponentCode);

    // =========================
    // COMPONENT MANUFACTURERS
    // =========================

    modelBuilder.Entity<ComponentManufacturers>().HasData(
        new ComponentManufacturers
        {
            Id = 1,
            Abbreviation = "INT",
            FullName = "Intel Corporation",
            FoundationDate = new DateOnly(1968, 7, 18)
        },
        new ComponentManufacturers
        {
            Id = 2,
            Abbreviation = "AMD",
            FullName = "Advanced Micro Devices",
            FoundationDate = new DateOnly(1969, 5, 1)
        },
        new ComponentManufacturers
        {
            Id = 3,
            Abbreviation = "NVD",
            FullName = "NVIDIA Corporation",
            FoundationDate = new DateOnly(1993, 4, 5)
        }
    );

    // =========================
    // COMPONENT TYPES
    // =========================

    modelBuilder.Entity<ComponentTypes>().HasData(
        new ComponentTypes
        {
            Id = 1,
            Abbreviation = "CPU",
            Name = "Processor"
        },
        new ComponentTypes
        {
            Id = 2,
            Abbreviation = "GPU",
            Name = "Graphics Card"
        },
        new ComponentTypes
        {
            Id = 3,
            Abbreviation = "RAM",
            Name = "Memory"
        }
    );

    // =========================
    // COMPONENTS
    // =========================

    modelBuilder.Entity<Component>().HasData(
        new Component
        {
            Code = "CPU001",
            Name = "Intel i9",
            Description = "High-end gaming processor",
            ComponentManufacturersId = 1,
            ComponentTypesId = 1
        },
        new Component
        {
            Code = "GPU001",
            Name = "RTX 4090",
            Description = "High-performance graphics card",
            ComponentManufacturersId = 3,
            ComponentTypesId = 2
        },
        new Component
        {
            Code = "RAM001",
            Name = "Corsair 32GB",
            Description = "DDR5 RAM kit",
            ComponentManufacturersId = 2,
            ComponentTypesId = 3
        }
    );

    // =========================
    // PCS
    // =========================

    modelBuilder.Entity<PC>().HasData(
        new PC
        {
            Id = 1,
            Name = "Gaming Beast XYZ",
            Weight = 12.5f,
            Warranty = 36,
            CreatedAt = new DateTime(2026, 5, 8, 9, 0, 0),
            Stock = 5
        },
        new PC
        {
            Id = 2,
            Name = "Office Mini Pro",
            Weight = 4.2f,
            Warranty = 24,
            CreatedAt = new DateTime(2026, 4, 15, 13, 30, 0),
            Stock = 12
        },
        new PC
        {
            Id = 3,
            Name = "Budget Home PC",
            Weight = 6.8f,
            Warranty = 12,
            CreatedAt = new DateTime(2026, 3, 10, 10, 0, 0),
            Stock = 8
        }
    );

    // =========================
    // PC COMPONENTS
    // =========================

    modelBuilder.Entity<PCComponent>().HasData(
        new PCComponent
        {
            PCId = 1,
            ComponentCode = "CPU001",
            Amount = 1
        },
        new PCComponent
        {
            PCId = 1,
            ComponentCode = "GPU001",
            Amount = 1
        },
        new PCComponent
        {
            PCId = 1,
            ComponentCode = "RAM001",
            Amount = 2
        },
        new PCComponent
        {
            PCId = 2,
            ComponentCode = "CPU001",
            Amount = 1
        },
        new PCComponent
        {
            PCId = 2,
            ComponentCode = "RAM001",
            Amount = 1
        },
        new PCComponent
        {
            PCId = 3,
            ComponentCode = "RAM001",
            Amount = 1
        }
    );
}
}