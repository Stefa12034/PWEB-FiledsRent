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

public class FieldProjectionSpec : BaseSpec<FieldProjectionSpec, Field, FieldDTO>
{
    public FieldProjectionSpec() : base()
    {
    }

    public FieldProjectionSpec(string name)
    {
        Query.Where(s => s.Name == name);
    }

    protected override Expression<Func<Field, FieldDTO>> Spec => e => new()
    {
        Id = e.Id,
        Name = e.Name,
        Location = e.Location,
        FieldDimensions = e.FieldDimensions,
        Prices = e.Prices,
    };

    /*public SportProjectionSpec(string? search)
    {
        search = !string.IsNullOrWhiteSpace(search) ? search.Trim() : null;

        if (search == null)
        {
            return;
        }

        var searchExpr = $"%{search.Replace(" ", "%")}%";

        Query.Where(e => EF.Functions.ILike(e.Name, searchExpr)); // This is an example on who database specific expressions can be used via C# expressions.
                                                                  // Note that this will be translated to the database something like "where user.Name ilike '%str%'".
    }*/
}
