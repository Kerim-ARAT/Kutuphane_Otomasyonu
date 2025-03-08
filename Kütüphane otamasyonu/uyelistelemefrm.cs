using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Npgsql;
using System.Drawing.Drawing2D;

namespace Kütüphane_otamasyonu
{
    public partial class uyelistelemefrm : Form
    {
        public uyelistelemefrm()
        {
            InitializeComponent();
        }

        private NpgsqlConnection baglanti = new NpgsqlConnection("Host=localhost;Port=5432;Username=postgres;Password=1234;Database=kutuphane;");

        // Sayfalama global değişkenleri
        private int currentPage = 1;       // Geçerli sayfa numarası
        private int pageSize = 11;         // Bir sayfada gösterilecek satır sayısı
        private int totalRecords = 0;      // Toplam kayıt sayısı
        private int totalPages = 0;        // Toplam sayfa sayısı
        private DataTable currentPageData; // Geçerli sayfanın verileri
        private DataTable allData;         // Tüm veriler

        // Sayfalama kontrolleri için panel (aşağıdaki dinamik oluşturulacak)
        private Panel paginationPanel;

        // Veritabanından veriyi çekip, sayfalama hesaplamalarını yapar
        private void uyelistele()
        {
            baglanti.Open();
            NpgsqlDataAdapter adtr = new NpgsqlDataAdapter("SELECT * FROM uye", baglanti);
            allData = new DataTable();
            adtr.Fill(allData);
            baglanti.Close();

            totalRecords = allData.Rows.Count;
            totalPages = (totalRecords + pageSize - 1) / pageSize; // Yuvarlama yöntemi

            int startRow = (currentPage - 1) * pageSize;

            // Eğer veri yoksa veya son sayfada satır sayısı pageSize'dan az ise kontrol
            if (allData.Rows.Count > 0 && startRow < allData.Rows.Count)
            {
                // Eğer kopyalanacak satır yoksa, CopyToDataTable hata verebilir. Bu yüzden kontrol ediyoruz.
                var pageRows = allData.AsEnumerable().Skip(startRow).Take(pageSize);
                if (pageRows.Any())
                {
                    currentPageData = pageRows.CopyToDataTable();
                    dataGridView1.DataSource = currentPageData;
                }
            }
            else
            {
                dataGridView1.DataSource = null;
            }
        }

        // DataGridView hücrelerini çizerek kenarlık ekler
        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // Başlıklar için varsayılan çizime dokunmayalım
            if (e.RowIndex < 0)
                return;

            e.Handled = true; // Kendi çizimimizi yapacağız
            Rectangle cellBounds = e.CellBounds;

            // Hücre kenarlıklarını çiz
            using (Pen borderPen = new Pen(Color.FromArgb(70, 70, 70), 1))
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.DrawRectangle(borderPen, cellBounds);
            }

            // Hücre içeriğini çiz
            e.PaintContent(cellBounds);
        }

        // DataGridView stil ve sayfalama butonlarının ayarlandığı metot
        private void StyleDataGridView()
        {
            // Form'un üstündeki standart butonlar
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.ControlBox = true;
            this.MaximizeBox = true;
            this.MinimizeBox = true;

            // Form'un border'larını ve butonları geri getirme
            this.FormBorderStyle = FormBorderStyle.Sizable;  // Büyütme ve küçültme için FormBorderStyle ayarı
            this.ControlBox = true;  // Kapatma, küçültme ve büyütme butonları görünsün
            this.MaximizeBox = true;  // Büyütme butonu ekle
            this.MinimizeBox = true;  // Küçültme butonu ekle

          

            // DataGridView için temel ayarlar
            dataGridView1.RowHeadersVisible = false;  // Yan seçim butonlarını gizle
            dataGridView1.ReadOnly = true;             // Düzenlemeyi kapat
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToResizeRows = false; // Satır boyutunu değiştirmeyi kaldırdık
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.EnableHeadersVisualStyles = false;

            // DataGridView başlık stil ayarları
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 170, 158); // Yeşil-mavi
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Hücre stil ayarları
            dataGridView1.DefaultCellStyle.BackColor = Color.FromArgb(234, 234, 234);  // Açık gri
            dataGridView1.DefaultCellStyle.ForeColor = Color.Black;  // Yazı rengi siyah
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.FromArgb(102, 210, 206); // Açık su yeşili
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.Black;  // Seçili satır yazısı siyah

            // Alternatif satır rengi
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(227, 210, 195); // Açık bej

            // DataGridView border'ları
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.Single;  // Hücre kenarlıkları
            dataGridView1.GridColor = Color.FromArgb(70, 70, 70);  // Satır çizgisi rengi

            // DataGridView başlık kenarlıkları
            dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

            // Başlık ve hücre arasındaki ince çizgi
            dataGridView1.BorderStyle = BorderStyle.None;  // Veri gridinin kendi kenarlarını kaldırıyoruz
            
            CreatePaginationControls();
        }

        // Sayfalama kontrollerini oluşturur
        private void CreatePaginationControls()
        {
            // Eğer daha önce oluşturulmamışsa, yeni panel oluştur
            if (paginationPanel == null)
            {
                paginationPanel = new Panel();
                // DataGridView'in hemen altında konumlandırıyoruz:
                paginationPanel.Location = new Point(dataGridView1.Left, dataGridView1.Bottom + 10);
                paginationPanel.Size = new Size(dataGridView1.Width, 50);
                this.Controls.Add(paginationPanel);
            }

            // Panel içeriğini temizle
            paginationPanel.Controls.Clear();

            int buttonWidth = 60;
            int spacing = 5;
            int x = 0;

            // Önceki butonu ("<")
            Button btnPrev = new Button();
            btnPrev.Text = "<";
            btnPrev.Width = buttonWidth;
            btnPrev.Height = 40;
            btnPrev.Location = new Point(x, 5);
            btnPrev.BackColor = Color.FromArgb(102, 210, 206); // Açık su yeşili
            btnPrev.ForeColor = Color.Black;
            btnPrev.FlatStyle = FlatStyle.Flat;
            btnPrev.FlatAppearance.MouseOverBackColor = Color.FromArgb(234, 234, 234); // Ten rengi
            btnPrev.Click += BtnPrev_Click;
            paginationPanel.Controls.Add(btnPrev);
            x += buttonWidth + spacing;

            // Sayfa numarası butonları
            for (int i = 1; i <= totalPages; i++)
            {
                Button btnPage = new Button();
                btnPage.Text = i.ToString();
                btnPage.Width = buttonWidth;
                btnPage.Height = 50;
                btnPage.Location = new Point(x, 5);
                btnPage.BackColor = Color.FromArgb(227, 210, 195);  // Açık bej tonu
                btnPage.ForeColor = Color.Black;
                btnPage.FlatStyle = FlatStyle.Flat;
                btnPage.FlatAppearance.MouseOverBackColor = Color.FromArgb(234, 234, 234); // Ten rengi

                // Geçerli sayfayı vurgulamak için renk değiştiriyoruz
                if (i == currentPage)
                {
                    btnPage.BackColor = Color.FromArgb(234, 234, 234); // Mor ton
                    btnPage.ForeColor = Color.White;
                }

                btnPage.Click += BtnPage_Click;
                paginationPanel.Controls.Add(btnPage);
                x += buttonWidth + spacing;
            }

            // Sonraki butonu (">")
            Button btnNext = new Button();
            btnNext.Text = ">";
            btnNext.Width = buttonWidth;
            btnNext.Height = 40;
            btnNext.Location = new Point(x, 5);
            btnNext.BackColor = Color.FromArgb(102, 210, 206); // Açık su yeşili
            btnNext.ForeColor = Color.Black;
            btnNext.FlatStyle = FlatStyle.Flat;
            btnNext.FlatAppearance.MouseOverBackColor = Color.FromArgb(234, 234, 234); // Ten rengi
            btnNext.Click += BtnNext_Click;
            paginationPanel.Controls.Add(btnNext);

        }

        // Sayfa geçişi olayları
        private void BtnPrev_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                uyelistele();
                CreatePaginationControls();
            }
        }

        private void BtnNext_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                uyelistele();
                CreatePaginationControls();
            }
        }

        private void BtnPage_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            currentPage = int.Parse(btn.Text);
            uyelistele();
            CreatePaginationControls();
        }

        // Formun Paint metodu: Formun dışına siyah border ekler
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (Pen pen = new Pen(Color.Black, 5))
            {
                e.Graphics.DrawRectangle(pen, 0, 0, this.ClientSize.Width - 1, this.ClientSize.Height - 1);
            }
        }

        private void uyelistelemefrm_Load(object sender, EventArgs e)
        {
            // Ekranın çalışma alanı genişliği kadar yapıyoruz
            this.Width = Screen.PrimaryScreen.WorkingArea.Width;
            StyleDataGridView();
            uyelistele();
        }

        // Aşağıdaki kısım diğer işlemler (güncelleme, silme, arama vs.) için mevcut
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            txtTc.Text = dataGridView1.CurrentRow.Cells["tc"].Value.ToString();
        }

        private void txtTc_TextChanged(object sender, EventArgs e)
        {
            baglanti.Open();
            NpgsqlCommand komut = new NpgsqlCommand("SELECT * FROM uye WHERE tc = @tc", baglanti);
            komut.Parameters.AddWithValue("@tc", txtTc.Text);
            NpgsqlDataReader reader = komut.ExecuteReader();
            while (reader.Read())
            {
                txtAdSoyad.Text = reader["adsoyad"].ToString();
                txtYas.Text = reader["yas"].ToString();
                comboCinsiyet.Text = reader["cinsiyet"].ToString();
                txtTelefon.Text = reader["telefon"].ToString();
                txtAdres.Text = reader["adres"].ToString();
                txtEmail.Text = reader["email"].ToString();
                txtOkunanSayi.Text = reader["okukitapsayisi"].ToString();
            }
            baglanti.Close();
        }

        DataSet daset = new DataSet();

        private void txtTcAra_TextChanged(object sender, EventArgs e)
        {
            string alinen_deger = comboBox1.Text;
            if (alinen_deger == "T.C ile Arama")
            {
                daset.Tables["uye"].Clear();
                baglanti.Open();
                NpgsqlDataAdapter adtr = new NpgsqlDataAdapter("SELECT * FROM uye WHERE tc LIKE @tc", baglanti);
                adtr.SelectCommand.Parameters.AddWithValue("@tc", "%" + txtTcAra.Text + "%");
                adtr.Fill(daset, "uye");
                dataGridView1.DataSource = daset.Tables["uye"];
                baglanti.Close();
            }
            else if (alinen_deger == "İsim ile arama")
            {
                daset.Tables["uye"].Clear();
                baglanti.Open();
                NpgsqlDataAdapter adtr = new NpgsqlDataAdapter("SELECT * FROM uye WHERE adsoyad LIKE @adsoyad", baglanti);
                adtr.SelectCommand.Parameters.AddWithValue("@adsoyad", "%" + txtTcAra.Text + "%");
                adtr.Fill(daset, "uye");
                dataGridView1.DataSource = daset.Tables["uye"];
                baglanti.Close();
            }
            else if (alinen_deger == "Yas")
            {
                daset.Tables["uye"].Clear();
                baglanti.Open();
                NpgsqlDataAdapter adtr = new NpgsqlDataAdapter("SELECT * FROM uye WHERE yas LIKE @yas", baglanti);
                adtr.SelectCommand.Parameters.AddWithValue("@yas", "%" + txtTcAra.Text + "%");
                adtr.Fill(daset, "uye");
                dataGridView1.DataSource = daset.Tables["uye"];
                baglanti.Close();
            }
            else if (alinen_deger == "Cinsiyet")
            {
                daset.Tables["uye"].Clear();
                baglanti.Open();
                NpgsqlDataAdapter adtr = new NpgsqlDataAdapter("SELECT * FROM uye WHERE cinsiyet LIKE @cinsiyet", baglanti);
                adtr.SelectCommand.Parameters.AddWithValue("@cinsiyet", "%" + txtTcAra.Text + "%");
                adtr.Fill(daset, "uye");
                dataGridView1.DataSource = daset.Tables["uye"];
                baglanti.Close();
            }
            else if (alinen_deger == "Adres")
            {
                daset.Tables["uye"].Clear();
                baglanti.Open();
                NpgsqlDataAdapter adtr = new NpgsqlDataAdapter("SELECT * FROM uye WHERE adres LIKE @adres", baglanti);
                adtr.SelectCommand.Parameters.AddWithValue("@adres", "%" + txtTcAra.Text + "%");
                adtr.Fill(daset, "uye");
                dataGridView1.DataSource = daset.Tables["uye"];
                baglanti.Close();
            }
            else if (alinen_deger == "Telefon")
            {
                daset.Tables["uye"].Clear();
                baglanti.Open();
                NpgsqlDataAdapter adtr = new NpgsqlDataAdapter("SELECT * FROM uye WHERE telefon LIKE @telefon", baglanti);
                adtr.SelectCommand.Parameters.AddWithValue("@telefon", "%" + txtTcAra.Text + "%");
                adtr.Fill(daset, "uye");
                dataGridView1.DataSource = daset.Tables["uye"];
                baglanti.Close();
            }
            else if (alinen_deger == "E-mail")
            {
                daset.Tables["uye"].Clear();
                baglanti.Open();
                NpgsqlDataAdapter adtr = new NpgsqlDataAdapter("SELECT * FROM uye WHERE email LIKE @email", baglanti);
                adtr.SelectCommand.Parameters.AddWithValue("@email", "%" + txtTcAra.Text + "%");
                adtr.Fill(daset, "uye");
                dataGridView1.DataSource = daset.Tables["uye"];
                baglanti.Close();
            }
            else if (alinen_deger == "Okunan Kitap")
            {
                daset.Tables["uye"].Clear();
                baglanti.Open();
                NpgsqlDataAdapter adtr = new NpgsqlDataAdapter("SELECT * FROM uye WHERE okukitapsayisi LIKE @okukitapsayisi", baglanti);
                adtr.SelectCommand.Parameters.AddWithValue("@okukitapsayisi", "%" + txtTcAra.Text + "%");
                adtr.Fill(daset, "uye");
                dataGridView1.DataSource = daset.Tables["uye"];
                baglanti.Close();
            }
        }

        private void btnİptal_Click(object sender, EventArgs e)
        {
            DialogResult dialog;
            dialog = MessageBox.Show("Bu sayfayı kapatmak istiyor musunuz?", "Kapat", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialog == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            DialogResult dialog;
            dialog = MessageBox.Show("Bu kaydı silmek istiyor musunuz?", "Sil", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialog == DialogResult.Yes)
            {
                baglanti.Open();
                NpgsqlCommand komut = new NpgsqlCommand("DELETE FROM uye WHERE tc = @tc", baglanti);
                komut.Parameters.AddWithValue("@tc", dataGridView1.CurrentRow.Cells["tc"].Value.ToString());
                komut.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show("Silme işlemi gerçekleşti");
                daset.Tables["uye"].Clear();
                uyelistele();
            }
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            DialogResult dialog;
            dialog = MessageBox.Show("Bu kaydı güncellemek istediğinize emin misiniz?", "Güncelle", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialog == DialogResult.Yes)
            {
                baglanti.Open();
                NpgsqlCommand komut = new NpgsqlCommand("UPDATE uye SET adsoyad = @adsoyad, yas = @yas, cinsiyet = @cinsiyet, telefon = @telefon, adres = @adres, email = @email, okukitapsayisi = @okukitapsayisi WHERE tc = @tc", baglanti);
                komut.Parameters.AddWithValue("@tc", txtTc.Text);
                komut.Parameters.AddWithValue("@adsoyad", txtAdSoyad.Text);
                komut.Parameters.AddWithValue("@yas", int.Parse(txtYas.Text));
                komut.Parameters.AddWithValue("@cinsiyet", comboCinsiyet.Text);
                komut.Parameters.AddWithValue("@telefon", txtTelefon.Text);
                komut.Parameters.AddWithValue("@adres", txtAdres.Text);
                komut.Parameters.AddWithValue("@email", txtEmail.Text);
                komut.Parameters.AddWithValue("@okukitapsayisi", int.Parse(txtOkunanSayi.Text));
                komut.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show("Güncelleme işlemi gerçekleşti");
                daset.Tables["uye"].Clear();
                uyelistele();
            }
        }

        private void x(object sender, DataGridViewCellEventArgs e)
        {
            // Boş bırakabilirsiniz.
        }
    }
}
