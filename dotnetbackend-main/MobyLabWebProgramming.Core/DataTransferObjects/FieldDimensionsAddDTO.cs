using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Core.DataTransferObjects;

/// <summary>
/// This DTO is used to transfer information about a user file within the application and to client application.
/// </summary>
public class FieldDimensionsAddDTO
{
    public Guid FieldId { get; set; } = default!;
    public int Width { get; set; } = default!;
    public int Length { get; set; } = default!;
}

