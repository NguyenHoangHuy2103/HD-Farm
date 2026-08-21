# POS App — Blazor WebAssembly

Đã scaffold bằng:

```bash
cd pos-app
dotnet new blazorwasm -n HdFarm.Pos --pwa
```

Cờ `--pwa` bật sẵn khả năng cài đặt như app + cache offline — dùng để đáp
ứng yêu cầu "hoạt động ổn định cả khi mạng chập chờn" (mục 6 tài liệu yêu cầu).
Kiểm tra Service Worker: mở DevTools → tab **Application** → **Service Workers**,
phải thấy service worker của app đã đăng ký (active).

## Cấu hình base URL backend API

`wwwroot/appsettings.json` khai báo `Api:BaseUrl` — đọc trong `Program.cs`
qua `builder.Configuration`, không hardcode URL vào code. Vì đây là app
Blazor WebAssembly (chạy hoàn toàn phía client), file này không chứa gì bí
mật — mọi giá trị trong `wwwroot/` đều công khai với trình duyệt, khác với
cách backend giấu connection string/API key thật qua user-secrets +
`appsettings.Development.json` (bị `.gitignore`).

## Chạy thử

```bash
cd pos-app/HdFarm.Pos
dotnet run
```

## Ghi chú
- Share DTO với `backend/src/HdFarm.Application` bằng cách tách 1 project
  `HdFarm.Shared` dùng chung giữa backend và pos-app (tránh định nghĩa lại
  model 2 lần).
- Ưu tiên màn hình theo đúng luồng đã note trong `../docs/design/`.
- Việc cần làm đầu tiên: xem `../tasks/giai-doan-1-mvp.md`.
