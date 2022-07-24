using Application.DTOs.Transaction;
using Application.Interfaces;
using Application.Services;
using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Application.UnitTest.Transaction
{
    public class TransactionServiceTest
    {
        private readonly TransactionService _sut;
        private readonly Mock<IBudgetRepository> _budgetRepositoryMock = new Mock<IBudgetRepository>();
        private readonly Mock<IMapper> _mapperMock = new Mock<IMapper>();
        private readonly Mock<ICurrentUserService> _userServiceMock = new Mock<ICurrentUserService>();
        private readonly Mock<ITransactionRepository> _transactionRepositoryMock = new Mock<ITransactionRepository>();
        public TransactionServiceTest()
        {
            _sut = new TransactionService(
                _mapperMock.Object,
                _userServiceMock.Object,
                _transactionRepositoryMock.Object,
                _budgetRepositoryMock.Object);
        }

        [Fact]
        public async void CreateTransaction_WithValidData_ReturnsCreatedTransactionModel()
        {
            //arrange
            var transactionModel = new CreateTransactionRequest()
            {
                BudgetId = 1,
                Type = "Test",
                Category = "Test",
                Value = 100,
                TransactionDate = DateTime.UtcNow,
                Description = "Test"
            };

            var expectedTransaction = new Domain.Entities.Transaction()
            {
                Id = 1,
                CreatedById = 1,
                BudgetId = 1,
                Type = "Test",
                Category = "Test",
                Value = 100,
                TransactionDate = DateTime.UtcNow,
                Description = "Test"
            };

            _mapperMock
                .Setup(m => m.Map<Domain.Entities.Transaction>(It.IsAny<CreateTransactionRequest>))
                .Returns(expectedTransaction);

            _budgetRepositoryMock
                .Setup(x => x.GetBudgetById(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new Domain.Entities.Budget());

            _transactionRepositoryMock
                .Setup(x => x.CreateTransaction(It.IsAny<Domain.Entities.Transaction>()))
                .ReturnsAsync(expectedTransaction);

            //act
            var createdTransaction = await _sut.CreateTransactionAsync(transactionModel);

            //assert
            Assert.NotNull(createdTransaction);
            Assert.Equal(1, createdTransaction.Id);
            Assert.Equal(expectedTransaction.Value, createdTransaction.Value);
            Assert.IsType<Domain.Entities.Transaction>(createdTransaction);
        }
    }
}
