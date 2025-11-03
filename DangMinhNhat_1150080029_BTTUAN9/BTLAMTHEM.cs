using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DangMinhNhat_1150080029_BTTUAN9 
{
    public partial class BTLAMTHEM : Form
    {
        string strCon = @"Data Source=NHAT;Initial Catalog=QuanLyBanHang;Integrated Security=True";        SqlConnection conn;
        DataTable dtSanPham;
        bool isNew; // Cờ để biết đang Thêm mới hay Sửa

        public BTLAMTHEM()
        {
            InitializeComponent();
        }

        private void BTLAMTHEM_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection(strCon);
            ResetForm();
            LoadData();
        }

        // Tải dữ liệu từ DB lên DataGridView
        private void LoadData()
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                string sql = "SELECT * FROM SanPham";
                SqlDataAdapter adapt = new SqlDataAdapter(sql, conn);
                dtSanPham = new DataTable();
                adapt.Fill(dtSanPham);

                dgvSanPham.DataSource = dtSanPham;
                SetupDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        // Cài đặt hiển thị cho DataGridView
        private void SetupDataGridView()
        {
            dgvSanPham.Columns[0].HeaderText = "Mã SP";
            dgvSanPham.Columns[1].HeaderText = "Tên Sản Phẩm";
            dgvSanPham.Columns[2].HeaderText = "Ngày Sản Xuất";
            dgvSanPham.Columns[3].HeaderText = "Ngày Hết Hạn";
            dgvSanPham.Columns[4].HeaderText = "Đơn Vị";
            dgvSanPham.Columns[5].HeaderText = "Đơn Giá";
            dgvSanPham.Columns[6].HeaderText = "Ghi Chú";

            dgvSanPham.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvSanPham.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgvSanPham.AllowUserToAddRows = false;
            dgvSanPham.EditMode = DataGridViewEditMode.EditProgrammatically;
            dgvSanPham.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        // Reset form về trạng thái ban đầu
        private void ResetForm()
        {
            // Vô hiệu hóa và xóa trắng GroupBox Chi tiết
            grbChiTiet.Enabled = false;
            txtMaSP.Text = "";
            txtTenSP.Text = "";
            dtpNgaySX.Value = DateTime.Now;
            dtpNgayHH.Value = DateTime.Now;
            txtDonVi.Text = "";
            txtDonGia.Text = "";
            txtGhiChu.Text = "";

            // Kích hoạt các nút chính, vô hiệu hóa nút chức năng
            btnThem.Enabled = true;
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btnTimKiem.Enabled = true;
            btnThoat.Enabled = true;

            btnLuu.Enabled = false;
            btnHuy.Enabled = false;
        }

        // Khi click vào 1 dòng trên DataGridView
        private void dgvSanPham_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra xem có phải đang ở chế độ Thêm/Sửa không
            if (grbChiTiet.Enabled) return;

            // Lấy index của dòng được chọn
            int index = e.RowIndex;
            if (index < 0 || index >= dgvSanPham.Rows.Count) return;

            // Lấy ra dòng dữ liệu
            DataRow row = dtSanPham.Rows[index];

            // Gán dữ liệu vào các control trong groupbox Chi tiết
            txtMaSP.Text = row["MaSP"].ToString();
            txtTenSP.Text = row["TenSP"].ToString();
            dtpNgaySX.Value = Convert.ToDateTime(row["NgaySX"]);
            dtpNgayHH.Value = Convert.ToDateTime(row["NgayHH"]);
            txtDonVi.Text = row["DonVi"].ToString();
            txtDonGia.Text = row["DonGia"].ToString();
            txtGhiChu.Text = row["GhiChu"].ToString();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            isNew = true; // Đánh dấu là Thêm mới
            ResetForm(); // Xóa trắng các ô nhập
            grbChiTiet.Enabled = true; // Kích hoạt groupbox chi tiết
            txtMaSP.Enabled = true; // Cho phép nhập Mã SP
            txtMaSP.Focus();

            // Vô hiệu hóa các nút chính
            btnThem.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;

            // Kích hoạt nút Lưu và Hủy
            btnLuu.Enabled = true;
            btnHuy.Enabled = true;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem đã chọn sản phẩm nào chưa
            if (string.IsNullOrEmpty(txtMaSP.Text))
            {
                MessageBox.Show("Bạn phải chọn một sản phẩm để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            isNew = false; // Đánh dấu là Sửa
            grbChiTiet.Enabled = true; // Kích hoạt groupbox chi tiết
            txtMaSP.Enabled = false; // Không cho phép sửa Mã SP (khóa chính)
            txtTenSP.Focus();

            // Vô hiệu hóa các nút chính
            btnThem.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;

            // Kích hoạt nút Lưu và Hủy
            btnLuu.Enabled = true;
            btnHuy.Enabled = true;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            // Kiểm tra dữ liệu nhập
            if (string.IsNullOrWhiteSpace(txtMaSP.Text) || string.IsNullOrWhiteSpace(txtTenSP.Text))
            {
                MessageBox.Show("Mã SP và Tên SP không được để trống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                if (isNew) // Nếu là Thêm mới
                {
                    // 1. Kiểm tra trùng mã
                    string checkSql = "SELECT COUNT(*) FROM SanPham WHERE MaSP = @MaSP";
                    SqlCommand checkCmd = new SqlCommand(checkSql, conn);
                    checkCmd.Parameters.AddWithValue("@MaSP", txtMaSP.Text.Trim());
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count > 0)
                    {
                        MessageBox.Show("Mã sản phẩm này đã tồn tại. Vui lòng nhập mã khác.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtMaSP.Focus();
                        return;
                    }

                    // 2. Thêm mới
                    string insertSql = @"INSERT INTO SanPham(MaSP, TenSP, NgaySX, NgayHH, DonVi, DonGia, GhiChu) 
                                         VALUES (@MaSP, @TenSP, @NgaySX, @NgayHH, @DonVi, @DonGia, @GhiChu)";
                    SqlCommand cmd = new SqlCommand(insertSql, conn);
                    BindParameters(cmd);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Thêm sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else // Nếu là Sửa
                {
                    string updateSql = @"UPDATE SanPham SET TenSP = @TenSP, NgaySX = @NgaySX, NgayHH = @NgayHH, 
                                         DonVi = @DonVi, DonGia = @DonGia, GhiChu = @GhiChu WHERE MaSP = @MaSP";
                    SqlCommand cmd = new SqlCommand(updateSql, conn);
                    BindParameters(cmd);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Cập nhật sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                LoadData(); // Tải lại dữ liệu
                ResetForm(); // Reset lại form
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        // Gán tham số cho SqlCommand (dùng chung cho Thêm và Sửa)
        private void BindParameters(SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("@MaSP", txtMaSP.Text.Trim());
            cmd.Parameters.AddWithValue("@TenSP", txtTenSP.Text.Trim());
            cmd.Parameters.AddWithValue("@NgaySX", dtpNgaySX.Value);
            cmd.Parameters.AddWithValue("@NgayHH", dtpNgayHH.Value);
            cmd.Parameters.AddWithValue("@DonVi", txtDonVi.Text.Trim());

            // Chuyển đổi Đơn giá sang decimal để tránh lỗi
            decimal donGia = 0;
            decimal.TryParse(txtDonGia.Text, out donGia);
            cmd.Parameters.AddWithValue("@DonGia", donGia);

            cmd.Parameters.AddWithValue("@GhiChu", txtGhiChu.Text.Trim());
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            ResetForm();
            // Lấy lại dữ liệu từ dòng đang chọn nếu có
            if (dgvSanPham.CurrentRow != null)
                dgvSanPham_CellClick(dgvSanPham, new DataGridViewCellEventArgs(0, dgvSanPham.CurrentRow.Index));
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaSP.Text))
            {
                MessageBox.Show("Bạn phải chọn một sản phẩm để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa sản phẩm này không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    string deleteSql = "DELETE FROM SanPham WHERE MaSP = @MaSP";
                    SqlCommand cmd = new SqlCommand(deleteSql, conn);
                    cmd.Parameters.AddWithValue("@MaSP", txtMaSP.Text);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Xóa sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    ResetForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string maSP = txtTimKiemMaSP.Text.Trim();
            string tenSP = txtTimKiemTenSP.Text.Trim();

            // Nếu cả hai ô tìm kiếm đều trống, tải lại toàn bộ dữ liệu
            if (string.IsNullOrEmpty(maSP) && string.IsNullOrEmpty(tenSP))
            {
                LoadData();
                return;
            }

            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                string searchSql = "SELECT * FROM SanPham WHERE 1=1";
                SqlCommand cmd = new SqlCommand();

                if (!string.IsNullOrEmpty(maSP))
                {
                    searchSql += " AND MaSP LIKE @MaSP";
                    cmd.Parameters.AddWithValue("@MaSP", "%" + maSP + "%");
                }
                if (!string.IsNullOrEmpty(tenSP))
                {
                    searchSql += " AND TenSP LIKE @TenSP";
                    cmd.Parameters.AddWithValue("@TenSP", "%" + tenSP + "%");
                }

                cmd.CommandText = searchSql;
                cmd.Connection = conn;

                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                dtSanPham = new DataTable();
                adapt.Fill(dtSanPham);

                dgvSanPham.DataSource = dtSanPham;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
        }
    }
}