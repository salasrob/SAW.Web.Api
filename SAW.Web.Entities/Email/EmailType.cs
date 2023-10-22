
namespace SAW.Web.Entities.Email
{
    public enum EmailType
    {
        NotSet = 0,
        NewUserConfirmation,
        TwoFactorAuthentication,
        PasswordReset
    }
}
