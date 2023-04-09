using MobyLabWebProgramming.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobyLabWebProgramming.Core.DataTransferObjects;

/// <summary>
/// This DTO is used to transfer information about a user file within the application and to client application.
/// </summary>
public class PriceDTO
{
    public Guid Id { get; set; } = default!;
    public int Price { get; set; } = 0;
    public Guid FieldId { get; set; }
    public Guid SportId { get; set; }
}

