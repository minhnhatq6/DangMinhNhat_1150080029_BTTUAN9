using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Lab7_Winform
{
    public partial class frmThucHanh2 : Form
    {
        // QUAN TRỌNG: Thay đổi đường dẫn đến file .mdf của bạn tại đây
        string strCon = @"Data Source=NHAT;Initial Catalog=QuanLyBanSach;Integrated Security=True"; SqlConnection sqlCon = null;
        SqlDataAdapter adapter = null;
        DataSet ds = null;

        public frmThucHanh2()
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

        private void frmThucHanh2_Load(object sender, EventArgs e)
        {
            HienThiDuLieu();
        }

        private void btnThemDL_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNXB.Text) || string.IsNullOrWhiteSpace(txtTenNXB.Text))
            {
                MessageBox.Show("Mã và Tên NXB không được để trống!");
                return;
            }

            try
            {
                MoKetNoi(); // Mở kết nối để adapter sử dụng
                DataRow row = ds.Tables["tblNhaXuatBan"].NewRow();
                row["NXB"] = txtNXB.Text.Trim();
                row["TenNXB"] = txtTenNXB.Text.Trim();
                row["DiaChi"] = txtDiaChi.Text.Trim();
                ds.Tables["tblNhaXuatBan"].Rows.Add(row);

                int kq = adapter.Update(ds.Tables["tblNhaXuatBan"]);
                if (kq > 0)
                {
                    MessageBox.Show("Thêm dữ liệu thành công!");
                    // Không cần gọi HienThiDuLieu() lại vì DataSet đã được cập nhật
                    XoaDuLieuForm();
                }
                else
                {
                    MessageBox.Show("Thêm dữ liệu không thành công!");
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