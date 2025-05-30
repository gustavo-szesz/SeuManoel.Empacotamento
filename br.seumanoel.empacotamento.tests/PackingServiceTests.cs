using br.seumanoel.empacotamento.api.Models;
using br.seumanoel.empacotamento.api.Models.Dto;
using br.seumanoel.empacotamento.api.Services;
using br.seumanoel.empacotamento.api.Interfaces;
using br.seumanoel.empacotamento.api.Factorie;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace br.seumanoel.empacotamento.tests
{
    public class PackingServiceTests
    {
        private readonly PackingService _packingService;

        public PackingServiceTests()
        {
            
            var boxFactory = new BoxFactory();
            
            _packingService = new PackingService(boxFactory);
        }

        [Fact]
        public void PackOrders_ProductTooLarge_ReturnsNullBox()
        {
            // Arrange
            var orders = new List<OrderDto>
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
            };

            // Act
            var result = _packingService.PackOrders(orders);

            // Assert
            Assert.Single(result);
            Assert.Equal(5, result[0].OrderId);
            Assert.Single(result[0].Boxes);
            Assert.Null(result[0].Boxes[0].BoxName);
            Assert.Single(result[0].Boxes[0].PackedProductIds);
            Assert.Equal("Cadeira Gamer", result[0].Boxes[0].PackedProductIds[0]);
            Assert.Contains("não cabe", result[0].Boxes[0].Observation);
        }

        [Fact]
        public void PackOrders_MultipleProducts_OptimizeBoxes()
        {
            // Arrange
            var orders = new List<OrderDto>
            {
                new OrderDto
                {
                    OrderId = 6,
                    Products = new List<ProductDto>
                    {
                        new ProductDto
                        {
                            ProductId = "Monitor",
                            Dimensions = new DimensionDto { Height = 50, Width = 60, Length = 20 }
                        },
                        new ProductDto
                        {
                            ProductId = "Notebook",
                            Dimensions = new DimensionDto { Height = 2, Width = 35, Length = 25 }
                        },
                        new ProductDto
                        {
                            ProductId = "Webcam",
                            Dimensions = new DimensionDto { Height = 7, Width = 10, Length = 5 }
                        },
                        new ProductDto
                        {
                            ProductId = "Microfone",
                            Dimensions = new DimensionDto { Height = 25, Width = 10, Length = 10 }
                        }
                    }
                }
            };

            // Act
            var result = _packingService.PackOrders(orders);

            // Assert
            Assert.Single(result);
            Assert.Equal(6, result[0].OrderId);
            Assert.Equal(2, result[0].Boxes.Count);
            
            // Check that the larger items are in a larger box
            var largeBox = result[0].Boxes.Find(box => box.PackedProductIds.Contains("Monitor"));
            Assert.NotNull(largeBox);
            Assert.True(largeBox.BoxName == "Caixa 2" || largeBox.BoxName == "Caixa 3");
            
            // Check that smaller items are grouped appropriately
            var smallItemsBox = result[0].Boxes.Find(box => box.PackedProductIds.Contains("Webcam"));
            Assert.NotNull(smallItemsBox);
        }
    }
}