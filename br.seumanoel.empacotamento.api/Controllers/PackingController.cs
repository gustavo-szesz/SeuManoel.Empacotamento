using br.seumanoel.empacotamento.api.Data;
using br.seumanoel.empacotamento.api.Models;
using br.seumanoel.empacotamento.api.Models.Dto;
using br.seumanoel.empacotamento.api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace br.seumanoel.empacotamento.api.Controllers
{

    /// <summary>
    /// Packing controller
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    [Tags("03-Pack Orders")]
    public class PackingController : ControllerBase
    {
        #region Constructor
        /// <summary>
        /// Initialize DbContext and PackingService
        /// </summary>
        private readonly AppDbContext _context;
        private readonly PackingService _packingService;

        public PackingController(PackingService packingService, AppDbContext context)
        {
            _packingService = packingService;
            _context = context;
        }
        #endregion


        #region Endpoint Pack Orders
        /// <summary>
        /// Process a list of orders and pack the objects into boxes.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>List of products and the packege that is better</returns>
        /// <response code="200">Sucesse, return the list</response>
        /// <response code="400">If the input is invalid or empty</response>
        /// <response code="401">If user is not authenticated</response>
        [HttpPost("packing-input")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<OrderPackedDto>))]
        public async Task<ActionResult<List<OrderPackedDto>>> PackGroupedOrders([FromBody] OrdersInputDto input)
        {
            if (input?.Orders == null)
                return BadRequest("Campo 'pedidos' é obrigatório.");


            var result = _packingService.PackOrders(input.Orders);

            // Map the result and save into the database
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

            return Ok(result);
        }
        #endregion
    }
}
