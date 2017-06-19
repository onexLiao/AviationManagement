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
        ///// <summary>
        ///// 一共有
        ///// </summary>
        //public int TotalSeats
        //{
        //    get
        //    {
        //        return VipSeats + BussinessSeats + EconomySeats;
        //    }
        //}
        /// <summary>
        /// 有几行
        /// </summary>
        public int VipRows { get; set; }
        public int BussinessRows { get; set; }
        public int EconomyRows { get; set; }
        /// <summary>
        /// 这些Seat分别叫什么（一个字母就是一个座位名）
        /// </summary>
        public string VipColumnName { get; set; }
        public string BussinessColumnName { get; set; }
        public string EconomyColumnName { get; set; }
        /// <summary>
        /// 每个仓一共有多少座位
        /// </summary>
        public int VipSeats { get; set; }
        public int BussinessSeats { get; set; }
        public int EconomySeats { get; set; }
    }
}
