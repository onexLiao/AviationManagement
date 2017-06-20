using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AviationManagement.Models
{
    public class Flight
    {
        /// <summary>
        /// 航班代码
        /// </summary>
        public Guid FligtID { get; set; }
        /// <summary>
        /// 起飞时间
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 降落时间
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 出发地
        /// </summary>
        public Area StartArea { get; set; }
        /// <summary>
        /// 目的地
        /// </summary>
        public Area Destination { get; set; }
        /// <summary>
        /// 飞机型号
        /// </summary>
        public Plane Plane { get; set; }
        /// <summary>
        /// 航班座位情况
        /// </summary>
        public ICollection<Ticket> Tickets { get; set; }
    }
}
