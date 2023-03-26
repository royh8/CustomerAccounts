using Microsoft.AspNetCore.Mvc;
using CustomerAccDLL.Engines;
using CustomerAccDLL.DataAccess;
using CustomerAccDLL.Models;
using static CustomerAccDLL.Engines.CustomerEngine;

namespace CustomerAccounts.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[Authorize]   if Exists Token And Security 
    public class CustomerAccountController : ControllerBase
    {
        private readonly CustomerEngine customerEngine = new CustomerEngine();

        [HttpPost("AddNewCustomer")]
        public async Task<Customer> AddNewCustomer(AddCustomerModel customer)
        {
            return await customerEngine.AddNewCustomer(customer);
        }

        [HttpPost("AddNewAccount")]
        public async Task<CustomerAccount> AddNewAccount(InitalCreditModel initalCreditModel)
        {
            return await customerEngine.AddNewAccount(initalCreditModel);
        }
        [HttpPost("AddTransaction")]
        public async Task<CustomerTransactions> AddTransaction(CustomerTransactions customerTransactions)
        {
            return await customerEngine.AddTransaction(customerTransactions);
        }

        [HttpGet("GetCustomerStatment")]
        public async Task<CustomerStatment> GetCustomerStatment(long id)
        {
            return await customerEngine.GetCustomerStatment(id);
        }

    }
}