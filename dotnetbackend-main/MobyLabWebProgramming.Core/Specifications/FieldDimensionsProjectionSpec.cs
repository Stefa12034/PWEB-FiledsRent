using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MobyLabWebProgramming.Core.Specifications;

public class FieldDimensionsProjectionSpec : BaseSpec<FieldDimensionsProjectionSpec, FieldDimensions, FieldDimensionsDTO>
{
    public FieldDimensionsProjectionSpec() : base()
    {
    }

    public FieldDimensionsProjectionSpec(Guid fieldId)
    {
        Query.Where(s => s.FieldId == fieldId);
    }

    protected override Expression<Func<FieldDimensions, FieldDimensionsDTO>> Spec => e => new()
    {
        Id = e.Id,
        FieldId = e.FieldId,
        Width = e.Width,
        Length = e.Length,
    };
}
