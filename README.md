# ProductEfConsole

Console App quản lý Product và Category, được xây dựng để thực hành đầy đủ roadmap Week 4: Entity Framework Core, PostgreSQL, Repository Pattern, Service Layer, LINQ, quan hệ dữ liệu, migration và quy trình Git/CI.

## 1. Mục tiêu học tập

Project tập trung vào các nội dung:

- Kết nối .NET Console App với PostgreSQL bằng Entity Framework Core.
- Sử dụng `DbContext`, `DbSet`, entity và migration.
- Thực hiện CRUD Product và Category.
- Tách truy cập dữ liệu bằng Repository Pattern.
- Tách nghiệp vụ bằng Service Layer.
- Dùng LINQ và `IQueryable` để search, sort và pagination.
- Thiết kế quan hệ one-to-many giữa Category và Product.
- Dùng `Include` để tải Product kèm Category.
- Chặn xóa Category nếu Category vẫn còn Product.
- Xây dựng Console App tương tác có menu và validation.
- Lưu connection string an toàn bằng User Secrets.
- Kiểm tra build tự động bằng GitHub Actions CI.

## 2. Công nghệ sử dụng

- C#
- .NET 10 Console App
- Entity Framework Core 10
- PostgreSQL
- Npgsql Entity Framework Core Provider
- pgAdmin 4
- LINQ và `IQueryable`
- User Secrets
- Local .NET Tool `dotnet-ef`
- Git, GitHub và GitHub Actions

## 3. Chức năng chính

### Category Management

- Xem danh sách Category.
- Thêm Category.
- Cập nhật Category.
- Xóa Category.
- Kiểm tra tên bắt buộc.
- Không cho thêm hoặc đổi thành tên Category đã tồn tại.
- Kiểm tra ID phải là số nguyên dương.
- Không cho xóa Category nếu vẫn còn Product.

### Product Management

- Xem danh sách Product kèm Category.
- Thêm Product.
- Cập nhật Product.
- Xóa Product.
- Product có thể không thuộc Category.
- Kiểm tra tên bắt buộc.
- Kiểm tra giá và số lượng không âm.
- Kiểm tra Category tồn tại trước khi gán cho Product.
- Cho phép giữ nguyên, thay đổi hoặc bỏ Category khi cập nhật Product.

### Product Search

- Tìm Product theo tên.
- Không phân biệt chữ hoa/thường ở luồng tìm kiếm hiện tại.
- Phân trang bằng `Skip` và `Take`.
- Sắp xếp theo:
  - `id`
  - `name`
  - `price`
- Kiểm tra page và page size phải là số nguyên dương.

## 4. Cấu trúc project

```text
ProductEfConsole/
├── .github/
│   └── workflows/
│       └── dotnet-ci.yml
├── Data/
│   └── AppDbContext.cs
├── Migrations/
│   ├── 20260710032424_InitialCreate.cs
│   ├── 20260710081440_AddCategoryProductRelationship.cs
│   └── AppDbContextModelSnapshot.cs
├── Models/
│   ├── Category.cs
│   └── Product.cs
├── Repositories/
│   ├── ICategoryRepository.cs
│   ├── CategoryRepository.cs
│   ├── IProductRepository.cs
│   └── ProductRepository.cs
├── Services/
│   ├── CategoryDeleteResult.cs
│   ├── CategoryService.cs
│   └── ProductService.cs
├── ConsoleApp.cs
├── Program.cs
├── appsettings.json
├── dotnet-tools.json
├── ProductEfConsole.csproj
├── README.md
└── daily_report.md
```

## 5. Kiến trúc ứng dụng

```text
Người dùng
    |
    v
ConsoleApp
    |  nhận input, validation và hiển thị output
    v
Service Layer
    |  xử lý nghiệp vụ
    v
Repository Interface / Repository
    |  xây dựng query và thao tác dữ liệu
    v
AppDbContext / Entity Framework Core
    |  chuyển thao tác entity và LINQ thành SQL
    v
PostgreSQL
```

### Trách nhiệm từng tầng

- `Program.cs`: composition root, khởi tạo DbContext, Repository, Service và Console App.
- `ConsoleApp.cs`: menu, nhập dữ liệu, validation và hiển thị kết quả.
- `ProductService`: nghiệp vụ Product và validation page/page size.
- `CategoryService`: nghiệp vụ Category, đặc biệt là chặn xóa Category còn Product.
- Repository interface: định nghĩa contract truy cập dữ liệu.
- Repository implementation: triển khai truy vấn bằng EF Core.
- `AppDbContext`: cấu hình kết nối và quan hệ entity.
- PostgreSQL: lưu dữ liệu thật.

## 6. Mô hình dữ liệu

### Product

```text
Product
├── Id: int
├── Name: string
├── Price: decimal
├── Quantity: int
├── CreatedAt: DateTime
├── CategoryId: int?
└── Category: Category?
```

### Category

```text
Category
├── Id: int
├── Name: string
└── Products: List<Product>
```

## 7. Quan hệ Category - Product

```text
Categories                         Products
+----------------+                +----------------+
| Id (PK)        |<---------------| CategoryId (FK)|
| Name           |      1 - N     | Id (PK)        |
+----------------+                | Name           |
                                  | Price          |
                                  | Quantity       |
                                  | CreatedAt      |
                                  +----------------+
```

- Một Category có nhiều Product.
- Một Product thuộc tối đa một Category.
- `Products.CategoryId` là foreign key trỏ tới `Categories.Id`.
- `CategoryId` cho phép null để Product có thể không thuộc Category.
- Foreign key nullable cũng giúp dữ liệu Product cũ hợp lệ khi thêm quan hệ bằng migration.

Quan hệ được cấu hình bằng Fluent API:

```csharp
modelBuilder.Entity<Category>()
    .HasMany(category => category.Products)
    .WithOne(product => product.Category)
    .HasForeignKey(product => product.CategoryId);
```

## 8. Repository Pattern và Service Layer

### Product Repository

Các thao tác chính:

- `GetAllAsync`
- `GetByIdAsync`
- `AddAsync`
- `UpdateAsync`
- `DeleteAsync`
- `GetPagedProductsAsync`
- `GetProductsWithCategoryAsync`

### Category Repository

Các thao tác chính:

- `GetAllAsync`
- `GetByIdAsync`
- `AddAsync`
- `UpdateAsync`
- `DeleteAsync`
- `HasProductsAsync`

### Lợi ích

- Console UI không truy cập DbContext trực tiếp.
- Logic database được tập trung trong Repository.
- Business rule được tập trung trong Service.
- Dễ đọc, bảo trì và thay đổi từng tầng.
- Tránh trộn giao diện, nghiệp vụ và truy cập database trong một file.

## 9. LINQ search, sorting và pagination

Luồng query:

```text
ConsoleApp.SearchProductsAsync
        |
        v
ProductService.GetPagedProductsAsync
        |  validate page và pageSize
        v
ProductRepository.GetPagedProductsAsync
        |
        +--> IQueryable<Product>
        +--> Where nếu có search
        +--> OrderBy theo id/name/price
        +--> Skip((page - 1) * pageSize)
        +--> Take(pageSize)
        +--> ToListAsync
        v
PostgreSQL
```

`IQueryable` cho phép ghép các điều kiện trước khi EF Core thực thi SQL.

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

## 10. Eager loading bằng Include

Khi lấy danh sách Product kèm Category:

```csharp
_context.Products
    .Include(product => product.Category)
    .ToListAsync();
```

Luồng:

```text
ConsoleApp
    |
    v
ProductService.GetProductsWithCategoryAsync
    |
    v
ProductRepository.GetProductsWithCategoryAsync
    |
    | Include(product => product.Category)
    v
Products + Categories
```

Nếu Product không có Category, ứng dụng hiển thị `No category`.

## 11. Business rule khi xóa Category

Ứng dụng không cho xóa Category đang được Product sử dụng.

```text
Delete Category
    |
    v
Tìm Category theo Id
    |
    +--> Không tồn tại --> NotFound
    |
    v
AnyAsync(Product.CategoryId == categoryId)
    |
    +--> Có Product --> HasProducts, không xóa
    |
    +--> Không có Product --> DeleteAsync --> Success
```

`AnyAsync` chỉ kiểm tra sự tồn tại của Product, không tải toàn bộ danh sách nên phù hợp hơn `ToListAsync` hoặc `CountAsync` trong trường hợp này.

## 12. Menu Console App

```text
=== Product EF Console ===
1. Manage Categories
2. Manage Products
3. Search Products
0. Exit
```

### Category Menu

```text
=== Category Management ===
1. List Categories
2. Add Category
3. Update Category
4. Delete Category
0. Back to Main Menu
```

### Product Menu

```text
=== Product Management ===
1. List Products
2. Add Product
3. Update Product
4. Delete Product
0. Back to Main Menu
```

## 13. Database và connection string

Database mặc định:

```text
product_ef_console
```

`appsettings.json` chỉ nên chứa connection string mẫu, không chứa password thật:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=product_ef_console;Username=postgres;Password=your_password"
  }
}
```

Password thật được lưu bằng User Secrets và không commit lên GitHub.

## 14. Cấu hình User Secrets

Khởi tạo User Secrets:

```powershell
dotnet user-secrets init
```

Lưu connection string thật:

```powershell
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=product_ef_console;Username=postgres;Password=YOUR_REAL_PASSWORD"
```

Kiểm tra cấu hình:

```powershell
dotnet user-secrets list
```

## 15. Cài đặt và chạy project

### Yêu cầu

- .NET 10 SDK
- PostgreSQL
- pgAdmin 4 hoặc công cụ quản lý PostgreSQL tương đương

### Clone và restore

```powershell
git clone https://github.com/TINVO04/product-ef-console.git
cd product-ef-console
dotnet restore
dotnet tool restore
```

### Cấu hình connection string

Thiết lập User Secrets theo hướng dẫn ở trên.

### Apply migration

```powershell
dotnet tool run dotnet-ef database update
```

### Build và chạy

```powershell
dotnet build
dotnet run
```

## 16. Migration

Danh sách migration hiện tại:

```text
20260710032424_InitialCreate
20260710081440_AddCategoryProductRelationship
```

Kiểm tra migration:

```powershell
dotnet tool run dotnet-ef migrations list
```

Apply migration mới nhất:

```powershell
dotnet tool run dotnet-ef database update
```

Tạo migration mới khi model thay đổi:

```powershell
dotnet tool run dotnet-ef migrations add MigrationName
```

Sau khi update thành công, PostgreSQL có các bảng chính:

```text
Products
Categories
__EFMigrationsHistory
```

## 17. Test case chính

### Category CRUD

| Test | Input | Kết quả mong đợi |
|---|---|---|
| Add tên rỗng | `""` | `Category name is required.` |
| Add hợp lệ | Tên mới | Thêm thành công và nhận ID |
| Add trùng tên | Tên đã tồn tại | `Category name already exists.` |
| Update ID sai | ID không phải số dương | Báo ID không hợp lệ |
| Update hợp lệ | ID tồn tại, tên mới | Cập nhật thành công |
| Delete không tồn tại | ID không tồn tại | `Category not found.` |
| Delete còn Product | Category đang có Product | Không cho xóa |
| Delete không còn Product | Category không có Product | Xóa thành công |

### Product CRUD

| Test | Input | Kết quả mong đợi |
|---|---|---|
| Add tên rỗng | `""` | `Product name is required.` |
| Giá âm | `-1` | Báo giá không hợp lệ |
| Số lượng âm | `-1` | Báo số lượng không hợp lệ |
| Category không tồn tại | ID không có trong database | `Category not found.` |
| Add hợp lệ | Product hợp lệ | Thêm thành công và nhận ID |
| List Product | Có Product trong database | Hiển thị Product và Category |
| Update hợp lệ | ID tồn tại | Cập nhật thành công |
| Delete không tồn tại | ID không tồn tại | Báo không tìm thấy |
| Delete hợp lệ | ID tồn tại | Xóa thành công |

### Search và pagination

| Test | Input | Kết quả mong đợi |
|---|---|---|
| Search theo tên | `key` | Trả Product có tên chứa `key` |
| Search rỗng | để trống | Trả danh sách theo trang |
| Sort name | `name` | Sắp xếp theo tên tăng dần |
| Sort price | `price` | Sắp xếp theo giá tăng dần |
| Sort id | `id` | Sắp xếp theo ID tăng dần |
| Page bằng 0 | `0` | Báo page phải là số dương |
| Page size bằng 0 | `0` | Báo page size phải là số dương |
| Không có kết quả | Keyword không tồn tại | `No products found.` |

## 18. Demo flow hoàn chỉnh

1. Chạy `dotnet tool restore`.
2. Chạy `dotnet tool run dotnet-ef migrations list`.
3. Chạy `dotnet tool run dotnet-ef database update`.
4. Chạy `dotnet build`.
5. Chạy `dotnet run`.
6. Thêm một Category demo.
7. Thêm một Product và gắn Category vừa tạo.
8. List Product để kiểm tra tên Category.
9. Search Product theo tên.
10. Thử xóa Category khi Product còn tồn tại và xác nhận bị chặn.
11. Xóa Product.
12. Xóa lại Category và xác nhận thành công.
13. Chọn `0` để thoát ứng dụng.

## 19. Nội dung hoàn thành theo roadmap

### Day 1 - EF Core và PostgreSQL

- [x] Tạo .NET Console App.
- [x] Cài EF Core và Npgsql provider.
- [x] Tạo Product entity.
- [x] Tạo AppDbContext và DbSet.
- [x] Cấu hình PostgreSQL.
- [x] Dùng User Secrets.
- [x] Tạo và apply InitialCreate migration.
- [x] Cấu hình local `dotnet-ef`.
- [x] Cấu hình GitHub Actions CI.

### Day 2 - Repository, Service và CRUD

- [x] Tạo IProductRepository.
- [x] Tạo ProductRepository.
- [x] Tạo ProductService.
- [x] Hoàn thành Product CRUD.
- [x] Dùng async/await với EF Core.
- [x] Xử lý trường hợp update/delete không tìm thấy Product.

### Day 3 - LINQ

- [x] Search Product bằng `Where`.
- [x] Sorting bằng `OrderBy`.
- [x] Pagination bằng `Skip` và `Take`.
- [x] Dùng `IQueryable` để build query.
- [x] Execute query bằng `ToListAsync`.
- [x] Validation page và page size.

### Day 4 - Relationship

- [x] Tạo Category entity.
- [x] Thiết kế one-to-many Category - Product.
- [x] Thêm nullable foreign key `CategoryId`.
- [x] Cấu hình Fluent API.
- [x] Tạo migration quan hệ.
- [x] Query Product kèm Category bằng `Include`.
- [x] Tạo Category Repository và Service.
- [x] Chặn xóa Category còn Product bằng `AnyAsync`.

### Day 5 - Hoàn thiện mini project

- [x] Hoàn thiện Product + Category Console App.
- [x] CRUD Category và Product qua menu tương tác.
- [x] Tích hợp Product search, pagination và sorting.
- [x] Review code và sửa naming/format.
- [x] Sửa EF Core tracking khi update entity.
- [x] Thêm validation input.
- [x] Loại dữ liệu mẫu tự động bị chèn trùng.
- [x] Tách Program và ConsoleApp.
- [x] Chuẩn bị luồng demo migration và chạy app.

## 20. GitHub Actions CI

Workflow CI chạy khi push hoặc tạo Pull Request.

Các bước chính:

```text
Checkout source
    |
    v
Setup .NET SDK
    |
    v
Restore dependencies
    |
    v
Build project
```

Mục tiêu là phát hiện sớm lỗi build trước khi merge code vào `main`.

## 21. Thuật ngữ đã học

- **Entity**: class đại diện cho bảng trong database.
- **DbContext**: class trung tâm để EF Core làm việc với database.
- **DbSet**: đại diện cho tập entity hoặc bảng.
- **Migration**: lịch sử thay đổi schema database.
- **Repository Pattern**: tách logic truy cập database khỏi UI và nghiệp vụ.
- **Service Layer**: chứa logic nghiệp vụ và điều phối Repository.
- **LINQ**: cú pháp truy vấn dữ liệu trong C#.
- **IQueryable**: query chưa thực thi, có thể ghép thêm điều kiện.
- **Deferred execution**: query chỉ chạy khi gọi method như `ToListAsync`.
- **One-to-many**: một entity cha có nhiều entity con.
- **Primary key**: khóa định danh duy nhất của bản ghi.
- **Foreign key**: cột liên kết tới primary key của bảng khác.
- **Navigation property**: property truy cập entity có quan hệ.
- **Include**: eager loading navigation property.
- **AnyAsync**: kiểm tra có bản ghi thỏa điều kiện.
- **User Secrets**: lưu secret ở máy local, không commit vào Git.
- **CI**: tự động restore/build/check source code.

## 22. Trạng thái project

- Build: pass.
- Warning: 0.
- Error: 0.
- Category CRUD: hoàn thành.
- Product CRUD: hoàn thành.
- Relationship và Include: hoàn thành.
- Search/pagination/sorting: hoàn thành.
- Interactive Console App: hoàn thành.
- Migration source: hoàn thành.
- GitHub Actions CI: đã cấu hình.
