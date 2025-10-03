using GrpcSample.App.Web;
using GrpcSample.App.Web.GRPCs;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGrpc();
var app = builder.Build();



app.UseHttpsRedirection();
app.UseRouting();
app.MapGet("/", () => "Hello World!");

app.UseEndpoints(endpoints =>
{
    endpoints.MapGrpcService<HelloGrpcService>();
    endpoints.MapGrpcService<ProductWebService>();

});
app.Run();
