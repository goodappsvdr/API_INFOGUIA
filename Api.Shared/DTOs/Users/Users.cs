using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Shared.DTOs.Users
{
    public class createUser_Input
    {
        public string? email { get; set; }
        public string? password { get; set; }

    }
    public class createUserSuperAdmin_Input
    {
        public string? email { get; set; }
        public string? password { get; set; }
        public int? roleId { get; set; }
        public string? firstName { get; set; }
        public int? userId { get; set; }

    }
}
