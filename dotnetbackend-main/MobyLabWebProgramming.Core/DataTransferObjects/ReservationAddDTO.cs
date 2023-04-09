using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Core.DataTransferObjects;

/// <summary>
/// This DTO is used to transfer information about a user file within the application and to client application.
/// </summary>
public class ReservationAddDTO
{
    public Guid UserId { get; set; } = default!;
    public Guid FieldId { get; set; } = default!;
    public Guid SportId { get; set; } = default!;
    public DateTime StartAt { get; set; } = default!;
    public int Period { get; set; } = default!;
}

