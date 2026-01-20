using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Shared.DTOs.Users
{

    public class UserDto
    {
        public int? userId { get; set; }
        public int? tenantId { get; set; }
        public int? roleId { get; set; }
        public string? email { get; set; }
        public string? password { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string? ImgProfile { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? CreatedByUserId { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public int? ModifiedByUserId { get; set; }

    }
    public class UpdateUserDto
    {
        public int? tenantId { get; set; }
        public int? roleId { get; set; }
        public string? email { get; set; }
        public string? password { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string? ImgProfile { get; set; }
        public bool IsActive { get; set; }

    }
}

