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
using System.Drawing.Printing;
namespace pmquanlybia
{
    public partial class Form1 : Form
    {
        SqlConnection connection;
        SqlCommand command;
        string str = @"Data Source=ASUSROG\SQLEXPRESS;Initial Catalog=QuanLyBiA;Persist Security Info=True;User ID=SA;Password=123456";
        SqlDataAdapter adapter = new SqlDataAdapter();
        DataTable tablebanchoi = new DataTable();
        DataTable tablekhachhang = new DataTable();
        DataTable tabledichvu = new DataTable();
        DataTable tablegiochoi = new DataTable();
        DataTable tablehoadon = new DataTable();
        void hienthi()
        {
            command = connection.CreateCommand();
            command.CommandText = "select * from BanChoi";
            adapter.SelectCommand = command;
            tablebanchoi.Clear();
            adapter.Fill(tablebanchoi);
            dataGridView3.DataSource = tablebanchoi;
        }
        void hienthi1()
        {
            command = connection.CreateCommand();
            command.CommandText = "select * from KhachHang";
            adapter.SelectCommand = command;
            tablekhachhang.Clear();
            adapter.Fill(tablekhachhang);
            dataGridView4.DataSource = tablekhachhang;
        }
        void hienthi2()
        {
            command = connection.CreateCommand();
            command.CommandText = "select * from DichVu";
            adapter.SelectCommand = command;
            tabledichvu.Clear();
            adapter.Fill(tabledichvu);
            dataGridView5.DataSource = tabledichvu;
        }
        void hienthisudungban()
        {
            command = connection.CreateCommand();
            command.CommandText = "SELECT MaBan, MaKhachHang, ThoiGianBat, ThoiGianTat, ChiPhi FROM SuDungBan";
            adapter.SelectCommand = command;
            tablegiochoi.Clear();
            adapter.Fill(tablegiochoi);
            dataGridView6.DataSource = tablegiochoi;

        }
        void hienthihoadon()
        {
            command = connection.CreateCommand();
            command.CommandText = "select * from HoaDon";
            adapter.SelectCommand = command;
            tablehoadon.Clear();
            adapter.Fill(tablehoadon);
            dataGridView7.DataSource = tablehoadon;
        }
        private void inhoadon(string invoiceId)
        {
            string connectionString = @"Data Source=ASUSROG\SQLEXPRESS;Initial Catalog=QuanLyBiA;Persist Security Info=True;User ID=SA;Password=123456";
            string query = "SELECT MaHoaDon, MaKhachHang, NgayLap, TongTien FROM HoaDon WHERE MaHoaDon = @Mahd";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Mahd", invoiceId);

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        string maHoaDon = reader["MaHoaDon"].ToString();
                        string maKhachHang = reader["MaKhachHang"].ToString();
                        string ngayThanhToan = Convert.ToDateTime(reader["NgayLap"]).ToString("dd/MM/yyyy");
                        string tongTien = Convert.ToDecimal(reader["TongTien"]).ToString("N0") + " VND";

                        // Gọi phương thức in hóa đơn
                        PrintInvoice(maHoaDon, maKhachHang, ngayThanhToan, tongTien);
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy hóa đơn!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi kết nối cơ sở dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }//hàm in hóa đơn
        private void PrintInvoice(string maHoaDon, string maKhachHang, string ngayThanhToan, string tongTien)
        {
            PrintDocument printDocument = new PrintDocument();
            printDocument.PrintPage += (sender, e) =>
            {
                float yPos = 10;
                float leftMargin = e.MarginBounds.Left;
                Font printFont = new Font("Arial", 12);

                e.Graphics.DrawString("HÓA ĐƠN ĐIỆN TỬ QUÁN BILLIARDS C# PHẠM VĂN ĐỒNG", new Font("Arial", 16, FontStyle.Bold), Brushes.Black, leftMargin, yPos);
                yPos += 50;
                e.Graphics.DrawString("------------------------------------------------------------------------------------------------- ", printFont, Brushes.Black, leftMargin, yPos);
                yPos += 35;
                e.Graphics.DrawString("Mã hóa đơn: " + maHoaDon, printFont, Brushes.Black, leftMargin, yPos);
                yPos += 50;
                e.Graphics.DrawString("Mã khách hàng: " + maKhachHang, printFont, Brushes.Black, leftMargin, yPos);
                yPos += 50;
                e.Graphics.DrawString("Ngày thanh toán: " + ngayThanhToan, printFont, Brushes.Black, leftMargin, yPos);
                yPos += 50;
                e.Graphics.DrawString("Tổng tiền: " + tongTien, printFont, Brushes.Black, leftMargin, yPos);
                yPos += 50;
                e.Graphics.DrawString("------------------------------------------------------------------------------------------------- ", printFont, Brushes.Black, leftMargin, yPos);
                yPos += 50;
                e.Graphics.DrawString("QUÝ KHÁCH CHUYỂN KHOẢN VÀO STK OR QR BÊN DƯỚI", new Font("Arial", 14), Brushes.Black, leftMargin, yPos);
                yPos += 30;
                e.Graphics.DrawString("Số tài khoản: 1026906112",printFont,Brushes.Black,leftMargin,yPos);
                yPos += 30;
                e.Graphics.DrawString("Tên người nhận: LE DUC TOAN", printFont, Brushes.Black, leftMargin, yPos);
                yPos += 30;
                e.Graphics.DrawString("Ngân hàng: Vietcombank", printFont, Brushes.Black, leftMargin, yPos);
                yPos += 30;
                string imagePath = @"D:\taive\nganhang.png";
                if (System.IO.File.Exists(imagePath))
                {
                    Image image = Image.FromFile(imagePath);
                    e.Graphics.DrawImage(image, leftMargin, yPos, 500, 450); // Điều chỉnh vị trí và kích thước
                }
                yPos += 450;
                e.Graphics.DrawString("------------------------------------------------------------------------------------------------- ", printFont, Brushes.Black, leftMargin, yPos);
                yPos += 100;
                e.Graphics.DrawString("Tổng tiền khách hàng cần thanh toán: " + tongTien, printFont, Brushes.Black, leftMargin, yPos);

            };
            PrintPreviewDialog previewDialog = new PrintPreviewDialog
            {
                Document = printDocument,
                Width = 800,
                Height = 600
            };

            previewDialog.ShowDialog();
        }//design cho in hóa đơn
        private void inhoadondichvu(string invoiceId1) 
        {
            string connectionString = @"Data Source=ASUSROG\SQLEXPRESS;Initial Catalog=QuanLyBiA;Persist Security Info=True;User ID=SA;Password=123456";
            string query = "select * from DichVu where MaDichVu=@madv";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@madv",invoiceId1);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        string maDichVu = reader["MaDichVu"].ToString();
                        string tenDichVu = reader["TenDichVu"].ToString();
                        string giaDichVu = reader["GiaDichVu"].ToString()+" VNĐ";
                        PrintInvoice1(maDichVu, tenDichVu, giaDichVu);
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy hóa đơn dịch vụ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Lỗi kết nối cơ sở dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }//in hóa đơn dịch vụ
        private void PrintInvoice1(string maDichVu, string tenDichVu, string giaDichVu)
        {

            PrintDocument printDocument = new PrintDocument();
            printDocument.PrintPage += (sender, e) =>
            {
                float yPos = 10;
                float leftMargin = e.MarginBounds.Left;
                Font printFont = new Font("Arial", 12);
                e.Graphics.DrawString("HÓA ĐƠN DỊCH VỤ QUÁN BILLIARDS C# PHẠM VĂN ĐỒNG", new Font("Arial", 16, FontStyle.Bold), Brushes.Black, leftMargin, yPos);
                yPos += 50;
                e.Graphics.DrawString("------------------------------------------------------------------------------------------------- ", printFont, Brushes.Black, leftMargin, yPos);
                yPos += 10;
                e.Graphics.DrawString("------------------------------------------------------------------------------------------------- ", printFont, Brushes.Black, leftMargin, yPos);
                yPos += 50;
                e.Graphics.DrawString("Mã dịch vụ: " + maDichVu, printFont, Brushes.Black, leftMargin, yPos);
                yPos += 50;
                e.Graphics.DrawString("Tên dịch vụ: " + tenDichVu, printFont, Brushes.Black, leftMargin, yPos);
                yPos += 50;
                e.Graphics.DrawString("Giá dịch vụ: " + giaDichVu, printFont, Brushes.Black, leftMargin, yPos);
                yPos += 50;
                e.Graphics.DrawString("------------------------------------------------------------------------------------------------- ", printFont, Brushes.Black, leftMargin, yPos);
                yPos += 10;
                e.Graphics.DrawString("------------------------------------------------------------------------------------------------- ", printFont, Brushes.Black, leftMargin, yPos);
                yPos += 50;
                e.Graphics.DrawString("QUÝ KHÁCH CHUYỂN KHOẢN VÀO STK OR QR BÊN DƯỚI", new Font("Arial", 14), Brushes.Black, leftMargin, yPos);
                yPos += 30;
                e.Graphics.DrawString("Số tài khoản: 1026906112", printFont, Brushes.Black, leftMargin, yPos);
                yPos += 30;
                e.Graphics.DrawString("Tên người nhận: LE DUC TOAN", printFont, Brushes.Black, leftMargin, yPos);
                yPos += 30;
                e.Graphics.DrawString("Ngân hàng: Vietcombank", printFont, Brushes.Black, leftMargin, yPos);
                yPos += 30;
                string imagePath = @"D:\taive\nganhang.png";
                if (System.IO.File.Exists(imagePath))
                {
                    Image image = Image.FromFile(imagePath);
                    e.Graphics.DrawImage(image, leftMargin, yPos, 500, 450); // Điều chỉnh vị trí và kích thước
                }
                yPos += 450;
                e.Graphics.DrawString("------------------------------------------------------------------------------------------------- ", printFont, Brushes.Black, leftMargin, yPos);
                yPos += 100;
                e.Graphics.DrawString("Tổng tiền dịch vụ khách hàng cần thanh toán: " + giaDichVu, printFont, Brushes.Black, leftMargin, yPos);
            };
            PrintPreviewDialog previewDialog = new PrintPreviewDialog
            {
                Document = printDocument,
                Width = 800,
                Height = 600
            };

            previewDialog.ShowDialog();
        }//design cho in hóa đơn dịch vụ
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }//no có gì

        private void Form1_Load(object sender, EventArgs e)
        {
            connection = new SqlConnection(str);
            connection.Open();
            hienthi();
            hienthi1();
            hienthi2();
            hienthisudungban();
            hienthihoadon();
            // TODO: This line of code loads data into the 'quanLyBiADataSet.NhaCungCap' table. You can move, or remove it, as needed.
            this.nhaCungCapTableAdapter.Fill(this.quanLyBiADataSet.NhaCungCap);
            // TODO: This line of code loads data into the 'quanLyBiADataSet.LoaiBan' table. You can move, or remove it, as needed.
            this.loaiBanTableAdapter.Fill(this.quanLyBiADataSet.LoaiBan);
            dataGridView1.Visible = false;
            dataGridView2.Visible = false;

        }//hàm này load các gridview

        private void button1_Click(object sender, EventArgs e)
        {
            if (!dataGridView1.Visible)
            {
                dataGridView1.DataSource = quanLyBiADataSet.LoaiBan;
            }
            dataGridView1.Visible = !dataGridView1.Visible;
        }//nút xem thông tin loại bàn chỉnh sửa thông tin ở sql

        private void button2_Click(object sender, EventArgs e)
        {
            if (!dataGridView2.Visible)
            {
                dataGridView2.DataSource = quanLyBiADataSet.NhaCungCap;
            }
            dataGridView2.Visible = !dataGridView2.Visible;
        }//nút xem thông tin nhà cung cấp chỉnh sửa thông tin ở sql

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int i;
            i = dataGridView3.CurrentRow.Index;
            txtmaban.Text = dataGridView3.Rows[i].Cells[0].Value.ToString();
            txtloaiban.Text = dataGridView3.Rows[i].Cells[1].Value.ToString();
            txtgiathue.Text = dataGridView3.Rows[i].Cells[2].Value.ToString();
            txttrangthaibox.Text = dataGridView3.Rows[i].Cells[3].Value.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                command = connection.CreateCommand();
                command.CommandText = "INSERT INTO BanChoi VALUES ('" + txtmaban.Text + "', '" + txtloaiban.Text + "', '" + txtgiathue.Text + "', '" + txttrangthaibox.Text + "')";
                command.ExecuteNonQuery();
                hienthi();
            }
            catch(Exception)
            {
                MessageBox.Show("Nhập mã bàn chơi không được trùng với bàn chơi có sẵn! ","Thông báo",MessageBoxButtons.OK);
            }
        }//thêm bàn chơi

        private void button4_Click(object sender, EventArgs e)
        {
            command.Connection.CreateCommand();
            command.CommandText="delete from BanChoi where MaBan = '"+txtmaban.Text+"'";
            command.ExecuteNonQuery();
            hienthi();
        }//xóa thông tin bàn chơi qua mã bàn

        private void button5_Click(object sender, EventArgs e)
        {
            command.Connection = connection;
            command.CommandText = "DELETE FROM ChiTietHoaDon WHERE MaBan = @MaBan";
            command.Parameters.Clear(); // Xóa tham số trước đó nếu có
            command.Parameters.AddWithValue("@MaBan", txtmaban.Text);
            command.ExecuteNonQuery();
            command.CommandText = "UPDATE BanChoi SET LoaiBan = @LoaiBan, GiaThue = @GiaThue, TrangThai = @TrangThai WHERE MaBan = @MaBan";

            // Thêm tham số cho lệnh UPDATE
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@LoaiBan", txtloaiban.Text);
            command.Parameters.AddWithValue("@GiaThue", txtgiathue.Text);
            command.Parameters.AddWithValue("@TrangThai", txttrangthaibox.Text);
            command.Parameters.AddWithValue("@MaBan", txtmaban.Text);
            command.ExecuteNonQuery();
            hienthi();
        }//cập nhật bàn chơi
        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }//ko có gì

        private void dataGridView4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int i;
            i = dataGridView4.CurrentRow.Index;
            txtmakh.Text = dataGridView4.Rows[i].Cells[0].Value.ToString();
            txttenkh.Text = dataGridView4.Rows[i].Cells[1].Value.ToString();
            txtdiachi.Text = dataGridView4.Rows[i].Cells[2].Value.ToString();
            txtsdt.Text = dataGridView4.Rows[i].Cells[3].Value.ToString();
            txtmail.Text = dataGridView4.Rows[i].Cells[4].Value.ToString();
            txtdiemtichluy.Text = dataGridView4.Rows[i].Cells[5].Value.ToString();

        }

        private void button6_Click(object sender, EventArgs e)
        {
            command = connection.CreateCommand();
            command.CommandText = "delete from KhachHang where MaKhachHang = '"+txtmakh.Text+"'";
            command.ExecuteNonQuery();
            hienthi1();
        }//xóa khách hàng qua mã khách hàng

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                command = connection.CreateCommand();
                command.CommandText = "insert into KhachHang values ('" + txtmakh.Text + "', '" + txttenkh.Text + "', '" + txtdiachi.Text + "', '" + txtsdt.Text + "', '" + txtmail.Text + "', '" + txtdiemtichluy.Text + "')";
                MessageBox.Show("Thêm khách hàng thành công! ");
                command.ExecuteNonQuery();
                hienthi1();
            }
            catch
            {
                MessageBox.Show("Mã khách hàng không được trùng!","Thông báo",MessageBoxButtons.OK);
            }
        }//thêm khách hàng
        private void button8_Click(object sender, EventArgs e)
        {
            command = connection.CreateCommand();
            command.CommandText = "update KhachHang set TenKhachHang = @TenKhachHang,DiaChi=@Diachi,DienThoai=@DienThoai,Email=@Email,DiemTichLuy=@DiemTichLuy where MaKhachHang=@MaKhachHang";
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@TenKhachHang",txttenkh.Text);
            command.Parameters.AddWithValue("@DiaChi", txtdiachi.Text);
            command.Parameters.AddWithValue("@Dienthoai", txtsdt.Text);
            command.Parameters.AddWithValue("@Email", txtmail.Text);
            command.Parameters.AddWithValue("@DiemTichLuy", txtdiemtichluy.Text);
            command.Parameters.AddWithValue("@MaKhachHang", txtmakh.Text);
            MessageBox.Show("Sửa khách hàng thành công! ");
            command.ExecuteNonQuery();
            hienthi1();
        }//sửa thông tin khách hàng
        private void button9_Click(object sender, EventArgs e)
        {
            this.Close();
        }//nút đóng cửa sổ

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                command = connection.CreateCommand();
                command.CommandText = "insert into DichVu values ('" + txtmadv.Text + "','" + txttendv.Text + "','" + txtgiadv.Text + "')";
                MessageBox.Show("Thêm dịch vụ thành công! ");
                command.ExecuteNonQuery();
                hienthi2();
            }
            catch(Exception)
            {
                MessageBox.Show("Nhập trùng mã dịch vụ đã có sẵn,Mời nhập lại!");
            }
        }//thêm dịch vụ

        private void dataGridView5_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int i;
            i = dataGridView5.CurrentRow.Index;
            txtmadv.Text = dataGridView5.Rows[i].Cells[0].Value.ToString();
            txttendv.Text = dataGridView5.Rows[i].Cells[1].Value.ToString();
            txtgiadv.Text = dataGridView5.Rows[i].Cells[2].Value.ToString();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            command.Connection.CreateCommand();
            command.CommandText = "delete from DichVu where MaDichVu = '" + txtmadv.Text + "'";
            command.ExecuteNonQuery();
            hienthi2();
        }//xóa dịch vụ qua mã dịch vụ

        private void button11_Click(object sender, EventArgs e)
        {
            command = connection.CreateCommand();
            command.CommandText = "update DichVu set TenDichVu= @tendichvu,GiaDichVu = @giadichvu where MaDichVu = @madichvu";
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@tendichvu", txttendv.Text);
            command.Parameters.AddWithValue("@giadichvu", txtgiadv.Text);
            command.Parameters.AddWithValue("@madichvu", txtmadv.Text);
            MessageBox.Show("Sửa thành công! ");
            command.ExecuteNonQuery();
            hienthi2();
        }//sửa dịch vụ

        private void button13_Click(object sender, EventArgs e)
        {
            /* string maban = txtnhapmabatban.Text;
             string makhachhang = txtnhapmakh.Text;
             if (string.IsNullOrWhiteSpace(maban) || string.IsNullOrWhiteSpace(makhachhang))
             {
                 MessageBox.Show("Vui lòng nhập đầy đủ mã bàn và mã khách hàng!", "Thông báo");
                 return;
             }
             using (SqlConnection connection = new SqlConnection(@"Data Source=ASUSROG\SQLEXPRESS;Initial Catalog=QuanLyBiA;Persist Security Info=True;User ID=SA;Password=123456"))
             {
                 string query = "INSERT INTO SuDungBan (MaBan, MaKhachHang, ThoiGianBat) VALUES (@MaBan, @MaKhachHang, @ThoiGianBat)";
                 SqlCommand command = new SqlCommand(query, connection);
                 command.Parameters.AddWithValue("@MaBan", maban);
                 command.Parameters.AddWithValue("@MaKhachHang", makhachhang);
                 command.Parameters.AddWithValue("@ThoiGianBat", DateTime.Now);

                 connection.Open();
                 command.ExecuteNonQuery();
                 connection.Close();

                 MessageBox.Show("Bàn đã được bật thành công!", "Thông báo");
                 hienthisudungban();
             }*/
            string maban = txtnhapmabatban.Text;
            string makhachhang = txtnhapmakh.Text;

            if (string.IsNullOrWhiteSpace(maban) || string.IsNullOrWhiteSpace(makhachhang))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ mã bàn và mã khách hàng!", "Thông báo");
                return;
            }

            using (SqlConnection connection = new SqlConnection(@"Data Source=ASUSROG\SQLEXPRESS;Initial Catalog=QuanLyBiA;Persist Security Info=True;User ID=SA;Password=123456"))
            {
                // Kiểm tra trạng thái bàn
                string checkQuery = "SELECT TrangThai FROM BanChoi WHERE MaBan = @MaBan";
                SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
                checkCommand.Parameters.AddWithValue("@MaBan", maban);

                connection.Open();
                var trangthai = checkCommand.ExecuteScalar()?.ToString();
                connection.Close();

                if (trangthai == "Hết")
                {
                    MessageBox.Show("Bàn này đã hết chỗ, vui lòng chọn bàn khác!", "Thông báo");
                    return;
                }

                if (trangthai == "Còn")
                {
                    string existQuery = "SELECT COUNT(*) FROM SuDungBan WHERE MaBan = @MaBan AND ThoiGianTat IS NULL";
                    SqlCommand existCommand = new SqlCommand(existQuery, connection);
                    existCommand.Parameters.AddWithValue("@MaBan", maban);

                    connection.Open();
                    int count = (int)existCommand.ExecuteScalar();
                    connection.Close();

                    if (count > 0)
                    {
                        MessageBox.Show("Bàn này đang được sử dụng. Vui lòng chọn bàn khác!", "Thông báo");
                        return;
                    }
                    string query = "INSERT INTO SuDungBan (MaBan, MaKhachHang, ThoiGianBat) VALUES (@MaBan, @MaKhachHang, @ThoiGianBat)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@MaBan", maban);
                    command.Parameters.AddWithValue("@MaKhachHang", makhachhang);
                    command.Parameters.AddWithValue("@ThoiGianBat", DateTime.Now);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();

                    MessageBox.Show("Bàn đã được bật thành công!", "Thông báo");
                    hienthisudungban();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy bàn hoặc trạng thái không hợp lệ!", "Thông báo");
                }
            }

        }//bật bàn

        private void button14_Click(object sender, EventArgs e)
        {
             string maBan = txtnhapmabatban.Text;

            if (string.IsNullOrWhiteSpace(maBan))
            {
                MessageBox.Show("Vui lòng nhập mã bàn!", "Thông báo");
                return;
            }

            using (SqlConnection connection = new SqlConnection(@"Data Source=ASUSROG\SQLEXPRESS;Initial Catalog=QuanLyBiA;Persist Security Info=True;User ID=SA;Password=123456"))
            {
                connection.Open();

                // Lấy thông tin bật bàn
                string queryGet = "SELECT TOP 1 ThoiGianBat, GiaThue FROM SuDungBan sb INNER JOIN BanChoi bc ON sb.MaBan = bc.MaBan WHERE sb.MaBan = @MaBan AND sb.ThoiGianTat IS NULL";
                SqlCommand commandGet = new SqlCommand(queryGet, connection);
                commandGet.Parameters.AddWithValue("@MaBan", maBan);
                SqlDataReader reader = commandGet.ExecuteReader();

                if (!reader.Read())
                {
                    MessageBox.Show("Không tìm thấy bàn đang bật!", "Thông báo");
                    connection.Close();
                    return;
                }

                DateTime thoiGianBat = reader.GetDateTime(0);
                float giaThue = (float)reader.GetDouble(1);
                reader.Close();

                // Tính thời gian và chi phí
                DateTime thoiGianTat = DateTime.Now;
                TimeSpan thoiGianSuDung = thoiGianTat - thoiGianBat;
                double chiPhi = thoiGianSuDung.TotalHours * giaThue;

                // Cập nhật SuDungBan
                string queryUpdate = "UPDATE SuDungBan SET ThoiGianTat = @ThoiGianTat, ChiPhi = @ChiPhi WHERE MaBan = @MaBan AND ThoiGianTat IS NULL";
                SqlCommand commandUpdate = new SqlCommand(queryUpdate, connection);
                commandUpdate.Parameters.AddWithValue("@ThoiGianTat", thoiGianTat);
                commandUpdate.Parameters.AddWithValue("@ChiPhi", chiPhi);
                commandUpdate.Parameters.AddWithValue("@MaBan", maBan);
                commandUpdate.ExecuteNonQuery();
                MessageBox.Show($"Bàn đã được tắt. Chi phí: {chiPhi:C}", "Thông báo");
            hienthisudungban();
            }
        }//tắt bàn

        private void button15_Click(object sender, EventArgs e)
        {
            string maBan = txtnhapmabatban.Text;
            string maKhachHang = txtnhapmakh.Text;

            if (string.IsNullOrWhiteSpace(maBan) || string.IsNullOrWhiteSpace(maKhachHang))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ mã bàn và mã khách hàng!", "Thông báo");
                return;
            }

            using (SqlConnection connection = new SqlConnection(@"Data Source=ASUSROG\SQLEXPRESS;Initial Catalog=QuanLyBiA;Persist Security Info=True;User ID=SA;Password=123456"))
            {
                connection.Open();

                // Lấy thông tin sử dụng bàn để tính tổng tiền
                string queryGetUsage = "SELECT TOP 1 ThoiGianBat, ChiPhi FROM SuDungBan WHERE MaBan = @MaBan AND ThoiGianTat IS NOT NULL ORDER BY ThoiGianTat DESC";
                SqlCommand commandGetUsage = new SqlCommand(queryGetUsage, connection);
                commandGetUsage.Parameters.AddWithValue("@MaBan", maBan);

                SqlDataReader reader = commandGetUsage.ExecuteReader();

                if (!reader.Read())
                {
                    MessageBox.Show("Không tìm thấy dữ liệu sử dụng bàn đã tắt để lập hóa đơn!", "Thông báo");
                    reader.Close();
                    connection.Close();
                    return;
                }

                DateTime thoiGianBat = reader.GetDateTime(0);
                double tongTien = reader.GetDouble(1);
                reader.Close();
                string queryInsertHoaDon = "INSERT INTO HoaDon (MaHoaDon, MaKhachHang, NgayLap, TongTien, TienDaTT) VALUES (@MaHoaDon, @MaKhachHang, @NgayLap, @TongTien, @TienDaTT)";
                SqlCommand commandInsertHoaDon = new SqlCommand(queryInsertHoaDon, connection);
                commandInsertHoaDon.Parameters.AddWithValue("@MaHoaDon", txtnhapmahd.Text);
                commandInsertHoaDon.Parameters.AddWithValue("@MaKhachHang", maKhachHang);
                commandInsertHoaDon.Parameters.AddWithValue("@NgayLap", DateTime.Now);
                commandInsertHoaDon.Parameters.AddWithValue("@TongTien", tongTien);
                commandInsertHoaDon.Parameters.AddWithValue("@TienDaTT", tongTien);

                commandInsertHoaDon.ExecuteNonQuery();

                MessageBox.Show("Lưu hóa đơn thành công!", "Thông báo");
            }

        }//lưu hóa đơn sau khi dùng bàn

        private void button16_Click(object sender, EventArgs e)
        {
            try
            {
                command = connection.CreateCommand();
                command.CommandText = "delete from SuDungBan where MaBan ='" + txtnhapmabatban.Text + "' AND MaKhachHang='" + txtnhapmakh.Text + "'";
                command.ExecuteNonQuery();
                hienthisudungban();
            }
            catch(Exception)
            {
                MessageBox.Show("Mã bàn hoặc mã khách hàng phải được điền đầy đủ","Thông báo",MessageBoxButtons.OK);
            }
        }//xóa dữ liệu ở tính tiền bàn

        private void dataGridView6_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int i;
            i = dataGridView6.CurrentRow.Index;
            txtnhapmabatban.Text = dataGridView6.Rows[i].Cells[0].Value.ToString();
            txtnhapmakh.Text = dataGridView6.Rows[i].Cells[1].Value.ToString();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            string maHoaDon = txttracuuhd.Text.Trim();

            if (string.IsNullOrWhiteSpace(maHoaDon))
            {
                MessageBox.Show("Vui lòng nhập mã hóa đơn!", "Thông báo");
                return;
            }

            using (SqlConnection connection = new SqlConnection(@"Data Source=ASUSROG\SQLEXPRESS;Initial Catalog=QuanLyBiA;Persist Security Info=True;User ID=SA;Password=123456"))
            {
                connection.Open();

                string query = "SELECT hd.MaHoaDon, hd.NgayLap, hd.TongTien, kh.TenKhachHang " +
                               "FROM HoaDon hd " +
                               "LEFT JOIN KhachHang kh ON hd.MaKhachHang = kh.MaKhachHang " +
                               "WHERE hd.MaHoaDon = @MaHoaDon";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@MaHoaDon", maHoaDon);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    string ngayLap = reader["NgayLap"].ToString();
                    string tongTien = reader["TongTien"].ToString();
                    string tenKhachHang = reader["TenKhachHang"].ToString();
                    hienthihoadon();
                    MessageBox.Show($"Hóa đơn: {maHoaDon}\nKhách hàng: {tenKhachHang}\nNgày lập: {ngayLap}\nTổng tiền: {tongTien} VND",
                                    "Thông tin hóa đơn");
                }
                else
                {
                    MessageBox.Show("Không tìm thấy hóa đơn với mã vừa nhập!", "Thông báo");
                }

                reader.Close();
            }
        }//tra cứu hóa đơn

        private void button18_Click(object sender, EventArgs e)
        {
            command = connection.CreateCommand();
            command.CommandText = "delete from HoaDon where MaHoaDon = '"+txttracuuhd.Text+"'";
            command.ExecuteNonQuery();
            hienthihoadon();
        }//xóa hóa đơn ở tra cứu

        private void button19_Click(object sender, EventArgs e)
        {
            string timban = txttimban.Text.Trim();
            if (string.IsNullOrWhiteSpace(timban))
            {
                MessageBox.Show("Nhập cái mã bàn vào mới tìm được chứ!","Thông báo");
                return;
            }
            using(SqlConnection connection =new SqlConnection(@"Data Source=ASUSROG\SQLEXPRESS;Initial Catalog=QuanLyBiA;Persist Security Info=True;User ID=SA;Password=123456"))
            {
                connection.Open();
                string query = "select MaBan,LoaiBan,GiaThue,TrangThai from BanChoi where MaBan ='"+txttimban.Text+"'";
                SqlCommand command = new SqlCommand(query,connection);
                command.Parameters.AddWithValue("MaBan", timban);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    string lb = reader["LoaiBan"].ToString();
                    string gt = reader["GiaThue"].ToString();
                    string tt = reader["TrangThai"].ToString();
                    MessageBox.Show($"Thông tin bàn tìm được như sau: \nMã bàn: {timban}\nLoại Bàn: {lb}\nGiá Thuê: {gt}\nTrạng thái: {tt}","Kết quả");
                }
                else
                {
                    MessageBox.Show("Khôngg tìm được thông tin bàn vì không có thông tin mã bàn để truy vấn!", "Thông báo");
                }
                reader.Close();
            }
        }//tìm bàn

        private void button20_Click(object sender, EventArgs e)
        {
            string timdichvu = txttimdichvu.Text.Trim();
            if (string.IsNullOrWhiteSpace(timdichvu))
            {
                MessageBox.Show("Nhập cái mã dịch vụ vào mới tìm được chứ!", "Thông báo");
                return;
            }
            using (SqlConnection connection = new SqlConnection(@"Data Source=ASUSROG\SQLEXPRESS;Initial Catalog=QuanLyBiA;Persist Security Info=True;User ID=SA;Password=123456"))
            {
                connection.Open();
                string query = "select MaDichVu,TenDichVu,GiaDichVu from DichVu where MaDichVu = '"+txttimdichvu.Text+"'";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("MaDichVu",timdichvu);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    string tendv = reader["TenDichVu"].ToString();
                    string giadv = reader["GiaDichVu"].ToString();
                    MessageBox.Show($"Thông tin dịch vụ tìm được như sau: \n Mã dịch vụ: {timdichvu}\nTên dịch vụ: {tendv}\n Giá dịch vụ: {giadv}","Kết quả");
                }
                else
                {
                    MessageBox.Show("Không tìm được thông tin dịch vụ!", "Thông báo");
                }
                reader.Close();
            }
        }//tìm dịch vụ

        private void label25_Click(object sender, EventArgs e)
        {

        }//ko có gì

        private void dataGridView7_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }//ko có gì

        private void button21_Click(object sender, EventArgs e)
        {
            hienthihoadon();
        }//reset hóa đơn
        private void button23_Click(object sender, EventArgs e)
        {
            string invoiceId = txtinhoadon.Text.Trim();

            if (string.IsNullOrEmpty(invoiceId))
            {
                MessageBox.Show("Vui lòng nhập mã hóa đơn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            inhoadon(invoiceId);
        }//in hóa đơn

        private void button22_Click(object sender, EventArgs e)
        {
            string invoiceId1 = txtinhddichvu.Text.Trim();
            if (string.IsNullOrEmpty(invoiceId1))
            {
                MessageBox.Show("Vui lòng nhập mã dịch vụ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            inhoadondichvu(invoiceId1);
        }//in hóa đơn dịch vụ

        private void button24_Click(object sender, EventArgs e)
        {
            string timkhach = txttimttkhach.Text.Trim();
            if (string.IsNullOrWhiteSpace(timkhach))
            {
                MessageBox.Show("Nhập mã khách hàng!","Thông báo");
                return;
            }
            using (SqlConnection connection = new SqlConnection(@"Data Source=ASUSROG\SQLEXPRESS;Initial Catalog=QuanLyBiA;Persist Security Info=True;User ID=SA;Password=123456"))
            {
                connection.Open();
                string query = "select * from KhachHang where MaKhachHang ='"+txttimttkhach.Text+"'";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("MaKhachHang", timkhach);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    string tenkh = reader["TenKhachHang"].ToString();
                    string diachi = reader["DiaChi"].ToString();
                    string dienthoai = reader["DienThoai"].ToString();
                    string email = reader["Email"].ToString();
                    string diemtichluy = reader["DiemTichLuy"].ToString();
                    MessageBox.Show($"Thông tin khách hàng tìm được như sau: \n Mã khách hàng: {timkhach}\nTên khách hàng: {tenkh}\n Địa chỉ: {diachi}\nĐiện thoại: {dienthoai}\nEmail khách: {email}\nĐiểm tích lũy thành viên: {diemtichluy}", "Kết quả");
                }
                else
                {
                    MessageBox.Show("Không tìm được thông tin khách hàng!","Thông báo");
                }
                reader.Close();
            }
        }//tìm khách hàng
    }
}
