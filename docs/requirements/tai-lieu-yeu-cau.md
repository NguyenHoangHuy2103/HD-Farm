# Tài liệu yêu cầu & tính năng — Web App Vật tư Nông nghiệp

> Copy từ Google Docs (Tai_lieu_yeu_cau_web_app_vat_tu_nong_nghiep_v5) để
> version-control cùng code. Bản gốc trên Google Docs vẫn là nơi thảo luận/
> comment chính; file này là snapshot để tham chiếu khi code.

Bán lẻ cho nông dân · Quản lý kho · Bán tại cửa hàng & bán hàng online
Phiên bản 1.0 | Tháng 8/2026

## 1. Giới thiệu

Tài liệu này mô tả yêu cầu và tính năng cho web app quản lý và bán vật tư nông nghiệp (phân bón, thuốc bảo vệ thực vật, hạt giống, dụng cụ canh tác...). Hệ thống phục vụ hai nhóm người dùng: cửa hàng (quản lý kho, bán hàng tại quầy) và khách hàng (xem sản phẩm, đặt hàng và thanh toán online).

## 2. Mục tiêu dự án

- Số hóa việc quản lý tồn kho vật tư nông nghiệp, giảm thất thoát và tình trạng hết hàng/tồn đọng.
- Tăng tốc độ và độ chính xác khi bán hàng tại quầy, thay thế ghi chép thủ công.
- Theo dõi hạn sử dụng và số lô của vật tư.
- Quản lý công nợ khách hàng (mua chịu phổ biến giữa nông dân và cửa hàng vật tư).
- Cung cấp báo cáo doanh thu, lợi nhuận, tồn kho theo thời gian thực.
- Mở kênh bán hàng online, đồng bộ tồn kho/đơn hàng với kênh tại quầy.
- Ứng dụng AI: tư vấn khách hàng, nhận diện sâu bệnh qua ảnh, dự báo nhập hàng.

## 3. Đối tượng người dùng

| Vai trò | Mô tả | Nhu cầu chính |
| --- | --- | --- |
| Chủ cửa hàng / Quản trị viên | Sở hữu và quản lý toàn bộ hệ thống | Báo cáo, cấu hình giá, quản lý người dùng, kiểm soát kho |
| Staff (gộp thu ngân + quản kho) | Bán hàng tại quầy, nhập/xuất kho, kiểm kho | Tạo đơn bán, tra cứu sản phẩm/giá, in hóa đơn, ghi nhận nhập hàng, cập nhật lô/hạn dùng |
| Khách hàng mua tại cửa hàng | Mua vật tư trực tiếp, có thể mua chịu | Ghi nhận công nợ, tra cứu lịch sử mua (qua nhân viên) |
| Khách hàng mua online | Nông dân tự đặt hàng qua web/app | Xem sản phẩm & giá, đặt hàng, thanh toán online, theo dõi đơn |

> Đã chốt: 2 vai trò vận hành — Quản trị viên và Staff (không tách riêng
> thu ngân / nhân viên kho như bản v5 gốc).

## 4. Phạm vi hệ thống

Phục vụ đồng thời 2 kênh: tại quầy (POS nội bộ) và online (cổng bán hàng công khai), dùng chung dữ liệu sản phẩm/tồn kho.

**Trong phạm vi:** đăng nhập/đăng xuất & phân quyền, quản lý sản phẩm, quản lý kho, POS, cổng bán hàng online, quản lý khách hàng & công nợ, quản lý nhà cung cấp, báo cáo thống kê.

**Ngoài phạm vi (giai đoạn sau):** app di động native, đa chi nhánh, giao hàng tự động qua API đối tác, chatbot (đưa vào Giai đoạn 2 dạng cơ bản).

## 5. Các module chức năng

### 5.1 Đăng nhập / Đăng xuất & phân quyền
- Đăng nhập tài khoản riêng từng nhân viên (username/SĐT + mật khẩu).
- "Ghi nhớ đăng nhập" trên thiết bị cố định tại quầy.
- Session timeout tự động đăng xuất.
- Quên mật khẩu / đặt lại qua SĐT hoặc admin cấp lại.
- Khóa tài khoản tạm thời sau nhiều lần đăng nhập sai.
- Audit log: lịch sử đăng nhập, sửa giá, xóa đơn, điều chỉnh tồn kho.
- Phân quyền theo vai trò: **quản trị viên, staff**.

### 5.2 Quản lý sản phẩm & danh mục
- Danh mục: phân bón, thuốc BVTV, hạt giống, dụng cụ, vật tư khác.
- Thông tin: tên, SKU/barcode, đơn vị tính, giá nhập, giá bán, nhà sản xuất.
- Quy đổi đơn vị (1 bao = 50kg, bán lẻ theo kg hoặc theo bao).
- Tìm kiếm nhanh theo tên/mã/quét mã vạch.
- Thành phần hoạt chất, hướng dẫn sử dụng (thuốc BVTV).
- **Giá theo cấp: lẻ / sỉ / đại lý** (bổ sung sau review).

### 5.3 Quản lý kho & tồn kho
- Nhập kho: số lượng, giá nhập, nhà cung cấp, số lô, ngày sản xuất, hạn dùng.
- Xuất kho tự động trừ khi bán; hỗ trợ xuất do hư hỏng/hủy.
- Cảnh báo tồn kho thấp, cảnh báo hàng sắp hết hạn.
- FIFO theo lô hàng.
- Kiểm kho định kỳ, ghi nhận chênh lệch.

### 5.4 Bán hàng tại quầy (POS)
- Giao diện bán nhanh: quét mã vạch/tìm kiếm.
- Giảm giá theo đơn hoặc theo sản phẩm.
- Thanh toán: tiền mặt, chuyển khoản, ghi nợ.
- **Xuất hóa đơn điện tử** (không chỉ in giấy — bổ sung sau review, xem mục Hóa đơn điện tử bên dưới).
- Trả hàng/đổi hàng, hoàn tiền/điều chỉnh công nợ.
- Lưu lịch sử đơn hàng.

### 5.5 Bán hàng online cho khách hàng
- Danh mục sản phẩm công khai, tìm kiếm/lọc theo nhóm, mục đích sử dụng.
- Giỏ hàng, đặt hàng (địa chỉ, hình thức nhận: giao hàng / tự lấy).
- Thanh toán online: VNPay/Momo/ZaloPay, hỗ trợ COD.
- Theo dõi trạng thái đơn, lịch sử đơn & **hóa đơn điện tử**.
- Đồng bộ tồn kho chung với kênh tại quầy.
- Màn hình tiếp nhận/xử lý đơn online phía cửa hàng.

> **Luồng mua hàng tối ưu (đã chốt sau review UX — xem docs/design/):**
> Danh mục sản phẩm → Chi tiết sản phẩm → Giỏ hàng/Checkout (gộp) → Đặt
> hàng thành công. Không bắt đăng ký tài khoản trước khi xem/mua hàng —
> chỉ cần SĐT ở bước thanh toán.

### 5.6 Quản lý khách hàng & công nợ
- Hồ sơ khách hàng: tên, SĐT, địa chỉ, diện tích canh tác (tuỳ chọn).
- Công nợ: số tiền còn nợ, lịch sử mua chịu/trả nợ.
- Nhắc nợ đến hạn, giới hạn hạn mức mua chịu.
- Lịch sử mua hàng (cả tại quầy và online).

### 5.7 Quản lý nhà cung cấp & nhập hàng
- Hồ sơ nhà cung cấp, công nợ phải trả.
- Phiếu nhập hàng, đối chiếu đơn đặt hàng.
- Theo dõi công nợ phải trả & lịch sử thanh toán.

### 5.8 Báo cáo & thống kê
- Doanh thu/lợi nhuận theo ngày/tuần/tháng, tách kênh tại quầy/online.
- Sản phẩm bán chạy/chậm theo mùa vụ.
- Tồn kho, giá trị tồn kho, hàng sắp hết hạn.
- Công nợ khách hàng & nhà cung cấp.
- Đơn hàng online: tỉ lệ hoàn tất, hủy, thời gian xử lý.
- Xuất Excel/PDF.

### 5.9 Tính năng AI (triển khai theo giai đoạn)
- **Chatbot tư vấn khách hàng** (Giai đoạn 2 — kịch bản từ khóa; nâng cấp LLM ở Giai đoạn 3).
- **Nhận diện sâu bệnh qua ảnh — AI Vision** (Giai đoạn 4).
- **Dự báo nhu cầu & tồn kho** (Giai đoạn 4).
- Nên thu thập ảnh sản phẩm/sâu bệnh + dữ liệu bán hàng có cấu trúc từ Giai đoạn 1.

## 6. Yêu cầu phi chức năng

| Nhóm | Yêu cầu |
| --- | --- |
| Hiệu năng | Thao tác bán hàng phản hồi dưới 1–2 giây |
| Thiết bị | PC/tablet tại quầy; hỗ trợ máy quét mã vạch, máy in hóa đơn |
| Độ tin cậy | Ổn định khi mạng chập chờn (POS cần chế độ offline tạm thời — dùng Blazor WASM PWA) |
| Bảo mật | Phân quyền rõ ràng, mã hóa mật khẩu, sao lưu định kỳ |
| Khả năng mở rộng | Cho phép bổ sung đa chi nhánh sau này không cần viết lại từ đầu |
| Ngôn ngữ | Giao diện tiếng Việt, đơn vị đo lường phù hợp thị trường VN |

## 7. Kiến trúc kỹ thuật (đã chốt)

- **Backend:** ASP.NET Core Web API + PostgreSQL.
- **POS nội bộ:** Blazor WebAssembly (PWA, hỗ trợ offline tạm thời).
- **Storefront online:** React + Next.js (SSR cho SEO).
- **Thanh toán online:** VNPay/Momo/ZaloPay qua API chính thức, không tự lưu thông tin thẻ.
- **Hóa đơn điện tử:** tích hợp qua nhà cung cấp trung gian đã công nhận (Viettel S-Invoice, VNPT eInvoice, MISA meInvoice, FPT eInvoice) — gọi API ngay sau khi đơn hàng thanh toán xong. Không tự xây module hóa đơn điện tử.
  - Theo NĐ 70/2025: hộ kinh doanh bán lẻ trực tiếp cho người tiêu dùng, doanh thu >1 tỷ/năm bắt buộc dùng hóa đơn điện tử khởi tạo từ máy tính tiền, kết nối dữ liệu cơ quan thuế. Cần xác nhận với kế toán loại hình đăng ký và doanh thu thực tế.
- **Hạ tầng:** cloud (VPS trong nước hoặc AWS/GCP), HTTPS bắt buộc.
- **AI/LLM:** API mô hình ngôn ngữ (vd Claude) cho chatbot; computer vision cho nhận diện sâu bệnh; pipeline dự báo từ dữ liệu bán hàng lịch sử.
- Lý do chọn stack: xem `docs/decisions/kien-truc-ky-thuat.md`.

## 8. Lộ trình phát triển

| Giai đoạn | Nội dung chính |
| --- | --- |
| Giai đoạn 1 – MVP | Đăng nhập/phân quyền, quản lý sản phẩm & kho cơ bản, bán hàng tại quầy (POS), hóa đơn điện tử. Bắt đầu thu thập dữ liệu có cấu trúc cho AI sau này |
| Giai đoạn 2 | Cổng bán hàng online, quản lý khách hàng & công nợ, Chatbot AI cơ bản |
| Giai đoạn 3 | Quản lý nhà cung cấp & nhập hàng, cảnh báo tồn kho/hạn dùng, báo cáo đầy đủ, nâng cấp chatbot bằng LLM |
| Giai đoạn 4 (mở rộng) | AI Vision nhận diện sâu bệnh, AI dự báo nhu cầu & tồn kho, app di động, đa chi nhánh, tích hợp đối tác giao hàng |

Chi tiết task từng giai đoạn: xem `tasks/`.
