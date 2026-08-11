using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Group_Project
{
    public partial class DashboardForm : Form
    {
     
        DataClasses1DataContext db = new DataClasses1DataContext();

        public DashboardForm()
        {
            InitializeComponent();
        }

        private void DashboardForm_Load(object sender, EventArgs e)
        {
            RefreshDashboard();
        }
        public void RefreshDashboard()
        {
            try
            {
                
                label4.Text = db.Reservations.Count().ToString();
                label6.Text = db.Halls.Count().ToString();
                label2.Text = db.Reservations.Count().ToString();

               
                decimal income = db.Payments.Sum(p => (decimal?)p.AdvanceAmount) ?? 0;
                label8.Text = "Rs." + income.ToString("N0");

              
                LoadUpcomingEvents();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error refreshing data: " + ex.Message);
            }
        }

      
        private void LoadUpcomingEvents()
        {
           
            dataGridView1.DataSource = db.Reservations.Select(r => new {
                EventDate = r.EventDate,         
                Hall_Name = r.Hall.HallName,     
                Customer_Name = r.Customer.Name  
            }).ToList();
        }

        private void button2_Click(object sender, EventArgs e) { new ReservationForm().Show(); }
        private void button3_Click(object sender, EventArgs e) { new HallForm().Show();}
        private void button5_Click(object sender, EventArgs e) {new ReportForm().Show();}

      
        private void button6_Click(object sender, EventArgs e)
        {
            LoadUpcomingEvents(); 
        }

      
        private void label2_Click(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
    }
}