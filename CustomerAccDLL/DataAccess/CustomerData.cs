using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomerAccDLL.Models;
namespace CustomerAccDLL.DataAccess
{
    //this should the data access layer connect to a Database using EF or ADO...... 
    //For Testing will Created a static List Of Customer and transaction


    public class CustomerData
    {
        public static List<Customer> customers { get; set; } = new List<Customer>();
        public static List<CustomerAccount> customerAccounts { get; set; } = new List<CustomerAccount>();
        public static List<CustomerTransactions> customerTransactions { get; set; } = new List<CustomerTransactions>();
    }
}
