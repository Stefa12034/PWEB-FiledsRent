using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;

namespace MobyLabWebProgramming.Infrastructure.Services.Interfaces;

/// <summary>
/// This service is a simple service to demonstrate how to work with files.
/// </summary>
public interface IPacketService
{
    public Task<ServiceResponse<PagedResponse<PacketDTO>>> GetSportPackets(Guid sportId, PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default);
    public Task<ServiceResponse> AddPacket(PacketAddDTO packet, UserDTO requestingUser, CancellationToken cancellationToken = default);
    public Task<ServiceResponse> UpdatePacket(PacketUpdateDTO packet, UserDTO? requestingUser = default, CancellationToken cancellationToken = default);
    public Task<ServiceResponse> DeletePacket(Guid id, UserDTO? requestingUser = default, CancellationToken cancellationToken = default);
}
