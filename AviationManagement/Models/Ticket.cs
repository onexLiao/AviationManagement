using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AviationManagement.Models
{
    public class Ticket
    {
        /// <summary>
        /// 机票的GUID
        /// </summary>
        public Guid TicketID { get; set; }
        /// <summary>
        /// 购票时间
        /// </summary>
        public DateTime BoughtTime { get; set; }
        /// <summary>
        /// 对应航班
        /// </summary>
        public Flight Flight { get; set; }
        /// <summary>
        /// 对应顾客
        /// </summary>
        public Customer Customer { get; set; }
    }
}
