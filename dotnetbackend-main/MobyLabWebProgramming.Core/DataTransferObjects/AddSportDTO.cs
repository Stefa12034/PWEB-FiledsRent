namespace MobyLabWebProgramming.Core.DataTransferObjects;

/// <summary>
/// This DTO is used to transfer information about a user file within the application and to client application.
/// </summary>
public class AddSportDTO
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
}
