using DataAccess;
using MyTrailer.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyTrailer.Services
{
    public class CustomerService
    {
        private readonly IDataAccess _dataAccess;

        public CustomerService(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess ?? throw new ArgumentNullException(nameof(dataAccess));
        }

        public async Task<Customer?> GetCustomerById(int id)
        {
            var sql = @"SELECT * FROM Customer WHERE Id = @Id";
            var parameters = new { Id = id };
            return await _dataAccess.GetById<Customer>(sql, parameters);
        }

        public async Task<List<Customer>> GetAllCustomers()
        {
            var sql = @"SELECT * FROM Customer";
            return await _dataAccess.GetAll<Customer>(sql);
        }

        public async Task<int> AddCustomer(Customer customer)
        {
            var sql = @"INSERT INTO Customer (Name, Email, PhoneNumber)
                        OUTPUT INSERTED.Id
                        VALUES (@Name, @Email, @PhoneNumber)";

            var parameters = new
            {
                customer.Name,
                customer.Email,
                customer.PhoneNumber
            };

            return await _dataAccess.InsertAndGetId<int>(sql, parameters);
        }

        public async Task UpdateCustomer(Customer customer)
        {
            var sql = @"UPDATE Customer
                        SET Name = @Name, Email = @Email, PhoneNumber = @PhoneNumber
                        WHERE Id = @Id";

            var parameters = new
            {
                customer.Name,
                customer.Email,
                customer.PhoneNumber,
                customer.Id
            };

            await _dataAccess.Update(sql, parameters);
        }

        public async Task DeleteCustomer(int id)
        {
            var sql = @"DELETE FROM Customer WHERE Id = @Id";
            var parameters = new { Id = id };
            await _dataAccess.Delete(sql, parameters);
        }

        public async Task ApplyLateFee(int customerId, decimal lateFee) {
            // Assuming there is a Payment system to track fees
            var payment = new Payment {
                CustomerId = customerId,
                Method = "Late Fee",
                History = $"Late return fee of {lateFee:C} applied."
            };

            await _dataAccess.Insert("INSERT INTO Payment (CustomerId, Method, History) VALUES (@CustomerId, @Method, @History)", payment);
        }
    }
}
