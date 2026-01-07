using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Shared.DTOs.ListingSocialLinks
{
    public class ListingSocialLinksDto
    {
        public int Id { get; set; }              // ListingSocialLinkId
        public int ListingId { get; set; }       // FK
        public string NetworkName { get; set; } = null!;
        public string ProfileUrl { get; set; } = null!;
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
    }
}

namespace Api.Shared.DTOs.ListingSocialLinks
{
    public class AddListingSocialLinksDTO
    {
        public int ListingId { get; set; }
        public string NetworkName { get; set; } = null!;
        public string ProfileUrl { get; set; } = null!;
        public int SortOrder { get; set; }
    }
}

namespace Api.Shared.DTOs.ListingSocialLinks
{
    public class UpdateListingSocialLinksDTO
    {
        public int Id { get; set; }               // ListingSocialLinkId
        public string NetworkName { get; set; } = null!;
        public string ProfileUrl { get; set; } = null!;
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
    }
}
