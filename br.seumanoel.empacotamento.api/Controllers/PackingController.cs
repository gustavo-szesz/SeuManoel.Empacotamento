using br.seumanoel.empacotamento.api.Data;
using br.seumanoel.empacotamento.api.Models;
using br.seumanoel.empacotamento.api.Models.Dto;
using br.seumanoel.empacotamento.api.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace br.seumanoel.empacotamento.api.Controllers
{
    [Authorize]

    [ApiController]
    [Route("[controller]")]
    public class PackingController : ControllerBase
    {
        private static readonly List<Box> Boxes = new()
        {
            new Box(30, 40, 80, "Caixa 1"), //96000 volume
            new Box(80, 50, 40, "Caixa 2"), //160000 volume
            new Box(50, 80, 60, "Caixa 3")  // 240000 volume
        };

        private static List<Order> MapToOrders(List<OrderDto> orderDtos)
        {
            return orderDtos.Select(o => new Order(
                o.OrderId,
                o.Products.Select(p => new Product(
                    p.ProductId,
                    p.Dimensions.Height,
                    p.Dimensions.Width,
                    p.Dimensions.Length
                )).ToList()
            )
            { OrderId = o.OrderId }).ToList();
        }


        private readonly AppDbContext _context;
        private readonly PackingService _packingService;

        public PackingController(PackingService packingService, AppDbContext context)
        {
            _packingService = packingService;
            _context = context;
        }

        [HttpPost("packing")]
        public async Task<ActionResult<List<OrderPackedDto>>> PackGroupedOrders([FromBody] OrdersInputDto input)
        {
            if (input?.Orders == null)
                return BadRequest("Campo 'pedidos' é obrigatório.");

            var result = _packingService.PackOrders(input.Orders);

            // Mapeia e salva no banco
            foreach (var orderPacked in result)
            {
                var packingResult = new PackingResult
                {
                    OrderId = orderPacked.OrderId,
                    Boxes = orderPacked.Boxes.Select(box => new PackedBox
                    {
                        BoxName = box.BoxName,
                        Observation = box.Observation,
                        Products = box.PackedProductIds.Select(pid => new PackedProduct
                        {
                            ProductId = pid
                        }).ToList()
                    }).ToList()
                };

                _context.PackingResults.Add(packingResult);
            }

            await _context.SaveChangesAsync();

            return Ok(result);
        }


    }
}
