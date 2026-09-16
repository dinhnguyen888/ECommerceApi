# Admin Panel Codebucket

## Tổng Quan

Đây là trang quản trị (admin dashboard) cho trang thương mại CodeBucket, được xây dựng bằng React và TypeScript. Hệ thống cung cấp giao diện toàn diện để quản lý các khía cạnh khác nhau của nền tảng thương mại điện tử bao gồm sản phẩm, thanh toán, tài khoản người dùng, phân quyền, tin tức và bình luận.

## Tính Năng

### Xác Thực & Phân Quyền

-   Hệ thống đăng nhập bảo mật với JWT
-   Kiểm soát truy cập dựa trên vai trò
-   Bảo vệ route cho người dùng đã xác thực
-   Tự động xử lý làm mới token

### Quản Lý Sản Phẩm

-   Xem danh sách sản phẩm
-   Thêm/Sửa/Xóa sản phẩm
-   Quản lý chi tiết sản phẩm
-   Quản lý bình luận cho sản phẩm

### Quản Lý Người Dùng

-   Quản lý tài khoản
-   Hệ thống phân quyền theo vai trò
-   Gán vai trò cho người dùng
-   Xem và quản lý giỏ hàng của người dùng

### Hệ Thống Thanh Toán

-   Xem tất cả các giao dịch
-   Quản lý trạng thái thanh toán
-   Xử lý thanh toán đang chờ
-   Theo dõi lịch sử thanh toán

### Quản Lý Nội Dung

-   Quản lý bài viết tin tức
-   Kiểm duyệt bình luận
-   Soạn thảo văn bản với rich text editor

### Bảng Điều Khiển & Phân Tích

-   Bảng điều khiển bán hàng
-   Hiển thị dữ liệu bằng biểu đồ
-   Theo dõi phân tích
-   Các chỉ số hiệu suất

## Công Nghệ Sử Dụng

### Frontend

-   React 18
-   TypeScript
-   Vite (Công cụ build)
-   React Router v6 (Định tuyến)

### Giao Diện & Style

-   Ant Design (Thư viện UI)
-   Ant Design Icons
-   React Quill (Trình soạn thảo văn bản)

### Quản Lý State & API

-   Context API để quản lý state
-   Axios để gọi API
-   JWT cho xác thực

### Hiển Thị Dữ Liệu

-   Recharts
-   Ant Design Plots

### Công Cụ Phát Triển

-   ESLint để kiểm tra code
-   TypeScript để kiểm tra kiểu dữ liệu
-   SWC cho biên dịch nhanh

## Bắt Đầu

1. Clone repository
2. Cài đặt các dependencies:

```bash
npm install
```

3. Tạo file `.env` với các biến môi trường:

```env
VITE_API_URL=url_api_của_bạn
```

4. Chạy server phát triển:

```bash
npm run dev
```

5. Build cho production:

```bash
npm run build
```

## Các Scripts Có Sẵn

-   `npm run dev` - Khởi chạy server phát triển
-   `npm run build` - Build cho môi trường production
-   `npm run lint` - Chạy ESLint
-   `npm run preview` - Xem trước bản build production

## Cấu Trúc Dự Án

```
src/
├── components/          # Components có thể tái sử dụng
│   ├── common/         # Components dùng chung
│   ├── layout/         # Components layout
│   ├── modal/          # Components modal
│   └── section/        # Components section
├── config/             # File cấu hình
├── context/            # React Context providers
├── pages/              # Components trang
├── services/           # Các service gọi API
├── types/              # Các type TypeScript
└── App.tsx            # Component ứng dụng chính
```

## Tính Năng Bảo Mật

-   Route được bảo vệ với kiểm tra xác thực
-   Quản lý token JWT
-   Tự động đăng xuất khi token hết hạn
-   Giao tiếp API bảo mật với headers xác thực

## Hỗ Trợ Trình Duyệt

-   Các trình duyệt hiện đại (Chrome, Firefox, Safari, Edge)
