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

public class FieldService : IFieldService
{
    private readonly IRepository<WebAppDatabaseContext> _repository;

    public FieldService(IRepository<WebAppDatabaseContext> repository)
    {
        _repository = repository;
    }
    public async Task<ServiceResponse> AddField(FieldAddDTO field, UserDTO requestingUser, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the admin can add fields!", ErrorCodes.CannotAdd));
        }

        var result = await _repository.GetAsync(new FieldProjectionSpec(field.Name), cancellationToken);

        if (result != null)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Conflict, "The field already exists!", ErrorCodes.FieldAlreadyExists));
        }

        await _repository.AddAsync(new Field
        {
            Name = field.Name,
            Location = field.Location
        }, cancellationToken);

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse<PagedResponse<FieldDTO>>> GetFields(PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default)
    {
        var result = await _repository.PageAsync(pagination, new FieldProjectionSpec(), cancellationToken);

        return ServiceResponse<PagedResponse<FieldDTO>>.ForSuccess(result);
    }

    public async Task<ServiceResponse> UpdateField(FieldUpdateDTO field, UserDTO? requestingUser, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the admin can update the field!", ErrorCodes.CannotUpdate));
        }

        var entity = await _repository.GetAsync(new FieldSpec(field.Id), cancellationToken);

        if (entity != null)
        {
            entity.Name = field.Name ?? entity.Name;
            entity.Location = field.Location ?? entity.Location;

            await _repository.UpdateAsync(entity, cancellationToken); // Update the entity and persist the changes.
        }

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> DeleteField(Guid id, UserDTO? requestingUser = default, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin) // Verify who can add the user, you can change this however you se fit.
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the admin can delete the field!", ErrorCodes.CannotDelete));
        }

        await _repository.DeleteAsync<Field>(id, cancellationToken); // Delete the entity.

        return ServiceResponse.ForSuccess();
    }
}
