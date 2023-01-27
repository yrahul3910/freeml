using System;

namespace MLServer.Application.ViewModels.v1.UserAccount
{
    public class RegisterNewUserAccountViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}