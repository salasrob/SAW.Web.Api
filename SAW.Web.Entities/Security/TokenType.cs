namespace SAW.Web.Entities.Security
{
    public enum TokenType
    {
        NotSet = 0,
        TwoFactorAuth,
        NewUserConfirmation,
        PasswordReset,
        JsonWebToken,
    }
}
