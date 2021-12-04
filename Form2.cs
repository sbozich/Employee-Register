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
    public partial class Register : Form
    {
        public Register()
        {
            InitializeComponent();
        }

        SqlConnection con = new SqlConnection("server=DESKTOP-N5IF2SJ\\SQLEXPRESS; database=hnb; Integrated Security=SSPI;");

        DataSet ds;
        DataView dv;
        CurrencyManager cv;
                     

        private void Binding()
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from records", con);
            ds = new DataSet();
            da.Fill(ds, "records");
            dv = new DataView(ds.Tables["records"]);
            cv = (CurrencyManager)this.BindingContext[dv];
            
            textBox1.DataBindings.Clear();
            textBox2.DataBindings.Clear();

            textBox1.DataBindings.Add("Text", dv, "id");
            textBox2.DataBindings.Add("Text", dv, "empname");

        }
        
        private void showposition()
        {
            label4.Text = cv.Position + 1 + "of" + cv.Count;
            byte[] arrimg = (byte[])ds.Tables[0].Rows[cv.Position]["photo"];
            MemoryStream ms = new MemoryStream(arrimg);
            pictureBox1.Image = Image.FromStream(ms);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.BorderStyle = BorderStyle.Fixed3D;

        }

        
        private void Form2_Load(object sender, EventArgs e)
        {
            Binding();
            showposition();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cv.Position = 0;
            showposition();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            cv.Position += 1;
            showposition();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            cv.Position -= 1;
            showposition();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            cv.Position = cv.Count - 1;
            showposition();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            int size = -1;

            DialogResult result = openFileDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                try
                {
                    this.pictureBox1.Image = Image.FromFile(openFileDialog1.FileName);
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    pictureBox1.BorderStyle = BorderStyle.Fixed3D;
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }



        private void update()
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                pictureBox1.Image.Save(ms, pictureBox1.Image.RawFormat);
                byte[] arrImage = ms.GetBuffer();
                ms.Close();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "update records set empname=@empname, photo=@photo where id=@id";
                cmd.Parameters.AddWithValue("@empname", textBox2.Text);
                cmd.Parameters.AddWithValue("@photo", arrImage);
                cmd.Parameters.AddWithValue("@id", textBox1.Text);
                con.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Record updated");
                con.Close();
                Binding();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            


        }

        private void delete()
        {
            DialogResult dr = MessageBox.Show("Are you sure want to delete record?", "Payroll", MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            
            if(dr==DialogResult.No)
            {

            }
            else
            {
                int p;
                p = this.BindingContext[dv].Position - 1;
                if(p<0)
                {
                    p = 0;
                }
                else
                {
                    try
                    {
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = con;
                        cmd.CommandText = "delete from records where id=@id";
                        cmd.Parameters.AddWithValue("@id", textBox1.Text);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Record deleted");
                        con.Close();
                        Binding();
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            update();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            delete();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button9_Click(object sender, EventArgs e)
        {
          
        }
    }
}
