using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;

namespace AviationManagement.Models.Manager
{
    class RedisTokenManager : ITokenManager
    {
        private readonly WebAPIDbContext _context;
        private readonly IDistributedCache _redis;

        public RedisTokenManager(IDistributedCache cache, WebAPIDbContext context)
        {
            _redis = cache;
            _context = context;
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
            var option = new DistributedCacheEntryOptions();
            option.SetSlidingExpiration(TimeSpan.FromHours(1));
            await _redis.SetStringAsync(userId, token, option);
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
            
            var token = await _redis.GetStringAsync(model.UserId);

            if (token != (model.TokenString))
            {
                return false;
            }
            // 如果验证成功，说明此用户进行了一次有效操作，延长 token 的过期时间
            await _redis.RefreshAsync(token);
            return true;
        }
        public async Task<bool> AlthorithmCheck(Token token)
        {
            if (!await CheckToken(token))
            {
                return false;
            }

            var user = await _context.CustomerAlthorithms.SingleOrDefaultAsync(c => c.ID.ToString() == token.UserId);
            if (user == null)
            {
                return false;
            }

            if (user.Role != Role.Admin)
            {
                return false;
            }

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
