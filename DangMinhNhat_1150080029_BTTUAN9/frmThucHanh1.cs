using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Lab7_Winform
{
    public partial class frmThucHanh1 : Form
    {

        string strCon = @"Data Source=NHAT;Initial Catalog=QuanLyBanSach;Integrated Security=True"; SqlConnection sqlCon = null;

        public frmThucHanh1()
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

        private void btnHienThi_Click(object sender, EventArgs e)
        {
            try
            {
                MoKetNoi();
                string sql = "select * from NhaXuatBan";
                SqlDataAdapter adapter = new SqlDataAdapter(sql, sqlCon);
                DataSet ds = new DataSet();
                adapter.Fill(ds, "tblNhaXuatBan");
                dgvDanhSach.DataSource = ds.Tables["tblNhaXuatBan"];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally
            {
                DongKetNoi();
            }
        }
    }
}