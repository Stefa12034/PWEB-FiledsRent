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
public class PacketController : AuthorizedController
{
    private readonly IPacketService _packetService;

    public PacketController(IUserService userService, IPacketService packetService) : base(userService)
    {
        _packetService = packetService;
    }

    // C
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<RequestResponse>> Add([FromBody] PacketAddDTO packet)
    {
        var currentUser = await GetCurrentUser();
        Console.Write("PacketAddController");

        return currentUser.Result != null ?
            this.FromServiceResponse(await _packetService.AddPacket(packet, currentUser.Result)) :
            this.ErrorMessageResult(currentUser.Error);
    }

    // R
    [Authorize]
    [HttpGet("{sportId:guid}")]
    public async Task<ActionResult<RequestResponse<PagedResponse<PacketDTO>>>> GetSportPackets([FromRoute] Guid sportId, [FromQuery] PaginationSearchQueryParams pagination) // The FromRoute attribute will bind the id from the route to this parameter.
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null ?
            this.FromServiceResponse(await _packetService.GetSportPackets(sportId, pagination)) :
            this.ErrorMessageResult<PagedResponse<PacketDTO>>(currentUser.Error);
    }

    // U
    [Authorize]
    [HttpPut]
    public async Task<ActionResult<RequestResponse>> Update([FromBody] PacketUpdateDTO packet) // The FromBody attribute indicates that the parameter is deserialized from the JSON body.
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null ?
            this.FromServiceResponse(await _packetService.UpdatePacket(packet, currentUser.Result)) :
            this.ErrorMessageResult(currentUser.Error);
    }

    // D
    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<RequestResponse>> Delete([FromRoute] Guid id)
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null ?
            this.FromServiceResponse(await _packetService.DeletePacket(id, currentUser.Result)) :
            this.ErrorMessageResult(currentUser.Error);
    }
}
