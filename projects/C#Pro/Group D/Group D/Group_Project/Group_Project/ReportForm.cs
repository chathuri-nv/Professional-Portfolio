using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Group_Project
{
    public partial class ReportForm : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();

        public ReportForm()
        {
            InitializeComponent();
        }

        private void ReportForm_Load(object sender, EventArgs e)
        {
            LoadFullReport();
        }

        private void LoadFullReport()
        {
            try
            {
                var report = from r in db.Reservations
                             join h in db.Halls on r.HallID equals h.HallID
                             join c in db.Customers on r.CustomerID equals c.CustomerID
                             orderby r.EventDate descending
                             select new
                             {
                                 ReservationID = r.ReservationID,
                                 Date = r.EventDate,
                                 Hall = h.HallName,
                                 Customer = c.Name,
                                 Contact = c.Contact,
                                 Status = r.Status
                             };

                dataGridView1.DataSource = report.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading report: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime selectedDate = dateTimePicker1.Value.Date;

                var filteredReport = from r in db.Reservations
                                     join h in db.Halls on r.HallID equals h.HallID
                                     join c in db.Customers on r.CustomerID equals c.CustomerID
                                     where r.EventDate == selectedDate
                                     select new
                                     {
                                         ReservationID = r.ReservationID,
                                         Date = r.EventDate,
                                         Hall = h.HallName,
                                         Customer = c.Name,
                                         Contact = c.Contact,
                                         Status = r.Status
                                     };

                dataGridView1.DataSource = filteredReport.ToList();

                if (!filteredReport.Any())
                {
                    MessageBox.Show("No reservations found for the selected date.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Search Error: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadFullReport();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}