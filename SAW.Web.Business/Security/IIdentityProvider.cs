using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAW.Web.Business.Security
{
    public interface IIdentityProvider<T>
    {
        T GetCurrentUserId();
    }
}
