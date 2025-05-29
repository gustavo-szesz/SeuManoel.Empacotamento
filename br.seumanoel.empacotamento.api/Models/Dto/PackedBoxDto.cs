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
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Observation { get; set; }


        // Converting Box (dimensions) to Caixa X
        public static string CastNameBox(string boxName)
        {
            return boxName switch
            {
                "Box 30x40x80" => "Caixa 1",
                "Box 80x50x40" => "Caixa 2",
                "Box 50x80x60" => "Caixa 3",
                _ => boxName
            };
        }

    }
}