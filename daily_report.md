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

---

# Daily Report - Week 4 Day 2

## Hôm nay đã làm

- Tạo `IProductRepository` để định nghĩa các thao tác CRUD cho `Product`.
- Tạo `ProductRepository` để thao tác database bằng EF Core.
- Viết đủ các method: `AddAsync`, `GetAllAsync`, `GetByIdAsync`, `UpdateAsync`, `DeleteAsync`.
- Tạo `ProductService` để service gọi repository, không gọi `DbContext` trực tiếp trong `Program.cs`.
- Cập nhật `Program.cs` để test CRUD flow với PostgreSQL database thật.
- Xử lý trường hợp update/delete không tìm thấy product bằng kết quả `bool` và message rõ ràng.

## File/class chính

- `Repositories/IProductRepository.cs`: interface định nghĩa CRUD contract.
- `Repositories/ProductRepository.cs`: implement CRUD bằng EF Core.
- `Services/ProductService.cs`: service gọi repository và xử lý logic đơn giản.
- `Program.cs`: chạy thử flow Add, GetAll, GetById, Update, Delete.

## Kiến thức đã học

- `AddAsync`: thêm entity vào DbSet.
- `FindAsync`: tìm entity theo primary key.
- `ToListAsync`: lấy danh sách dữ liệu từ database.
- `SaveChangesAsync`: lưu thay đổi thật xuống database.
- `Remove`: xóa entity khỏi DbSet.
- Repository Pattern: tách logic truy cập database ra khỏi code chạy chương trình.
- Service Layer: tách logic nghiệp vụ, giúp `Program.cs` gọn hơn.
- Null handling: xử lý trường hợp không tìm thấy product.

## Lệnh quan trọng đã dùng

```powershell
dotnet build
dotnet run
```

## Kết quả cuối ngày

- CRUD Product chạy được với PostgreSQL database.
- Có Repository và Service theo đúng roadmap.
- `Program.cs` gọi Service để test CRUD flow.
- Project build thành công.
- Code đã được commit theo từng bước nhỏ.

## Phần cần nắm để vấn đáp

- Repository Pattern là gì.
- Vì sao cần `IProductRepository`.
- `ProductRepository` khác `ProductService` như thế nào.
- Vì sao cần gọi `SaveChangesAsync`.
- `AddAsync`, `FindAsync`, `ToListAsync`, `Remove` dùng để làm gì.
- Vì sao update/delete nên xử lý trường hợp không tìm thấy dữ liệu.

## Việc tiếp theo Day 3

- Học LINQ với EF Core.
- Viết search/filter sản phẩm.
- Viết pagination.
- Viết sort theo giá hoặc tên.
