using System;
using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Models
{
    public class Order
    {
        public int Id { get; set; }
        [Required]
        public string CustomerName { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }
        public DateTime OrderDate { get; set; }
        [Required]
        public string DeliveryStatus { get; set; }
    }
}