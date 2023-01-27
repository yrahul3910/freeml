using System;
using System.Collections.Generic;
using System.Xml.Linq;
using MLServer.Domain.Core.Models;

namespace MLServer.Domain.Models
{
    public class UserAccount : Entity
    {
        public UserAccount(Guid id, string name)
        {
            Id = id;
            Name = name;
        } 

        // Empty constructor for EF
        protected UserAccount()
        {
        }

        public string Name { get; private set; }

        public UserAccount Update(string name)
        {
            Name = name;

            return this;
        }
    }
}