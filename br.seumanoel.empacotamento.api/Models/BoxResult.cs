namespace br.seumanoel.empacotamento.api.Models
{
    public class BoxResult
    {
        public string BoxName { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int Length { get; set; }
        public List<Product> Products { get; set; }

        public int RemainingVolume { get; set; }

        public BoxResult(string boxName, int height, int width, int length, List<Product> products, int remainingVolume)
        {
            BoxName = boxName;
            Height = height;
            Width = width;
            Length = length;
            Products = products;
            RemainingVolume = height * width * length;
        }
    }
}
