using Application.DTOs.Transaction;
using Application.Exceptions;
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

        [Fact]
        public async void CreateTransaction_WithBudgetIdNotExisting_ThrowsBudgetNotFound()
        {
            //arrange
            var transactionModel = new CreateTransactionRequest() { };
            var expectedTransaction = new Domain.Entities.Transaction() { };
            
            _mapperMock
                .Setup(m => m.Map<Domain.Entities.Transaction>(It.IsAny<CreateTransactionRequest>))
                .Returns(expectedTransaction);

            _budgetRepositoryMock
                .Setup(x => x.GetBudgetById(It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new NotFoundException());

            _transactionRepositoryMock
                .Setup(x => x.CreateTransaction(It.IsAny<Domain.Entities.Transaction>()))
                .ReturnsAsync(expectedTransaction);

            //act
            var act = Assert.ThrowsAsync<NotFoundException>(() => _sut.CreateTransactionAsync(transactionModel));

            //assert
            Assert.IsType<Task<NotFoundException>>(act);
        }
        [Fact]
        public async void GetTransactionById_WithIdNotExisting_ThrowsNotFoundException()
        {
            // arrange
            _transactionRepositoryMock.Setup(x => x.GetTransactionById(It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new NotFoundException());

            // act
            var act = Assert.ThrowsAsync<NotFoundException>(() => _sut.GetTransactionByIdAsync(It.IsAny<int>()));

            // assert
            Assert.IsType<Task<NotFoundException>>(act);
        }
        [Fact]
        public async void GetTransactionById_WithIdExisting_ReturnsTransactionModel()
        {
            // arrange
            var transactionModel = new TransactionViewModel()
            {
                Id = 1
            };
            _transactionRepositoryMock.Setup(x => x.GetTransactionById(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new Domain.Entities.Transaction());

            _mapperMock.Setup(m => m.Map<TransactionViewModel>(It.IsAny<Domain.Entities.Transaction>()))
                .Returns(transactionModel);

            // act
            var transaction = await _sut.GetTransactionByIdAsync(transactionModel.Id);

            // assert
            Assert.IsType<TransactionViewModel>(transaction);
            Assert.Equal(transactionModel.Id, transaction.Id);
        }
        [Fact]
        public async void GetTransactionList_WithValidRequest_ReturnsTransactionList()
        {
            // arrange 
            IEnumerable<Domain.Entities.Transaction> transactionList = new List<Domain.Entities.Transaction>();
            _transactionRepositoryMock.Setup(x => x.GetAllTransactions(It.IsAny<int>()))
                .ReturnsAsync(transactionList);

            var expectedTransactionList = new List<TransactionViewModel>
            {
                new TransactionViewModel
            {
                Id = 1,
                CreatedById = 1,
                Type = "Expense",
                Value = 100,
                Description = "Test",
            },
                    new TransactionViewModel
            {
                Id = 2,
                CreatedById = 1,
                Type = "Expense",
                Value = 100,
                Description = "Test",
            }
            };
            IEnumerable<TransactionViewModel> expectedTransactionEnumerable = expectedTransactionList;

            _mapperMock.Setup(m => m.Map<IEnumerable<TransactionViewModel>>(It.IsAny<IEnumerable<Domain.Entities.Transaction>>()))
                .Returns(expectedTransactionEnumerable);

            // act
            var transactions = await _sut.GetAllTransactionsAsync();

            // assert
            Assert.IsAssignableFrom<IEnumerable<TransactionViewModel>>(transactions);
            Assert.Equal(2, transactions.Count());
        }


    }
}

