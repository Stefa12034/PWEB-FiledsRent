using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MobyLabWebProgramming.Infrastructure.Configurations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Infrastructure.Services.Interfaces;
using MobyLabWebProgramming.Core.Responses;
using MobyLabWebProgramming.Infrastructure.Repositories.Interfaces;
using MobyLabWebProgramming.Infrastructure.Database;
using MobyLabWebProgramming.Core.Enums;
using MobyLabWebProgramming.Core.Errors;
using System.Net;
using MobyLabWebProgramming.Core.Specifications;
using MobyLabWebProgramming.Core.Entities;
using MobyLabWebProgramming.Core.Requests;

namespace MobyLabWebProgramming.Infrastructure.Services.Implementations;

public class PacketService : IPacketService
{
    private readonly IRepository<WebAppDatabaseContext> _repository;

    /// <summary>
    /// Inject the required services through the constructor.
    /// </summary>
    public PacketService(IRepository<WebAppDatabaseContext> repository)
    {
        _repository = repository;
    }
    public async Task<ServiceResponse> AddPacket(PacketAddDTO packet, UserDTO requestingUser, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("PacketAddService");
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin) // Verify who can add the user, you can change this however you se fit.
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the admin can add sport packets!", ErrorCodes.CannotAdd));
        }

        var result = await _repository.GetAsync(new PacketProjectionSpec(packet.SportId, packet.Name), cancellationToken);

        if (result != null)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Conflict, "The name already exists for this sport!", ErrorCodes.SportAlreadyExists));
        }

        await _repository.AddAsync(new Packet
        {
            SportId = packet.SportId,
            Name = packet.Name,
            Description = packet.Description,
            AddedPrice = packet.Price
        }, cancellationToken); // A new entity is created and persisted in the database.

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse<PagedResponse<PacketDTO>>> GetSportPackets(Guid sportId, PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("GetSportPacketsService");

        var result = await _repository.PageAsync(pagination, new PacketProjectionSpec(sportId), cancellationToken);

        return ServiceResponse<PagedResponse<PacketDTO>>.ForSuccess(result);
    }

    public async Task<ServiceResponse> UpdatePacket(PacketUpdateDTO packet, UserDTO? requestingUser, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin) // Verify who can add the user, you can change this however you se fit.
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the admin can update the packet!", ErrorCodes.CannotUpdate));
        }

        var entity = await _repository.GetAsync(new PacketSpec(packet.Id), cancellationToken);

        if (entity != null)
        {
            entity.Name = packet.Name ?? entity.Name;
            entity.Description = packet.Description ?? entity.Description;
            entity.AddedPrice = packet.Price ?? entity.AddedPrice;

            await _repository.UpdateAsync(entity, cancellationToken); // Update the entity and persist the changes.
        }

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> DeletePacket(Guid id, UserDTO? requestingUser = default, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin) // Verify who can add the user, you can change this however you se fit.
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the admin can delete the packet!", ErrorCodes.CannotDelete));
        }

        await _repository.DeleteAsync<Packet>(id, cancellationToken); // Delete the entity.

        return ServiceResponse.ForSuccess();
    }
}
