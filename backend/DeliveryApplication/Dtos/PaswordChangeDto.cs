﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryApplication.Dtos
{
    public class PaswordChangeDto
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
