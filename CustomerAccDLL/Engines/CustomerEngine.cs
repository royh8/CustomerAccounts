using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomerAccDLL.Models;
using CustomerAccDLL.DataAccess;
namespace CustomerAccDLL.Engines
{
    public class CustomerEngine
    {
        #region Used Class To Return Or map From API
        public class InitalCreditModel
        {
            public long customerID { get; set; }
            public decimal initialCredit { get; set; }
        }
        public class AddCustomerModel
        {
            public string Name { get; set; }
            public string Surname { get; set; }
        }

        public class CustomerStatment : Customer
        {
            public List<CustomerTransactions> Transactions { get; set; }
        }

        #endregion
        public async Task<Customer> AddNewCustomer(AddCustomerModel customer)
        {
            var newCustomer = new Customer()
            {
                Name = customer.Name,
                Surname = customer.Surname,
                Id = (CustomerData.customers.Any() ? CustomerData.customers.Max(max => max.Id) : 0) + 1,
                Balance = 0
            };
            CustomerData.customers.Add(newCustomer);
            return newCustomer;
        }

        public async Task<CustomerAccount> AddNewAccount(InitalCreditModel initalCreditModel)
        {
            if (!CustomerData.customers.Where(c => c.Id == initalCreditModel.customerID).Any())
            {
                throw new Exception("Customer ID Does  Not Exists");
            }
            var newCustomerAccount = new CustomerAccount()
            {
                Id = (CustomerData.customerAccounts.Any() ? CustomerData.customerAccounts.Max(max => max.Id) : 0) + 1,
                CustomerId = initalCreditModel.customerID,
            };
            CustomerData.customerAccounts.Add(newCustomerAccount);
            if (initalCreditModel.initialCredit > 0)
            {
                var newCustomerTransactions = new CustomerTransactions()
                {
                    Id = (CustomerData.customerTransactions.Any() ? CustomerData.customerTransactions.Max(max => max.Id) : 0) + 1,
                    CustomerAccountId = newCustomerAccount.Id,
                    Amount = initalCreditModel.initialCredit,
                    Type = "C"
                };
                CustomerData.customerTransactions.Add(newCustomerTransactions);
                CustomerData.customers.Where(c => c.Id == initalCreditModel.customerID)
                    .First().Balance += initalCreditModel.initialCredit;
            }
            return newCustomerAccount;
        }

        public async Task<CustomerTransactions> AddTransaction(CustomerTransactions customerTransactions)
        {

            if (customerTransactions.Type !="C" && customerTransactions.Type != "D")
            {
                throw new Exception("type Must be C or D");
            }
            var currentAccount = CustomerData.customerAccounts.Where(w => w.Id == customerTransactions.CustomerAccountId).FirstOrDefault();
            if (currentAccount == default(CustomerAccount))
            {
                throw new Exception("Account ID Does  Not Exists");
            }
            var newCustomerTransactions = new CustomerTransactions()
            {
                Id = (CustomerData.customerTransactions.Any() ? CustomerData.customerTransactions.Max(max => max.Id) : 0) + 1,
                CustomerAccountId = customerTransactions.CustomerAccountId,
                Amount = customerTransactions.Amount,
                Type = customerTransactions.Type
            };
            CustomerData.customerTransactions.Add(newCustomerTransactions);
            CustomerData.customers.Where(c => c.Id == currentAccount.CustomerId)
                .First().Balance += (newCustomerTransactions.Type == "C" ? newCustomerTransactions.Amount : -newCustomerTransactions.Amount);

            return newCustomerTransactions;
        }

        public async Task<CustomerStatment> GetCustomerStatment(long id)
        {
            var currentCustomerInfo = DataAccess.CustomerData.customers.Where(c => c.Id == id).FirstOrDefault();
            if (currentCustomerInfo == default(Customer))
            {
                throw new Exception("Customer ID Does  Not Exists");
            }
            var accountIds = CustomerData.customerAccounts
                .Where(acc => acc.CustomerId == currentCustomerInfo.Id)
                ?.Select(acc => acc.Id).ToList();
            if (accountIds == null)
            {
                throw new Exception("No Account Exists");
            }
            var transactions = CustomerData.customerTransactions
                .Where(t => accountIds.Contains(t.CustomerAccountId));

            var customerStatment = new CustomerStatment()
            {
                Id = currentCustomerInfo.Id,
                Balance = currentCustomerInfo.Balance,
                Name = currentCustomerInfo.Name,
                Surname = currentCustomerInfo.Surname,
                Transactions = transactions.ToList()
            };
            return customerStatment;
        }

    }


}
