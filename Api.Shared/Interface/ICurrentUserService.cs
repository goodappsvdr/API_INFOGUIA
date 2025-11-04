
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Shared.Interface
{
    public interface ICurrentUserService
    {
        string? GetUserID();
    }
}
