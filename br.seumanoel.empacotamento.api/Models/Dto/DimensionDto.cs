using System.Text.Json.Serialization;

namespace br.seumanoel.empacotamento.api.Models.Dto
{
    public class DimensionDto
    {
        [JsonPropertyName("altura")]
        public int Height { get; set; }

        [JsonPropertyName("largura")]
        public int Width { get; set; }

        [JsonPropertyName("comprimento")]
        public int Length { get; set; }

    }
}