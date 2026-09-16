# CodeBucket: website thương mại điện tử

## Tổng Quan

Ứng dụng web thương mại điện tử hiện đại được xây dựng bằng React và Vite, với giao diện người dùng đẹp và tương thích. Dự án này đã được chuyển đổi từ NextJS sang ReactJS để tối ưu hiệu suất khi triển khai trên Vercel và Digital Ocean.

## Tính Năng

- **Xác Thực**
    - Đăng nhập và đăng ký người dùng
    - Tích hợp đăng nhập Google One-tap
    - Đăng nhập bằng Google, Github
    - Xác thực dựa trên JWT
- **Quản Lý Sản Phẩm**
    - Danh sách và tìm kiếm sản phẩm
    - Chi tiết sản phẩm kèm bình luận/đánh giá
    - Danh mục: Phần mềm, Công cụ, Ứng dụng Web
- **Trải Nghiệm Mua Sắm**
    - Chức năng giỏ hàng
    - Quy trình thanh toán
    - Đa dạng phương thức thanh toán (VNPay, Momo, PayOS, PayPal)
- **Quản Lý Nội Dung**
    - Phần tin tức và bài viết (đang trong quá trình phát triễn thêm)
    - Khu vực tài nguyên miễn phí
    - Trang giới thiệu sản phẩm
- **Trải Nghiệm Người Dùng**
    - Thiết kế tương thích đa thiết bị
    - Chức năng tìm kiếm

## Công Nghệ Sử Dụng

- **Frontend Framework**: React 19
- **Công Cụ Build**: Vite 6
- **Thư Viện UI**:
    - Ant Design (antd)
    - TailwindCSS
    - React Icons
- **Quản Lý State**: ContextAPI
- **Định Tuyến**: React Router DOM v7
- **HTTP Client**: Axios
- **Xác Thực**: JWT + Google One-tap Login
- **Công Cụ Phát Triển**:
    - ESLint
    - Prettier
    - Hỗ trợ TypeScript

## Các Repository Liên Quan

- Backend API: [https://github.com/dinhnguyen888/ECommerceApi.git](https://github.com/dinhnguyen888/ECommerceApi.git)
- Admin Panel: [https://github.com/dinhnguyen888/e-commerce-admin.git](https://github.com/dinhnguyen888/e-commerce-admin.git)

## Hướng Dẫn Cài Đặt

### Yêu Cầu Hệ Thống

- Node.js (khuyến nghị phiên bản LTS)
- npm hoặc yarn

### Cài Đặt

1. Clone repository

```bash
git clone https://github.com/dinhnguyen888/e-commerce-website.git
```

2. Cài đặt các dependencies

```bash
npm install
# hoặc
yarn install
```

3. Thiết lập biến môi trường
   Tạo file `.env` trong thư mục gốc với các biến sau:

```env
VITE_API_URL=https://bettercalldinh.ddns.net/api/
VITE_GOOGLE_API=[your-google-api-key]
```

4. Khởi động server phát triển

```bash
npm run dev
# hoặc
yarn dev
```

Ứng dụng sẽ chạy tại `http://localhost:5173`

### Build cho Production

```bash
npm run build
# hoặc
yarn build
```
