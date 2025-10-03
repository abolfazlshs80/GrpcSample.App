// See https://aka.ms/new-console-template for more information
using Grpc.Net.Client;
using ServerApp.Service.Proto;
using static ServerApp.Service.Proto.Helloservice;

Console.WriteLine("Hello, World!");
GrpcChannel channel = GrpcChannel.ForAddress("https://localhost:7208/");

HelloserviceClient helloService = new HelloserviceClient(channel);

var _request = new request { Name = "Mojtaba" };
helloService.sayHello(_request);
Console.WriteLine(helloService.sayHello(_request));

Console.WriteLine("----------------------------------------");
Console.WriteLine("-------------Product--------------------");
Console.WriteLine("----------------------------------------");

var productService=new ProductService.ProductServiceClient(channel);
var RequestCreateProduct = new CreateProductRequestDTO
{
    Brand = "signal",
    Name = "Car",
    Pame = 1
};
var ResponseCreateProduct = productService.CreateProduct(RequestCreateProduct);

Console.WriteLine(ResponseCreateProduct.Message);

Console.ReadKey();