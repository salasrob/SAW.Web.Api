namespace SAW.Web.Entities.Domain
{
    public class UserBase : IUserAuthData
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public object TenantId { get; set; }
    }
}
