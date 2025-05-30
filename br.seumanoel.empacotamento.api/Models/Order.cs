using System.Text.Json.Serialization;

namespace br.seumanoel.empacotamento.api.Models
{
    public class Order
    {
        // JsonPropertyName convert the property name 
        // to be equal to the name in the example (entrada.json)
        public int Id { get; set; }

        [JsonPropertyName("produtos")]
        public List<Product> Products { get; set; }

        [JsonPropertyName("pedido_id")]
        public int OrderId { get; set; }

        public Order(int id, List<Product> products)
        {
            Id = id;
            Products = products;
        }
    }
}
