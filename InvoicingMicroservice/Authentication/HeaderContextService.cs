using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Authentication
{
    public class HeaderContextService : IHeaderContextService
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;

        public HeaderContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public IHeaderDictionary Header => _httpContextAccessor.HttpContext?.Request.Headers;

        public int GetUserDomainId()
        {
            return int.Parse(getItem("claim_id"));
        }

        public string GetUserUsername()
        {
            return getItem("claim_username").ToString();
        }

        public int GetEudId()
        {
            return int.Parse(getItem("param_eud_id"));
        }

        private StringValues getItem(string itemName)
        {
            if (Header is null)
            {
                throw new Exception("Header not define");
            }

            StringValues claim;
            bool operationStatus = Header.TryGetValue(itemName, out claim);

            if (operationStatus)
            {
                return claim;
            } else
            {
                throw new Exception($"Not found item: {itemName} in header");
            }
        }

    }
}
