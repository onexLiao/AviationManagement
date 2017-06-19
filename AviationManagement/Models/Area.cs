using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AviationManagement.Models
{
    public class Area
    {
        /// <summary>
        /// 地区代码
        /// </summary>
        public string AreaID { get; set; }
        /// <summary>
        /// 地区实际名称
        /// </summary>
        public string Name { get; set; }
    }
}
