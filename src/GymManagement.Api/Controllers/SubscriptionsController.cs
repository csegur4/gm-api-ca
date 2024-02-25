using GymManagement.Application.Subscriptions.Commands.CreateSubscription;
using GymManagement.Contracts.Subscriptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Api.Controllers;
[ApiController]
[Route("[controller]")]
public class SubscriptionsController : Controller
{
    private readonly ISender _mediator; // I can use a ISender interface from Mediator

    public SubscriptionsController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateSubscription(CreateSubscriptionRequest request )
    {
        var command = new CreateSubscriptionCommand(
            request.SubscriptionType.ToString(),
            request.AdminId); 
        
        //Used Mediator instead Services in App Layer (/Services) 
        //Also, used Result Pattern by ErrorOr Lib
        var createSubscription = await _mediator.Send(command);
        // var subscriptionId = _subscriptionsWriteService.CreateSubscription(
        //     request.SubscriptionType.ToString(), 
        //     request.Admin);

        // if (createSubscription.IsError)
        // {
        //     return Problem();
        // }
        // var response = new SubscriptionResponse(
        //     createSubscription.Value, 
        //     request.SubscriptionType);
        // return Ok(response);
        
        //More efficiently 
        return createSubscription.MatchFirst(
            guid => Ok(new SubscriptionResponse(guid, request.SubscriptionType)),
            error => Problem());
    }
}