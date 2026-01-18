# Hướng dẫn chạy project

## Yêu cầu hệ thống

1. **Node.js** (v16+): ✅ Đã cài đặt (v21.0.0)
2. **.NET SDK** (v8.0+): ❌ Chưa cài đặt - Cần cài đặt trước

## Cài đặt .NET SDK

### Windows:
1. Truy cập: https://dotnet.microsoft.com/download/dotnet/8.0
2. Tải và cài đặt .NET 8 SDK (Runtime + SDK)
3. Khởi động lại terminal sau khi cài đặt

### Kiểm tra cài đặt:
```bash
dotnet --version
```

## Chạy project

### Bước 1: Chạy Backend (.NET API)

Mở terminal thứ nhất:
```bash
cd AlphaBinanceAPI
dotnet restore
dotnet run
```

Backend sẽ chạy tại: `http://localhost:5000`

### Bước 2: Chạy Frontend (React)

Mở terminal thứ hai:
```bash
cd AlphaBinanceFrontend
npm run dev
```

Frontend sẽ chạy tại: `http://localhost:3000`

## Lưu ý

- Phải chạy **backend trước** rồi mới chạy frontend
- CORS đã được cấu hình để cho phép kết nối giữa frontend và backend
- Nếu backend chưa chạy, frontend sẽ hiển thị lỗi "Không thể tải dữ liệu"
