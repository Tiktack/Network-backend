namespace Tiktack.Messaging.DataAccessLayer.Entities
{
    public class UserInfoDBLayer
    {
        public int Id { get; set; }
        public string UserIdentifier { get; set; }
        public string Name { get; set; }
        public string PictureUrl { get; set; }
        public string Email { get; set; }
    }
}
