using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FinalProj.Models;

[Table("Order")]
public partial class Order
{
    [Key]
    public int OrderId { get; set; }

    public int? UserId { get; set; }

    public int? CartId { get; set; }

    public DateOnly? OrderDate { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? OrderAmount { get; set; }

    public int? OrderStatusId { get; set; }

    public DateOnly? ShippingDate { get; set; }

    [ForeignKey("CartId")]
    [InverseProperty("Orders")]
    public virtual Cart? Cart { get; set; }

    [InverseProperty("Order")]
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    [ForeignKey("OrderStatusId")]
    [InverseProperty("Orders")]
    public virtual OrderStatus? OrderStatus { get; set; }

    [InverseProperty("Order")]
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    [ForeignKey("UserId")]
    [InverseProperty("Orders")]
    public virtual User? User { get; set; }
}
