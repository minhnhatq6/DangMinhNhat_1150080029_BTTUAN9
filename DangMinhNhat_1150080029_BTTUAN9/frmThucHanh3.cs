using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Lab7_Winform
{
    public partial class frmThucHanh3 : Form
    {
        
        string strCon = @"Data Source=NHAT;Initial Catalog=QuanLyBanSach;Integrated Security=True"; SqlConnection sqlCon = null;
        SqlDataAdapter adapter = null;
        DataSet ds = null;
        int vt = -1; // Vị trí dòng đang chọn trong DataGridView

        public frmThucHanh3()
        {
            InitializeComponent();
        }

        // Hàm mở kết nối
        private void MoKetNoi()
        {
            if (sqlCon == null)
            {
                sqlCon = new SqlConnection(strCon);
            }
            if (sqlCon.State == ConnectionState.Closed)
            {
                sqlCon.Open();
            }
        }

        // Hàm đóng kết nối
        private void DongKetNoi()
        {
            if (sqlCon != null && sqlCon.State == ConnectionState.Open)
            {
                sqlCon.Close();
            }
        }

        // Hàm xóa dữ liệu trên form
        private void XoaDuLieuForm()
        {
            txtNXB.Text = "";
            txtTenNXB.Text = "";
            txtDiaChi.Text = "";
            vt = -1;
            txtNXB.Focus();
        }

        // Hàm hiển thị dữ liệu trên datagridview
        private void HienThiDuLieu()
        {
            try
            {
                MoKetNoi();
                string query = "SELECT * FROM NhaXuatBan";
                adapter = new SqlDataAdapter(query, sqlCon);
                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                ds = new DataSet();
                adapter.Fill(ds, "tblNhaXuatBan");
                dgvDanhSach.DataSource = ds.Tables["tblNhaXuatBan"];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message);
            }
            finally
            {
                DongKetNoi();
            }
        }

        private void frmThucHanh3_Load(object sender, EventArgs e)
        {
            HienThiDuLieu();
            XoaDuLieuForm();
        }

        private void dgvDanhSach_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            vt = e.RowIndex;
            if (vt == -1 || vt >= ds.Tables["tblNhaXuatBan"].Rows.Count) return;

            DataRow row = ds.Tables["tblNhaXuatBan"].Rows[vt];
            txtNXB.Text = row["NXB"].ToString().Trim();
            txtTenNXB.Text = row["TenNXB"].ToString().Trim();
            txtDiaChi.Text = row["DiaChi"].ToString().Trim();
        }

        private void btnChinhSuaThongTin_Click(object sender, EventArgs e)
        {
            if (vt == -1)
            {
                MessageBox.Show("Bạn chưa chọn dữ liệu để chỉnh sửa!");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNXB.Text) || string.IsNullOrWhiteSpace(txtTenNXB.Text))
            {
                MessageBox.Show("Mã và Tên NXB không được để trống!");
                return;
            }

            try
            {
                MoKetNoi();
                DataRow row = ds.Tables["tblNhaXuatBan"].Rows[vt];
                row.BeginEdit();
                row["NXB"] = txtNXB.Text.Trim();
                row["TenNXB"] = txtTenNXB.Text.Trim();
                row["DiaChi"] = txtDiaChi.Text.Trim();
                row.EndEdit();

                int kq = adapter.Update(ds.Tables["tblNhaXuatBan"]);
                if (kq > 0)
                {
                    MessageBox.Show("Chỉnh sửa dữ liệu thành công!");
                    HienThiDuLieu(); // Tải lại để chắc chắn
                    XoaDuLieuForm();
                }
                else
                {
                    MessageBox.Show("Chỉnh sửa dữ liệu không thành công! Dữ liệu có thể không thay đổi.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
                HienThiDuLieu(); // Tải lại dữ liệu gốc nếu có lỗi
            }
            finally
            {
                DongKetNoi();
            }
        }
    }
}