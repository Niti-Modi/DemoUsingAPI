using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoUsingApiWithDatabase.Data
{
    public class Orders
    {
        public int OrderId { get; set; }
        public int CustomerID { get; set; }

        public int EmployeeID { get; set; }

        public int ShipperID { get; set; }
    }
}
