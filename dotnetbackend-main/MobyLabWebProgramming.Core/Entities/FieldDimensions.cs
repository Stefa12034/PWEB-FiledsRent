using MobyLabWebProgramming.Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobyLabWebProgramming.Core.Entities;

/// <summary>
/// This is an example for a user entity, it will be mapped to a single table and each property will have it's own column except for entity object references also known as navigation properties.
/// </summary>
public class FieldDimensions : BaseEntity
{
    [ForeignKey("Field")]
    public Guid FieldId { get; set; }
    public Field Field { get; set; } = default!; // One-to-One
    public int Width { get; set; } = default!;
    public int Length { get; set; } = default!;
}
