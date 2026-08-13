# Backend — ASP.NET Core Web API

Chưa scaffold (cần .NET SDK trên máy bạn, sandbox này không có sẵn dotnet).

## Khởi tạo lần đầu

```bash
cd backend
dotnet new webapi -n HdFarm.Api --use-controllers
dotnet new sln -n HdFarm
dotnet sln add HdFarm.Api
dotnet add HdFarm.Api package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add HdFarm.Api package Microsoft.EntityFrameworkCore.Design
```

## Cấu trúc đề xuất

```
backend/
├── HdFarm.sln
├── src/
│   ├── HdFarm.Api/            # controllers, Program.cs, DI, swagger
│   ├── HdFarm.Domain/         # entities, enums (khớp database/schema.sql)
│   ├── HdFarm.Application/    # services, DTO, business logic
│   └── HdFarm.Infrastructure/ # EF Core DbContext, migrations, tích hợp
│       ├── einvoice/          # tích hợp Viettel/VNPT/MISA e-invoice API
│       └── payment/           # tích hợp VNPay/Momo/ZaloPay
└── tests/
    └── HdFarm.Api.Tests/
```

## Kết nối database

Import `../database/schema.sql` vào PostgreSQL local trước, sau đó dùng
`dotnet ef dbcontext scaffold` hoặc tạo entity thủ công khớp schema
(khuyến nghị tạo thủ công để kiểm soát tên, tránh scaffold sinh code thừa).

## Việc cần làm đầu tiên (Giai đoạn 1)

Xem `../tasks/giai-doan-1-mvp.md`.
