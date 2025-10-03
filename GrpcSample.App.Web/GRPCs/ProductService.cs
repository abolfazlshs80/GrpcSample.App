using Grpc.Core;
using ServerApp.Service.Proto;

namespace GrpcSample.App.Web.GRPCs;

public class ProductWebService:ProductService.ProductServiceBase
{
    public override Task<CreateProductResponseDTO> CreateProduct(CreateProductRequestDTO request, ServerCallContext context)
    {
        return Task.FromResult(new CreateProductResponseDTO { Message = "Created successFully" });
    }
}
