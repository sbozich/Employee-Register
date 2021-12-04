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
using System.IO;

namespace Simple_Record_Navigation
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection con = new SqlConnection("server=DESKTOP-N5IF2SJ\\SQLEXPRESS; database=hnb; Integrated Security=SSPI;");
        


        private void save()
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                pictureBox1.Image.Save(ms, pictureBox1.Image.RawFormat);
                byte[] arrImage = ms.GetBuffer();
                ms.Close();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "insert into records(empname,photo) values(@empname, @photo)";
                cmd.Parameters.AddWithValue("@empname", textBox1.Text);
                cmd.Parameters.AddWithValue("@photo", arrImage);
                con.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Record added");
                con.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show("Please select the picture");
                //MessageBox.Show(ex.Message);

            }

        }


        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int size = -1;

            DialogResult result = openFileDialog1.ShowDialog();

            if(result==DialogResult.OK)
            {
                try
                {
                    this.pictureBox1.Image = Image.FromFile(openFileDialog1.FileName);
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    pictureBox1.BorderStyle = BorderStyle.Fixed3D;
                }

                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            save();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Register f2= new Register();
            f2.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }

}
