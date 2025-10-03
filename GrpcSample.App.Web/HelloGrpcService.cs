using Grpc.Core;
using ServerApp.Service.Proto;

namespace GrpcSample.App.Web
{
    public class HelloGrpcService: Helloservice.HelloserviceBase
    {
        public override async Task<response> sayHello(request request, ServerCallContext context)
        {
            return await Task.FromResult(new response { Message=request.Name });
        }
    }
}
