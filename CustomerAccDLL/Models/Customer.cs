using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerAccDLL.Models
{
    public class Customer
    {
        public long  Id { get; set; }
        public string  Name { get; set; }
        public string Surname { get; set; }
        public decimal Balance { get; set; }
    }
}
