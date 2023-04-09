using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;

namespace MobyLabWebProgramming.Infrastructure.Services.Interfaces;

/// <summary>
/// This service is a simple service to demonstrate how to work with files.
/// </summary>
public interface IFieldService
{
    public Task<ServiceResponse<PagedResponse<FieldDTO>>> GetFields(PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default);

    public Task<ServiceResponse> AddField(FieldAddDTO field, UserDTO requestingUser, CancellationToken cancellationToken = default);
    public Task<ServiceResponse> UpdateField(FieldUpdateDTO field, UserDTO? requestingUser = default, CancellationToken cancellationToken = default);
    public Task<ServiceResponse> DeleteField(Guid id, UserDTO? requestingUser = default, CancellationToken cancellationToken = default);
}
