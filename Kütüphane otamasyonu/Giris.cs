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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Npgsql;

namespace Kütüphane_otamasyonu
{
    public partial class Giris : Form
    {
        public Giris()
        {
            InitializeComponent();
        }
        private bool isThere;
        private string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=1234;Database=kutuphane;";

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtKulAd.Text;
            string pass = txtŞifre.Text;

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    // Veritabanına bağlan
                    connection.Open();

                    // login tablosuna göre sorgu yazılıyor
                    string query = "SELECT COUNT(*) FROM login WHERE username = @username AND pass = @pass";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, connection))
                    {
                        // Parametreleri ekliyoruz
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@pass", pass);

                        // Kullanıcı var mı kontrol ediyoruz
                        int userCount = Convert.ToInt32(cmd.ExecuteScalar());
                        isThere = userCount > 0;
                    }
                }
                catch (Exception ex)
                {
                    // Bağlantı hatası durumunda kullanıcıya mesaj göster
                    MessageBox.Show("Bağlantı hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            // Eğer kullanıcı bulunduysa, anasayfa formunu göster
            if (isThere)
            {
                form1 anasayfa = new form1();
                anasayfa.Show();
                this.Hide();
            }
            else
            {
                // Kullanıcı adı veya şifre hatalıysa, hata mesajı göster
                MessageBox.Show("Giriş yapılamadı! Kullanıcı adı veya şifre hatalı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // Mesaj içeriği ve başlık belirleyin


     
        

       



        private void Giris_Load(object sender, EventArgs e)
        {

        }
    }
 }

