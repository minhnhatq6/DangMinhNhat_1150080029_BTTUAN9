using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Lab7_Winform
{
    public partial class frmThucHanh4 : Form
    {
        // QUAN TRỌNG: Thay đổi đường dẫn đến file .mdf của bạn tại đây
        string strCon = @"Data Source=NHAT;Initial Catalog=QuanLyBanSach;Integrated Security=True"; SqlConnection sqlCon = null;
        SqlDataAdapter adapter = null;
        DataSet ds = null;
        int vt = -1; // Vị trí dòng đang chọn

        public frmThucHanh4()
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

        private void frmThucHanh4_Load(object sender, EventArgs e)
        {
            HienThiDuLieu();
        }

        private void dgvDanhSach_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            vt = e.RowIndex;
        }

        private void XoaDuLieu()
        {
            try
            {
                MoKetNoi();
                DataRow row = ds.Tables["tblNhaXuatBan"].Rows[vt];
                row.Delete();
                int kq = adapter.Update(ds.Tables["tblNhaXuatBan"]);
                if (kq > 0)
                {
                    MessageBox.Show("Xóa dữ liệu thành công!");
                    vt = -1; // Reset vị trí
                }
                else
                {
                    MessageBox.Show("Xóa dữ liệu không thành công!");
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

        private void btnXoaDuLieu_Click(object sender, EventArgs e)
        {
            if (vt == -1)
            {
                MessageBox.Show("Bạn chưa chọn dữ liệu để xóa!");
                return;
            }

            DialogResult result = MessageBox.Show("Bạn có thực sự muốn xóa dòng đã chọn không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                XoaDuLieu();
                // HienThiDuLieu() không cần gọi lại vì DataSet đã được cập nhật
            }
        }
    }
}