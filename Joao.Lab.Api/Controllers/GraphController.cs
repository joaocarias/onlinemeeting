using Joao.Lab.Api.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;

namespace Joao.Lab.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GraphController : ControllerBase
    {
        private readonly ILogger<GraphController> _logger;
        private readonly IGraphService _graphService;

        public GraphController(ILogger<GraphController> logger, IGraphService graphService)
        {
            _logger = logger;
            _graphService = graphService;
        }

        [HttpGet]
        public async Task<IActionResult> Test()
        {
            _logger.LogInformation("Test()");

            var accessToken = await _graphService.CreateAccessToken();
            var graphClient = await _graphService.CreateClient(accessToken);

            var users = await graphClient.Users
                                .Request()
                                .GetAsync();

              var me = users.FirstOrDefault();
            //  var me = await graphClient.Me
            //                    .Request()
            //                  .GetAsync();


            var om = await graphClient.Me.OnlineMeetings.Request().AddAsync
                (
                new OnlineMeeting()
                {
                    StartDateTime = DateTime.Now.AddDays(1),
                    EndDateTime = DateTime.Now.AddDays(1).AddMinutes(30),
                    Subject = "Meu Teste"
                }
            );

            //var onlineMeeting = await graphClient.Me.OnlineMeetings.Request().AddAsync
            //(
            //    new OnlineMeeting()
            //    {
            //        StartDateTime = DateTime.Now.AddDays(1),
            //        EndDateTime = DateTime.Now.AddDays(1).AddMinutes(30),
            //        Subject = "Meu Teste"
            //    }
            //);

            return Ok(new { cont = users.Count, me = me.Id, email = me.Mail /*, om = om.Subject*/ });
            // return Ok(onlineMeeting.Subject);
        }
    }
}
