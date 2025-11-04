using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Shared.Interface
{
    public interface IAuditableEntity
    {

        public string? CreatedByUserID { get; set; }
        public DateTime CreatedAt { get; set; }

        public string? ModifiedByUserID { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
