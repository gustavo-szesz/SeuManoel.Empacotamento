using System.Text.Json.Serialization;

namespace br.seumanoel.empacotamento.api.Models.Dto
{
    public class OrderPackedDto
    {
        // JsonPropertyName convert the property name
        // to be equal to the name in the example (saida.json)
        [JsonPropertyName("pedido_id")]
        public int OrderId { get; set; }

        [JsonPropertyName("caixas")]

        public List<PackedBoxDto> Boxes { get; set; }

    }
}
