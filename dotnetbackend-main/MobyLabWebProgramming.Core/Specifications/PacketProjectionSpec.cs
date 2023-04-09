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

public class PacketProjectionSpec : BaseSpec<PacketProjectionSpec, Packet, PacketDTO>
{
    public PacketProjectionSpec() : base()
    {
    }
    public PacketProjectionSpec(Guid sportId)
    {
        Query.Where(s => s.SportId == sportId);
    }

    public PacketProjectionSpec(Guid sportId, string name)
    {
        Query.Where(s => s.SportId == sportId && s.Name == name);
    }

    protected override Expression<Func<Packet, PacketDTO>> Spec => e => new()
    {
        Id = e.Id,
        Name = e.Name,
        Description = e.Description,
        AddedPrice = e.AddedPrice,
        SportId = e.SportId,
    };
}
