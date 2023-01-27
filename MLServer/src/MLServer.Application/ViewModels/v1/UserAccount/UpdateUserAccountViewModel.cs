using System;
using System.Text.Json.Serialization;

namespace MLServer.Application.ViewModels.v1.UserAccount
{
    public class UpdateUserAccountViewModel
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}