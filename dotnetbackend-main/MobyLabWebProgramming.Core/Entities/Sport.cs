using MobyLabWebProgramming.Core.Enums;
using System.Collections;

namespace MobyLabWebProgramming.Core.Entities;

/// <summary>
/// This is an example for a user entity, it will be mapped to a single table and each property will have it's own column except for entity object references also known as navigation properties.
/// </summary>
public class Sport : BaseEntity
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public ICollection<Prices> Prices { get; set; } = default!;
    public ICollection<Packet> Packets { get; set; } = default!; // One-To-Many
    public ICollection<Reservation> Reservations { get; set; } = default!;
}
