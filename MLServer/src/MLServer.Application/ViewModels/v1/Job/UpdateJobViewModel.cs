using System;
using System.Text.Json.Serialization;

namespace MLServer.Application.ViewModels.v1.Job
{
    public class UpdateJobViewModel
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Owner { get; set; }
        public int Status { get; set; }
    }
}