using System.Text.Json.Serialization;

namespace br.seumanoel.empacotamento.api.Models.Dto
{
    public class OrderDto
    {
        [JsonPropertyName("pedido_id")]
        public int OrderId { get; set; }

        [JsonPropertyName("produtos")]
        public List<ProductDto> Products { get; set; }
    }
}
