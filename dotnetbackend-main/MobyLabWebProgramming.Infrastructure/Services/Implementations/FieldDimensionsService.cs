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

public class FieldDimensionsService : IFieldDimensionsService
{
    private readonly IRepository<WebAppDatabaseContext> _repository;

    public FieldDimensionsService(IRepository<WebAppDatabaseContext> repository)
    {
        _repository = repository;
    }
    public async Task<ServiceResponse> AddFieldDimensions(FieldDimensionsAddDTO fieldDimensions, UserDTO requestingUser, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the admin can add fields dimensions!", ErrorCodes.CannotAdd));
        }

        var result = await _repository.GetAsync(new FieldDimensionsProjectionSpec(fieldDimensions.FieldId), cancellationToken);

        if (result != null)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Conflict, "The field already has dimensions!", ErrorCodes.FieldAlreadyHasDimensions));
        }

        await _repository.AddAsync(new FieldDimensions
        {
            FieldId = fieldDimensions.FieldId,
            Width = fieldDimensions.Width,
            Length = fieldDimensions.Length
        }, cancellationToken);

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse<PagedResponse<FieldDimensionsDTO>>> GetFieldDimensions(Guid fieldId, PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default)
    {
        var result = await _repository.PageAsync(pagination, new FieldDimensionsProjectionSpec(fieldId), cancellationToken);

        return ServiceResponse<PagedResponse<FieldDimensionsDTO>>.ForSuccess(result);
    }

    public async Task<ServiceResponse> UpdateFieldDimensions(FieldDimensionsUpdateDTO fieldDimensions, UserDTO? requestingUser, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the admin can update the field dimensions!", ErrorCodes.CannotUpdate));
        }

        var entity = await _repository.GetAsync(new FieldDimensionsProjectionSpec(fieldDimensions.FieldId), cancellationToken);

        if (entity != null)
        {
            var fieldDimensionsEntity = await _repository.GetAsync(new FieldDimensionsSpec(entity.Id), cancellationToken);
            if (fieldDimensionsEntity != null)
            {
                fieldDimensionsEntity.Width = fieldDimensions.Width ?? fieldDimensionsEntity.Width;
                fieldDimensionsEntity.Length = fieldDimensions.Length ?? fieldDimensionsEntity.Length;

                await _repository.UpdateAsync(fieldDimensionsEntity, cancellationToken);
            }
            else
            {
                return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Field dimensions not found!", ErrorCodes.FieldDimaensionsNotFound));
            }
        }
        else
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Field not found!", ErrorCodes.FieldNotFound));
        }

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> DeleteFieldDimensions(Guid fieldId, UserDTO? requestingUser = default, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the admin can delete the field dimensions!", ErrorCodes.CannotDelete));
        }

        var entity = await _repository.GetAsync(new FieldDimensionsProjectionSpec(fieldId), cancellationToken);
        if (entity != null)
        {
            await _repository.DeleteAsync<FieldDimensions>(entity.Id, cancellationToken);
        }
        else
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Field not found!", ErrorCodes.FieldNotFound));
        }

        return ServiceResponse.ForSuccess();
    }
}
