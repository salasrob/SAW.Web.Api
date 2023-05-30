
namespace SAW.Web.Entities
{
    public interface IUserAuthData
    {
        int Id { get; }
        string UserName { get; }
        IEnumerable<string> Roles { get; }
        object TenantId { get; }
    }
}
