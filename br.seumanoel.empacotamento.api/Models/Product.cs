namespace br.seumanoel.empacotamento.api.Models
{
    public class Product
    {
        public string Id { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
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
    }
}
