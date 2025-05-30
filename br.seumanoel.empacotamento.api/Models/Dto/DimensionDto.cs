using System.Text.Json.Serialization;

namespace br.seumanoel.empacotamento.api.Models.Dto
{
    /// <summary>
    /// DTO for dimensions of a box or product.
    /// </summary>
    public class DimensionDto
    {
        // JsonPropertyName convert the property name 
        // to be equal to the name in the example (entrada.json)
        [JsonPropertyName("altura")]
        public int Height { get; set; }

        [JsonPropertyName("largura")]
        public int Width { get; set; }

        [JsonPropertyName("comprimento")]
        public int Length { get; set; }

    }
}