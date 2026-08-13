# Giai đoạn 2 — Bán hàng online + Công nợ + Chatbot cơ bản

## Storefront online (5.5)
- [ ] Scaffold `storefront/` (Next.js, SSR)
- [ ] Trang danh mục sản phẩm công khai (không cần đăng nhập)
- [ ] Trang chi tiết sản phẩm
- [ ] Giỏ hàng (thêm nhanh bằng dấu "+" ở trang danh sách + thêm từ trang chi tiết)
- [ ] Màn Giỏ hàng/Checkout gộp: chọn địa chỉ nhận hoặc tự lấy tại cửa hàng, chọn thanh toán, tùy chọn cọc
- [ ] Cho khách chưa có tài khoản đặt hàng được — chỉ cần SĐT lúc thanh toán, không bắt đăng ký trước
      (xem chi tiết luồng: `docs/design/ghi_chu_sua_luong_man_hinh.md`)
- [ ] Màn "Đặt hàng thành công" riêng (không đẩy thẳng vào danh sách đơn)
- [ ] Thanh toán online: tích hợp VNPay/Momo/ZaloPay, hỗ trợ COD
- [ ] Theo dõi trạng thái đơn hàng
- [ ] Đồng bộ tồn kho với kênh POS (dùng chung API/database)
- [ ] Màn tiếp nhận/xử lý đơn online phía staff (nối liền: danh sách đơn → chọn đơn → cập nhật trạng thái, không tách rời như bản vẽ gốc)

## Khách hàng & công nợ (5.6)
- [ ] Hồ sơ khách hàng (tên, SĐT, địa chỉ, diện tích canh tác)
- [ ] Theo dõi công nợ (debt_transactions) — tính từ giao dịch, không lưu số dư cố định
- [ ] Giới hạn hạn mức mua chịu theo khách hàng
- [ ] Nhắc nợ đến hạn
- [ ] Lịch sử mua hàng gộp cả 2 kênh (tại quầy + online)

## Chatbot AI cơ bản (5.9)
- [ ] Kịch bản trả lời theo từ khóa (chưa cần LLM ở giai đoạn này)
- [ ] Entry point chatbot trên storefront (floating button)

## Việc còn thiếu từ Giai đoạn 1 (nếu chưa làm)
- [ ] Quản lý địa chỉ giao hàng (cần cho bước chọn địa chỉ khi checkout)
- [ ] Đăng xuất, quên mật khẩu cho tài khoản khách hàng online
