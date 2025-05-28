namespace br.seumanoel.empacotamento.api.Models
{
    public class Order
    {
        public int Id { get; set; }
        public List<Product> Products { get; private set; }

        public Order(int id, List<Product> products)
        {
            Id = id;
            Products = products;
        }


        public bool IsEmpty()
        {
            return !Products.Any();
        }
    }
}
