using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace br.seumanoel.empacotamento.api.Models.Dto
{
    public class OrdersInputDto
    {
        [JsonPropertyName("pedidos")]
        public List<OrderDto> Orders { get; set; }
    }
}
