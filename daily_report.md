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

---

# Daily Report - Week 4 Day 3

## Hôm nay đã làm

- Thêm method `GetPagedProductsAsync` vào `IProductRepository`.
- Implement `GetPagedProductsAsync` trong `ProductRepository` bằng LINQ.
- Thêm method `GetPagedProductsAsync` trong `ProductService` để validate `page` và `pageSize`.
- Cập nhật `Program.cs` để thêm dữ liệu mẫu và test search, pagination, sort.
- Cập nhật `README.md` với nội dung Day 3, sơ đồ luồng hoạt động và test case.

## File/class chính

- `Repositories/IProductRepository.cs`: định nghĩa contract cho method search/pagination/sort.
- `Repositories/ProductRepository.cs`: xử lý query bằng `IQueryable`, `Where`, `OrderBy`, `Skip`, `Take`, `ToListAsync`.
- `Services/ProductService.cs`: validate input cơ bản trước khi gọi repository.
- `Program.cs`: chạy test case Day 3 và in kết quả ra console.
- `README.md`: ghi lại flow, test case và checklist Day 3.

## Kiến thức đã học

- `IQueryable` dùng để build query từng bước trước khi chạy SQL thật.
- `Where` dùng để lọc dữ liệu theo điều kiện.
- `OrderBy` dùng để sắp xếp dữ liệu tăng dần.
- `Skip` dùng để bỏ qua dữ liệu của các trang trước.
- `Take` dùng để lấy số lượng dữ liệu cần hiển thị trong một trang.
- `ToListAsync` là thời điểm EF Core execute query xuống database.
- Search có thể xử lý case-insensitive đơn giản bằng `ToLower()`.

## Test case đã chạy

### Search theo keyword

Input:

```text
search = key
page = 1
pageSize = 5
sortBy = price
```

Kết quả:

```text
Keyboard
Keycap Set
```

### Sort theo tên

Input:

```text
search = null
page = 1
pageSize = 5
sortBy = name
```

Kết quả:

```text
Keyboard
Keycap Set
Laptop Stand
Monitor
Mouse
```

### Pagination

- Dùng `page = 1` và `pageSize = 5`.
- Kết quả chỉ hiển thị tối đa 5 sản phẩm trong một lần query.

## Lệnh quan trọng đã dùng

```powershell
dotnet build
dotnet run
```

## Kết quả cuối ngày

- Project build thành công.
- Chức năng search sản phẩm theo keyword chạy được.
- Chức năng pagination bằng `page` và `pageSize` chạy được.
- Chức năng sort theo `price` và `name` chạy được.
- README đã có sơ đồ flow và test case Day 3.

## Phần cần nắm để vấn đáp

- LINQ là gì và dùng để làm gì trong EF Core.
- `IQueryable` khác `List` như thế nào.
- Vì sao nên build query bằng `IQueryable` trước khi gọi `ToListAsync`.
- `Where`, `OrderBy`, `Skip`, `Take` dùng để làm gì.
- Công thức phân trang: `(page - 1) * pageSize`.
- Flow từ `Program.cs` đến Service, Repository, DbContext và PostgreSQL.

## Việc tiếp theo Day 4

- Học relationship Category-Product.
- Tạo `Category` entity.
- Thêm `CategoryId` cho `Product`.
- Tạo migration thêm quan hệ.
- Query danh sách Product kèm CategoryName bằng `Include`.

---

# Daily Report - Week 4 Day 4

## Hôm nay đã làm

- Tạo entity `Category` với `Id`, `Name` và collection `Products`.
- Thêm `CategoryId` và navigation property `Category` vào `Product`.
- Thêm `DbSet<Category>` và cấu hình quan hệ một-nhiều trong `AppDbContext`.
- Tạo và apply migration `AddCategoryProductRelationship`.
- Thêm method query Product kèm Category bằng `Include`.
- Test in Product cùng CategoryName trong `Program.cs`.
- Tạo `ICategoryRepository`, `CategoryRepository` và `CategoryService`.
- Dùng `AnyAsync` để chặn xóa Category nếu vẫn còn Product.

## File/class chính

- `Models/Category.cs`: entity Category và collection navigation Products.
- `Models/Product.cs`: thêm foreign key `CategoryId` và navigation property `Category`.
- `Data/AppDbContext.cs`: thêm Categories và cấu hình relationship.
- `Migrations/AddCategoryProductRelationship`: tạo bảng, cột và foreign key.
- `Repositories/ProductRepository.cs`: query Product kèm Category bằng `Include`.
- `Repositories/CategoryRepository.cs`: kiểm tra Category còn Product và thực hiện xóa.
- `Services/CategoryService.cs`: xử lý nghiệp vụ chặn xóa Category.
- `Services/CategoryDeleteResult.cs`: định nghĩa kết quả Success, NotFound, HasProducts.
- `Program.cs`: test Include và validate xóa Category.

## Kiến thức đã học

- Quan hệ one-to-many giữa Category và Product.
- Foreign key dùng để liên kết hai bảng.
- Navigation property dùng để truy cập entity liên quan.
- `OnModelCreating` dùng để cấu hình relationship bằng Fluent API.
- `Include` dùng để tải navigation property khi query.
- `AnyAsync` dùng để kiểm tra dữ liệu tồn tại mà không tải toàn bộ danh sách.
- Dùng enum để trả kết quả nghiệp vụ rõ hơn boolean.

## Migration đã chạy

```powershell
dotnet tool run dotnet-ef migrations add AddCategoryProductRelationship
dotnet tool run dotnet-ef database update
```

Kết quả schema:

- Có bảng `Categories`.
- Bảng `Products` có cột `CategoryId`.
- Có foreign key từ `Products.CategoryId` tới `Categories.Id`.
- `CategoryId` cho phép null để Product cũ vẫn hợp lệ.

## Test case đã chạy

### Query Product kèm Category

Kết quả:

```text
Name: Mechanical Keyboard, Category: Accessory, Price: 1200000, Quantity: 3
```

### Chặn xóa Category còn Product

Kết quả:

```text
=== Day 4 Delete Category Validation Test ===
Cannot delete category because it still has products.
```

Category không bị xóa vì vẫn còn Product liên kết.

## Lệnh quan trọng đã dùng

```powershell
dotnet build
dotnet run
dotnet tool run dotnet-ef migrations add AddCategoryProductRelationship
dotnet tool run dotnet-ef database update
```

## Kết quả cuối ngày

- Relationship Category-Product chạy đúng với PostgreSQL.
- Migration đã apply thành công.
- Query `Include` lấy được CategoryName.
- Product cũ không có Category vẫn được xử lý bằng `No category`.
- Chặn xóa Category nếu còn Product hoạt động đúng.
- Project build và run thành công.

## Phần cần nắm để vấn đáp

- Quan hệ one-to-many là gì.
- `CategoryId` và navigation property khác nhau như thế nào.
- Vì sao `CategoryId` được khai báo nullable.
- `Include` dùng để làm gì.
- Nếu không dùng `Include`, navigation property có dữ liệu hay không trong flow hiện tại.
- Vì sao dùng `AnyAsync` thay vì tải danh sách Product.
- Flow chặn xóa Category từ Program, Service, Repository tới database.

## Việc tiếp theo Day 5

- Hoàn thiện Product + Category Console App.
- Bổ sung CRUD Category và Product.
- Review search, pagination, relationship và Include.
- Dọn code, naming và tài liệu.
- Demo luồng migration và chạy app.

---

# Daily Report - Week 4 Day 5

## Hôm nay đã làm

- Hoàn thiện CRUD Category trong Repository và Service.
- Bổ sung Category CRUD test trước khi chuyển sang giao diện tương tác.
- Review và sửa lỗi EF Core tracking khi cập nhật Product và Category.
- Ngăn dữ liệu demo bị thêm trùng khi chạy chương trình nhiều lần.
- Refactor `Program.cs` thành composition root ngắn gọn.
- Tách menu và xử lý input sang `ConsoleApp.cs`.
- Hoàn thiện menu Category Management: list, add, update, delete.
- Hoàn thiện menu Product Management: list, add, update, delete.
- Tích hợp search, pagination và sorting vào Console App.
- Thêm validation cho tên, ID, giá, số lượng, page, page size và sort field.
- Giữ business rule không cho xóa Category nếu còn Product.
- Cập nhật README đầy đủ nội dung Week 4 và demo flow.

## File/class chính

- `Program.cs`: khởi tạo DbContext, Repository, Service và Console App.
- `ConsoleApp.cs`: menu tương tác, validation và hiển thị kết quả.
- `Repositories/ICategoryRepository.cs`: contract CRUD Category.
- `Repositories/CategoryRepository.cs`: CRUD Category và kiểm tra Product tồn tại.
- `Services/CategoryService.cs`: CRUD và nghiệp vụ xóa Category.
- `Services/ProductService.cs`: CRUD Product và cập nhật entity đang được tracking.
- `README.md`: tài liệu tổng hợp Week 4, test case và hướng dẫn demo.

## Kiến trúc sau refactor

```text
Người dùng
    |
    v
ConsoleApp
    |
    v
ProductService / CategoryService
    |
    v
ProductRepository / CategoryRepository
    |
    v
AppDbContext / EF Core
    |
    v
PostgreSQL
```

- `ConsoleApp` nhận input và hiển thị output.
- Service xử lý nghiệp vụ.
- Repository truy vấn và lưu dữ liệu.
- `AppDbContext` kết nối EF Core với PostgreSQL.

## Chức năng đã hoàn thiện

### Category

- List Category.
- Add Category.
- Update Category.
- Delete Category.
- Không cho tên rỗng hoặc tên trùng.
- Không cho xóa Category còn Product.

### Product

- List Product kèm CategoryName.
- Add Product và chọn Category tùy chọn.
- Update Product và Category.
- Delete Product.
- Kiểm tra giá và số lượng không âm.
- Kiểm tra Category tồn tại trước khi gán.

### Search

- Search Product theo tên.
- Pagination bằng page và page size.
- Sort theo ID, tên hoặc giá.
- Validation input và hiển thị kết quả rỗng.

## Vấn đề đã review và sửa

### 1. EF Core tracking khi update

Vấn đề:

- Service tìm entity hiện tại nhưng truyền một object khác cùng ID vào `Update`.
- EF Core có thể tracking hai instance cùng khóa.

Cách xử lý:

- Cập nhật thuộc tính trên entity đã được tìm thấy.
- Truyền chính entity đang được tracking vào Repository.
- Không thay đổi `CreatedAt` khi update Product.

### 2. Dữ liệu mẫu bị trùng

Vấn đề:

- `Program.cs` cũ tự thêm Product và Category mỗi lần chạy.

Cách xử lý:

- Ngăn dữ liệu seed bị thêm trùng trong bước review.
- Sau đó loại toàn bộ test/seed tự động khỏi startup.
- App mới chỉ thay đổi dữ liệu khi người dùng chọn thao tác CRUD.

### 3. Program.cs quá dài

Vấn đề:

- Test Day 2 đến Day 5 nằm trong cùng một file.

Cách xử lý:

- `Program.cs` chỉ còn nhiệm vụ ghép dependency.
- Menu và input được chuyển sang `ConsoleApp.cs`.

## Test case đã chạy

- Add Category hợp lệ và nhận ID.
- Update Category thành công.
- Delete Category không có Product thành công.
- Delete Category còn Product bị chặn.
- Add Product với tên, giá, số lượng và Category hợp lệ.
- List Product hiển thị đúng CategoryName.
- Search Product theo keyword.
- Pagination và sorting chạy đúng.
- Input không hợp lệ được từ chối bằng message rõ ràng.

Ví dụ thêm Product:

```text
Product name: computer
Price: 25000
Quantity: 12
Category id: 1
Added product successfully. Id: 44
```

Luồng lưu:

```text
ConsoleApp.AddProductAsync
    -> ProductService.AddProductAsync
    -> ProductRepository.AddAsync
    -> AppDbContext.SaveChangesAsync
    -> PostgreSQL
```

## Migration và lệnh demo

```powershell
dotnet tool restore
dotnet tool run dotnet-ef migrations list
dotnet tool run dotnet-ef database update
dotnet build
dotnet run
```

Migration trong source:

```text
20260710032424_InitialCreate
20260710081440_AddCategoryProductRelationship
```

## Kết quả cuối ngày

- Product + Category Console App đã hoàn thiện.
- CRUD Category và Product hoạt động qua menu tương tác.
- Search, pagination và sorting đã được tích hợp.
- Relationship và Include hoạt động đúng.
- Validation và business rule hoạt động đúng.
- Code được tách theo ConsoleApp, Service và Repository.
- README đã được cập nhật đầy đủ roadmap Week 4.
- Project build thành công với 0 warning và 0 error.

## Phần cần nắm để vấn đáp

- Vai trò của `Program.cs`, `ConsoleApp`, Service và Repository.
- Flow thêm Product từ input tới PostgreSQL.
- Vì sao `CategoryId` nullable.
- Vì sao dùng `Include` khi list Product kèm Category.
- Vì sao dùng `AnyAsync` để chặn xóa Category.
- `IQueryable` và deferred execution hoạt động thế nào.
- Công thức pagination bằng `Skip` và `Take`.
- Vì sao phải cập nhật entity đang được EF Core tracking.
- Migration dùng để quản lý thay đổi schema như thế nào.
