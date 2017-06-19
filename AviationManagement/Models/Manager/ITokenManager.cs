using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AviationManagement.Models;

namespace AviationManagement.Models.Manager
{
    public interface ITokenManager
    {
        /// <summary> 创建一个 token 关联上指定用户 </summary>
        /// <param name="userId"> userId 指定用户的 id </param>
        /// <returns> 生成的 token </returns>
        Task<Token> CreateToken(string userId);
        /// <summary> 检查 token 是否有效 </summary>
        /// <param name="model"> model token </param>
        /// <returns> 是否有效 </returns>
        Task<bool> CheckToken(Token model);
        /// <summary> 从字符串中解析 token </summary>
        /// <param name="authentication"> authentication 加密后的字符串 </param>
        /// <returns> Tocken </returns>
        Task<Token> GetToken(String authentication);
        /// <summary> 清除 token </summary>
        /// <param name="userId"> userId 登录用户的 id </param>
        Task DeleteToken(string userId);
    }
}
