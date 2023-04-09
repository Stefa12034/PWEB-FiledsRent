using MobyLabWebProgramming.Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobyLabWebProgramming.Core.Entities;

/// <summary>
/// This is an example for a user entity, it will be mapped to a single table and each property will have it's own column except for entity object references also known as navigation properties.
/// </summary>
public class Field : BaseEntity
{
    public string Name { get; set; } = default!;
    public string Location { get; set; } = default!;
    public ICollection<Reservation> Reservations { get; set; } = default!; // One-to-Many
    public FieldDimensions FieldDimensions { get; set; } = default!; // One-to-One
    public ICollection<Prices> Prices { get; set; } = default!;
}
