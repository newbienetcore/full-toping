﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Contracts.Common.Events;
using Contracts.Domains;
using Ordering.Domain.Enums;
using Ordering.Domain.OrderAggregate.Events;

namespace Ordering.Domain.Entities;

public class Order : AuditableEventEntity<Guid>
{
    [Required]
    [Column(TypeName = "varchar(250)")]
    public string UserName { get; set; }
    [Column(TypeName = "decimal(10,2)")]
    public decimal TotalPrice { get; set; }
    
    [Required]
    [Column(TypeName = "nvarchar(50)")]
    public string FirstName { get; set; }
    [Required]
    [Column(TypeName = "nvarchar(250)")]
    public string LastName { get; set; }
    [Required]
    [EmailAddress]
    [Column(TypeName = "nvarchar(250)")]
    public string EmailAddress { get; set; }
    
    [Column(TypeName = "nvarchar(max)")]
    public string ShippingAddress { get; set; }
    [Column(TypeName = "nvarchar(max)")]
    public string InvoiceAddress { get; set; }
    
    public EOrderStatus Status { get; set; }

    public Order AddedOrder()
    {
        AddDomainEvent(default!);
        return this;
    }
}