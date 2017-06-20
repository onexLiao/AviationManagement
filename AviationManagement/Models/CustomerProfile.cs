using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AviationManagement.Models
{
    public enum Sex { Male, Female, Unknown }

    public class CustomerProfile
    {
        public Guid CustomerProfileID { get; set; }

        // 外键连接
        public Guid CustomerAlthorithmID { get; set; }

        public CustomerAlthorithm CustomerAlthorithm { get; set; }

        // 个人资料
        public string EMail { get; set; }

        public Sex Sex { get; set; }

        public string NickName { get; set; }


        public IEnumerable<Ticket> Tickets { get; set; }
    }
}
