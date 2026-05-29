using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Coffee_shop_Manager
{
    public partial class FormQuantity : Form
    {
        public FormQuantity()
        {
            InitializeComponent();
        }

        //Gọi FormOrder
        private FormOrder formChinh = null;
        private string monID = null;

        //Hiện thông tin tên món và giá món
        public FormQuantity(string maMon, string tenMon, string giaMon, string soLuong, string soBan, FormOrder f1)
        {
            InitializeComponent();
            lblTenMon.Text = tenMon;
            lblDonGia.Text = giaMon;
            lblSoBan.Text = soBan;
            
            //Lưu số lượng của món đã chọn
            txtQuantity.Text = soLuong;

            //Ghi nhớ Form1 chuyển sang
            formChinh = f1;

            //Mã món
            monID = maMon;
        }

        private void txtQuantity_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //Lấy số lượng
                int soLuong = Convert.ToInt32(((TextBox)sender).Text);

                //Mở khóa nút xác nhận
                btnConfirmQuantity.Enabled = true;

                //Số lượng phải trong khoảng từ 0 đến 100
                if (soLuong < 0 || soLuong > 100)
                {
                    MessageBox.Show("Số lượng mặt hàng phải trong khoảng từ 0 - 100!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtQuantity.Text = "0";
                    return;
                }

                //Lấy đơn giá
                int donGia = Convert.ToInt32(lblDonGia.Text.Replace(",", "").Replace("đ", ""));

                //Tính giá thành: số lượng * đơn giá
                int thanhTien = donGia * soLuong;

                //Hiển thị giá thành
                lblThanhTien.Text = $"{thanhTien.ToString("N0")}đ";
            }
            catch
            {
                if (txtQuantity.Text == "")
                {
                    //Vô hiệu hóa nút "Xác nhận" nếu nội dung nhập là rỗng
                    btnConfirmQuantity.Enabled = false;
                }
                else
                {
                    MessageBox.Show("Dữ liệu không hợp lệ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtQuantity.Text = "0";
                }
            }
        }

        //Nút tăng
        private void btnIncrease_Click(object sender, EventArgs e)
        {
            int soLuong = Convert.ToInt32(((TextBox)txtQuantity).Text);

            soLuong += 1;
            txtQuantity.Text = soLuong.ToString();
        }

        //Nút giảm
        private void btnDecrease_Click(object sender, EventArgs e)
        {
            int soLuong = Convert.ToInt32(((TextBox)txtQuantity).Text);

            soLuong -= 1;
            txtQuantity.Text = soLuong.ToString();
        }

        //Reset giá trị
        private void btnResetQuantity_Click(object sender, EventArgs e)
        {
            txtQuantity.Text = "0";
        }

        //Xác nhận
        private void btnConfirmQuantity_Click(object sender, EventArgs e)
        {
            //Lấy dữ liệu đã nhập
            string tenMon = lblTenMon.Text;
            string soLuong = txtQuantity.Text;
            string giaThanh = lblThanhTien.Text;
            string donGia = lblDonGia.Text;
            string soBan = lblSoBan.Text;
            string maMon = monID;

            //Thêm vào danh sách
            if (formChinh != null)
            {
                formChinh.themVaoDanhSach(maMon, tenMon, soLuong, donGia, giaThanh, soBan);
            }

            //Đóng form
            Close();
        }
    }
}
