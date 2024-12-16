using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using De_01.Model;
using System.Data.Entity;

namespace De_01
{
    public partial class Form1 : Form
    {
        private Model1 _context;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _context = new Model1();
            var lops = _context.Lops.ToList();
            cbLop.DataSource = lops;
            cbLop.DisplayMember = "TenLop";
            cbLop.ValueMember = "MaLop";
            LoadListView();

        }
        private void LoadListView()
        {
            lvSinhvien.Items.Clear();
            var Sinhviens = _context.Sinhviens.Include(s => s.Lop).ToList();

            foreach (var sv in Sinhviens)
            {
                var item = new ListViewItem(sv.MaSV);
                item.SubItems.Add(sv.HoTenSV);
                item.SubItems.Add(sv.NgaySinh.ToString("dd/MM/yyyy"));

                // Kiểm tra null trước khi truy cập sv.Lop.TenLop
                item.SubItems.Add(sv.Lop != null ? sv.Lop.TenLop : "");

                lvSinhvien.Items.Add(item);
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            var sv = new Sinhvien
            {
                MaSV = txtMaSV.Text,
                HoTenSV = txtHoten.Text,
                NgaySinh = dtNgaySinh.Value,
                MaLop = cbLop.SelectedValue.ToString()
            };

            _context.Sinhviens.Add(sv);
            _context.SaveChanges();

            MessageBox.Show("Thêm sinh viên thành công!");
            LoadListView();

        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                var maSV = txtMaSV.Text;
                var sinhvien = _context.Sinhviens.FirstOrDefault(s => s.MaSV == maSV);

                if (sinhvien == null)
                {
                    MessageBox.Show("Không tìm thấy sinh viên cần sửa!");
                    return;
                }

                sinhvien.HoTenSV = txtHoten.Text; // Sửa lại tên textbox
                sinhvien.NgaySinh = dtNgaySinh.Value;
                sinhvien.MaLop = cbLop.SelectedValue.ToString();

                _context.SaveChanges();

                MessageBox.Show("Cập nhật thông tin sinh viên thành công!");
                LoadListView();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }

        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                var maSV = txtMaSV.Text;
                var sv = _context.Sinhviens.FirstOrDefault(s => s.MaSV == maSV);

                if (sv == null)
                {
                    MessageBox.Show("Không tìm thấy sinh viên!");
                    return;
                }

                var confirm = MessageBox.Show("Bạn có chắc chắn muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                {
                    _context.Sinhviens.Remove(sv);
                    _context.SaveChanges();

                    MessageBox.Show("Xóa sinh viên thành công!");
                    LoadListView();
                    ClearForm(); // Xóa dữ liệu trên Form
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }
            private void ClearForm()
        {
            txtMaSV.Clear();
            txtHoten.Clear();
            dtNgaySinh.Value = DateTime.Now;
            cbLop.SelectedIndex = -1; // Bỏ chọn ComboBox
        }

        

        
        

        private void lvSinhvien_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvSinhvien.SelectedItems.Count > 0)
            {
                var selectedItem = lvSinhvien.SelectedItems[0];
                txtMaSV.Text = selectedItem.SubItems[0].Text; // MaSV
                txtHoten.Text = selectedItem.SubItems[1].Text; // HotenSV
                dtNgaySinh.Value = DateTime.ParseExact(selectedItem.SubItems[2].Text, "dd/MM/yyyy", null); // NgaySinh
                cbLop.Text = selectedItem.SubItems[3].Text; // TenLop
            }
        }

        private void btnTimKiem_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                btnTimKiem.PerformClick();
            }

        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            var keyword = txtTimKiem.Text.Trim();

            if (string.IsNullOrWhiteSpace(keyword))
            {
                LoadListView();
                return;
            }

            var Sinhviens = _context.Sinhviens
                .Include(s => s.Lop)
                .Where(s => s.HoTenSV.Contains(keyword))
                .ToList();

            lvSinhvien.Items.Clear();
            if (Sinhviens.Count == 0)
            {
                MessageBox.Show("Không tìm thấy sinh viên nào có tên phù hợp!");
                return;
            }

            foreach (var sv in Sinhviens)
            {
                var item = new ListViewItem(sv.MaSV);
                item.SubItems.Add(sv.HoTenSV);
                item.SubItems.Add(sv.NgaySinh.ToString("dd/MM/yyyy hh:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture));
                item.SubItems.Add(sv.Lop.TenLop);
                lvSinhvien.Items.Add(item);
            }
        }

        private void BtnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
  }

