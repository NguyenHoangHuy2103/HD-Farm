# Database

`schema.sql` — DDL PostgreSQL đầy đủ cho Giai đoạn 1-3 (25 bảng: người dùng,
sản phẩm, kho, bán hàng POS+online, khách hàng/công nợ, nhà cung cấp, hóa
đơn điện tử). Đã kiểm tra cú pháp bằng parser thật của PostgreSQL (`pglast`).

## Dùng thử local

```bash
createdb hdfarm
psql hdfarm -f schema.sql
```

## Ghi chú quan trọng (xem thêm comment cuối file schema.sql)
- Tồn kho và công nợ luôn tính từ bảng giao dịch (`stock_movements`,
  `debt_transactions`), không lưu số dư cố định.
- `order_items.batch_id` gán theo FIFO ở tầng ứng dụng, không phải trong DB.
- Bảng cho tính năng AI (Giai đoạn 4) chưa thiết kế — làm khi bắt đầu triển khai.

## Migration
Khi có EF Core, dùng `dotnet ef migrations add InitialCreate` trong
`backend/` thay vì chỉnh tay `schema.sql` — giữ `schema.sql` này làm bản
tham chiếu gốc (source of truth cho thiết kế), migration là công cụ áp dụng.
