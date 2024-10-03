using DataAccess;
using MyTrailer.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyTrailer.Services
{
    public class PaymentService
    {
        private readonly IDataAccess _dataAccess;

        public PaymentService(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess ?? throw new ArgumentNullException(nameof(dataAccess));
        }

        public async Task<Payment?> GetPaymentById(int id)
        {
            var sql = @"SELECT * FROM Payments WHERE Id = @Id";
            var parameters = new { Id = id };
            return await _dataAccess.GetById<Payment>(sql, parameters);
        }

        public async Task<List<Payment>> GetAllPayments()
        {
            var sql = @"SELECT * FROM Payments";
            return await _dataAccess.GetAll<Payment>(sql);
        }

        public async Task<int> AddPayment(Payment payment)
        {
            var sql = @"INSERT INTO Payments (CustomerId, Method, History)
                        OUTPUT INSERTED.Id
                        VALUES (@CustomerId, @Method, @History)";

            var parameters = new
            {
                payment.CustomerId,
                payment.Method,
                payment.History
            };

            return await _dataAccess.InsertAndGetId<int>(sql, parameters);
        }

        public async Task UpdatePayment(Payment payment)
        {
            var sql = @"UPDATE Payments
                        SET CustomerId = @CustomerId, Method = @Method, History = @History
                        WHERE Id = @Id";

            var parameters = new
            {
                payment.CustomerId,
                payment.Method,
                payment.History,
                payment.Id
            };

            await _dataAccess.Update(sql, parameters);
        }

        public async Task DeletePayment(int id)
        {
            var sql = @"DELETE FROM Payments WHERE Id = @Id";
            var parameters = new { Id = id };
            await _dataAccess.Delete(sql, parameters);
        }
    }
}
