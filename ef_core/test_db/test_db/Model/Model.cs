using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test_db.Model
{
        public class User
        {
            public Guid UserId { get; set; }
            public string Username { get; set; }
            public string City { get; set; }

            public List<Order> Orders { get; set; }
        }

        public class Status
        {
            public int StatusId { get; set; }
            public string StatusName { get; set; }

            public List<Order> Orders { get; set; }
        }

        public class Order
        {
            public Guid OrderId { get; set; }
            public Guid UserId { get; set; }
            public DateTime OrderDate { get; set; }
            public decimal TotalAmount { get; set; }
            public int StatusId { get; set; }

            public User User { get; set; }
            public Status Status { get; set; }
        }
}


