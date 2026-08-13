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
  commit thẳng. **Bắt buộc bật branch protection** (xem mục 8) để GitHub
  tự chặn push thẳng, không dựa vào tự giác.
- Nhánh feature đặt tên: `feature/<module>-<mo-ta-ngan>`, ví dụ
  `feature/backend-dang-nhap`, `feature/pos-app-tao-don-hang`.
- Commit message theo Conventional Commits, mỗi commit chỉ nên gộp 1 nhóm
  việc liên quan (không `git add .` rồi commit hết mọi thứ cùng lúc):
  - `feat: ...` — thêm tính năng
  - `fix: ...` — sửa lỗi
  - `docs: ...` — sửa tài liệu
  - `chore: ...` — việc vặt (setup, dependency, CI)
  - `refactor: ...` — sửa cấu trúc code, không đổi hành vi
  - `test: ...` — thêm/sửa test
- Có thể viết tiếng Việt hoặc tiếng Anh sau dấu `:`, miễn nhất quán trong
  cùng 1 PR. Ví dụ: `feat(backend): add login endpoint`.

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

## 8. Branch protection (chưa bật — làm sau khi có nhiều người cùng code)

Tạm thời chưa cần, vì đang làm 1 mình nên tự giác theo quy tắc "không
commit thẳng vào main" ở mục 4 là đủ. Bật lại khi nào có thêm người cùng
làm, hoặc thấy hay lỡ tay push thẳng. Cách bật:

Vào repo → **Settings → Branches → Add branch protection rule** →
áp dụng cho nhánh `main`:
- Tick **"Require a pull request before merging"** — chặn push thẳng.
- Tick **"Require status checks to pass before merging"** sau khi CI đã
  bật thật (mục 6 file backend/README.md) — chặn merge code không build
  được.

## 9. Quản lý secrets / biến môi trường

- Không commit connection string, API key (thanh toán, hóa đơn điện tử,
  AI) vào repo — các file này đã nằm trong `.gitignore`
  (`appsettings.Development.json`, `.env`, `.env.local`).
- Mỗi project (`backend/`, `pos-app/`, `storefront/`) nên có file mẫu
  **không chứa giá trị thật** để biết cần khai báo biến gì, ví dụ
  `backend/appsettings.Example.json`, `storefront/.env.example` — các
  file `.example` này ĐƯỢC commit bình thường, chỉ file thật (không có
  đuôi `.example`) mới bị ignore.
- Khi deploy thật, secrets nằm trong GitHub Actions Secrets (Settings →
  Secrets and variables → Actions) hoặc biến môi trường trên server/cloud,
  không nằm trong code.

## 10. Database — Supabase

Dùng chung 1 project **Supabase** (PostgreSQL managed) thay vì mỗi máy tự
cài/host riêng — mọi người luôn thấy cùng 1 dữ liệu, khỏi lệch schema
giữa các máy. Chi tiết setup + connection string: `database/README.md`.
Có phương án Docker local dự phòng (`docker-compose.yml`) nếu cần môi
trường tách biệt offline, nhưng không phải lựa chọn chính.

## 11. Thông báo Discord

Repo đã gắn webhook GitHub → Discord (kênh thông báo chung), nhận sự
kiện: Issues, Pull requests, Pushes. Không cần check GitHub liên tục,
theo dõi qua Discord là đủ cho nhịp làm việc hàng ngày. Một số action
phụ (như "gắn label", có thể cả "edited") sẽ không hiện tin nhắn dù
GitHub gửi thành công (204) — đây là giới hạn của Discord, không phải
lỗi cấu hình, không cần xử lý gì thêm.
