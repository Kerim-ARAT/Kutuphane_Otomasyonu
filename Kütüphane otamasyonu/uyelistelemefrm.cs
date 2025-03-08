using System;
using System.Data;
using System.Windows.Forms;
using Npgsql;

namespace Kütüphane_otamasyonu
{
    public partial class uyelistelemefrm : Form
    {
        public uyelistelemefrm()
        {
            InitializeComponent();
        }

        private NpgsqlConnection baglanti = new NpgsqlConnection("Host=localhost;Port=5432;Username=postgres;Password=1234;Database=kutuphane;");

        private void uyelistele()
        {
            baglanti.Open();
            NpgsqlDataAdapter adtr = new NpgsqlDataAdapter("SELECT * FROM uye", baglanti);
            adtr.Fill(daset, "uye");
            dataGridView1.DataSource = daset.Tables["uye"];
            baglanti.Close();
        }

        private void uyelistelemefrm_Load(object sender, EventArgs e)
        {
            uyelistele();
        }


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
                komut.Parameters.AddWithValue("@yas", txtYas.Text);
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

        }
    }
}
