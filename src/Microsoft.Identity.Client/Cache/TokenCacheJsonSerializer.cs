﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Identity.Client.Cache.Items;
using Microsoft.Identity.Client.Utils;

namespace Microsoft.Identity.Client.Cache
{
    internal class TokenCacheJsonSerializer : ITokenCacheSerializer
    {
        private readonly ITokenCacheAccessor _accessor;

        public TokenCacheJsonSerializer(ITokenCacheAccessor accessor)
        {
            _accessor = accessor;
        }

        public byte[] Serialize()
        {
            var cache = new CacheRoot();
            foreach (var t in _accessor.GetAllAccessTokens())
            {
                cache.AccessTokens[t.GetKey().ToString()] = t;
            }

            foreach (var t in _accessor.GetAllRefreshTokens())
            {
                cache.RefreshTokens[t.GetKey().ToString()] = t;
            }

            foreach (var t in _accessor.GetAllIdTokens())
            {
                cache.IdTokens[t.GetKey().ToString()] = t;
            }

            foreach (var t in _accessor.GetAllAccounts())
            {
                cache.Accounts[t.GetKey().ToString()] = t;
            }

            return cache.ToString().ToByteArray();
        }

        public void Deserialize(byte[] bytes)
        {
            _accessor.Clear();

            var cache = CacheRoot.FromJsonString(CoreHelpers.ByteArrayToString(bytes));

            if (cache.AccessTokens != null)
            {
                foreach (var atItem in cache.AccessTokens.Values)
                {
                    _accessor.SaveAccessToken(atItem);
                }
            }

            if (cache.RefreshTokens != null)
            {
                foreach (var rtItem in cache.RefreshTokens.Values)
                {
                    _accessor.SaveRefreshToken(rtItem);
                }
            }

            if (cache.IdTokens != null)
            {
                foreach (var idItem in cache.IdTokens.Values)
                {
                    _accessor.SaveIdToken(idItem);
                }
            }

            if (cache.Accounts != null)
            {
                foreach (var account in cache.Accounts.Values)
                {
                    _accessor.SaveAccount(account);
                }
            }
        }
    }
}