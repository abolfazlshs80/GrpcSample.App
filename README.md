
## 🧭 فهرست مطالب

1. [gRPC چیست؟](#1-gRPC-چیست)
2. [چرا gRPC؟ (مقایسه با REST)](#2-چرا-gRPC-مقایسه-با-REST)
3. [پروتکل‌های gRPC و Protocol Buffers](#3-پروتکل‌های-gRPC-و-Protocol-Buffers)
4. [ساختار فایل `.proto`](#4-ساختار-فایل-proto)
5. [نصب و راه‌اندازی gRPC در .NET](#5-نصب-و-راه‌اندازی-gRPC-در-NET)
6. [ساخت سرویس gRPC (Server)](#6-ساخت-سرویس-gRPC-Server)
7. [ساخت کلاینت gRPC (Client)](#7-ساخت-کلاینت-gRPC-Client)
8. [نمونه کامل: HelloService + ProductService](#8-نمونه-کامل-HelloService--ProductService)
9. [خطاهای رایج و رفع آنها](#9-خطاهای-رایج-و-رفع-آنها)
10. [منابع معتبر و لینک‌های مفید](#10-منابع-معتبر-و-لینک‌های-مفید)

---

## 1. gRPC چیست؟

**gRPC** (Google Remote Procedure Call) یک فریمورک **RPC** (Remote Procedure Call) است که توسط گوگل توسعه داده شده و بر پایه **HTTP/2** و **Protocol Buffers** کار می‌کند.

> 💡 به زبان ساده:  
> gRPC به شما اجازه می‌دهد که مثل اینکه یک متد روی همون برنامه خودتون فراخوانی می‌کنید، یک متد روی یک سرور دوردست (مثلاً یک سرویس میکروسرویس) فراخوانی کنید.

### 🔹 ویژگی‌های اصلی:
- **سرعت بالا** (به دلیل استفاده از HTTP/2 و Binary Serialization)
- **پشتیبانی از چندین زبان** (C#, Java, Python, Go, Node.js و ...)
- **Streaming** (فرستادن داده به صورت جریانی — یک‌طرفه، دوطرفه، ...)
- **تایپ‌شده بودن** (با استفاده از `.proto` فایل‌ها)

---

## 2. چرا gRPC؟ (مقایسه با REST)

| ویژگی | REST | gRPC |
|--------|------|------|
| پروتکل | HTTP/1.1 | HTTP/2 |
| فرمت داده | JSON (متنی) | Protocol Buffers (باینری) |
| سرعت | کندتر | سریع‌تر |
| Streaming | ندارد (یا با WebSocket) | دارد (Server-side, Client-side, Bi-directional) |
| تایپ‌شده بودن | نه (JSON بدون schema) | بله (با `.proto`) |
| کاربرد | API عمومی، وب، موبایل | میکروسرویس‌ها، سرویس‌های داخلی |

> ✅ برای **سرویس‌های داخلی** (Microservices) و **کاربردهای با عملکرد بالا** → gRPC  
> ✅ برای **API عمومی** و **وب/موبایل** → REST

---

## 3. پروتکل‌های gRPC و Protocol Buffers

### 🔹 Protocol Buffers (protobuf)

یک فرمت **binary serialization** است که توسط گوگل طراحی شده.  
فایل‌های `.proto` برای تعریف ساختار داده‌ها و سرویس‌ها استفاده می‌شوند.

#### 🔸 ساختار پایه:

```protobuf
syntax = "proto3"; // نسخه پروتکل

option csharp_namespace = "MyApp.Service.Proto"; // نام‌فضای C#

message Person {
    string name = 1;
    int32 age = 2;
}

service Greeter {
    rpc SayHello(Person) returns (Person) {}
}
```

- `message`: تعریف یک نوع داده (مثل کلاس)
- `service`: تعریف یک سرویس (مثل interface)
- `rpc`: تعریف یک متد روتین
- `=1`, `=2`: شماره فیلد (برای سریالیزه شدن صحیح)

---

## 4. ساختار فایل `.proto`

### 🔹 مثال اول: `HelloService.proto`

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

### 🔹 مثال دوم: `ProductService.proto`

```protobuf
syntax = "proto3";

option csharp_namespace = "ServerApp.Service.Proto";

message CreateProductRequestDTO {
    string Name = 1;
    string Brand = 2;
    int32 Price = 3; // تصحیح شده از Pame به Price
}

message CreateProductResponseDTO {
    string message = 1;
}

service ProductService {
    rpc CreateProduct(CreateProductRequestDTO) returns (CreateProductResponseDTO) {}
}
```

> ⚠️ توجه: در مثال شما `Pame` اشتباه تایپ شده، من آن را به `Price` تصحیح کردم.

---

## 5. نصب و راه‌اندازی gRPC در .NET

### 🔹 نیازمندی‌ها:

- .NET 6 یا بالاتر (توصیه: .NET 8)
- Visual Studio 2022 یا VS Code
- پکیج `Grpc.AspNetCore` و `Grpc.Net.Client`

### 🔹 نصب پکیج‌ها:

#### برای سرور (Web API):

```bash
dotnet add package Grpc.AspNetCore
```

#### برای کلاینت (Console یا Web):

```bash
dotnet add package Grpc.Net.Client
```

#### برای تولید کد از `.proto`:

```bash
dotnet add package Grpc.Tools --version 2.59.0
```

> ✅ این پکیج به صورت خودکار کد C# را از فایل‌های `.proto` تولید می‌کند.

---

## 6. ساخت سرویس gRPC (Server)

### 🔹 ساخت پروژه Web API با gRPC

```bash
dotnet new grpc -n GrpcSample.App.Web
cd GrpcSample.App.Web
```

### 🔹 قرار دادن فایل‌های `.proto`

فایل‌های `HelloService.proto` و `ProductService.proto` را در پوشه `Protos` قرار دهید.

### 🔹 تنظیمات فایل `.csproj` (برای تولید کد از proto)

در فایل `GrpcSample.App.Web.csproj` اضافه کنید:

```xml
<ItemGroup>
  <Protobuf Include="Protos\HelloService.proto" GrpcServices="Server" />
  <Protobuf Include="Protos\ProductService.proto" GrpcServices="Server" />
</ItemGroup>
```

> ⚙️ `GrpcServices="Server"` یعنی کد سرویس سرور را تولید کند.

### 🔹 ساخت کلاس سرویس (Implement Service)

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
        // شبیه‌سازی ذخیره محصول
        Console.WriteLine($"Product created: {request.Name}, Brand: {request.Brand}, Price: {request.Price}");

        return Task.FromResult(new CreateProductResponseDTO
        {
            Message = $"Product '{request.Name}' created successfully."
        });
    }
}
```

### 🔹 ثبت سرویس در `Program.cs`

```csharp
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<HelloGrpcService>();
app.MapGrpcService<ProductService>();

app.Run();
```

> ✅ حالا سرور شما آماده است!

---

## 7. ساخت کلاینت gRPC (Client)

### 🔹 ساخت پروژه کنسول

```bash
dotnet new console -n GrpcSample.App.Console
cd GrpcSample.App.Console
```

### 🔹 نصب پکیج‌ها

```bash
dotnet add package Grpc.Net.Client
dotnet add package Grpc.Tools
```

### 🔹 کپی فایل‌های `.proto`

فایل‌های `HelloService.proto` و `ProductService.proto` را از سرور کپی کنید و در پوشه `Protos` قرار دهید.

### 🔹 تنظیمات `.csproj` برای کلاینت

```xml
<ItemGroup>
  <Protobuf Include="Protos\HelloService.proto" GrpcServices="Client" />
  <Protobuf Include="Protos\ProductService.proto" GrpcServices="Client" />
</ItemGroup>
```

> ⚙️ `GrpcServices="Client"` یعنی کد کلاینت را تولید کند.

### 🔹 نوشتن کد کلاینت

#### ➤ `Program.cs`

```csharp
using Grpc.Net.Client;
using ServerApp.Service.Proto;

var channel = GrpcChannel.ForAddress("https://localhost:7024"); // آدرس سرور

var helloClient = new Helloservice.HelloserviceClient(channel);
var productClient = new ProductService.ProductServiceClient(channel);

// تست HelloService
var helloResponse = await helloClient.sayHelloAsync(new request { Name = "Ali" });
Console.WriteLine(helloResponse.Message);

// تست ProductService
var productResponse = await productClient.CreateProductAsync(new CreateProductRequestDTO
{
    Name = "Laptop",
    Brand = "Dell",
    Price = 1200
});
Console.WriteLine(productResponse.Message);

Console.ReadKey();
```

> ✅ اگر سرور را اجرا کرده باشید، کلاینت باید خروجی را نمایش دهد.

---

## 8. نمونه کامل: HelloService + ProductService

### 🔹 ساختار پروژه

```
GrpcSample.App/
├── GrpcSample.App.Console/       ← کلاینت
│   ├── Protos/
│   │   ├── HelloService.proto
│   │   └── ProductService.proto
│   └── Program.cs
└── GrpcSample.App.Web/           ← سرور
    ├── Protos/
    │   ├── HelloService.proto
    │   └── ProductService.proto
    ├── GRPCs/
    │   ├── HelloGrpcService.cs
    │   └── ProductService.cs
    └── Program.cs
```

### 🔹 اجرای سرور و کلاینت

1. سرور را اجرا کنید (`dotnet run` در پوشه `GrpcSample.App.Web`)
2. کلاینت را اجرا کنید (`dotnet run` در پوشه `GrpcSample.App.Console`)

> ✅ خروجی مورد انتظار:
```
Hello, Ali!
Product 'Laptop' created successfully.
```

---

## 9. خطاهای رایج و رفع آنها

| خطا | علت | راه حل |
|------|------|--------|
| `Could not find type '...'` | فایل `.proto` در سرور یا کلاینت وجود ندارد یا `GrpcServices` اشتباه است | فایل‌ها را کپی کنید و `GrpcServices="Server"` یا `"Client"` را بررسی کنید |
| `The SSL connection could not be established` | سرور با HTTPS اجرا شده ولی کلاینت اعتماد نمی‌کند | در کلاینت اضافه کنید: `AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);` یا از `http://localhost:5000` استفاده کنید |
| `Method not found` | اسم متد در `.proto` با کد C# مطابقت ندارد | حروف کوچک/بزرگ را بررسی کنید (مثلاً `sayHello` در proto → `SayHelloAsync` در C#) |
| `Port already in use` | پورت 7024 یا 5000 در حال استفاده است | در `appsettings.json` پورت را تغییر دهید یا سرور قبلی را ببندید |

---

## 10. منابع معتبر و لینک‌های مفید

✅ **مستندات رسمی Microsoft (بهترین منبع):**  
🔗 https://learn.microsoft.com/en-us/aspnet/core/grpc/

✅ **آموزش gRPC در .NET (GitHub Sample):**  
🔗 https://github.com/grpc/grpc-dotnet

✅ **پروتکل‌های gRPC و Protocol Buffers:**  
🔗 https://grpc.io/docs/languages/csharp/basics/

✅ **مثال‌های کامل و کاربردی:**  
🔗 https://github.com/grpc/grpc-dotnet/tree/main/examples

✅ **آموزش تصویری و ویدئویی (YouTube):**  
🔗 https://www.youtube.com/watch?v=VWQXKzBpE3Y (gRPC with .NET 6 by Nick Chapsas)

✅ **راهنمای تبدیل REST به gRPC:**  
🔗 https://learn.microsoft.com/en-us/aspnet/core/grpc/migration?view=aspnetcore-8.0

---

## 🎯 نتیجه‌گیری

gRPC یک ابزار قدرتمند برای ساخت سرویس‌های کارآمد و سریع در .NET است. با استفاده از `.proto` فایل‌ها، شما می‌توانید ساختار داده‌ها و رفتار سرویس‌ها را به صورت واضح و تایپ‌شده تعریف کنید.

