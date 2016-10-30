﻿using System;

namespace FaaS.Services.DataTransferModels
{
    public class Form
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public DateTime Created { get; set; }

        public string Description { get; set; }

        public Project Project { get; set; }
    }
}
