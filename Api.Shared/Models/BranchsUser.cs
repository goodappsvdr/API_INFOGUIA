using System;
using System.Collections.Generic;

namespace Api.Shared.Models;

public partial class BranchsUser
{
    public int BranchUserId { get; set; }

    public int? UserId { get; set; }

    public int? BranchOfficeId { get; set; }
}
