# Alpha Binance Token Analysis

Ứng dụng web full-stack để hiển thị danh sách các token Alpha mới list trên Binance trong vòng 30 ngày gần đây.

## Công nghệ sử dụng

- **Backend**: .NET 8 Web API
- **Frontend**: React.js với Vite
- **API Source**: Binance Alpha Token List API

## Cấu trúc dự án

```
AlphaBinaceAnalysist/
├── AlphaBinanceAPI/          # Backend .NET
│   ├── Controllers/
│   ├── Models/
│   └── Program.cs
└── AlphaBinanceFrontend/     # Frontend React
    ├── src/
    ├── package.json
    └── vite.config.js
```

## Hướng dẫn chạy ứng dụng

### 1. Backend (.NET API)

```bash
cd AlphaBinanceAPI
dotnet restore
dotnet run
```

API sẽ chạy tại: `http://localhost:5000`

### 2. Frontend (React)

```bash
cd AlphaBinanceFrontend
npm install
npm run dev
```

Frontend sẽ chạy tại: `http://localhost:3000`

## Tính năng

- ✅ Lấy dữ liệu token từ Binance API
- ✅ Lọc các token có thời gian list < 30 ngày
- ✅ Hiển thị tên token (name + symbol)
- ✅ Đếm ngược số ngày còn lại (30 ngày - số ngày đã qua)
- ✅ Sử dụng múi giờ Việt Nam (Hà Nội UTC+7)
- ✅ UI đơn giản và hiện đại
- ✅ Tự động refresh mỗi 5 phút

## API Endpoints

- `GET /api/token/alpha-tokens` - Lấy danh sách token alpha đã được lọc

## Lưu ý

- Đảm bảo backend chạy trước khi mở frontend
- CORS đã được cấu hình để cho phép frontend kết nối từ `http://localhost:3000`
