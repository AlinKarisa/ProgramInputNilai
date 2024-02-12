using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Windows.Forms;

namespace ProgramInputNilaiSiswa
{
    public class Siswa
    {
        private string nama;
        private string Predikat;
        private int Nilai;
        private string Kelas;
        private int id;
        private int Nilai2;
        private int Nilai3;
        private string Keterangan;
        private int Mean;

        public string Nama { get => nama; }
        public string Predikat1 { get => Predikat; }
        public int Nilai1 { get => Nilai; }
        public string Kelas1 { get => Kelas; }
        public int ID { get => id; }
        public int Nilai21 { get => Nilai2;}
        public int Nilai31 { get => Nilai3;}
        public string Keterangan1 { get => Keterangan; }
        public int Mean1 { get => Mean; }


        public void EditData(string getNama, string getKelas, int getNilai, int getNilai2, int getNilai3, string getKeterangan, int getMean, string getPredikat)
        {
            this.nama = getNama;
            this.Kelas = getKelas;
            this.Nilai = getNilai;
            this.Nilai2 = getNilai2;
            this.Nilai3 = getNilai3;
            this.Keterangan = getKeterangan;
            this.Mean = getMean;
            this.Predikat = getPredikat;
        }

        public void IsiData(int getId, string getNama, string getKelas, int getNilai, int getNilai2, int getNilai3, string getKeterangan, int getMean, string getPredikat)
        {
            this.id = getId;
            this.nama = getNama;
            this.Kelas = getKelas;
            this.Nilai = getNilai;
            this.Nilai2 = getNilai2;
            this.Nilai3 = getNilai3;
            this.Keterangan = getKeterangan;
            this.Mean = getMean;
            this.Predikat = getPredikat;
        }

        public double CalculateMean()
        {
            Mean = (int)((Nilai + Nilai2 + Nilai3) / 3.0);
            return Mean;
        }

        public string CalculateGrade()
        {
            if (Mean >= 90)
            {
                Predikat = "A";
            }
            else if (Mean >= 80)
            {
                Predikat = "B";
            }
            else if (Mean >= 70)
            {
                Predikat = "C";
            }
            else if (Mean >= 60)
            {
                Predikat = "D";
            }
            else if (Mean <= 60)
            {
                Predikat = "E";
            }
            else if (Mean == 0)
            {
                Predikat = "TIDAK NAIK KELAS";
            }

            return Predikat;
        }

    }
}