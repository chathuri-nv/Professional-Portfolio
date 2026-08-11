using System;
using System.Linq;
using System.Windows.Forms;

namespace Group_Project
{
    public partial class PaymentForm : Form
    {
        
        int resID;
        decimal totalPrice;

       
        DataClasses1DataContext db = new DataClasses1DataContext();

        public PaymentForm(int id, decimal price)
        {
            InitializeComponent();
            this.resID = id;
            this.totalPrice = price;
        }

        private void PaymentForm_Load(object sender, EventArgs e)
        {
            
            textBox1.Text = totalPrice.ToString("F2");

            
            textBox1.ReadOnly = true;
            textBox3.ReadOnly = true; 
        }

    
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (decimal.TryParse(textBox2.Text, out decimal advance))
            {
                decimal balance = totalPrice - advance;
                textBox3.Text = balance.ToString("F2");
            }
            else
            {
                
                textBox3.Text = totalPrice.ToString("F2");
            }
        }

        
        private void button1_Click(object sender, EventArgs e)
        {
            
            if (string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Please enter the Advance Amount.");
                return;
            }

            try
            {
                decimal advance = decimal.Parse(textBox2.Text);
                decimal balance = totalPrice - advance;

               
                Payment p = new Payment
                {
                    ReservationID = resID,
                    TotalAmount = totalPrice,
                    AdvanceAmount = advance,
                    Balance = balance
                };

              
                db.Payments.InsertOnSubmit(p);
                db.SubmitChanges();

                MessageBox.Show("Payment Successful! Balance: Rs." + balance.ToString("N2"), "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Close(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error processing payment: " + ex.Message);
            }
        }

        
        private void PaymentForm_FormClosed(object sender, FormClosedEventArgs e)
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

        
    }
}