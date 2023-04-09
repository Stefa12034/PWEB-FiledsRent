using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Core.DataTransferObjects;

/// <summary>
/// This DTO is used to transfer information about a user file within the application and to client application.
/// </summary>
public class FieldDTO
{
    public Guid Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Location { get; set; } = default!;
    // public ICollection<Reservation> Reservations { get; set; } = default!; // One-to-Many
    public FieldDimensions FieldDimensions { get; set; } = default!; // One-to-One
    public ICollection<Prices> Prices { get; set; } = default!;
}

