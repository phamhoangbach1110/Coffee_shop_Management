using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gk
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string chuoiketnoi = @"Data Source=MSI\SQLEXPRESS;Initial Catalog=QuanNet;Integrated Security=True;TrustServerCertificate=True";
        SqlConnection conn = null;

        private void btnMay1_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            if (btn.BackColor == Color.Transparent)
            {
                DialogResult r = MessageBox.Show("Xác nhận mở máy?", "Thông báo!", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                if (r == DialogResult.OK)
                    btn.BackColor = Color.Red;
            } else
            {
                MessageBox.Show("Máy đang được dùng, mời chọn máy khác", "Thông báo!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void txtGioVao1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (((TextBox)sender).Text == "")
                    return;

                int input = Convert.ToInt32(((TextBox)sender).Text);
            }
            catch
            {
                MessageBox.Show("Chỉ được nhập số nguyên!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);               
                ((TextBox)sender).Clear();
            }

            if (txtGioVao1.Text == "" || txtGioRa1.Text == "")
                return;
            else if (txtGioVao1.Text != "" && txtGioRa1.Text != "")
            {
                int gioVao = Convert.ToInt32(txtGioVao1.Text);
                int gioRa = Convert.ToInt32(txtGioRa1.Text);

                if (gioVao >= gioRa)
                {
                    MessageBox.Show("Giờ vào phải nhỏ hơn giờ ra!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtGioRa1.Clear();
                    return;
                }

                int gioChoi = gioRa - gioVao;
                int tongTien = gioChoi * 5000;
                txtTongTien1.Text = tongTien.ToString();
            }
        }

        private void txtGioVao2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (((TextBox)sender).Text == "")
                    return;

                int input = Convert.ToInt32(((TextBox)sender).Text);
            }
            catch
            {
                MessageBox.Show("Chỉ được nhập số nguyên!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ((TextBox)sender).Clear();
            }

            if (txtGioVao2.Text == "" || txtGioRa2.Text == "")
                return;
            else if (txtGioVao2.Text != "" && txtGioRa2.Text != "")
            {
                int gioVao = Convert.ToInt32(txtGioVao2.Text);
                int gioRa = Convert.ToInt32(txtGioRa2.Text);

                if (gioVao >= gioRa)
                {
                    MessageBox.Show("Giờ vào phải nhỏ hơn giờ ra!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtGioRa1.Clear();
                    return;
                }

                int gioChoi = gioRa - gioVao;
                int tongTien = gioChoi * 5000;
                txtTongTien2.Text = tongTien.ToString();
            }
        }

        private void txtGioVao3_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (((TextBox)sender).Text == "")
                    return;

                int input = Convert.ToInt32(((TextBox)sender).Text);
            }
            catch
            {
                MessageBox.Show("Chỉ được nhập số nguyên!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ((TextBox)sender).Clear();
            }

            if (txtGioVao3.Text == "" || txtGioRa3.Text == "")
                return;
            else if (txtGioVao3.Text != "" && txtGioRa3.Text != "")
            {
                int gioVao = Convert.ToInt32(txtGioVao3.Text);
                int gioRa = Convert.ToInt32(txtGioRa3.Text);

                if (gioVao >= gioRa)
                {
                    MessageBox.Show("Giờ vào phải nhỏ hơn giờ ra!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtGioRa1.Clear();
                    return;
                }

                int gioChoi = gioRa - gioVao;
                int tongTien = gioChoi * 5000;
                txtTongTien3.Text = tongTien.ToString();
            }
        }

        private void txtGioVao4_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (((TextBox)sender).Text == "")
                    return;

                int input = Convert.ToInt32(((TextBox)sender).Text);
            }
            catch
            {
                MessageBox.Show("Chỉ được nhập số nguyên!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ((TextBox)sender).Clear();
            }

            if (txtGioVao4.Text == "" || txtGioRa4.Text == "")
                return;
            else if (txtGioVao4.Text != "" && txtGioRa4.Text != "")
            {
                int gioVao = Convert.ToInt32(txtGioVao4.Text);
                int gioRa = Convert.ToInt32(txtGioRa4.Text);

                if (gioVao >= gioRa)
                {
                    MessageBox.Show("Giờ vào phải nhỏ hơn giờ ra!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtGioRa4.Clear();
                    return;
                }

                int gioChoi = gioRa - gioVao;
                int tongTien = gioChoi * 5000;
                txtTongTien4.Text = tongTien.ToString();
            }
        }

        private void txtGioVao5_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (((TextBox)sender).Text == "")
                    return;

                int input = Convert.ToInt32(((TextBox)sender).Text);
            }
            catch
            {
                MessageBox.Show("Chỉ được nhập số nguyên!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ((TextBox)sender).Clear();
            }

            if (txtGioVao5.Text == "" || txtGioRa5.Text == "")
                return;
            else if (txtGioVao5.Text != "" && txtGioRa5.Text != "")
            {
                int gioVao = Convert.ToInt32(txtGioVao5.Text);
                int gioRa = Convert.ToInt32(txtGioRa5.Text);

                if (gioVao >= gioRa)
                {
                    MessageBox.Show("Giờ vào phải nhỏ hơn giờ ra!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtGioRa5.Clear();
                    
                }

                int gioChoi = gioRa - gioVao;
                int tongTien = gioChoi * 5000;
                txtTongTien5.Text = tongTien.ToString();
            }
        }

        private void btnThanhToan1_Click(object sender, EventArgs e)
        {
            DialogResult r = MessageBox.Show("Bạn có chắc muốn thoát không?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (r == DialogResult.OK)
            {
                using (conn = new SqlConnection(chuoiketnoi))
                {
                    conn.Open();
                    string sql = @"INSERT INTO DungMay (SoMay, GioVao, GioRa) 
                                   VALUES (@SoMay, @GioVao, @GioRa);";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@SoMay", "Máy 1");
                    cmd.Parameters.AddWithValue("@SoMay", txtGioRa1);
                    cmd.Parameters.AddWithValue("@SoMay", txtGioRa1);

                    cmd.ExecuteNonQuery();
                    conn.Close();
                }

                btnMay1.BackColor = Color.Transparent;
                txtGioRa1.Clear();
                txtGioVao1.Clear();
                txtTongTien1.Clear();
            }
        }

        private void btnThanhToan2_Click(object sender, EventArgs e)
        {
            DialogResult r = MessageBox.Show("Bạn có chắc muốn thoát không?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (r == DialogResult.OK)
            {
                using (conn = new SqlConnection(chuoiketnoi))
                {
                    conn.Open();
                    string sql = @"INSERT INTO DungMay (SoMay, GioVao, GioRa) 
                                   VALUES (@SoMay, @GioVao, @GioRa);";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@SoMay", "Máy 2");
                    cmd.Parameters.AddWithValue("@SoMay", txtGioRa2);
                    cmd.Parameters.AddWithValue("@SoMay", txtGioRa2);

                    cmd.ExecuteNonQuery();
                    conn.Close();
                }

                btnMay2.BackColor = Color.Transparent;
                txtGioRa2.Clear();
                txtGioVao2.Clear();
            }
        }

        private void btnThanhToan3_Click(object sender, EventArgs e)
        {
            DialogResult r = MessageBox.Show("Bạn có chắc muốn thoát không?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (r == DialogResult.OK)
            {
                using (conn = new SqlConnection(chuoiketnoi))
                {
                    conn.Open();
                    string sql = @"INSERT INTO DungMay (SoMay, GioVao, GioRa) 
                                   VALUES (@SoMay, @GioVao, @GioRa);";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@SoMay", "Máy 3");
                    cmd.Parameters.AddWithValue("@SoMay", txtGioRa3);
                    cmd.Parameters.AddWithValue("@SoMay", txtGioRa3);

                    cmd.ExecuteNonQuery();
                    conn.Close();
                }

                btnMay3.BackColor = Color.Transparent;
                txtGioRa3.Clear();
                txtGioVao3.Clear();
            }
        }

        private void btnThanhToan4_Click(object sender, EventArgs e)
        {
            DialogResult r = MessageBox.Show("Bạn có chắc muốn thoát không?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (r == DialogResult.OK)
            {
                using (conn = new SqlConnection(chuoiketnoi))
                {
                    conn.Open();
                    string sql = @"INSERT INTO DungMay (SoMay, GioVao, GioRa) 
                                   VALUES (@SoMay, @GioVao, @GioRa);";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@SoMay", "Máy 4");
                    cmd.Parameters.AddWithValue("@SoMay", txtGioRa4);
                    cmd.Parameters.AddWithValue("@SoMay", txtGioRa4);

                    cmd.ExecuteNonQuery();
                    conn.Close();
                }

                btnMay4.BackColor = Color.Transparent;
                txtGioRa4.Clear();
                txtGioVao4.Clear();
            }
        }

        private void btnThanhToan5_Click(object sender, EventArgs e)
        {
            DialogResult r = MessageBox.Show("Bạn có chắc muốn thoát không?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (r == DialogResult.OK)
            {
                using (conn = new SqlConnection(chuoiketnoi))
                {
                    conn.Open();
                    string sql = @"INSERT INTO DungMay (SoMay, GioVao, GioRa) 
                                   VALUES (@SoMay, @GioVao, @GioRa);";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@SoMay", "Máy 5");
                    cmd.Parameters.AddWithValue("@SoMay", txtGioRa5);
                    cmd.Parameters.AddWithValue("@SoMay", txtGioRa5);

                    cmd.ExecuteNonQuery();
                    conn.Close();
                }

                btnMay5.BackColor = Color.Transparent;
                txtGioRa5.Clear();
                txtGioVao5.Clear();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            using (conn = new SqlConnection(chuoiketnoi))
            {
                string sql = "SELECT * FROM DungMay";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvDS.DataSource = dt;
            }
        }
    }
}
