# Quy trình làm việc — HD Farm

Hướng dẫn cách dùng cấu trúc thư mục + GitHub Issues/Projects để đi từ
tài liệu yêu cầu tới sản phẩm chạy được, không bị lạc task giữa chừng.

## 1. Vai trò của từng thư mục trong lúc làm việc hàng ngày

| Thư mục | Dùng khi nào |
| --- | --- |
| `docs/requirements/` | Tra cứu yêu cầu gốc khi không chắc 1 tính năng phải làm gì |
| `docs/decisions/` | Tra lý do đã chọn kiến trúc — tránh tự hỏi lại "sao lại chọn Blazor" giữa chừng |
| `docs/design/` | Tra luồng màn hình trước khi code UI |
| `database/schema.sql` | Nguồn tham chiếu gốc cho toàn bộ entity/bảng |
| `tasks/giai-doan-X.md` | Danh sách việc cần làm của từng giai đoạn — nguồn để tạo GitHub Issues |
| `tasks/progress.md` | Nhật ký tiến độ theo tuần, cập nhật tay |

`tasks/*.md` không thay thế GitHub Issues — nó là bản checklist gốc, tĩnh,
dễ đọc nhanh. Issues trên GitHub mới là nơi theo dõi trạng thái thực tế
hàng ngày (ai làm, đang làm, xong chưa).

## 2. Setup GitHub Project (làm 1 lần)

1. Vào repo trên GitHub → tab **Projects** → **New project** → chọn
   template **Board**.
2. Tạo 4 cột: `Backlog`, `To do`, `In progress`, `Done`.
3. Thêm 2 trường tùy chỉnh (Fields):
   - `Giai đoạn` (single select): Giai đoạn 1, 2, 3, 4
   - `Module` (single select): backend, pos-app, storefront, database, docs
4. Tạo **Milestones** (tab Issues → Milestones → New milestone), 1
   milestone cho mỗi giai đoạn: `Giai đoạn 1 - MVP`, `Giai đoạn 2`,...
5. Tạo **Labels**: `giai-doan-1`...`giai-doan-4`, `backend`, `pos-app`,
   `storefront`, `database`, `bug`, `docs`.

## 3. Biến checklist trong `tasks/` thành Issues

Mỗi dòng `- [ ]` trong `tasks/giai-doan-X.md` → 1 Issue. Không cần tạo hết
1 lần — tạo issue cho phần đang làm trong tuần, còn lại giữ nguyên trong
file làm backlog tham khảo.

Tạo thủ công trên GitHub UI, hoặc nhanh hơn bằng GitHub CLI (`gh`) nếu đã
cài:

```bash
gh issue create \
  --title "[Giai đoạn 1][backend] Đăng nhập bằng SĐT + mật khẩu" \
  --body "Xem tasks/giai-doan-1-mvp.md mục Đăng nhập / phân quyền" \
  --label "giai-doan-1,backend" \
  --milestone "Giai đoạn 1 - MVP"
```

Đặt tên issue theo mẫu: `[Giai đoạn X][module] Mô tả ngắn` — dễ lọc, dễ
nhìn trên board.

## 4. Nhánh & commit

- `main`: luôn phải chạy được, chỉ nhận thay đổi qua Pull Request, không
  commit thẳng.
- Nhánh feature đặt tên: `feature/<module>-<mo-ta-ngan>`, ví dụ
  `feature/backend-dang-nhap`, `feature/pos-app-tao-don-hang`.
- Commit message theo Conventional Commits:
  - `feat: ...` — thêm tính năng
  - `fix: ...` — sửa lỗi
  - `docs: ...` — sửa tài liệu
  - `chore: ...` — việc vặt (setup, dependency)
  - `refactor: ...` — sửa cấu trúc code, không đổi hành vi

## 5. Quy trình 1 vòng làm việc (từ task tới xong)

1. Chọn 1 task trong `tasks/giai-doan-X.md`.
2. Tạo Issue tương ứng trên GitHub (bước 3), gắn label + milestone, kéo
   vào cột `To do` trên Project board.
3. Kéo issue sang `In progress` khi bắt đầu code.
4. Tạo nhánh từ `main`, code, commit theo chuẩn ở mục 4.
5. Mở Pull Request, trong mô tả PR ghi `Closes #<số issue>` — khi PR merge,
   GitHub tự đóng issue.
6. Review lại (tự review nếu làm 1 mình, hoặc nhờ người khác) trước khi
   merge vào `main`.
7. Merge xong: card trên Project board tự chuyển `Done` (nếu đã bật
   automation trong Project settings → Workflows).
8. Tick `- [x]` dòng tương ứng trong `tasks/giai-doan-X.md` để file và
   GitHub luôn khớp nhau.

## 6. Definition of Done — trước khi merge PR

- [ ] Build không lỗi (`dotnet build` / `npm run build`)
- [ ] Chạy thử đúng luồng đã mô tả trong `docs/design/` (nếu là UI)
- [ ] Không phá schema hiện có trong `database/schema.sql` (nếu đổi DB,
      cập nhật file này + tạo migration tương ứng)
- [ ] Cập nhật `docs/decisions/` nếu đổi quyết định kiến trúc
- [ ] Tick checkbox tương ứng trong `tasks/`

## 7. Nhịp làm việc hàng tuần

- Đầu tuần: xem `tasks/giai-doan-X.md`, chọn task, tạo issue cho tuần này.
- Cuối tuần: cập nhật `tasks/progress.md` (đã xong gì / đang làm gì / đang
  vướng gì), dọn Project board (đóng issue đã xong, dời issue trễ hẹn).
- Hết 1 giai đoạn: review lại toàn bộ checklist trong file giai đoạn đó,
  đảm bảo tick hết trước khi mở milestone kế tiếp.
