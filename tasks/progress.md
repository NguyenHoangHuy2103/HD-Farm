# Tiến độ dự án

Nhật ký các việc đã làm, cập nhật theo tuần/phiên làm việc — để sau này
nhìn lại biết đã đi qua những gì, không phải nhớ lại từ đầu.

---

## 2026-08-13 — Khởi động dự án: từ tài liệu tới backend chạy được + nối Supabase

### 1. Review tài liệu yêu cầu
- Đọc và đánh giá tài liệu yêu cầu v5 (Google Docs) — nhận xét các module đã ổn.
- Phát hiện và chốt sửa qua comment trên Google Docs:
  - Gộp 2 vai trò "thu ngân" + "nhân viên kho" thành 1 vai trò `staff` duy nhất (cùng `admin`).
  - Bổ sung giá theo 3 cấp: lẻ / sỉ / đại lý.
  - Bổ sung yêu cầu xuất hóa đơn điện tử (không chỉ in giấy) cho cả POS và online.

### 2. Chốt kiến trúc kỹ thuật
- So sánh Razor/MVC vs Web API, React vs Vue, Next.js vs lựa chọn khác.
- Chốt: **ASP.NET Core Web API + PostgreSQL** (backend), **Blazor WebAssembly (PWA)** cho POS nội bộ, **React + Next.js (SSR)** cho storefront online.
- Lý do chọn lưu tại `docs/decisions/kien-truc-ky-thuat.md`.
- Tìm hiểu quy định hóa đơn điện tử (Nghị định 70/2025) — hộ kinh doanh bán lẻ doanh thu >1 tỷ/năm bắt buộc hóa đơn điện tử khởi tạo từ máy tính tiền. Hướng giải quyết: tích hợp qua nhà cung cấp trung gian (Viettel/VNPT/MISA/FPT), không tự xây.

### 3. Thiết kế database
- Thiết kế đầy đủ `database/schema.sql`: 25 bảng cho Giai đoạn 1-3 (người dùng, sản phẩm, kho theo lô/FIFO, bán hàng POS+online dùng chung, khách hàng/công nợ, nhà cung cấp, hóa đơn điện tử).
- Kiểm tra cú pháp bằng `pglast` (parser thật của PostgreSQL) — 48 statement, không lỗi, không có tham chiếu khóa ngoại ngược thứ tự.

### 4. Review UX / luồng màn hình
- Đọc file `HD_Farm_LuongManHinh.drawio`, đánh giá logic luồng khách hàng + luồng nhân viên/POS — phát hiện nhiều điểm thiếu logic (giỏ hàng không nối với danh mục, tạo đơn không nối thanh toán, thiếu màn xác nhận thành công...).
- Rút gọn luồng mua hàng còn **4 màn hình lõi**: Danh mục → Chi tiết sản phẩm → Giỏ hàng/Checkout gộp → Đặt hàng thành công. Cho phép khách vãng lai đặt hàng không cần đăng ký trước.
- Ghi lại tại `docs/design/ghi_chu_sua_luong_man_hinh.md` (không sửa trực tiếp file .drawio).

### 5. Dựng khung repo
- Tạo cấu trúc: `backend/`, `pos-app/`, `storefront/`, `database/`, `docs/{requirements,design,decisions}`, `tasks/`, `.github/workflows/`.
- Viết `WORKFLOW.md` — quy tắc chung: vai trò từng thư mục, cách dùng GitHub Issues/Projects, quy tắc nhánh + commit (Conventional Commits), Definition of Done, nhịp làm việc hàng tuần, quản lý secrets, database (Supabase), thông báo Discord.
- Sửa lỗi thao tác git (`git add .` nhầm, `index.lock` bị kẹt) — rút kinh nghiệm add/commit theo từng nhóm file.

### 6. Setup GitHub (Issues, Projects, Discord)
- Tạo Milestones (Giai đoạn 1-4), Labels (theo giai đoạn + theo module).
- Tạo GitHub Project dạng Board, thêm 2 field tùy chỉnh: Giai đoạn, Module.
- Soạn danh sách 10 issue cho Giai đoạn 1 (tiếng Anh), hướng dẫn tạo issue chi tiết từng bước.
- Kết nối Discord webhook nhận thông báo GitHub (Issues, Pull requests, Pushes) — xử lý xong lỗi không hiện tin nhắn (do action `labeled` không được Discord render, không phải lỗi cấu hình).
- Quyết định tạm hoãn branch protection (đang làm 1 mình).

### 7. Setup database thật — Supabase
- Tạo project Supabase (region Tokyo — ap-northeast-1), chạy `schema.sql` qua SQL Editor — đủ 25 bảng.
- Thêm file mẫu secrets: `backend/appsettings.Example.json`, `storefront/.env.example`.
- Thêm `docker-compose.yml` làm phương án PostgreSQL local dự phòng (không phải lựa chọn chính).

### 8. Scaffold backend + nối Supabase (mốc quan trọng nhất trong ngày)
- `dotnet new webapi` tạo `HdFarm.Api` (.NET 10) — phát hiện template mới không còn Swagger UI mặc định, chỉ có `/openapi/v1.json`. Thêm package `Scalar.AspNetCore` để có giao diện test API tại `/scalar/v1`.
- Tạo entity `User`, `AuditLog` + `HdFarmDbContext` (EF Core), đăng ký `AddDbContext` trong `Program.cs`.
- Xử lý chuỗi lỗi kết nối Supabase:
  1. Connection string dạng URI (`postgresql://...`) không đúng định dạng Npgsql cần (`Host=...;Port=...`).
  2. Host "Direct connection" của Supabase chỉ phân giải qua IPv6 → mạng không hỗ trợ → lỗi "No such host is known". Chuyển sang **Session pooler** (hỗ trợ IPv4).
  3. Gõ nhầm placeholder `<region>` thay vì giá trị thật `ap-northeast-1`.
  4. Migration `InitialUsers` báo bảng `users` đã tồn tại (vì đã tạo qua SQL Editor trước đó) → tự thêm bản ghi vào `__EFMigrationsHistory` để đánh dấu migration đã áp dụng, không chạy lại DDL.
- Kết quả: `dotnet ef database update` chạy thành công, backend chính thức nối được Supabase.
- Cập nhật `tasks/giai-doan-1-mvp.md`, commit theo từng nhóm việc.

### Trạng thái cuối phiên làm việc
- **Xong:** hạ tầng backend (project chạy được, có UI test API, nối DB thật).
- **Chưa làm:** scaffold `pos-app/`, project `Shared` dùng chung DTO, CI thật, và toàn bộ tính năng nghiệp vụ (đăng nhập, sản phẩm, kho, POS, hóa đơn điện tử) — vẫn là API rỗng.
- **Việc tiếp theo:** chọn 1 trong 2 — làm đăng nhập/phân quyền (đã có sẵn entity `User`) hoặc làm sản phẩm & danh mục.
