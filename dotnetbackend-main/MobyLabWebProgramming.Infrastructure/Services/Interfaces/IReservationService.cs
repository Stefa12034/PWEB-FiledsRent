using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;

namespace MobyLabWebProgramming.Infrastructure.Services.Interfaces;

/// <summary>
/// This service is a simple service to demonstrate how to work with files.
/// </summary>
public interface IReservationService
{
    public Task<ServiceResponse<PagedResponse<ReservationDTO>>> GetUserReservations(Guid userId, UserDTO requestingUser, PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default);
    public Task<ServiceResponse> AddReservation(ReservationAddDTO reservation, UserDTO requestingUser, CancellationToken cancellationToken = default);
    public Task<ServiceResponse> UpdateReservation(ReservationUpdateDTO reservation, UserDTO requestingUser, CancellationToken cancellationToken = default);
    public Task<ServiceResponse> DeleteReservation(Guid id, UserDTO requestingUser, CancellationToken cancellationToken = default);
}
