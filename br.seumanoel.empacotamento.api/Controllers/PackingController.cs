using br.seumanoel.empacotamento.api.Models;
using br.seumanoel.empacotamento.api.Models.Dto;
using br.seumanoel.empacotamento.api.Service;
using Microsoft.AspNetCore.Mvc;

namespace br.seumanoel.empacotamento.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PackingController : ControllerBase
    {
        private static readonly List<Box> Boxes = new()
        {
            new Box(30, 40, 80, "Caixa 1"),
            new Box(80, 50, 40, "Caixa 2"),
            new Box(50, 80, 60, "Caixa 3")
        };

        private readonly ILogger<PackingController> _logger;
        private readonly PackingService _packingService;

        public PackingController(PackingService packingService)
        {
            _packingService = packingService;
        }

        [HttpPost("pack")]
        public ActionResult<List<OrderPackedDto>> PackOrders([FromBody] List<OrderDto> orders)
        {
            var result = _packingService.PackOrders(orders);
            return Ok(result);
        }

        [HttpPost("pack/grouped")]
        public ActionResult<List<OrderPackedDto>> PackGroupedOrders([FromBody] OrdersInputDto input)
        {
            if (input?.Orders == null)
                return BadRequest("Campo 'pedidos' é obrigatório.");

            var result = _packingService.PackOrders(input.Orders);
            return Ok(result);
        }
    }
}
