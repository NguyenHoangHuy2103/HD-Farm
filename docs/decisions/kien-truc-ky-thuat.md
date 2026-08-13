# Quyết định kiến trúc kỹ thuật

## Stack đã chốt
- **Backend:** ASP.NET Core Web API + PostgreSQL
- **POS nội bộ:** Blazor WebAssembly
- **Storefront online:** React + Next.js (SSR)

## Lý do

1. **Tách 1 Web API dùng chung cho mọi client** (POS, web online, app di
   động và tích hợp AI ở Giai đoạn 4 sau này) thay vì làm rời từng phần —
   đỡ phải viết lại khi mở rộng.
2. **Blazor WASM cho POS:** giữ team 1 ngôn ngữ C# với backend, share
   chung DTO, dễ đóng gói thành PWA để hoạt động tạm khi mất mạng tại
   quầy (đáp ứng yêu cầu phi chức năng "Độ tin cậy").
3. **React/Next.js cho storefront:** có SSR sẵn nên SEO tốt (khách tìm sản
   phẩm qua Google), tải trang nhanh, hệ sinh thái component thương mại
   điện tử (giỏ hàng, thanh toán) phong phú, dễ tuyển dev hơn Blazor cho
   phần public-facing.
4. **PostgreSQL:** mã nguồn mở, ổn định, hợp dữ liệu quan hệ phức tạp (lô
   hàng, hạn dùng, công nợ, đơn hàng).

## Phương án khác đã cân nhắc và loại
- **Razor Pages/MVC thuần (server-rendered toàn bộ):** loại vì Giai đoạn 4
  đã có kế hoạch app di động + tích hợp đối tác giao hàng qua API — sớm
  muộn cũng cần tách API riêng, làm Razor monolith trước coi như làm 2 lần.
- **Next.js là lựa chọn duy nhất cho storefront?** Không bắt buộc — Nuxt.js
  (Vue), Astro, hoặc Blazor SSR (.NET 8) đều là lựa chọn hợp lệ. Chọn
  Next.js vì hệ sinh thái e-commerce mạnh hơn và dễ tuyển dev hơn.

## Hóa đơn điện tử
Không tự xây module — tích hợp qua nhà cung cấp trung gian đã được công
nhận (Viettel S-Invoice, VNPT eInvoice, MISA meInvoice, FPT eInvoice) qua
API, gọi ngay sau khi đơn hàng thanh toán xong. Theo Nghị định 70/2025,
hộ kinh doanh bán lẻ trực tiếp cho người tiêu dùng doanh thu >1 tỷ/năm bắt
buộc dùng hóa đơn điện tử khởi tạo từ máy tính tiền — cần xác nhận loại
hình đăng ký kinh doanh và doanh thu thực tế với kế toán trước khi chốt
nhà cung cấp tích hợp.
