# API Thương Mại Điện Tử

API backend mạnh mẽ được xây dựng bằng .NET 8.0, cung cấp đầy đủ tính năng để quản lý cửa hàng trực tuyến.

## 🛠 Công Nghệ Sử Dụng

-   **Framework**: .NET 8.0
-   **Xác thực**: JWT, OAuth (Google, GitHub)
-   **Cơ sở dữ liệu**:
    -   MySQL (CSDL chính)
    -   MongoDB (CSDL phụ)
-   **Tài liệu API**: Swagger/OpenAPI
-   **ORM**: Entity Framework Core
-   **Mapping**: AutoMapper
-   **Tích hợp thanh toán**:
    -   VNPay
    -   Momo
    -   PayOS
    -   PayPal

## ✨ Tính Năng

### Xác Thực & Phân Quyền

-   Xác thực bằng JWT
-   Tích hợp OAuth (Google, GitHub)
-   Phân quyền theo vai trò (Admin, User)
-   Cơ chế refresh token
-   Hỗ trợ đăng nhập qua mạng xã hội

### Quản Lý Sản Phẩm

-   Thao tác CRUD với sản phẩm
-   Quản lý danh mục
-   Tìm kiếm và lọc sản phẩm
-   Bình luận và đánh giá sản phẩm

### Tính Năng Mua Sắm

-   Giỏ hàng
-   Quản lý đơn hàng
-   Nhiều phương thức thanh toán
-   Quản lý banner quảng cáo

### Quản Lý Người Dùng

-   Quản lý hồ sơ người dùng
-   Quản lý vai trò
-   Thông báo qua email
-   Quản lý mật khẩu

### Tích Hợp Thanh Toán

-   Tích hợp VNPay
-   Hỗ trợ thanh toán Momo
-   Tích hợp PayOS
-   Tích hợp PayPal

### Tin Tức & Nội Dung

-   Quản lý tin tức
-   Dịch vụ crawl tin tức
-   Hệ thống bình luận

## 🗃 Cấu Trúc Cơ Sở Dữ Liệu

### MySQL (CSDL Chính)

-   Tài khoản người dùng
-   Sản phẩm
-   Danh mục
-   Đơn hàng
-   Thanh toán
-   Vai trò
-   Refresh Token
-   Banner
-   Bình luận

### MongoDB

-   Lưu trữ tin tức và nội dung

## 🔐 Tính Năng Bảo Mật

-   Xác thực JWT
-   Mã hóa mật khẩu
-   Cấu hình CORS
-   Kiểm soát truy cập theo vai trò
-   Xử lý thanh toán an toàn

## 💻 Các Repository Liên Quan

-   Repository Frontend: [E-commerce Frontend](https://github.com/dinhnguyen888/e-commerce-website.git)
-   Repository Admin Panel: [E-commerce Admin Panel](https://github.com/dinhnguyen888/e-commerce-admin.git)

## 🚀 Chi Tiết API

### Thao Tác Người Dùng

-   Đăng ký và xác thực
-   Quản lý hồ sơ
-   Khôi phục mật khẩu
-   Đăng nhập qua mạng xã hội

### Thao Tác Sản Phẩm

-   Danh sách và chi tiết sản phẩm
-   Lọc theo danh mục
-   Chức năng tìm kiếm
-   Đánh giá và nhận xét

### Thao Tác Mua Sắm

-   Quản lý giỏ hàng
-   Xử lý đơn hàng
-   Đa dạng phương thức thanh toán
-   Lịch sử đơn hàng

### Thao Tác Admin

-   Quản lý người dùng
-   Quản lý sản phẩm
-   Quản lý đơn hàng
-   Quản lý banner
-   Quản lý vai trò
-   Quản lý tin tức

## 🔧 Cấu Hình

Ứng dụng sử dụng các cấu hình cho:

-   Kết nối cơ sở dữ liệu (MySQL, MongoDB)
-   Cài đặt JWT
-   Thông tin xác thực OAuth
-   Cấu hình cổng thanh toán
-   Cài đặt dịch vụ email

## 🛡 Bảo Mật API

-   Xác thực dựa trên JWT
-   Phân quyền theo vai trò
-   Kiểm tra đầu vào
-   Chính sách CORS
-   Xử lý thanh toán an toàn

## 📥 Hướng Dẫn Cài Đặt

### Yêu Cầu Hệ Thống

-   .NET 8.0 SDK
-   MySQL Server
-   MongoDB Server

### Cấu Trúc File appsettings.json

```json
{
    "Logging": {
        "LogLevel": {
            "Default": "Information",
            "Microsoft.AspNetCore": "Warning"
        }
    },
    "DefaultPassword": {
        "Admin": "your_admin_password",
        "User": "your_user_password"
    },
    "ConnectionStrings": {
        "MySQLConnection": "your_mysql_connection_string",
        "MongoDBConnection": "your_mongodb_connection_string"
    },
    "MongoDBDatabase": "your_mongodb_database_name",
    "Smtp": {
        "Host": "smtp_host",
        "Port": "smtp_port",
        "Username": "smtp_username",
        "Password": "smtp_password",
        "EnableSsl": "true/false",
        "Email": "smtp_email"
    },
    "Github": {
        "ClientId": "your_github_client_id",
        "ClientSecret": "your_github_client_secret"
    },
    "Google": {
        "ClientId": "your_google_client_id",
        "ClientSecret": "your_google_client_secret"
    },
    "Vnpay": {
        "TmnCode": "your_vnpay_tmn_code",
        "HashSecret": "your_vnpay_hash_secret",
        "BaseUrl": "vnpay_base_url",
        "ReturnUrl": "your_return_url"
    },
    "Momo": {
        "Endpoint": "momo_endpoint",
        "PartnerCode": "your_partner_code",
        "AccessKey": "your_access_key",
        "SecretKey": "your_secret_key"
    },
    "PayOS": {
        "CLIENT_ID": "your_client_id",
        "API_KEY": "your_api_key",
        "CHECKSUM_KEY": "your_checksum_key",
        "CANCEL_URL": "your_cancel_url",
        "RETURN_URL": "your_return_url"
    },
    "Paypal": {
        "ClientId": "your_paypal_client_id",
        "SecretKey": "your_paypal_secret_key",
        "BaseUrl": "paypal_api_base_url"
    },
    "Jwt": {
        "Key": "your_jwt_secret_key",
        "Issuer": "your_issuer",
        "Audience": "your_audience",
        "ExpiresInDays": 1
    },
    "URL": {
        "FrontendUrlPaymentCallback": "your_frontend_payment_callback_url",
        "FrontendUrl": "your_frontend_url",
        "FrontendUrlOAuthCallback": "your_frontend_oauth_callback_url"
    },
    "AllowedHosts": "*"
}
```

### Các Bước Cài Đặt

1. Clone repository
2. Tạo file appsettings.json theo cấu trúc trên
3. Cập nhật các giá trị cấu hình theo môi trường
4. Mở terminal và chạy các lệnh:

```bash
dotnet restore
dotnet ef database update
dotnet run
```

API sẽ chạy mặc định tại địa chỉ: https://localhost:7202
