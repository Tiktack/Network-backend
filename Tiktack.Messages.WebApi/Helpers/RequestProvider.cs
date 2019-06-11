namespace Tiktack.Messaging.WebApi.Hubs
{
    public class RequestProvider
    {
        public RequestProvider(string token)
        {
            Token = token;
        }

        public string Token { get; }
    }
}