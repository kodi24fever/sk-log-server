using System.Security.Cryptography;

namespace SharkValleyServer.Services
{
    
    public static class Auth
    {
        public static bool IsValidAPIKey(HttpRequest request) {
            Microsoft.Extensions.Primitives.StringValues key;
            if (request.Headers.TryGetValue("x-api-key", out key))
            {
                return key.ToString() == "73f158e8a5d9cdc79db9e667a203eedb";
            }
            return false;
        }


        public static string? getUserId(HttpRequest request)
        {
            Microsoft.Extensions.Primitives.StringValues id;
            if (request.Headers.TryGetValue("clientId", out id))
            {
                return  id;
            }
            return null;
        }
    }
}
