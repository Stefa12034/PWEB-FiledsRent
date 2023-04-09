using MobyLabWebProgramming.Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MobyLabWebProgramming.Core.Entities;

/// <summary>
/// This is an example for a user entity, it will be mapped to a single table and each property will have it's own column except for entity object references also known as navigation properties.
/// </summary>
public class Prices : BaseEntity
{
    // [Key, Column(Order = 0)]
    public Guid FieldId { get; set; } = default!;
    // [Key, Column(Order = 1)]
    public Guid SportId { get; set; } = default!;
    public Field Field { get; set; } = default!; // Many-to-One
    public Sport Sport { get; set; } = default!; // Many-to-One
    public int Price { get; set; } = default!;
}
