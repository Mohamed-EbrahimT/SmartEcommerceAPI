using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FinalProj.Models;

[Table("OrderStatus")]
public partial class OrderStatus
{
    [Key]
    public int OrderStatusId { get; set; }

    [StringLength(50)]
    public string? OrderStatusName { get; set; }

    [InverseProperty("OrderStatus")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
