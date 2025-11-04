using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Shared.DTOs.Identity.AspNetUsers
{
    public class User_Update
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public double Lat { get; set; } = 0;

        public double Lng { get; set; } = 0;

        public bool Notification { get; set; } = false;

        public int CountryId { get; set; } = 0;
    }
}
