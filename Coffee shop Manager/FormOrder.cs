using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

//This is new
namespace Coffee_shop_Manager
{
    public partial class FormOrder : Form
    {
        string chuoiketnoi = @"Data Source=MSI\SQLEXPRESS;Initial Catalog=Coffee Management;Integrated Security=True;TrustServerCertificate=True";
        SqlConnection conn = null;

        private void LoadMenu(string menuCategory)
        {
            flpDanhSachMon.Controls.Clear();
            using (conn = new SqlConnection(chuoiketnoi))
            {
                try
                {
                    string sql = "SELECT MenuItemID, ItemName, UnitPrice FROM Menu WHERE Category = @dm";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@dm", menuCategory);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); // Dùng Reader để đọc từng dòng tốc độ cao

                    // Vòng lặp: Cứ mỗi món trong SQL, C# sẽ tự tạo ra 1 nút bấm
                    while (reader.Read())
                    {
                        // Lấy dữ liệu từ SQL ra biến
                        string maMon = reader["MenuItemID"].ToString();
                        string tenMon = reader["ItemName"].ToString();
                        double giaMon = Convert.ToDouble(reader["UnitPrice"]);

                        // Khởi tạo một đối tượng Button mới bằng code
                        Button btn = new Button();
                        btn.Text = $"{tenMon}\n{giaMon:N0}đ"; // Hiển thị Tên và Giá xuống dòng
                        btn.Width = 160;                      // Định kích thước nút
                        btn.Height = 45;
                        btn.FlatStyle = FlatStyle.Flat;       // Chỉnh style phẳng cho đẹp
                        btn.FlatAppearance.BorderSize = 1;
                        btn.FlatAppearance.BorderColor = Color.Gray;
                        btn.BackColor = Color.FromArgb(45, 45, 45); // Màu nền tối cho nút
                        btn.ForeColor = Color.White;
                        btn.Font = new Font("Times New Roman", 10);
                        btn.Cursor = Cursors.Hand;

                        //Lưu tạm mã món vào thuộc tính tag
                        btn.Tag = maMon;

                        // Gán sự kiện Click chung cho nút bấm
                        btn.Click += BtnMonAn_Click;

                        // Thả nút vừa tạo vào FlowLayoutPanel
                        flpDanhSachMon.Controls.Add(btn);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " +  ex.Message);
                }
            }
        }

        private void BtnMonAn_Click(object sender, EventArgs e)
        {
            // Xác định chính xác nút bấm nào vừa được click
            Button btn = (Button)sender;

            string[] noiDungButton = btn.Text.Split('\n');
            string tenMon = noiDungButton[0];               //Lấy tên món
            string giaMon = noiDungButton[1];               //Lấy giá món

            string tieuDe = lblOrderTitle.Text;             //Lấy tên tiêu đề
            string soBan = (tieuDe.EndsWith("Mang về")) ? "00" : tieuDe.Substring(tieuDe.Length - 2, 2);

            string maMon = btn.Tag.ToString();

            string soLuong = "0";

            //Lấy số lượng món đã chọn đã lưu trong database
            using (conn = new SqlConnection(chuoiketnoi))
            {
                string sqlGetSoLuong = @"SELECT od.QuanTiTy 
                                         FROM OrderDetails AS od
                                         INNER JOIN Orders AS o ON od.OrderID = o.OrderID
                                         WHERE od.MenuItemID = @MenuItemID AND o.TableNumber = @TableNumber AND o.OrderStatus = N'Đang phục vụ';";
                SqlCommand cmdGetSoLuong = new SqlCommand(sqlGetSoLuong, conn);
                cmdGetSoLuong.Parameters.AddWithValue("@MenuItemID", maMon);
                cmdGetSoLuong.Parameters.AddWithValue("@TableNumber", soBan);

                conn.Open();
                object check = cmdGetSoLuong.ExecuteScalar();           //Giá trị trả về của câu lệnh sql trên
                conn.Close();

                if (check != null)                                      //Nếu giá trị không null thì gán soLuong = giá trị đó
                    soLuong = check.ToString();
            }

            FormQuantity f = new FormQuantity(maMon, tenMon, giaMon, soLuong, soBan, this);     //Truyền dữ liệu vào form2
            f.ShowDialog();
        }

        public FormOrder()
        {
            InitializeComponent();
        }

        private void btnBan1_Click(object sender, EventArgs e)
        {
            lblOrderTitle.Text = "Order - " + ((Button)sender).Text;

            //Trạng thái bàn
            if (((Button)sender).BackColor == Color.DarkGreen)
            {
                lblStatusTag.BackColor = Color.DarkGreen;
                lblStatusTag.ForeColor = Color.FromArgb(192, 255, 192);
                lblStatusTag.Text = "Trống";
                btnThanhToan.Hide();
            }
            else if (((Button)sender).BackColor == Color.DarkOrange)
            {
                lblStatusTag.BackColor = Color.DarkOrange;
                lblStatusTag.ForeColor = Color.White;
                lblStatusTag.Text = "Đang dùng";
                btnThanhToan.Show();
            }

            //HightLight nút bàn đang chọn
            foreach (Button btn in tlpTableList.Controls)
            {
                if (btn.ForeColor == Color.Red)
                {
                    btn.ForeColor = Color.FromArgb(192, 255, 192);
                    break;
                }
            }

            ((Button)sender).ForeColor = Color.Red;

            //Lấy dữ liệu
            string tenBan = ((Button)sender).Text;
            string soBan = (tenBan == "Mang về") ? "00" : tenBan.Substring(tenBan.Length - 2, 2);
            LoadDataGridView(soBan);
        }

        private void btnCaPhe_Click(object sender, EventArgs e)
        {
            //Chọn bàn trước
            if (lblOrderTitle.Text == "Vui lòng chọn bàn")
            {
                MessageBox.Show("Vui lòng chọn bàn trước!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //HightLight nút danh mục đã chọn
            foreach (Button btn in tlpCategory.Controls)
            {
                btn.BackColor = Color.Lavender;
                btn.ForeColor = Color.FromArgb(255, 128, 0);
            }
            ((Button)sender).BackColor = Color.FromArgb(255, 128, 0);
            ((Button)sender).ForeColor = Color.Lavender;

            //Truyền tên danh mục vào, lấy dữ liệu từ database
            LoadMenu(((Button)sender).Text);
        }

        //Hủy
        private void btnCancel_Click(object sender, EventArgs e)
        {
            //Xác nhận hủy đơn
            DialogResult r = MessageBox.Show("Bạn có muốn hủy đơn!", "Lưu ý", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (r != DialogResult.Yes)
                return;

            //Kiểm tra đặt đồ chưa
            if (dgvChosen.Rows.Count == 0)
            {
                MessageBox.Show("Vui lòng đặt món!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //Hủy đơn hàng
            foreach (Button table in tlpTableList.Controls)
            {
                if (table.ForeColor == Color.Red)
                {
                    table.BackColor = Color.DarkGreen;
                    lblStatusTag.BackColor = Color.DarkGreen;
                    lblStatusTag.Text = "Trống";
                    btnThanhToan.Hide();

                    using (conn = new SqlConnection(chuoiketnoi))
                    {
                        string tenBan = table.Text;
                        string soBan = (tenBan == "Mang về") ? "00" : tenBan.Substring(tenBan.Length - 2, 2);

                        string sqlCancelOrder = @"UPDATE Orders SET OrderStatus = @OrderStatus WHERE TableNumber = @TableNumber;";
                        SqlCommand cmdCancelOrder = new SqlCommand(sqlCancelOrder, conn);
                        cmdCancelOrder.Parameters.AddWithValue("@OrderStatus", "Đã hủy");
                        cmdCancelOrder.Parameters.AddWithValue("@TableNumber", soBan);

                        conn.Open();
                        cmdCancelOrder.ExecuteNonQuery();
                        conn.Close();

                        LoadTableStatusFree(soBan);

                        LoadDataGridView(soBan);
                    }

                    MessageBox.Show("Hủy đơn hàng thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
        }

        //Xác nhận
        private void btnBookTable_Click(object sender, EventArgs e)
        {
            //Kiểm tra đã chọn đồ chưa
            if (dgvChosen.Rows.Count == 0)
            {
                MessageBox.Show("Vui lòng đặt món!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //Chuyển trạng thái bàn đang chọn thành "đang dùng"
            foreach (Button btn in tlpTableList.Controls)
            {
                if (btn.ForeColor == Color.Red)
                {
                    btn.BackColor = Color.DarkOrange;
                    lblStatusTag.BackColor = Color.DarkOrange;
                    lblStatusTag.Text = "Đang dùng";
                    btnThanhToan.Show();

                    string tenBan = btn.Text;
                    string soBan = (tenBan == "Mang về") ? "00" : tenBan.Substring(tenBan.Length - 2, 2);
                    LoadTableStatusInUse(soBan);

                    return;
                }
            }

            MessageBox.Show("Vui lòng chọn bàn", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            //Xác nhận thanh toán
            DialogResult r = MessageBox.Show("Xác nhận thanh toán!", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (r != DialogResult.Yes)
                return;

            foreach (Button btn in tlpTableList.Controls)
            {
                if (btn.ForeColor == Color.Red && btn.BackColor == Color.DarkOrange)
                {
                    btn.BackColor = Color.DarkGreen;
                    lblStatusTag.BackColor = Color.DarkGreen;
                    lblStatusTag.Text = "Trống";
                    btnThanhToan.Hide();

                    using (conn = new SqlConnection(chuoiketnoi))
                    {
                        string tenBan = btn.Text;
                        string soBan = (tenBan == "Mang về") ? "00" : tenBan.Substring(tenBan.Length - 2, 2);

                        string sqlCancelOrder = @"UPDATE Orders SET OrderStatus = @OrderStatus WHERE TableNumber = @TableNumber;";
                        SqlCommand cmdCancelOrder = new SqlCommand(sqlCancelOrder, conn);
                        cmdCancelOrder.Parameters.AddWithValue("@OrderStatus", "Đã thanh toán");
                        cmdCancelOrder.Parameters.AddWithValue("@TableNumber", soBan);

                        conn.Open();
                        cmdCancelOrder.ExecuteNonQuery();
                        conn.Close();

                        LoadTableStatusFree(soBan);

                        LoadDataGridView(soBan);
                    }

                    MessageBox.Show("Thanh toán thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            MessageBox.Show("Vui lòng chọn bàn", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        //Thêm vào danh sách các món đã chọn, hàm công khai
        //Quy trình được thực hiện sau khi đã chọn bàn, chọn tên món, nhập số lượng và bấm vào nút "Thêm món"
        //Tóm tắt quy trình nạp dữ liệu vào CSDL:
        //B1: Kiểm tra xem trong bảng Orders, cùng một bàn, có đơn có OrderID = x đang là trạng thái "Đang phục vụ" không
        //TH1: Nếu chưa có -> Thêm vào bảng Orders: OrderID = x, Trạng thái "Đang phục vụ", sau đó lưu mã đơn x vào một biến int maDon
        //TH2: Nếu có rồi, lưu mã đơn x đó vào một biến int maDon
        //B2: Kiểm tra xem trong bảng OrderDetails, cùng khóa ngoại OrderID = x, đã có món cùng MenuItemID với món đã chọn chưa
        //TH1: Nếu chưa có -> Thêm vào bảng OrderDetails: OrderID = maDon (đã lưu ở bước 1)
        //TH2: Nếu đã có, kiểm tra xem số lượng có thay đổi không
        //TH2.1: Nếu số lượng không thay đổi -> bỏ qua
        //TH2.2: Nêu số lượng = 0 -> xóa khỏi bảng OrderDetails
        //TH2.3: Nếu số lượng có giá trị thay đổi khác 0, -> cập nhật số lượng mới vào chính dòng có OrderID = x và MenuItemID đó

        public void themVaoDanhSach(string maMon, string tenMon, string soLuong, string giaThanh, string soBan)
        {
            int maDon = -1;
            //Thêm vào cơ sở dữ liệu
            try
            {
                using (conn = new SqlConnection(chuoiketnoi))
                {
                    //Kiểm tra xem bàn đã có đơn với orderID = x đang phục vụ chưa
                    string sqlCheck = @"SELECT OrderID 
                                        FROM Orders 
                                        WHERE TableNumber = @tablenumber AND OrderStatus = @status";
                    SqlCommand cmdCheck = new SqlCommand(sqlCheck, conn);
                    cmdCheck.Parameters.AddWithValue("@tablenumber", soBan);
                    cmdCheck.Parameters.AddWithValue("@status", "Đang phục vụ");

                    //Truyền giá trị vào check
                    conn.Open();
                    object check = cmdCheck.ExecuteScalar();
                    conn.Close();

                    //BƯỚC 1: Bảng Orders
                    //Nếu check = null -> chưa có, tạo đơn mới thêm vào bảng Orders
                    //Nếu đã có, lấy mã đơn đó
                    if (check == null)
                    {
                        //Không thêm vào dữ liệu nếu số lượng là 0
                        if (soLuong == "0")
                            return;

                        //Hóa đơn
                        string sqlInsertOrder = @"INSERT INTO Orders (TableNumber, OrderTime, OrderStatus)
                                                VALUES (@tablenumber, @orderdate, @status);
                                                SELECT SCOPE_IDENTITY();";

                        SqlCommand cmdInsertOrder = new SqlCommand(sqlInsertOrder, conn);
                        cmdInsertOrder.Parameters.AddWithValue("@tablenumber", soBan);
                        cmdInsertOrder.Parameters.AddWithValue("@orderdate", DateTime.Now);
                        cmdInsertOrder.Parameters.AddWithValue("@status", "Đang phục vụ");

                        conn.Open();
                        maDon = Convert.ToInt32(cmdInsertOrder.ExecuteScalar());
                        conn.Close();
                    }
                    else
                    {
                        //Lấy mã id của đơn đang xét
                        maDon = Convert.ToInt32(check);
                    }


                    //BƯỚC 2: Bảng OrderDetails
                    //Kiểm tra xem trong orderdetails, orderID = x đã tồn tại món đã chọn chưa
                    string sqlCheckDetail = @"SELECT Quantity
                                              FROM OrderDetails
                                              WHERE OrderID = @OrderID AND MenuItemID = @MenuItemID;";
                    SqlCommand cmdCheckDetail = new SqlCommand(sqlCheckDetail, conn);
                    cmdCheckDetail.Parameters.AddWithValue("@OrderID", maDon);
                    cmdCheckDetail.Parameters.AddWithValue("@MenuItemID", maMon);

                    conn.Open();
                    object detailCheck = cmdCheckDetail.ExecuteScalar();
                    conn.Close();

                    //Nếu chưa có -> cập nhật vào bảng orderDetails dòng mới với orderID = x
                    //Nếu có rồi -> kiểm tra xem số lượng món đó có thay đổi không
                    if (detailCheck == null)
                    {
                        //Tránh lỗi chia cho 0
                        if (soLuong == "0")
                            return;

                        //Tính đơn giá
                        int donGia = Convert.ToInt32(giaThanh.Replace(",", "").Replace("đ", "")) / Convert.ToInt32(soLuong);

                        //Món chưa có trong bảng -> cập nhật thêm món mới
                        string sqlInsertDetail = @"INSERT INTO OrderDetails (OrderID, MenuItemID, Quantity, UnitPrice)
                                                   VALUES (@OrderID, @MenuItemID, @Quantity, @UnitPrice);";

                        SqlCommand cmdInsertDetail = new SqlCommand(sqlInsertDetail, conn);
                        cmdInsertDetail.Parameters.AddWithValue("OrderID", maDon);
                        cmdInsertDetail.Parameters.AddWithValue("MenuItemID", maMon);
                        cmdInsertDetail.Parameters.AddWithValue("Quantity", soLuong);
                        cmdInsertDetail.Parameters.AddWithValue("UnitPrice", donGia);

                        conn.Open();
                        cmdInsertDetail.ExecuteNonQuery();
                        conn.Close();
                    } else
                    {
                        //Món đã có trong bảng nhưng thay đổi số lượng
                        int soLuongCu = Convert.ToInt32(detailCheck);

                        //Xóa món khỏi danh sách nếu chỉnh số lượng món về 0
                        if (soLuong == "0")
                        {
                            string sqlRemoveDetail = @"DELETE FROM OrderDetails
                                                       WHERE OrderID = @OrderID AND MenuItemID = @MenuItemID";
                            SqlCommand cmdRemoveDetail = new SqlCommand (sqlRemoveDetail, conn);
                            cmdRemoveDetail.Parameters.AddWithValue("@OrderID", maDon);
                            cmdRemoveDetail.Parameters.AddWithValue("@MenuItemID", maMon);

                            conn.Open();
                            cmdRemoveDetail.ExecuteNonQuery();
                            conn.Close();
                        } else if (Convert.ToInt32(soLuong) != soLuongCu) //Thay đôi số lượng món
                        {
                            string sqlUpdateDetail = @"UPDATE OrderDetails
                                                       SET Quantity = @Quantity
                                                       WHERE OrderID = @OrderID AND MenuItemID = @MenuItemID";
                            SqlCommand cmdUpdateDetail = new SqlCommand(sqlUpdateDetail, conn);
                            cmdUpdateDetail.Parameters.AddWithValue("@Quantity", soLuong);
                            cmdUpdateDetail.Parameters.AddWithValue("@OrderID", maDon);
                            cmdUpdateDetail.Parameters.AddWithValue("@MenuItemID", maMon);

                            conn.Open();
                            cmdUpdateDetail.ExecuteNonQuery();
                            conn.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally
            {
                using (conn = new SqlConnection(chuoiketnoi))
                {
                    //Tính tổng giá
                    int tongGia = 0;

                    string sqlGetTotalPrice = @"SELECT SUM(QuanTity * UnitPrice) FROM OrderDetails WHERE OrderID = @OrderID";
                    SqlCommand cmdGetTotalPrice = new SqlCommand(sqlGetTotalPrice, conn);
                    cmdGetTotalPrice.Parameters.AddWithValue("@OrderID", maDon);

                    conn.Open();
                    object TotalPrice = cmdGetTotalPrice.ExecuteScalar();
                    conn.Close();

                    if (TotalPrice != null && TotalPrice != DBNull.Value)
                        tongGia = Convert.ToInt32(TotalPrice);

                    //Cập nhật tổng giá vào bảng Orders
                    string sqlUpdateTotalPrice = @"UPDATE Orders SET TotalPrice = @TotalPrice WHERE OrderID = @OrderID";
                    SqlCommand cmdUpdateTotalPrice = new SqlCommand(sqlUpdateTotalPrice, conn);
                    cmdUpdateTotalPrice.Parameters.AddWithValue("@TotalPrice", tongGia);
                    cmdUpdateTotalPrice.Parameters.AddWithValue("@OrderID", maDon);

                    conn.Open();
                    cmdUpdateTotalPrice.ExecuteNonQuery();
                    conn.Close();
                }

                LoadDataGridView(soBan);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btnThanhToan.Hide();
            LoadTableList();
        }

        private void LoadDataGridView(string soBan)
        {
            try
            {
                using (conn = new SqlConnection(chuoiketnoi))
                {
                    //Đổ dữ liệu lên bảng tương ứng với bàn được chọn
                    string sql = @"SELECT mn.ItemName AS 'Món', od.QuanTity AS 'Số lượng', (od.UnitPrice * od.QuanTity) AS 'Giá'
                                   FROM OrderDetails AS od
                                   INNER JOIN Menu AS mn ON mn.MenuItemID = od.MenuItemID
                                   INNER JOIN Orders AS o ON o.OrderID = od.OrderID
                                   WHERE o.TableNumber = @tableNumber AND o.OrderStatus = N'Đang phục vụ';";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@tableNumber", soBan);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();

                    conn.Open();
                    da.Fill(dt);
                    dgvChosen.DataSource = dt;

                    //Tính tổng giá
                    string sqlGetTotalPrice = @"SELECT TotalPrice FROM Orders WHERE TableNumber = @TableNumber AND OrderStatus = N'Đang phục vụ';";
                    SqlCommand cmdGetTotalPrice = new SqlCommand(sqlGetTotalPrice, conn);
                    cmdGetTotalPrice.Parameters.AddWithValue("@TableNumber", soBan);

                    lblTotalPrice.Text = (cmdGetTotalPrice.ExecuteScalar() == null) ? "0đ" : cmdGetTotalPrice.ExecuteScalar().ToString() + "đ";
                    conn.Close();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void LoadTableStatusInUse(string soBan)
        {
            using (conn = new SqlConnection(chuoiketnoi))
            {
                string sqlStatusTable = @"UPDATE Tables SET TableStatus = @TableStatus WHERE TableNumber = @TableNumber";
                SqlCommand cmdStatusTable = new SqlCommand(sqlStatusTable, conn);
                cmdStatusTable.Parameters.AddWithValue("@TableStatus", "Đang dùng");
                cmdStatusTable.Parameters.AddWithValue("@TableNumber", soBan);

                conn.Open();
                cmdStatusTable.ExecuteNonQuery();
                conn.Close();
            }
        }

        private void LoadTableStatusFree(string soBan)
        {
            using (conn = new SqlConnection(chuoiketnoi))
            {
                string sqlStatusTable = @"UPDATE Tables SET TableStatus = @TableStatus WHERE TableNumber = @TableNumber";
                SqlCommand cmdStatusTable = new SqlCommand(sqlStatusTable, conn);
                cmdStatusTable.Parameters.AddWithValue("@TableStatus", "Trống");
                cmdStatusTable.Parameters.AddWithValue("@TableNumber", soBan);

                conn.Open();
                cmdStatusTable.ExecuteNonQuery();
                conn.Close();
            }
        }

        private void LoadTableList()
        {
            try
            {
                using (conn = new SqlConnection(chuoiketnoi))
                {
                    string sqlTable = @"SELECT * FROM Tables";
                    SqlCommand cmdTable = new SqlCommand(sqlTable, conn);

                    conn.Open();
                    SqlDataReader reader = cmdTable.ExecuteReader();            //Đọc dữ liệu từ bảng Tables

                    Dictionary<int, string> listTableStatus = new Dictionary<int, string>();    //Tạo một Dictionary lưu dữ liệu từ bảng

                    while (reader.Read())
                    {
                        int soBan = Convert.ToInt32(reader["TableNumber"]);         //Lấy số bàn
                        string trangThai = reader["TableStatus"].ToString();        //Lấy trạng thái bàn đó
                        listTableStatus.Add(soBan, trangThai);                      //Add vào Dictionary
                    }
                    conn.Close();

                    foreach (Button table in tlpTableList.Controls)
                    {
                        string tenBan = table.Text;
                        int soBan = (tenBan == "Mang về") ? 0 : Convert.ToInt32(tenBan.Substring(tenBan.Length - 2, 2));    //Lấy số bàn

                        if (listTableStatus.ContainsKey(soBan))
                        {
                            string trangThai = listTableStatus[soBan];          //Lấy trạng thái bàn

                            //Đổi màu
                            if (trangThai == "Trống")
                            {
                                table.BackColor = Color.DarkGreen;
                                table.ForeColor = Color.FromArgb(192, 255, 192);
                                table.Enabled = true;
                            }
                            else if (trangThai == "Đang dùng")
                            {
                                table.BackColor = Color.DarkOrange;
                                table.ForeColor = Color.FromArgb(192, 255, 192);
                                table.Enabled = true;
                            }
                            else
                            {
                                table.BackColor = Color.Gray;
                                table.ForeColor = Color.White;
                                table.Enabled = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
    }
}