using System.Text.Json.Serialization;

namespace br.seumanoel.empacotamento.api.Models.Dto
{
    public class OrderPackedDto
    {
        [JsonPropertyName("pedido_id")]
        public int OrderId { get; set; }

        [JsonPropertyName("caixas")]

        public List<PackedBoxDto> Boxes { get; set; }

    }
}
