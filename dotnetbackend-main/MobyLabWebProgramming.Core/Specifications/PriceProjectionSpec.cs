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

public class PriceProjectionSpec : BaseSpec<PriceProjectionSpec, Prices, PriceDTO>
{
    public PriceProjectionSpec() : base()
    {
    }

    public PriceProjectionSpec(Guid fieldId)
    {
        Query.Where(s => s.FieldId == fieldId);
    }

    public PriceProjectionSpec(Guid fieldId, Guid sportId)
    {
        Query.Where(s => s.FieldId == fieldId && s.SportId == sportId);
    }

    protected override Expression<Func<Prices, PriceDTO>> Spec => e => new()
    {
        Id = e.Id,
        FieldId = e.FieldId,
        SportId = e.SportId,
        Price = e.Price,
    };
}
