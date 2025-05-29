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

        private readonly AppDbContext _context;
        private readonly PackingService _packingService;

        public PackingController(PackingService packingService, AppDbContext context)
        {
            _packingService = packingService;
            _context = context;
        }


        [HttpPost("packing-input")]
        public async Task<ActionResult<List<OrderPackedDto>>> PackGroupedOrders([FromBody] OrdersInputDto input)
        {
            if (input?.Orders == null)
                return BadRequest("Campo 'pedidos' é obrigatório.");

            // Get the packing results - this maintains null boxName values in the DTO
            var result = _packingService.PackOrders(input.Orders);

            // Mapeia e salva no banco - here we handle the database constraint
            foreach (var orderPacked in result)
            {
                var packingResult = new PackingResult
                {
                    OrderId = orderPacked.OrderId,
                    Boxes = orderPacked.Boxes.Select(box => new PackedBox
                    {
                        // For database: use "Produto sem caixa" when null
                        BoxName = box.BoxName ?? "Produto sem caixa",
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

            // Return the original result (with null values intact) to match expected output
            return Ok(result);
        }




    }
}
