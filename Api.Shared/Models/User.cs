using System;
using System.Collections.Generic;

namespace Api.Shared.Models;

public partial class User
{
    public int UserId { get; set; }

    public int? TenantId { get; set; }

    public int RoleId { get; set; }

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? ImgProfile { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? CreatedByUserId { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public int? ModifiedByUserId { get; set; }

    public virtual ICollection<Category> CategoryCreatedByUsers { get; set; } = new List<Category>();

    public virtual ICollection<Category> CategoryModifiedByUsers { get; set; } = new List<Category>();

    public virtual User? CreatedByUser { get; set; }

    public virtual ICollection<User> InverseCreatedByUser { get; set; } = new List<User>();

    public virtual ICollection<User> InverseModifiedByUser { get; set; } = new List<User>();

    public virtual User? ModifiedByUser { get; set; }

    public virtual Role Role { get; set; } = null!;

    public virtual Tenant? Tenant { get; set; }

    public virtual ICollection<Tenant> TenantCreatedByUsers { get; set; } = new List<Tenant>();

    public virtual ICollection<Tenant> TenantModifiedByUsers { get; set; } = new List<Tenant>();
}
