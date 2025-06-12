using System.ComponentModel.DataAnnotations.Schema;
using Api._2.Common.Models.Generic;

namespace Api._2.Common.Models.Security
{
    [Table("Logins")]
    public class Login : Entity
    {
        public string Ip { get; set; }
        public string TokenJwt { get; set; }
    }
}
