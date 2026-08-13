# HD Farm — Web App Quản lý & Bán Vật tư Nông nghiệp

Bán lẻ cho nông dân · Quản lý kho · Bán tại cửa hàng & bán hàng online.

> Trang giới thiệu tạm thời (`index.html` + `CNAME`) đang chạy qua GitHub
> Pages tại hdfarm.cloud — không xóa/di chuyển 2 file này.

## Cấu trúc thư mục

```
backend/       ASP.NET Core Web API (chưa scaffold — xem backend/README.md)
pos-app/       Blazor WebAssembly, dùng cho POS tại quầy (chưa scaffold)
storefront/    Next.js, trang bán hàng online (chưa scaffold)
database/      schema.sql — DDL PostgreSQL đầy đủ
docs/
  requirements/  Snapshot tài liệu yêu cầu (bản gốc trên Google Docs)
  design/        Luồng màn hình (.drawio) + ghi chú UX cần sửa
  decisions/     Lý do chọn kiến trúc kỹ thuật
tasks/         Checklist theo từng giai đoạn (1-4) + tiến độ theo tuần
.github/workflows/  CI (đang tắt job cho tới khi có project thật)
```

## Bắt đầu

1. Đọc `docs/requirements/tai-lieu-yeu-cau.md` để nắm toàn bộ scope.
2. Đọc `docs/decisions/kien-truc-ky-thuat.md` để hiểu vì sao chọn stack này.
3. Làm theo `tasks/giai-doan-1-mvp.md` trước — đây là scope MVP.
4. Scaffold từng phần theo hướng dẫn trong `backend/README.md`,
   `pos-app/README.md`, `database/README.md` (storefront để Giai đoạn 2).
5. Cập nhật `tasks/progress.md` theo tuần.

## Stack
Backend: ASP.NET Core Web API + PostgreSQL · POS: Blazor WebAssembly (PWA) ·
Storefront: React + Next.js (SSR) · Hóa đơn điện tử: tích hợp API nhà cung
cấp trung gian (Viettel/VNPT/MISA/FPT).
