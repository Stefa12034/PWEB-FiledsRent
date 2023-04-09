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

public class SportService : ISportService
{
    private readonly IRepository<WebAppDatabaseContext> _repository;

    /// <summary>
    /// Inject the required services through the constructor.
    /// </summary>
    public SportService(IRepository<WebAppDatabaseContext> repository)
    {
        _repository = repository;
    }
    public async Task<ServiceResponse> AddSport(AddSportDTO sport, UserDTO requestingUser, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("AddSportService");
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin) // Verify who can add the user, you can change this however you se fit.
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the admin can add sports!", ErrorCodes.CannotAdd));
        }

        var result = await _repository.GetAsync(new SportProjectionSpec(sport.Name), cancellationToken);

        if (result != null)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Conflict, "The sport already exists!", ErrorCodes.SportAlreadyExists));
        }

        await _repository.AddAsync(new Sport
        {
            Name = sport.Name,
            Description = sport.Description
        }, cancellationToken); // A new entity is created and persisted in the database.

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse<PagedResponse<SportDTO>>> GetSports(PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("GetSportsService");
        // var result = await _repository.GetAsync(new SportProjectionSpec(), cancellationToken); // Get a user using a specification on the repository.
        var result = await _repository.PageAsync(pagination, new SportProjectionSpec(), cancellationToken); // Use the specification and pagination API to get only some entities from the database.

        // AddSportDTO sportDTO = new AddSportDTO();
        return ServiceResponse<PagedResponse<SportDTO>>.ForSuccess(result);
    }

    public async Task<ServiceResponse> UpdateSport(SportUpdateDTO sport, UserDTO? requestingUser, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin) // Verify who can add the user, you can change this however you se fit.
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the admin can update the sport!", ErrorCodes.CannotUpdate));
        }

        var entity = await _repository.GetAsync(new SportSpec(sport.Id), cancellationToken);

        if (entity != null)
        {
            entity.Name = sport.Name ?? entity.Name;
            entity.Description = sport.Description ?? entity.Description;

            await _repository.UpdateAsync(entity, cancellationToken); // Update the entity and persist the changes.
        }

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> DeleteSport(Guid id, UserDTO? requestingUser = default, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin) // Verify who can add the user, you can change this however you se fit.
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the admin can delete the sport!", ErrorCodes.CannotDelete));
        }

        await _repository.DeleteAsync<Sport>(id, cancellationToken); // Delete the entity.

        return ServiceResponse.ForSuccess();
    }
}
