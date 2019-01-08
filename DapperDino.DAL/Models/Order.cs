using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DapperDino.DAL.Models
{
    public class Order : IEntity
    {
        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        public OrderStatus Status { get; set; }

        public virtual List<ProductAmount> ProductAmounts { get; set; }

        public DateTime CreationDate { get; set; } = DateTime.Now;

        public DateTime SendDate { get; set; }
    }

    public enum OrderStatus
    {
        Open = 0,
        Completed = 1,
        Aborted = 2
    }

    public class ProductAmount:IEntity
    {

        [Required]
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public ProductEdition Product { get; set; }

        [Required, Range(0, 100)]
        public int Amount { get; set; }
    }
}
