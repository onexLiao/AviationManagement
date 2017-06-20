using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AviationManagement.Models.Forms
{
    /// <summary>
    /// 用于登陆和创建账号等操作
    /// </summary>
    public class AccountForm
    {
        public Guid ID { get; set; }

        public string Account { get; set; }

        public string Password { get; set; }

        public string Token { get; set; }
    }
}
