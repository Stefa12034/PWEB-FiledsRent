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

public class PriceService : IPriceService
{
    private readonly IRepository<WebAppDatabaseContext> _repository;

    /// <summary>
    /// Inject the required services through the constructor.
    /// </summary>
    public PriceService(IRepository<WebAppDatabaseContext> repository)
    {
        _repository = repository;
    }
    public async Task<ServiceResponse> AddPrice(PriceAddDTO price, UserDTO requestingUser, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("AddPriceService");
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin) // Verify who can add the user, you can change this however you se fit.
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the admin can add prices!", ErrorCodes.CannotAdd));
        }

        var result = await _repository.GetAsync(new PriceSpec(price.FieldId, price.SportId), cancellationToken);

        if (result != null)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Conflict, "This sport on this field already has a price!", ErrorCodes.FieldAlreadyExists));
        }

        await _repository.AddAsync(new Prices
        {
            FieldId = price.FieldId,
            SportId = price.SportId,
            Price = price.Price
        }, cancellationToken); // A new entity is created and persisted in the database.

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse<PagedResponse<PriceDTO>>> GetFieldPrices(Guid fieldId, PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("GetFieldsPriceService");
        var result = await _repository.PageAsync(pagination, new PriceProjectionSpec(fieldId), cancellationToken); // Use the specification and pagination API to get only some entities from the database.

        return ServiceResponse<PagedResponse<PriceDTO>>.ForSuccess(result);
    }

    public async Task<ServiceResponse> UpdatePrice(PriceUpdateDTO price, UserDTO? requestingUser, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin) // Verify who can add the user, you can change this however you se fit.
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the admin can update the price!", ErrorCodes.CannotUpdate));
        }

        var entity = await _repository.GetAsync(new PriceSpec(price.FieldId, price.SportId), cancellationToken);

        if (entity != null)
        {
            entity.Price = price.Price ?? entity.Price;

            await _repository.UpdateAsync(entity, cancellationToken); // Update the entity and persist the changes.
        }

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> DeletePrice(Guid id, UserDTO? requestingUser = default, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin) // Verify who can add the user, you can change this however you se fit.
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the admin can delete the price!", ErrorCodes.CannotDelete));
        }

        await _repository.DeleteAsync<Prices>(id, cancellationToken); // Delete the entity.

        return ServiceResponse.ForSuccess();
    }
}
