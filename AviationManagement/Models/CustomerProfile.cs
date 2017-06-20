using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AviationManagement.Models
{
    public enum Sex { Male, Female, Unknown }

    public enum Prefer
    {
        BehindWindow,
        LeaveWindow,
        Smoke,
        NonSmoke,
        Vegetarian,
        MeatEater,
    }

    public class CustomerProfile
    {
        public Guid CustomerProfileID { get; set; }

        public CustomerAlthorithm CustomerAlthorithm { get; set; }

        // 个人资料
        public string EMail { get; set; }

        public Sex Sex { get; set; }

        public string NickName { get; set; }

        public double FlightMiles { get; set; }

        public int VIPLevel { get; set; }

        public Prefer Prefer { get; set; }

        public string SSR { get; set; }

        public IEnumerable<Ticket> Tickets { get; set; }
    }
}
