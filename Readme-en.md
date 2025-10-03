## 🧭 Table of Contents

1. [What is gRPC?](#1-what-is-grpc)
2. [Why gRPC? (vs. REST)](#2-why-grpc-vs-rest)
3. [gRPC Protocols and Protocol Buffers](#3-grpc-protocols-and-protocol-buffers)
4. [Structure of `.proto` Files](#4-structure-of-proto-files)
5. [Setting Up gRPC in .NET](#5-setting-up-grpc-in-net)
6. [Building a gRPC Service (Server)](#6-building-a-grpc-service-server)
7. [Building a gRPC Client](#7-building-a-grpc-client)
8. [Complete Example: HelloService + ProductService](#8-complete-example-helloservice--productservice)
9. [Common Errors and Fixes](#9-common-errors-and-fixes)
10. [Trusted Resources & References](#10-trusted-resources--references)

---

## 1. What is gRPC?

**gRPC** (Google Remote Procedure Call) is a high-performance **RPC framework** developed by Google, built on **HTTP/2** and **Protocol Buffers**.

> 💡 In simple terms:  
> gRPC lets you call a method on a remote server as if it were a local function in your own code.

### 🔹 Key Features:
- **High performance** (thanks to HTTP/2 and binary serialization)
- **Multi-language support** (C#, Java, Python, Go, Node.js, etc.)
- **Streaming** (server-side, client-side, and bidirectional)
- **Strongly typed contracts** (via `.proto` files)

---

## 2. Why gRPC? (vs. REST)

| Feature | REST | gRPC |
|--------|------|------|
| Protocol | HTTP/1.1 | HTTP/2 |
| Data Format | JSON (text-based) | Protocol Buffers (binary) |
| Speed | Slower | Faster |
| Streaming | Not natively supported | Fully supported |
| Type Safety | No (JSON is schema-less) | Yes (via `.proto`) |
| Best For | Public APIs, Web/Mobile | Internal microservices, high-performance systems |

> ✅ Use **gRPC** for internal services and performance-critical systems  
> ✅ Use **REST** for public APIs and web/mobile clients

---

## 3. gRPC Protocols and Protocol Buffers

### 🔹 Protocol Buffers (protobuf)

A **binary serialization format** developed by Google.  
`.proto` files define data structures and service contracts.

#### 🔸 Basic Structure:

```protobuf
syntax = "proto3"; // Protocol version

option csharp_namespace = "MyApp.Service.Proto"; // C# namespace

message Person {
    string name = 1;
    int32 age = 2;
}

service Greeter {
    rpc SayHello(Person) returns (Person) {}
}
```

- `message`: defines a data type (like a class)
- `service`: defines a service contract (like an interface)
- `rpc`: declares a remote method
- `=1`, `=2`: field numbers (required for serialization)

---

## 4. Structure of `.proto` Files

### 🔹 Example 1: `HelloService.proto`

```protobuf
syntax = "proto3";

option csharp_namespace = "ServerApp.Service.Proto";

message request {
    string name = 1;
}

message response {
    string message = 1;
}

service Helloservice {
    rpc sayHello(request) returns (response) {}
}
```

### 🔹 Example 2: `ProductService.proto`

```protobuf
syntax = "proto3";

option csharp_namespace = "ServerApp.Service.Proto";

message CreateProductRequestDTO {
    string Name = 1;
    string Brand = 2;
    int32 Price = 3; // Fixed typo: was "Pame"
}

message CreateProductResponseDTO {
    string message = 1;
}

service ProductService {
    rpc CreateProduct(CreateProductRequestDTO) returns (CreateProductResponseDTO) {}
}
```

> ⚠️ Note: Fixed a typo in your original example (`Pame` → `Price`).

---

## 5. Setting Up gRPC in .NET

### 🔹 Requirements:

- .NET 6 or higher (recommended: .NET 8)
- Visual Studio 2022 or VS Code
- NuGet packages: `Grpc.AspNetCore` and `Grpc.Net.Client`

### 🔹 Install Required Packages:

#### For Server (ASP.NET Core):

```bash
dotnet add package Grpc.AspNetCore
```

#### For Client (Console or Web):

```bash
dotnet add package Grpc.Net.Client
```

#### For Code Generation from `.proto`:

```bash
dotnet add package Grpc.Tools --version 2.59.0
```

> ✅ This package automatically generates C# code from `.proto` files during build.

---

## 6. Building a gRPC Service (Server)

### 🔹 Create a gRPC Project

```bash
dotnet new grpc -n GrpcSample.App.Web
cd GrpcSample.App.Web
```

### 🔹 Add `.proto` Files

Place `HelloService.proto` and `ProductService.proto` in the `Protos` folder.

### 🔹 Configure `.csproj` for Code Generation

In `GrpcSample.App.Web.csproj`, add:

```xml
<ItemGroup>
  <Protobuf Include="Protos\HelloService.proto" GrpcServices="Server" />
  <Protobuf Include="Protos\ProductService.proto" GrpcServices="Server" />
</ItemGroup>
```

> ⚙️ `GrpcServices="Server"` tells the tooling to generate server-side stubs.

### 🔹 Implement the Service

#### ➤ `HelloGrpcService.cs`

```csharp
using Grpc.Core;
using ServerApp.Service.Proto;

namespace GrpcSample.App.Web.GRPCs;

public class HelloGrpcService : Helloservice.HelloserviceBase
{
    public override Task<response> sayHello(request request, ServerCallContext context)
    {
        return Task.FromResult(new response
        {
            Message = $"Hello, {request.Name}!"
        });
    }
}
```

#### ➤ `ProductService.cs`

```csharp
using Grpc.Core;
using ServerApp.Service.Proto;

namespace GrpcSample.App.Web.GRPCs;

public class ProductService : ProductService.ProductServiceBase
{
    public override Task<CreateProductResponseDTO> CreateProduct(CreateProductRequestDTO request, ServerCallContext context)
    {
        // Simulate saving product
        Console.WriteLine($"Product created: {request.Name}, Brand: {request.Brand}, Price: {request.Price}");

        return Task.FromResult(new CreateProductResponseDTO
        {
            Message = $"Product '{request.Name}' created successfully."
        });
    }
}
```

### 🔹 Register Services in `Program.cs`

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<HelloGrpcService>();
app.MapGrpcService<ProductService>();

app.Run();
```

> ✅ Your server is now ready!

---

## 7. Building a gRPC Client

### 🔹 Create a Console Project

```bash
dotnet new console -n GrpcSample.App.Console
cd GrpcSample.App.Console
```

### 🔹 Install Packages

```bash
dotnet add package Grpc.Net.Client
dotnet add package Grpc.Tools
```

### 🔹 Copy `.proto` Files

Copy `HelloService.proto` and `ProductService.proto` into a `Protos` folder.

### 🔹 Configure `.csproj` for Client

```xml
<ItemGroup>
  <Protobuf Include="Protos\HelloService.proto" GrpcServices="Client" />
  <Protobuf Include="Protos\ProductService.proto" GrpcServices="Client" />
</ItemGroup>
```

> ⚙️ `GrpcServices="Client"` generates client-side code.

### 🔹 Write Client Code

#### ➤ `Program.cs`

```csharp
using Grpc.Net.Client;
using ServerApp.Service.Proto;

var channel = GrpcChannel.ForAddress("https://localhost:7024"); // Server address

var helloClient = new Helloservice.HelloserviceClient(channel);
var productClient = new ProductService.ProductServiceClient(channel);

// Test HelloService
var helloResponse = await helloClient.sayHelloAsync(new request { Name = "Ali" });
Console.WriteLine(helloResponse.Message);

// Test ProductService
var productResponse = await productClient.CreateProductAsync(new CreateProductRequestDTO
{
    Name = "Laptop",
    Brand = "Dell",
    Price = 1200
});
Console.WriteLine(productResponse.Message);

Console.ReadKey();
```

> ✅ If the server is running, the client will display the responses.

---

## 8. Complete Example: HelloService + ProductService

### 🔹 Project Structure

```
GrpcSample.App/
├── GrpcSample.App.Console/       ← Client
│   ├── Protos/
│   │   ├── HelloService.proto
│   │   └── ProductService.proto
│   └── Program.cs
└── GrpcSample.App.Web/           ← Server
    ├── Protos/
    │   ├── HelloService.proto
    │   └── ProductService.proto
    ├── GRPCs/
    │   ├── HelloGrpcService.cs
    │   └── ProductService.cs
    └── Program.cs
```

### 🔹 Running the Apps

1. Run the server: `dotnet run` in `GrpcSample.App.Web`
2. Run the client: `dotnet run` in `GrpcSample.App.Console`

> ✅ Expected output:
```
Hello, Ali!
Product 'Laptop' created successfully.
```

---

## 9. Common Errors and Fixes

| Error | Cause | Solution |
|------|------|--------|
| `Could not find type '...'` | Missing `.proto` file or wrong `GrpcServices` setting | Copy `.proto` files and verify `GrpcServices="Server"` or `"Client"` |
| `The SSL connection could not be established` | HTTPS mismatch or untrusted cert | Add `AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);` or use `http://localhost:5000` |
| `Method not found` | Method name mismatch between `.proto` and C# | Check casing (e.g., `sayHello` → `SayHelloAsync`) |
| `Port already in use` | Port 7024 or 5000 is occupied | Change port in `appsettings.json` or kill the previous process |

---

## 10. Trusted Resources & References

✅ **Official Microsoft Documentation (Best Source):**  
🔗 https://learn.microsoft.com/en-us/aspnet/core/grpc/

✅ **gRPC in .NET GitHub Repository:**  
🔗 https://github.com/grpc/grpc-dotnet

✅ **Protocol Buffers & gRPC Basics:**  
🔗 https://grpc.io/docs/languages/csharp/basics/

✅ **Full Working Examples:**  
🔗 https://github.com/grpc/grpc-dotnet/tree/main/examples

✅ **Video Tutorial (.NET 6):**  
🔗 https://www.youtube.com/watch?v=VWQXKzBpE3Y (by Nick Chapsas)

✅ **Migrating from REST to gRPC:**  
🔗 https://learn.microsoft.com/en-us/aspnet/core/grpc/migration?view=aspnetcore-8.0

---

## 🎯 Conclusion

gRPC is a powerful tool for building fast, efficient, and type-safe distributed systems in .NET. By defining contracts in `.proto` files, you get clear, maintainable, and language-agnostic service definitions.

**Written with care for beginners and intermediate developers alike.**  
📌 Feel free to ask questions — I'm here to help!
