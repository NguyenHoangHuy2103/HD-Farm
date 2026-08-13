# Database

`schema.sql` — DDL PostgreSQL đầy đủ cho Giai đoạn 1-3 (25 bảng: người dùng,
sản phẩm, kho, bán hàng POS+online, khách hàng/công nợ, nhà cung cấp, hóa
đơn điện tử). Đã kiểm tra cú pháp bằng parser thật của PostgreSQL (`pglast`).

## Database dùng chính — Supabase (đã chốt)

Không host PostgreSQL local nữa, dùng thẳng Supabase:

1. Tạo project trên [supabase.com](https://supabase.com), region gần VN
   nhất (Singapore).
2. Chạy `schema.sql` qua **SQL Editor** trên dashboard Supabase (dán
   nguyên nội dung file, bấm Run) — cách nhanh nhất, không cần cài `psql`.
3. Lấy connection string ở **Project Settings → Database → Connection
   string**. Dùng **Direct connection** (cổng `5432`) vì backend ASP.NET
   Core chạy dài hạn, không phải serverless. Đổi sang pooler (`6543`)
   sau này nếu cần scale nhiều connection đồng thời.
4. Lưu connection string bằng `dotnet user-secrets` (xem `backend/README.md`),
   **không** commit vào repo.

Mỗi người dev cùng trỏ vào 1 project Supabase → dữ liệu luôn đồng bộ giữa
các máy, khỏi lo lệch schema/dữ liệu test giữa từng người.

## Chạy local bằng Docker (phương án dự phòng, không bắt buộc)

Nếu cần môi trường tách biệt hoàn toàn offline (không có mạng, hoặc test
riêng không muốn ảnh hưởng dữ liệu chung trên Supabase):
```bash
docker compose up -d
```
Chạy ở thư mục gốc repo (`docker-compose.yml` đã trỏ sẵn tới `schema.sql`
để tự import khi container khởi tạo lần đầu). Kết nối vào `localhost:5432`,
database `hdfarm`, user `hdfarm`, password `hdfarm_local_only`.

## Ghi chú quan trọng (xem thêm comment cuối file schema.sql)
- Tồn kho và công nợ luôn tính từ bảng giao dịch (`stock_movements`,
  `debt_transactions`), không lưu số dư cố định.
- `order_items.batch_id` gán theo FIFO ở tầng ứng dụng, không phải trong DB.
- Bảng cho tính năng AI (Giai đoạn 4) chưa thiết kế — làm khi bắt đầu triển khai.

## Migration
Khi có EF Core, dùng `dotnet ef migrations add InitialCreate` trong
`backend/` thay vì chỉnh tay `schema.sql` — giữ `schema.sql` này làm bản
tham chiếu gốc (source of truth cho thiết kế), migration là công cụ áp dụng.
