using DemoUsingApiWithDatabase.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoUsingApiWithDatabase.Models
{
    public class CombinedModel
    {


        public int Id { get; set; }

        
        public string Title { get; set; }

        public string Description { get; set; }

        public List<EBooks> EbooksList { get; set; }

    }
}
