using MobyLabWebProgramming.Core.Entities;
using Ardalis.Specification;

namespace MobyLabWebProgramming.Core.Specifications;

/// <summary>
/// This is a simple specification to filter the user entities from the database via the constructors.
/// Note that this is a sealed class, meaning it cannot be further derived.
/// </summary>
public sealed class ReservationSpec : BaseSpec<ReservationSpec, Reservation>
{
    public ReservationSpec(Guid id) : base(id)
    {
    }

    public ReservationSpec(Guid fieldId, Guid sportId)
    {
        Query.Where(e => e.FieldId == fieldId && e.SportId == sportId);
    }
}