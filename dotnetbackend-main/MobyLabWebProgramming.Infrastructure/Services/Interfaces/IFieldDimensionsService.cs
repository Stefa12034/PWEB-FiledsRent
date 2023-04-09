using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;

namespace MobyLabWebProgramming.Infrastructure.Services.Interfaces;

/// <summary>
/// This service is a simple service to demonstrate how to work with files.
/// </summary>
public interface IFieldDimensionsService
{
    public Task<ServiceResponse<PagedResponse<FieldDimensionsDTO>>> GetFieldDimensions(Guid fieldId, PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default);
    public Task<ServiceResponse> AddFieldDimensions(FieldDimensionsAddDTO fieldDimensions, UserDTO requestingUser, CancellationToken cancellationToken = default);
    public Task<ServiceResponse> UpdateFieldDimensions(FieldDimensionsUpdateDTO fieldDimensions, UserDTO? requestingUser = default, CancellationToken cancellationToken = default);
    public Task<ServiceResponse> DeleteFieldDimensions(Guid fieldId, UserDTO? requestingUser = default, CancellationToken cancellationToken = default);
}
