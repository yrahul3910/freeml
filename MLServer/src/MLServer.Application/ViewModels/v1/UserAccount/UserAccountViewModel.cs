using System;
using System.Collections.Generic;

namespace MLServer.Application.ViewModels.v1.UserAccount
{
    public class UserAccountViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }
    }
}