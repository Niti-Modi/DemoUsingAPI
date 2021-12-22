using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoUsingApiWithDatabase.Data
{
    public class Customers
    {

        public int Id { get; set; }
        public string CustomerName { get; set; }

        public string  ContactName { get; set; }

        public string City { get; set; }
    }
}
