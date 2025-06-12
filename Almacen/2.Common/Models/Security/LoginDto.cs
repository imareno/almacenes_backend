using Api._2.Common.Models.Generic;

namespace Api._2.Common.Models.Security
{
    public class LoginDto : EntityDto
    {
        public string Ip { get; set; }
        public string TokenJwt { get; set; }
    }
}
