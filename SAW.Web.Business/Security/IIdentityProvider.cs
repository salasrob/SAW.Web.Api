

namespace SAW.Web.Business.Security
{
    public interface IIdentityProvider<T>
    {
        T GetCurrentUserId();
    }
}
