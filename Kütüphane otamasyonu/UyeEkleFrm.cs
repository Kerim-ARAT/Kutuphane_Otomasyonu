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
using System.Net.NetworkInformation;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using Npgsql;

namespace Kütüphane_otamasyonu
{
    public partial class UyeEkleFrm : Form
    {
        public UyeEkleFrm()
        {
            InitializeComponent();
        }

        // PostgreSQL bağlantısı (SQL Server yerine)
        private NpgsqlConnection baglanti = new NpgsqlConnection("Host=localhost;Port=5432;Username=postgres;Password=1234;Database=kutuphane;");

        private void UyeEkleFrm_Load(object sender, EventArgs e)
        {

            this.FormBorderStyle = FormBorderStyle.None;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (Pen pen = new Pen(Color.Black, 5)) // 5px kalınlığında siyah kenarlık
            {
                e.Graphics.DrawRectangle(pen, 0, 0, this.ClientSize.Width - 1, this.ClientSize.Height - 1);
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

        private void btnUyeEkle_Click(object sender, EventArgs e)
        {
            DialogResult dialog;
            dialog = MessageBox.Show("Bu kişiyi eklemek istiyor musunuz?", "Kayıt", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dialog == DialogResult.Yes)
            {
                try
                {
                    baglanti.Open();

                    // PostgreSQL için uygun INSERT komutu
                    string query = "INSERT INTO uye (tc, adsoyad, yas, cinsiyet, telefon, adres, email, okukitapsayisi) " +
                                   "VALUES (@tc, @adsoyad, @yas, @cinsiyet, @telefon, @adres, @email, @okukitapsayisi)";

                    using (NpgsqlCommand komut = new NpgsqlCommand(query, baglanti))
                    {
                        komut.Parameters.AddWithValue("@tc", txtTc.Text);
                        komut.Parameters.AddWithValue("@adsoyad", txtAdSoyad.Text);
                        komut.Parameters.AddWithValue("@yas", int.Parse(txtYas.Text));  // PostgreSQL int türüne çeviri
                        komut.Parameters.AddWithValue("@cinsiyet", comboCinsiyet.Text);
                        komut.Parameters.AddWithValue("@telefon", txtTelefon.Text);
                        komut.Parameters.AddWithValue("@adres", txtAdres.Text);
                        komut.Parameters.AddWithValue("@email", txtEmail.Text);
                        komut.Parameters.AddWithValue("@okukitapsayisi", int.Parse(txtOkunanSayi.Text)); // Sayısal tür olduğundan int dönüşümü

                        komut.ExecuteNonQuery();
                    }

                    MessageBox.Show("Kayıt işlemi başarıyla yapıldı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    baglanti.Close();
                }

                // Formu temizleme işlemi
                foreach (Control item in Controls)
                {
                    if (item is TextBox)
                    {
                        if (item != txtOkunanSayi) // Okunan kitap sayısını silme
                        {
                            item.Text = "";
                        }
                    }
                }
            }
        }

        private void txtTc_Enter(object sender, EventArgs e)
        {
           
        }
        
    }
}
