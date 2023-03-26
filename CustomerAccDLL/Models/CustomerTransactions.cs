using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerAccDLL.Models
{
    public class CustomerTransactions
    {
        public long? Id { get; set; }
        public long CustomerAccountId { get; set; }
        public decimal Amount { get; set; } 
        //If transaction is debit or credit
        public string Type { get; set; }
        

    }
}
