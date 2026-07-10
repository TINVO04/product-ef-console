# ProductEfConsole

Console App học Entity Framework Core với PostgreSQL theo roadmap Week 4.

## Mục tiêu

Project dùng để thực hành:

- EF Core với PostgreSQL.
- DbContext, DbSet, Entity và Migration.
- CRUD database bằng Repository Pattern và Service Layer.
- LINQ query: search, pagination và sorting.
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

## Nội dung đã làm Day 3

- Thêm method `GetPagedProductsAsync` vào `IProductRepository`, `ProductRepository` và `ProductService`.
- Search sản phẩm theo keyword bằng LINQ `Where`.
- Phân trang sản phẩm bằng `Skip` và `Take`.
- Sắp xếp sản phẩm theo `price` hoặc `name` bằng `OrderBy`.
- Dùng `IQueryable` để build query từng bước trước khi gọi `ToListAsync`.
- Cập nhật `Program.cs` để test search, pagination và sort với database thật.

## Sơ đồ luồng hoạt động Day 3

```text
Người chạy chương trình
        |
        v
Program.cs
        |
        | gọi GetPagedProductsAsync(search, page, pageSize, sortBy)
        v
ProductService
        |
        | validate page/pageSize
        | gọi repository
        v
IProductRepository
        |
        v
ProductRepository
        |
        | tạo IQueryable<Product>
        | áp dụng Where nếu có search
        | áp dụng OrderBy theo price/name
        | áp dụng Skip/Take để phân trang
        | gọi ToListAsync
        v
AppDbContext
        |
        v
PostgreSQL database
```

Giải thích ngắn:

- `Program.cs` chỉ dùng để chạy thử và in kết quả ra console.
- `ProductService` kiểm tra input cơ bản như `page` và `pageSize`.
- `ProductRepository` là nơi viết LINQ query để làm việc với database.
- `IQueryable` giúp nối nhiều điều kiện query trước khi EF Core chạy SQL thật.
- `ToListAsync` là thời điểm query được execute xuống PostgreSQL.

## Test case Day 3

Chạy project:

```powershell
dotnet run
```

Dữ liệu mẫu được thêm trong `Program.cs`:

```text
Keyboard      - 250000
Mouse         - 150000
Monitor       - 2500000
Keycap Set    - 350000
Laptop Stand  - 450000
USB Cable     - 80000
```

### Test case 1: Search theo keyword

Input:

```text
search = key
page = 1
pageSize = 5
sortBy = price
```

Kết quả mong đợi:

```text
Keyboard
Keycap Set
```

Ý nghĩa:

- Tên sản phẩm có chứa `key` sẽ được lấy ra.
- Search hiện tại dùng case-insensitive đơn giản bằng `ToLower()`.
- Kết quả được sort theo giá tăng dần.

### Test case 2: Sort theo tên

Input:

```text
search = null
page = 1
pageSize = 5
sortBy = name
```

Kết quả mong đợi:

```text
Keyboard
Keycap Set
Laptop Stand
Monitor
Mouse
```

Ý nghĩa:

- Không lọc keyword.
- Lấy trang đầu tiên.
- Mỗi trang tối đa 5 sản phẩm.
- Sắp xếp theo tên tăng dần.

### Test case 3: Pagination

Input:

```text
page = 1
pageSize = 5
```

Kết quả mong đợi:

```text
Chỉ hiển thị tối đa 5 sản phẩm trong một lần query.
```

Công thức phân trang:

```text
Skip = (page - 1) * pageSize
Take = pageSize
```

Ví dụ:

```text
page = 2
pageSize = 5
Skip = (2 - 1) * 5 = 5
Take = 5
```

## Checklist Day 3

- [x] Có method `GetPagedProductsAsync` trong repository interface.
- [x] Có implement search bằng `Where`.
- [x] Có implement sort bằng `OrderBy`.
- [x] Có implement pagination bằng `Skip` và `Take`.
- [x] Có dùng `IQueryable` trước khi gọi `ToListAsync`.
- [x] `Program.cs` có test search/pagination/sort.
- [x] `dotnet build` pass.
- [x] `dotnet run` hiển thị đúng kết quả test.

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
- LINQ: cú pháp query dữ liệu trong C#.
- IQueryable: query chưa chạy ngay, có thể nối thêm filter/sort/pagination.
- Where: lọc dữ liệu theo điều kiện.
- OrderBy: sắp xếp dữ liệu tăng dần.
- Skip: bỏ qua một số dòng dữ liệu.
- Take: lấy số dòng dữ liệu cần dùng.
- ToListAsync: execute query và trả kết quả dạng list.

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
