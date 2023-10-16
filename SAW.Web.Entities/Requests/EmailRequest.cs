using System.ComponentModel.DataAnnotations;

namespace SAW.Web.Entities.Requests
{
    public class EmailRequest
    {
        [DataType(DataType.EmailAddress)]
        public string To { get; set; }


        [DataType(DataType.EmailAddress)]
        public string From { get; set; }

        public string Subject { get; set; }

        [StringLength(10000000, MinimumLength = 2)]
        public string Body { get; set; }
    }
}
