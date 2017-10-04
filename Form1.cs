using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace Thuchanhbuoi3_1
{
    public partial class Form1 : Form
    {
        private SqlConnection cn = null;
        private SqlCommand cmd = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string cnStr = "Server = . ; Database = QLBanHang ; Integrated security = true ;";
            cn = new SqlConnection(cnStr);
            dvgKetQua.DataSource = GetData();
        }

        private List<object> GetData()
        {
            Connect();
            string sql = "SELECT * FROM LoaiSP";
            List<object> list = new List<object>();
            try 
            {
                cmd = new SqlCommand(sql, cn);
                SqlDataReader dr = cmd.ExecuteReader();
                int id;
                string name;

                while (dr.Read())
                {
                    id = dr.GetInt32(0);
                    name = dr.GetString(1);
                    var prod = new
                    {
                        MaLoaiSP = id,
                        TenLoaiSP = name,
                    };
                    list.Add(prod);
                }
                dr.Close();
            }
            catch(SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Disconnect();
            }
            return list;
        }

        private void Connect()
        {
            try
            {
                if (cn != null && cn.State != ConnectionState.Open)
                {
                    cn.Open();
                    MessageBox.Show("Ket noi thanh cong");
                }
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
                ;
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (ConfigurationErrorsException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Disconnect()
        {
            if (cn != null && cn.State != ConnectionState.Closed)
            {
                cn.Close();
            }
        }

        private void btXoa_Click(object sender, EventArgs e)
        {
            string sql = "DELETE FROM LoaiSP WHERE MaLoaiSP = " + txtMaLoai.Text;
            cmd = new SqlCommand(sql, cn);
            int numberOfRows = 0 ;
            Connect();
            try
            {
                 numberOfRows = cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Disconnect();
            }
            MessageBox.Show("So dong da xoa : " + numberOfRows.ToString());
            dvgKetQua.DataSource = GetData();
        }
      
    }
}
