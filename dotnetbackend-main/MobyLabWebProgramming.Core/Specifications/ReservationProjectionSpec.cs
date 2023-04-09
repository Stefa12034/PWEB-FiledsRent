using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MobyLabWebProgramming.Core.Specifications;

public class ReservationProjectionSpec : BaseSpec<ReservationProjectionSpec, Reservation, ReservationDTO>
{
    public ReservationProjectionSpec() : base()
    {
    }

    public ReservationProjectionSpec(Guid userId)
    {
        Query.Where(s => s.UserId == userId);
    }

    public ReservationProjectionSpec(Guid fieldId, DateTime startAt, int period)
    {
        Query.Where(r => r.FieldId == fieldId &&
        ((r.StartAt == startAt && r.Period == period) ||
        (startAt > r.StartAt && startAt < r.StartAt.AddHours(r.Period)) ||
        (startAt.AddHours(period) > r.StartAt && startAt.AddHours(period) < r.StartAt.AddHours(r.Period)) ||
        (r.StartAt > startAt && r.StartAt < startAt.AddHours(period)) ||
        (r.StartAt.AddHours(r.Period) > startAt && r.StartAt.AddHours(r.Period) < startAt.AddHours(period))
        ));
    }

    public ReservationProjectionSpec(Guid fieldId, DateTime startAt, int period, Guid reservationId)
    {
        Query.Where(r => r.Id != reservationId && r.FieldId == fieldId &&
        ((r.StartAt == startAt && r.Period == period) ||
        (startAt > r.StartAt && startAt < r.StartAt.AddHours(r.Period)) ||
        (startAt.AddHours(period) > r.StartAt && startAt.AddHours(period) < r.StartAt.AddHours(r.Period)) ||
        (r.StartAt > startAt && r.StartAt < startAt.AddHours(period)) ||
        (r.StartAt.AddHours(r.Period) > startAt && r.StartAt.AddHours(r.Period) < startAt.AddHours(period))
        ));
    }

    protected override Expression<Func<Reservation, ReservationDTO>> Spec => e => new()
    {
        Id = e.Id,
        UserId = e.UserId,
        FieldId = e.FieldId,
        SportId = e.SportId,
        StartAt = e.StartAt,
        Period = e.Period,
    };
}
