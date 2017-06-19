using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AviationManagement.Models
{
    public class Token
    {
        public Token(string userId, String token)
        {
            UserId = userId;
            TokenString = token;
        }

        // 用户 ID 
        public string UserId { get; set; }
        // 随机生成的 uuid
        public String TokenString { get; set; }
    }
}
