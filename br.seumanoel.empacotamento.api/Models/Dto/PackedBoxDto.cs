using System.Text.Json.Serialization;

namespace br.seumanoel.empacotamento.api.Models.Dto
{
    public class PackedBoxDto
    {
        [JsonPropertyName("caixa_id")]
        public string BoxName { get; set; }

        [JsonPropertyName("produtos")]
        public List<string> PackedProductIds { get; set; }

        [JsonPropertyName("observacao")]
        public string? Observation { get; set; }
    }
}