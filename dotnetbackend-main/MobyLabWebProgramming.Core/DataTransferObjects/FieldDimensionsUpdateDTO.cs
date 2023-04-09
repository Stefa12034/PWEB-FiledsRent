namespace MobyLabWebProgramming.Core.DataTransferObjects;

/// <summary>
/// This DTO is used to transfer information about a user file within the application and to client application.
/// </summary>
public record FieldDimensionsUpdateDTO(Guid FieldId, int? Width = default, int? Length = default);


