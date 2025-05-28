using System.Text.Json.Serialization;

namespace br.seumanoel.empacotamento.api.Models.Dto
{
    public class ProductDto
    {
        [JsonPropertyName("produto_id")]
        public string ProductId { get; set; }

        [JsonPropertyName("dimensoes")]
        public DimensionDto Dimensions { get; set; }
    }
}