# Storefront — Next.js (React)

Chưa scaffold. Node đã có sẵn, có thể khởi tạo trực tiếp trên máy bạn:

```bash
cd storefront
npx create-next-app@latest . --typescript --app --tailwind --eslint
```

## Ghi chú
- Gọi API qua `backend` (REST, xem OpenAPI/Swagger do backend expose).
- Cần SSR cho các trang danh mục/chi tiết sản phẩm để đảm bảo SEO (đã thống
  nhất lý do trong `../docs/decisions/`).
- Luồng mua hàng ưu tiên làm trước: xem `../docs/design/ghi_chu_sua_luong_man_hinh.md`
  mục 1 (4 bước: danh mục → chi tiết sản phẩm → giỏ hàng/checkout → đặt hàng
  thành công). Không bắt đăng ký tài khoản trước khi xem/mua hàng.
- Việc cần làm đầu tiên: xem `../tasks/giai-doan-2.md` (storefront thuộc
  Giai đoạn 2 theo roadmap).
