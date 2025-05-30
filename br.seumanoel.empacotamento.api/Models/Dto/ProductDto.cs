using System.Text.Json.Serialization;

namespace br.seumanoel.empacotamento.api.Models.Dto
{
    public class ProductDto
    {
        // JsonPropertyName convert the property name 
        // to be equal to the name in the example (entrada.json)
        [JsonPropertyName("produto_id")]
        public string ProductId { get; set; }

        [JsonPropertyName("dimensoes")]
        public DimensionDto Dimensions { get; set; }
    }
}