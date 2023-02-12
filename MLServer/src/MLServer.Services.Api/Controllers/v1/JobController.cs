using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Diagnostics;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MLServer.Application.Interfaces;
using MLServer.Application.ViewModels.v1.Job;
using MLServer.Domain.Core.Bus;
using MLServer.Domain.Core.Notifications;
using MLServer.Services.Api.Helpers;

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

        private const long MaxFileSize = 70000000;

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

        [HttpGet("upload/{id:guid}")]
        [ProducesResponseType(typeof(OkResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BadRequestObjectResult), (int)HttpStatusCode.BadRequest)]
        public IActionResult UploadUpdates(Guid id, [FromBody] Dictionary<string, byte[]> updates)
        {
            // TODO: This filename is temporary! Change it later!
            var filePath = Path.Combine(AppContext.BaseDirectory, "jobs", id.ToString(), "update.bin");
            System.IO.File.WriteAllBytes(filePath, updates["updates"]);

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/usr/local/bin/python3.10",
                    Arguments = $"PythonScripts/update.py {id.ToString()}",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();
            process.WaitForExit();

            return Ok();
        }

        [HttpGet("download/{id:guid}/{req}")]
        [ProducesResponseType(typeof(PhysicalFileResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BadRequestObjectResult), (int)HttpStatusCode.BadRequest)]
        public IActionResult DownloadFile(Guid id, string req)
        {
            if (req != "model" && req != "data")
            {
                return BadRequest("Invalid request.");
            }

            string filePath = Path.Combine(AppContext.BaseDirectory, "jobs", id.ToString(), req);
            return PhysicalFile(filePath, MimeTypes.GetMimeType(filePath), Path.GetFileName(filePath));
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
        [RequestSizeLimit(MaxFileSize)]
        [RequestFormLimits(MultipartBodyLengthLimit = MaxFileSize)]
        [ProducesResponseType(typeof(JobViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Post([FromForm] RegisterNewJobViewModel job)
        {
            // Make sure the job status is correct.
            if (job.Status != 0)
            {
                return BadRequest("Invalid status.");
            }

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