﻿using System;

namespace MLServer.Application.ViewModels.v1.Job
{
    public class RegisterNewJobViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
    }
}