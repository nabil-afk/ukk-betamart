using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BetaMart
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void txtCari_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                if (string.IsNullOrEmpty(txtCari.Text))
                {
                    this.barangTableAdapter.Fill(this.dbBetaMartDataSet.Barang);
                    barangBindingSource.DataSource = this.dbBetaMartDataSet.Barang;
                    //dataGridView.DataSource = barangBindingSource;
                }
                else
                {
                    var query = from o in this.dbBetaMartDataSet.Barang
                                where o.NamaBarang.Contains(txtCari.Text) || o.HargaBarang == txtCari.Text || o.StokBarang == txtCari.Text || o.Keterangan.Contains(txtCari.Text)
                                select o;
                    barangBindingSource.DataSource = query.ToList();
                    //dataGridView.DataSource = query.ToList();
                }
            }
        }

        private void dataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (MessageBox.Show("Apa kamu yakin ingin menghapus data ini?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    barangBindingSource.RemoveCurrent();
                else 
                    barangBindingSource.ResetBindings(false);
            }
        }

        private void btnTelusuri_Click(object sender, EventArgs e)
        {
            try
            {
                using(OpenFileDialog ofd = new OpenFileDialog() { Filter = "JPEG|*.jpg", ValidateNames = true, Multiselect = false})
                {
                    if (ofd.ShowDialog() == DialogResult.OK)
                        pictureBox.Image = Image.FromFile(ofd.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBaru_Click(object sender, EventArgs e)
        {
            try
            {
                panel.Enabled = true;
                txtNamabar.Focus();
                this.dbBetaMartDataSet.Barang.AddBarangRow(this.dbBetaMartDataSet.Barang.NewBarangRow());
                barangBindingSource.MoveLast();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                barangBindingSource.ResetBindings(false);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            panel.Enabled = true;
            txtNamabar.Focus();
        }

        private void btnBatal_Click(object sender, EventArgs e)
        {
            panel.Enabled = false;
            barangBindingSource.ResetBindings(false);
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            try
            {
                barangBindingSource.EndEdit();
                barangTableAdapter.Update(this.dbBetaMartDataSet.Barang);
                panel.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                barangBindingSource.ResetBindings(false);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dbBetaMartDataSet.Barang' table. You can move, or remove it, as needed.
            this.barangTableAdapter.Fill(this.dbBetaMartDataSet.Barang);
            barangBindingSource.DataSource = this.dbBetaMartDataSet.Barang;
        }
    }
}
