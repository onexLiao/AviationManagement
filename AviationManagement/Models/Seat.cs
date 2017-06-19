using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AviationManagement.Models
{
    public class Seat
    {
        public string SeatNum { get; set; }

        public string FlightID { get; set; }

        public Guid TicketID { get; set; }

        public bool Empty { get; set; }
    }
}
