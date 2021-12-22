using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoUsingApiWithDatabase.Models
{
    public class customersOrderModel
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public int OrderId { get; set; }
    }
}
