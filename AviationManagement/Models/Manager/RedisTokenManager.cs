using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Security.Cryptography;

namespace AviationManagement.Models.Manager
{
    class RedisTokenManager : ITokenManager
    {
        private IDistributedCache _redis;

        public RedisTokenManager(IDistributedCache cache)
        {
            _redis = cache;
        }

        /// <summary> 创建一个 token 关联上指定用户 </summary>
        /// <param name="userId"> userId 指定用户的 id </param>
        /// <returns> 生成的 token </returns>
        public async Task<Token> CreateToken(string userId)
        {
            // 使用 uuid 作为源 token
            String token = Guid.NewGuid().ToString().Replace("-", "");
            var model = new Token(userId, token);
            // 存储到 redis 并设置过期时间
            await _redis.SetAsync(userId, Encoding.UTF8.GetBytes(token),
                new DistributedCacheEntryOptions() { SlidingExpiration = new TimeSpan(1, 0, 0) });
            return model;
        }
        /// <summary> 创建一个 token 关联上指定用户 </summary>
        /// <param name="userId"> userId 指定用户的 id </param>
        /// <returns> 生成的 token </returns>
        public async Task<Token> CreateToken(Guid userId)
        {
            return await CreateToken(userId.ToString());
        }
        /// <summary> 检查 token 是否有效 </summary>
        /// <param name="model"> model token </param>
        /// <returns> 是否有效 </returns>
        public async Task<bool> CheckToken(Token model)
        {
            if (model == null)
            {
                return false;
            }
            
            var bytes = await _redis.GetAsync(model.TokenString);
            string token = Encoding.UTF8.GetString(bytes);
            if (token == null || !(token == model.TokenString))
            {
                return false;
            }
            // 如果验证成功，说明此用户进行了一次有效操作，延长 token 的过期时间
            await _redis.RefreshAsync(token);
            return true;
        }
        ///// <summary> 从字符串中解析 token </summary>
        ///// <param name="authentication"> authentication 加密后的字符串 </param>
        ///// <returns> Tocken </returns>
        //public async Task<Token> GetToken(String authentication)
        //{
        //    if (authentication == null || authentication.Length == 0)
        //    {
        //        return null;
        //    }
        //    String[] param = authentication.Split('_');
        //    if (param.Length != 2)
        //    {
        //        return null;
        //    }
        //    // 使用 userId 和源 token 简单拼接成的 token，可以增加加密措施
        //    string userId = param[0];
        //    String token = param[1];
        //    return new Token(userId, token);
        //}
        /// <summary> 清除 token </summary>
        /// <param name="userId"> userId 登录用户的 id </param>
        public async Task DeleteToken(string userId)
        {
            await _redis.RemoveAsync(userId);
        }
    }
}
