# ProductEfConsole

Console App học Entity Framework Core với PostgreSQL theo roadmap Week 4 - Day 1.

## Mục tiêu Day 1

- Tạo project .NET Console.
- Cấu hình EF Core với PostgreSQL.
- Tạo entity `Product`.
- Tạo `AppDbContext` và `DbSet<Product>`.
- Tạo migration `InitialCreate`.
- Apply migration để tạo database table thật.

## Công nghệ sử dụng

- C#
- .NET 10 Console App
- Entity Framework Core
- PostgreSQL
- pgAdmin 4
- User Secrets
- GitHub Actions CI

## Cấu trúc chính

```text
ProductEfConsole
├── Data/
│   └── AppDbContext.cs
├── Models/
│   └── Product.cs
├── Migrations/
│   └── InitialCreate
├── appsettings.json
├── dotnet-tools.json
├── ProductEfConsole.csproj
└── Program.cs
```

## Database

Database dùng cho bài học:

```text
product_ef_console
```

Connection string mẫu trong `appsettings.json`:

```text
Host=localhost;Port=5432;Database=product_ef_console;Username=postgres;Password=your_password
```

Không commit password thật lên GitHub. Password thật được lưu bằng User Secrets.

## Cấu hình User Secrets

Khởi tạo User Secrets nếu máy mới chưa có:

```powershell
dotnet user-secrets init
```

Set connection string thật:

```powershell
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=product_ef_console;Username=postgres;Password=YOUR_REAL_PASSWORD"
```

Kiểm tra:

```powershell
dotnet user-secrets list
```

## Cài local tool

Project dùng local tool `dotnet-ef` trong `dotnet-tools.json`.

Nếu clone project về máy mới, chạy:

```powershell
dotnet tool restore
```

Kiểm tra EF tool:

```powershell
dotnet tool run dotnet-ef --version
```

## Cách chạy project

```powershell
dotnet restore
dotnet build
dotnet run
```

## Cách chạy migration

Tạo migration mới:

```powershell
dotnet tool run dotnet-ef migrations add InitialCreate
```

Apply migration vào PostgreSQL:

```powershell
dotnet tool run dotnet-ef database update
```

Sau khi update, kiểm tra trong pgAdmin 4 có bảng:

```text
Products
__EFMigrationsHistory
```

## Nội dung đã làm Day 1

- Setup Git, branch, `.gitignore` và GitHub Actions CI.
- Tạo .NET Console project.
- Cài EF Core PostgreSQL provider.
- Tạo entity `Product` gồm `Id`, `Name`, `Price`, `Quantity`, `CreatedAt`.
- Tạo `AppDbContext` kết nối PostgreSQL.
- Cấu hình connection string bằng `appsettings.json` và User Secrets.
- Tạo và apply migration `InitialCreate`.

## Thuật ngữ đã học

- Entity: class đại diện cho table trong database.
- DbContext: class trung tâm để EF Core làm việc với database.
- DbSet: đại diện cho một table.
- Migration: lịch sử thay đổi schema database.
- User Secrets: nơi lưu secret local, không commit lên GitHub.
- CI: quy trình tự động build/check code trên GitHub.
