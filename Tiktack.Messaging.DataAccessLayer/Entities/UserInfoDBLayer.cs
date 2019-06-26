namespace Tiktack.Messaging.DataAccessLayer.Entities
{
    public class UserInfoDBLayer
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string UserIdentifier { get; set; }
        public string Name { get; set; }
        public string PictureUrl { get; set; }
        public string Email { get; set; }
    }
}
