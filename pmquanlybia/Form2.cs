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
namespace pmquanlybia
{
    public partial class Form2 : Form
    {
        public List<TaiKhoan> DanhSachTaiKhoan { get; set; }//gọi list tài khoản dùng đăng nhập
        public Form2()
        {
            InitializeComponent();
            DanhSachTaiKhoan = new List<TaiKhoan>
            {
                new TaiKhoan("admin", "123456"),
                new TaiKhoan("user1", "password1"),
                new TaiKhoan("user2", "password2")
            };
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        public class TaiKhoan
        {
            public string TenDangNhap { get; set; }
            public string MatKhau { get; set; }
            public TaiKhoan(string user,string pass)
            {
                TenDangNhap = user;
                MatKhau = pass;
            }
        }//khởi tạo tài khoản mật khẩu cho app
        private void button1_Click(object sender, EventArgs e)
        {
            string tenDangNhap = txttaikhoan.Text;
            string matKhau = txtmatkhau.Text;

            bool isValidLogin = false;
            foreach (var tk in DanhSachTaiKhoan)
            {
                if (tk.TenDangNhap == tenDangNhap && tk.MatKhau == matKhau)
                {
                    isValidLogin = true;
                    break;
                }
            }

            if (isValidLogin)
            {
                MessageBox.Show("Đăng nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Form1 f = new Form1();
                f.ShowDialog();
            }
            else
            {
                MessageBox.Show("Sai tài khoản hoặc mật khẩu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }//hàm kiểm tra thông tin đăng nhập

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                txtmatkhau.UseSystemPasswordChar = false; 
            }
            else
            {
                txtmatkhau.UseSystemPasswordChar = true; 
            }
        }
    }
}

