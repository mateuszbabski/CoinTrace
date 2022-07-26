using Application.DTOs.Budget;
using Application.Exceptions;
using Application.Interfaces;
using Application.Services;
using AutoMapper;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Application.UnitTest.Budget
{
    public class BudgetServiceTest
    {
        private readonly BudgetService _sut;
        private readonly Mock<IBudgetRepository> _budgetRepositoryMock = new Mock<IBudgetRepository>();
        private readonly Mock<IMapper> _mapperMock = new Mock<IMapper>();
        private readonly Mock<ICurrentUserService> _userServiceMock = new Mock<ICurrentUserService>();
        public BudgetServiceTest()
        {
            _sut = new BudgetService(_mapperMock.Object, _userServiceMock.Object, _budgetRepositoryMock.Object);
        }
            

        [Fact]
        public async void GetBudgetById_WithExistId_ReturnsBudget()
        {
            //    arrange
            var budgetId = 1;
            var userId = 1;

            var newBudget = new BudgetViewModel
            {
                Id = budgetId,
                Name = "Test",
                Description = "Test",
                CreatedById = userId
            };

            _budgetRepositoryMock
                .Setup(x => x.GetBudgetById(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new Domain.Entities.Budget());

            _mapperMock
                .Setup(m => m.Map<BudgetViewModel>(It.IsAny<Domain.Entities.Budget>()))
                .Returns(newBudget);

            //    act
            var result = await _sut.GetBudgetByIdAsync(budgetId);

            //    assert
            Assert.Equal(budgetId, newBudget.Id);
            Assert.Equal(userId, newBudget.CreatedById);
            Assert.IsType<BudgetViewModel>(result);
        }


        [Fact]
        public async void GetBudgetById_WithNoExistId_ThrowsNotFoundException()
        {
            //    arrange
            var budgetId = new Random().Next(1, 10);
            _budgetRepositoryMock
            .Setup(x => x.GetBudgetById(It.IsAny<int>(), It.IsAny<int>()))
            .Throws<NotFoundException>();
            
            //    act
            var act = Assert.ThrowsAsync<NotFoundException>(() => _sut.GetBudgetByIdAsync(budgetId));
            
            //    assert
            Assert.IsType<Task<NotFoundException>>(act);
        }

        [Fact]
        public async void GetAllBudgets_WithValidData_ReturnsBudgetList()
        {
            //    arrange
            IEnumerable<Domain.Entities.Budget> budgets = new List<Domain.Entities.Budget>();
            _budgetRepositoryMock
                .Setup(x => x.GetAllBudgets(It.IsAny<int>()))
                .ReturnsAsync(budgets);
            
            var expectedBudgetList = new List<BudgetViewModel>
            {
                new BudgetViewModel
            {
                Id = 1,
                Name = "Test",
                Description = "Test",
                CreatedById = 1
            },
                    new BudgetViewModel
            {
                Id = 2,
                Name = "Test2",
                Description = "Test2",
                CreatedById = 1
            }
            };
            IEnumerable<BudgetViewModel> expectedBudgetEnumerable = expectedBudgetList;
            
            _mapperMock
                .Setup(m => m.Map<IEnumerable<BudgetViewModel>>(It.IsAny<IEnumerable<Domain.Entities.Budget>>()))
                .Returns(expectedBudgetEnumerable);

            //    act
            var budgetList = await _sut.GetAllBudgetsAsync();

            //    assert
            Assert.IsAssignableFrom<IEnumerable<BudgetViewModel>>(budgetList);
            Assert.Equal(2, budgetList.Count());
        }



        [Fact]
        public async void CreateBudget_WithValidData_ReturnsCreatedBudgetModel()
        {
            //      arrange
            var budgetModel = new CreateBudgetRequest()
            {
                Name = "Test",
                Description = "Test"
            };
            
            var expectedBudget = new Domain.Entities.Budget()
            {
                Id = 1,
                Name = "Test",
                Description = "Test",
                CreatedById = 1
            };
            
            _mapperMock
                .Setup(m => m.Map<Domain.Entities.Budget>(It.IsAny<CreateBudgetRequest>))
                .Returns(expectedBudget);

            _budgetRepositoryMock
                .Setup(x => x.CreateBudget(It.IsAny<Domain.Entities.Budget>()))
                .ReturnsAsync(expectedBudget);
                
            //      act
            var createdBudget = await _sut.CreateBudgetAsync(budgetModel);

            //      assert
            Assert.NotNull(createdBudget);
            Assert.IsType<Domain.Entities.Budget>(createdBudget);
        }

    }
}

            




            

            
            
























