# POS App — Blazor WebAssembly

Chưa scaffold (cần .NET SDK trên máy bạn).

## Khởi tạo lần đầu

```bash
cd pos-app
dotnet new blazorwasm -n HdFarm.Pos --pwa
```

Cờ `--pwa` bật sẵn khả năng cài đặt như app + cache offline — dùng để đáp
ứng yêu cầu "hoạt động ổn định cả khi mạng chập chờn" (mục 6 tài liệu yêu cầu).

## Ghi chú
- Share DTO với `backend/src/HdFarm.Application` bằng cách tách 1 project
  `HdFarm.Shared` dùng chung giữa backend và pos-app (tránh định nghĩa lại
  model 2 lần).
- Ưu tiên màn hình theo đúng luồng đã note trong `../docs/design/`.
- Việc cần làm đầu tiên: xem `../tasks/giai-doan-1-mvp.md`.
