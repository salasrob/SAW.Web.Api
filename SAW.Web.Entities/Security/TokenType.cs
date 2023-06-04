using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAW.Web.Entities.Security
{
    public enum TokenType
    {
        NotSet = 0,
        TwoFactorAuth,
        ResetPassword,
        PasswordResent,
        JsonWebToken,
    }
}
