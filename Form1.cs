using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ProgramInputNilaiSiswa
{
    public partial class FormInputNilai : Form
    {
        List<Siswa> listSiswa = new List<Siswa>();
        public int siswaID = 0;

        public FormInputNilai()
        {
            InitializeComponent();
        }

        private void FormInputNilai_Load(object sender, EventArgs e)
        {
            LoadData();
            RefreshDataGrid();
        }

        private void SaveData()
        {
            if (File.Exists("data.csv"))
                File.Delete("data.csv");
            StreamWriter sw = new StreamWriter("data.csv");
            sw.WriteLine("#siswaID, siswaNama, siswaKelas, siswaNilai, siswaNilai2, siswaNilai3, siswaKeterangan, siswaMean, siswaPredikat");
            foreach (Siswa getSiswa in listSiswa)
                sw.WriteLine(getSiswa.ID.ToString() + "," +
                             getSiswa.Nama.ToString() + "," +
                             getSiswa.Kelas1.ToString() + "," +
                             getSiswa.Nilai1.ToString() + "," +
                             getSiswa.Nilai21.ToString() + "," +
                             getSiswa.Nilai31.ToString() + "," +
                             getSiswa.Keterangan1.ToString() + "," +
                             getSiswa.Mean1.ToString() + "," +
                             getSiswa.Predikat1.ToString() + "");
            sw.Close();
        }

        private void LoadData()
        {
            if (File.Exists("data.csv"))
            {
                StreamReader sr = new StreamReader("data.csv");
                string line = sr.ReadLine();
                while (line != null)
                {
                    if (!line.Contains("#"))
                    {
                        string[] strSplit = line.Split(',');

                        // Add debug statements
                        Console.WriteLine($"Line: {line}");
                        Console.WriteLine($"strSplit length: {strSplit.Length}");

                        if (strSplit.Length == 9) // Ensure that there are 9 elements in the array
                        {
                            int id = int.Parse(strSplit[0]);
                            string nama = strSplit[1];
                            string kelas = strSplit[2];
                            int nilai = int.Parse(strSplit[3]);
                            int nilai2 = int.Parse(strSplit[4]);
                            int nilai3 = int.Parse(strSplit[5]);
                            string keterangan = strSplit[6];
                            int mean = int.Parse(strSplit[7]);
                            string predikat = strSplit[8];

                            Siswa newSiswa = new Siswa();
                            newSiswa.IsiData(id, nama, kelas, nilai, nilai2, nilai3, keterangan, mean, predikat);
                            listSiswa.Add(newSiswa);
                        }
                        else
                        {
                            // Print an error message if the array length is not as expected
                            Console.WriteLine("Error: Incorrect number of elements in the CSV line.");
                        }
                    }
                    line = sr.ReadLine();
                }
                sr.Close();
            }
        }

        private int GetFreeID()
        {
            int nowID = 0;
            while (true)
            {
                bool adaYgSama = false;
                foreach (Siswa checkSiswa in listSiswa)
                {
                    if (checkSiswa.ID == nowID)
                        adaYgSama = true;
                }
                if (adaYgSama)
                    nowID += 1;
                else
                    break;
            }
            return nowID;
        }

        private void RefreshDataGrid()
        {
            dataGridView1.Rows.Clear();
            foreach (Siswa getSiswa in listSiswa)
            {
                string[] newRow = { "", "", "", "", "", "", "", "", "" };
                newRow[0] = getSiswa.ID.ToString();
                newRow[1] = getSiswa.Nama;
                newRow[2] = getSiswa.Kelas1;
                newRow[3] = getSiswa.Nilai1.ToString();
                newRow[4] = getSiswa.Nilai21.ToString();
                newRow[5] = getSiswa.Nilai31.ToString();
                newRow[6] = getSiswa.Keterangan1.ToString();
                newRow[7] = getSiswa.Mean1.ToString();
                newRow[8] = getSiswa.Predikat1;
                dataGridView1.Rows.Add(newRow);
            }
        }

        private Siswa SelectSiswa()
        {
            int getID = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i += 1)
            {
                if (dataGridView1.Rows[i].Selected)
                {
                    getID = int.Parse(dataGridView1.Rows[i].Cells[0].Value.ToString());
                    break;
                }
            }
            Siswa getSiswa = new Siswa();
            foreach (Siswa checkSiswa in listSiswa)
            {
                if (checkSiswa.ID == getID)
                    getSiswa = checkSiswa;
            }
            return getSiswa;
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TambahNamaSiswa.Text) ||
                string.IsNullOrWhiteSpace(TambahKelasSiswa.Text))
            {
                MessageBox.Show("Nama dan Kelas siswa tidak boleh kosong.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Siswa siswaBaru = new Siswa();
            siswaID = GetFreeID();
            siswaBaru.IsiData(siswaID, TambahNamaSiswa.Text, TambahKelasSiswa.Text, (int)NumericTambahNilai.Value, (int)NumericTambahNilai2.Value, (int)NumericTambahNilai3.Value, TambahKeterangan.Text, 0, "");
            siswaBaru.CalculateMean();
            siswaBaru.CalculateGrade();

            siswaID += 1;
            listSiswa.Add(siswaBaru);
            RefreshDataGrid();
            SaveData();

            // Show success message
            MessageBox.Show("Data siswa berhasil ditambahkan.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Reset TextBoxes
            TambahNamaSiswa.Text = "";
            TambahKelasSiswa.Text = "";
            NumericTambahNilai.Value = 0;
            NumericTambahNilai2.Value = 0;
            NumericTambahNilai3.Value = 0;
            TambahKeterangan.Text = "";

            // Make GroupBox 1 visible
            groupBox1.Visible = true;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EditNamaSiswa.Text) ||
                string.IsNullOrWhiteSpace(EditKelasSiswa.Text))
            {
                MessageBox.Show("Nama dan Kelas siswa tidak boleh kosong.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Siswa getSiswa = SelectSiswa();
            getSiswa.EditData(EditNamaSiswa.Text, EditKelasSiswa.Text, (int)NumericEditNilai.Value, (int)NumericEditNilai2.Value, (int)NumericEditNilai3.Value, "", 0, "");
            getSiswa.CalculateGrade();

            panel1.Visible = false;
            RefreshDataGrid();
            groupBox2.Enabled = false;
            SaveData();
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            Siswa getSiswa = SelectSiswa();
            if (listSiswa.Contains(getSiswa))
                listSiswa.Remove(getSiswa);
            RefreshDataGrid();
            SaveData();
        }

        private void dataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            Siswa getSiswa = SelectSiswa();
            groupBox2.Enabled = true;
            EditNamaSiswa.Text = getSiswa.Nama;
            EditKelasSiswa.Text = getSiswa.Kelas1;
            NumericEditNilai.Value = getSiswa.Nilai1;
            NumericEditNilai2.Value = getSiswa.Nilai21;
            NumericEditNilai3.Value = getSiswa.Nilai31;
            panel1.Visible = true;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
            printPreviewDialog.Document = new PrintDocument();
            printPreviewDialog.Document.PrintPage += new PrintPageEventHandler(PrintDocument_PrintPage);
            printPreviewDialog.ShowDialog();
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            Bitmap bmp = new Bitmap(dataGridView1.Width, dataGridView1.Height);
            dataGridView1.DrawToBitmap(bmp, new Rectangle(0, 0, dataGridView1.Width, dataGridView1.Height));

            RectangleF printArea = e.PageSettings.PrintableArea;
            float scale = Math.Min(printArea.Width / dataGridView1.Width, printArea.Height / dataGridView1.Height);

            e.Graphics.DrawImage(bmp, printArea.Left, printArea.Top, dataGridView1.Width * scale, dataGridView1.Height * scale);
        }

        private void label5_Click(object sender, EventArgs e)
        {
            // Your code here, if needed
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
            // Your code here, if needed
        }

        private void cbNilaiTinggi_CheckedChanged(object sender, EventArgs e)
        {
            if (cbNilaiTinggi.Checked)
                RefreshDataGridNilaiTinggi();
            else
                RefreshDataGrid();
        }

        private void RefreshDataGridNilaiTinggi()
        {
            dataGridView1.Rows.Clear();

            // Sort the student list based on the Mean property in descending order
            var sortedSiswaList = listSiswa.OrderByDescending(siswa => siswa.Mean1);

            foreach (Siswa getSiswa in sortedSiswaList)
            {
                string[] newRow = { "", "", "", "", "", "", "", "", "" };
                newRow[0] = getSiswa.ID.ToString();
                newRow[1] = getSiswa.Nama;
                newRow[2] = getSiswa.Kelas1;
                newRow[3] = getSiswa.Nilai1.ToString();
                newRow[4] = getSiswa.Nilai21.ToString();
                newRow[5] = getSiswa.Nilai31.ToString();
                newRow[6] = getSiswa.Keterangan1.ToString();
                newRow[7] = getSiswa.Mean1.ToString();
                newRow[8] = getSiswa.Predikat1;
                dataGridView1.Rows.Add(newRow);
            }
        }

        private void cbNilaiRendah_CheckedChanged(object sender, EventArgs e)
        {
            if (cbNilaiRendah.Checked)
                RefreshDataGridNilaiRendah();
            else
                RefreshDataGrid();
        }

        private void RefreshDataGridNilaiRendah()
        {
            dataGridView1.Rows.Clear();

            // Sort the student list based on the Mean property in ascending order
            var sortedSiswaList = listSiswa.OrderBy(siswa => siswa.Mean1);

            foreach (Siswa getSiswa in sortedSiswaList)
            {
                string[] newRow = { "", "", "", "", "", "", "", "", "" };
                newRow[0] = getSiswa.ID.ToString();
                newRow[1] = getSiswa.Nama;
                newRow[2] = getSiswa.Kelas1;
                newRow[3] = getSiswa.Nilai1.ToString();
                newRow[4] = getSiswa.Nilai21.ToString();
                newRow[5] = getSiswa.Nilai31.ToString();
                newRow[6] = getSiswa.Keterangan1.ToString();
                newRow[7] = getSiswa.Mean1.ToString();
                newRow[8] = getSiswa.Predikat1;
                dataGridView1.Rows.Add(newRow);
            }
        }
    }
}
