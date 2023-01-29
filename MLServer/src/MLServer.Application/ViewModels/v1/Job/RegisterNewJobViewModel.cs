using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MLServer.Application.ViewModels.v1.Job
{
    public class RegisterNewJobViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        [FromForm]
        public IFormFile Model { get; set; }
        [FromForm]
        public IFormFile Dataset { get; set; }
    }
}