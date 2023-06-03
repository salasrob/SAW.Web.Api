using Azure.Communication.Email;
using SAW.Web.Entities.Requests;
using System;
namespace SAW.Web.Business
{
    public interface IEmailerBusinessService
    {
        public Task<bool> SendEmail(EmailMessage email);
    }
}
