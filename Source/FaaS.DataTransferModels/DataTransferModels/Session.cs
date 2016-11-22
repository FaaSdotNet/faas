﻿using System;

namespace FaaS.DataTransferModels
{
    public class Session
    {
        public Guid Id { get; set; }

        public DateTime Filled { get; set; }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Filled)}: {Filled}";
        }
    }
}
