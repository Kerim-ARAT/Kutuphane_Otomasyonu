using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kütüphane_otamasyonu
{ 
    public partial class form1 : Form
    {
        public form1()
        {
            InitializeComponent();
        }

        

        private void btnUyeEkle_Click(object sender, EventArgs e)
        {
            UyeEkleFrm uye = new UyeEkleFrm();
            uye.ShowDialog();
        }

        private void btnUyeListele_Click(object sender, EventArgs e)
        {
            uyelistelemefrm frm = new uyelistelemefrm();    
            frm.ShowDialog();

        }

        private void btnKitapEkle_Click(object sender, EventArgs e)
        {
            KitapEklefrmcs kitap = new KitapEklefrmcs();    
            kitap.ShowDialog(); 
        }

        private void btnKitapListele_Click(object sender, EventArgs e)
        {
            KitapListelefrm kitapListelefrm = new KitapListelefrm();    
            kitapListelefrm.ShowDialog();

        }

        private void btnEmanetVer_Click(object sender, EventArgs e)
        {
            EmanetKitapVermecs emanet = new EmanetKitapVermecs();
            emanet.Show();

        }

        private void btnEmanetListele_Click(object sender, EventArgs e)
        {
            EmanetKitapListelefrm frm = new EmanetKitapListelefrm();    
            frm.ShowDialog();
        }

        private void btnEmanetİade_Click(object sender, EventArgs e)
        {
            EmanetKitapİaedefrm iade = new EmanetKitapİaedefrm();
            iade.ShowDialog();
        }

        private void btnSiralama_Click(object sender, EventArgs e)
        {
            siralamafrm frm = new siralamafrm();    
            frm.ShowDialog();

        }

        private void btnGirafik_Click(object sender, EventArgs e)
        {
            sonuçlar girafik = new  sonuçlar();
            girafik.ShowDialog();
        }

        private void btnBiz_Click(object sender, EventArgs e)
        {
            bizkimiz biz = new bizkimiz();
            biz.ShowDialog();
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void btncıkıs_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void form1_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
        }
    }
}
