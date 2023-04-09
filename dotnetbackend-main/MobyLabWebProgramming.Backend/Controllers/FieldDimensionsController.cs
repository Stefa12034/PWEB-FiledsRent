using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;
using MobyLabWebProgramming.Infrastructure.Authorization;
using MobyLabWebProgramming.Infrastructure.Extensions;
using MobyLabWebProgramming.Infrastructure.Services.Implementations;
using MobyLabWebProgramming.Infrastructure.Services.Interfaces;
using System.Net.Mime;

namespace MobyLabWebProgramming.Backend.Controllers;

/// <summary>
/// This is a controller example to show who to work with files and form data.
/// </summary>
[ApiController] // This attribute specifies for the framework to add functionality to the controller such as binding multipart/form-data.
[Route("api/[controller]/[action]")] // The Route attribute prefixes the routes/url paths with template provides as a string, the keywords between [] are used to automatically take the controller and method name.
public class FieldDimensionsController : AuthorizedController
{
    private readonly IFieldDimensionsService _fieldDimensionsService;

    public FieldDimensionsController(IUserService userService, IFieldDimensionsService fieldDimensionsService) : base(userService)
    {
        _fieldDimensionsService = fieldDimensionsService;
    }

    // C
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<RequestResponse>> Add([FromBody] FieldDimensionsAddDTO fieldDimensions)
    {
        var currentUser = await GetCurrentUser();
        Console.Write("AddSportController");

        return currentUser.Result != null ?
            this.FromServiceResponse(await _fieldDimensionsService.AddFieldDimensions(fieldDimensions, currentUser.Result)) :
            this.ErrorMessageResult(currentUser.Error);
    }

    // R
    [Authorize]
    [HttpGet("{fieldId:guid}")]
    public async Task<ActionResult<RequestResponse<PagedResponse<FieldDimensionsDTO>>>> GetFieldDimensions([FromRoute] Guid fieldId, [FromQuery] PaginationSearchQueryParams pagination) // The FromRoute attribute will bind the id from the route to this parameter.
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null ?
            this.FromServiceResponse(await _fieldDimensionsService.GetFieldDimensions(fieldId, pagination)) :
            this.ErrorMessageResult<PagedResponse<FieldDimensionsDTO>>(currentUser.Error);
    }

    // U
    [Authorize]
    [HttpPut]
    public async Task<ActionResult<RequestResponse>> Update([FromBody] FieldDimensionsUpdateDTO fieldDimensions) // The FromBody attribute indicates that the parameter is deserialized from the JSON body.
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null ?
            this.FromServiceResponse(await _fieldDimensionsService.UpdateFieldDimensions(fieldDimensions, currentUser.Result)) :
            this.ErrorMessageResult(currentUser.Error);
    }

    // D
    [Authorize]
    [HttpDelete("{fieldId:guid}")]
    public async Task<ActionResult<RequestResponse>> Delete([FromRoute] Guid fieldId)
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null ?
            this.FromServiceResponse(await _fieldDimensionsService.DeleteFieldDimensions(fieldId, currentUser.Result)) :
            this.ErrorMessageResult(currentUser.Error);
    }
}
