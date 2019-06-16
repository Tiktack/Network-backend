namespace Tiktack.Messaging.WebApi.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
        public string UserIdentifier { get; set; }
        public string Name { get; set; }
        public string PictureUrl { get; set; }
        public string Email { get; set; }
    }
}
