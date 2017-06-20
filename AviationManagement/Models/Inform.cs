using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AviationManagement.Models
{
    public class Inform
    {
        public Guid InformID { get; set; }

        public DateTime PublishTime { get; set; }

        public string Text { get; set; }
    }
}
