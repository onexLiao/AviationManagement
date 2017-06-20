﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AviationManagement.Models
{
    public class CustomerAlthorithm
    {
        public Guid ID { get; set; }

        public CustomerProfile CustomerProfile { get; set; }

        public string Account { get; set; }

        public string Password { get; set; }
    }
}
