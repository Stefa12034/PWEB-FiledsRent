using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;

namespace MobyLabWebProgramming.Infrastructure.Services.Interfaces;

/// <summary>
/// This service is a simple service to demonstrate how to work with files.
/// </summary>
public interface ISportService
{
    public Task<ServiceResponse<PagedResponse<SportDTO>>> GetSports(PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default);

    public Task<ServiceResponse> AddSport(AddSportDTO sport, UserDTO requestingUser, CancellationToken cancellationToken = default);
    public Task<ServiceResponse> UpdateSport(SportUpdateDTO sport, UserDTO? requestingUser = default, CancellationToken cancellationToken = default);
    public Task<ServiceResponse> DeleteSport(Guid id, UserDTO? requestingUser = default, CancellationToken cancellationToken = default);
}
