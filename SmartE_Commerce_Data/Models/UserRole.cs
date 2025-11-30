using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FinalProj.Models;

[Table("UserRole")]
public partial class UserRole
{
    [Key]
    public int UserRoleId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? UserRoleName { get; set; }

    [InverseProperty("UserRole")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
