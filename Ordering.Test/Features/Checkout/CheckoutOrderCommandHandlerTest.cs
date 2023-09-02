using AutoMapper;
using Moq;
using Ordering.Application.Contracts;
using Ordering.Application.Features.Commands.Checkout;
using Ordering.Application.Features.Queries.GetOrders;
using Ordering.Application.Features.Queries.GetOrdersByUser;
using Ordering.Domain.Entities;
using System.Linq.Expressions;

namespace Ordering.Test.Features.Checkout
{
    public class CheckoutOrderCommandHandlerTest
    {
        // creamos Mocks de los objetos repository y mapper
        private readonly Mock<IGenericRepository<Order>> repositoryMock;
        private readonly Mock<IMapper> mapperMock;
        private readonly CheckoutOrderCommandHandler handler;

        public CheckoutOrderCommandHandlerTest()
        {
            repositoryMock = new Mock<IGenericRepository<Order>>();
            mapperMock = new Mock<IMapper>();
            // hacemos la instancia del handler enviandole los objetos de la inyeccion de dependencia
            handler = new CheckoutOrderCommandHandler(repositoryMock.Object, mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldAddNewOrder()
        {

            //Arrange
            var checkoutOrderCommand = new CheckoutOrderCommand()
            {
                UserName = "Test",
                Address = "Hola@hola.com",
                FirstName = "Test",
                LastName = "Test",
                PaymentMethod = 1,
                TotalPrice = 10
            };

            var orderEntity = new Order { Id = 12 };

            mapperMock.Setup(x => x.Map<Order>(checkoutOrderCommand)).Returns(orderEntity);
            repositoryMock.Setup(x => x.AddAsync(orderEntity)).ReturnsAsync(orderEntity);

            //Act
            var result = await handler.Handle(checkoutOrderCommand, CancellationToken.None);


            //Assert
            repositoryMock.Verify(r => r.AddAsync(orderEntity), Times.Once);
            mapperMock.Verify(m => m.Map<Order>(checkoutOrderCommand), Times.Once);
            Assert.Equal(result, orderEntity.Id);

        }


        [Fact]
        public async Task Handle_ValidRequest_ReturnsOrders()
        {
            // Arrange
            var username = "testUser";
            var orders = new List<Order>
            {
                new Order
                {
                    Id = 1,
                    UserName = username,
                    TotalPrice = 100.0m,
                    FirstName = "John",
                    LastName = "Doe",
                    Address = "123 Main St",
                    PaymentMethod = 1
                },
                new Order
                {
                    Id = 2,
                    UserName = username,
                    TotalPrice = 75.0m,
                    FirstName = "Jane",
                    LastName = "Smith",
                    Address = "456 Elm St",
                    PaymentMethod = 2
                },
                // Add more orders as needed for your test scenario
            };

            var genericRepositoryMock = new Mock<IGenericRepository<Order>>();
           
            
            genericRepositoryMock
                .Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Order, bool>>>()))
                .ReturnsAsync(orders);

            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(m => m.Map<List<GetOrdersViewModel>>(It.IsAny<List<Order>>()))
                .Returns((List<Order> input) => input.Select(order => new GetOrdersViewModel
                {
                    Id = order.Id,
                    UserName = order.UserName,
                    TotalPrice = order.TotalPrice,
                    // Map other properties accordingly
                }).ToList());

            var handler = new GetOrdersByUserQueryHandler(genericRepositoryMock.Object, mapperMock.Object);
            var request = new GetOrdersByUserQuery { Username = username };
            var cancellationToken = new CancellationToken();

            // Act
            var result = await handler.Handle(request, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<GetOrdersViewModel>>(result);
            Assert.Equal(orders.Count, result.Count);
            // Add more assertions as needed for your specific scenario
        }
    }
}
