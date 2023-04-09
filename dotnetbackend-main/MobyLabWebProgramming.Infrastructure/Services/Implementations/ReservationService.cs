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

public class ReservationService : IReservationService
{
    private readonly IRepository<WebAppDatabaseContext> _repository;

    /// <summary>
    /// Inject the required services through the constructor.
    /// </summary>
    public ReservationService(IRepository<WebAppDatabaseContext> repository)
    {
        _repository = repository;
    }
    public async Task<ServiceResponse> AddReservation(ReservationAddDTO reservation, UserDTO requestingUser, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("ReservationAddService");
        if (requestingUser == null || (requestingUser.Role != UserRoleEnum.Admin && requestingUser.Id != reservation.UserId)) // Verify who can add the user, you can change this however you se fit.
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the admin and the signed in user can add reservations!", ErrorCodes.CannotAdd));
        }

        var result = await _repository.GetAsync(new ReservationProjectionSpec(reservation.FieldId, reservation.StartAt, reservation.Period), cancellationToken);

        if (result != null)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Conflict, "This field is already reserved in that date at that time!", ErrorCodes.FieldAlreadyReserved));
        }

        await _repository.AddAsync(new Reservation
        {
            UserId = reservation.UserId,
            SportId = reservation.SportId,
            FieldId = reservation.FieldId,
            StartAt = reservation.StartAt,
            Period = reservation.Period,
        }, cancellationToken); // A new entity is created and persisted in the database.

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse<PagedResponse<ReservationDTO>>> GetUserReservations(Guid userId, UserDTO requestingUser, PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("GetUserReservationsService");
        if (requestingUser == null || (requestingUser.Role != UserRoleEnum.Admin && requestingUser.Id != userId)) // Verify who can add the user, you can change this however you se fit.
        {
            return ServiceResponse<PagedResponse<ReservationDTO>>.FromError(new(HttpStatusCode.Conflict, "Only the admin and the current logged in user can see the reservations!", ErrorCodes.OwnReservations));
        }
        var result = await _repository.PageAsync(pagination, new ReservationProjectionSpec(userId), cancellationToken); // Use the specification and pagination API to get only some entities from the database.

        return ServiceResponse<PagedResponse<ReservationDTO>>.ForSuccess(result);
    }

    public async Task<ServiceResponse> UpdateReservation(ReservationUpdateDTO reservation, UserDTO requestingUser, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetAsync(new ReservationSpec(reservation.Id), cancellationToken);

        if (entity != null)
        {
            if (requestingUser == null || (requestingUser.Role != UserRoleEnum.Admin && requestingUser.Id != entity.UserId)) // Verify who can add the user, you can change this however you se fit.
            {
                return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the admin and the currently signed in user can update the reservation!", ErrorCodes.CannotUpdate));
            }

            entity.StartAt = reservation.StartAt ?? entity.StartAt;
            entity.Period = reservation.Period ?? entity.Period;

            var result = await _repository.GetAsync(new ReservationProjectionSpec(entity.FieldId, entity.StartAt, entity.Period, entity.Id), cancellationToken);

            if (result != null)
            {
                return ServiceResponse.FromError(new(HttpStatusCode.Conflict, "This field is already reserved in that date at that time!", ErrorCodes.FieldAlreadyReserved));
            }

            await _repository.UpdateAsync(entity, cancellationToken); // Update the entity and persist the changes.
        }

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> DeleteReservation(Guid id, UserDTO requestingUser, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetAsync(new ReservationSpec(id), cancellationToken);
        if (entity != null)
        {
            if (requestingUser == null || (requestingUser.Role != UserRoleEnum.Admin && requestingUser.Id != entity.UserId)) // Verify who can add the user, you can change this however you se fit.
            {
                return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only the admin and the currently signed in user can delete the reservation!", ErrorCodes.CannotDelete));
            }
            await _repository.DeleteAsync<Reservation>(id, cancellationToken); // Delete the entity.
        }            
        return ServiceResponse.ForSuccess();
    }
}
