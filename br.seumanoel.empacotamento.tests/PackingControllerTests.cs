using br.seumanoel.empacotamento.api.Controllers;
using br.seumanoel.empacotamento.api.Data;
using br.seumanoel.empacotamento.api.Models;
using br.seumanoel.empacotamento.api.Models.Dto;
using br.seumanoel.empacotamento.api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace br.seumanoel.empacotamento.tests
{
    public class PackingControllerTests
    {
        private readonly PackingService _packingService;
        private readonly AppDbContext _context;
        private readonly PackingController _controller;

        public PackingControllerTests()
        {
            // Create the packing service directly instead of mocking it
            // If you need to control its behavior, use a mock for IBoxFactory instead
            _packingService = new PackingService(new api.Factorie.BoxFactory());
            
            // Set up in-memory database for testing
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestPackingDb")
                .Options;
            _context = new AppDbContext(options);
            
            // Clean database before each test
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            
            // Create the controller with our dependencies
            _controller = new PackingController(_packingService, _context);
        }

        [Fact]
        public async Task PackGroupedOrders_ValidInput_ReturnsOk()
        {
            // Arrange
            var input = new OrdersInputDto
            {
                Orders = new List<OrderDto>
                {
                    new OrderDto
                    {
                        OrderId = 1,
                        Products = new List<ProductDto>
                        {
                            new ProductDto
                            {
                                ProductId = "Test1",
                                Dimensions = new DimensionDto { Height = 10, Width = 10, Length = 10 }
                            }
                        }
                    }
                }
            };

            // Act
            var result = await _controller.PackGroupedOrders(input);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsAssignableFrom<List<OrderPackedDto>>(okResult.Value);
            Assert.Equal(1, returnValue[0].OrderId);
            
            // Verify database was updated
            var dbResults = await _context.PackingResults.Include(p => p.Boxes).ToListAsync();
            Assert.Single(dbResults);
        }

        [Fact]
        public async Task PackGroupedOrders_NullBoxName_HandlesCorrectly()
        {
            // Arrange
            var input = new OrdersInputDto
            {
                Orders = new List<OrderDto>
                {
                    new OrderDto
                    {
                        OrderId = 5,
                        Products = new List<ProductDto>
                        {
                            new ProductDto
                            {
                                ProductId = "Cadeira Gamer",
                                Dimensions = new DimensionDto { Height = 120, Width = 60, Length = 70 }
                            }
                        }
                    }
                }
            };

            // Act
            var result = await _controller.PackGroupedOrders(input);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsAssignableFrom<List<OrderPackedDto>>(okResult.Value);
            
            // Check that returned result has null BoxName
            Assert.Null(returnValue[0].Boxes[0].BoxName);
            
            // Check that db was updated with default name for null box
            var dbResults = await _context.PackingResults
                .Include(p => p.Boxes)
                .FirstAsync();
            
            Assert.Equal("Produto sem caixa", dbResults.Boxes[0].BoxName);
        }
        
        [Fact]
        public async Task PackGroupedOrders_EmptyInput_ReturnsBadRequest()
        {
            // Act
            var result = await _controller.PackGroupedOrders(null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }
    }
}