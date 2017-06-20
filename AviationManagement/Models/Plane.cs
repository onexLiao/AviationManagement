using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AviationManagement.Models
{
    public class Plane
    {
        /// <summary>
        /// 飞机码
        /// </summary>
        public string PlaneID { get; set; }
        /// <summary>
        /// 每个仓一共有多少座位
        /// </summary>
        public int VipSeats { get; set; }
        public int BussinessSeats { get; set; }
        public int EconomySeats { get; set; }

        public int TotalSeats { get { return VipSeats + BussinessSeats + EconomySeats; } }
    }
}
