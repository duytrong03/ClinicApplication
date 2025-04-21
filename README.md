# Clinic Management API
Thực hành làm việc với API quản lý thông tin bệnh nhân, phiếu khám bệnh, các cơ sở khám bệnh. Sử dụng ASP.Net Core WebAPI + Dapper + PostgreSQL.

## Mục lục
-[Kiến trúc dự án](#-kiến-trúc-dự-án)
-[Chức năng](#-chức-năng)
-[Chạy chương trình](#-chạy-chương-trình)

## Kiến trúc dự án
Dự án được chia thành các tầng: Controller, Service, Repository, Model, ViewModel.
Mỗi tầng đảm nhiệm một nhiệm vụ riêng biệt nhằm phân tách rõ ràng, dễ bảo trì.

### 1. **Controllers**
- **Mô tả**: Chịu trách nhiệm nhận các yêu cầu HTTP và gọi các Service để xử lý.
### 2. **Services**
- **Mô tả**: Chứa logic nghiệp vụ của ứng dụng. Nhận yêu cầu từ Controllers và gọi các Repositories để thao tác với cơ sở dữ liệu.
### 3. **Repositories**
- **Mô tả**: Quản lý các truy vấn cơ sở dữ liệu. Chúng thao tác trực tiếp với cơ sở dữ liệu, thực hiện các lệnh SQL.
### 4. **Models**
- **Mô tả**: Đại diện cho các đối tượng dữ liệu trong cơ sở dữ liệu.
### 5. **ViewModels**
- **Mô tả**: Dùng để truyền dữ liệu giữa Controllers và Client. Chứa các thông tin mà API sẽ gửi lại cho Client.
  
### Luồng Hoạt Động
1. **Nhận yêu cầu**: Controller nhận yêu cầu HTTP từ Client.
2. **Xử lý nghiệp vụ**: Service xử lý yêu cầu và gọi Repository để thao tác dữ liệu.
3. **Truy vấn cơ sở dữ liệu**: Repository thực hiện truy vấn cơ sở dữ liệu và trả về kết quả.
4. **Trả kết quả cho Client**: Service trả kết quả về Controller và Controller gửi kết quả về cho Client.
