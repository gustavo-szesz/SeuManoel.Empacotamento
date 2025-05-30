using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace br.seumanoel.empacotamento.api.Models.Dto
{
    public class OrdersInputDto
    {
        // JsonPropertyName convert the property name 
        // to be equal to the name in the example (entrada.json)
        [JsonPropertyName("pedidos")]
        public List<OrderDto> Orders { get; set; }
    }
}
