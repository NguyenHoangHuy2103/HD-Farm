-- =====================================================================
-- WEB APP QUẢN LÝ & BÁN VẬT TƯ NÔNG NGHIỆP
-- Database schema (PostgreSQL)
-- Phạm vi: Giai đoạn 1-3 theo roadmap (không gồm bảng AI giai đoạn 4)
-- =====================================================================

-- ---------------------------------------------------------------------
-- ENUM TYPES
-- ---------------------------------------------------------------------
CREATE TYPE user_role AS ENUM ('admin', 'staff');
CREATE TYPE order_channel AS ENUM ('pos', 'online');
CREATE TYPE order_status AS ENUM ('pending', 'processing', 'shipping', 'completed', 'cancelled');
CREATE TYPE payment_method AS ENUM ('cash', 'bank_transfer', 'debt', 'gateway', 'cod');
CREATE TYPE stock_movement_type AS ENUM ('import', 'export_sale', 'export_damage', 'adjustment');
CREATE TYPE price_tier AS ENUM ('retail', 'wholesale', 'agent');
CREATE TYPE debt_transaction_type AS ENUM ('purchase_on_credit', 'repayment');
CREATE TYPE einvoice_status AS ENUM ('pending', 'issued', 'error', 'cancelled');

-- =====================================================================
-- 1. NGƯỜI DÙNG & PHÂN QUYỀN (mục 5.1)
-- =====================================================================

CREATE TABLE users (
    id                    BIGSERIAL PRIMARY KEY,
    username              VARCHAR(50) UNIQUE,
    phone                 VARCHAR(20) UNIQUE,
    password_hash         TEXT NOT NULL,
    full_name             VARCHAR(150) NOT NULL,
    role                  user_role NOT NULL DEFAULT 'staff',
    is_active             BOOLEAN NOT NULL DEFAULT TRUE,
    failed_login_attempts INT NOT NULL DEFAULT 0,
    locked_until          TIMESTAMPTZ,
    last_login_at         TIMESTAMPTZ,
    created_at            TIMESTAMPTZ NOT NULL DEFAULT now(),
    updated_at            TIMESTAMPTZ NOT NULL DEFAULT now(),
    deleted_at            TIMESTAMPTZ,
    CONSTRAINT users_login_identifier_chk CHECK (username IS NOT NULL OR phone IS NOT NULL)
);

CREATE TABLE audit_logs (
    id          BIGSERIAL PRIMARY KEY,
    user_id     BIGINT REFERENCES users(id),
    action      VARCHAR(100) NOT NULL,
    entity_type VARCHAR(50),
    entity_id   BIGINT,
    detail      JSONB,
    created_at  TIMESTAMPTZ NOT NULL DEFAULT now()
);

-- =====================================================================
-- 2. SẢN PHẨM & DANH MỤC (mục 5.2)
-- =====================================================================

CREATE TABLE categories (
    id         BIGSERIAL PRIMARY KEY,
    name       VARCHAR(100) NOT NULL,
    parent_id  BIGINT REFERENCES categories(id),
    created_at TIMESTAMPTZ NOT NULL DEFAULT now(),
    updated_at TIMESTAMPTZ NOT NULL DEFAULT now(),
    deleted_at TIMESTAMPTZ
);

CREATE TABLE products (
    id                BIGSERIAL PRIMARY KEY,
    sku               VARCHAR(50) UNIQUE NOT NULL,
    barcode           VARCHAR(50) UNIQUE,
    name              VARCHAR(255) NOT NULL,
    category_id       BIGINT REFERENCES categories(id),
    base_unit         VARCHAR(20) NOT NULL,
    manufacturer      VARCHAR(150),
    active_ingredient TEXT,
    usage_instruction TEXT,
    description       TEXT,
    is_active         BOOLEAN NOT NULL DEFAULT TRUE,
    created_at        TIMESTAMPTZ NOT NULL DEFAULT now(),
    updated_at        TIMESTAMPTZ NOT NULL DEFAULT now(),
    deleted_at        TIMESTAMPTZ
);

-- Quy đổi đơn vị, vd: 1 bao = 50kg
CREATE TABLE product_units (
    id              BIGSERIAL PRIMARY KEY,
    product_id      BIGINT NOT NULL REFERENCES products(id) ON DELETE CASCADE,
    unit_name       VARCHAR(20) NOT NULL,
    conversion_rate NUMERIC(12,3) NOT NULL CHECK (conversion_rate > 0),
    is_default      BOOLEAN NOT NULL DEFAULT FALSE,
    UNIQUE (product_id, unit_name)
);

-- Giá theo cấp: lẻ / sỉ / đại lý
CREATE TABLE product_prices (
    id              BIGSERIAL PRIMARY KEY,
    product_id      BIGINT NOT NULL REFERENCES products(id) ON DELETE CASCADE,
    price_tier      price_tier NOT NULL DEFAULT 'retail',
    price           NUMERIC(14,2) NOT NULL CHECK (price >= 0),
    effective_from  TIMESTAMPTZ NOT NULL DEFAULT now(),
    UNIQUE (product_id, price_tier)
);

-- =====================================================================
-- 3. NHÀ CUNG CẤP (mục 5.7)
-- =====================================================================

CREATE TABLE suppliers (
    id           BIGSERIAL PRIMARY KEY,
    name         VARCHAR(200) NOT NULL,
    contact_name VARCHAR(150),
    phone        VARCHAR(20),
    address      TEXT,
    created_at   TIMESTAMPTZ NOT NULL DEFAULT now(),
    updated_at   TIMESTAMPTZ NOT NULL DEFAULT now(),
    deleted_at   TIMESTAMPTZ
);

CREATE TABLE purchase_orders (
    id           BIGSERIAL PRIMARY KEY,
    supplier_id  BIGINT NOT NULL REFERENCES suppliers(id),
    status       VARCHAR(30) NOT NULL DEFAULT 'draft',
    created_by   BIGINT REFERENCES users(id),
    created_at   TIMESTAMPTZ NOT NULL DEFAULT now()
);

CREATE TABLE purchase_order_items (
    id                BIGSERIAL PRIMARY KEY,
    purchase_order_id BIGINT NOT NULL REFERENCES purchase_orders(id) ON DELETE CASCADE,
    product_id        BIGINT NOT NULL REFERENCES products(id),
    quantity          NUMERIC(14,3) NOT NULL CHECK (quantity > 0),
    unit_price        NUMERIC(14,2) NOT NULL CHECK (unit_price >= 0)
);

CREATE TABLE supplier_payments (
    id          BIGSERIAL PRIMARY KEY,
    supplier_id BIGINT NOT NULL REFERENCES suppliers(id),
    amount      NUMERIC(14,2) NOT NULL CHECK (amount > 0),
    paid_at     TIMESTAMPTZ NOT NULL DEFAULT now(),
    note        TEXT
);

-- =====================================================================
-- 4. KHO & TỒN KHO (mục 5.3)
-- =====================================================================

-- Lô hàng: cốt lõi cho theo dõi hạn dùng + FIFO
CREATE TABLE stock_batches (
    id            BIGSERIAL PRIMARY KEY,
    product_id    BIGINT NOT NULL REFERENCES products(id),
    batch_no      VARCHAR(50),
    supplier_id   BIGINT REFERENCES suppliers(id),
    mfg_date      DATE,
    exp_date      DATE,
    import_price  NUMERIC(14,2) NOT NULL CHECK (import_price >= 0),
    qty_received  NUMERIC(14,3) NOT NULL CHECK (qty_received >= 0),
    qty_remaining NUMERIC(14,3) NOT NULL CHECK (qty_remaining >= 0),
    created_at    TIMESTAMPTZ NOT NULL DEFAULT now()
);

-- Nhật ký mọi biến động kho (nhập/xuất/hủy/điều chỉnh)
CREATE TABLE stock_movements (
    id             BIGSERIAL PRIMARY KEY,
    batch_id       BIGINT NOT NULL REFERENCES stock_batches(id),
    movement_type  stock_movement_type NOT NULL,
    quantity       NUMERIC(14,3) NOT NULL,
    reference_type VARCHAR(50),
    reference_id   BIGINT,
    created_by     BIGINT REFERENCES users(id),
    created_at     TIMESTAMPTZ NOT NULL DEFAULT now()
);

CREATE TABLE stock_takes (
    id         BIGSERIAL PRIMARY KEY,
    take_date  DATE NOT NULL DEFAULT CURRENT_DATE,
    created_by BIGINT REFERENCES users(id),
    note       TEXT,
    created_at TIMESTAMPTZ NOT NULL DEFAULT now()
);

CREATE TABLE stock_take_items (
    id            BIGSERIAL PRIMARY KEY,
    stock_take_id BIGINT NOT NULL REFERENCES stock_takes(id) ON DELETE CASCADE,
    product_id    BIGINT NOT NULL REFERENCES products(id),
    batch_id      BIGINT REFERENCES stock_batches(id),
    system_qty    NUMERIC(14,3) NOT NULL,
    actual_qty    NUMERIC(14,3) NOT NULL,
    diff          NUMERIC(14,3) GENERATED ALWAYS AS (actual_qty - system_qty) STORED
);

-- =====================================================================
-- 5. KHÁCH HÀNG & CÔNG NỢ (mục 5.6)
-- =====================================================================

CREATE TABLE customers (
    id                BIGSERIAL PRIMARY KEY,
    name              VARCHAR(150) NOT NULL,
    phone             VARCHAR(20) UNIQUE,
    address           TEXT,
    cultivation_area  NUMERIC(10,2),
    credit_limit      NUMERIC(14,2) NOT NULL DEFAULT 0,
    is_online_account BOOLEAN NOT NULL DEFAULT FALSE,
    email             VARCHAR(150) UNIQUE,
    password_hash     TEXT,
    created_at        TIMESTAMPTZ NOT NULL DEFAULT now(),
    updated_at        TIMESTAMPTZ NOT NULL DEFAULT now(),
    deleted_at        TIMESTAMPTZ
);

CREATE TABLE delivery_addresses (
    id             BIGSERIAL PRIMARY KEY,
    customer_id    BIGINT NOT NULL REFERENCES customers(id) ON DELETE CASCADE,
    recipient_name VARCHAR(150),
    phone          VARCHAR(20),
    address_line   TEXT NOT NULL,
    is_default     BOOLEAN NOT NULL DEFAULT FALSE
);

-- =====================================================================
-- 6. GIỎ HÀNG ONLINE (mục 5.5)
-- =====================================================================

CREATE TABLE carts (
    id          BIGSERIAL PRIMARY KEY,
    customer_id BIGINT NOT NULL REFERENCES customers(id) ON DELETE CASCADE,
    created_at  TIMESTAMPTZ NOT NULL DEFAULT now(),
    updated_at  TIMESTAMPTZ NOT NULL DEFAULT now()
);

CREATE TABLE cart_items (
    id         BIGSERIAL PRIMARY KEY,
    cart_id    BIGINT NOT NULL REFERENCES carts(id) ON DELETE CASCADE,
    product_id BIGINT NOT NULL REFERENCES products(id),
    quantity   NUMERIC(14,3) NOT NULL CHECK (quantity > 0),
    UNIQUE (cart_id, product_id)
);

-- =====================================================================
-- 7. BÁN HÀNG — DÙNG CHUNG POS (5.4) & ONLINE (5.5)
-- =====================================================================

CREATE TABLE orders (
    id                  BIGSERIAL PRIMARY KEY,
    order_no            VARCHAR(30) UNIQUE NOT NULL,
    channel             order_channel NOT NULL,
    customer_id         BIGINT REFERENCES customers(id),
    staff_id            BIGINT REFERENCES users(id),
    status              order_status NOT NULL DEFAULT 'completed',
    subtotal            NUMERIC(14,2) NOT NULL DEFAULT 0,
    discount_amount     NUMERIC(14,2) NOT NULL DEFAULT 0,
    total_amount        NUMERIC(14,2) NOT NULL DEFAULT 0,
    delivery_address_id BIGINT REFERENCES delivery_addresses(id),
    note                TEXT,
    created_at          TIMESTAMPTZ NOT NULL DEFAULT now(),
    updated_at          TIMESTAMPTZ NOT NULL DEFAULT now()
);

CREATE TABLE order_items (
    id               BIGSERIAL PRIMARY KEY,
    order_id         BIGINT NOT NULL REFERENCES orders(id) ON DELETE CASCADE,
    product_id       BIGINT NOT NULL REFERENCES products(id),
    batch_id         BIGINT REFERENCES stock_batches(id),
    quantity         NUMERIC(14,3) NOT NULL CHECK (quantity > 0),
    unit_price       NUMERIC(14,2) NOT NULL CHECK (unit_price >= 0),
    discount_amount  NUMERIC(14,2) NOT NULL DEFAULT 0,
    line_total       NUMERIC(14,2) GENERATED ALWAYS AS (quantity * unit_price - discount_amount) STORED
);

CREATE TABLE payments (
    id             BIGSERIAL PRIMARY KEY,
    order_id       BIGINT NOT NULL REFERENCES orders(id) ON DELETE CASCADE,
    method         payment_method NOT NULL,
    amount         NUMERIC(14,2) NOT NULL CHECK (amount > 0),
    gateway_txn_id VARCHAR(100),
    paid_at        TIMESTAMPTZ NOT NULL DEFAULT now()
);

CREATE TABLE order_returns (
    id            BIGSERIAL PRIMARY KEY,
    order_id      BIGINT NOT NULL REFERENCES orders(id),
    reason        TEXT,
    refund_amount NUMERIC(14,2) NOT NULL DEFAULT 0,
    processed_by  BIGINT REFERENCES users(id),
    created_at    TIMESTAMPTZ NOT NULL DEFAULT now()
);

CREATE TABLE order_return_items (
    id               BIGSERIAL PRIMARY KEY,
    order_return_id  BIGINT NOT NULL REFERENCES order_returns(id) ON DELETE CASCADE,
    order_item_id    BIGINT NOT NULL REFERENCES order_items(id),
    quantity         NUMERIC(14,3) NOT NULL CHECK (quantity > 0)
);

-- Công nợ khách hàng: luôn tính từ bảng giao dịch, không lưu số dư cố định
CREATE TABLE debt_transactions (
    id             BIGSERIAL PRIMARY KEY,
    customer_id    BIGINT NOT NULL REFERENCES customers(id),
    order_id       BIGINT REFERENCES orders(id),
    type           debt_transaction_type NOT NULL,
    amount         NUMERIC(14,2) NOT NULL CHECK (amount > 0),
    balance_after  NUMERIC(14,2) NOT NULL,
    created_by     BIGINT REFERENCES users(id),
    created_at     TIMESTAMPTZ NOT NULL DEFAULT now()
);

-- Hóa đơn điện tử (tích hợp qua nhà cung cấp trung gian: Viettel/VNPT/MISA/FPT)
CREATE TABLE einvoices (
    id            BIGSERIAL PRIMARY KEY,
    order_id      BIGINT NOT NULL UNIQUE REFERENCES orders(id),
    provider      VARCHAR(50),
    invoice_no    VARCHAR(50),
    status        einvoice_status NOT NULL DEFAULT 'pending',
    issued_at     TIMESTAMPTZ,
    pdf_url       TEXT,
    raw_response  JSONB,
    created_at    TIMESTAMPTZ NOT NULL DEFAULT now()
);

-- =====================================================================
-- INDEXES
-- =====================================================================

CREATE INDEX idx_products_barcode ON products (barcode);
CREATE INDEX idx_products_sku ON products (sku);
CREATE INDEX idx_products_category ON products (category_id);

CREATE INDEX idx_stock_batches_product ON stock_batches (product_id);
CREATE INDEX idx_stock_batches_exp_date ON stock_batches (exp_date);
CREATE INDEX idx_stock_movements_batch ON stock_movements (batch_id);

CREATE INDEX idx_orders_created_at ON orders (created_at);
CREATE INDEX idx_orders_customer ON orders (customer_id);
CREATE INDEX idx_orders_channel_status ON orders (channel, status);

CREATE INDEX idx_order_items_order ON order_items (order_id);
CREATE INDEX idx_order_items_product ON order_items (product_id);

CREATE INDEX idx_customers_phone ON customers (phone);
CREATE INDEX idx_debt_transactions_customer ON debt_transactions (customer_id);

CREATE INDEX idx_audit_logs_user ON audit_logs (user_id);
CREATE INDEX idx_audit_logs_entity ON audit_logs (entity_type, entity_id);

-- =====================================================================
-- GHI CHÚ
-- =====================================================================
-- 1. Tồn kho / công nợ luôn TÍNH TOÁN từ bảng giao dịch (stock_movements,
--    debt_transactions), không lưu số dư cố định trong products/customers,
--    để tránh lệch số khi có chỉnh sửa hoặc lỗi.
-- 2. order_items.batch_id nên được gán tự động theo nguyên tắc FIFO
--    (lấy stock_batches có exp_date sớm nhất còn qty_remaining > 0) tại
--    thời điểm xử lý đơn hàng ở tầng ứng dụng/service, không phải ở DB.
-- 3. Báo cáo (mục 5.8) dùng VIEW / MATERIALIZED VIEW tổng hợp từ các bảng
--    trên, không cần bảng riêng ở giai đoạn này.
-- 4. Các bảng AI (chatbot log, kết quả nhận diện sâu bệnh, dự báo nhu cầu)
--    thuộc Giai đoạn 4, chưa đưa vào schema này — thiết kế khi bắt đầu
--    triển khai để tránh đoán sai cấu trúc dữ liệu cần cho mô hình.
