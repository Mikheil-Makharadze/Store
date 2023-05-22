using Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Order
{
    public class Order : BaseEntity
    {
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public double Subtotal { get; set; }

        //Relationships
        public string UserEmail { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
