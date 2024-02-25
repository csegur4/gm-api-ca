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
        
        var subscriptionId = await _mediator.Send(command);
        // var subscriptionId = _subscriptionsWriteService.CreateSubscription(
        //     request.SubscriptionType.ToString(), 
        //     request.Admin);

        var response = new SubscriptionResponse(subscriptionId, request.SubscriptionType);
        
        return Ok(response);
    }
}