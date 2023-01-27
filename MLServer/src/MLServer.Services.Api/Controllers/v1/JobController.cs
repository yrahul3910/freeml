using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MLServer.Application.Interfaces;
using MLServer.Application.ViewModels.v1.Job;
using MLServer.Domain.Core.Bus;
using MLServer.Domain.Core.Notifications;

namespace MLServer.Services.Api.Controllers.v1
{
    [Authorize]
    [ApiController]
    [Route("api/v1/job")]
    [Produces("application/json")]
    public class JobController : ApiController
    {
        private readonly IJobAppService _jobAppService;
        private readonly DomainNotificationHandler _notifications;

        public JobController(
            IJobAppService jobAppService,
            INotificationHandler<DomainNotification> notifications,
            IMediatorHandler mediator)
            : base(notifications, mediator)
        {
            _jobAppService = jobAppService;
            _notifications = (DomainNotificationHandler)notifications;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<JobViewModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), (int)HttpStatusCode.BadRequest)]
        public IActionResult Get()
        {
            var response = _jobAppService.GetAll();

            if (IsValidOperation())
            {
                return Ok(response);
            }

            return BadRequest(_notifications.GetNotifications().Select(n => n.Value));
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(JobViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), (int)HttpStatusCode.BadRequest)]
        public IActionResult Get(Guid id)
        {
            var response = _jobAppService.GetById(id);

            if (IsValidOperation())
            {
                return Ok(response);
            }

            return BadRequest(_notifications.GetNotifications().Select(n => n.Value));
        }

        [HttpPost]
        [ProducesResponseType(typeof(JobViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Post([FromBody] RegisterNewJobViewModel job)
        {
            var response = await _jobAppService.Register(job);

            if (IsValidOperation())
            {
                return Ok(response);
            }

            return BadRequest(_notifications.GetNotifications().Select(n => n.Value));
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(IEnumerable<string>), (int)HttpStatusCode.BadRequest)]
        public IActionResult Put(Guid id, [FromBody] UpdateJobViewModel job)
        {
            job.Id = id;
            _jobAppService.Update(job);

            if (IsValidOperation())
            {
                return NoContent();
            }

            return BadRequest(_notifications.GetNotifications().Select(n => n.Value));
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(IEnumerable<string>), (int)HttpStatusCode.BadRequest)]
        public IActionResult Delete(Guid id)
        {
            _jobAppService.Remove(id);

            if (IsValidOperation())
            {
                return NoContent();
            }

            return BadRequest(_notifications.GetNotifications().Select(n => n.Value));
        }
    }
}