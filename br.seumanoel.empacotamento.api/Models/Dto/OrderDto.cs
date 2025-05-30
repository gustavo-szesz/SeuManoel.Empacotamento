using System.Text.Json.Serialization;

namespace br.seumanoel.empacotamento.api.Models.Dto
{
    public class OrderDto
    {
        // JsonPropertyName convert the property name 
        // to be equal to the name in the example (entrada.json)
        [JsonPropertyName("pedido_id")]
        public int OrderId { get; set; }

        [JsonPropertyName("produtos")]
        public List<ProductDto> Products { get; set; }
    }
}
