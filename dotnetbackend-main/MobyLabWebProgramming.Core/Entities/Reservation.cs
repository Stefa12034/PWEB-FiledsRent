using MobyLabWebProgramming.Core.Enums;

namespace MobyLabWebProgramming.Core.Entities;

/// <summary>
/// This is an example for a user entity, it will be mapped to a single table and each property will have it's own column except for entity object references also known as navigation properties.
/// </summary>
public class Reservation : BaseEntity
{
    public Guid UserId { get; set; } = default!;
    public Guid FieldId { get; set; } = default!;
    public Guid SportId { get; set; } = default!;
    public User User { get; set; } = default!; // Many-to-One
    public Field Field { get; set; } = default!; // Many-to-One
    public Sport Sport { get; set; } = default!; // Many-to-One
    public DateTime StartAt { get; set; } = default!;
    public int Period { get; set; } = default!;
}
