using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Shared.Data
{
    public partial class Context : DbContext
    {
        public Context(DbContextOptions<Context> options)
            : base(options)
        {
        }
    }
}
