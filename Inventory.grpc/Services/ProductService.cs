using Grpc.Core;
using Inventory.grpc.Protos;

namespace Inventory.grpc.Services
{
    public class ProductService : ExistenceService.ExistenceServiceBase
    {
        public override Task<ProductExistenceReply> CheckExistence(ProductRequest request, ServerCallContext context)
        {
            // logica va aqui

            return Task.FromResult(new ProductExistenceReply() { PrductQty = 99 });
        }
    }
}
