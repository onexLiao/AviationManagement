using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AviationManagement.Models
{
    public class Customer
    {
        public Guid CustomerID { get; set; }

        public string EMail { get; set; }

        public string Sex { get; set; }

        public string NickName { get; set; }
    }
}
