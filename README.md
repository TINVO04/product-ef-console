# ProductEfConsole

Console App học Entity Framework Core với PostgreSQL theo roadmap Week 4.

## Mục tiêu

Project dùng để thực hành:

- EF Core với PostgreSQL.
- DbContext, DbSet, Entity và Migration.
- CRUD database bằng Repository Pattern và Service Layer.
- Cấu hình connection string an toàn bằng User Secrets.

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
├── Repositories/
│   ├── IProductRepository.cs
│   └── ProductRepository.cs
├── Services/
│   └── ProductService.cs
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

## Nội dung đã làm Day 2

- Tạo `IProductRepository` để định nghĩa CRUD contract cho Product.
- Tạo `ProductRepository` để thao tác database bằng EF Core.
- Viết đủ các thao tác CRUD:
  - `AddAsync`
  - `GetAllAsync`
  - `GetByIdAsync`
  - `UpdateAsync`
  - `DeleteAsync`
- Tạo `ProductService` để service gọi repository.
- Cập nhật `Program.cs` để test CRUD flow với database thật.
- Xử lý trường hợp không tìm thấy sản phẩm khi update/delete bằng kết quả `bool` và message rõ ràng.

## Repository Pattern

Repository Pattern giúp tách logic truy cập database ra khỏi `Program.cs`.

Flow hiện tại:

```text
Program.cs -> ProductService -> IProductRepository/ProductRepository -> AppDbContext -> PostgreSQL
```

Vai trò từng phần:

- `Program.cs`: chạy thử flow CRUD.
- `ProductService`: xử lý logic nghiệp vụ đơn giản.
- `IProductRepository`: interface định nghĩa các thao tác CRUD.
- `ProductRepository`: implement CRUD bằng EF Core.
- `AppDbContext`: cấu hình EF Core và kết nối PostgreSQL.

## CRUD flow đã test

Khi chạy:

```powershell
dotnet run
```

Chương trình test các bước:

1. Add product mẫu `Keyboard`.
2. Lấy danh sách sản phẩm.
3. Tìm sản phẩm theo `Id`.
4. Update `Price` và `Quantity`.
5. Delete sản phẩm.
6. Lấy lại danh sách sau khi delete.

Kết quả mong đợi:

```text
=== Product EF Console - Day 2 CRUD Test ===
Added product: Keyboard
Product list:
Found product by id ...
Updated product successfully.
Deleted product successfully.
Product list after delete:
Done.
```

## Thuật ngữ đã học

- Entity: class đại diện cho table trong database.
- DbContext: class trung tâm để EF Core làm việc với database.
- DbSet: đại diện cho một table.
- Migration: lịch sử thay đổi schema database.
- User Secrets: nơi lưu secret local, không commit lên GitHub.
- CI: quy trình tự động build/check code trên GitHub.
- Repository Pattern: pattern tách logic truy cập database ra khỏi tầng chạy chương trình.
- Service Layer: tầng xử lý nghiệp vụ, gọi repository thay vì gọi DbContext trực tiếp.
- Async/Await: cách viết code bất đồng bộ khi làm việc với database.

## Checklist Day 2

- [x] Có `IProductRepository`.
- [x] Có `ProductRepository`.
- [x] Có `ProductService`.
- [x] Có Add product.
- [x] Có GetAll products.
- [x] Có GetById product.
- [x] Có Update product.
- [x] Có Delete product.
- [x] `Program.cs` gọi Service để test CRUD.
- [x] Project build thành công.
- [x] CRUD chạy với PostgreSQL database thật.
