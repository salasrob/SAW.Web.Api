﻿namespace SAW.Web.Entities.Security
{
    public class DomainSecurityToken
    {
        public string UserToken { get; set; }
        public int UserId { get; set; }
        public int TokenType { get; set; }
    }
}
