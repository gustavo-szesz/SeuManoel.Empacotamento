using br.seumanoel.empacotamento.api.Interfaces;
using br.seumanoel.empacotamento.api.Models;
using br.seumanoel.empacotamento.api.Models.Dto;

namespace br.seumanoel.empacotamento.api.Service
{
    public class PackingService
    {
        private readonly IBoxFactory _boxFactory;
        public PackingService(IBoxFactory boxFactory)
        {
            _boxFactory = boxFactory;
        }
        public List<OrderPackedDto> PackOrders(List<OrderDto> orders)
        {
            var availableBoxes = _boxFactory.GetAllBoxes();
            var result = new List<OrderPackedDto>();

            foreach (var order in orders)
            {
                var packedBoxes = new List<PackedBoxDto>();
                var productsToPack = new Queue<ProductDto>(order.Products);

                while (productsToPack.Count > 0)
                {
                    var selectedBox = availableBoxes
                        .OrderBy(b => b.Volume)
                        .FirstOrDefault(box =>
                            productsToPack.Any(p =>
                                box.CanHave(new Product(
                                    p.ProductId,
                                    p.Dimensions.Height,
                                    p.Dimensions.Width,
                                    p.Dimensions.Length
                                ))
                            )
                        );

                    if (selectedBox == null)
                        break;

                    var packedBox = new PackedBoxDto
                    {
                        BoxName = selectedBox.Name,
                        PackedProductIds = new List<string>()
                    };

                    var remainingProducts = new Queue<ProductDto>();

                    while (productsToPack.Count > 0)
                    {
                        var product = productsToPack.Dequeue();
                        var productEntity = new Product(
                            product.ProductId,
                            product.Dimensions.Height,
                            product.Dimensions.Width,
                            product.Dimensions.Length
                        );

                        if (selectedBox.CanHave(productEntity))
                        {
                            packedBox.PackedProductIds.Add(product.ProductId);
                            selectedBox.AddProduct(productEntity); // Atualiza volume ocupado
                        }
                        else
                        {
                            remainingProducts.Enqueue(product);
                        }
                    }

                    productsToPack = remainingProducts;
                    packedBoxes.Add(packedBox);
                }

                result.Add(new OrderPackedDto
                {
                    OrderId = order.OrderId,
                    Boxes = packedBoxes
                });
            }

            return result;
        }

    }
}
