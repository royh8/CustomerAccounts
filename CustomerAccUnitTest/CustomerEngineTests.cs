using CustomerAccDLL.Engines;
using CustomerAccDLL.DataAccess;
using CustomerAccDLL.Models;
namespace CustomerAccUnitTest
{
    public class CustomerEngineTests
    {

        private CustomerEngine customerEngine;

        [SetUp]
        public void Setup()
        {
            customerEngine = new CustomerEngine();
        }

        [Test]
        public async Task AddNewCustomer_ShouldAddNewCustomer()
        {
            var customer = new CustomerEngine.AddCustomerModel
            {
                Name = "John",
                Surname = "Doe"
            };
            var result = await customerEngine.AddNewCustomer(customer);
            Assert.IsNotNull(result);
            Assert.That(result.Name, Is.EqualTo("John"));
            Assert.That(result.Surname, Is.EqualTo("Doe"));
        }
        [Test]
        public async Task AddNewAccount_ShouldAddNewAccount()
        {
            await AddNewCustomer_ShouldAddNewCustomer();
            long customerId = 1;
            decimal initialCredit = 100;
            var result = await customerEngine.AddNewAccount(new CustomerEngine.InitalCreditModel
            {
                customerID = customerId,
                initialCredit = initialCredit
            });
            Assert.IsNotNull(result);
            Assert.That(result.CustomerId, Is.EqualTo(customerId));
            var transactions = CustomerData.customerTransactions
                .Where(t => t.CustomerAccountId == result.Id).ToList();
            Assert.That(transactions.Count, Is.EqualTo(1));
            Assert.That(transactions.First().Amount, Is.EqualTo(initialCredit));
            Assert.That(transactions.First().Type, Is.EqualTo("C"));
        }

        [Test]
        public async Task AddTransaction_ShouldAddTransaction()
        {
            await AddNewAccount_ShouldAddNewAccount();
            var account = CustomerData.customerAccounts.First();
            var initialBalance = CustomerData.customers.First(c => c.Id == account.CustomerId).Balance;
            decimal amount = 500000;
            var type = "D";

            var result = await customerEngine.AddTransaction(new CustomerTransactions
            {
                CustomerAccountId = account.Id,
                Amount = amount,
                Type = type
            });

            Assert.IsNotNull(result);
            Assert.That(result.Amount, Is.EqualTo(amount));
            Assert.That(result.Type, Is.EqualTo(type));

            var customer = CustomerData.customers.First(c => c.Id == account.CustomerId);
            Assert.That(customer.Balance, Is.EqualTo(initialBalance - amount));
        }



        [Test]
        public async Task GetCustomerStatment_ShouldReturnCustomerStatement()
        {
            await AddTransaction_ShouldAddTransaction();
            long customerId = 1;

            var result = await customerEngine.GetCustomerStatment(customerId);

            Assert.IsNotNull(result);
            Assert.That(result.Id, Is.EqualTo(customerId));

            var accountIds = CustomerData.customerAccounts
                .Where(acc => acc.CustomerId == customerId)
                ?.Select(acc => acc.Id).ToList();

            Assert.IsNotNull(accountIds);
            Assert.IsNotEmpty(accountIds);

            var transactions = CustomerData.customerTransactions
                .Where(t => accountIds.Contains(t.CustomerAccountId)).ToList();

            Assert.That(result.Transactions.Count, Is.EqualTo(transactions.Count));
        }

    }
}