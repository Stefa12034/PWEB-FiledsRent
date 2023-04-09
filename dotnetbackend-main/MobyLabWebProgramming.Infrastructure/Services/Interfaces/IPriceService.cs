using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;

namespace MobyLabWebProgramming.Infrastructure.Services.Interfaces;

/// <summary>
/// This service is a simple service to demonstrate how to work with files.
/// </summary>
public interface IPriceService
{
    public Task<ServiceResponse<PagedResponse<PriceDTO>>> GetFieldPrices(Guid fieldId, PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default);

    public Task<ServiceResponse> AddPrice(PriceAddDTO price, UserDTO requestingUser, CancellationToken cancellationToken = default);
    public Task<ServiceResponse> UpdatePrice(PriceUpdateDTO price, UserDTO? requestingUser = default, CancellationToken cancellationToken = default);
    public Task<ServiceResponse> DeletePrice(Guid id, UserDTO? requestingUser = default, CancellationToken cancellationToken = default);
}
