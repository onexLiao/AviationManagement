using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AviationManagement.Models
{
    public class CustomerAlthorithm
    {
        public Guid CustomerAlthorithmID { get; set; }

        public Guid CustomerID { get; set; }
       // public Customer Customer { get; set; }

        public string Account { get; set; }

        public string Password { get; set; }
    }
}
