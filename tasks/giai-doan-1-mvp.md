# Giai đoạn 1 — MVP

Mục tiêu: đăng nhập/phân quyền, quản lý sản phẩm & kho cơ bản, bán hàng
tại quầy (POS), hóa đơn điện tử. Bắt đầu thu thập dữ liệu có cấu trúc cho
AI ở giai đoạn sau.

## Hạ tầng / khung dự án
- [x] Scaffold `backend/` (ASP.NET Core Web API, có Scalar UI để test API)
- [ ] Scaffold `pos-app/` (Blazor WebAssembly, bật PWA)
- [x] Database dùng Supabase (không phải local) — đã chạy `schema.sql` qua SQL Editor
- [x] Backend nối được Supabase qua EF Core (Npgsql) — migration `InitialUsers` đã đồng bộ, entity `User`/`AuditLog` đã có
- [ ] Tách project `Shared` dùng chung DTO giữa backend và pos-app
- [ ] Setup CI cơ bản (bật lại job `backend` trong `.github/workflows/ci.yml`)

## Đăng nhập / phân quyền (5.1)
- [ ] Đăng nhập bằng username/SĐT + mật khẩu
- [ ] "Ghi nhớ đăng nhập" trên thiết bị cố định
- [ ] Session timeout tự động đăng xuất
- [ ] Quên mật khẩu / đặt lại qua SĐT hoặc admin cấp lại
- [ ] Khóa tài khoản tạm sau nhiều lần đăng nhập sai
- [ ] Audit log cho sửa giá, xóa đơn, điều chỉnh tồn kho
- [ ] Phân quyền: quản trị viên / staff

## Sản phẩm & danh mục (5.2)
- [ ] CRUD danh mục (categories, hỗ trợ cây cha-con)
- [ ] CRUD sản phẩm (sku, barcode, giá nhập/bán, mô tả, hoạt chất)
- [ ] Quy đổi đơn vị (product_units)
- [ ] Giá theo cấp: lẻ / sỉ / đại lý (product_prices)
- [ ] Tìm kiếm theo tên/mã/quét mã vạch

## Kho & tồn kho (5.3)
- [ ] Nhập kho theo lô (batch_no, ngày SX, hạn dùng, giá nhập)
- [ ] Xuất kho tự động khi bán (FIFO theo hạn dùng)
- [ ] Xuất kho do hư hỏng/hủy
- [ ] Cảnh báo tồn kho thấp
- [ ] Cảnh báo hàng sắp hết hạn
- [ ] Kiểm kho định kỳ (stock_takes)

## Bán hàng tại quầy — POS (5.4)
- [ ] Giao diện bán nhanh: quét mã vạch/tìm kiếm sản phẩm
- [ ] Tạo đơn → thanh toán (tiền mặt/chuyển khoản/ghi nợ) — **nối liền 2
      bước này**, không để tách rời như trong bản vẽ luồng gốc
- [ ] Giảm giá theo đơn/theo sản phẩm
- [ ] Trả hàng/đổi hàng, hoàn tiền hoặc điều chỉnh công nợ
- [ ] Lưu lịch sử đơn hàng

## Hóa đơn điện tử
- [ ] Xác nhận với kế toán: loại hình đăng ký (hộ kinh doanh/công ty) + doanh thu dự kiến → chốt loại hóa đơn bắt buộc
- [ ] Đăng ký tài khoản với 1 nhà cung cấp trung gian (Viettel/VNPT/MISA/FPT)
- [ ] Tích hợp API phát hành hóa đơn ngay sau khi đơn hàng thanh toán xong
- [ ] Lưu trạng thái hóa đơn (bảng `einvoices`)

## Chuẩn bị dữ liệu cho AI (giai đoạn sau)
- [ ] Lưu ảnh sản phẩm có cấu trúc (để dùng cho AI Vision Giai đoạn 4)
- [ ] Đảm bảo dữ liệu bán hàng đủ trường (product_id, batch_id, ngày, số lượng...) để dùng cho dự báo sau này
