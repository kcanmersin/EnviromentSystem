﻿using System.ComponentModel;

namespace API.Contracts.Electric
{
    public class DeleteElectricRequest
    {
        [DefaultValue("7A7252C6-4558-4B12-BF21-1D2A26C1E8E9")]
        public Guid Id { get; set; }
    }
}