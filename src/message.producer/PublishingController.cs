namespace message.producer
{
    using System.Threading.Tasks;
    using MassTransit;
    using Microsoft.AspNetCore.Mvc;
    using NPS.Contracts.LoadMessaging;

    [ApiController]
    public class PublishingController : ControllerBase
    {
        private readonly IBus _bus;

        public PublishingController(IBus bus)
        {
            _bus = bus;
        }

        [HttpPost]
        [Route("api/publish")]
        public async Task<ActionResult> PublishMessage()
        {
            await _bus.Publish<IMessage>(new { });
            return Ok();
        }
    }
}
