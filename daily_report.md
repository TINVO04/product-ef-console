# Daily Report - Week 4 Day 1

## Hôm nay đã làm

- Setup project `ProductEfConsole` bằng .NET Console.
- Kết nối repository GitHub và làm việc trên branch Day 1.
- Cấu hình `.gitignore` để không commit file local/tài liệu học.
- Setup GitHub Actions CI để chạy `dotnet restore` và `dotnet build`.
- Cài EF Core packages cho PostgreSQL.
- Tạo entity `Product`.
- Tạo `AppDbContext` và `DbSet<Product>`.
- Cấu hình connection string bằng `appsettings.json` và User Secrets.
- Fix lỗi `dotnet ef` bằng local tool `dotnet-tools.json`.
- Tạo migration `InitialCreate` và apply vào PostgreSQL.

## File/class chính

- `Models/Product.cs`: entity sản phẩm, map với bảng `Products`.
- `Data/AppDbContext.cs`: DbContext dùng để kết nối EF Core với PostgreSQL.
- `Migrations/`: chứa migration tạo schema database.
- `appsettings.json`: chứa connection string mẫu, không chứa password thật.
- `dotnet-tools.json`: khai báo local tool `dotnet-ef`.

## Lỗi đã gặp và cách xử lý

### 1. `dotnet ef` không chạy được

Lỗi:

```text
dotnet-ef does not exist
```

Nguyên nhân:

- Global tool `dotnet-ef` đã cài nhưng terminal không nhận PATH.

Cách xử lý:

- Tạo local tool manifest.
- Cài `dotnet-ef` local cho project.
- Dùng lệnh:

```powershell
dotnet tool run dotnet-ef --version
```

### 2. Không commit password database thật

Vấn đề:

- Connection string có password thật không nên đưa lên GitHub.

Cách xử lý:

- Dùng User Secrets để lưu connection string thật.
- `appsettings.json` chỉ để password mẫu `your_password`.

## Lệnh quan trọng đã dùng

```powershell
dotnet build
dotnet tool restore
dotnet tool run dotnet-ef --version
dotnet tool run dotnet-ef migrations add InitialCreate
dotnet tool run dotnet-ef database update
```

## Kết quả cuối ngày

- Project build thành công.
- Migration `InitialCreate` đã được tạo.
- PostgreSQL đã có bảng `Products`.
- Database có bảng `__EFMigrationsHistory` để theo dõi migration.
- Code đã được commit theo từng bước nhỏ.

## Phần cần nắm để vấn đáp

- EF Core là gì.
- Entity là gì.
- `DbContext` dùng để làm gì.
- `DbSet<Product>` đại diện cho gì.
- Migration `Up` và `Down` khác nhau thế nào.
- Vì sao không commit password thật.
- Vì sao dùng User Secrets.

## Việc tiếp theo Day 2

- Học CRUD với EF Core.
- Viết các thao tác Add, GetAll, GetById, Update, Delete.
- Tách Repository và Service theo roadmap.
