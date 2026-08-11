using System;
using System.Linq;
using System.Windows.Forms;

namespace Group_Project
{
    public partial class ReservationForm : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();

        public ReservationForm()
        {
            InitializeComponent();
        }

        private void ReservationForm_Load(object sender, EventArgs e)
        {
            try
            {
                comboBox1.DataSource = db.Halls.Select(h => h.HallName).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading halls: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text) || comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Please enter Customer Name and select a Hall.");
                return;
            }

            try
            {
                var hall = db.Halls.First(h => h.HallName == comboBox1.Text);

                Customer c = new Customer
                {
                    Name = textBox1.Text.Trim(),
                    Contact = textBox2.Text.Trim()
                };
                db.Customers.InsertOnSubmit(c);
                db.SubmitChanges();

                Reservation res = new Reservation
                {
                    CustomerID = c.CustomerID,
                    HallID = hall.HallID,
                    EventDate = dateTimePicker1.Value.Date,
                    Status = "Confirmed"
                };
                db.Reservations.InsertOnSubmit(res);
                db.SubmitChanges();

                MessageBox.Show("Reservation saved successfully! Redirecting to Payment...");

                PaymentForm payForm = new PaymentForm(res.ReservationID, hall.Price);
                payForm.FormClosed += (s, args) => RefreshDashboardData();
                payForm.Show();

                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error processing reservation: " + ex.Message);
            }
        }

        private void RefreshDashboardData()
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

        private void ReservationForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            RefreshDashboardData();
        }

        private void label4_Click(object sender, EventArgs e) { }
    }
}