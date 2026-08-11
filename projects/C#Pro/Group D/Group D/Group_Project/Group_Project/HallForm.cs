using System;
using System.Linq;
using System.Windows.Forms;

namespace Group_Project
{
    public partial class HallForm : Form
    {
        
        DataClasses1DataContext db = new DataClasses1DataContext();

        public HallForm()
        {
            InitializeComponent();
        }

        private void HallForm_Load(object sender, EventArgs e)
        {
            LoadData();
        }


        private void LoadData()
        {
            try
            {
                
                dataGridView1.DataSource = db.Halls.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }
        }

     
        private void ClearFields()
        {
            textBox1.Clear(); 
            textBox2.Clear(); 
            textBox3.Clear(); 
            textBox4.Clear(); 
        }

     
        private void HallForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            foreach (Form f in Application.OpenForms)
            {
                if (f is DashboardForm dash)
                {
                    dash.RefreshDashboard();
                    break;
                }
            }
        }

  
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(textBox3.Text))
            {
                MessageBox.Show("Please fill in all the information.");
                return;
            }

            try
            {
                Hall h = new Hall
                {
                    HallName = textBox1.Text.Trim(),
                    Capacity = int.Parse(textBox2.Text),
                    Price = decimal.Parse(textBox3.Text)
                };

                db.Halls.InsertOnSubmit(h);
                db.SubmitChanges();

                MessageBox.Show("Hall added successfully!");
                ClearFields();
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Insertion Error: " + ex.Message);
            }
        }

       
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textBox4.Text)) { MessageBox.Show("Please enter a Hall ID to update."); return; }

                int id = int.Parse(textBox4.Text);
                var upHall = db.Halls.FirstOrDefault(h => h.HallID == id);

                if (upHall != null)
                {
                    upHall.HallName = textBox1.Text.Trim();
                    upHall.Capacity = int.Parse(textBox2.Text);
                    upHall.Price = decimal.Parse(textBox3.Text);

                    db.SubmitChanges();
                    MessageBox.Show("Information updated successfully!");
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Hall record not found.");
                }
            }
            catch (Exception ex) { MessageBox.Show("Update Error: " + ex.Message); }
        }

        
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textBox4.Text)) { MessageBox.Show("Please enter a Hall ID to delete."); return; }

                int id = int.Parse(textBox4.Text);
                var delHall = db.Halls.FirstOrDefault(h => h.HallID == id);

                if (delHall != null)
                {
                    db.Halls.DeleteOnSubmit(delHall);
                    db.SubmitChanges();
                    MessageBox.Show("Data deleted successfully!");
                    ClearFields();
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Hall record not found.");
                }
            }
            catch (Exception ex) { MessageBox.Show("Delete Error: " + ex.Message); }
        }

      
        private void button4_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

      
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textBox4.Text)) { MessageBox.Show("Please enter an " +
                    "ID to search."); return; }

                int id = int.Parse(textBox4.Text);
                var hall = db.Halls.FirstOrDefault(h => h.HallID == id);

                if (hall != null)
                {
                    textBox1.Text = hall.HallName;
                    textBox2.Text = hall.Capacity.ToString();
                    textBox3.Text = hall.Price.ToString();
                }
                else
                {
                    MessageBox.Show("No data found for the given ID.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Search Error: " + ex.Message);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e) { }
    }
}