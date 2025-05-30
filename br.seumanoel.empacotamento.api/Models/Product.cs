using System.Text.Json.Serialization;

namespace br.seumanoel.empacotamento.api.Models
{
    /// <summary>
    /// Representa um produto que será empacotado.
    /// </summary>
    public class Product
    {
        // JsonPropertyName convert the property name 
        // to be equal to the name in the example (entrada.json)

        [JsonPropertyName("produto_id")]
        public string Id { get; set; }

        [JsonPropertyName("altura")]
        public int Height { get; set; }

        [JsonPropertyName("largura")]
        public int Width { get; set; }

        [JsonPropertyName("comprimento")]
        public int Length { get; set; }

        public Product(string id, int height, int width, int length)
        {
            Id = id;
            Height = height;
            Width = width;
            Length = length;
        }

        public int CalculateVolume()
        {
            return Height * Width * Length;
        }

        public void Add(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product), "Product cannot be null.");
            if (product.Id != Id)
                throw new InvalidOperationException("Cannot add product with different ID.");
            Height += product.Height;
            Width += product.Width;
            Length += product.Length;
        }
    }
}
